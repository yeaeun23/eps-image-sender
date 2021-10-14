using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace ImageMgr
{
    public partial class SJForm : Form
    {
        DataTable dt;
        StringBuilder sb = new StringBuilder(255);

        public static ArrayList selectedArrSH = new ArrayList();        // 아이디
        public static ArrayList selectedArrSHTitle = new ArrayList();   // 제목 

        int imgNumPerPage = 12;                                         // 페이지 당 이미지 개수
        int themeColor = 0;
        FontDialog fontDialog;

        public static int IDX_SH_TITLE = 0;
        public static int IDX_SH_COLOR = 1;
        public static int IDX_SH_REGDATE = 2;
        public static int IDX_SH_SIZE = 3;
        public static int IDX_SH_PRE_FILENAME = 4;
        public static int IDX_SH_ID = 5;

        public delegate void mainRefreshDelegate();
        public event mainRefreshDelegate mainRefresh;

        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        public SJForm(FontDialog fontDialog, int themeColor)
        {
            InitializeComponent();

            this.themeColor = themeColor;
            this.fontDialog = fontDialog;            
        }

        private void SJForm_Load(object sender, EventArgs e)
        {            
            // 환경설정 세팅
            Form1.GetPrivateProfileString("SPLITTER", "DISTANCE_SJ", "", sb, sb.Capacity, Form1.iniFilePath);
            if (sb.ToString() != "")
                splitContainer2.SplitterDistance = Convert.ToInt32(sb.ToString());

            listViewSH.Font = fontDialog.Font;
            listViewSH.ForeColor = fontDialog.Color;

            if (themeColor == Form1.IDX_THEME_BLUE)
                themeColorSet(Form1.themeBlue);
            else if (themeColor == Form1.IDX_THEME_WHITE)
                themeColorSet(Form1.themeWhite);

            toolTip1.SetToolTip(lvSH_nextPageBtn, "다음 페이지");
            toolTip1.SetToolTip(lvSH_lastPageBtn, "마지막 페이지");
            toolTip1.SetToolTip(lvSH_prevPageBtn, "이전 페이지");
            toolTip1.SetToolTip(lvSH_firstPageBtn, "처음 페이지");

            // 컬럼 헤더 크기 세팅
            for (int i = 0; i < listViewSH.Columns.Count; i++)
            {
                Form1.GetPrivateProfileString("LV_COLUMN_WIDTH", "LV3_" + i, "", sb, sb.Capacity, Form1.iniFilePath);
                if (sb.ToString() != "")
                    listViewSH.Columns[i].Width = Convert.ToInt32(sb.ToString());
            }

            // 검색
            sangju_Search();
        }

        private void SJForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 환경설정 저장
            Form1.WritePrivateProfileString("SPLITTER", "DISTANCE_SJ", splitContainer2.SplitterDistance.ToString(), Form1.iniFilePath);

            // 컬럼 헤더 크기 저장
            for (int i = 0; i < listViewSH.Columns.Count; i++)
                Form1.WritePrivateProfileString("LV_COLUMN_WIDTH", "LV3_" + i, listViewSH.Columns[i].Width.ToString(), Form1.iniFilePath);
        }

        private void themeColorSet(Color[] color)
        {
            splitContainer2.Panel1.BackColor = color[0];
            splitContainer2.Panel2.BackColor = color[1];
            BackColor = color[2];
        }        
        
        private void sangju_Search()
        {
            string totalImgNum = "";    // 전체 이미지 개수
            string resultImgNum = "";   // 검색 이미지 개수            

            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.Abort();
                backgroundWorker1.Dispose();
            }

            // 전체 이미지 개수 구하기
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select count(*) from
(select img_id 
from [Prodarx2005].[dbo].[CLIPIMG_TBL]
where bunryu2 != 2 and media = 65)
as num")), "SELECT");

            totalImgNum = dt.Rows[0].ItemArray[0].ToString();
            
            // 검색 이미지 개수 구하기
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select count(*) from 
(select img_id 
from [Prodarx2005].[dbo].[CLIPIMG_TBL]
where title like '%{0}%' and bunryu2 != 2 and media = 65)
as num", tb2_title.Text.Trim())), "SELECT");

            resultImgNum = dt.Rows[0].ItemArray[0].ToString();

            // 라벨 값 세팅
            lvSH_totalCntLB.Text = "결과: " + resultImgNum + " / " + totalImgNum + " (검색 이미지 수 / 전체 이미지 수)";
            lvSH_totalPageLB.Text = string.Format("/ {0}", Convert.ToInt32(Convert.ToDouble(resultImgNum) / imgNumPerPage + 0.5));
            lvSH_presentPageLB.Text = "1";

            // 페이지 불러오기
            ContentsViewSH(PageViewSH());
        }

        // 쿼리 스트링 생성
        private string PageViewSH()
        {
            string strSQL;
            string strData = "";
            SqlConnection db = null;

            strSQL = string.Format(@"select top {0} img_id, title, 
case color when 1 then '○' else '' end as color, 
convert(varchar, cre_date, 23) cre_date, 
thumb_file, 
case when len(real_hsz) < 3 then STUFF(REPLICATE('0', 3 - len(real_hsz)) + CONVERT(varchar, real_hsz), 2, 0, '.') 
else STUFF(real_hsz, len(real_hsz) - 1, 0, '.') end as real_hsz, 
case when len(real_vsz) < 3 then STUFF(REPLICATE('0', 3 - len(real_vsz)) + CONVERT(varchar, real_vsz), 2, 0, '.') 
else STUFF(real_vsz, len(real_vsz) - 1, 0, '.') end as real_vsz 
from [Prodarx2005].[dbo].[CLIPIMG_TBL] 
where title like '%{1}%' and bunryu2 != 2 and media = 65 
and img_id not in (select top {2} img_id 
from [Prodarx2005].[dbo].[CLIPIMG_TBL] 
where title like '%{1}%' and bunryu2 != 2 and media = 65 ", imgNumPerPage, tb2_title.Text.Trim(), imgNumPerPage * (Convert.ToInt32(lvSH_presentPageLB.Text) - 1)).Replace("\r\n", "");

            if (sort_CB.Text == "등록순▼")
                strSQL += "order by img_id desc) order by img_id desc";
            else if (sort_CB.Text == "등록순▲")
                strSQL += "order by img_id asc) order by img_id asc";
            else if (sort_CB.Text == "제목순▼")
                strSQL += "order by title desc) order by title desc";
            else if (sort_CB.Text == "제목순▲")
                strSQL += "order by title asc) order by title asc";

            try
            {
                Cursor = Cursors.WaitCursor;

                listViewSH.Items.Clear();
                imageList2.Images.Clear();
                pictureBox1.Image = null;
                toolTip1.SetToolTip(pictureBox1, "");

                db = new SqlConnection("");
                SqlCommand dbCmd = new SqlCommand(strSQL, db);
                db.Open();

                SqlDataReader reader = dbCmd.ExecuteReader();

                while (reader.Read())
                {
                    strData += reader["img_id"].ToString().Trim() + "∥"
                              + reader["title"].ToString().Trim() + "∥"
                              + reader["color"].ToString().Trim() + "∥"
                              + reader["cre_date"].ToString().Trim() + "∥"
                              + reader["thumb_file"].ToString().Trim() + "∥"
                              + reader["real_hsz"].ToString().Trim() + "∥"
                              + reader["real_vsz"].ToString().Trim() + "|";
                }

                Cursor = Cursors.Default;

                reader.Close();
                db.Close();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                db.Close();
                MessageBox.Show(this, ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return strData;
        }

        // 리스트뷰 값 세팅
        private void ContentsViewSH(string str)
        {
            string[] article = str.Split(new char[] { '|' });
            string[] columns = null;
            ListViewItem item = null;

            try
            {
                listViewSH.BeginUpdate();

                for (int i = 0; i < article.Length - 1; i++)
                {
                    columns = article[i].Split(new char[] { '∥' });

                    // 0: 아이디, 1: 제목, 2: 칼라, 3: 등록일, 4: 파일명, 5: 크기
                    item = new ListViewItem(columns[1]);
                    item.SubItems.Add(columns[2]);
                    item.SubItems.Add(columns[3]);
                    item.SubItems.Add(columns[5] + " × " + columns[6]);
                    item.SubItems.Add(columns[4]);
                    item.SubItems.Add(columns[0]);

                    item.ToolTipText = "제목: " + item.SubItems[0].Text + "\n등록일: " + item.SubItems[2].Text + "\n크기: " + item.SubItems[3].Text;

                    listViewSH.Items.Add(item);
                }

                listViewSH.EndUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            lvSH_viewType_Check();
        }

        // 리스트뷰 보기 타입에 따른 설정
        private void lvSH_viewType_Check()
        {
            Cursor = Cursors.WaitCursor;

            if (listViewSH_RB1.Checked)         // 타입 1(자세히)일 경우
            {
                listViewSH.View = View.Details;

                for (int i = 0; i < listViewSH.Items.Count; i++)
                    listViewSH.Items[i].Text = listViewSH.Items[i].ToolTipText.Split('\n')[0].Substring(4);
            }
            else if (listViewSH_RB2.Checked)    // 타입 2(간단히)일 경우
            {
                listViewSH.LargeImageList = imageList2;
                listViewSH.View = View.LargeIcon;

                try
                {
                    backgroundWorker1.RunWorkerAsync(); // 이미지 하나씩 로드   
                }
                catch (Exception)
                {
                }

                for (int i = 0; i < listViewSH.Items.Count; i++)
                {
                    if (listViewSH.Items[i].Text.Length > 10)
                        listViewSH.Items[i].Text = listViewSH.Items[i].ToolTipText.Split('\n')[0].Substring(4, 10) + "...";
                    else
                        listViewSH.Items[i].Text = listViewSH.Items[i].ToolTipText.Split('\n')[0].Substring(4);

                    listViewSH.Items[i].Text += "\n" + listViewSH.Items[i].SubItems[IDX_SH_REGDATE].Text.Substring(2) + " | " + listViewSH.Items[i].SubItems[IDX_SH_SIZE].Text.Replace(" ", "").Replace("cm", "");
                }
            }

            Cursor = Cursors.Default;
        }

        // 이미지 하나씩 추가
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            for (int i = 0; i < listViewSH.Items.Count; i++)
            {
                imageList2.Images.Add(ViewWebImage(string.Format("{0}/{1}", Form1.ReadValue("CONTENTMGR", "상주화상이미지URI"), listViewSH.Items[i].SubItems[IDX_SH_PRE_FILENAME].Text)));

                listViewSH.Items[i].ImageIndex = i;
            }
        }

        // 웹 이미지 다운로드
        private Bitmap ViewWebImage(string url)
        {
            try
            {
                WebClient Downloader = new WebClient();
                Stream ImageStream = Downloader.OpenRead(url);
                Bitmap DownloadImage = Bitmap.FromStream(ImageStream) as Bitmap;

                return setThumbSize(DownloadImage);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // 이미지 사이즈 조정
        private Bitmap setThumbSize(Bitmap DownloadImage)
        {
            int tw, th, tx, ty;
            int w = DownloadImage.Width;
            int h = DownloadImage.Height;
            double whRatio = (double)w / h;

            if (DownloadImage.Width >= DownloadImage.Height)
            {
                tw = imageList2.ImageSize.Width;
                th = (int)(tw / whRatio);
            }
            else
            {
                th = imageList2.ImageSize.Height;
                tw = (int)(th * whRatio);
            }

            tx = (imageList2.ImageSize.Width - tw) / 2;
            ty = (imageList2.ImageSize.Height - th) / 2;

            Bitmap thumb = new Bitmap(imageList2.ImageSize.Width, imageList2.ImageSize.Height, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(thumb);

            g.Clear(Color.White);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(DownloadImage, new Rectangle(tx, ty, tw, th), new Rectangle(0, 0, w, h), GraphicsUnit.Pixel);

            return thumb;
        }

        private void listViewSH_Click(object sender, EventArgs e)
        {
            if (listViewSH.SelectedItems.Count == 0)
            {
                //MessageBox.Show(this, "선택된 요소가 없습니다. 요소를 선택 후 작업 바랍니다.", "확인", MessageBoxButtons.OK);
                return;
            }

            try
            {
                // 파일명을 파싱하여 파일 경로를 구함
                string fileName = listViewSH.FocusedItem.SubItems[4].Text;

                if (fileName.Substring(0, 2) == "SJ") // ns5이미지
                    fileName = fileName.Replace("T.jpg", "P.jpg");

                string fullUrl = string.Format("{0}/{1}", Form1.ReadValue("CONTENTMGR", "상주화상이미지URI"), fileName);

                using (WebClient webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted_SJIMAGE);
                    webClient.QueryString.Add("file", fileName); // here you can add values
                    webClient.DownloadFileAsync(new Uri(fullUrl), Form1.ReadValue("CONTENTMGR", "통합요소다운로드경로") + "\\" + fileName);
                    Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 파일 로드)" + fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void listViewSH_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 229)
                return;

            if (listViewSH.SelectedItems.Count == 0)
            {
                //MessageBox.Show(this, "선택된 요소가 없습니다. 요소를 선택 후 작업 바랍니다.", "확인", MessageBoxButtons.OK);
                return;
            }

            try
            {
                // 파일명을 파싱하여 파일 경로를 구함
                string fileName = listViewSH.FocusedItem.SubItems[4].Text;

                if (fileName.Substring(0, 2) == "SJ")//ns5이미지
                    fileName = fileName.Replace("T.jpg", "P.jpg");

                string strYear = fileName.Substring(2, 4);
                string strDate = fileName.Substring(6, 4);
                string fullUrl = String.Format("{0}/{1}", Form1.ReadValue("CONTENTMGR", "상주화상이미지URI"), fileName);

                using (WebClient webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted_SJIMAGE);
                    webClient.QueryString.Add("file", fileName);
                    webClient.DownloadFileAsync(new Uri(fullUrl), Form1.ReadValue("CONTENTMGR", "통합요소다운로드경로") + "\\" + fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void DownloadCompleted_SJIMAGE(object sender, AsyncCompletedEventArgs e)
        {
            string fileName = ((WebClient)(sender)).QueryString["file"];

            if (e.Error != null)
            {
                MessageBox.Show(this, "파일 다운로드에 실패했습니다. IT개발부로 문의 바랍니다." + e.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 파일 로드 실패) " + fileName);
            }                

            pictureBox1.ImageLocation = Form1.ReadValue("CONTENTMGR", "통합요소다운로드경로") + "\\" + fileName;
            toolTip1.SetToolTip(pictureBox1, fileName);
        }

        // 리스트 보기 타입1 (자세히)
        private void listViewSH_RB1_Click(object sender, EventArgs e)
        {
            if (listViewSH.View == View.LargeIcon)
                lvSH_viewType_Check();
        }

        // 리스트 보기 타입2 (간단히)
        private void listViewSH_RB2_Click(object sender, EventArgs e)
        {
            if (listViewSH.View == View.Details)
                lvSH_viewType_Check();
        }

        private void tb2_title_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                sangju_Search();
        }

        // 페이지 처음(<<) 버튼 클릭
        private void lvSH_firstPageBtn_Click(object sender, EventArgs e)
        {
            if (lvSH_totalPageLB.Text.Replace("/ ", "") == "0")
                return;

            lvSH_presentPageLB.Text = string.Format("{0}", 1);

            ContentsViewSH(PageViewSH());
        }

        // 페이지 이전(<) 버튼 클릭
        private void lvSH_prevPageBtn_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lvSH_presentPageLB.Text) == 1 || lvSH_totalPageLB.Text.Replace("/ ", "") == "0")
                return;

            lvSH_presentPageLB.Text = string.Format("{0}", Convert.ToInt32(lvSH_presentPageLB.Text) - 1);

            ContentsViewSH(PageViewSH());
        }

        // 페이지 다음(>) 버튼 클릭
        private void lvSH_nextPageBtn_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lvSH_presentPageLB.Text) == Convert.ToInt32(lvSH_totalPageLB.Text.Replace("/ ", "")) || lvSH_totalPageLB.Text.Replace("/ ", "") == "0")
                return;

            lvSH_presentPageLB.Text = string.Format("{0}", Convert.ToInt32(lvSH_presentPageLB.Text) + 1);

            ContentsViewSH(PageViewSH());
        }

        // 페이지 마지막(>>) 버튼 클릭
        private void lvSH_lastPageBtn_Click(object sender, EventArgs e)
        {
            if (lvSH_totalPageLB.Text.Replace("/ ", "") == "0")
                return;

            lvSH_presentPageLB.Text = string.Format("{0}", lvSH_totalPageLB.Text.Replace("/ ", ""));

            ContentsViewSH(PageViewSH());
        }

        // 페이지 입력 후 엔터
        private void lvSH_presentPageLB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (Convert.ToInt32(lvSH_presentPageLB.Text) < 1 || Convert.ToInt32(lvSH_presentPageLB.Text) > Convert.ToInt32(lvSH_totalPageLB.Text.Replace("/ ", "")) || lvSH_totalPageLB.Text.Replace("/ ", "") == "0")
                        return;
                }
                catch (Exception)
                {
                    return;
                }

                ContentsViewSH(PageViewSH());
            }
        }        

        // Preview
        private void SJ_ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listViewSH.SelectedItems.Count != 1)
            {
                //MessageBox.Show(this, "선택된 요소가 없습니다. 요소를 선택 후 작업 바랍니다.", "확인", MessageBoxButtons.OK);
                return;
            }
            else
            {
                ImgPreviewForm imgPreviewForm = new ImgPreviewForm(listViewSH.SelectedItems[0].SubItems[IDX_SH_PRE_FILENAME].Text, pictureBox1.Image);
                imgPreviewForm.Show();
            }
        }

        // 파일 다운로드 - Original
        private void SJ_ToolStripMenuItem2_SubItem1_Click(object sender, EventArgs e)
        {
            if (listViewSH.SelectedItems.Count != 1)
            {
                //MessageBox.Show(this, "선택된 요소가 없습니다. 요소를 선택 후 작업 바랍니다.", "확인", MessageBoxButtons.OK);
                return;
            }

            string fileName = listViewSH.FocusedItem.SubItems[4].Text.Replace("T.JPG", "R.EPS").Replace("T.jpg", "R.EPS"); ;

            // 원본 파일이 존재하는지 체크
            if (fileName.Substring(0, 2) == "SJ") // ns5이미지
            {
                string fullUrl0 = string.Format("{0}/{1}", Form1.ReadValue("CONTENTMGR", "상주화상이미지URI"), fileName);

                fileName = fileName.Replace("T.jpg", "R.EPS");

                WebRequest request = WebRequest.Create(new Uri(fullUrl0));

                request.Method = "HEAD";

                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        if (response.ContentLength == 0)
                        {
                            MessageBox.Show(this, "EPS 파일이 존재하지 않습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(this, "EPS 파일이 존재하지 않습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            SaveFileDialog saveFile = new SaveFileDialog();

            saveFile.InitialDirectory = Form1.downloadFolderPath;
            saveFile.Title = "저장";
            saveFile.DefaultExt = "EPS";
            saveFile.FileName = fileName;
            saveFile.Filter = "Eps Files (*.eps, *.EPS)|*.eps;*.EPS";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 파일명을 파싱하여 파일 경로를 구함
                    if (fileName.Substring(0, 2) == "SJ")//ns5이미지
                        fileName = fileName.Replace("T.jpg", "R.EPS");

                    string strYear = fileName.Substring(2, 4);
                    string strDate = fileName.Substring(6, 4);
                    string fullUrl = string.Format("{0}/{1}", Form1.ReadValue("CONTENTMGR", "상주화상이미지URI"), fileName);

                    using (WebClient webClient = new WebClient())
                    {
                        webClient.Proxy = null;
                        webClient.QueryString.Add("file", fileName); // here you can add values
                        webClient.DownloadFileAsync(new Uri(fullUrl), saveFile.FileName);
                    }

                    MessageBox.Show(this, "파일 다운로드 완료.", "확인", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                Form1.downloadFolderPath = Path.GetDirectoryName(saveFile.FileName);
            }
        }

        // 파일 다운로드 - Preview
        private void SJ_ToolStripMenuItem2_SubItem2_Click(object sender, EventArgs e)
        {
            if (listViewSH.SelectedItems.Count != 1)
            {
                //MessageBox.Show(this, "선택된 요소가 없습니다. 요소를 선택 후 작업 바랍니다.", "확인", MessageBoxButtons.OK);
                return;
            }

            string fileName = listViewSH.FocusedItem.SubItems[4].Text;
            SaveFileDialog saveFile = new SaveFileDialog();

            saveFile.InitialDirectory = Form1.downloadFolderPath;
            saveFile.Title = "저장";
            saveFile.DefaultExt = "jpg";
            saveFile.FileName = fileName;
            saveFile.Filter = "Jpg Files (*.jpg, *.JPG)|*.jpg;*.JPG";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 파일명을 파싱하여 파일 경로를 구함
                    if (fileName.Substring(0, 2) == "SJ")//ns5이미지
                        fileName = fileName.Replace("T.jpg", "P.jpg");

                    string strYear = fileName.Substring(2, 4);
                    string strDate = fileName.Substring(6, 4);
                    string fullUrl = string.Format("{0}/{1}", Form1.ReadValue("CONTENTMGR", "상주화상이미지URI"), fileName);

                    using (WebClient webClient = new WebClient())
                    {
                        webClient.Proxy = null;
                        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted_SJIMAGE);
                        webClient.QueryString.Add("file", fileName); // here you can add values
                        webClient.DownloadFileAsync(new Uri(fullUrl), saveFile.FileName);
                    }

                    MessageBox.Show(this, "파일 다운로드 완료.", "확인", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                Form1.downloadFolderPath = Path.GetDirectoryName(saveFile.FileName);
            }
        }

        // 파일 수정
        private void SJ_ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            // 선택된 요소가 1개가 아닐 경우
            if (listViewSH.SelectedItems.Count != 1)
            {
                //MessageBox.Show(this, "1개의 상주화상을 선택해 주세요.", "확인", MessageBoxButtons.OK);
                return;
            }
            else
            {
                SJEditForm img_sj_edit = null;

                selectedArrSH.Clear();
                selectedArrSHTitle.Clear();

                foreach (ListViewItem item in listViewSH.Items)
                {
                    if (item.Selected)
                    {
                        selectedArrSH.Add(item.SubItems[IDX_SH_ID].Text);
                        selectedArrSHTitle.Add(item.ToolTipText.Split('\n')[0].Substring(4));
                    }
                }

                img_sj_edit = new SJEditForm(pictureBox1.Image);

                if (img_sj_edit.ShowDialog() == DialogResult.OK)
                    ContentsViewSH(PageViewSH());
            }
        }

        // 파일 삭제
        private void SJ_ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (listViewSH.SelectedItems.Count == 0)
            {
                //MessageBox.Show(this, "선택된 요소가 없습니다. 요소를 선택 후 작업 바랍니다.", "확인", MessageBoxButtons.OK);
                return;
            }
            else
            {
                selectedArrSH.Clear();
                selectedArrSHTitle.Clear();

                foreach (ListViewItem item in listViewSH.Items)
                {
                    if (item.Selected)
                    {
                        selectedArrSH.Add(item.SubItems[IDX_SH_ID].Text);
                        selectedArrSHTitle.Add(item.ToolTipText.Split('\n')[0].Substring(4));
                    }
                }

                string strSelectedArrSHTitle = "";

                foreach (string title in selectedArrSHTitle)
                {
                    strSelectedArrSHTitle += "'" + title + "'\n";
                }

                var confirmResult = MessageBox.Show(this, strSelectedArrSHTitle + "\n삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2);

                if (confirmResult == DialogResult.Yes)
                {
                    foreach (string id in selectedArrSH)
                    {
                        try
                        {
                            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"delete from [Prodarx2005].[dbo].[CLIPIMG_TBL]
where img_id = {0}", id)), "DELETE");

                            Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 삭제 완료)");
                        }
                        catch (Exception ex)
                        {
                            Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 삭제 실패) " + ex.ToString());
                            MessageBox.Show(this, "상주화상 삭제에 실패했습니다. IT개발부로 문의 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }

                    MessageBox.Show(this, "상주화상 삭제 완료.", "확인", MessageBoxButtons.OK);
                    ContentsViewSH(PageViewSH());
                }
            }
        }

        private void listViewSH_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) // 오른쪽 클릭일 경우
            {
                foreach (ListViewItem item in listViewSH.Items)
                {
                    if (item.Bounds.Contains(e.Location))
                    {
                        contextMenuStrip1.Show(Cursor.Position);
                        break;
                    }                        
                }
            }
        }        

        private void sort_CB_TextChanged(object sender, EventArgs e)
        {
            sangju_Search();
        }

        private void SJ_ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (listViewSH.SelectedItems.Count != 1)
            {
                //MessageBox.Show(this, "선택된 요소가 없습니다. 요소를 선택 후 작업 바랍니다.", "확인", MessageBoxButtons.OK);
                return;
            }

            SJSendForm sjSendForm = new SJSendForm(listViewSH.SelectedItems, pictureBox1.Image);
            if (sjSendForm.ShowDialog() == DialogResult.OK)
                mainRefresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ImgPreviewForm imgPreviewForm = new ImgPreviewForm(toolTip1.GetToolTip(pictureBox1), pictureBox1.Image);
            imgPreviewForm.Show();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            ImgPreviewForm imgPreviewForm = new ImgPreviewForm(toolTip1.GetToolTip(pictureBox1), pictureBox1.Image);
            imgPreviewForm.Show();
        }

        private void sort_CB_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            sort_CB.DroppedDown = true;
        }

        private void sort_CB_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
