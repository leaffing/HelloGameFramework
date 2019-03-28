using System;
using LitJson;
using UnityEngine;

namespace Leaf.POI.Style
{
    /// <summary>
    /// TagParameterConfig：参数配置信息
    /// </summary>
    public class TagParameterConfig
    {
        /// <summary>
        /// 标签属性配置
        /// </summary>
        public TagParameterConfig(){ }

        #region Fields
        private int _fontSize = 14;
        private bool _isSaveOnly;
        private bool _alterStyle;
        private double _height;
        private double _width;
        private bool _useHeaderIcon;
        private bool _isDisplayProperty;
        private string _valueCode;
        private string _margin;
        private TagParameterType _type;
        private string _description;
        private bool _isRequired;
        private ITagExtraConfig _extraConfig;
        private string _extraConfigContent;
        private string _headerIconPath;
        private int _index;
        private int _id;
        private bool _isSelected;
        #endregion

        /// <summary>
        /// 内容字体大小
        /// </summary>
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        /// <summary>
        /// 是否只写（只用来保存信息供其它业务使用）
        /// </summary>
        public bool IsSaveOnly
        {
            get { return _isSaveOnly; }
            set { _isSaveOnly = value; }
        }

        /// <summary>
        /// 是否使用额外样式
        /// </summary>
        public bool AlterStyle
        {
            get { return _alterStyle; }
            set { _alterStyle = value; }
        }

        /// <summary>
        /// 控件高
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
        /// 是否使用小图标
        /// </summary>
        public bool UseHeaderIcon
        {
            get { return _useHeaderIcon; }
            set { _useHeaderIcon = value; }
        }

        /// <summary>
        /// 小图标地址
        /// </summary>
        public string HeaderIconPath
        {
            get { return _headerIconPath; }
            set { _headerIconPath = value; }
        }

        /// <summary>
        /// 是否只读属性（只读取配置并显示，不能在创建标签时修改）
        /// </summary>
        public bool IsDisplayProperty
        {
            get { return _isDisplayProperty; }
            set { _isDisplayProperty = value; }
        }

        /// <summary>
        /// 准备额外属性
        /// </summary>
        public void PrepareExtraContent()
        {
            if (_extraConfig != null)
            {
                _extraConfigContent = _extraConfig.GetConfigJson();
            }
        }

        /// <summary>
        /// 额外属性Json内容
        /// </summary>
        public string ExtraConfigContent
        {
            get { return _extraConfigContent; }
            set { _extraConfigContent = value; }
        }

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
        /// 标签组件的标识符
        /// </summary>
        public string ValueCode
        {
            get { return _valueCode; }
            set { _valueCode = value; }
        }

        /// <summary>
        /// 控件的相对间隙
        /// </summary>
        public string Margin
        {
            get { return _margin; }
            set { _margin = value; }
        }

        /// <summary>
        /// 控件类型
        /// </summary>
        public TagParameterType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                if (_type == TagParameterType.ShowBrowser)
                {
                    if (!(_extraConfig is TagBrowserParameterExtraConfig))
                        ExtraConfig = new TagBrowserParameterExtraConfig();
                }
                else if (_type == TagParameterType.TagIconOverride)
                {
                    if (!(_extraConfig is TagIconParameterExtraConfig))
                        ExtraConfig = new TagIconParameterExtraConfig();
                }
                else
                {
                    _extraConfig = null;
                }

                if (string.IsNullOrEmpty(_extraConfigContent)) return;
                if (Type == TagParameterType.ShowBrowser)
                {
                    try
                    {
                        var target = JsonMapper.ToObject<TagBrowserParameterExtraConfig>(_extraConfigContent);
                        _extraConfig = target;
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }

                if (Type != TagParameterType.TagIconOverride) return;
                try
                {
                    var target = JsonMapper.ToObject<TagIconParameterExtraConfig>(_extraConfigContent);
                    _extraConfig = target;
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }
        
        /// <summary>
        /// 控件描述（供ToolTip使用）
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }        

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired
        {
            get { return _isRequired; }
            set { _isRequired = value; }
        }

        /// <summary>
        /// 索引（创建编辑标签时的属性顺序；详情控件里的控件层级）
        /// </summary>
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }
    }
}