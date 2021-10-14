using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ImageMgr
{
    public partial class SJEditForm : Form
    {
        DataTable dt;

        string strID = SJForm.selectedArrSH[0].ToString();           // 아이디
        string strTitle = SJForm.selectedArrSHTitle[0].ToString();   // 제목
        Image img = null;

        public SJEditForm(Image img)
        {
            InitializeComponent();

            this.img = img;
        }

        private void SJEditForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = strTitle;
            title_TB.Text = strTitle;
            pictureBox1.Image = img;

            ActiveControl = title_TB;
        }
        
        private void title_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ok_BTN.Focus();
                ok_BTN_Click(sender, e);
            }                
        }

        private void ok_BTN_Click(object sender, EventArgs e)
        {
            if (title_TB.Text.Trim() == "")
            {
                MessageBox.Show(this, "새 제목을 입력해 주세요.", "확인", MessageBoxButtons.OK);
                return;
            }
            else if (checkSameTitle())
            {
                MessageBox.Show(this, "동일한 제목이 존재합니다.", "확인", MessageBoxButtons.OK);
                return;
            }

            try
            {
                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"update [Prodarx2005].[dbo].[CLIPIMG_TBL]
set title = '{0}'
where img_id = {1}", title_TB.Text, strID)), "UPDATE");
            }
            catch (Exception ex)
            {
                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 수정 실패) " + ex.ToString());
                MessageBox.Show(this, "상주화상 수정에 실패했습니다. IT개발부로 문의 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 수정 완료)");
            MessageBox.Show(this, "상주화상 수정 완료.", "확인", MessageBoxButtons.OK);

            DialogResult = DialogResult.OK;
            Close();
        }

        private bool checkSameTitle()
        {
            dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select title from [Prodarx2005].[dbo].[CLIPIMG_TBL] order by img_id desc")), "SELECT");

            foreach (DataRow dr in dt.Rows)
            {
                if (title_TB.Text.Trim() == dr[0].ToString().Trim())
                    return true;
            }

            return false;
        }
        
        private void cancel_BTN_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
