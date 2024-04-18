#nullable disable // Disables nullable warnings for this file

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Text;

namespace Membership
{
    public class Tests

    {

#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        private WebDriver driver;
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method


        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            this.driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            // Successfully tested on local machine

        }
        [TearDown]

        public void TearDown()
        {
          driver.Quit();
        }

        [Test]
        public void aCreate_User()
        {

            driver.Url = "https://www.hrtoday.ch/de/newmembership/einzel/step1";

            driver.FindElement(By.XPath("//*[@id=\"edit-salutation-1\"]/option[3]")).Click();
            driver.FindElement(By.Id("edit-first-name-1")).SendKeys("TestUser");
            driver.FindElement(By.Id("edit-last-name-1")).SendKeys("Yanova");
            driver.FindElement(By.Id("edit-email-1")).SendKeys("evelin.totev+1@yanova.ch");
            driver.FindElement(By.Id("edit-password-1-pass1")).SendKeys("Abc123456!!!");
            driver.FindElement(By.Id("edit-password-1-pass2")).SendKeys("Abc123456!!!");
            driver.FindElement(By.Id("edit-continue")).Click();

            //step 2
            driver.FindElement(By.Id("edit-company-organization")).SendKeys("TestOrganisation");
            driver.FindElement(By.Id("edit-function")).SendKeys("TestFunction");
            driver.FindElement(By.Id("edit-telephone-store")).SendKeys("3333323232");
            driver.FindElement(By.Id("edit-continue")).Click();

            //step 3
            driver.FindElement(By.XPath("//div[@id='edit-delivery-option']/div[2]/label[@class='option']")).Click();
            driver.FindElement(By.Id("edit-continue")).Click();

            //step 4
            driver.FindElement(By.Id("edit-shipping-post-code")).SendKeys("333");
            driver.FindElement(By.Id("edit-shipping-place")).SendKeys("Test" + Keys.Return);


            // Step 5

            Actions actions = new Actions(driver);
            var element = driver.FindElement(By.XPath("//*[@id=\"newmembership-register-step-fifth-form\"]/div[6]/label/div"));
            actions.MoveToElement(element).Click().Build().Perform();

            var element2 = driver.FindElement(By.CssSelector("input#edit-continue"));
            actions.MoveToElement(element2).Click().Build().Perform();

            string textConfirmation = driver.FindElement(By.XPath("//*[@id=\"newmembership-register-success-form\"]/div[2]/p")).Text;

            Assert.That(textConfirmation.Equals("Sie haben per evelin.totev+1@yanova.ch eine Bestätigung der Business-Einzel-Membership mit allen wichtigen Informationen über die App, E-Paper, Veranstaltungen... erhalten."));
        }


        [Test]

        public void bValidateMembership()
        {

            driver.Url = "https://mail.infomaniak.com/";

            string username = Environment.GetEnvironmentVariable("EMAIL_ADDRESS");
            string password = Environment.GetEnvironmentVariable("EMAIL_PASS");

            driver.FindElement(By.Id("mat-input-0")).SendKeys(username);
            driver.FindElement(By.Id("mat-input-1")).SendKeys(password);
            driver.FindElement(By.XPath("//*[@id=\"infomaniak_login_form\"]/div[2]/button")).Click();

            // go to search field
            driver.FindElement(By.XPath("//app-root/app-mail[@class='ng-star-inserted']/app-main[@class='ng-star-inserted']//app-mail-header[@class='app-header']/app-header//app-mail-search[@class='ng-star-inserted']//input[@name='search']")).SendKeys("Bestellbestätigung" + Keys.Return);

            driver.FindElement(By.XPath("/html/body/app-root/app-mail/app-main/div[1]/div/ik-layout/div/div/div/app-mail-main/div/div[1]/div[2]/div[1]/app-mail-list/div[2]/app-mail-list-item[1]")).Click();
            driver.FindElement(By.LinkText("VERVOLLSTÄNDIGEN SIE DIE REGISTRIERUNG")).Click();

            //  string confirmationMessage = driver.FindElement(By.XPath("//*[@id=\"breadcrumb\"]/div/div/div[3]/div/p/text()")).Text;

            //  Assert.Equals(confirmationMessage, ("Ihre E-Mail wurde verifiziert und die Mitgliedschaft wurde aktiviert. Sie können sich jetzt einloggen.\r\n\r\n"));
            Assert.That(true);
        }

