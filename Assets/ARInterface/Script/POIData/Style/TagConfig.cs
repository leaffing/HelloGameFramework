using LitJson;
using Leaf.POI;

namespace Leaf.POI.Style
{
    /// <summary>
    /// TagConfig:标签配置类
    /// </summary>
    public class TagConfig : DataModelBase
    {
        /// <summary>
        /// 标签自定义配置
        /// </summary>
        public TagConfig()
        {

        }

        #region Fields
        private string _lineColorString = "#FF00FFFF";
        private int _fontSize = 22;
        private bool _isCameraDisable;
        private string _extraConfigContent;
        private int _tapSubType;
        private int _tagType;
        private bool _isHidden;
        private bool _isReadOnly;
        private string _tagTypeName;
        private string _tagSubTypeName;
        private bool _isAutoTagConfig;
        private int _autoTagIdentity;
        private TagControlType _tagControlType;
        private string _iconPath;
        private string _backgroundPath;
        private string _pointerPath;
        private bool _hasRemoteFile;
        private string _cameraSelectionDescription;
        private bool _isMultiSubCamera;

        private TagDetailConfig _tagDetailConfig;
        private ITagExtraConfig _extraConfig;
        private float _iconOpacity;
        private float _backgroundOpacity;
        private float _pointerOpacity;
        private int _index;
        private int _platformId;
        #endregion

        /// <summary>
        /// 线条颜色
        /// </summary>
        public string LineColorString
        {
            get { return _lineColorString; }
            set { _lineColorString = value; }
        }

