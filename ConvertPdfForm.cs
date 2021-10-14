using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace ImageMgr
{
    public partial class ConvertPdfForm : Form
    {        
        string realFilePath;
        string tempFilePath;
        DateTime startTime1;
        bool success = true;
        Process p = new Process();

        public ConvertPdfForm(string realFilePath, string tempFilePath)
        {
            InitializeComponent();

            this.realFilePath = realFilePath;
            this.tempFilePath = tempFilePath.Replace(".jpg", ".pdf");            
            startTime1 = DateTime.Now;            
        }        

        private void ConvertPdfForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();

            backgroundWorker1.RunWorkerAsync();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (label1.Text.Contains("..."))
                label1.Text = "PDF 변환 진행중 .";
            else
                label1.Text += ".";
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label2.Text = new TimeSpan(DateTime.Now.Ticks - startTime1.Ticks).ToString(@"hh\:mm\:ss\.ff");
        }

        private void cancel_BTN_Click(object sender, EventArgs e)
        {
            p.Kill();
            p.Dispose();

            Close();
        }

        private void open_BTN_Click(object sender, EventArgs e)
        {
            Process.Start(tempFilePath);
        }

        private void ok_BTN_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ConvertEpsToPdf(object sender, DoWorkEventArgs e)
        {
            if (!File.Exists(Form1.gsFilePath))
            {
                MessageBox.Show(this, "'" + Form1.gsFilePath + "'을(를) 찾을 수 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            ProcessStartInfo psi = new ProcessStartInfo { FileName = Form1.gsFilePath };
            try
            {
                psi.CreateNoWindow = false;
                psi.UseShellExecute = true;
                psi.Arguments = "-dQUIET -dBATCH -dNOPAUSE -sDEVICE=pdfwrite -dEPSFitPage -sOutputFile=" + (char)34 + tempFilePath + (char)34 + " " + (char)34 + realFilePath + (char)34;

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
                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (파일 변환 실패 - EPS → PDF)\n" + ex.ToString());
            }
            finally
            {
                p.Dispose();
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cancel_BTN.Enabled = false;
            ok_BTN.Enabled = true;
            ok_BTN.Focus();

            timer1.Stop();
            timer1.Dispose();
            timer2.Stop();
            timer2.Dispose();

            if (success)
            {
                label1.Text = "PDF 변환 완료";
                open_BTN.Enabled = true;
            }
            else
            {
                label1.Text = "PDF 변환 실패";
                label1.ForeColor = Color.Red;   
            }
        }
    }
}
