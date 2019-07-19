using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorController : MonoBehaviour
{
    public Transform[] points;
    public Projector projector;
    public Material mat;
    private Vector4[] worldPos;//存储待绘制的多边形顶点坐标 

	// Use this for initialization
	void Start ()
    {
        mat.SetInt ("PointNum",6);//传递顶点数量给shader
    }
	
	// Update is called once per frame
	void Update () {
        worldPos = new Vector4[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            worldPos[i] = points[i].position;
        }
        mat.SetVectorArray("Value",worldPos);//传递顶点屏幕位置信息给shader
	}
}
