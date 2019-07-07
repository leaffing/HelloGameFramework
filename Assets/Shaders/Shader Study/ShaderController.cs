using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    public int lod = 600;
    // Start is called before the first frame update
    void Start()
    {
        Shader.Find("Custom/ShaderOptions").maximumLOD = lod;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
