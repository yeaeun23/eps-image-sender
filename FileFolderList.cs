using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using ImageMgr;

namespace GCS.Windows.Forms
{
    enum Types
    {
        FILE, FOLDER
    }

    struct ItemType
    {
        public object ItemInfo;
        public Types Type;
    }

    public partial class FileFolderList : ListView
    {
        Win32 win32 = new Win32();
        ImageList il16 = new ImageList();
        ImageList il32 = new ImageList();
        BackgroundWorker bgIconLoader = new BackgroundWorker();
        List<ItemType> Paths = new List<ItemType>();
        bool use16 = true;
        static string[] _validExtensions = { ".eps" };

        #region Properties
        string _DefaultPath = "C:\\";
        public string DefaultPath
        {
            get { return _DefaultPath; }
            set { _DefaultPath = value; }
        }

        string _SelectedPath = string.Empty;
        public string SelectedPath
        {
            get { return _SelectedPath; }
            set { _SelectedPath = value; }
        }

        bool _isSoloBrowser = true;
        public bool isSoloBrowser
        {
            get { return _isSoloBrowser; }
            set { _isSoloBrowser = value; }
        }
        #endregion

        public FileFolderList()
        {
        }

        public void Load()
        {
            il16.ColorDepth = ColorDepth.Depth32Bit;
            il16.ImageSize = new Size(16, 20);
            il32.ColorDepth = ColorDepth.Depth32Bit;
            il32.ImageSize = new Size(32, 32);

            SmallImageList = il16;
            LargeImageList = il32;
            //View = System.Windows.Forms.View.Details;
            Activation = ItemActivation.TwoClick;
            MultiSelect = false;

            bgIconLoader.WorkerReportsProgress = true;
            bgIconLoader.WorkerSupportsCancellation = true;
            bgIconLoader.DoWork += new DoWorkEventHandler(bgIconLoader_DoWork);
            bgIconLoader.ProgressChanged += new ProgressChangedEventHandler(bgIconLoader_ProgressChanged);
            bgIconLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgIconLoader_RunWorkerCompleted);

            ItemActivate += new EventHandler(FileFolderList_ItemActivate);
            ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(FileFolderList_ItemSelectionChanged);

            Browse(_DefaultPath);
        }

        void bgIconLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Invoke(new MethodInvoker(delegate
            //{
            //    AutoColResize();
            //}));
        }

        // ColumnContent에 맞게 너비 자동 조정
        void AutoColResize()
        {
            foreach (ColumnHeader col in this.Columns)
            {
                //Invoke(new MethodInvoker(delegate
                //{
                col.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                //col.Width = -2;
                //}));
            }
        }

        void bgIconLoader_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                string fullname = (string)e.UserState;
                string name = Path.GetFileName((string)e.UserState);
                ListViewItem item = null;

                Invoke(new MethodInvoker(delegate
                {
                    item = FindItemWithText(name, false, 0, true);
                }));

                if (item != null)
                {
                    try
                    {
                        //if (use16)
                        //{
                        //    if (!il16.Images.ContainsKey(fullname))
                        //    {
                        //        il16.Images.Add(fullname, this.win32.GetIcon(fullname, true));
                        //    }
                        //}
                        //else
                        //{
                        //    if (!il32.Images.ContainsKey(fullname))
                        //    {
                        //        il32.Images.Add(fullname, this.win32.GetIcon(fullname, false));
                        //    }
                        //}

                        System.Diagnostics.Debug.WriteLine(fullname);

                        item.ImageKey = fullname;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                    finally
                    {
                        Application.DoEvents();
                    }
                }
            }
        }

        void bgIconLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            string fullname = string.Empty;

