using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System.Reflection;

namespace Automation.Framework
{
    [TestClass]
    public class SeleniumBase : IDisposable
    {
        protected IWebDriver driver;

        public TestContext? TestContext { get; set; }

        [TestInitialize]
        public void CreateDriver()
        {
            BrowserAttribute browser = (BrowserAttribute)GetCustomAttribute<BrowserAttribute>(TestContext.FullyQualifiedTestClassName, TestContext.TestName);

            if (browser == null)
            {
                throw new InvalidOperationException("No Browser attribute is specified. ");
            }

            if (browser.IsRemote)
            {
                var capabilities = new RemoteSessionSettings();
                capabilities.AddMetadataSetting("browserName", browser.Browser);
                capabilities.AddMetadataSetting("version", browser.Version);
                capabilities.AddMetadataSetting("platform", Platform.CurrentPlatform.ToString());
                Uri uri = new Uri(browser.Url);

                driver = new RemoteWebDriver(uri, capabilities);
            }
            else
            {
                switch (browser.Browser)
                {
                    case "Chrome":
                        driver = new ChromeDriver();
                        break;
                    case "Edge":
                        driver = new EdgeDriver();
                        break;
                    default:
                        throw new InvalidOperationException("Browser " + browser.Browser + " not found!");
                }
            }

            PageAttribute page = (PageAttribute)GetCustomAttribute<PageAttribute>(TestContext.FullyQualifiedTestClassName, TestContext.TestName);

            if (page == null)
            {
                throw new InvalidOperationException("No Page attribute is specified. ");
            }

            driver.Navigate().GoToUrl(page.Url);

        }

        [TestCleanup]
        public void QuitDriver()
        {
            if (driver != null)
                driver.Quit();
        }

        private Attribute GetCustomAttribute<T>(string className, string testName)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var currentType = assembly.GetTypes().FirstOrDefault(f => f.FullName == className);

                if (currentType == null)
                    continue;

                var classAttribute = currentType.GetCustomAttribute(typeof(T));
                if (classAttribute != null)
                {
                    return classAttribute;
                }

                var currentMethod = currentType.GetMethod(testName);

                var methodAttribute = currentMethod?.GetCustomAttribute(typeof(T));
                if (methodAttribute != null)
                {
                    return methodAttribute;
                }
            }

            return null;
        }

        public void Dispose()
        {
            driver.Dispose();
        }
    }
}
