namespace Leaf.POI
{
    /// <summary>
    /// ��ǩ������������
    /// </summary>
    public enum TagParameterType
    {
        /// <summary>
        /// �ı�
        /// </summary>
        String = 0,
        /// <summary>
        /// ��ʾ�����
        /// </summary>
        ShowBrowser = 1,
        /// <summary>
        /// ��ѯ���ݱ�
        /// </summary>
        QueryDataTable = 2,
        /// <summary>
        /// ѡ��ͼƬ
        /// </summary>
        ImageSelector = 4,
        /// <summary>
        /// ѡ���ļ�
        /// </summary>
        FileSelector = 5,
        /// <summary>
        /// ѡ���ͼƬ����·��
        /// </summary>
        ImageSetSelector = 6,
        /// <summary>
        /// ѡ����ļ�����·��
        /// </summary>
        FileSetSelector = 7,
        /// <summary>
        /// ���ı����л����޽ضϣ�
        /// </summary>
        RichText = 8,
        /// <summary>
        /// ����������
        /// </summary>
        QueryDataRow = 9,
        /// <summary>
        /// ѡ��ͼ���ǩ
        /// </summary>
        TagIconOverride = 10,
        /// <summary>
        /// �ɱ��ͼƬ����
        /// </summary>
        ExtendImageSetSelector =11,
      
    }

    /// <summary>
    /// ����ͷ��ʾģʽ
    /// </summary>
    public enum TagCameraShowMode
    {
        /// <summary>
        /// ���ְ�ť
        /// </summary>
        Button = 0,
        /// <summary>
        /// ֱ�Ӳ��ſ�
        /// </summary>
        Viewer = 1,
        /// <summary>
        /// ����Ƶ����
        /// </summary>
        MultiScreen = 2,
        /// <summary>
        /// ����ʾ��Ƶ��ť
        /// </summary>
        NotShow = 3,
        /// <summary>
        /// ͼƬ��ť
        /// </summary>
        VideoButton = 4,
        /// <summary>
        /// չ�����ڣ�ֹͣTagDetail����
        /// </summary>
        ShowSubWindowSuppressed = 5,
        /// <summary>
        /// չ�����ڣ�����ʾTagDetail����
        /// </summary>
        ShowSubWindowAlongWith = 6
    }
}
