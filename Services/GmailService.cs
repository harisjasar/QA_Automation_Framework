using MailKit.Search;
using MailKit;
using QA_AutomationFramework.Contracts;
using System.Text.RegularExpressions;
using MailKit.Net.Imap;
using QA_AutomationFramework.Configs;


namespace QA_AutomationFramework.Services
{
    public class GmailService : IEmailService
    {
        private ImapClient _client;
        private string _email;
        private string _appPassword;
        private readonly TestSettings testSettings;

        public GmailService(TestSettings testSettings)
        {
            _client = new ImapClient();
            this.testSettings = testSettings;
        }

        public void Connect()
        {
            _client.Connect(testSettings.EmailSettings.Host, testSettings.EmailSettings.Port, testSettings.EmailSettings.UseSsl);
        }

        public void Authenticate(string email, string appPassword)
        {
            _email = email;
            _appPassword = appPassword;
            _client.Authenticate(email, appPassword);
        }

        public void Disconnect()
        {
            _client.Disconnect(true);
        }

        public IEnumerable<string> GetRecentEmails(int offsetSeconds, string subject)
        {
            _client.Inbox.Open(FolderAccess.ReadOnly);
            DateTime now = DateTime.UtcNow;
            DateTime startDateTime = now.AddSeconds(-offsetSeconds);
            var query = SearchQuery.DeliveredAfter(startDateTime.Date);
            var uids = _client.Inbox.Search(query);
            var messages = _client.Inbox.Fetch(uids, MessageSummaryItems.Envelope | MessageSummaryItems.InternalDate | MessageSummaryItems.UniqueId);
            return messages
                .Where(message => message.InternalDate >= startDateTime && message.InternalDate <= now)
                .Where(message => message.Envelope.Subject != null && message.Envelope.Subject.Contains(subject))
                .Select(message => _client.Inbox.GetMessage(message.UniqueId).HtmlBody)
                .Where(htmlBody => !string.IsNullOrEmpty(htmlBody))
                .ToList();
        }

        public string ExtractOtpFromEmail(string emailContent)
        {
            string pattern = @"<strong[^>]*>(\d{6})<\/strong>";
            Match match = Regex.Match(emailContent, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        public string GetMostRecentOtp(int maxRetries, int retryDelaySeconds, int offsetSeconds)
        {
            int retries = 0;
            while (retries < maxRetries)
            {
                try
                {
                    var email = testSettings.EmailSettings.Email;
                    var appPassword = testSettings.EmailSettings.AppPassword;

                    Connect();
                    Authenticate(email, appPassword);

                    var recentEmails = GetRecentEmails(offsetSeconds, "Your Anchor account One Time Passcode");
                    if (recentEmails.Count() != 1)
                    {
                        throw new Exception($"Expected one email, but found: {recentEmails.Count()}");
                    }

                    string otp = ExtractOtpFromEmail(recentEmails.First());
                    if (string.IsNullOrEmpty(otp))
                    {
                        throw new Exception($"Unable to extract OTP from the email. Email content: {recentEmails.First()}");
                    }

                    return otp;
                }
                catch (Exception ex)
                {
                    retries++;
                    if (retries >= maxRetries)
                    {
                        throw new Exception($"Failed to get OTP after {maxRetries} retries", ex);
                    }

                    Thread.Sleep(retryDelaySeconds * 1000);
                }
                finally
                {
                    Disconnect();
                }
            }

            throw new Exception("Retries exhausted. Unable to retrieve OTP.");
        }


    }
}
