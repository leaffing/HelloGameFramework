namespace Leaf.POI.Style
{
    /// <summary>
    /// 摄像头入口参数配置
    /// </summary>
    public class CameraParameterConfig : TagParameterConfig
    {
        private TagCameraShowMode _cameraShowMode;
        private bool _cameraCodeUseConverter;
        private string _converterUrl;
        private string _converterParameterCode;

        /// <summary>
        /// 视频响应模式
        /// </summary>
        public TagCameraShowMode CameraShowMode
        {
            get { return _cameraShowMode; }
            set { _cameraShowMode = value; }
        }

        /// <summary>
        /// 播放设备信息时，是否使用转换器
        /// </summary>
        public bool CameraCodeUseConverter
        {
            get { return _cameraCodeUseConverter; }
            set { _cameraCodeUseConverter = value; }
        }

        /// <summary>
        /// 转换器的地址
        /// </summary>
        public string ConverterUrl
        {
            get { return _converterUrl; }
            set { _converterUrl = value; }
        }

        /// <summary>
        /// 转换器使用的参数
        /// </summary>
        public string ConverterParameterCode
        {
            get { return _converterParameterCode; }
            set { _converterParameterCode = value; }
        }
    }
}