        /// <summary>
        /// 索引
        /// </summary>
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        /// <summary>
        /// 平台Id
        /// </summary>
        public int PlatformId
        {
            get { return _platformId; }
            set { _platformId = value; }
        }
        /// <summary>
        /// 字体大小
        /// </summary>
        public int FontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;
            }
        }

        /// <summary>
        /// 摄像头是否可用
        /// </summary>
        public bool IsCameraDisabled
        {
            get { return _isCameraDisable; }
            set { _isCameraDisable = value; }
        }

        /// <summary>
        /// 额外属性
        /// </summary>
        public ITagExtraConfig ExtraConfig
        {
            get
            {
                if (_extraConfig == null) return _extraConfig;
                return _extraConfig;
            }
            set { _extraConfig = value; }
        }

        /// <summary>
        /// 额外属性Json内容
        /// </summary>
        public string ExtraConfigContent
        {
            get { return _extraConfigContent; }
            set { _extraConfigContent = value; }
        }

        /// <summary>
        /// 配置对应数据库的TagSubType ID
        /// </summary>
        public int TagSubType
        {
            get { return _tapSubType; }
            set { _tapSubType = value; }
        }

        /// <summary>
        /// 配置对应数据库的TagType ID
        /// </summary>
        public int TagType
        {
            get { return _tagType; }
            set { _tagType = value; }
        }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool IsHidden
        {
            get { return _isHidden; }
            set { _isHidden = value; }
        }
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; }
        }

        /// <summary>
        /// 配置对应数据库的内容项
        /// </summary>
        public string Content { get; set; }


        /// <summary>
        /// 配置对应类型名
        /// </summary>
        public string TagTypeName
        {
            get { return _tagTypeName; }
            set { _tagTypeName = value; }
        }

        /// <summary>
        /// 样式对应名
        /// </summary>
        public string TagSubTypeName
        {
            get { return _tagSubTypeName; }
            set { _tagSubTypeName = value; }
        }

        /// <summary>
        /// 是否自动标签
        /// </summary>
        public bool IsAutoTagConfig
        {
            get { return _isAutoTagConfig; }
            set { _isAutoTagConfig = value; }
        }

        /// <summary>
        /// 自动标签类型标识
        /// </summary>
        public int AutoTagIdentity
        {
            get { return _autoTagIdentity; }
            set { _autoTagIdentity = value; }
        }

        /// <summary>
        /// 配置对应的标签外观类型，如点、引导点、摄像头等
        /// </summary>
        public TagControlType TagControlType
        {
            get { return _tagControlType; }
            set
            {
                _tagControlType = value;
                if (_tagControlType == TagControlType.CircleAnimationIcon)
                {
                    if (!string.IsNullOrEmpty(ExtraConfigContent))
                    {
                        ExtraConfig = JsonMapper.ToObject<CircleAnimationIconExtraConfig>(ExtraConfigContent);
                    }
                    if (ExtraConfig == null)
                    {
                        ExtraConfig = new CircleAnimationIconExtraConfig();
                    }
                }
                else if (_tagControlType == TagControlType.DataSubscribeViewer)
                {
                    if (!string.IsNullOrEmpty(ExtraConfigContent))
                    {
                        ExtraConfig = JsonMapper.ToObject<DataSubscribeTagExtraConfig>(ExtraConfigContent);
                    }
                    if (ExtraConfig == null)
                    {
                        ExtraConfig = new DataSubscribeTagExtraConfig();
                    }
                }
                else
                {
                    ExtraConfig = null;
                }
            }
        }

        /// <summary>
        /// 标签的图标图片路径
        /// </summary>
        public string IconPath
        {
            get { return _iconPath; }
            set
            {
                if (string.Equals(_iconPath, value)) return;
                _iconPath = value;
            }
        }

        /// <summary>
        /// 标签的图标图片透明度
        /// </summary>
        public float IconOpacity
        {
            get { return _iconOpacity; }
            set { _iconOpacity = value; }
        }

        /// <summary>
        /// 标签的背景图片路径
        /// </summary>
        public string BackgroundPath
        {
            get { return _backgroundPath; }
            set { _backgroundPath = value; }
        }

        /// <summary>
        /// 标签的背景图片透明度
        /// </summary>
        public float BackgroundOpacity
        {
            get { return _backgroundOpacity; }
            set { _backgroundOpacity = value; }
        }

        /// <summary>
        /// 标签的指示点图片路径
        /// </summary>
        public string PointerPath
        {
            get { return _pointerPath; }
            set { _pointerPath = value; }
        }

        /// <summary>
        /// 标签的指示点图片透明度
        /// </summary>
        public float PointerOpacity
        {
            get { return _pointerOpacity; }
            set { _pointerOpacity = value; }
        }

        /// <summary>
        /// 是否有远程文件
        /// </summary>
        public bool HasRemoteFile
        {
            get { return _hasRemoteFile; }
            set { _hasRemoteFile = value; }
        }

        /// <summary>
        /// 摄像头描述
        /// </summary>
        public string CameraSelectionDescription
        {
            get
            {
                if (string.IsNullOrEmpty(_cameraSelectionDescription)) return null;
                return _cameraSelectionDescription;
            }
            set { _cameraSelectionDescription = value; }
        }

        /// <summary>
        /// 标签的详情外观配置信息
        /// </summary>
        public TagDetailConfig TagDetailConfig
        {
            get { return _tagDetailConfig; }
            set { _tagDetailConfig = value; }
        }
        
        /// <summary>
        /// 是否多摄像头
        /// </summary>
        public bool IsMultiSubCamera
        {
            get { return _isMultiSubCamera; }
            set { _isMultiSubCamera = value; }
        }
        
        /// <summary>
        /// 准备数据
        /// </summary>
        public void CheckAndPrepareConfig()
        {
            if (TagDetailConfig.ParameterStyles != null)
            {
                foreach (var tagParameterConfig in TagDetailConfig.ParameterStyles)
                {
                    tagParameterConfig.PrepareExtraContent();
                }
            }
            ExtraConfigContent = ExtraConfig.GetConfigJson();
        }

        /// <summary>
        /// 克隆模板数据
        /// </summary>
        /// <param name="getOrigin">是否使用已保存好的Content字段作为克隆对象</param>
        /// <returns>模板数据</returns>
        public TagConfig Clone(bool getOrigin = true)
        {
            var content = Content;
            if (string.IsNullOrEmpty(content) && getOrigin) return null;
            if (!getOrigin) content = JsonMapper.ToJson(this);
            var clone = JsonMapper.ToObject<TagConfig>(content);
            clone.BackgroundPath = BackgroundPath;
            clone.IconPath = IconPath;
            clone.HasRemoteFile = HasRemoteFile;
            clone.PointerPath = PointerPath;
            clone.Content = Content;
            clone.TagType = TagType;
            clone.TagSubType = TagSubType;
            clone.TagTypeName = TagTypeName;
            clone.TagSubTypeName = TagSubTypeName;
            clone.Id = Id;
            if (clone.TagDetailConfig != null && TagDetailConfig != null)
                clone.TagDetailConfig.BackgroundImagePath = TagDetailConfig.BackgroundImagePath;
            return clone;
        }

    }
}
