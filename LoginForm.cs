using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace ImageMgr
{   
    public partial class LoginForm : Form
    {
        DataTable dt;
        StringBuilder sb = new StringBuilder(255);
        
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // 버전 라벨 세팅
            Form1.GetPrivateProfileString("VERSION", "VER", "", sb, sb.Capacity, Form1.iniFilePath);
            toolStripStatusLabel1.Text = "Ver " + sb.ToString();

            // 아이디 세팅
            Form1.GetPrivateProfileString("LOGIN", "ID_SAVE", "", sb, sb.Capacity, Form1.iniFilePath);            
            if (sb.ToString() == "TRUE")
            {
                idSave_CHK.Checked = true;

                Form1.GetPrivateProfileString("LOGIN", "ID", "", sb, sb.Capacity, Form1.iniFilePath);
                id_TB.Text = sb.ToString();                
            }

            // 포커스
            if (id_TB.Text == "")
                ActiveControl = id_TB;
            else
                ActiveControl = pw_TB;
        }

        private void login_BTN_Click(object sender, EventArgs e)
        {
            if (id_TB.Text == "")
            {
                MessageBox.Show(this, "아이디를 입력해 주세요.", "확인", MessageBoxButtons.OK);
                ActiveControl = id_TB;
            }
            else if (pw_TB.Text == "")
            {
                MessageBox.Show(this, "비밀번호를 입력해 주세요.", "확인", MessageBoxButtons.OK);
                ActiveControl = pw_TB;
            }
            else
            {
                dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select * from [CMSCOM].[dbo].[T_USERINFO] where B_NOTUSE = 0 and V_USERID = '" + id_TB.Text + "'")), "SELECT");

                // 아이디 확인
                if (dt.Rows.Count == 1)
                {
                    // 비밀번호 확인
                    if (MD5Create(pw_TB.Text) == dt.Rows[0]["mpassword"].ToString().ToLower())
                    {
                        dt = Util.ExecuteQuery(new SqlCommand(string.Format(@"select * from [CMSCOM].[dbo].[R_USEREXE_PV] a inner join [CMSCOM].[dbo].[T_USERINFO] b on a.ID_USERCODE = b.ID_USERCODE where a.ID_SOFTCODE = 3 and b.B_NOTUSE = 0 and b.V_USERID = '" + id_TB.Text + "'")), "SELECT");

                        // 권한 확인
                        if (dt.Rows.Count == 1)
                        {
                            Form1.empNo = id_TB.Text;
                            Form1.empName = dt.Rows[0]["V_USERNAME"].ToString();
                            Form1.empCode = dt.Rows[0]["ID_USERCODE"].ToString();
                            Form1.empPubpart = dt.Rows[0]["ID_PUBPART"].ToString();

                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            MessageBox.Show(this, "로그인 권한이 없는 사용자입니다.", "확인", MessageBoxButtons.OK);
                        }                        
                    }
                    else
                    {
                        MessageBox.Show(this, "비밀번호를 확인해 주세요.", "확인", MessageBoxButtons.OK);
                        ActiveControl = pw_TB;
                    }
                }
                else
                {
                    MessageBox.Show(this, "아이디(사번 7자리)를 확인해 주세요.", "확인", MessageBoxButtons.OK);
                    ActiveControl = id_TB;
                }
            }
        }

        private void id_TB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
                login_BTN_Click(sender, e);
        }
        
        private void pw_TB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
                login_BTN_Click(sender, e);
        }

        private string MD5Create(string pw)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(pw);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                    sb.Append(hashBytes[i].ToString("X2"));

                return sb.ToString().ToLower();
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 아이디 저장 체크
            if (idSave_CHK.Checked)
            {
                Form1.WritePrivateProfileString("LOGIN", "ID", id_TB.Text, Form1.iniFilePath);
                Form1.WritePrivateProfileString("LOGIN", "ID_SAVE", "TRUE", Form1.iniFilePath);
            }
            else
            {
                Form1.WritePrivateProfileString("LOGIN", "ID", "", Form1.iniFilePath);
                Form1.WritePrivateProfileString("LOGIN", "ID_SAVE", "FALSE", Form1.iniFilePath);
            }
        }        
    }
}
