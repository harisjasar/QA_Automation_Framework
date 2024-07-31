using AventStack.ExtentReports.Model;
using NUnit.Framework;
using QA_AutomationFramework.Pages;

namespace QA_AutomationFramework.Tests
{
    [Category("LoginTests")]
    public class LoginTests : BaseTest
    {
        private LoginPage _loginPage;

        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();
            _loginPage = new LoginPage(driver, emailService, testData);
        }

        private void NavigateAndEnterEmail(string email)
        {
            Step("Navigate to login page", () => _loginPage.NavigateToPageByUrl(testData.BaseUrl));
            Step("Enter email", () => _loginPage.EnterEmail(testData.Email));
            Step("Submit email", () => _loginPage.SubmitEmail());
        }

        [Test]
        public void ValidOTP_ShouldLogin()
        {
            NavigateAndEnterEmail(testData.Email);
            Step("Click Continue on Confirm Email button", () => _loginPage.ClickContinue());
            Step("Extract and Enter OTP", () => _loginPage.EnterOtp(emailService.GetMostRecentOtp()));
            Step("Click Continune button to confirm OTP", () => _loginPage.ClickContinue());
            Step("Verify user info email", () => Assert.That(_loginPage.GetUserInfoEmail(), Is.EqualTo(testData.Email)));
        }

        [Test]
        public void InvalidOTP_ShouldShowErrorMessage()
        {
            Random rnd = new Random();
            int randomOtp = rnd.Next(100000, 999999);
            NavigateAndEnterEmail(testData.Email);
            Step("Click Continue on Confirm Email button", () => _loginPage.ClickContinue());
            Step("Extract and Enter OTP", () => _loginPage.EnterOtp(emailService.GetMostRecentOtp() + "5"));
            Step("Click Continune button to confirm OTP", () => _loginPage.ClickContinue());
            Step("Verify error message text", () => Assert.That(_loginPage.GetErrorMessageText(), Is.EqualTo("WE'RE SORRY, SOMETHING WENT WRONG")));
            
        }

        [Test]
        public void InvalidConfirmEmail_ShouldShowErrorMessage()
        {
            NavigateAndEnterEmail(testData.Email);
            Step("Edit email on Confirm Email input", () => _loginPage.EditConfirmEmail(testData.InvalidEmail));
            Step("Click Continue on Confirm Email button", () => _loginPage.ClickContinue());
            Step("Verify error message text", () =>  Assert.That(_loginPage.GetInvalidEmailErrorMessageText(), Is.EqualTo("Email is invalid")));
        }

        [Test]
        [Category("smoke")]
        public void InvalidEmail_ShouldDisableContinueButton()
        {
            Step("Navigate to the main page", () => _loginPage.NavigateToPageByUrl(testData.BaseUrl));
            Step("Enter invalid email", () => _loginPage.EnterEmail(testData.InvalidEmail));
            Step("Verify continue button disabled on invalid email", () => Assert.That(_loginPage.ContinueWithEmailButtonHasClass("cursor-not-allowed"), Is.True));
        }
    }
}
