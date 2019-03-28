using UnityEngine;

public class ProjectCamera : MonoBehaviour {

    public Texture2D tex;

    private Camera mainProjectCamera;

    //correction矩阵:一个顶点，经过MVP变化之后，其xyz分量的取值范围是[-1, 1]
    //使用这个变化过的顶点值来找到shadow depth map中对应的点来比较深度，即要作为UV使用，而UV的取值范围是[0, 1]
    //所以需要进行一个值域的变换，这就是这个矩阵的作用。
    //要使这个矩阵成立，该vector4的w分量必须是1
    Matrix4x4 posToUV = new Matrix4x4();

    public Camera MainProjectCamera
    {
        get
        {
            return mainProjectCamera;
        }
        private set
        {
            mainProjectCamera = value;
        }
    }

    // Use this for initialization
    void Start() {
        MainProjectCamera = GetComponent<Camera>();
        MainProjectCamera.fieldOfView = 7f;
        MainProjectCamera.aspect = 1;
        print("当前投射相机宽高比为：" + MainProjectCamera.aspect);

        Shader.EnableKeyword("MAINCAMERAON");
        Shader.SetGlobalTexture("_MainLightMaps", tex);
        Shader.SetGlobalTexture("_ProjectTexture", tex);
        Shader.SetGlobalTexture("_MainCameraDepthTexture", Texture2D.blackTexture);

        posToUV.SetRow(0, new Vector4(0.5f, 0, 0, 0.5f));
        posToUV.SetRow(1, new Vector4(0, 0.5f, 0, 0.5f));
        posToUV.SetRow(2, new Vector4(0, 0, 1, 0));
        posToUV.SetRow(3, new Vector4(0, 0, 0, 1));
    }

    // Update is called once per frame
    void Update()
    {
        SetShader();
    }

    private void SetShader()
    {
        //完成MVP的V（world -> view）
        Matrix4x4 worldToView = MainProjectCamera.worldToCameraMatrix;
        //完成MVP的P（view -> projection）
        Matrix4x4 projection = GL.GetGPUProjectionMatrix(MainProjectCamera.projectionMatrix, false);
        Matrix4x4 m = posToUV * projection * worldToView;
        //将投射矩阵写入shader的全局属性
        Shader.SetGlobalMatrix("_MainLightProjection", m);

        //完成MVP的V（world -> view）
        Matrix4x4 CameraWorldToView = Camera.main.worldToCameraMatrix;
        //完成MVP的P（view -> projection）
        Matrix4x4 CameraProjection = GL.GetGPUProjectionMatrix(Camera.main.projectionMatrix, false);
        Matrix4x4 cm = posToUV * CameraProjection * CameraWorldToView;
        //将投射矩阵写入shader的全局属性
        Shader.SetGlobalMatrix("_MainCameraProjection", m);

        Shader.SetGlobalVector("_MainCameraWorldPos", MainProjectCamera.transform.position);
    }
    /// <summary>
    /// 利用屏幕中不同直线三点坐标求平面
    /// </summary>
    /// <param name="P1">点1坐标</param>
    /// <param name="P2">点2坐标</param>
    /// <param name="P3">点3坐标</param>
    /// <returns>平面系数(A,B,C,D)</returns>
    private static Vector4 GetPlan(Vector3 P1, Vector3 P2, Vector3 P3)
    {
        float A = (P3.y - P1.y) * (P3.z - P1.z) - (P2.z - P1.z) * (P3.y - P1.y);
        float B = (P3.x - P1.x) * (P2.z - P1.z) - (P2.x - P1.x) * (P3.z - P1.z);
        float C = (P2.x - P1.x) * (P3.y - P1.y) - (P3.x - P1.x) * (P2.y - P1.y);
        float D = -(A * P1.x + B * P1.y + C * P1.z);
        return new Vector4(A, B, C, D);
    }

    private void GetPerspectivePlane()
    {
        float fov = mainProjectCamera.fieldOfView;
        float asp = mainProjectCamera.aspect;
        float yf = Mathf.Tan(fov / 2 * Mathf.Deg2Rad);
        float xf = yf * asp;
        Matrix4x4 l2w = mainProjectCamera.transform.localToWorldMatrix;
        Vector3 LeftBottomVector = l2w * new Vector3(-xf, -yf, 1);
        Vector3 LeftTopVector = l2w * new Vector3(-xf, yf, 1);
        Vector3 RightBottomVector = l2w * new Vector3(xf, -yf, 1);
        Vector3 RightTopVector = l2w * new Vector3(xf, yf, 1);
        //float fcp = mainProjectCamera.farClipPlane;
        //float ncp = mainProjectCamera.nearClipPlane;
        //Vector3 cpt = mainProjectCamera.transform.position;
        //Vector3 farLeftBottom = cpt + fcp * LeftBottomVector;
        //Vector3 farLeftTop = cpt + fcp * LeftTopVector;
        //Vector3 farRightBotoom = cpt + fcp * RightBottomVector;
        //Vector3 farRightTop = cpt + fcp * RightTopVector;
        //Vector4 farClipPlane = GetPlan(farLeftBottom, farLeftTop, farRightBotoom);
        //Shader.SetGlobalVector("_PerspectivePlane", farClipPlane);

        Ray ray = new Ray(MainProjectCamera.transform.position, LeftBottomVector);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Shader.SetGlobalVector("_MainLeftBottomPos", hitInfo.point);
        }
        ray = new Ray(MainProjectCamera.transform.position, LeftTopVector);
        if (Physics.Raycast(ray, out hitInfo))
        {
            Shader.SetGlobalVector("_MainLeftLeftTopPos", hitInfo.point);
        }
        ray = new Ray(MainProjectCamera.transform.position, RightBottomVector);
        if (Physics.Raycast(ray, out hitInfo))
        {
            Shader.SetGlobalVector("_MainLeftRightBottomPos", hitInfo.point);
        }
        //ray = new Ray(MainProjectCamera.transform.position, RightTopVector);
        //if (Physics.Raycast(ray, out hitInfo))
        //{
        //    Shader.SetGlobalVector("_MainLeftRightTopPos", hitInfo.point);
        //}
    }
}