            foreach (ItemType item in Paths)
            {
                if (bgIconLoader.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                #region // get associated icon
                if (item.Type == Types.FOLDER)
                {
                    DirectoryInfo di = (DirectoryInfo)item.ItemInfo;
                    if (use16)
                    {
                        if (!il16.Images.ContainsKey(di.FullName))
                        {
                            il16.Images.Add(di.FullName, win32.GetIcon(di.FullName, true));
                        }
                    }
                    else
                    {
                        Invoke(new MethodInvoker(delegate
                        {
                            if (!il32.Images.ContainsKey(di.FullName))
                            {
                                il32.Images.Add(di.FullName, win32.GetIcon(di.FullName, false));
                            }
                        }));
                    }

                    //this.bgIconLoader.ReportProgress(1, di.FullName);
                    fullname = di.FullName;
                }
                else
                {
                    FileInfo fi = (FileInfo)item.ItemInfo;
                    if (use16)
                    {
                        if (!il16.Images.ContainsKey(fi.FullName))
                        {
                            il16.Images.Add(fi.FullName, win32.GetIcon(fi.FullName, true));
                        }
                    }
                    else
                    {
                        Invoke(new MethodInvoker(delegate
                        {
                            if (!il32.Images.ContainsKey(fi.FullName))
                            {
                                il32.Images.Add(fi.FullName, win32.GetIcon(fi.FullName, false));
                            }
                        }));
                    }

                    //bgIconLoader.ReportProgress(1, fi.FullName);
                    fullname = fi.FullName;
                }
                #endregion

                #region // add to listview
                string name = Path.GetFileName(fullname);
                ListViewItem lvItem = null;

                try
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        lvItem = FindItemWithText(name, false, 0, true);
                    }));
                }
                catch (InvalidOperationException)
                {
                }

                if (lvItem != null)
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        lvItem.ImageKey = fullname;

                        //if (View == View.Details)
                        {
                            if (item.Type == Types.FILE)
                            {
                                lvItem.SubItems[Form1.IDX_FF_TYPE].Text = win32.GetFileType(fullname);
                                lvItem.SubItems[Form1.IDX_FF_SIZE].Text = win32.GetFileSize(fullname);
                            }
                        }
                    }));
                }
                #endregion
            }
        }

        void FileFolderList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (SelectedItems.Count <= 0) { return; }

            //try
            {
                ListViewItem item = SelectedItems[0];
                ItemType type = (ItemType)item.Tag;

                if (type.Type == Types.FOLDER)
                {
                    DirectoryInfo di = (DirectoryInfo)type.ItemInfo;
                    _SelectedPath = di.FullName;
                }
                else
                {
                    FileInfo fi = (FileInfo)type.ItemInfo;
                    _SelectedPath = fi.FullName;
                }
            }
            //catch { }
        }

        void FileFolderList_ItemActivate(object sender, EventArgs e)
        {
            if (SelectedItems.Count <= 0) { return; }

            ListViewItem thisItem = SelectedItems[0];
            ItemType type = (ItemType)thisItem.Tag;

            if (type.Type == Types.FOLDER)
            {
                DirectoryInfo di = (DirectoryInfo)type.ItemInfo;

                if (Directory.Exists(di.FullName))
                    Browse(di.FullName);
            }
        }

        // ListViewItem 추가 - 이전, 폴더, 파일
        public void Browse(string path)
        {
            bgIconLoader.CancelAsync();

            if (View == View.LargeIcon || View == View.Tile)
            {
                use16 = false;
            }

            while (bgIconLoader.IsBusy)
            {
                Application.DoEvents();
            }

            //Sorting = System.Windows.Forms.SortOrder.None;
            //for (int i = 0; i < Columns.Count; i++)
            //{
            //    Columns[i].Text = Columns[i].Text.Replace("▲", "");
            //    Columns[i].Text = Columns[i].Text.Replace("▼", "");
            //}

            Paths.Clear();
            Items.Clear();
            BeginUpdate();

            #region // add "back" item if necessary
            if (_isSoloBrowser)
            {
                DirectoryInfo currentPath = new DirectoryInfo(path);
                if (currentPath.FullName.Length > 3)
                {
                    ListViewItem item = new ListViewItem("...");
                    item.Tag = new ItemType()
                    {
                        ItemInfo = currentPath.Parent,
                        Type = Types.FOLDER
                    };

                    for (int i = 0; i < Columns.Count; i++)
                    {
                        item.SubItems.Add(string.Empty);
                    }

                    Items.Add(item);
                }
            }
            #endregion

            #region // get folders
            foreach (string folder in Directory.GetDirectories(path))
            {
                DirectoryInfo di = new DirectoryInfo(folder);

                if (di.Attributes.ToString().Contains("System"))
                    continue;

                // 숨긴 항목 가리기
                if ((di.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    continue;

                ListViewItem item = new ListViewItem(di.Name);

                //if (!il16.Images.ContainsKey(di.FullName))
                //{
                //    Icon ico32;
                //    il16.Images.Add(di.FullName, win32.GetIcon(di.FullName, out ico32));
                //    il32.Images.Add(di.FullName, ico32);
                //}

                item.ImageKey = di.FullName;
                item.Tag = new ItemType()
                {
                    ItemInfo = di,
                    Type = Types.FOLDER
                };

                // add temp subitems if View was set to Details
                //if (this.View == System.Windows.Forms.View.Details)
                {
                    for (int i = 0; i < Columns.Count; i++)
                    {
                        item.SubItems.Add(string.Empty);
                    }

                    item.SubItems[Form1.IDX_FF_DATE].Text = di.LastWriteTime.ToString();
                    item.SubItems[Form1.IDX_FF_TYPE].Text = "파일 폴더";
                }

                Items.Add(item);

                Paths.Add((ItemType)item.Tag);
            }
            #endregion

            #region // get files
            foreach (string file in Directory.GetFiles(path))
            {
                FileInfo fi = new FileInfo(file);

                if (fi.Attributes.ToString().Contains("System"))
                {
                    continue;
                }

                if (!IsImageExtension(fi.Extension))
                {
                    continue;
                }

                ListViewItem item = new ListViewItem(fi.Name);

                //if (!il16.Images.ContainsKey(fi.FullName))
                //{
                //    Icon ico32;
                //    il16.Images.Add(fi.FullName, this.win32.GetIcon(fi.FullName, out ico32));
                //    il32.Images.Add(fi.FullName, ico32);
                //}

                item.ImageKey = fi.FullName;
                item.Tag = new ItemType()
                {
                    ItemInfo = fi,
                    Type = Types.FILE
                };

                // add temp subitems if View was set to Details
                //if (this.View == System.Windows.Forms.View.Details)
                {
                    for (int i = 0; i < Columns.Count; i++)
                    {
                        item.SubItems.Add(string.Empty);
                    }

                    item.SubItems[Form1.IDX_FF_DATE].Text = fi.LastWriteTime.ToString();
                    item.SubItems[Form1.IDX_FF_EXT].Text = fi.Extension;
                }

                Items.Add(item);

                Paths.Add((ItemType)item.Tag);
            }
            #endregion

            EndUpdate();
            //Refresh();

            Application.DoEvents();

            bgIconLoader.RunWorkerAsync();
        }

        // 파일 확장자 체크
        public static bool IsImageExtension(string ext)
        {
            return _validExtensions.Contains(ext.ToLower());
        }
    }

    internal class Win32
    {
        [StructLayout(LayoutKind.Sequential)]
        struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        static class FILE_ATTRIBUTE
        {
            public const uint FILE_ATTRIBUTE_NORMAL = 0x80;
        }

        static class SHGFI
        {
            public const uint SHGFI_TYPENAME = 0x000000400;
            public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
        }

        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0;    // 'Large icon
        public const uint SHGFI_SMALLICON = 0x1;    // 'Small icon

        [DllImport("shell32.dll")]
        static extern IntPtr SHGetFileInfo(string pszPath,
                                    uint dwFileAttributes,
                                    ref SHFILEINFO psfi,
                                    uint cbSizeFileInfo,
                                    uint uFlags);

        [DllImport("user32")]
        public static extern int DestroyIcon(IntPtr hIcon);

        public string GetFileSize(string fullpath)
        {
            FileInfo fi = new FileInfo(fullpath);
            long size = fi.Length;
            string sizeString = String.Format(new FileSizeFormatProvider(), "{0:fs}", size);

            return sizeString;
        }

        public string GetFileType(string fullpath)
        {
            uint dwFileAttributes = FILE_ATTRIBUTE.FILE_ATTRIBUTE_NORMAL;
            uint uFlags = (SHGFI.SHGFI_TYPENAME | SHGFI.SHGFI_USEFILEATTRIBUTES);
            SHFILEINFO shinfo = new SHFILEINFO();
            IntPtr n = SHGetFileInfo(fullpath, dwFileAttributes, ref shinfo, (uint)Marshal.SizeOf(shinfo), uFlags);

            return shinfo.szTypeName;
        }

        public Icon GetIcon(string fullpath, bool use16)
        {
            Icon ico = null;
            IntPtr hImgSmall;
            IntPtr hImgLarge;
            SHFILEINFO shinfo = new SHFILEINFO();

            if (use16)
            {
                //Use this to get the small Icon
                hImgSmall = SHGetFileInfo(fullpath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON);
                ico = Icon.FromHandle(shinfo.hIcon);
            }
            else
            {
                //Use this to get the large Icon
                hImgLarge = SHGetFileInfo(fullpath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);
                ico = Icon.FromHandle(shinfo.hIcon);
            }

            return ico;
        }
    }

    internal class FileSizeFormatProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter)) return this;
            return null;
        }

        private const string fileSizeFormat = "fs";
        private const Decimal OneKiloByte = 1024M;
        private const Decimal OneMegaByte = OneKiloByte * 1024M;
        private const Decimal OneGigaByte = OneMegaByte * 1024M;

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (format == null || !format.StartsWith(fileSizeFormat))
            {
                return defaultFormat(format, arg, formatProvider);
            }

            if (arg is string)
            {
                return defaultFormat(format, arg, formatProvider);
            }

            Decimal size;

            try
            {
                size = Convert.ToDecimal(arg);
            }
            catch (InvalidCastException)
            {
                return defaultFormat(format, arg, formatProvider);
            }

            string suffix;
            if (size > OneGigaByte)
            {
                size /= OneGigaByte;
                suffix = "GB";
            }
            else if (size > OneMegaByte)
            {
                size /= OneMegaByte;
                suffix = "MB";
            }
            else if (size > OneKiloByte)
            {
                size /= OneKiloByte;
                suffix = "KB";
            }
            else
            {
                suffix = "B";
            }

            string precision = format.Substring(2);
            if (String.IsNullOrEmpty(precision)) precision = "0";
            return String.Format("{0:N" + precision + "}{1}", size, suffix);

        }

        private static string defaultFormat(string format, object arg, IFormatProvider formatProvider)
        {
            IFormattable formattableArg = arg as IFormattable;
            if (formattableArg != null)
            {
                return formattableArg.ToString(format, formatProvider);
            }
            return arg.ToString();
        }
    }
}

// https://www.codeproject.com/Tips/378074/File-and-Folder-ListView