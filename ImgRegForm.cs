using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ImageMgr
{
    public partial class ImgRegForm : Form
    {
        ListView.SelectedListViewItemCollection selectedItems;
        string filePath;
        bool date_RB_Checked;
        DateTime paperDate_CAL_Value;
        int media_CB_SelectedIndex = 1;
        int jibang_CB_SelectedIndex = 1;
        string pan_CB_Text = "5";
        string myun_CB_Text = "1";
        int type_CB_SelectedIndex = 7;

        public ImgRegForm(ListView.SelectedListViewItemCollection selectedItems, string filePath, bool date_RB_Checked, DateTime paperDate_CAL_Value, int media_CB_SelectedIndex, int jibang_CB_SelectedIndex, string pan_CB_Text, string myun_CB_Text, int type_CB_SelectedIndex)
        {
            InitializeComponent();

            this.selectedItems = selectedItems;
            this.filePath = filePath;
            this.date_RB_Checked = date_RB_Checked;
            this.paperDate_CAL_Value = paperDate_CAL_Value;
            this.media_CB_SelectedIndex = media_CB_SelectedIndex;
            this.jibang_CB_SelectedIndex = jibang_CB_SelectedIndex;
            this.pan_CB_Text = pan_CB_Text;
            this.myun_CB_Text = myun_CB_Text;
            this.type_CB_SelectedIndex = type_CB_SelectedIndex;
        }

        public ImgRegForm(ListView.SelectedListViewItemCollection selectedItems, string filePath)
        {
            InitializeComponent();

            this.selectedItems = selectedItems;
            this.filePath = filePath;
        }

        private void ImgRegForm_Load(object sender, EventArgs e)
        {
            title_TB.Text = selectedItems[0].SubItems[Form1.IDX_FF_FILENAME].Text.Replace(selectedItems[0].SubItems[Form1.IDX_FF_EXT].Text, "");
            filePath_TB.Text = filePath;
            registrant_TB.Text = Form1.empName;

            if (selectedItems[0].SubItems[Form1.IDX_FF_EXT].Text.ToLower() == ".eps")
                pictureBox1.ImageLocation = Form1.tempFolderPath + "\\" + selectedItems[0].SubItems[Form1.IDX_FF_FILENAME].Text.Replace(".eps", ".jpg").Replace(".EPS", ".jpg");
            else
                pictureBox1.ImageLocation = filePath;

            if (date_RB_Checked)
                paperDate_CAL.Value = paperDate_CAL_Value;
            else
                paperDate_CAL.Value = DateTime.Now.AddDays(1);            

            registerDate_TB.Text = DateTime.Now.ToString("yyyy-MM-dd dddd");

            Form1.setMediaCBList(media_CB);
            Form1.setJibangCBList(jibang_CB);
            Form1.setPanCBList(pan_CB);
            Form1.setMyunCBList(myun_CB);            

            media_CB.SelectedIndex = media_CB_SelectedIndex == 0 ? 1 : media_CB_SelectedIndex;
            jibang_CB.SelectedIndex = jibang_CB_SelectedIndex == 0 ? 1 : jibang_CB_SelectedIndex;
            pan_CB.SelectedItem = pan_CB_Text.Replace("전체", "5");
            myun_CB.SelectedItem = myun_CB_Text.Replace("전체", "1");

            if (type_CB_SelectedIndex != 0)
                ((RadioButton)Controls.Find("type_RB" + type_CB_SelectedIndex, true).FirstOrDefault()).Checked = true;
            else if (Form1.empPubpart == "55")  // 광고
                type_RB4.Checked = true;
            else if (Form1.empPubpart == "9")   // 미술
                type_RB6.Checked = true;            
        }

        private void ok_BTN_Click(object sender, EventArgs e)
        {
            int x;

            if (title_TB.Text == "")
            {
                MessageBox.Show(this, "제목을 입력해 주세요.", "확인", MessageBoxButtons.OK);
                title_TB.Focus();
            }
            else if (!color_RB1.Checked && !color_RB2.Checked)
            {
                MessageBox.Show(this, "칼라를 선택해 주세요. (흑백 자동 변환 X)", "확인", MessageBoxButtons.OK);
            }
            else if (media_CB.SelectedIndex == 0)
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
            else if (!type_RB1.Checked && !type_RB2.Checked && !type_RB3.Checked && !type_RB4.Checked && !type_RB5.Checked && !type_RB6.Checked && !type_RB7.Checked)
            {
                MessageBox.Show(this, "타입을 선택해 주세요.", "확인", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    regist();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void regist()
        {
            string sql1 = "";
            string sql2 = "";
            string serverUrl = "";  // ex) ctssvr1.seoul.co.kr/ImageMgr/filesave.aspx?media=65&year=2019&month=04&day=19&desFileName=AI2019041905990102010107.eps

            // sql1: [Prodarx2005].[dbo].[ADIMGINFO_TBL]
            string adimg_id = ImgIDSet();
            if (adimg_id == null)
                return;
            string media = media_CB.Text.Split('-')[1];
            string month = Convert.ToInt64(paperDate_CAL.Value.ToString().Substring(5, 2)).ToString();
            string day = Convert.ToInt64(paperDate_CAL.Value.ToString().Substring(8, 2)).ToString();
            string pan = pan_CB.Text.Replace("판", "");
            string myun = myun_CB.Text.Replace("면", "");
            string jibang = jibang_CB.Text.Split('-')[1];
            string img_type = "0";
            string reg_month = Convert.ToInt64(registerDate_TB.Text.Substring(5, 2)).ToString();
            string reg_day = Convert.ToInt64(registerDate_TB.Text.Substring(8, 2)).ToString();
            string title = title_TB.Text;
            string ad_host = registrant_TB.Text;
            string input_type = link_CHK.Checked ? "19" : "16";
            string color = color_RB1.Checked ? "1" : "0";
            string b_thumb = "1";
            string b_print = "0";
            string b_real = "1";
            string b_deliv = "1";
            string dummy2 = "NULL";
            string thumb_name = "AI" + paperDate_CAL.Value.ToString().Substring(0, 10).Replace("-", "") + pan.PadLeft(2, '0') + myun.PadLeft(2, '0') + jibang.PadLeft(2, '0') + adimg_id.PadLeft(8, '0') + "T.jpg";
            string print_name = thumb_name.Replace("T.jpg", "P.jpg");
            string real_name = thumb_name.Replace("T.jpg", ".eps");
            Image img = Image.FromFile(pictureBox1.ImageLocation);
            string x_pixel = img.Size.Width.ToString();
            string y_pixel = img.Size.Height.ToString();
            string real_hsize = string.Format("{0:f2}", (double.Parse(x_pixel) * 2.54 / img.HorizontalResolution)).Replace(".", "");
            string real_vsize = string.Format("{0:f2}", (double.Parse(y_pixel) * 2.54 / img.VerticalResolution)).Replace(".", "");
            string x_resolution = Convert.ToInt64(img.HorizontalResolution).ToString();
            string y_resolution = Convert.ToInt64(img.VerticalResolution).ToString();
            string xy_units = "1";
            string onoff_c = "0";
            string onoff_m = "0";
            string onoff_y = "0";
            string onoff_k = "0";
            string format = "0";
            string hochul_stat = "0";
            string hochul_id = "0";
            string org_img_id = adimg_id;
            string save_count = "1";
            string ref_id = adimg_id;
            string reg_pan = pan;
            string paper_date = paperDate_CAL.Value.ToString().Substring(0, 10).Replace("-", "");
            string regist_date = registerDate_TB.Text.Substring(0, 10).Replace("-", "");
                      
            if (type_RB1.Checked)
                img_type = "3";
            else if (type_RB2.Checked)
                img_type = "5";
            else if (type_RB3.Checked)
                img_type = "6";
            else if (type_RB4.Checked)
                img_type = "7";
            else if (type_RB5.Checked)
                img_type = "11";
            else if (type_RB6.Checked)
                img_type = "13";

            sql1 = "insert into [Prodarx2005].[dbo].[ADIMGINFO_TBL] values (" + adimg_id + ", " + media + ", " + month + ", " + day + ", " + pan + ", " + myun + ", " + jibang + ", " + img_type + ", " + reg_month + ", " + reg_day + ", '" + title + "', '" + ad_host + "', " + input_type + ", " + color + ", " + b_thumb + ", " + b_print + ", " + b_real + ", " + b_deliv + ", " + dummy2 + ", '" + thumb_name + "', '" + print_name + "', '" + real_name + "', " + real_hsize + ", " + real_vsize + ", " + x_pixel + ", " + y_pixel + ", " + x_resolution + ", " + y_resolution + ", " + xy_units + ", " + onoff_c + ", " + onoff_m + ", " + onoff_y + ", " + onoff_k + ", " + format + ", " + hochul_stat + ", " + hochul_id + ", " + org_img_id + ", " + save_count + ", " + ref_id + ", " + reg_pan + ", " + paper_date + ", " + regist_date + ")";

            // sql2: [Prodarx2005].[dbo].[IMG_REF_TBL] 
            string ref_count = "1";
            string cre_user = Form1.empCode;
            string thumb_sz = "0";
            string real_sz = "0";
            string print_sz = "0";
            string kisasvr_id = "NULL";
            string is_backup = "0";

            sql2 = "insert into [Prodarx2005].[dbo].[IMG_REF_TBL] values (" + adimg_id + ", '" + real_name + "', '" + thumb_name + "', '" + print_name + "', " + save_count + ", " + ref_count + ", " + img_type + ", " + input_type + ", " + color + ", " + onoff_c + ", '" + onoff_m + "', '" + onoff_y + "', " + onoff_k + ", getdate(), " + cre_user + ", " + format + ", " + b_thumb + ", " + thumb_sz + ", '" + ad_host + "', '" + x_pixel + "', '" + y_pixel + "', '" + x_resolution + "', " + y_resolution + ", " + xy_units + ", " + b_real + ", " + real_sz + ", " + print_sz + ", " + month + ", " + day + ", " + media + ", " + real_hsize + ", " + real_vsize + ", " + kisasvr_id + ", " + is_backup + ", " + paper_date + ")";

            // serverUrl
            serverUrl = @"http://ctssvr1.seoul.co.kr/ImageMgr/filesave.aspx?";
            serverUrl += "media=" + media_CB.Text.Split('-')[1] + "&";
            serverUrl += "year=" + paperDate_CAL.Value.ToString().Substring(0, 4) + "&";
            serverUrl += "month=" + paperDate_CAL.Value.ToString().Substring(5, 2) + "&";
            serverUrl += "day=" + paperDate_CAL.Value.ToString().Substring(8, 2) + "&desFileName=";

            FileUploadForm fileUploadForm = new FileUploadForm(adimg_id, sql1, sql2, serverUrl, real_name, print_name, thumb_name, filePath_TB.Text);
            if (fileUploadForm.ShowDialog() == DialogResult.OK)
            {
                DialogResult = DialogResult.OK;
            }
        }

        public static string ImgIDSet()
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
                        dbCmd.Parameters["@nTBLKindID"].Value = 2;
                        
                        dbCmd.ExecuteNonQuery();

                        return dbCmd.Parameters["@nGenID"].Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("파일 등록에 실패했습니다. IT개발부로 문의 바랍니다.\n\n" + ex.ToString(), "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Form1.SaveLog("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] (파일 등록 실패 - SP) " + ex.ToString());
                        return null;
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

        private void ImgRegForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }
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
