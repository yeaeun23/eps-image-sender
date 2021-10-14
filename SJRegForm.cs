using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace ImageMgr
{
    public partial class SJRegForm : Form
    {
        DataTable dt;

        ListView.SelectedListViewItemCollection selectedItems;
        Image img = null;

        public SJRegForm(ListView.SelectedListViewItemCollection selectedItems, Image img)
        {
            InitializeComponent();

            this.selectedItems = selectedItems;
            this.img = img;
        }

        private void SJRegForm_Load(object sender, EventArgs e)
        {
            title_TB.Text = selectedItems[0].SubItems[Form1.IDX_PI_TITLE].Text;
            registerDate_TB.Text = DateTime.Now.ToString("yyyy-MM-dd dddd");
            registrant_TB.Text = Form1.empName;
            pictureBox1.Image = img;
        }

        private void cancel_BTN_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void ok_BTN_Click(object sender, EventArgs e)
        {
            string strSJID = "";            // 상주 아이디

            if (title_TB.Text.Trim() == "")
            {
                MessageBox.Show(this, "제목을 입력해 주세요.", "확인", MessageBoxButtons.OK);
                return;
            }
            else if (checkSameTitle())
            {
                MessageBox.Show(this, "동일한 제목이 존재합니다.", "확인", MessageBoxButtons.OK);
                return;
            }

            try
            {
                // 상주 화상 테이블의 img_id 값 구하기(SP 실행)
                strSJID = getStrID();

                // 화상 테이블에서 상주 화상 테이블에 insert
                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"insert into [Prodarx2005].[dbo].[CLIPIMG_TBL] select {0}, '{1}', img_type, input_type, color, onoff_c, onoff_m, onoff_y, onoff_k, hochul_stat, hochul_id, uuid, getdate(), {2}, format, b_thumb, thumb_sz, dummy1, thumb_hsz, thumb_vsz, '{3}', ad_host, x_pixel, y_pixel, x_resolution, y_resolution, xy_units, b_real, real_sz, real_hsz, real_vsz, '{4}', 0, 0, '{5}', 65, DATEPART(year, getdate()), DATEPART(month, getdate()), DATEPART(day, getdate()), NULL from [DAPS].[dbo].[CMS_IMGINFO] where img_id = {6}", strSJID, title_TB.Text.Trim(), Form1.empCode, getStrFileName(strSJID, 'T'), getStrFileName(strSJID, 'R'), Form1.empName, selectedItems[0].SubItems[Form1.IDX_PI_ID].Text)), "INSERT");
            }
            catch (Exception ex)
            {
                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 등록 실패 - insert) " + ex.ToString());
                MessageBox.Show(this, "상주화상 등록에 실패했습니다. IT개발부로 문의 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // 파일 복사
            copySJFile(strSJID);
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

        private string getStrID()
        {
            using (SqlConnection db = new SqlConnection(@""))
            {
                using (SqlCommand dbCmd = new SqlCommand("sp_SEQ_GETID", db))
                {
                    try
                    {
                        db.Open();

                        dbCmd.CommandType = CommandType.StoredProcedure;

                        dbCmd.Parameters.Add("@nGenID", SqlDbType.Decimal, 18);
                        dbCmd.Parameters.Add("@nTBLKindID", SqlDbType.Int);

                        dbCmd.Parameters["@nGenID"].Direction = ParameterDirection.Output;
                        dbCmd.Parameters["@nTBLKindID"].Value = 5;

                        dbCmd.ExecuteNonQuery();

                        return dbCmd.Parameters["@nGenID"].Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "상주화상 등록에 실패했습니다. IT개발부로 문의 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 등록 실패 - SP) " + ex.ToString());
                        return null;
                    }
                    finally
                    {
                        db.Close();
                    }
                }
            }
        }

        private string getStrFileName(string strSJID, char column)
        {
            if (column == 't' || column == 'T')
                return "SJ65" + string.Format("{0:D9}", Convert.ToInt32(strSJID)) + "T.JPG";
            else    // column == 'r' || column == 'R'
                return "SJ65" + string.Format("{0:D9}", Convert.ToInt32(strSJID)) + "R.EPS";
        }

        private void copySJFile(string strSJID)
        {
            string url = "";    // ex) 172.30.1.103/seoulsmartnews/CopySJImage.aspx?srcPath=2018\0101\&srcFile=test.JPG&desFile=test2.JPG
            string urlJPG = "";
            string urlEPS = "";
            string result = "";

            try
            {
                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select thumb_file, real_file
from [DAPS].[dbo].[CMS_IMGINFO]
where img_id = {0}
union
select thumb_file, real_file
from [Prodarx2005].[dbo].[CLIPIMG_TBL]
where img_id = {1}", selectedItems[0].SubItems[Form1.IDX_PI_ID].Text, strSJID)), "SELECT");
            }
            catch (Exception ex)
            {
                deleteDB(strSJID, ex);
                return;
            }

            url = "http://172.30.1.103/seoulsmartnews/CopySJImage.aspx?srcPath=";

            url += dt.Rows[0].ItemArray[0].ToString().Substring(2, 4) + @"\";
            url += dt.Rows[0].ItemArray[0].ToString().Substring(6, 2) + dt.Rows[0].ItemArray[0].ToString().Substring(8, 2) + @"\";

            urlJPG = url + "&srcFile=" + dt.Rows[0].ItemArray[0].ToString() + "&desFile=" + dt.Rows[1].ItemArray[0].ToString();
            urlEPS = url + "&srcFile=" + dt.Rows[0].ItemArray[1].ToString() + "&desFile=" + dt.Rows[1].ItemArray[1].ToString();

            using (WebClient client = new WebClient())
            {
                result = client.DownloadString(urlJPG) + client.DownloadString(urlEPS);
            }

            if (result == "SS")
            {
                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 등록 완료)");
                MessageBox.Show(this, "상주화상 등록 완료.", "확인", MessageBoxButtons.OK);

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                deleteDB(strSJID, result);
                return;
            }
        }

        private void deleteDB(string strSJID, Exception ex)
        {
            Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 등록 실패 - WS) " + ex.ToString());
            MessageBox.Show(this, "상주화상 등록에 실패했습니다. IT개발부로 문의 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            try
            {
                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"delete from [Prodarx2005].[dbo].[CLIPIMG_TBL]
where img_id = {0}", strSJID)), "DELETE");

                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 등록 insert 되돌리기 완료)");
            }
            catch (Exception ex1)
            {
                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 등록 insert 되돌리기 실패) " + ex1.ToString());
            }

            return;
        }

        private void deleteDB(string strSJID, string ex)
        {
            Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 등록 실패 - WS) " + ex.ToString());
            MessageBox.Show(this, "상주화상 등록에 실패했습니다. IT개발부로 문의 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            try
            {
                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"delete from [Prodarx2005].[dbo].[CLIPIMG_TBL]
where img_id = {0}", strSJID)), "DELETE");

                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 등록 insert 되돌리기 완료)");
            }
            catch (Exception ex1)
            {
                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 등록 insert 되돌리기 실패) " + ex1.ToString());
            }

            return;
        }

        private void title_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ok_BTN.Focus();
                ok_BTN_Click(sender, e);
            }                
        }
    }
}
