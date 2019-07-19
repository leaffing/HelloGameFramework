using UnityEngine;
using System.Collections;
 
[RequireComponent(typeof(Camera))]
public class DepthRenderer : MonoBehaviour {
 
    public Camera depthCamera;
    Shader replacementShader=null;
 
    public RenderTexture depthTexture;
    // Use this for initialization
    void Start () 
    {
        depthCamera.enabled=false;
        depthCamera.hideFlags=HideFlags.HideAndDontSave;
         
        depthCamera.CopyFrom(Camera.main);
        depthCamera.cullingMask=1<<0; // default layer for now
        depthCamera.clearFlags=CameraClearFlags.Depth;
 
        replacementShader=Shader.Find("ShadowMap/DepthTextureShader");
        if (replacementShader==null)
        {
            Debug.LogError("could not find 'RenderDepth' shader");
        }
    }
     
    // Update is called once per frame
    void OnPreRender () 
    {
        if (replacementShader!=null)
        {
            depthCamera.backgroundColor = Color.black;
            depthCamera.clearFlags = CameraClearFlags.Color; ;
            depthCamera.targetTexture = depthTexture;
            depthCamera.enabled = false;
            depthCamera.RenderWithShader(Shader.Find("ShadowMap/DepthTextureShader"), "RenderType");
        }
    }
}