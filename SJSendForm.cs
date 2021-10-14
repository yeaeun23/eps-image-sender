using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using System.Text;
using System.Collections.Specialized;

namespace ImageMgr
{
    public partial class SJSendForm : Form
    {
        ListView.SelectedListViewItemCollection selectedItems;
        Image img = null;        

        public SJSendForm(ListView.SelectedListViewItemCollection selectedItems, Image img)
        {
            InitializeComponent();

            this.selectedItems = selectedItems;
            this.img = img;
        }

        private void SJSendForm_Load(object sender, EventArgs e)
        {
            title_TB.Text = selectedItems[0].SubItems[SJForm.IDX_SH_TITLE].Text;
            registrant_TB.Text = Form1.empName;
            pictureBox1.Image = img;                    
            paperDate_CAL.Value = DateTime.Now.AddDays(1);
            registerDate_TB.Text = DateTime.Now.ToString("yyyy-MM-dd dddd");

            Form1.setMediaCBList(media_CB);
            Form1.setJibangCBList(jibang_CB);
            Form1.setPanCBList(pan_CB);
            Form1.setMyunCBList(myun_CB);

            media_CB.SelectedIndex = 1;
            jibang_CB.SelectedIndex = 1;
            pan_CB.SelectedItem = "5";
            myun_CB.SelectedItem = "1";
        }

        private void ok_BTN_Click(object sender, EventArgs e)
        {
            int x;
            
            if (media_CB.SelectedIndex == 0)
            {
                MessageBox.Show(this, "매체를 선택해 주세요.", "확인", MessageBoxButtons.OK);
                media_CB.Focus();
            }
            else if (jibang_CB.SelectedIndex == 0)
            {
                MessageBox.Show(this, "지방을 선택해 주세요.", "확인", MessageBoxButtons.OK);
                jibang_CB.Focus();
            }
            else if (pan_CB.SelectedIndex == 0)
            {
                MessageBox.Show(this, "판을 선택해 주세요.", "확인", MessageBoxButtons.OK);
                pan_CB.Focus();
            }
            else if (!int.TryParse(pan_CB.Text.Replace("판", ""), out x))
            {
                MessageBox.Show(this, "판: 올바른 값이 아닙니다.", "확인", MessageBoxButtons.OK);
                pan_CB.Focus();
            }
            else if (myun_CB.SelectedIndex == 0)
            {
                MessageBox.Show(this, "면을 선택해 주세요.", "확인", MessageBoxButtons.OK);
                myun_CB.Focus();
            }
            else if (!int.TryParse(myun_CB.Text.Replace("면", ""), out x))
            {
                MessageBox.Show(this, "면: 올바른 값이 아닙니다.", "확인", MessageBoxButtons.OK);
                myun_CB.Focus();
            }
            else
            {
                regist();
            }
        }

