using System;
using System.Runtime.InteropServices;

namespace Leaf.Common
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class OpenDialogFile
    {
        public int structSize = 0;
        public IntPtr dlgOwner = IntPtr.Zero;
        public IntPtr instance = IntPtr.Zero;
        public string filter = null;
        public string customFilter = null;
        public int maxCustFilter = 0;
        public int filterIndex = 0;
        public string file = null;
        public int maxFile = 0;
        public string fileTitle = null;
        public int maxFileTitle = 0;
        public string initialDir = null;
        public string title = null;
        public int flags = 0;
        public short fileOffset = 0;
        public short fileExtension = 0;
        public string defExt = null;
        public IntPtr custData = IntPtr.Zero;
        public IntPtr hook = IntPtr.Zero;
        public string templateName = null;
        public IntPtr reservedPtr = IntPtr.Zero;
        public int reservedInt = 0;
        public int flagsEx = 0;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class OpenDialogDir
    {
        public IntPtr hwndOwner = IntPtr.Zero;
        public IntPtr pidlRoot = IntPtr.Zero;
        public string pszDisplayName = null;
        public string lpszTitle = null;
        public uint ulFlags = 0;
        public IntPtr lpfn = IntPtr.Zero;
        public IntPtr lParam = IntPtr.Zero;
        public int iImage = 0;
    }

    public class DllOpenFileDialog
    {
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetOpenFileName([In, Out] OpenDialogFile ofn);

        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetSaveFileName([In, Out] OpenDialogFile ofn);

        [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SHBrowseForFolder([In, Out] OpenDialogDir ofn);

        [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool SHGetPathFromIDList([In] IntPtr pidl, [In, Out] char[] fileName);
    }

    /// <summary>
    /// Windows对话框帮助类
    /// </summary>
    public class WindowsDialogHelper
    {
        /// <summary>
        /// 文件夹选择对话框
        /// </summary>
        /// <param name="title">对话框名称</param>
        /// <returns>返回选择的文件夹路径，未正确选择则返回空字符串</returns>
        public static string GetFolderPath(string title)
        {
            OpenDialogDir ofn2 = new OpenDialogDir();
            ofn2.pszDisplayName = new string(new char[2000]); ;     // 存放目录路径缓冲区  
            ofn2.lpszTitle = title;// 标题  
            //ofn2.ulFlags = BIF_NEWDIALOGSTYLE | BIF_EDITBOX; // 新的样式,带编辑框  
            IntPtr pidlPtr = DllOpenFileDialog.SHBrowseForFolder(ofn2);
            char[] charArray = new char[2000];
            for (int i = 0; i < 2000; i++)
                charArray[i] = '\0';
            DllOpenFileDialog.SHGetPathFromIDList(pidlPtr, charArray);
            string fullDirPath = new String(charArray);
            fullDirPath = fullDirPath.Substring(0, fullDirPath.IndexOf('\0'));
            return fullDirPath;
        }

        /// <summary>
        /// 文件选择对话框
        /// </summary>
        /// <param name="title">对话框标题</param>
        /// <param name="filter">文件选择过滤器（格式参考："图片文件(*.png;*.jpg) \0 *.png;*.jpg"）</param>
        /// <param name="defaultPath">默认路径</param>
        /// <returns>选择的文件路径，未正确选择则返回空字符串</returns>
        public static string GetFilePath(string title, string filter, string defaultPath)
        {
            OpenDialogFile openFileName = new OpenDialogFile();
            openFileName.structSize = Marshal.SizeOf(openFileName);
            openFileName.filter = filter;
            openFileName.file = new string(new char[256]);
            openFileName.maxFile = openFileName.file.Length;
            openFileName.fileTitle = new string(new char[64]);
            openFileName.maxFileTitle = openFileName.fileTitle.Length;
            openFileName.initialDir = defaultPath;//默认路径
            openFileName.title = title;
            openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

            if (DllOpenFileDialog.GetSaveFileName(openFileName))
            {
                return openFileName.file;
            }
            else
                return string.Empty;
        }
    }
}