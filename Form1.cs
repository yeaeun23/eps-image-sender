using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace ImageMgr
{
    public partial class Form1 : Form
    {
        public static DataTable dt;
        public static StringBuilder sb = new StringBuilder(255);

        public static string empNo = "";
        public static string empName = "";
        public static string empCode = "0";
        public static string empPubpart = "";

        public static string workFolderPath = "C:\\";
        public static string downloadFolderPath = "C:\\Work\\ImageMgr\\Download";
        public static string tempFolderPath = "C:\\Work\\ImageMgr\\Temp";
        public static string pdfDistFilePath = "";
        public static string gsFilePath = Application.StartupPath + "\\gswin64c.exe";
        public static string iniFilePath = Application.StartupPath + "\\ImageMgr.ini";

        public static int IDX_FF_FILENAME = 0;
        public static int IDX_FF_DATE = 1;
        public static int IDX_FF_TYPE = 2;
        public static int IDX_FF_SIZE = 3;
        public static int IDX_FF_EXT = 4;
        public static int IDX_PI_ID = 0;
        public static int IDX_PI_MEDIA = 1;
        public static int IDX_PI_PAN = 2;
        public static int IDX_PI_PAN_O = 3;
        public static int IDX_PI_MYUN = 4;
        public static int IDX_PI_JIBANG = 5;
        public static int IDX_PI_TYPE = 6;
        public static int IDX_PI_TITLE = 7;
        public static int IDX_PI_SIZE = 8;
        public static int IDX_PI_COLOR = 9;
        public static int IDX_PI_LINK = 10;
        public static int IDX_PI_PAPERDATE = 11;
        public static int IDX_PI_REGDATE = 12;
        public static int IDX_PI_THUMB_FILENAME = 13;
        public static int IDX_PI_PRE_FILENAME = 14;
        public static int IDX_PI_REAL_FILENAME = 15;

        public static int IDX_THEME_BLUE = 0;
        public static int IDX_THEME_WHITE = 1;
        public static Color[] themeBlue = { SystemColors.InactiveCaption, SystemColors.InactiveBorder, Color.White };
        public static Color[] themeWhite = { Color.White, Color.White, SystemColors.MenuBar };

        string tempFilePath = "";                           // EPS 변환 파일 경로
        List<string> realFilePathList = new List<string>(); // EPS 파일 경로
        List<string> pdfFilePathList = new List<string>();  // PDF 파일 경로
        string downloadServerUrl = "";

        string srcDirName = @"\\daps.seoul.co.kr\PatchImageMgr";
        string destDirName = @"C:\이미지배정기";
        string srcVer = "";
        string destVer = "";

        bool isFirstClick = true;
        bool isDoubleClick = false;
        int milliseconds = 0;
        const int DoubleClickTime = 150;

        // ini 파일 쓰기
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        // ini 파일 읽기
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        // 창 활성화
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        // 창 최상위로
        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct NETRESOURCE
        {
            public uint dwScope;
            public uint dwType;
            public uint dwDisplayType;
            public uint dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        }
        [DllImport("mpr.dll", CharSet = CharSet.Auto)]
        public static extern int WNetUseConnection(IntPtr hwndOwner, [MarshalAs(UnmanagedType.Struct)] ref NETRESOURCE lpNetResource, string lpPassword, string lpUserID, uint dwFlags, StringBuilder lpAccessName, ref int lpBufferSize, out uint lpResult);

        public static string ReadValue(string Section, string Key)
        {
            GetPrivateProfileString(Section, Key, string.Empty, sb, 255, Application.StartupPath + "\\seoulcts.ini");
            return sb.ToString();
        }

        public Form1()
        {
            Delay(2000);

            // 열려있는 앱 확인
            Process[] processList = Process.GetProcessesByName("ImageMgr");
            if (processList.Length == 2)
            {
                if (processList[0].StartTime > processList[1].StartTime)
                {
                    ShowWindowAsync(processList[1].MainWindowHandle, 1);
                    SetForegroundWindow(processList[1].MainWindowHandle);
                    processList[0].Kill();
                }
                else
                {
                    ShowWindowAsync(processList[0].MainWindowHandle, 1);
                    SetForegroundWindow(processList[0].MainWindowHandle);
                    processList[1].Kill();
                }
            }
            else
            {
                // 버전 체크
                VersionCheck();

                if (srcVer != destVer)  // 버전이 다르면
                {
                    // 업데이트
                    Process.Start(destDirName + @"\UpdateImageMgr.exe");
                    Process.GetCurrentProcess().Kill();
                }
                else                    // 버전이 같으면
                {
                    // 로그인
                    LoginForm loginForm = new LoginForm();
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (로그인 완료) " + empNo);
                        InitializeComponent();
                    }
                    else
                    {
                        Process.GetCurrentProcess().Kill();
                    }
                }
            }
        }

        private DateTime Delay(int ms)
        {
            DateTime dateTimeNow = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, ms);
            DateTime dateTimeAdd = dateTimeNow.Add(duration);

            while (dateTimeAdd >= dateTimeNow)
            {
                Application.DoEvents();
                dateTimeNow = DateTime.Now;
            }

            return DateTime.Now;
        }

        private void VersionCheck()
        {
            int res = netUse();

            if (res != 0 && res != 1219)
            {
                MessageBox.Show(this, "DAPS 서버에 연결할 수 없습니다. 에러코드: " + res.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                srcVer = File.ReadAllText(srcDirName + @"\ImageMgr_Ver.txt");

                GetPrivateProfileString("VERSION", "VER", "", sb, sb.Capacity, destDirName + @"\ImageMgr.ini");
                if (sb.ToString() != "")
                    destVer = sb.ToString();
            }
        }

        private int netUse()
        {
            int capacity = 64;
            uint resultFlags = 0;
            StringBuilder sb = new StringBuilder(capacity);
            NETRESOURCE ns = new NETRESOURCE();

            ns.dwType = 1;           // 공유 디스크
            ns.lpLocalName = null;   // 로컬 드라이브
            ns.lpRemoteName = srcDirName;
            ns.lpProvider = null;

            return WNetUseConnection(IntPtr.Zero, ref ns, "!!updateuser@@", "daps\\updateuser", 0, sb, ref capacity, out resultFlags);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Form 초기 크기 세팅
            GetPrivateProfileString("FORM_SIZE", "WIDTH", "", sb, sb.Capacity, iniFilePath);
            if (sb.ToString() != "")
                Width = Convert.ToInt32(sb.ToString());

            GetPrivateProfileString("FORM_SIZE", "HEIGHT", "", sb, sb.Capacity, iniFilePath);
            if (sb.ToString() != "")
                Height = Convert.ToInt32(sb.ToString());

            // Form 초기 크기보다 스크린이 작을 경우
            if (Screen.PrimaryScreen.Bounds.Width < Size.Width || Screen.PrimaryScreen.Bounds.Height < Size.Height)
                WindowState = FormWindowState.Maximized;    // Form 최대화

            // 메뉴 렌더링 디버그 해결
            toolStrip1.Renderer = new NewToolStripRenderer();

            // 상태 표시줄 세팅
            toolStripStatusLabel1.Text = empName + "(" + empNo + ")님이 사용중입니다.";

            GetPrivateProfileString("VERSION", "VER", "", sb, sb.Capacity, iniFilePath);
            toolStripStatusLabel2.Text = "Ver " + sb.ToString();

            // 환경설정 세팅
            if (!Directory.Exists(downloadFolderPath))
                Directory.CreateDirectory(downloadFolderPath);

            if (!Directory.Exists(tempFolderPath))
                Directory.CreateDirectory(tempFolderPath);

            GetPrivateProfileString("FONT", "FONT", "", sb, sb.Capacity, iniFilePath);
            if (sb.ToString() != "")
                fontDialog1.Font = (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(sb.ToString());

            GetPrivateProfileString("FONT", "COLOR", "", sb, sb.Capacity, iniFilePath);
            if (sb.ToString() != "")
                fontDialog1.Color = (Color)TypeDescriptor.GetConverter(typeof(Color)).ConvertFromString(sb.ToString());

            fontSet(sender, e);

            GetPrivateProfileString("THEME", "COLOR", "", sb, sb.Capacity, iniFilePath);
            if (sb.ToString().ToLower() == "white")
                themeColorSet(IDX_THEME_WHITE, themeWhite);

            GetPrivateProfileString("FOLDER_PATH", "WORK", "", sb, sb.Capacity, iniFilePath);
            if (sb.ToString() != "")
                workFolderPath = sb.ToString();

            GetPrivateProfileString("FOLDER_PATH", "DOWNLOAD", "", sb, sb.Capacity, iniFilePath);
            if (sb.ToString() != "")
                downloadFolderPath = sb.ToString();

            GetPrivateProfileString("FOLDER_PATH", "TEMP", "", sb, sb.Capacity, iniFilePath);
            if (sb.ToString() != "")
                tempFolderPath = sb.ToString();

            GetPrivateProfileString("FOLDER_PATH", "PDFDIST", "", sb, sb.Capacity, iniFilePath);
            if (sb.ToString() != "")
                pdfDistFilePath = sb.ToString();

            GetPrivateProfileString("SPLITTER", "DISTANCE_V", "", sb, sb.Capacity, iniFilePath);
            if (sb.ToString() != "")
                splitContainer2.SplitterDistance = Convert.ToInt32(sb.ToString());

            GetPrivateProfileString("SPLITTER", "DISTANCE_H", "", sb, sb.Capacity, iniFilePath);
            if (sb.ToString() != "")
                splitContainer1.SplitterDistance = Convert.ToInt32(sb.ToString());

            toolTip1.SetToolTip(folderBrowse_BTN, "폴더 찾아보기");

            // 상주화상 권한 체크
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select ID_SOFTCODE from [CMSCOM].[dbo].[R_USEREXE_PV] where id_usercode = '" + empCode + "' and id_softcode = 2")), "SELECT");
            if (empNo != "" && dt.Rows.Count == 0)
            {
                sj_BTN.Visible = false;
                PI_ToolStripMenuItem6.Visible = false;
            }

            // ListView, PictureBox 로드
            setMediaCBList(media_CB);
            setPanCBList(pan_CB);
            setMyunCBList(myun_CB);
            setJibangCBList(jibang_CB);

            fileFolderListLoad();
            paperImgListLoad();
            pictureBoxLabelSet();

            ActiveControl = toolStrip1;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 열린 SJForm 닫기
            Form[] sjFormList = Application.OpenForms.Cast<Form>().Where(x => x.Name == "SJForm").ToArray();
            if (sjFormList.Length != 0)
                sjFormList[0].Close();

            // 열린 ImgPreviewForm 닫기
            Form[] imgPreviewFormList = Application.OpenForms.Cast<Form>().Where(x => x.Name == "ImgPreviewForm").ToArray();
            foreach (Form imgPreviewForm in imgPreviewFormList)
                imgPreviewForm.Close();

            // pictureBox 정리
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
                toolTip1.SetToolTip(pictureBox1, "");
            }

            // EPS 변환 파일 삭제
            GC.Collect();
            GC.WaitForPendingFinalizers();

            DirectoryInfo di = new DirectoryInfo(tempFolderPath);
            foreach (FileInfo file in di.EnumerateFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (IOException)
                {
                    //MessageBox.Show(ex.ToString());
                }
            }

            // PDF 변환 파일 삭제
            foreach (string file in pdfFilePathList)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }

            // 환경설정 저장
            WritePrivateProfileString("FONT", "FONT", TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(fontDialog1.Font), iniFilePath);
            WritePrivateProfileString("FONT", "COLOR", TypeDescriptor.GetConverter(typeof(Color)).ConvertToString(fontDialog1.Color), iniFilePath);
            WritePrivateProfileString("THEME", "COLOR", MENU_ToolStripMenuItem2_SubItem1.Checked ? "BLUE" : "WHITE", iniFilePath);
            WritePrivateProfileString("FOLDER_PATH", "WORK", workFolderPath, iniFilePath);
            WritePrivateProfileString("FOLDER_PATH", "DOWNLOAD", downloadFolderPath, iniFilePath);
            WritePrivateProfileString("FOLDER_PATH", "TEMP", tempFolderPath, iniFilePath);
            WritePrivateProfileString("FOLDER_PATH", "PDFDIST", pdfDistFilePath, iniFilePath);
            WritePrivateProfileString("FORM_SIZE", "WIDTH", Size.Width.ToString(), iniFilePath);
            WritePrivateProfileString("FORM_SIZE", "HEIGHT", Size.Height.ToString(), iniFilePath);
            WritePrivateProfileString("SPLITTER", "DISTANCE_V", splitContainer2.SplitterDistance.ToString(), iniFilePath);
            WritePrivateProfileString("SPLITTER", "DISTANCE_H", splitContainer1.SplitterDistance.ToString(), iniFilePath);

            // 컬럼 헤더 크기 저장
            for (int i = 0; i < fileFolderList1.Columns.Count; i++)
                WritePrivateProfileString("LV_COLUMN_WIDTH", "LV1_" + i, fileFolderList1.Columns[i].Width.ToString(), iniFilePath);

            for (int i = 0; i < paperImgList1.Columns.Count; i++)
                WritePrivateProfileString("LV_COLUMN_WIDTH", "LV2_" + i, paperImgList1.Columns[i].Width.ToString(), iniFilePath);
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            pictureBoxLabelSet();
        }

        private void pictureBoxLabelSet()
        {
            label1.Size = new Size(pictureBox1.Width - 2, pictureBox1.Height - 2);
            label1.Location = new Point(pictureBox1.Location.X + 1, pictureBox1.Location.Y + 1);
        }

        public void fileFolderListLoad()
        {
            // 컬럼 헤더 크기 세팅
            for (int i = 0; i < fileFolderList1.Columns.Count; i++)
            {
                GetPrivateProfileString("LV_COLUMN_WIDTH", "LV1_" + i, "", sb, sb.Capacity, iniFilePath);
                if (sb.ToString() != "")
                    fileFolderList1.Columns[i].Width = Convert.ToInt32(sb.ToString());
            }

            if (!Directory.Exists(workFolderPath))
                workFolderPath = "C:\\";

            fileFolderList1.DefaultPath = workFolderPath;
            fileFolderList1.Load();

            fileFolderPath_TB.Text = workFolderPath;
        }

        public void paperImgListLoad()
        {
            // 컬럼 헤더 크기 세팅
            for (int i = 0; i < paperImgList1.Columns.Count; i++)
            {
                GetPrivateProfileString("LV_COLUMN_WIDTH", "LV2_" + i, "", sb, sb.Capacity, iniFilePath);
                if (sb.ToString() != "")
                    paperImgList1.Columns[i].Width = Convert.ToInt32(sb.ToString());
            }

            media_CB.SelectedIndex = 1;
            date_CAL.Value = DateTime.Now.AddDays(1);
            pan_CB.SelectedIndex = 1;
            myun_CB.SelectedIndex = empPubpart == "47" ? 1 : 0; // 편집    
            jibang_CB.SelectedIndex = 0;

            if (empPubpart == "55")         // 광고
                type_CB.SelectedIndex = 4;
            else if (empPubpart == "9")     // 미술
                type_CB.SelectedIndex = 6;
            else
                type_CB.SelectedIndex = 0;

            paperImgListSearch();
        }

        // 매체 CB 리스트 세팅
        public static void setMediaCBList(ComboBox cb)
        {
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select id_mechae, name FROM [DAPS].[dbo].[CMS_MECHAE] order by id_mechae")), "SELECT");

            foreach (DataRow dr in dt.Rows)
                cb.Items.Add(dr["name"].ToString() + "-" + dr["id_mechae"].ToString()); // "서울신문-65"              
        }

        // 판 CB 리스트 세팅
        public static void setPanCBList(ComboBox cb)
        {
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select v_pan_name from [DAPS].[dbo].[CMS_PAN] where id_mechae = 65 order by n_pan_code")), "SELECT");

            foreach (DataRow dr in dt.Rows)
                cb.Items.Add(dr["v_pan_name"].ToString().Replace("판", "")); // "5"
        }

        // 면 CB 리스트 세팅
        public static void setMyunCBList(ComboBox cb)
        {
            // 1 ~ 99면까지 추가
            for (int i = 1; i < 100; i++)
                cb.Items.Add(i.ToString()); // "1"
        }

        // 지방 CB 리스트 세팅
        public static void setJibangCBList(ComboBox cb)
        {
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select remark, code from [prodarx2005].[dbo].[JIBANGCODE_TBL] order by code")), "SELECT");

            foreach (DataRow dr in dt.Rows)
                cb.Items.Add(dr["remark"].ToString() + "-" + dr["code"].ToString()); // "전국-1"
        }

        public void paperImgListSearch()
        {
            string sql = string.Format(@"select * from (
select img_id, media, pan, reg_pan, myun, jibang, 
case when img_type is null or img_type = 0 then '' when img_type = 3 then '표' when img_type = 5 then '사진' when img_type = 6 then '기타' when img_type = 7 then '광고' when img_type = 11 then '스캔' when img_type = 13 then '미술' end as img_type, 
title, 
case when len(real_hsz) < 3 and len(real_vsz) < 3 
then STUFF(REPLICATE('0', 3 - len(real_hsz)) + CONVERT(varchar, real_hsz), 2, 0, '.') + ' × ' + STUFF(REPLICATE('0', 3 - len(real_vsz)) + CONVERT(varchar, real_vsz), 2, 0, '.') 
when len(real_hsz) < 3 
then STUFF(REPLICATE('0', 3 - len(real_hsz)) + CONVERT(varchar, real_hsz), 2, 0, '.') + ' × ' + STUFF(real_vsz, len(real_vsz) - 1, 0, '.') 
when len(real_vsz) < 3 
then STUFF(real_hsz, len(real_hsz) - 1, 0, '.') + ' × ' + STUFF(REPLICATE('0', 3 - len(real_vsz)) + CONVERT(varchar, real_vsz), 2, 0, '.') 
else STUFF(real_hsz, len(real_hsz) - 1, 0, '.') + ' × ' + STUFF(real_vsz, len(real_vsz) - 1, 0, '.') end as real_size, 
case when color = 1 then '○' when color = 1 then '' end as color, 
case when input_type = 19 then '○' when input_type = 16 or input_type = 0 then '' end as input_type, 
SUBSTRING(paper_date, 3, 2) + '-' + SUBSTRING(paper_date, 5, 2) + '-' + SUBSTRING(paper_date, 7, 2) as paper_date, 
SUBSTRING(CONVERT(CHAR(19), cre_date, 20), 3, 17) as regist_date, 
thumb_file, print_file, real_file 
from [DAPS].[dbo].[CMS_IMGINFO] where ").Replace("\r\n", "");

            if (date_RB1.Checked)        // 게재일 
                sql += "paper_date = '" + date_CAL.Value.ToString().Substring(0, 10).Replace("-", "") + "' ";
            else if (date_RB2.Checked)   // 등록일
                sql += "CONVERT(CHAR(8), cre_date, 112) = '" + date_CAL.Value.ToString().Substring(0, 10).Replace("-", "") + "' ";

            if (jibang_CB.SelectedIndex > 0)
                sql += "and jibang = '" + jibang_CB.Text.Split('-')[1] + "' ";

            sql += string.Format(@"union 
select sj_id as img_id, media, pan, '' as reg_pan, myun, jibang, 
'소조' as img_type, 
title, 
'' as real_size, 
'' as color, 
'' as input_type, 
SUBSTRING(paper_date, 3, 2) + '-' + SUBSTRING(paper_date, 5, 2) + '-' + SUBSTRING(paper_date, 7, 2) as paper_date, 
SUBSTRING(CONVERT(CHAR(19), reg_time, 20), 3, 17) as regist_date, 
preview_file as thumb_file, '' as print_file, preview_file as real_file 
from [DAPS].[dbo].[CMS_SOJOINFO] where ").Replace("\r\n", "");

            if (date_RB1.Checked)        // 게재일 
                sql += "paper_date = '" + date_CAL.Value.ToString().Substring(0, 10).Replace("-", "") + "' ";
            else if (date_RB2.Checked)   // 등록일
                sql += "CONVERT(CHAR(8), reg_time, 112) = '" + date_CAL.Value.ToString().Substring(0, 10).Replace("-", "") + "' ";

            if (jibang_CB.SelectedIndex > 0)
                sql += "and jibang = '" + jibang_CB.Text.Split('-')[1] + "'";

            sql += ") a where ";

            if (media_CB.SelectedIndex > 0)
                sql += "media = " + media_CB.Text.Split('-')[1] + " ";

            int x;
            if (int.TryParse(pan_CB.Text, out x))
            {
                if (sql.Substring(sql.Length - 6) != "where ")
                    sql += "and ";

                sql += "pan = '" + pan_CB.Text.Replace("판", "") + "' ";
            }

            if (int.TryParse(myun_CB.Text, out x))
            {
                if (sql.Substring(sql.Length - 6) != "where ")
                    sql += "and ";

                sql += "myun = '" + myun_CB.Text.Replace("면", "") + "' ";
            }

            if (type_CB.SelectedIndex > 0)
            {
                if (sql.Substring(sql.Length - 6) != "where ")
                    sql += "and ";

                if (type_CB.Text == "선택안함")
                    sql += "(img_type = '' or img_type is null) ";
                else
                    sql += "img_type = '" + type_CB.Text + "' ";
            }

            if (sql.Substring(sql.Length - 6) == "where ")
                sql = sql.Substring(0, sql.Length - 6);

            sql += "order by myun, regist_date";

            SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + sql);

            paperImgList1.Items.Clear();
            paperImgList1.BeginUpdate();

            dt = Util.ExecuteQuery(new SqlCommand(sql), "SELECT");
            foreach (DataRow dr in dt.Rows)
            {
                ListViewItem item = new ListViewItem(dr["img_id"].ToString());

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    if (i == IDX_PI_JIBANG)
                        item.SubItems.Add(getJibangName(dr.ItemArray[i].ToString()));
                    else
                        item.SubItems.Add(dr.ItemArray[i].ToString());
                }

                paperImgList1.Items.Add(item);
            }

            paperImgList1.EndUpdate();
            //paperImgList1.Refresh();

            SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (제작 리스트 호출 완료)");
            contextMenuStrip2.Hide();
        }

        private string getJibangName(string code)
        {
            DataTable dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select remark from [Prodarx2005].[dbo].[JIBANGCODE_TBL] where code = '{0}'", code)), "SELECT");

            return dt.Rows[0]["remark"].ToString(); // "전국"            
        }

        private void doubleClickTimer_Tick(object sender, EventArgs e)
        {
            milliseconds += 100;

            // The timer has reached the double click time limit.
            if (milliseconds >= DoubleClickTime)
            {
                doubleClickTimer.Stop();

                if (isDoubleClick)
                    fileFolderList_DoubleClickAction(sender, e);
                else
                    fileFolderList_FirstClickAction(sender, e);

                // Allow the MouseDown event handler to process clicks again.
                isFirstClick = true;
                isDoubleClick = false;
                milliseconds = 0;
            }
        }

        private void fileFolderList1_Click(object sender, EventArgs e)
        {
            if (isFirstClick)
            {
                isFirstClick = false;
                doubleClickTimer.Start();
            }
            else
            {
                if (milliseconds < DoubleClickTime)
                    isDoubleClick = true;
            }
        }

        private void fileFolderList1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) // 오른쪽 클릭일 경우
            {
                if (fileFolderList1.SelectedItems[0].SubItems[IDX_FF_EXT].Text != "")   // 파일일 경우
                {
                    if (fileFolderList1.SelectedItems[0].Bounds.Contains(e.Location))
                        contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }

        private void fileFolderList_FirstClickAction(object sender, EventArgs e)
        {
            if (Directory.Exists(fileFolderList1.SelectedPath.Substring(0, 1) + ":"))
            {
                if (Directory.Exists(fileFolderList1.SelectedPath)) // 폴더 선택 - 존재하면
                {
                    fileFolderPath_TB.Text = fileFolderList1.SelectedPath;
                }
                else if (File.Exists(fileFolderList1.SelectedPath)) // 파일 선택 - 존재하면
                {
                    fileFolderPath_TB.Text = fileFolderList1.SelectedPath;
                    fileFolderListImgLoad();
                }
                else                                                // 폴더/파일 선택 - 존재하지 않으면
                {
                    MessageBox.Show(this, "'" + fileFolderList1.SelectedPath + "'을(를) 찾을 수 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    fileFolderPath_TB.Text = Path.GetDirectoryName(fileFolderList1.SelectedPath);
                    fileFolderList1.SelectedPath = fileFolderPath_TB.Text;
                    fileFolderListSearch();
                }
            }
            else    // 드라이브 연결 끊어졌으면
            {
                MessageBox.Show(this, "'" + fileFolderList1.SelectedPath + "'을(를) 찾을 수 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                fileFolderPath_TB.Text = "C:\\";
                fileFolderList1.SelectedPath = fileFolderPath_TB.Text;
                fileFolderListSearch();
            }
        }

        private void fileFolderList_DoubleClickAction(object sender, EventArgs e)
        {
            fileFolderList_FirstClickAction(sender, e);

            if (fileFolderList1.SelectedItems.Count != 0)
            {
                if (fileFolderList1.SelectedItems[0].SubItems[IDX_FF_EXT].Text != "")
                {
                    ImgRegForm imgRegForm = new ImgRegForm(fileFolderList1.SelectedItems, fileFolderPath_TB.Text, date_RB1.Checked, date_CAL.Value, media_CB.SelectedIndex, jibang_CB.SelectedIndex, pan_CB.Text, myun_CB.Text, type_CB.SelectedIndex);
                    if (imgRegForm.ShowDialog() == DialogResult.OK)
                        paperImgListSearch();
                }
            }
        }

        private void fileFolderListImgLoad()
        {
            if (fileFolderList1.SelectedItems.Count == 1)
            {
                if (fileFolderList1.SelectedItems[0].SubItems[IDX_FF_EXT].Text != "")   // 파일 선택할 경우
                {
                    label1.Visible = false;

                    if (fileFolderList1.SelectedItems[0].SubItems[IDX_FF_EXT].Text.ToLower() == ".eps")   // EPS 파일일 경우
                    {
                        tempFilePath = tempFolderPath + "\\" + fileFolderList1.SelectedItems[0].SubItems[IDX_FF_FILENAME].Text.Replace(".eps", ".jpg").Replace(".EPS", ".jpg");

                        if (!File.Exists(tempFilePath)) // 파일 변환 내역이 없으면
                        {
                            // 파일 변환
                            ConvertFile();
                        }
                        else if (TempFileCheck())       // 파일 변환 내역이 있는데, '같은 이름 다른 폴더'의 파일이였으면
                        {
                            // 열린 ImgPreviewForm 닫기
                            Form[] imgPreviewFormList = Application.OpenForms.Cast<Form>().Where(x => x.Name == "ImgPreviewForm").ToArray();
                            foreach (Form imgPreviewForm in imgPreviewFormList)
                            {
                                if (imgPreviewForm.Text == Path.GetFileName(tempFilePath))
                                    imgPreviewForm.Close();
                            }

                            // 파일 삭제
                            File.Delete(tempFilePath);

                            // 파일 변환
                            ConvertFile();
                        }
                        else                            // 파일 변환 내역이 있으면
                        {
                            pictureBox1.ImageLocation = tempFilePath;
                            toolTip1.SetToolTip(pictureBox1, Path.GetFileName(tempFilePath));
                        }
                    }
                    else
                    {
                        pictureBox1.ImageLocation = fileFolderPath_TB.Text;
                        toolTip1.SetToolTip(pictureBox1, Path.GetFileName(fileFolderPath_TB.Text));
                    }
                }
            }
        }

        private void ConvertFile()
        {
            if (ConvertEpsToJpg(tempFilePath))
            {
                pictureBox1.ImageLocation = tempFilePath;
                toolTip1.SetToolTip(pictureBox1, Path.GetFileName(tempFilePath));
                realFilePathList.Add(fileFolderPath_TB.Text);
            }
            else
            {
                pictureBox1.Image = null;
                toolTip1.SetToolTip(pictureBox1, "");
                label1.Text = "파일 변환에 실패했습니다. IT개발부로 문의 바랍니다.";
                label1.Visible = true;
            }
        }

        private bool TempFileCheck()
        {
            foreach (string path in realFilePathList)
            {
                if (Path.GetFileName(fileFolderPath_TB.Text).ToLower() == Path.GetFileName(path).ToLower()) // 파일명이 같음
                {
                    if (fileFolderPath_TB.Text.ToLower() != path.ToLower()) // 폴더는 다름
                    {
                        realFilePathList.Remove(path);
                        return true;
                    }
                }
            }

            return false;
        }

        private void fileFolderList1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            fileFolderPath_TB.Text = fileFolderList1.SelectedPath;

            if (((ListViewItem)e.Item).SubItems[IDX_FF_EXT].Text != "")
                DoDragDrop(e.Item, DragDropEffects.Copy);
        }

        private void fileFolderList1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                fileFolderListImgLoad();

                ImgRegForm imgRegForm = new ImgRegForm(fileFolderList1.SelectedItems, fileFolderPath_TB.Text, date_RB1.Checked, date_CAL.Value, media_CB.SelectedIndex, jibang_CB.SelectedIndex, pan_CB.Text, myun_CB.Text, type_CB.SelectedIndex);
                if (imgRegForm.ShowDialog() == DialogResult.OK)
                    paperImgListSearch();
            }
        }

        public bool ConvertEpsToJpg(string tmpFilePath)
        {
            if (!File.Exists(gsFilePath))
            {
                MessageBox.Show(this, "'" + gsFilePath + "'을(를) 찾을 수 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            pictureBox1.Image = null;
            toolTip1.SetToolTip(pictureBox1, "");
            label1.Text = "파일 변환중입니다. 잠시만 기다려 주세요.";
            label1.Visible = true;
            Cursor = Cursors.WaitCursor;

            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo { FileName = gsFilePath };
            bool success = true;
            string dpi = EPSFileCreatorCheck();

            try
            {
                psi.CreateNoWindow = false;
                psi.UseShellExecute = true;
                psi.Arguments = "-dSAFER -dBATCH -dNOPAUSE -dNOPROMPT -dMaxBitmap=500000000 -dEPSCrop -dAlignToPixels=0 -dGridFitTT=2 -sDEVICE=jpeg -dTextAlphaBits=4 -dGraphicsAlphaBits=4 -dUseCIEColor -r" + dpi + "x" + dpi + " -dProcessColorModel=/DeviceRGB -sOUTPUTFILE=" + (char)34 + tmpFilePath + (char)34 + " " + (char)34 + fileFolderPath_TB.Text + (char)34;

                p.StartInfo = psi;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.Start();
                p.WaitForExit();

                if (p.ExitCode != 0)
                    success = false;    // 변환 실패

                p.Kill();
            }
            catch (Exception ex)
            {
                SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (파일 변환 실패 - EPS → JPG)\n" + ex.ToString());
            }
            finally
            {
                p.Dispose();

                label1.Visible = false;
                Cursor = Cursors.Default;
            }

            return success;
        }

        private string EPSFileCreatorCheck()
        {
            if (File.ReadAllLines(fileFolderPath_TB.Text).FirstOrDefault(l => l.StartsWith("%%Creator:")).Contains("CorelDRAW"))
                return "150";
            else
                return "72";
        }

        private void fileFolderPath_TB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                fileFolderListSearch();
        }

        private void fileFolderListSearch()
        {
            if (Directory.Exists(fileFolderPath_TB.Text.Substring(0, 1) + ":"))
            {
                if (!Directory.Exists(fileFolderPath_TB.Text))    // 폴더 경로가 아닐 경우
                {
                    if (!File.Exists(fileFolderPath_TB.Text)) // 파일 경로도 아닐 경우
                    {
                        MessageBox.Show(this, "'" + fileFolderPath_TB.Text + "'을(를) 찾을 수 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        fileFolderPath_TB.Text = Path.GetDirectoryName(fileFolderPath_TB.Text);
                        fileFolderList1.SelectedPath = fileFolderPath_TB.Text;
                        fileFolderListSearch();
                        return;
                    }
                    else                                      // 파일 경로일 경우 - 속한 폴더 경로로 바꿔줌
                    {
                        fileFolderPath_TB.Text = Path.GetDirectoryName(fileFolderPath_TB.Text);
                    }
                }
            }
            else    // 드라이브 연결 끊어졌으면
            {
                MessageBox.Show(this, "'" + fileFolderPath_TB.Text + "'을(를) 찾을 수 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                fileFolderPath_TB.Text = "C:\\";
            }

            fileFolderList1.Browse(fileFolderPath_TB.Text);
            contextMenuStrip1.Hide();
        }

        private void fileFolderList1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            fileFolderListSearch();
            lvSort(fileFolderList1, e);
        }

        private void paperImgList1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            paperImgListSearch();
            lvSort(paperImgList1, e);
        }

        private void lvSort(ListView lv, ColumnClickEventArgs e)
        {
            // 컬럼헤더 텍스트 초기화
            for (int i = 0; i < lv.Columns.Count; i++)
            {
                lv.Columns[i].Text = lv.Columns[i].Text.Replace("▲", "");
                lv.Columns[i].Text = lv.Columns[i].Text.Replace("▼", "");
            }

            // 소트
            if (lv.Sorting == System.Windows.Forms.SortOrder.Descending)    // 내림차순이면
            {
                lv.ListViewItemSorter = new ListViewItemComparer(e.Column, "asc");
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                lv.Columns[e.Column].Text = lv.Columns[e.Column].Text + "▲";
            }
            else
            {
                lv.ListViewItemSorter = new ListViewItemComparer(e.Column, "desc");
                lv.Sorting = System.Windows.Forms.SortOrder.Descending;
                lv.Columns[e.Column].Text = lv.Columns[e.Column].Text + "▼";
            }

            lv.Sort();
        }

        private void media_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            paperImgListSearch();
        }

        private void date_RB1_CheckedChanged(object sender, EventArgs e)
        {
            paperImgListSearch();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            paperImgListSearch();
        }

        private void pan_CB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Tab)
                paperImgListSearch();
        }

        private void pan_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            paperImgListSearch();
        }

        private void myun_CB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Tab)
                paperImgListSearch();
        }

        private void myun_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            paperImgListSearch();
        }

        private void jibang_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            paperImgListSearch();
        }

        private void type_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            paperImgListSearch();
        }

        private void paperImgList1_Click(object sender, EventArgs e)
        {
            if (paperImgList1.SelectedItems.Count == 1)
            {
                if (paperImgList1.SelectedItems[0].SubItems[IDX_PI_TYPE].Text == "소조")
                {
                    try
                    {
                        string fileName = null;
                        string strSQL = string.Format(@"select preview_file from  daps.dbo.CMS_SOJOINFO where sj_id = {0}", paperImgList1.SelectedItems[0].SubItems[IDX_PI_ID].Text);

                        using (SqlConnection db = new SqlConnection(""))
                        {
                            using (SqlCommand dbCmd = new SqlCommand(strSQL, db))
                            {
                                try
                                {
                                    db.Open();

                                    SqlDataReader reader = dbCmd.ExecuteReader();

                                    while (reader.Read())
                                        fileName = reader["preview_file"].ToString().Trim();

                                    reader.Close();
                                    db.Close();
                                }
                                catch (Exception)
                                {
                                    db.Close();
                                }
                            }
                        }

                        string fullUrl = string.Format("{0}/{1}/{2}", ReadValue("PATTERNMGR", "소조URI"), paperImgList1.SelectedItems[0].SubItems[IDX_PI_MEDIA].Text, fileName);

                        using (WebClient webClient = new WebClient())
                        {
                            webClient.Proxy = null;
                            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted_SOJO);
                            webClient.QueryString.Add("file", fileName);
                            webClient.DownloadFileAsync(new Uri(fullUrl), ReadValue("CONTENTMGR", "통합요소다운로드경로") + "\\" + fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "파일 미리보기에 실패했습니다. IT개발부로 문의 바랍니다.\n\n" + ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    string fileName = paperImgList1.SelectedItems[0].SubItems[IDX_PI_THUMB_FILENAME].Text;

                    pictureBox1.Image = ViewWebImage(@"http://172.30.1.108:8080/" + paperImgList1.SelectedItems[0].SubItems[IDX_PI_MEDIA].Text + "/" + fileName.Substring(2, 4) + "/" + fileName.Substring(6, 4) + "/" + fileName);
                    toolTip1.SetToolTip(pictureBox1, fileName);
                }
            }
        }

        private void DownloadCompleted_SOJO(object sender, AsyncCompletedEventArgs e)
        {
            string fileName = ((WebClient)(sender)).QueryString["file"];

            if (e.Error != null)
                MessageBox.Show(this, "파일 미리보기에 실패했습니다. IT개발부로 문의 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            pictureBox1.ImageLocation = ReadValue("CONTENTMGR", "통합요소다운로드경로") + "\\" + fileName;
            toolTip1.SetToolTip(pictureBox1, fileName);
        }

        private Bitmap ViewWebImage(string url)
        {
            try
            {
                WebClient Downloader = new WebClient();
                Stream ImageStream = Downloader.OpenRead(url);
                Bitmap DownloadImage = Bitmap.FromStream(ImageStream) as Bitmap;

                return DownloadImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "파일 미리보기에 실패했습니다. IT개발부로 문의 바랍니다.\n\n" + ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (파일 미리보기 실패) " + url + "\n" + ex.ToString());
                return null;
            }
        }

        private void paperImgList1_KeyUp(object sender, KeyEventArgs e)
        {
            paperImgList1_Click(sender, e);
        }

        private void fileFolderList1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && fileFolderList1.SelectedItems[0].SubItems[IDX_FF_EXT].Text != "")
                FF_ToolStripMenuItem4_Click(sender, e);
            else if (e.KeyCode != Keys.F5)
                fileFolderList_FirstClickAction(sender, e);
        }

        private void FF_ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fileName = "";
            Image img = null;

            if (fileFolderList1.SelectedItems[0].SubItems[IDX_FF_EXT].Text.ToLower() == ".eps")
                fileName = fileFolderList1.SelectedItems[0].SubItems[IDX_FF_FILENAME].Text.Replace(fileFolderList1.SelectedItems[0].SubItems[IDX_FF_EXT].Text, ".jpg");
            else
                fileName = fileFolderList1.SelectedItems[0].SubItems[IDX_FF_FILENAME].Text;

            if (fileFolderList1.SelectedItems[0].SubItems[IDX_FF_EXT].Text.ToLower() == ".eps")
                img = Image.FromFile(tempFolderPath + "\\" + fileName);
            else
                img = Image.FromFile(fileFolderPath_TB.Text);

            ImgPreviewForm imgPreviewForm = new ImgPreviewForm(fileName, img);
            imgPreviewForm.Show();
        }

        private void regImg_BTN_Click(object sender, EventArgs e)
        {
            if (fileFolderList1.SelectedItems.Count != 0)
            {
                if (fileFolderList1.SelectedItems[0].SubItems[IDX_FF_EXT].Text != "")
                {
                    ImgRegForm imgRegForm = new ImgRegForm(fileFolderList1.SelectedItems, fileFolderPath_TB.Text);
                    if (imgRegForm.ShowDialog() == DialogResult.OK)
                        paperImgListSearch();
                }
            }
        }

        private void MENU_ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                fontSet(sender, e);
            }
        }

        private void fontSet(object sender, EventArgs e)
        {
            fileFolderList1.Font = fontDialog1.Font;
            fileFolderList1.ForeColor = fontDialog1.Color;

            paperImgList1.Font = fontDialog1.Font;
            paperImgList1.ForeColor = fontDialog1.Color;
        }

        private void MENU_ToolStripMenuItem2_SubItem1_Click(object sender, EventArgs e)
        {
            themeColorSet(IDX_THEME_BLUE, themeBlue);
        }

        private void MENU_ToolStripMenuItem2_SubItem2_Click(object sender, EventArgs e)
        {
            themeColorSet(IDX_THEME_WHITE, themeWhite);
        }

        private void themeColorSet(int index, Color[] color)
        {
            for (int i = 0; i < MENU_ToolStripMenuItem2.DropDownItems.Count; i++)
            {
                if (i == index)
                    ((ToolStripMenuItem)MENU_ToolStripMenuItem2.DropDownItems[i]).Checked = true;
                else
                    ((ToolStripMenuItem)MENU_ToolStripMenuItem2.DropDownItems[i]).Checked = false;
            }
            //((ToolStripMenuItem)sender).Checked = true;

            splitContainer2.Panel1.BackColor = color[0];
            splitContainer2.Panel2.BackColor = color[0];
            splitContainer1.Panel2.BackColor = color[1];
            BackColor = color[2];
        }

        private void paperImgList1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (paperImgList1.SelectedItems[0].Bounds.Contains(e.Location))
                {
                    contextMenuStrip2.Show(Cursor.Position);
                }
            }
        }

        private void paperImgInit_BTN_Click(object sender, EventArgs e)
        {
            paperImgListLoad();
        }

        private void FF_ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (fileFolderList1.SelectedItems[0].SubItems[IDX_FF_EXT].Text != "")
            {
                ImgRegForm imgRegForm = new ImgRegForm(fileFolderList1.SelectedItems, fileFolderPath_TB.Text, date_RB1.Checked, date_CAL.Value, media_CB.SelectedIndex, jibang_CB.SelectedIndex, pan_CB.Text, myun_CB.Text, type_CB.SelectedIndex);
                if (imgRegForm.ShowDialog() == DialogResult.OK)
                    paperImgListSearch();
            }
        }

        private void PI_ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fileName = "";
            Image img = null;

            fileName = paperImgList1.SelectedItems[0].SubItems[IDX_PI_PRE_FILENAME].Text == "" ? paperImgList1.SelectedItems[0].SubItems[IDX_PI_THUMB_FILENAME].Text : paperImgList1.SelectedItems[0].SubItems[IDX_PI_PRE_FILENAME].Text;

            if (fileName.Contains(".eps") || fileName.Contains(".EPS"))
                fileName = paperImgList1.SelectedItems[0].SubItems[IDX_PI_THUMB_FILENAME].Text;

            if (paperImgList1.SelectedItems[0].SubItems[IDX_PI_TYPE].Text == "소조")
            {
                ImgPreviewForm imgPreviewForm = new ImgPreviewForm(fileName, pictureBox1.Image);
                imgPreviewForm.Show();
            }
            else
            {
                img = ViewWebImage(@"http://172.30.1.108:8080/" + paperImgList1.SelectedItems[0].SubItems[IDX_PI_MEDIA].Text + "/" + fileName.Substring(2, 4) + "/" + fileName.Substring(6, 4) + "/" + fileName);

                if (img != null)
                {
                    ImgPreviewForm imgPreviewForm = new ImgPreviewForm(fileName, img);
                    imgPreviewForm.Show();
                }
            }
        }

        private void MENU_ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            FolderSetForm folderSetForm = new FolderSetForm();

            if (folderSetForm.ShowDialog() == DialogResult.OK)
            {
                fileFolderPath_TB.Text = workFolderPath;
                fileFolderList1.Browse(fileFolderPath_TB.Text);
            }
        }

        private void logout_BTN_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show(this, "로그아웃 완료.\n\n프로그램을 재실행하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2);

            if (confirmResult == DialogResult.Yes)
            {
                SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (로그아웃 완료 + 프로그램 재실행)" + empNo);
                Application.Restart();
            }
            else
            {
                SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (로그아웃 완료 + 프로그램 종료)" + empNo);
                Application.Exit();
            }
        }

        public static void SaveLog(string msg)
        {
            FileStream fo = null;
            StreamWriter sw = null;

            try
            {
                if (!Directory.Exists(Application.StartupPath + "\\log"))
                    Directory.CreateDirectory(Application.StartupPath + "\\log");

                fo = new FileStream(Application.StartupPath + "\\log\\ImageMgr_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append);
                sw = new StreamWriter(fo);

                sw.WriteLine(msg);

                sw.Close();
                fo.Close();
            }
            catch (Exception)
            {
                if (sw != null)
                    sw.Close();

                if (fo != null)
                    fo.Close();
            }
        }

        private void PI_ToolStripMenuItem4_SubItem1_Click(object sender, EventArgs e)
        {
            if (paperImgList1.SelectedItems[0].SubItems[IDX_PI_REAL_FILENAME].Text == "")
                MessageBox.Show(this, "파일이 존재하지 않습니다.", "확인", MessageBoxButtons.OK);
            else
                DownloadFile(paperImgList1.SelectedItems[0].SubItems[IDX_PI_REAL_FILENAME].Text);
        }

        private void PI_ToolStripMenuItem4_SubItem2_Click(object sender, EventArgs e)
        {
            if (paperImgList1.SelectedItems[0].SubItems[IDX_PI_PRE_FILENAME].Text == "")
                MessageBox.Show(this, "파일이 존재하지 않습니다.", "확인", MessageBoxButtons.OK);
            else
                DownloadFile(paperImgList1.SelectedItems[0].SubItems[IDX_PI_PRE_FILENAME].Text);
        }

        private void PI_ToolStripMenuItem4_SubItem3_Click(object sender, EventArgs e)
        {
            if (paperImgList1.SelectedItems[0].SubItems[IDX_PI_THUMB_FILENAME].Text == "")
                MessageBox.Show(this, "파일이 존재하지 않습니다.", "확인", MessageBoxButtons.OK);
            else
                DownloadFile(paperImgList1.SelectedItems[0].SubItems[IDX_PI_THUMB_FILENAME].Text);
        }

        private void DownloadFile(string fileName)
        {
            if (paperImgList1.SelectedItems[0].SubItems[IDX_PI_TYPE].Text == "소조")
            {
                SaveFileDialog saveFile = new SaveFileDialog();

                try
                {
                    fileName = null;

                    string strSQL = string.Format(@"select preview_file from  daps.dbo.CMS_SOJOINFO where sj_id = {0}", paperImgList1.SelectedItems[0].SubItems[IDX_PI_ID].Text);

                    using (SqlConnection db = new SqlConnection(""))
                    {
                        using (SqlCommand dbCmd = new SqlCommand(strSQL, db))
                        {
                            try
                            {
                                db.Open();

                                SqlDataReader reader = dbCmd.ExecuteReader();

                                while (reader.Read())
                                    fileName = reader["preview_file"].ToString().Trim();

                                reader.Close();
                                db.Close();
                            }
                            catch (Exception)
                            {
                                db.Close();
                            }
                        }
                    }

                    saveFile.FileName = fileName;
                    saveFile.InitialDirectory = downloadFolderPath;
                    saveFile.Filter = "Jpg Files (*.jpg, *.JPG)|*.jpg;*.JPG";

                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        string fullUrl = string.Format("{0}/{1}/{2}", ReadValue("PATTERNMGR", "소조URI"), paperImgList1.SelectedItems[0].SubItems[IDX_PI_MEDIA].Text, fileName);

                        using (WebClient webClient = new WebClient())
                        {
                            webClient.Proxy = null;
                            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted_SOJO);
                            webClient.QueryString.Add("file", fileName);
                            webClient.DownloadFileAsync(new Uri(fullUrl), saveFile.FileName);
                        }

                        MessageBox.Show(this, "파일 다운로드 완료.", "확인", MessageBoxButtons.OK);
                        SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (파일 다운로드 완료) " + saveFile.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "파일 다운로드에 실패했습니다. IT개발부로 문의 바랍니다.\n\n" + ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (파일 다운로드 실패) " + saveFile.FileName + "\n" + ex.ToString());
                }

                downloadFolderPath = Path.GetDirectoryName(saveFile.FileName);
            }
            else
            {
                downloadServerUrl = @"http://172.30.1.108:8080/";
                downloadServerUrl += paperImgList1.SelectedItems[0].SubItems[IDX_PI_MEDIA].Text + @"/";
                downloadServerUrl += fileName.Substring(2, 4) + @"/";
                downloadServerUrl += fileName.Substring(6, 4) + @"/";
                downloadServerUrl += fileName;

                saveFileDialog1.FileName = fileName;
                saveFileDialog1.InitialDirectory = downloadFolderPath;
                saveFileDialog1.ShowDialog();
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFileAsync(new Uri(downloadServerUrl), saveFileDialog1.FileName);
                }

                MessageBox.Show(this, "파일 다운로드 완료.", "확인", MessageBoxButtons.OK);
                SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (파일 다운로드 완료) " + saveFileDialog1.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "파일 다운로드에 실패했습니다. IT개발부로 문의 바랍니다.\n\n" + ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (파일 다운로드 실패) " + saveFileDialog1.FileName + "\n" + ex.ToString());
            }

            downloadFolderPath = Path.GetDirectoryName(saveFileDialog1.FileName);
        }

        private void folderBrowse_BTN_Click(object sender, EventArgs e)
        {
            // 열린 작업 폴더로 세팅
            if (File.Exists(fileFolderPath_TB.Text))
                folderBrowserDialog1.SelectedPath = Path.GetDirectoryName(fileFolderPath_TB.Text);
            else
                folderBrowserDialog1.SelectedPath = fileFolderPath_TB.Text;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                fileFolderPath_TB.Text = folderBrowserDialog1.SelectedPath;
                fileFolderListSearch();
            }
        }

        private void fileFolderPath_TB_TextChanged(object sender, EventArgs e)
        {
            // workFolderPath 재설정
            if (File.Exists(fileFolderPath_TB.Text))
                workFolderPath = Path.GetDirectoryName(fileFolderPath_TB.Text);
            else if (Directory.Exists(fileFolderPath_TB.Text))
                workFolderPath = fileFolderPath_TB.Text;

            // fileSystemWatcher1.Path 재설정
            fileSystemWatcher1.Path = workFolderPath;
        }

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            fileSystemWatcher1_Event(e);
        }

        private void fileSystemWatcher1_Deleted(object sender, FileSystemEventArgs e)
        {
            fileSystemWatcher1_Event(e);
        }

        private void fileSystemWatcher1_Renamed(object sender, RenamedEventArgs e)
        {
            fileSystemWatcher1_Event(e);
        }

        private void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
        {
            fileSystemWatcher1_Event(e);
        }

        private void fileSystemWatcher2_Changed(object sender, FileSystemEventArgs e)
        {
            fileSystemWatcher1_Event(e);
        }

        private void fileSystemWatcher2_Created(object sender, FileSystemEventArgs e)
        {
            fileSystemWatcher1_Event(e);
        }

        private void fileSystemWatcher2_Deleted(object sender, FileSystemEventArgs e)
        {
            fileSystemWatcher1_Event(e);
        }

        private void fileSystemWatcher2_Renamed(object sender, RenamedEventArgs e)
        {
            fileSystemWatcher1_Event(e);
        }

        private void fileSystemWatcher1_Event(FileSystemEventArgs e)
        {
            string tempFilePath = tempFolderPath + "\\" + e.Name.Replace(".eps", ".jpg").Replace(".EPS", ".jpg");

            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }

        private void FF_ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            //ConvertPdfForm convertPdfForm = new ConvertPdfForm(fileFolderPath_TB.Text, tempFilePath);
            //convertPdfForm.ShowDialog();

            if (pdfDistFilePath == "")
            {
                MessageBox.Show(this, "'PDF 변환 프로그램 (acrodist.exe)'을(를) 찾을 수 없습니다.\n\n* 경로 설정: 환경설정 > 폴더/파일 설정", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                ProcessStartInfo info = new ProcessStartInfo(pdfDistFilePath, "\"" + fileFolderPath_TB.Text + "\"");
                Process.Start(info);

                pdfFilePathList.Add(fileFolderPath_TB.Text.Replace(Path.GetExtension(fileFolderPath_TB.Text), ".pdf"));
            }
        }

        private void PI_ToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (paperImgList1.SelectedItems[0].SubItems[IDX_PI_TYPE].Text != "소조")
            {
                SJRegForm sjRegForm = new SJRegForm(paperImgList1.SelectedItems, pictureBox1.Image);
                sjRegForm.ShowDialog();
            }
            else
            {
                MessageBox.Show(this, "소조를 상주화상으로 등록할 수 없습니다.", "확인", MessageBoxButtons.OK);
            }
        }

        private void sj_BTN_Click(object sender, EventArgs e)
        {
            Form[] sjFormList = Application.OpenForms.Cast<Form>().Where(x => x.Name == "SJForm").ToArray();

            if (sjFormList.Length != 0) // 열려있으면 앞으로
            {
                sjFormList[0].TopMost = true;
                sjFormList[0].TopMost = false;
            }
            else
            {
                SJForm sjForm = new SJForm(fontDialog1, MENU_ToolStripMenuItem2_SubItem1.Checked ? IDX_THEME_BLUE : IDX_THEME_WHITE);
                sjForm.Show();
                sjForm.mainRefresh += new SJForm.mainRefreshDelegate(refresh);
            }
        }

        private void refresh_BTN_Click(object sender, EventArgs e)
        {
            refresh();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool bHandled = false;

            switch (keyData)
            {
                case Keys.F5:
                    refresh();
                    bHandled = true;
                    break;
            }

            return bHandled;
        }

        private void refresh()
        {
            // 전체 리스트 새로고침
            fileFolderList1.Sorting = System.Windows.Forms.SortOrder.None;
            for (int i = 0; i < fileFolderList1.Columns.Count; i++)
            {
                fileFolderList1.Columns[i].Text = fileFolderList1.Columns[i].Text.Replace("▲", "");
                fileFolderList1.Columns[i].Text = fileFolderList1.Columns[i].Text.Replace("▼", "");
            }
            fileFolderListSearch();

            paperImgList1.Sorting = System.Windows.Forms.SortOrder.None;
            for (int i = 0; i < paperImgList1.Columns.Count; i++)
            {
                paperImgList1.Columns[i].Text = paperImgList1.Columns[i].Text.Replace("▲", "");
                paperImgList1.Columns[i].Text = paperImgList1.Columns[i].Text.Replace("▼", "");
            }
            paperImgListSearch();

            // pictureBox 정리
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
                toolTip1.SetToolTip(pictureBox1, "");
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            ImgPreviewForm imgPreviewForm = new ImgPreviewForm(toolTip1.GetToolTip(pictureBox1), pictureBox1.Image);
            imgPreviewForm.Show();
        }

        private void FF_ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show(this, fileFolderPath_TB.Text + "\n\n삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2);

            if (confirmResult == DialogResult.Yes)
            {
                FileSystem.DeleteFile(fileFolderPath_TB.Text, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                MessageBox.Show(this, "파일 삭제 완료.", "확인", MessageBoxButtons.OK);

                fileFolderPath_TB.Text = Path.GetDirectoryName(fileFolderPath_TB.Text);
                fileFolderList1.SelectedPath = fileFolderPath_TB.Text;
                fileFolderListSearch();
            }
        }

        private void jibang_CB_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            jibang_CB.DroppedDown = true;
        }

        private void jibang_CB_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void media_CB_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            media_CB.DroppedDown = true;
        }

        private void media_CB_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void type_CB_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            type_CB.DroppedDown = true;
        }

        private void type_CB_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
