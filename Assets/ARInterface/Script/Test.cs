using Leaf.Common;
using UnityEngine;
using UnityEngine.UI;
using Leaf.VideoCapture;
using Leaf.POI.Style;
using Newtonsoft.Json;

public class Test : MonoBehaviour {

    public Button SaveButton, DisplayButton;
    public RawImage SaveImage, DisplayImage;
	// Use this for initialization
	void Start () {
        TagConfig config = new TagConfig();
        config.TagDetailConfig = new TagDetailConfig();
        config.TagDetailConfig.CameraItemConfig = new CameraParameterConfig();
        config.TagDetailConfig.CameraItemConfig.Margin = "12,34,0,0";
        string json = JsonConvert.SerializeObject(config);
        print(json);
        TagConfig config2 = JsonConvert.DeserializeObject<TagConfig>(json);
        print(config2.TagDetailConfig.CameraItemConfig.ExtraConfigContent);
        print(config2.TagDetailConfig.CameraItemConfig.Margin);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Select()
    {
        //print(FileHelper.SelectFolder());
        print(WindowsDialogHelper.GetFilePath("选择文件", "图片文件(*.png;*.jpg;*.jpeg)\0*.png;*.jpg;*.jpeg",
            Application.streamingAssetsPath.Replace('/', '\\')));
        //print(WindowsDialogHelper.GetFolderPath("OpenFolder"));
    }

    public void SaveTexture()
    {
        Texture2D tex = SaveImage.texture as Texture2D;
        //VideoCapture.CaptureToPng(tex, Application.streamingAssetsPath + "/", "SaveImage");
        //VideoCapture.CaptureToJpg(tex, Application.streamingAssetsPath + "/", "SaveImage");
        StartCoroutine(VideoCaptureHelper.CaptureScreenToPng(Camera.allCameras, Application.streamingAssetsPath + "/", "SaveImage"));
    }

    public void LoadTexture()
    {
        //Texture2D tex = VideoCapture.LoadCapturePngByIO(Application.streamingAssetsPath + "/", "SaveImage");
        Texture2D tex = VideoCaptureHelper.LoadCapturePngByIO(Application.streamingAssetsPath + "/", "SaveImage");
        DisplayImage.texture = tex;
    }
}
