
namespace Membership
{
    public class Base
    {
#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        public WebDriver driver;
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method

        [SetUp]
        public void Setup()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            var options = new ChromeOptions();
            //options.AddArgument("--headless=new");
            options.AddArgument("--disable-search-engine-choice-screen");
            options.AddArgument("no-sandbox");
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
    }
}
