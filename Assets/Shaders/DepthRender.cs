using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class DepthRender : MonoBehaviour
{
    public Camera mainCamera, depthCamera;
    public RenderTexture rt;
    //GameObject depthCamera = null;
    Shader replacementShader = null;

    // Use this for initialization
    void Start()
    {
        //depthCamera = new GameObject();
        //depthCamera.AddComponent<Camera>();
        depthCamera.enabled = false;
        depthCamera.hideFlags = HideFlags.HideAndDontSave;

        depthCamera.CopyFrom(mainCamera);
        depthCamera.cullingMask = 1 << 0; // default layer for now
        depthCamera.clearFlags = CameraClearFlags.Depth;

        replacementShader = Shader.Find("Custom/Depth");
        if (replacementShader == null)
        {
            Debug.LogError("could not find 'RenderDepth' shader");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (replacementShader != null)
        {
            Camera camCopy = depthCamera;

            // copy position and location;
            //rt = RenderTexture.GetTemporary(256, 256, 0);
            camCopy.transform.position = mainCamera.transform.position;
            camCopy.transform.rotation = mainCamera.transform.rotation;
            camCopy.backgroundColor = Color.black;
            camCopy.clearFlags = CameraClearFlags.Color; ;
            camCopy.targetTexture = rt;
            camCopy.enabled = false;
            camCopy.RenderWithShader(replacementShader, "RenderType");
            //RenderTexture.ReleaseTemporary(rt);
        }
    }
}