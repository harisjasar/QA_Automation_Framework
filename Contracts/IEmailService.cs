namespace QA_AutomationFramework.Contracts
{
    public interface IEmailService
    {
        void Connect();
        void Authenticate(string email, string appPassword);
        void Disconnect();
        IEnumerable<string> GetRecentEmails(int offsetSeconds, string subject);
        string ExtractOtpFromEmail(string emailContent);
        string GetMostRecentOtp(int maxRetries = 20, int retryDelaySeconds = 1, int offsetSeconds = 5);
    }
}
