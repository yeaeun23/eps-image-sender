using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ImageMgr
{
    public partial class FileUploadForm : Form
    {
        DataTable dt;
        StringBuilder sb = new StringBuilder(255);
        
        string adimg_id = "";
        string sql1 = "";
        string sql2 = "";
        string serverUrl = "";
        string real_name = "";
        string preview_name = "";
        string thumb_name = "";
        string srcFilePath = "";
          
        public FileUploadForm(string adimg_id, string sql1, string sql2, string serverUrl, string real_name, string preview_name, string thumb_name, string srcFilePath)
        {
            this.adimg_id = adimg_id;
            this.sql1 = sql1;
            this.sql2 = sql2;
            this.serverUrl = serverUrl;
            this.real_name = real_name;
            this.preview_name = preview_name;
            this.thumb_name = thumb_name;
            this.srcFilePath = srcFilePath;

            InitializeComponent();   
        }

        private void FileUploadForm_Load(object sender, EventArgs e)
        {
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";
            label10.Text = "";
            label11.Text = "";
            progressBar1.Value = 0;

            Form1.GetPrivateProfileString("UPLOAD_FORM", "AUTO_CLOSE", "", sb, sb.Capacity, Form1.iniFilePath);
            if (sb.ToString() == "TRUE")
                autoClose_CHK.Checked = true;

            ActiveControl = ok_BTN;

            backgroundWorker1.RunWorkerAsync();
        }

        private void FileUploadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (autoClose_CHK.Checked)
                Form1.WritePrivateProfileString("UPLOAD_FORM", "AUTO_CLOSE", "TRUE", Form1.iniFilePath);
            else
                Form1.WritePrivateProfileString("UPLOAD_FORM", "AUTO_CLOSE", "FALSE", Form1.iniFilePath);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Thread.Sleep(1000);
                dt = Util.ExecuteQuery(new SqlCommand(sql1), "INSERT");
                progressBar1.PerformStep();
                label6.Text = "Completed";
                label11.Text = "(1 / 5) 파일 등록 진행중 ...";
            }
            catch (Exception ex)
            {
                label6.Text = "Error";
                label6.ForeColor = Color.Red;
                label11.Text = "(1 / 5) 파일 등록 실패";
                ok_BTN.Enabled = true;
                MessageBox.Show(this, "파일 등록에 실패했습니다. IT개발부로 문의 바랍니다.\n\n" + ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (파일 등록 실패 - 1/5) " + sql1 + "\n" + ex.ToString());
                return;
            }

            try
            {
                //Thread.Sleep(1000);
                dt = Util.ExecuteQuery(new SqlCommand(sql2), "INSERT");
                progressBar1.PerformStep();
                label7.Text = "Completed";
                label11.Text = "(2 / 5) 파일 등록 진행중 ...";
            }
            catch (Exception ex)
            {
                label7.Text = "Error";
                label7.ForeColor = Color.Red;
                label11.Text = "(2 / 5) 파일 등록 실패";
                ok_BTN.Enabled = true;
                MessageBox.Show(this, "파일 등록에 실패했습니다. IT개발부로 문의 바랍니다.\n\n" + ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (파일 등록 실패 - 2/5) " + sql2 + "\n" + ex.ToString());
                deleteSQL1();
                return;
            }
            
            try
            {
                //Thread.Sleep(1000);
                FileUpload(serverUrl + real_name, srcFilePath);                
                progressBar1.PerformStep();
                label8.Text = "Completed";
                label11.Text = "(3 / 5) 파일 등록 진행중 ...";
            }
            catch (Exception ex)
            {
                label8.Text = "Error";
                label8.ForeColor = Color.Red;
                label11.Text = "(3 / 5) 파일 등록 실패";
                ok_BTN.Enabled = true;
                MessageBox.Show(this, "파일 등록에 실패했습니다. IT개발부로 문의 바랍니다.\n\n" + ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (파일 등록 실패 - 3/5) " + serverUrl + real_name + "\n" + ex.ToString());
                deleteSQL1();
                return;
            }

            try
            {
                //Thread.Sleep(1000);
                FileUpload(serverUrl + preview_name, Form1.tempFolderPath + "\\" + Path.GetFileName(srcFilePath).Replace(Path.GetExtension(srcFilePath), ".jpg"));
                progressBar1.PerformStep();
                label9.Text = "Completed";
                label11.Text = "(4 / 5) 파일 등록 진행중 ...";                
            }
            catch (Exception ex)
            {
                label9.Text = "Error";
                label9.ForeColor = Color.Red;
                label11.Text = "(4 / 5) 파일 등록 실패";
                ok_BTN.Enabled = true;
                MessageBox.Show(this, "파일 등록에 실패했습니다. IT개발부로 문의 바랍니다.\n\n" + ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (파일 등록 실패 - 4/5) " + serverUrl + preview_name + "\n" + ex.ToString());
                deleteSQL1();
                return;
            }

            try
            {
                //Thread.Sleep(1000);
                FileUpload(serverUrl + thumb_name, Form1.tempFolderPath + "\\" + Path.GetFileName(srcFilePath).Replace(Path.GetExtension(srcFilePath), ".jpg"));                
                progressBar1.PerformStep();
                label10.Text = "Completed";
                label11.Text = "(5 / 5) 파일 등록 완료";
                ok_BTN.Enabled = true;
                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (파일 등록 완료) adimg_id = " + adimg_id + ", cre_user = " + Form1.empCode + ", real_name = " + real_name + ", preview_name = " + preview_name + ", thumb_name = " + thumb_name);

                // 자동으로 창 닫기
                if (autoClose_CHK.Checked)
                {
                    Thread.Sleep(800);
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                label10.Text = "Error";
                label10.ForeColor = Color.Red;
                label11.Text = "(5 / 5) 파일 등록 실패";
                ok_BTN.Enabled = true;
                MessageBox.Show(this, "파일 등록에 실패했습니다. IT개발부로 문의 바랍니다.\n\n" + ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (파일 등록 실패 - 5/5) " + serverUrl + thumb_name + "\n" + ex.ToString());
                deleteSQL1();
                return;
            }
        }

        private void FileUpload(string serverUrl, string srcFilePath)
        {
            string result = "F";

            using (WebClient wc = new WebClient())
            {
                byte[] resultArray = wc.UploadFile(serverUrl, srcFilePath);
                result = ((Char)resultArray[0]).ToString();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (result != "S")
                return;
        }

        private void ok_BTN_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }        

        private void deleteSQL1()
        {
            try
            {
                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"delete from [Prodarx2005].[dbo].[ADIMGINFO_TBL] where adimg_id = {0}", adimg_id)), "DELETE");
                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (파일 등록 롤백 완료) " + adimg_id);
            }
            catch (Exception ex)
            {
                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (파일 등록 롤백 실패) adimg_id = " + adimg_id + "\n" + ex.ToString());
            }            
        }
    }
}
