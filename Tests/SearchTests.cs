using NUnit.Framework;
using QA_AutomationFramework.Pages;

namespace QA_AutomationFramework.Tests
{
    [Category("SearchTests")]
    public class SearchTests : BaseTest
    {
        private LoginPage _loginPage;
        private HomePage _homePage;

        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();
            _loginPage = new LoginPage(driver, emailService, testData);
            _homePage = new HomePage(driver);
        }


        [Test]
        public void SearchItem_ShouldReturnSingleResultWithMatchingName()
        {
            var searchTerm = "Book";
            Step("Login into the app", () => _loginPage.LoginIntoApp());
            Step("Click on the Home menu item", () => _homePage.ClickOnHomeMenuItem());
            Step("Enter search term", () => _homePage.EnterSearchTerm(searchTerm));
            Step("Verify table contains only one element", () => Assert.That(_homePage.GetTableItems().Count(), Is.EqualTo(1)));
            Step("Verify row item name contains the search term", () => Assert.That(_homePage.GetItemName(0), Does.Contain(searchTerm)));
        }
    }
}
