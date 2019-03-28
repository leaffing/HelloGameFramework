using UnityEngine;
using System;
using System.Data;
using UnityEngine.UI;
using SaveGetImageMySql;

public class MySQLTest : MonoBehaviour
{
    public Texture2D texture;
    public RawImage image;

    string Error = null;
    void Start()
    {
        TestSQL();

        //SqlAccess sql = new SqlAccess();
        //sql.SendFileBytesToDatabase("test_image", texture.EncodeToPNG());
        //Texture2D tex = new Texture2D(1, 1);
        //tex.LoadImage(sql.GetFileFromDatabase("test_image"));
        //tex.Apply();
        //image.texture = tex;
        //sql.Close();
    }

    private void TestSQL()
    {
        try
        {
            SqlAccess sql = new SqlAccess();
            //sql.CreateTableAutoID("user",new string[]{"id","name","qq","email","blog"}, new string[]{"int","text","text","text","text"});
            //sql.CreateTable("user",new string[]{"name","qq","email","blog"}, new string[]{"text","text","text","text"});
            sql.InsertInto("user", new string[] { "name", "qq", "email", "blog" }, new string[] { "circle", "289187120", "iiccttff@gmail.com", "circle.com" });
            sql.InsertInto("user", new string[] { "name", "qq", "email", "blog" }, new string[] { "circle01", "34546546", "circle01@gmail.com", "circle01.com" });

            DataSet ds = sql.SelectWhere("user", new string[] { "name", "qq" }, new string[] { "id" }, new string[] { "!=" }, new string[] { "1" });
            if (ds != null)
            {
                DataTable table = ds.Tables[0];
                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        Debug.Log(row[column]);
                    }
                }
            }
            sql.UpdateInto("user", new string[] { "name", "qq" }, new string[] { "'circle01'", "'11111111'" }, "email", "'010101@gmail.com'");
            sql.Delete("user", new string[] { "id", "email" }, new string[] { "1", "'000@gmail.com'" });
            sql.Close();
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
    }

    void OnGUI()
    {
        if (Error != null)
        {
            GUILayout.Label(Error);
        }
    }
}