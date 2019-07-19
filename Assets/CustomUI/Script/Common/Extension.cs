using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Custom.UI
{
    public static class Extension
    {

        static Camera worldCamera = null;
        public static Vector2 GetPositionInScreen(this Graphic graphic, Vector3 pos)
        {
            if (worldCamera == null)
            {
                var temp = graphic.rectTransform as Transform;
                Canvas canvas = null;
                while (true)
                {
                    canvas = temp.GetComponentInParent<Canvas>();
                    if (canvas != null)
                        break;
                    if (temp.parent == null)
                        break;
                    temp = temp.parent;
                }
                worldCamera = canvas.worldCamera;
            }
            return RectTransformUtility.WorldToScreenPoint(worldCamera, pos);
        }
        public static void SetWorldCamera(Camera camera)
        {
            worldCamera = camera;
        }

        public static Bounds GetBoundRect(this List<Vector3> vector3s)
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue);
            for (int i = 0; i < (vector3s?.Count ?? 0); ++i)
            {
                min.x = vector3s[i].x > min.x ? min.x : vector3s[i].x;
                min.y = vector3s[i].y > min.y ? min.y : vector3s[i].y;
                max.x = vector3s[i].x < max.x ? max.x : vector3s[i].x;
                max.y = vector3s[i].y < max.y ? max.y : vector3s[i].y;
            }
            Bounds bounds = new Bounds();
            bounds.max = max;
            bounds.min = min;
            return bounds;
        }

        public static Vector2 GetPositionInLoccalRectangle(RectTransform rect, Vector2 pos)
        {
            if (rect == null) rect = worldCamera.transform as RectTransform;
            Vector2 localPosition = Vector2.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, pos, worldCamera, out localPosition))
            {

            }
            return localPosition;
        }
        public static int[] GetTriangulateIndex(this List<Vector2> m_points)
        {
            List<int> indices = new List<int>();

            int n = m_points.Count;
            //如果顶点个数不能构成三角形，那么直接返回顶点序列
            if (n < 3)
                return indices.ToArray();

            int[] V = new int[n];//顶点索引
                                 //计算面积是否大于零，如果面积小于零，则对顶点进行逆序，保证顶点按逆时针排序
            if (Area(m_points) > 0)
            {
                for (int v = 0; v < n; v++)
                    V[v] = v;
            }
            else
            {
                for (int v = 0; v < n; v++)
                    V[v] = (n - 1) - v;
            }

            int nv = n;
            int count = 2 * nv;
            for (int m = 0, v = nv - 1; nv > 2;)
            {
                if ((count--) <= 0)
                    return indices.ToArray();

                int u = v;
                if (nv <= u)
                    u = 0;
                v = u + 1;
                if (nv <= v)
                    v = 0;
                int w = v + 1;
                if (nv <= w)
                    w = 0;

                if (Snip(m_points, u, v, w, nv, V))
                {
                    int a, b, c, s, t;
                    a = V[u];
                    b = V[v];
                    c = V[w];
                    indices.Add(a);
                    indices.Add(b);
                    indices.Add(c);
                    m++;
                    for (s = v, t = v + 1; t < nv; s++, t++)
                        V[s] = V[t];
                    nv--;
                    count = 2 * nv;
                }
            }

            indices.Reverse();
            return indices.ToArray();
        }
        private static float Area(List<Vector2> m_points)
        {
            int n = m_points.Count;
            float A = 0.0f;
            for (int p = n - 1, q = 0; q < n; p = q++)
            {
                Vector2 pval = m_points[p];
                Vector2 qval = m_points[q];
                A += pval.x * qval.y - qval.x * pval.y;
            }
            return (A * 0.5f);
        }
        private static bool Snip(List<Vector2> m_points, int u, int v, int w, int n, int[] V)
        {
            int p;
            Vector2 A = m_points[V[u]];
            Vector2 B = m_points[V[v]];
            Vector2 C = m_points[V[w]];
            if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
                return false;
            for (p = 0; p < n; p++)
            {
                if ((p == u) || (p == v) || (p == w))
                    continue;
                Vector2 P = m_points[V[p]];
                if (InsideTriangle(A, B, C, P))
                    return false;
            }
            return true;
        }
        private static bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
        {
            float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
            float cCROSSap, bCROSScp, aCROSSbp;

            ax = C.x - B.x; ay = C.y - B.y;
            bx = A.x - C.x; by = A.y - C.y;
            cx = B.x - A.x; cy = B.y - A.y;
            apx = P.x - A.x; apy = P.y - A.y;
            bpx = P.x - B.x; bpy = P.y - B.y;
            cpx = P.x - C.x; cpy = P.y - C.y;

            aCROSSbp = ax * bpy - ay * bpx;
            cCROSSap = cx * apy - cy * apx;
            bCROSScp = bx * cpy - by * cpx;
            return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
        }

        public static bool ContainsThePoint(this List<Vector2> polygon,Vector2 point)
        {
            int polySides = (polygon?.Count ?? 0);
            if (0 >= polySides)
                return false;
            int i, j = polySides - 1;
            bool oddNodes = false;

            for (i = 0; i < polySides; i++)
            {
                if ((polygon[i].y < point.y && polygon[j].y >= point.y
                || polygon[j].y < point.y && polygon[i].y >= point.y)
                && (polygon[i].x <= point.x || polygon[j].x <= point.x))
                {
                    oddNodes ^= (polygon[i].x + (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) * (polygon[j].x - polygon[i].x) < point.x);
                }
                j = i;
            }
            return oddNodes;
        }
    }
}
