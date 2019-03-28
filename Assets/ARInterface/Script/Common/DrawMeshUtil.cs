using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{

    public class DrawMeshUtil
    {
        public static GameObject DrawPolygon(Color color,List<Vector3> points, Material mat, Transform parent)
        {
            GameObject go = new GameObject("DrawnMesh");
            go.transform.SetParent(parent);
            MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
            meshRenderer.material = mat;
            Color colorT = new Color(color.r, color.g, color.b, 0.27f);
            meshRenderer.material.color = colorT;
            MeshFilter meshFilter = go.AddComponent<MeshFilter>();
            //通过渲染器对象得到网格对象
            Mesh mesh = meshFilter.mesh;
            mesh.vertices = points.ToArray();

            List<int> indexes = new List<int>();
            for (int i = 0; i < points.Count; i++)
            {
                indexes.Add(i);
            }
            mesh.triangles = Triangulation.WidelyTriangleIndex(points , indexes).ToArray();

            //Vector3[] normals = new Vector3[mesh.vertices.Length];
            //for (int i = 0; i < mesh.vertices.Length; ++i)
            //{
            //    normals[i] = new Vector3(0, 0, 1);
            //}
            //mesh.normals = normals;
            mesh.RecalculateBounds();
            //mesh.RecalculateTangents();
            //targetFilter.mesh = mesh;
            return go;
        }

        /// <summary>
        /// 绘制三角形网格：为正常显示，双面绘制
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        public static GameObject DrawTriangleMesh(Material material, Vector3 point1, Vector3 point2, Vector3 point3, Transform parent)
        {
            GameObject go = new GameObject("DrawnMesh");
            go.transform.SetParent(parent);
            MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
            meshRenderer.material = material;
            MeshFilter meshFilter = go.AddComponent<MeshFilter>();
            //通过渲染器对象得到网格对象
            Mesh mesh = meshFilter.mesh;

            //设置顶点，这个属性非常重要
            //三个点确定一个面，所以Vector3数组的数量一定是3个倍数
            //遵循顺时针三点确定一面
            //这里的数量为6 也就是创建了2个三角面
            //依次填写3D坐标点
            mesh.vertices = new Vector3[] { point1, point2, point3 };

            //设置贴图点，因为面确定出来以后就是就是2D 
            //所以贴纸贴图数量为Vector2 
            //第一个三角形设置5个贴图
            //第二个三角形设置一个贴图
            //数值数量依然要和顶点的数量一样
            mesh.uv = new Vector2[] { new Vector2(point1.x,point1.z), new Vector2(point2.x, point2.z), new Vector2(point3.x, point3.z) };

            //设置三角形索引，这个索引是根据上面顶点坐标数组的索引
            //对应着定点数组Vector3中的每一项
            //最后将两个三角形绘制在平面中
            //数值数量依然要和顶点的数量一样
            mesh.triangles = new int[] { 0, 1, 2, 0, 2, 1 };
            return go;
        }

        public static GameObject DrawLine(Color color, Vector3[] drawPoints, Material mat, float lineWidth, Transform parent)
        {
            Color colorT = new Color(color.r, color.g, color.b, 0.6f);

            GameObject go = new GameObject("LineRender");
            go.transform.SetParent(parent);
            LineRenderer lineRender = go.AddComponent<LineRenderer>();
            lineRender.startWidth = lineWidth;
            lineRender.endWidth = lineWidth;
            lineRender.material = mat;
            lineRender.material.color = colorT;

            lineRender.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            lineRender.receiveShadows = false;
            lineRender.positionCount = drawPoints.Length;
            //Vector3[] points = new Vector3[] { drawPoints[0], drawPoints[1] };
            lineRender.SetPositions(drawPoints);
            //for (int i = 1; i < drawPoints.Length - 1; i++)
            //{
            //    GameObject goChild = new GameObject("LineRender");
            //    goChild.transform.SetParent(go.transform);
            //    LineRenderer lineRenderChild = goChild.AddComponent<LineRenderer>();
            //    lineRenderChild.startWidth = lineWidth;
            //    lineRenderChild.endWidth = lineWidth;
            //    lineRenderChild.material = mat;
            //    lineRenderChild.material.color = colorT;
            //    lineRenderChild.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //    lineRenderChild.receiveShadows = false;
            //    lineRenderChild.positionCount = 2;
            //    lineRenderChild.SetPositions(new Vector3[] { drawPoints[i], drawPoints[i+1] });
            //}
            return go;
        }
    }

}
