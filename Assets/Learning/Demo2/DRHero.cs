using System;
using System.IO;
using GameFramework;
using GameFramework.DataTable;

public class DRHero : IDataRow
{
    public int ID { get; protected set; }
    public string Name { get; private set; }
    public int HP { get; private set; }

    public int Id
    {
        get
        {
            return ID;
        }
    }

    bool IDataRow.ParseDataRow(GameFrameworkSegment<string> dataRowSegment)
    {
        string[] text = dataRowSegment.Source.Substring(dataRowSegment.Offset).Split('\t');
        int index = 0;
        index++; // 跳过#注释列
        ID = int.Parse(text[index++]);
        Name = text[index++];
        HP = int.Parse(text[index++]);
        return true;
    }

    bool IDataRow.ParseDataRow(GameFrameworkSegment<byte[]> dataRowSegment)
    {
        throw new NotImplementedException();
    }

    bool IDataRow.ParseDataRow(GameFrameworkSegment<Stream> dataRowSegment)
    {
        throw new NotImplementedException();
    }
}