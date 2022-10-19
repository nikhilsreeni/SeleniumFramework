
namespace Automation.Framework
{
    [AttributeUsage(validOn: AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class PageAttribute : Attribute
    {
        public string Url { get; set; } = String.Empty;
    }
}