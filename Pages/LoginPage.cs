using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace QA_AutomationFramework.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver) { }

        private IWebElement ContinueWithEmailButton => WaitForElementToBeClickable(By.Id("email-btn"));
        private IWebElement ContinueButton => WaitForElementToBeClickable(By.Id("1-submit"));
        private IWebElement EmailInputField => WaitForElementToBePresent(By.CssSelector("input[type='email']"));
        private IWebElement OtpInputField => WaitForElementToBeClickable(By.Id("1-vcode"));
        private IWebElement UserInfoEmailField => WaitForElementToBePresent(By.Id("user-info-email"));
        private IWebElement ErrorMessageDiv => WaitForElementToBePresent(By.CssSelector(".auth0-global-message-error"));
        private IWebElement ConfirmEmailInputField => WaitForElementToBePresent(By.Id("1-email"));
        private IWebElement InvalidEmailDiv => WaitForElementToBePresent(By.CssSelector(".auth0-lock-error-invalid-hint"));

        public void SubmitEmail() => ContinueWithEmailButton.Click();
        public void EnterEmail(string email) => EmailInputField.SendKeys(email);
        public void EnterOtp(string otp) => OtpInputField.SendKeys(otp);
        public void ClickContinue() => ContinueButton.Click();
        public string GetUserInfoEmail() => UserInfoEmailField.Text;
        public string GetInvalidEmailErrorMessageText() => InvalidEmailDiv.Text;
        public bool ContinueWithEmailButtonHasClass(string className) => ElementHasClass(ContinueWithEmailButton, className);

        public string GetErrorMessageText()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            IWebElement errorMessageSpan =  wait.Until(ElementIsVisible(ErrorMessageDiv, By.CssSelector("span")));
            return errorMessageSpan.Text;
        }

        public void EditConfirmEmail(string newEmail)
        {
            ConfirmEmailInputField.SendKeys(Keys.Control + "a");
            ConfirmEmailInputField.SendKeys(Keys.Delete);
            ConfirmEmailInputField.SendKeys(newEmail);
        }

        public bool ElementHasClass(IWebElement element, string className)
        {
            var classes = element.GetAttribute("class");
            return classes != null && classes.Split(' ').Contains(className);
        }
    }
}