        private void regist()
        {
            string strID = selectedItems[0].SubItems[SJForm.IDX_SH_ID].Text;
            string paperdate = paperDate_CAL.Value.ToString().Substring(0, 10).Replace("-", "");
            string _year = paperdate.Substring(0, 4);
            string _date = paperdate.Substring(4, 4);
            ReturnSP res = callSP("상주화상", "COPY", strID, media_CB.Text.Split('-')[1], paperdate, pan_CB.Text.Replace("판", ""), myun_CB.Text.Replace("면", ""), jibang_CB.Text.Split('-')[1], "개발부 테스트", Form1.empNo);

            if (Convert.ToInt32(res.newID) > 0)
            {
                string res1 = null;
                string res2 = null;

                using (WebClient client = new WebClient())
                {
                    byte[] response = client.UploadValues("http://172.30.1.103/seoulsmartnews/CopyImage.aspx", new NameValueCollection()
                    {
                        { "srcPath", _year + @"\" + _date + @"\" },
                        { "desFile", res.fileName1 },
                        { "srcFile", selectedItems[0].SubItems[SJForm.IDX_SH_PRE_FILENAME].Text.Replace("T.JPG", "R.EPS").Replace("t.jpg", ".eps") }
                    });

                    res1 = Encoding.UTF8.GetString(response);

                    if (res1.Substring(0, 1) != "S")
                    {
                        Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 요소화 실패 - 1/2) " + res1);
                        MessageBox.Show(this, "상주화상 요소화에 실패했습니다. IT개발부로 문의 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

                using (WebClient client = new WebClient())
                {
                    byte[] response = client.UploadValues("http://172.30.1.103/seoulsmartnews/CopyImage.aspx", new NameValueCollection()
                    {
                        { "srcPath", _year + @"\" + _date + @"\" },
                        { "desFile", res.fileName2 },
                        { "srcFile", selectedItems[0].SubItems[SJForm.IDX_SH_PRE_FILENAME].Text }
                    });

                    res2 = Encoding.UTF8.GetString(response);

                    if (res2.Substring(0, 1) != "S")
                    {
                        Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 요소화 실패 - 2/2) " + res2);
                        MessageBox.Show(this, "상주화상 요소화에 실패했습니다. IT개발부로 문의 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

                Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (상주화상 요소화 완료)");
                MessageBox.Show(this, "상주화상 요소화 완료.", "확인", MessageBoxButtons.OK);
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(this, "상주화상 요소화에 실패했습니다. IT개발부로 문의 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private ReturnSP callSP(string gubunName, string gubun, string ID, string media, string paperdate, string pan, string page, string jibangnum, string title, string empno)
        {
            string strSQL = "";

            using (SqlConnection db = new SqlConnection(""))
            {
                using (SqlCommand dbCmd = new SqlCommand(strSQL, db))
                {
                    try
                    {
                        db.Open();

                        dbCmd.CommandType = CommandType.StoredProcedure;
                        dbCmd.CommandText = "daps.dbo.sp_ObjectHandle";

                        dbCmd.Parameters.Add("@GubunName", SqlDbType.VarChar, 8);
                        dbCmd.Parameters.Add("@Gubun", SqlDbType.VarChar, 6);
                        dbCmd.Parameters.Add("@ID", SqlDbType.Int);
                        dbCmd.Parameters.Add("@Media", SqlDbType.SmallInt);
                        dbCmd.Parameters.Add("@PaperDate", SqlDbType.Char, 8);
                        dbCmd.Parameters.Add("@Pan", SqlDbType.TinyInt);
                        dbCmd.Parameters.Add("@Myun", SqlDbType.TinyInt);
                        dbCmd.Parameters.Add("@Jibang", SqlDbType.TinyInt);
                        dbCmd.Parameters.Add("@Title", SqlDbType.VarChar, 64);
                        dbCmd.Parameters.Add("@EmpNo", SqlDbType.Char, 7);

                        dbCmd.Parameters["@GubunName"].Value = gubunName;
                        dbCmd.Parameters["@Gubun"].Value = gubun;
                        dbCmd.Parameters["@ID"].Value = ID;
                        dbCmd.Parameters["@Media"].Value = media;
                        dbCmd.Parameters["@PaperDate"].Value = paperdate;
                        dbCmd.Parameters["@Pan"].Value = pan;
                        dbCmd.Parameters["@Myun"].Value = page;
                        dbCmd.Parameters["@Jibang"].Value = jibangnum;
                        dbCmd.Parameters["@Title"].Value = title;
                        dbCmd.Parameters["@EmpNo"].Value = empno;

                        SqlParameter outPutNewID = dbCmd.Parameters.Add("@NewID", SqlDbType.Int);
                        outPutNewID.Direction = ParameterDirection.Output;
                        SqlParameter outFileName1 = dbCmd.Parameters.Add("@FileName1", SqlDbType.VarChar, 64);
                        outFileName1.Direction = ParameterDirection.Output;
                        SqlParameter outFileName2 = dbCmd.Parameters.Add("@FileName2", SqlDbType.VarChar, 64);
                        outFileName2.Direction = ParameterDirection.Output;

                        dbCmd.ExecuteNonQuery();

                        return new ReturnSP(Convert.ToInt32(outPutNewID.Value), outFileName1.Value.ToString(), outFileName2.Value.ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return new ReturnSP(-2, null, null);
                    }
                    finally
                    {
                        db.Close();
                    }
                }
            }
        }

        private void cancel_BTN_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void myun_CB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ok_BTN.Focus();
                ok_BTN_Click(sender, e);
            }
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

        private void jibang_CB_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            jibang_CB.DroppedDown = true;
        }

        private void jibang_CB_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
