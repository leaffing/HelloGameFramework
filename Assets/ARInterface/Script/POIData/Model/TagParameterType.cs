namespace Leaf.POI
{
    /// <summary>
    /// 标签详情属性类型
    /// </summary>
    public enum TagParameterType
    {
        /// <summary>
        /// 文本
        /// </summary>
        String = 0,
        /// <summary>
        /// 显示浏览器
        /// </summary>
        ShowBrowser = 1,
        /// <summary>
        /// 查询数据表
        /// </summary>
        QueryDataTable = 2,
        /// <summary>
        /// 选择图片
        /// </summary>
        ImageSelector = 4,
        /// <summary>
        /// 选择文件
        /// </summary>
        FileSelector = 5,
        /// <summary>
        /// 选择简单图片集合路径
        /// </summary>
        ImageSetSelector = 6,
        /// <summary>
        /// 选择简单文件集合路径
        /// </summary>
        FileSetSelector = 7,
        /// <summary>
        /// 长文本（有换行无截断）
        /// </summary>
        RichText = 8,
        /// <summary>
        /// 查找数据行
        /// </summary>
        QueryDataRow = 9,
        /// <summary>
        /// 选择图标标签
        /// </summary>
        TagIconOverride = 10,
        /// <summary>
        /// 可标记图片集合
        /// </summary>
        ExtendImageSetSelector =11,
      
    }

    /// <summary>
    /// 摄像头显示模式
    /// </summary>
    public enum TagCameraShowMode
    {
        /// <summary>
        /// 文字按钮
        /// </summary>
        Button = 0,
        /// <summary>
        /// 直接播放框
        /// </summary>
        Viewer = 1,
        /// <summary>
        /// 多视频播放
        /// </summary>
        MultiScreen = 2,
        /// <summary>
        /// 不显示视频按钮
        /// </summary>
        NotShow = 3,
        /// <summary>
        /// 图片按钮
        /// </summary>
        VideoButton = 4,
        /// <summary>
        /// 展开窗口，停止TagDetail界面
        /// </summary>
        ShowSubWindowSuppressed = 5,
        /// <summary>
        /// 展开窗口，并显示TagDetail界面
        /// </summary>
        ShowSubWindowAlongWith = 6
    }
}
