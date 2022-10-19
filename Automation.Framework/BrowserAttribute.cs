

namespace Automation.Framework
{

    [AttributeUsage(validOn: AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class BrowserAttribute : Attribute
    {
        public string Browser { get; set; } = String.Empty;
        public string Version { get; set; } = String.Empty;
        public bool IsRemote { get; set; }
        public string Url { get; set; } = String.Empty;
    }
}
