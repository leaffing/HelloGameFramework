using System.Collections.Generic;

namespace Leaf.POI.Style
{
    /// <summary>
    /// TagDetailConfig：详情控件的配置
    /// </summary>
    public class TagDetailConfig
    {
        #region Fields
        private string _backgroundImagePath;
        private float _backgroundImageOpacity;
        private double _width;
        private double _height;
        private List<TagParameterConfig> _parameterStyles;
        private CameraParameterConfig _cameraItemConfig;
        private TagParameterConfig _nameItemConfig;
        private TagParameterConfig _closeItemConfig;
        #endregion
        /// <summary>
        /// 摄像头相关配置
        /// </summary>
        public CameraParameterConfig CameraItemConfig
        {
            get { return _cameraItemConfig; }
            set { _cameraItemConfig = value; }
        }

        /// <summary>
        /// 名称原件配置
        /// </summary>
        public TagParameterConfig NameItemConfig
        {
            get { return _nameItemConfig; }
            set { _nameItemConfig = value; }
        }

        /// <summary>
        /// 关闭页面原件配置
        /// </summary>
        public TagParameterConfig CloseItemConfig
        {
            get { return _closeItemConfig; }
            set { _closeItemConfig = value; }
        }

        /// <summary>
        /// 详情控件高
        /// </summary>
        public double Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// 控件宽
        /// </summary>
        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// 背景图片路径
        /// </summary>
        public string BackgroundImagePath
        {
            get { return _backgroundImagePath; }
            set { _backgroundImagePath = value; }
        }

        /// <summary>
        /// 背景图片透明度
        /// </summary>
        public float BackgroundImageOpacity
        {
            get { return _backgroundImageOpacity; }
            set { _backgroundImageOpacity = value; }
        }

        /// <summary>
        /// 可配置参数的配置列表
        /// </summary>
        public List<TagParameterConfig> ParameterStyles
        {
            get { return _parameterStyles; }
            set { _parameterStyles = value; }
        }
    }
}