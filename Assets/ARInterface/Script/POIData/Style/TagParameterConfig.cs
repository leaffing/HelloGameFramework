using System;
using LitJson;
using UnityEngine;

namespace Leaf.POI.Style
{
    /// <summary>
    /// TagParameterConfig������������Ϣ
    /// </summary>
    public class TagParameterConfig
    {
        /// <summary>
        /// ��ǩ��������
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
        /// ���������С
        /// </summary>
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        /// <summary>
        /// �Ƿ�ֻд��ֻ����������Ϣ������ҵ��ʹ�ã�
        /// </summary>
        public bool IsSaveOnly
        {
            get { return _isSaveOnly; }
            set { _isSaveOnly = value; }
        }

        /// <summary>
        /// �Ƿ�ʹ�ö�����ʽ
        /// </summary>
        public bool AlterStyle
        {
            get { return _alterStyle; }
            set { _alterStyle = value; }
        }

        /// <summary>
        /// �ؼ���
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
        /// �Ƿ�ʹ��Сͼ��
        /// </summary>
        public bool UseHeaderIcon
        {
            get { return _useHeaderIcon; }
            set { _useHeaderIcon = value; }
        }

        /// <summary>
        /// Сͼ���ַ
        /// </summary>
        public string HeaderIconPath
        {
            get { return _headerIconPath; }
            set { _headerIconPath = value; }
        }

        /// <summary>
        /// �Ƿ�ֻ�����ԣ�ֻ��ȡ���ò���ʾ�������ڴ�����ǩʱ�޸ģ�
        /// </summary>
        public bool IsDisplayProperty
        {
            get { return _isDisplayProperty; }
            set { _isDisplayProperty = value; }
        }

        /// <summary>
        /// ׼����������
        /// </summary>
        public void PrepareExtraContent()
        {
            if (_extraConfig != null)
            {
                _extraConfigContent = _extraConfig.GetConfigJson();
            }
        }

        /// <summary>
        /// ��������Json����
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
        /// ��ǩ����ı�ʶ��
        /// </summary>
        public string ValueCode
        {
            get { return _valueCode; }
            set { _valueCode = value; }
        }

        /// <summary>
        /// �ؼ�����Լ�϶
        /// </summary>
        public string Margin
        {
            get { return _margin; }
            set { _margin = value; }
        }

        /// <summary>
        /// �ؼ�����
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
        /// �ؼ���������ToolTipʹ�ã�
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }        

        /// <summary>
        /// �Ƿ����
        /// </summary>
        public bool IsRequired
        {
            get { return _isRequired; }
            set { _isRequired = value; }
        }

        /// <summary>
        /// �����������༭��ǩʱ������˳������ؼ���Ŀؼ��㼶��
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