using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using GameFramework.Localization;
using System;

namespace Leaf.EditorTools
{
    public class LocalizationWindow : EditorWindow
    {
        private string path, lanKey, lanValue;
        private static XmlDocument xmlDoc;
        private XmlNode dictionaryNodes;
        private Language language;
        private Vector2 _scrollPos;

        [MenuItem("Leaf/Localization Tool")]
        public static void Open()
        {
            xmlDoc = new XmlDocument();
            var window = GetWindow<LocalizationWindow>(true, "多语言设置", true);
            window.minSize = new Vector2(600, 630f);
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            language = (Language)EditorGUILayout.EnumPopup(language, GUILayout.Width(200));
            if (GUILayout.Button("加载语言文件"))
            {
                LoadLocalization();
            }
            GUI.enabled = dictionaryNodes != null;
            if (GUILayout.Button("保存语言文件"))
            {
                SaveXML();
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            lanKey = EditorGUILayout.TextField("Key: ", lanKey);
            lanValue = EditorGUILayout.TextField("Value: ", lanValue);
            EditorGUILayout.BeginHorizontal();
            {
                GUI.enabled = dictionaryNodes != null;
                if (GUILayout.Button("查询"))
                {
                    SearchStringKey(ref lanKey, ref lanValue);
                }
                GUI.enabled = dictionaryNodes != null && !string.IsNullOrEmpty(lanKey) && !string.IsNullOrEmpty(lanValue);
                if (GUILayout.Button("增加"))
                {
                    AddString(lanKey, lanValue);
                }
                GUI.enabled = dictionaryNodes != null && !string.IsNullOrEmpty(lanKey);
                if (GUILayout.Button("删除"))
                {
                    DeleteString(lanKey);
                }
            }
            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            if (dictionaryNodes != null)
            {
                foreach (XmlNode node in dictionaryNodes.ChildNodes)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(node.Attributes["Key"].Value);
                    node.Attributes["Value"].Value = EditorGUILayout.TextField(node.Attributes["Value"].Value);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void LoadLocalization()
        {
            string lan = language == Language.Unspecified ? Language.ChineseSimplified.ToString() : language.ToString(); 
            path = string.Format("Assets/GameMain/Localization/{0}/Dictionaries/{1}.xml", lan, "Default");
            if (File.Exists(path))
            {
                xmlDoc.Load(path);
                dictionaryNodes = xmlDoc.SelectSingleNode("Dictionaries/Dictionary");
                EditorUtility.DisplayDialog("Tip", "成功加载多语言文件！", "确定");
            }
            else
                EditorUtility.DisplayDialog("Tip", "多语言文件不存在!", "确定");
        }

        private void DeleteString(string key)
        {
            XmlNode deleteNode = null;
            foreach (XmlNode node in dictionaryNodes.ChildNodes)
            {
                if (node.Attributes["Key"].Value == key)
                {
                    if (EditorUtility.DisplayDialog("Tip", string.Format("是否确定删除Key为\"{0}\"的键值对？", key), "确定", "取消"))
                    {
                        deleteNode = node;
                        break;
                    }
                    else
                        return;
                }
            }
            dictionaryNodes.RemoveChild(deleteNode);
            SaveXML();
        }

        private void SearchStringKey(ref string key, ref string value)
        {
            foreach (XmlNode node in dictionaryNodes.ChildNodes)
            {
                if (node.Attributes["Key"].Value == key || (string.IsNullOrEmpty(key) && node.Attributes["Value"].Value == value))
                {
                    key = node.Attributes["Key"].Value;
                    value = node.Attributes["Value"].Value;
                    return;
                }
            }
            //key = value = string.Empty;
            EditorUtility.DisplayDialog("Tip", "多语言文件中不存在您查询的键值对!", "确定");
        }

        private void AddString(string key, string value)
        {
            if (IsExitsKey(key))
            {
                if (EditorUtility.DisplayDialog("Tip", string.Format("Key\"{0}\"已经存在，是否需要替换原有值？", key), "确定", "取消"))
                {
                    ModifyString(key, value);
                }
            }
            else
            {
                XmlElement group = xmlDoc.CreateElement("String");
                group.SetAttribute("Key", key);
                group.SetAttribute("Value", value);
                dictionaryNodes.AppendChild(group);
                SaveXML();
            }
            
        }
        private bool IsExitsKey(string key)
        {
            foreach (XmlNode node in dictionaryNodes.ChildNodes)
            {
                if (node.Attributes["Key"].Value == key)
                {
                    return true;
                }
            }
            return false;
        }

        private void SaveXML()
        {
            try
            {
                xmlDoc.Save(path);
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Tip", "保存语言文件成功！", "确定");
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("Tip", string.Format("保存语言文件发生错误：{0}", e.Message), "确定");
            }
        }

        private void ModifyString(string key, string value)
        {
            foreach (XmlNode node in dictionaryNodes.ChildNodes)
            {
                if (node.Attributes["Key"].Value == key)
                {
                    node.Attributes["Value"].Value = value;
                    SaveXML();
                    return;
                }
            }
        }
    }
}