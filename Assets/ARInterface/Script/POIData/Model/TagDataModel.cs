using System;
using System.Collections.Generic;
using System.Linq;

namespace Leaf.POI
{
    /// <summary>
    /// 标签类
    /// </summary>
    public class TagDataModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 平台标签详细信息
        /// </summary>
        public PlatformTagExtend PlatformTagExtendModel { get; set; }

        /// <summary>
        /// 标签层级(0始终显示，否则根据层级显示)
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 标签所属大类
        /// </summary>
        public int TagType { get; set; }

        /// <summary>
        /// 标签子类型
        /// </summary>
        public int TagSubType { get; set; }

        /// <summary>
        /// 所属高点id
        /// </summary>
        public long HighCamId { get; set; }

        /// <summary>
        /// 所属高点名称
        /// </summary>
        public string HighCamName { get; set; }

        /// <summary>
        /// 关联低点id组
        /// </summary>
        public string LowCamId { get; set; }

        /// <summary>
        /// 关联低点名称组
        /// </summary>
        public string LowCamName { get; set; }

        /// <summary>
        /// 标签详细信息
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 平台id(有多级平台时使用)
        /// </summary>
        public long PlatformId { get; set; }

        private int _tagMode = 1;
        /// <summary>
        /// 标签模式，0设备标签，1平台标签
        /// </summary>
        public int TagMode
        {
            get { return _tagMode; }
            set { _tagMode = value; }
        }

        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// GPS标签
        /// </summary>
        public bool IsAutoTag { get; set; }

        /// <summary>
        /// GPS标签设备Id
        /// </summary>
        public string AutoTagDeviceId { get; set; }

        /// <summary>
        /// 标识自动标签是否超出配置的视野范围
        /// </summary>
        public bool IsOutOfViewRange { get; set; }

        /// <summary>
        /// 标签组件数据列表
        /// </summary>
        public List<TagParameter> TagParameters { get; set; }

        /// <summary>
        /// 获取是否存在通用位置描述信息。如果没有PT也没有GPS，返回false
        /// </summary>
        /// <returns></returns>
        public bool IsCommonLocationDataExist()
        {
            if (PlatformTagExtendModel == null) return false;
            if (PlatformTagExtendModel.PanTilt == null && PlatformTagExtendModel.LongitudeAndLatitude == null)
                return false;
            if (PlatformTagExtendModel.LongitudeAndLatitude.All(x => x.x == 0 && x.y == 0) &&
                PlatformTagExtendModel.PanTilt == null) return false;
            if (PlatformTagExtendModel.PanTilt.All(x => x.x == 0 && x.y == 0) && PlatformTagExtendModel.LongitudeAndLatitude == null) return false;
            return true;
        }

        public TagDataModel Clone()
        {
            return new TagDataModel
            {
                Id = Id,
                Name = Name,
                Level = Level,
                Param = Param,
                HighCamId = HighCamId,
                LowCamId = LowCamId,
                PlatformId = PlatformId,
                TagType = TagType,
                TagSubType = TagSubType,
                TagMode = TagMode,
                Longitude = Longitude,
                Latitude = Latitude,
                UpdateTime = UpdateTime,
                PlatformTagExtendModel = PlatformTagExtendModel,
                AutoTagDeviceId = AutoTagDeviceId
            };
        }

        public bool Equals(TagDataModel tagModel)
        {
            if (tagModel == null) return false;
            if (!Equals(Id, tagModel.Id)) return false;
            if (!string.Equals(Name, tagModel.Name)) return false;
            if (!Equals(Level, tagModel.Level)) return false;
            if (!Equals(TagSubType, tagModel.TagSubType)) return false;
            if (!string.IsNullOrEmpty(Param) && !string.IsNullOrEmpty(tagModel.Param) && !string.Equals(Param, tagModel.Param)) return false;
            if (!string.IsNullOrEmpty(LowCamId) && !string.IsNullOrEmpty(tagModel.LowCamId) && !string.Equals(LowCamId, tagModel.LowCamId)) return false;
            if (!Equals(TagMode, tagModel.TagMode)) return false;
            if (!Equals(Longitude, tagModel.Longitude)) return false;
            if (!Equals(Latitude, tagModel.Latitude)) return false;
            return true;
        }
    }
}
