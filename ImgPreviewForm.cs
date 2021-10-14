using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageMgr
{
    public partial class ImgPreviewForm : Form
    {
        string fileName = "";
        Image img = null;
        int imgOriginWidth = 0;
        int imgOriginHeight = 0;
        
        bool isDragging = false;
        int currentX;
        int currentY;   
        int imgMargin = 30;            

        public ImgPreviewForm(string fileName, Image img)
        {
            InitializeComponent();

            this.fileName = fileName;
            this.img = img;

            imgOriginWidth = img.Width;
            imgOriginHeight = img.Height;
        }

        private void ImgPreviewForm_Load(object sender, EventArgs e)
        {
            // Form 초기 크기 세팅
            ClientSize = new Size(img.Width + imgMargin * 2, img.Height + toolStrip1.Height + statusStrip1.Height + imgMargin * 2);

            // 메뉴 렌더링 디버그 해결
            toolStrip1.Renderer = new NewToolStripRenderer();
            statusStrip1.Renderer = new NewToolStripRenderer();

            // 상태 표시줄 세팅
            toolStripStatusLabel1.Text = "Info: " + img.Width + " × " + img.Height + " px, " + img.HorizontalResolution + " dpi";
            toolStripStatusLabel2.Text = "Pos: " + imgMargin + ", " + imgMargin;

            // Form 타이틀(파일명) 세팅
            Text = fileName;            

            // 이미지 로드
            pictureBox1.Image = img;
            pictureBox1.Location = new Point(imgMargin, imgMargin);
            
            // 배경색 이미지 세팅
            bgColorImgSet();

            // 활성 컨트롤 세팅
            ActiveControl = pictureBox1;
        }

        private void ImgPreviewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (pictureBox1.Image != null)
            //{
            //    pictureBox1.Image.Dispose();
            //    pictureBox1.Image = null;
            //}
        }
        
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                panel1.BackColor = colorDialog1.Color;
                bgColorImgSet();
            }
        }

        private void bgColorImgSet()
        {
            Bitmap bgColorImg = new Bitmap(toolStripButton1.Image.Size.Width, toolStripButton1.Image.Size.Height);
            Graphics g = Graphics.FromImage(bgColorImg);

            g.Clear(BackColor);
            toolStripButton1.Image = bgColorImg;
        }        
        
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging) // 드래그 중이면
            {
                if (pictureBox1.Top + (e.Y - currentY) >= imgMargin - pictureBox1.Height && pictureBox1.Top + (e.Y - currentY) <= panel1.Height - imgMargin && pictureBox1.Left + (e.X - currentX) >= imgMargin - pictureBox1.Width && pictureBox1.Left + (e.X - currentX) <= panel1.Width - imgMargin) // 드래그 가능 범위 안이면
                {
                    pictureBox1.Top = pictureBox1.Top + (e.Y - currentY);
                    pictureBox1.Left = pictureBox1.Left + (e.X - currentX);
                }
            }

            toolStripStatusLabel2.Text = "Pos: " + pictureBox1.Top + ", " + pictureBox1.Left;
        }        

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;

            currentX = e.X;
            currentY = e.Y;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void ImgPreviewForm_SizeChanged(object sender, EventArgs e)
        {
            //if (panel1.Height <= img.Height)   // 세로 스크롤 발생 시
            //    pictureBox1.Location = new Point(pictureBox1.Location.X, 0);
            //else
            //    pictureBox1.Location = new Point(pictureBox1.Location.X, (panel1.Height - pictureBox1.Height) / 2);

            //if (panel1.Width <= img.Width)     // 가로 스크롤 발생 시
            //    pictureBox1.Location = new Point(0, pictureBox1.Location.Y);
            //else
            //    pictureBox1.Location = new Point((panel1.Width - pictureBox1.Width) / 2, pictureBox1.Location.Y);
        }
    }
}
