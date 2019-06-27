using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapCoordinate : MonoBehaviour
{
    public double lat, lng;
    public int zoom;
    public RawImage[] image;
    public Text text;

    public Tilemap map;
    public float zoomSpeed = 10;
    private int currentZoom;
    private Tile[] arrTiles;
	// Use this for initialization
	void Start ()
    {
        AsyncWait();
        ChangeText();
        FuncTest(3, 5, r => Debug.LogError(r));

        arrTiles = new Tile[25];
        for (int i = 0; i < 25; i++)
        {
            arrTiles[i] = ScriptableObject.CreateInstance<Tile>(); //创建Tile，注意，要使用这种方式
            arrTiles[i].color = Color.white;
            //arrTiles[i].sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 4, 4), Vector2.zero);
        }
        currentZoom = zoom;
        LoadMap(currentZoom, lng , lat);
        Vector2 offset= Vector2.zero;
        LnglatToTilePixelOffset(currentZoom, new GpsData(lng, lat), out offset);
        map.tileAnchor = new Vector3(-offset.x, offset.y - 1, 0);

    }

    private int imageNum;
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.D))
        {
            LoadMap(currentZoom, lng , lat);
        }

        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = -map.transform.position.y + Camera.main.transform.position.y;
            Vector3 wordPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3Int cellPosition = map.WorldToCell(wordPosition);
            Debug.LogError(cellPosition.x + "-" + cellPosition.y) ;
            Debug.Log("鼠标坐标" + mousePosition + "世界" + wordPosition + "cell" + cellPosition);
        }

        if (Input.GetMouseButtonDown(1))
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.transform.position.y;
            Vector3 wordPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            var target = WebMercatorUnityToWGS84(new GpsData(0, 0), wordPosition);
            Vector2 offset= Vector2.zero;
            LnglatToTilePixelOffset(currentZoom, target, out offset);
            Debug.LogError(wordPosition + "-" + target.Latitude + "-" + target.Longitude) ;
            map.transform.parent.position = wordPosition;
            Camera.main.transform.position = wordPosition + Vector3.up * 200;
            map.tileAnchor = new Vector3(-offset.x, offset.y - 1, 0);
            LoadMap(currentZoom, target.Longitude, target.Latitude);
        }

        Camera.main.fieldOfView += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed;
        if (Camera.main.fieldOfView >= 120)
        {
            currentZoom--;
            LoadMap(currentZoom, lng , lat);
        }
        else if (Camera.main.fieldOfView <= 30)
        {
            currentZoom++;
            LoadMap(currentZoom, lng , lat);
        }
    }

    private void LoadMap(int zoom, double lng, double lat)
    {
        Camera.main.fieldOfView = 60;
        imageNum = 0;
        var tile = LaglatToTile(zoom, new GpsData(lng, lat));
        for (int i = tile.Y - 2; i < tile.Y + 3; i++)
        {
            for (int j = tile.X - 2; j < tile.X + 3; j++)
            {
                var url = string.Format(
                    "http://mt2.google.cn/vt/lyrs=m@167000000&hl=zh-CN&gl=cn&x={0}&y={1}&z={2}&s=Galil", j, i, tile.Zoom);
                var num = imageNum;
                var ti = i - tile.Y + 2;
                var tj = j - tile.X + 2;
                StartCoroutine(GetTextureByWebRequest(url, (t) =>
                {
                    //image[num].texture = t;
                    var spr = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 100f);
                    map.GetComponentInParent<UnityEngine.Grid>().cellSize = new Vector3(256f/100, 256f/ 100, 0);
                    arrTiles[num].sprite = spr;
                    map.SetTile(new Vector3Int(tj - 2, 2 - ti, 0), arrTiles[num]);
                    map.RefreshTile(new Vector3Int(tj - 2, 2 - ti, 0));
                }));
                imageNum++;
            }
        }
    }

    IEnumerator GetTextureByWebRequest(string url, Action<Texture2D> CallBack)
    {
        UnityWebRequest request = new UnityWebRequest(url);
        request.downloadHandler = new DownloadHandlerTexture();
        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.LogError("Net error!");
        }
        else
        {
            Texture2D tex = (request.downloadHandler as DownloadHandlerTexture).texture;
            if (tex != null)
                CallBack?.Invoke(tex);
        }

    }

    public static GpsData TileToLnglat(GoogleTile tile) {
        double n = Math.Pow(2, tile.Zoom);
        double lng = tile.X / n * 360.0 - 180.0;
        double lat = Math.Atan(Math.Sinh(Math.PI * (1 - 2 * tile.Y / n)));
        lat = lat * 180.0 / Math.PI;
        return new GpsData(lng, lat);

    }


    //将lnglat坐标系转换为tile坐标系
    public static GoogleTile LaglatToTile(int zoom, GpsData lnglat) {
        double n = Math.Pow(2, zoom);
        double tileX = ((lnglat.Longitude + 180) / 360) * n;
        double tileY = (1 - (Math.Log(Math.Tan(Mathf.Deg2Rad * lnglat.Latitude) + (1 / Math.Cos(Mathf.Deg2Rad * lnglat.Latitude))) / Math.PI)) / 2 * n;
        return new GoogleTile((int)tileX, (int)tileY, zoom);
    }

    public static void LnglatToTilePixelOffset(int zoom, GpsData lnglat, out Vector2 offset)
    {
        double n = Math.Pow(2, zoom);
        double tileX = ((lnglat.Longitude + 180) / 360) * n;
        double tileY = (1 - (Math.Log(Math.Tan(Mathf.Deg2Rad * lnglat.Latitude) + (1 / Math.Cos(Mathf.Deg2Rad * lnglat.Latitude))) / Math.PI)) / 2 * n;
        offset = new Vector2((float)(tileX - (int)tileX), (float)(tileY - (int)tileY)); 
    }

    /// <summary>
    /// 84坐标系经纬度转换为WebMercator模拟Untiy坐标系(相对)
    /// </summary>
    /// <param name="centerWGS">Unity坐标原点对应的84坐标值</param>
    /// <param name="targetWGS">目标点84坐标值</param>
    /// <param name="height">相对原点高程</param>
    /// <param name="mapScale">地图比例值</param>
    /// <returns>Unity坐标值(Vector2值，x为纬度，y为经度)</returns>
    public static Vector3 WGS84ToWebMercatorUnity(GpsData centerWGS, GpsData targetWGS, float height = 0, float mapScale = 10000)
    {
        double initX = centerWGS.Longitude * 20037508.34f / (180 * mapScale);
        double initZ = (float)(Math.Log(Math.Tan((90 + centerWGS.Latitude) * Math.PI / 360)) / (Math.PI / 180));
        initZ = initZ * 20037508.34f / (180 * mapScale);
        double x = (targetWGS.Longitude * 20037508.34 / 180) / mapScale - initX;
        double z = (Math.Log(Math.Tan((90 + targetWGS.Latitude) * Math.PI / 360)) / (Math.PI / 180));
        z = (z * 20037508.34 / 180) / mapScale - initZ;
        float y = height / mapScale;
        Vector3 result = new Vector3((float)x, y, (float)z);
        return result;
    }

    /// <summary>
    /// WebMercator模拟Unity坐标转84坐标系经纬度(相对)
    /// </summary>
    /// <param name="centerWGS">Unity坐标原点对应84经纬度(Vector2值，x为纬度，y为经度)</param>
    /// <param name="targetPoint">目标点Unity世界坐标值</param>
    /// <param name="mapScale">地图比例值</param>
    /// <returns>目标点84经纬度(Vector2值，x为纬度，y为经度)</returns>
    public static GpsData WebMercatorUnityToWGS84(GpsData centerWGS, Vector3 targetPoint, float mapScale = 10000)
    {
        double initX = centerWGS.Longitude * 20037508.34f / (180 * mapScale);
        double initZ = (float)(Math.Log(Math.Tan((90 + centerWGS.Latitude) * Math.PI / 360)) / (Math.PI / 180));
        initZ = initZ * 20037508.34f / (180 * mapScale);
        double _lat = ((targetPoint.z + initZ) / 20037508.34f) * 180 * mapScale;
        double lat = (float)(180 / Math.PI * (2 * Math.Atan(Math.Exp(_lat * Math.PI / 180)) - Math.PI / 2));
        double lng = (float)((180 * mapScale * (targetPoint.x + initX)) / 20037508.34);
        return new GpsData(lat, lng, 0);
    }

    public struct GpsData
    {
        public double Longitude;
        public double Latitude;
        public double Altitude;
        public GpsData(double lng, double lat)
        {
            Longitude = lng;
            Latitude = lat;
            Altitude = 0;
        }
        public GpsData(double lng, double lat, double altitude)
        {
            Longitude = lng;
            Latitude = lat;
            Altitude = altitude;
        }
    }

    public struct GoogleTile
    {
        public int X;
        public int Y;
        public int Zoom;

        public GoogleTile(int x, int y, int zoom)
        {
            X = x;
            Y = y;
            Zoom = zoom;
        }
    }

    void FuncTest(int a, int b, Action<int> CallBack)
    {
        Func<int, int, int> func = new Func<int, int, int>(Add);
        func.BeginInvoke(a, b, (ar) =>
        {
            int r = func.EndInvoke(ar);
            CallBack(r);
        }, null);
    }

    public int Add(int a, int b)
    {
        System.Threading.Thread.Sleep(5000);
        return a + b;
    }

    static void AsyncWait()
    {
        Debug.LogError("Task Start !");
        //DotaskWithThread();
        var task=  DOTaskWithAsync();
        Debug.LogError("Task End !");
        //task.Wait();
    }
 
    public static async Task DOTaskWithAsync()
    {
        Debug.LogError("Await Taskfunction Start");
        await Task.Run(() =>
        {
            Dotaskfunction();
        });
 
 
    }

    private async void ChangeText()
    {
        var t = Task.Run(() => {
            Thread.Sleep(5000);
            return "Hello I am TimeConsumingMethod";
        });
        text.text = await t;
    }

    public static void Dotaskfunction()
    {
        for (int i = 0; i <= 5; i++)
        {
            Thread.Sleep(1000);
            Debug.LogErrorFormat("task {0}  has been done!", i);
        }
    }

}
