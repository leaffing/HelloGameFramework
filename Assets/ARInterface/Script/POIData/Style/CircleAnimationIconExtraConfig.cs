using LitJson;
using UnityEngine;

namespace Leaf.POI.Style
{
    /// 负责人：徐冠杰
    /// <summary>
    /// 标签自定义配置标签外观类型为CircleAnimationIcon的额外信息集合，仅供XAML反射属性用
    /// </summary>
    public class CircleAnimationIconExtraConfig : ITagExtraConfig
    {
        private double _iconHeight;
        private double _iconWidth;
        private string _iconMargin;
        private double _circleHeight;
        private double _circleWidth;
        private string _circleColor = "#FFFFFFFF";
        /// <summary>
        /// 图标高
        /// </summary>
        public double IconHeight
        {
            get { return _iconHeight; }
            set { _iconHeight = value; }
        }
        /// <summary>
        /// 图标位置
        /// </summary>
        public string IconMargin
        {
            get { return _iconMargin; }
            set { _iconMargin = value; }
        }
        /// <summary>
        /// 图标宽
        /// </summary>
        public double IconWidth
        {
            get { return _iconWidth; }
            set { _iconWidth = value; }
        }
        /// <summary>
        /// 圈高
        /// </summary>
        public double CircleHeight
        {
            get { return _circleHeight; }
            set { _circleHeight = value; }
        }
        /// <summary>
        /// 圈宽
        /// </summary>
        public double CircleWidth
        {
            get { return _circleWidth; }
            set { _circleWidth = value; }
        }
        /// <summary>
        /// 圈颜色
        /// </summary>
        public string CircleColor
        {
            get { return _circleColor; }
            set { _circleColor = value; }
        }

        /// <summary>
        /// 获取配置的Json
        /// </summary>
        /// <returns>Json</returns>
        public string GetConfigJson()
        {
            return JsonMapper.ToJson(this);
        }
    }
}
