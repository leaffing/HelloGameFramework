using System.Collections.Generic;

namespace Leaf.POI.Style
{
    /// <summary>
    /// TagDetailConfig������ؼ�������
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
        /// ����ͷ�������
        /// </summary>
        public CameraParameterConfig CameraItemConfig
        {
            get { return _cameraItemConfig; }
            set { _cameraItemConfig = value; }
        }

        /// <summary>
        /// ����ԭ������
        /// </summary>
        public TagParameterConfig NameItemConfig
        {
            get { return _nameItemConfig; }
            set { _nameItemConfig = value; }
        }

        /// <summary>
        /// �ر�ҳ��ԭ������
        /// </summary>
        public TagParameterConfig CloseItemConfig
        {
            get { return _closeItemConfig; }
            set { _closeItemConfig = value; }
        }

        /// <summary>
        /// ����ؼ���
        /// </summary>
        public double Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// �ؼ���
        /// </summary>
        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// ����ͼƬ·��
        /// </summary>
        public string BackgroundImagePath
        {
            get { return _backgroundImagePath; }
            set { _backgroundImagePath = value; }
        }

        /// <summary>
        /// ����ͼƬ͸����
        /// </summary>
        public float BackgroundImageOpacity
        {
            get { return _backgroundImageOpacity; }
            set { _backgroundImageOpacity = value; }
        }

        /// <summary>
        /// �����ò����������б�
        /// </summary>
        public List<TagParameterConfig> ParameterStyles
        {
            get { return _parameterStyles; }
            set { _parameterStyles = value; }
        }
    }
}