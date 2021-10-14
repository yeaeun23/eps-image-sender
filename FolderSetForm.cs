using System;
using System.IO;
using System.Windows.Forms;

namespace ImageMgr
{
    public partial class FolderSetForm : Form
    {
        public FolderSetForm()
        {
            InitializeComponent();
        }

        private void FolderSetForm_Load(object sender, EventArgs e)
        {
            workFolderPath_TB.Text = Form1.workFolderPath;
            downloadFolderPath_TB.Text = Form1.downloadFolderPath;
            tempFolderPath_TB.Text = Form1.tempFolderPath;
            pdfDistFilePath_TB.Text = Form1.pdfDistFilePath;

            toolTip1.SetToolTip(workFolderBrowse_BTN, "폴더 찾아보기");
            toolTip1.SetToolTip(downloadFolderBrowse_BTN, "폴더 찾아보기");
            toolTip1.SetToolTip(tempFolderBrowse_BTN, "폴더 찾아보기");
            toolTip1.SetToolTip(pdfDistFileBrowse_BTN, "파일 열기");
        }
        
        private void workFolderBrowse_BTN_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = workFolderPath_TB.Text;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                workFolderPath_TB.Text = folderBrowserDialog1.SelectedPath;
        }
        
        private void downloadFolderBrowse_BTN_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = downloadFolderPath_TB.Text;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                downloadFolderPath_TB.Text = folderBrowserDialog1.SelectedPath;
        }

        private void tempFolderBrowse_BTN_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = tempFolderPath_TB.Text;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                tempFolderPath_TB.Text = folderBrowserDialog1.SelectedPath;
        }

        private void pdfDistFileBrowse_BTN_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = pdfDistFilePath_TB.Text;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                pdfDistFilePath_TB.Text = openFileDialog1.FileName;
        }

        private void ok_BTN_Click(object sender, EventArgs e)
        {
            if (workFolderPath_TB.Text == "" || downloadFolderPath_TB.Text == "" || tempFolderPath_TB.Text == "")
            {
                MessageBox.Show(this, "폴더/파일 경로를 설정해 주세요.", "확인", MessageBoxButtons.OK);
            }
            else 
            {
                if (!Directory.Exists(workFolderPath_TB.Text))
                {
                    MessageBox.Show(this, "작업 폴더: '" + workFolderPath_TB.Text + "'을(를) 찾을 수 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    workFolderPath_TB.Focus();
                }
                else if (!Directory.Exists(downloadFolderPath_TB.Text))
                {
                    MessageBox.Show(this, "다운로드 폴더: '" + downloadFolderPath_TB.Text + "'을(를) 찾을 수 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    downloadFolderPath_TB.Focus();
                }
                else if (!Directory.Exists(tempFolderPath_TB.Text))
                {
                    MessageBox.Show(this, "EPS 파일 변환 폴더: '" + tempFolderPath_TB.Text + "'을(를) 찾을 수 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tempFolderPath_TB.Focus();
                }
                else if (pdfDistFilePath_TB.Text != "" && !File.Exists(pdfDistFilePath_TB.Text))
                {
                    MessageBox.Show(this, "PDF 변환 프로그램: '" + pdfDistFilePath_TB.Text + "'을(를) 찾을 수 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    pdfDistFilePath_TB.Focus();
                }
                else
                {
                    Form1.workFolderPath = workFolderPath_TB.Text;
                    Form1.downloadFolderPath = downloadFolderPath_TB.Text;
                    Form1.tempFolderPath = tempFolderPath_TB.Text;
                    Form1.pdfDistFilePath = pdfDistFilePath_TB.Text;

                    DialogResult = DialogResult.OK;
                }   
            }
        }

        private void cancel_BTN_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
