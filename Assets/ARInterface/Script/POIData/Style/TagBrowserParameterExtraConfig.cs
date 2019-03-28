using LitJson;

namespace Leaf.POI.Style
{
    public class TagBrowserParameterExtraConfig : ITagExtraConfig
    {
        private TagBrowserParameterType _parameterType;
        private string _baseAddress;
        private string _referenceConfigCode;
        private double _browserHeight = 800;
        private double _browserWidth = 1600;
        private WebKernelType _browserKernel = WebKernelType.Chrome;

        public TagBrowserParameterType ParameterType
        {
            get { return _parameterType; }
            set { _parameterType = value; }
        }

        public string BaseAddress
        {
            get { return _baseAddress; }
            set { _baseAddress = value; }
        }

        public string ReferenceConfigCode
        {
            get { return _referenceConfigCode; }
            set { _referenceConfigCode = value; }
        }

        public double BrowserHeight
        {
            get { return _browserHeight; }
            set { _browserHeight = value; }
        }

        public double BrowserWidth
        {
            get { return _browserWidth; }
            set { _browserWidth = value; }
        }

        public WebKernelType BrowserKernel
        {
            get { return _browserKernel; }
            set { _browserKernel = value; }
        }

        public string GetConfigJson()
        {
            return JsonMapper.ToJson(this);
        }
    }
}