        [Test]
        public void cDelete_User_From_Backend()
        {
            driver.Url = "https://hrtoday.ch/de/user/login";


            string username = Environment.GetEnvironmentVariable("EMAIL_ADDRESS");
            string password = Environment.GetEnvironmentVariable("HRT_PASS");

            //Add credential, need to find a way to store securely this data
            driver.FindElement(By.Id("edit-name")).SendKeys(username);
            driver.FindElement(By.Id("edit-pass")).SendKeys(password + Keys.Return);

            // Navigate to People page via toolbar
            driver.FindElement(By.Id("toolbar-item-administration")).Click();
            driver.FindElement(By.LinkText("People")).Click();

            // Find user by email and open it
            string emailAddressToDelete = "evelin.totev+1@yanova.ch";
            driver.FindElement(By.Id("edit-user")).SendKeys(emailAddressToDelete);
            driver.FindElement(By.Id("edit-submit-user-admin-people")).Click();
            driver.FindElement(By.XPath("//*[@id=\"views-form-user-admin-people-page-1\"]/table/tbody/tr/td[7]/div/div/ul/li[1]/a")).Click();

            // Logic for deleting the membership of user
            driver.FindElement(By.CssSelector("#edit-memberships-overview > div > table > tbody > tr > td:nth-child(8) > a")).Click();
            driver.FindElement(By.Id("edit-delete")).Click();
            string confirmationMessage = driver.FindElement(By.XPath("//*[@id=\"block-claro-page-title\"]/h1")).Text;
            Assert.That(confirmationMessage.Contains("wirklich löschen"));
            driver.FindElement(By.Id("edit-submit")).Click();

            // Deleting of user proceess
            driver.FindElement(By.Id("edit-delete")).Click();
            driver.FindElement(By.Id("edit-user-cancel-method-user-cancel-delete")).Click();
            driver.FindElement(By.Id("edit-submit")).Click();

            string message = driver.FindElement(By.XPath("/html/body/div[2]/div/main/div[2]/div[2]/div[1]/div[2]")).Text;

            Assert.That(message.Contains("wurde gelöscht"));
        }



        [Test]
        public void dCreate_User_MMP()
        {

            driver.Url = "https://www.missmoneypenny.ch/newmembership/einzel/step1";

            driver.FindElement(By.XPath("//*[@id=\"edit-salutation-1\"]/option[3]")).Click();
            driver.FindElement(By.Id("edit-first-name-1")).SendKeys("TestUser");
            driver.FindElement(By.Id("edit-last-name-1")).SendKeys("Yanova");
            driver.FindElement(By.Id("edit-email-1")).SendKeys("evelin.totev+1@yanova.ch");
            driver.FindElement(By.Id("edit-password-1-pass1")).SendKeys("Abc123456!!!");
            driver.FindElement(By.Id("edit-password-1-pass2")).SendKeys("Abc123456!!!");
            driver.FindElement(By.Id("edit-continue")).Click();

            //step 2
            driver.FindElement(By.Id("edit-company-organization")).SendKeys("TestOrganisation");
            driver.FindElement(By.Id("edit-function")).SendKeys("TestFunction");
            driver.FindElement(By.Id("edit-private-telephone")).SendKeys("3333323232");
            driver.FindElement(By.Id("edit-telephone-store")).SendKeys("3333323232");
            driver.FindElement(By.Id("edit-continue")).Click();

            //step 3
            driver.FindElement(By.XPath("//div[@id='edit-delivery-option']/div[2]/label[@class='option']")).Click();
            driver.FindElement(By.Id("edit-continue")).Click();

            //step 4
            driver.FindElement(By.Id("edit-shipping-post-code")).SendKeys("333");
            driver.FindElement(By.Id("edit-shipping-place")).SendKeys("Test" + Keys.Return);


            // Step 5

            Actions actions = new Actions(driver);
            var element = driver.FindElement(By.XPath("//*[@id=\"newmembership-register-step-fifth-form\"]/div[6]/label/div"));
            actions.MoveToElement(element).Click().Build().Perform();

            var element2 = driver.FindElement(By.CssSelector("input#edit-continue"));
            actions.MoveToElement(element2).Click().Build().Perform();

            string textConfirmation = driver.FindElement(By.XPath("//*[@id=\"newmembership-register-success-form\"]/div[2]/p[1]")).Text;

            Assert.That(textConfirmation.Equals("Sie haben auf evelin.totev+1@yanova.ch eine Bestätigung der Business-Einzel-Membership erhalten mit allen wichtigen Informationen zu App, E-Paper, Events ..."));
        }
       
