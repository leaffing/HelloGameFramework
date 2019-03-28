using UnityEngine;

namespace Leaf.POI
{
    /// <summary>
    /// 平台标签拓展信息类
    /// </summary>
    public class PlatformTagExtend
    {
        /// <summary>
        /// 标签类型:0 点,1 矢量,2 区域
        /// </summary>
        public int TagType { get; set; }

        /// <summary>
        /// PT
        /// </summary>
        public Vector2[] PanTilt { get; set; }

        /// <summary>
        /// 经纬度
        /// </summary>
        public Vector2[] LongitudeAndLatitude { get; set; }

        /// <summary>
        /// 焦距
        /// </summary>
        public double Distance { get; set; }
    }
}
