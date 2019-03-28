/*
using System;
using LitJson;

namespace Leaf.POI.Style
{
    public class TagConfigJsonReader
    {
        public static TagConfig JsonToTagConfig(string json)
        {
            TagConfig config = new TagConfig();

            JsonData configData = JsonMapper.ToObject(json);
            config.FontSize = (int)configData["FontSize"];
            config.ExtraConfigContent = configData["ExtraConfigContent"].ToString();
            config.EnableCamera = (bool)configData["EnableCamera"];
            config.Id = (long)configData["Id"];
            config.TagSubType = (int)configData["TagSubType"];
            config.TagType = (int)configData["TagType"];
            config.Index = (int)configData["Index"];
            config.IsHidden = (bool)configData["IsHidden"];
            config.IsReadOnly = (bool)configData["IsReadOnly"];
            config.Content = configData["content"].ToString();
            config.TagTypeName = configData["TagTypeName"].ToString();
            config.PlatformId = (int)configData["platformId"];
            config.TagSubTypeName = configData["TagSubTypeName"].ToString();
            config.IsAutoTagConfig = (bool)configData["IsAutoTagConfig"];
            config.AutoTagIdentity = (int)configData["AutoTagIdentity"];
            config.TagControlType = (TagControlType)(int)configData["TagControlType"];
            config.IconPath = configData["IconPath"].ToString();
            config.IconOpacity = (float)configData["IconOpacity"];
            config.BackgroundPath = configData["BackgroundPath"].ToString();
            config.BackgroundOpacity = (float)configData["BackgroundOpacity"];
            config.PointerPath = configData["PointerPath"].ToString();
            config.IconOpacity = (float)configData["PointerPath"];
            config.HasRemoteFile = (bool)configData["HasRemoteFile"];
            config.IsMultiSubCamera = (bool)configData["IsMultiSubCamera"];
            config.CameraSelectionDescription = configData["CameraSelectionDescription"].ToString();
            config.TagDetailConfig = JsonToTagDetailConfig(configData["TagDetailConfig"].ToString());
            return config;
        }

        public static TagDetailConfig JsonToTagDetailConfig(string json)
        {
            TagDetailConfig config = new TagDetailConfig();
            JsonData configData = JsonMapper.ToObject(json);
            config.NameItemConfig = new TagParameterConfig();
            config.CameraItemConfig = new CameraParameterConfig();
            config.CloseItemConfig = new TagParameterConfig();
            config.NameItemConfig.UseHeaderIcon = (bool)configData["UseNameHeaderIcon"];

            return config;
        }
    }
}
*/