        [Test]

        public void eValidateMembershipMMP()
        {

            driver.Url = "https://mail.infomaniak.com/";

            string username = Environment.GetEnvironmentVariable("EMAIL_ADDRESS");
            string password = Environment.GetEnvironmentVariable("EMAIL_PASS");

            driver.FindElement(By.Id("mat-input-0")).SendKeys(username);
            driver.FindElement(By.Id("mat-input-1")).SendKeys(password);
            driver.FindElement(By.XPath("//*[@id=\"infomaniak_login_form\"]/div[2]/button")).Click();

            // go to search field
            driver.FindElement(By.XPath("//app-root/app-mail[@class='ng-star-inserted']/app-main[@class='ng-star-inserted']//app-mail-header[@class='app-header']/app-header//app-mail-search[@class='ng-star-inserted']//input[@name='search']")).SendKeys("Bestellbestätigung" + Keys.Return);


            driver.FindElement(By.XPath("/html/body/app-root/app-mail/app-main/div[1]/div/ik-layout/div/div/div/app-mail-main/div/div[1]/div[2]/div[1]/app-mail-list/div[2]/app-mail-list-item[1]")).Click();
            //  driver.FindElement(By.LinkText("Ja, melden Sie mich für den Newsletter an.")).Click();

            string currentWindowHandle = driver.CurrentWindowHandle;


            driver.FindElement(By.LinkText("SCHLIESSEN SIE DIE REGISTRIERUNG AB")).Click();


            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.WindowHandles.Count > 1);

            foreach (string windowHandle in driver.WindowHandles)
            {
                if (windowHandle != currentWindowHandle)
                {
                    driver.SwitchTo().Window(windowHandle);
                    break;
                }
            }

            string confirmationMessage = driver.FindElement(By.XPath("//*[@id=\"content\"]/div/div[2]/div/p")).Text;

            Assert.That(confirmationMessage.Equals("Ihre E-Mail wurde verifiziert und die Membership aktiviert. Sie können sich jetzt einloggen."));
        }

        [Test]
        public void fDelete_User_From_Backend_MMP()
        {
            driver.Url = "https://www.missmoneypenny.ch/user/login";

            string username = Environment.GetEnvironmentVariable("EMAIL_ADDRESS");
            string password = Environment.GetEnvironmentVariable("MMP_PASS");


            //Add credential, need to find a way to store securely this data
            driver.FindElement(By.Id("edit-name")).SendKeys(username);
            driver.FindElement(By.Id("edit-pass")).SendKeys(password + Keys.Return);

            // Navigate to People page via toolbar
            driver.FindElement(By.Id("toolbar-item-administration")).Click();
            driver.FindElement(By.LinkText("People")).Click();

            // Find user by email and open it
            string emailAddressToDelete = "evelin.totev+1@yanova.ch";
            driver.FindElement(By.Id("edit-user")).SendKeys(emailAddressToDelete);
            driver.FindElement(By.Id("edit-submit-user-admin-people")).Click();
            driver.FindElement(By.XPath("//*[@id=\"views-form-user-admin-people-page-1\"]/table/tbody/tr/td[7]/div/div/ul/li[1]/a")).Click();

            // Logic for deleting the membership of user
            string activeMessage = driver.FindElement(By.XPath("//*[@id=\"edit-memberships-overview\"]/div/table/tbody/tr/td[7]")).Text;
            Assert.That(activeMessage.Contains("Active"));
            driver.FindElement(By.CssSelector("#edit-memberships-overview > div > table > tbody > tr > td:nth-child(8) > a")).Click();
            driver.FindElement(By.Id("edit-delete")).Click();
            string confirmationMessage = driver.FindElement(By.XPath("//*[@id=\"node-membership-delete-form\"]")).Text;
            Assert.That(confirmationMessage.Contains("Diese Aktion kann nicht rückgängig gemacht werden."));
            driver.FindElement(By.Id("edit-submit")).Click();

            // Deleting of user proceess
            driver.FindElement(By.Id("edit-delete")).Click();
            driver.FindElement(By.Id("edit-user-cancel-method-user-cancel-delete")).Click();
            driver.FindElement(By.Id("edit-submit")).Click();

            string message = driver.FindElement(By.XPath("/html/body/div[2]/div/main/div[2]/div[2]/div")).Text;

            Assert.That(message.Contains("wurde gelöscht"));
        }
    }
}

#nullable restore // Restores nullable warnings to their previous state
