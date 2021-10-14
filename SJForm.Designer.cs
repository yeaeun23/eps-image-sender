namespace ImageMgr
{
    partial class SJForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SJForm));
            this.sort_CB = new System.Windows.Forms.ComboBox();
            this.listViewSH_RB2 = new System.Windows.Forms.RadioButton();
            this.lvSH_presentPageLB = new System.Windows.Forms.TextBox();
            this.lvSH_prevPageBtn = new System.Windows.Forms.Button();
            this.listViewSH_RB1 = new System.Windows.Forms.RadioButton();
            this.lvSH_totalPageLB = new System.Windows.Forms.Label();
            this.lvSH_firstPageBtn = new System.Windows.Forms.Button();
            this.lvSH_nextPageBtn = new System.Windows.Forms.Button();
            this.listViewSH = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.lvSH_totalCntLB = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lvSH_lastPageBtn = new System.Windows.Forms.Button();
            this.tb2_title = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SJ_ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.SJ_ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.SJ_ToolStripMenuItem2_SubItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.SJ_ToolStripMenuItem2_SubItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.SJ_ToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.SJ_ToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.SJ_ToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundWorker1 = new totalmgr.AbortableBackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // sort_CB
            // 
            this.sort_CB.FormattingEnabled = true;
            this.sort_CB.Items.AddRange(new object[] {
            "등록순▼",
            "등록순▲",
            "제목순▼",
            "제목순▲"});
            this.sort_CB.Location = new System.Drawing.Point(10, 10);
            this.sort_CB.Name = "sort_CB";
            this.sort_CB.Size = new System.Drawing.Size(70, 20);
            this.sort_CB.TabIndex = 10;
            this.sort_CB.Text = "등록순▼";
            this.sort_CB.TextChanged += new System.EventHandler(this.sort_CB_TextChanged);
            this.sort_CB.Click += new System.EventHandler(this.sort_CB_Click);
            this.sort_CB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.sort_CB_KeyPress);
            // 
            // listViewSH_RB2
            // 
            this.listViewSH_RB2.AutoSize = true;
            this.listViewSH_RB2.Location = new System.Drawing.Point(567, 12);
            this.listViewSH_RB2.Name = "listViewSH_RB2";
            this.listViewSH_RB2.Size = new System.Drawing.Size(59, 16);
            this.listViewSH_RB2.TabIndex = 21;
            this.listViewSH_RB2.Text = "간단히";
            this.listViewSH_RB2.UseVisualStyleBackColor = true;
            this.listViewSH_RB2.Click += new System.EventHandler(this.listViewSH_RB2_Click);
            // 
            // lvSH_presentPageLB
            // 
            this.lvSH_presentPageLB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lvSH_presentPageLB.Location = new System.Drawing.Point(704, 10);
            this.lvSH_presentPageLB.Name = "lvSH_presentPageLB";
            this.lvSH_presentPageLB.Size = new System.Drawing.Size(30, 21);
            this.lvSH_presentPageLB.TabIndex = 35;
            this.lvSH_presentPageLB.Text = "0";
            this.lvSH_presentPageLB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lvSH_presentPageLB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvSH_presentPageLB_KeyUp);
            // 
            // lvSH_prevPageBtn
            // 
            this.lvSH_prevPageBtn.BackColor = System.Drawing.Color.White;
            this.lvSH_prevPageBtn.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lvSH_prevPageBtn.Location = new System.Drawing.Point(668, 10);
            this.lvSH_prevPageBtn.Name = "lvSH_prevPageBtn";
            this.lvSH_prevPageBtn.Size = new System.Drawing.Size(30, 21);
            this.lvSH_prevPageBtn.TabIndex = 30;
            this.lvSH_prevPageBtn.Text = "<";
            this.lvSH_prevPageBtn.UseVisualStyleBackColor = false;
            this.lvSH_prevPageBtn.Click += new System.EventHandler(this.lvSH_prevPageBtn_Click);
            // 
            // listViewSH_RB1
            // 
            this.listViewSH_RB1.AutoSize = true;
            this.listViewSH_RB1.Checked = true;
            this.listViewSH_RB1.Location = new System.Drawing.Point(502, 12);
            this.listViewSH_RB1.Name = "listViewSH_RB1";
            this.listViewSH_RB1.Size = new System.Drawing.Size(59, 16);
            this.listViewSH_RB1.TabIndex = 20;
            this.listViewSH_RB1.TabStop = true;
            this.listViewSH_RB1.Text = "자세히";
            this.listViewSH_RB1.UseVisualStyleBackColor = true;
            this.listViewSH_RB1.Click += new System.EventHandler(this.listViewSH_RB1_Click);
            // 
            // lvSH_totalPageLB
            // 
            this.lvSH_totalPageLB.AutoSize = true;
            this.lvSH_totalPageLB.Location = new System.Drawing.Point(740, 14);
            this.lvSH_totalPageLB.Name = "lvSH_totalPageLB";
            this.lvSH_totalPageLB.Size = new System.Drawing.Size(21, 12);
            this.lvSH_totalPageLB.TabIndex = 57;
            this.lvSH_totalPageLB.Text = "/ 0";
            // 
            // lvSH_firstPageBtn
            // 
            this.lvSH_firstPageBtn.BackColor = System.Drawing.Color.White;
            this.lvSH_firstPageBtn.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lvSH_firstPageBtn.Location = new System.Drawing.Point(632, 10);
            this.lvSH_firstPageBtn.Name = "lvSH_firstPageBtn";
            this.lvSH_firstPageBtn.Size = new System.Drawing.Size(30, 21);
            this.lvSH_firstPageBtn.TabIndex = 25;
            this.lvSH_firstPageBtn.Text = "<<";
            this.lvSH_firstPageBtn.UseVisualStyleBackColor = false;
            this.lvSH_firstPageBtn.Click += new System.EventHandler(this.lvSH_firstPageBtn_Click);
            // 
            // lvSH_nextPageBtn
            // 
            this.lvSH_nextPageBtn.BackColor = System.Drawing.Color.White;
            this.lvSH_nextPageBtn.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lvSH_nextPageBtn.Location = new System.Drawing.Point(779, 10);
            this.lvSH_nextPageBtn.Name = "lvSH_nextPageBtn";
            this.lvSH_nextPageBtn.Size = new System.Drawing.Size(30, 21);
            this.lvSH_nextPageBtn.TabIndex = 40;
            this.lvSH_nextPageBtn.Text = ">";
            this.lvSH_nextPageBtn.UseVisualStyleBackColor = false;
            this.lvSH_nextPageBtn.Click += new System.EventHandler(this.lvSH_nextPageBtn_Click);
            // 
            // listViewSH
            // 
            this.listViewSH.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewSH.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader9,
            this.columnHeader12,
            this.columnHeader4,
            this.columnHeader13,
            this.columnHeader6});
            this.listViewSH.ForeColor = System.Drawing.SystemColors.WindowText;
            this.listViewSH.FullRowSelect = true;
            this.listViewSH.GridLines = true;
            this.listViewSH.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewSH.HideSelection = false;
            this.listViewSH.Location = new System.Drawing.Point(10, 40);
            this.listViewSH.Margin = new System.Windows.Forms.Padding(3, 3, 3, 100);
            this.listViewSH.Name = "listViewSH";
            this.listViewSH.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.listViewSH.ShowItemToolTips = true;
            this.listViewSH.Size = new System.Drawing.Size(835, 335);
            this.listViewSH.SmallImageList = this.imageList1;
            this.listViewSH.TabIndex = 51;
            this.listViewSH.TabStop = false;
            this.listViewSH.UseCompatibleStateImageBehavior = false;
            this.listViewSH.View = System.Windows.Forms.View.Details;
            this.listViewSH.Click += new System.EventHandler(this.listViewSH_Click);
            this.listViewSH.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listViewSH_KeyUp);
            this.listViewSH.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewSH_MouseClick);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "제목";
            this.columnHeader5.Width = 300;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "C/L";
            this.columnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "등록일";
            this.columnHeader12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader12.Width = 120;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "크기(cm)";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 120;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "파일명(P)";
            this.columnHeader13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader13.Width = 150;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "ID";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(1, 20);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(738, 382);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 12);
            this.label7.TabIndex = 35;
            this.label7.Text = "페이지 당 개수: 12";
            // 
            // lvSH_totalCntLB
            // 
            this.lvSH_totalCntLB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lvSH_totalCntLB.AutoSize = true;
            this.lvSH_totalCntLB.Location = new System.Drawing.Point(10, 382);
            this.lvSH_totalCntLB.Name = "lvSH_totalCntLB";
            this.lvSH_totalCntLB.Size = new System.Drawing.Size(251, 12);
            this.lvSH_totalCntLB.TabIndex = 53;
            this.lvSH_totalCntLB.Text = "결과: 0 / 0 (검색 이미지 수 / 전체 이미지 수)";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(10, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(835, 240);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.DoubleClick += new System.EventHandler(this.pictureBox1_DoubleClick);
            // 
            // lvSH_lastPageBtn
            // 
            this.lvSH_lastPageBtn.BackColor = System.Drawing.Color.White;
            this.lvSH_lastPageBtn.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lvSH_lastPageBtn.Location = new System.Drawing.Point(815, 10);
            this.lvSH_lastPageBtn.Name = "lvSH_lastPageBtn";
            this.lvSH_lastPageBtn.Size = new System.Drawing.Size(30, 21);
            this.lvSH_lastPageBtn.TabIndex = 45;
            this.lvSH_lastPageBtn.Text = ">>";
            this.lvSH_lastPageBtn.UseVisualStyleBackColor = false;
            this.lvSH_lastPageBtn.Click += new System.EventHandler(this.lvSH_lastPageBtn_Click);
            // 
            // tb2_title
            // 
            this.tb2_title.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.tb2_title.Location = new System.Drawing.Point(130, 10);
            this.tb2_title.Name = "tb2_title";
            this.tb2_title.Size = new System.Drawing.Size(366, 21);
            this.tb2_title.TabIndex = 15;
            this.tb2_title.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tb2_title_KeyUp);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(86, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "제목: ";
            // 
            // imageList2
            // 
            this.imageList2.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList2.ImageSize = new System.Drawing.Size(90, 120);
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SJ_ToolStripMenuItem1,
            this.SJ_ToolStripMenuItem2,
            this.SJ_ToolStripMenuItem3,
            this.SJ_ToolStripMenuItem4,
            this.SJ_ToolStripMenuItem5});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(151, 114);
            // 
            // SJ_ToolStripMenuItem1
            // 
            this.SJ_ToolStripMenuItem1.Name = "SJ_ToolStripMenuItem1";
            this.SJ_ToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.SJ_ToolStripMenuItem1.Text = "Preview..";
            this.SJ_ToolStripMenuItem1.Click += new System.EventHandler(this.SJ_ToolStripMenuItem1_Click);
            // 
            // SJ_ToolStripMenuItem2
            // 
            this.SJ_ToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SJ_ToolStripMenuItem2_SubItem1,
            this.SJ_ToolStripMenuItem2_SubItem2});
            this.SJ_ToolStripMenuItem2.Name = "SJ_ToolStripMenuItem2";
            this.SJ_ToolStripMenuItem2.Size = new System.Drawing.Size(150, 22);
            this.SJ_ToolStripMenuItem2.Text = "파일 다운로드";
            // 
            // SJ_ToolStripMenuItem2_SubItem1
            // 
            this.SJ_ToolStripMenuItem2_SubItem1.Name = "SJ_ToolStripMenuItem2_SubItem1";
            this.SJ_ToolStripMenuItem2_SubItem1.Size = new System.Drawing.Size(116, 22);
            this.SJ_ToolStripMenuItem2_SubItem1.Text = "Original";
            this.SJ_ToolStripMenuItem2_SubItem1.Click += new System.EventHandler(this.SJ_ToolStripMenuItem2_SubItem1_Click);
            // 
            // SJ_ToolStripMenuItem2_SubItem2
            // 
            this.SJ_ToolStripMenuItem2_SubItem2.Name = "SJ_ToolStripMenuItem2_SubItem2";
            this.SJ_ToolStripMenuItem2_SubItem2.Size = new System.Drawing.Size(116, 22);
            this.SJ_ToolStripMenuItem2_SubItem2.Text = "Preview";
            this.SJ_ToolStripMenuItem2_SubItem2.Click += new System.EventHandler(this.SJ_ToolStripMenuItem2_SubItem2_Click);
            // 
            // SJ_ToolStripMenuItem3
            // 
            this.SJ_ToolStripMenuItem3.Name = "SJ_ToolStripMenuItem3";
            this.SJ_ToolStripMenuItem3.Size = new System.Drawing.Size(150, 22);
            this.SJ_ToolStripMenuItem3.Text = "파일 수정";
            this.SJ_ToolStripMenuItem3.Click += new System.EventHandler(this.SJ_ToolStripMenuItem3_Click);
            // 
            // SJ_ToolStripMenuItem4
            // 
            this.SJ_ToolStripMenuItem4.Name = "SJ_ToolStripMenuItem4";
            this.SJ_ToolStripMenuItem4.Size = new System.Drawing.Size(150, 22);
            this.SJ_ToolStripMenuItem4.Text = "파일 삭제";
            this.SJ_ToolStripMenuItem4.Click += new System.EventHandler(this.SJ_ToolStripMenuItem4_Click);
            // 
            // SJ_ToolStripMenuItem5
            // 
            this.SJ_ToolStripMenuItem5.Name = "SJ_ToolStripMenuItem5";
            this.SJ_ToolStripMenuItem5.Size = new System.Drawing.Size(150, 22);
            this.SJ_ToolStripMenuItem5.Text = "파일 요소화";
            this.SJ_ToolStripMenuItem5.Click += new System.EventHandler(this.SJ_ToolStripMenuItem5_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.splitContainer2.Panel1.Controls.Add(this.listViewSH);
            this.splitContainer2.Panel1.Controls.Add(this.label7);
            this.splitContainer2.Panel1.Controls.Add(this.sort_CB);
            this.splitContainer2.Panel1.Controls.Add(this.lvSH_totalPageLB);
            this.splitContainer2.Panel1.Controls.Add(this.lvSH_totalCntLB);
            this.splitContainer2.Panel1.Controls.Add(this.listViewSH_RB1);
            this.splitContainer2.Panel1.Controls.Add(this.lvSH_firstPageBtn);
            this.splitContainer2.Panel1.Controls.Add(this.label6);
            this.splitContainer2.Panel1.Controls.Add(this.lvSH_prevPageBtn);
            this.splitContainer2.Panel1.Controls.Add(this.listViewSH_RB2);
            this.splitContainer2.Panel1.Controls.Add(this.lvSH_nextPageBtn);
            this.splitContainer2.Panel1.Controls.Add(this.tb2_title);
            this.splitContainer2.Panel1.Controls.Add(this.lvSH_presentPageLB);
            this.splitContainer2.Panel1.Controls.Add(this.lvSH_lastPageBtn);
            this.splitContainer2.Panel1MinSize = 100;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.splitContainer2.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer2.Panel2MinSize = 100;
            this.splitContainer2.Size = new System.Drawing.Size(855, 665);
            this.splitContainer2.SplitterDistance = 400;
            this.splitContainer2.TabIndex = 58;
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 300;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // SJForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(855, 665);
            this.Controls.Add(this.splitContainer2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SJForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "상주화상";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SJForm_FormClosing);
            this.Load += new System.EventHandler(this.SJForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox sort_CB;
        private System.Windows.Forms.RadioButton listViewSH_RB2;
        private System.Windows.Forms.TextBox lvSH_presentPageLB;
        private System.Windows.Forms.Button lvSH_prevPageBtn;
        private System.Windows.Forms.RadioButton listViewSH_RB1;
        private System.Windows.Forms.Label lvSH_totalPageLB;
        private System.Windows.Forms.Button lvSH_firstPageBtn;
        private System.Windows.Forms.Button lvSH_nextPageBtn;
        private System.Windows.Forms.ListView listViewSH;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lvSH_totalCntLB;
        private System.Windows.Forms.Button lvSH_lastPageBtn;
        private System.Windows.Forms.TextBox tb2_title;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ImageList imageList1;
        private totalmgr.AbortableBackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem SJ_ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem SJ_ToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem SJ_ToolStripMenuItem2_SubItem2;
        private System.Windows.Forms.ToolStripMenuItem SJ_ToolStripMenuItem2_SubItem1;
        private System.Windows.Forms.ToolStripMenuItem SJ_ToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem SJ_ToolStripMenuItem4;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStripMenuItem SJ_ToolStripMenuItem5;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}