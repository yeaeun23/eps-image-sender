namespace ImageMgr
{
    partial class SJSendForm
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
            this.ok_BTN = new System.Windows.Forms.Button();
            this.cancel_BTN = new System.Windows.Forms.Button();
            this.registerDate_TB = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.myun_CB = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.media_CB = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.pan_CB = new System.Windows.Forms.ComboBox();
            this.jibang_CB = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.paperDate_CAL = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.title_TB = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.registrant_TB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ok_BTN
            // 
            this.ok_BTN.BackColor = System.Drawing.Color.White;
            this.ok_BTN.Location = new System.Drawing.Point(163, 330);
            this.ok_BTN.Name = "ok_BTN";
            this.ok_BTN.Size = new System.Drawing.Size(75, 23);
            this.ok_BTN.TabIndex = 73;
            this.ok_BTN.Text = "등   록";
            this.ok_BTN.UseVisualStyleBackColor = false;
            this.ok_BTN.Click += new System.EventHandler(this.ok_BTN_Click);
            // 
            // cancel_BTN
            // 
            this.cancel_BTN.BackColor = System.Drawing.Color.White;
            this.cancel_BTN.Location = new System.Drawing.Point(258, 330);
            this.cancel_BTN.Name = "cancel_BTN";
            this.cancel_BTN.Size = new System.Drawing.Size(75, 23);
            this.cancel_BTN.TabIndex = 74;
            this.cancel_BTN.Text = "취  소";
            this.cancel_BTN.UseVisualStyleBackColor = false;
            this.cancel_BTN.Click += new System.EventHandler(this.cancel_BTN_Click);
            // 
            // registerDate_TB
            // 
            this.registerDate_TB.Location = new System.Drawing.Point(310, 35);
            this.registerDate_TB.Name = "registerDate_TB";
            this.registerDate_TB.ReadOnly = true;
            this.registerDate_TB.Size = new System.Drawing.Size(140, 21);
            this.registerDate_TB.TabIndex = 30;
            this.registerDate_TB.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.registerDate_TB);
            this.groupBox2.Controls.Add(this.myun_CB);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.media_CB);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.pan_CB);
            this.groupBox2.Controls.Add(this.jibang_CB);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.paperDate_CAL);
            this.groupBox2.Location = new System.Drawing.Point(10, 166);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(470, 150);
            this.groupBox2.TabIndex = 72;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "등록 정보 2";
            // 
            // myun_CB
            // 
            this.myun_CB.FormattingEnabled = true;
            this.myun_CB.Items.AddRange(new object[] {
            "선택"});
            this.myun_CB.Location = new System.Drawing.Point(310, 95);
            this.myun_CB.Name = "myun_CB";
            this.myun_CB.Size = new System.Drawing.Size(140, 20);
            this.myun_CB.TabIndex = 50;
            this.myun_CB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.myun_CB_KeyUp);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(250, 100);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 20;
            this.label11.Text = "   면   ";
            // 
            // media_CB
            // 
            this.media_CB.FormattingEnabled = true;
            this.media_CB.Items.AddRange(new object[] {
            "선택"});
            this.media_CB.Location = new System.Drawing.Point(80, 65);
            this.media_CB.Name = "media_CB";
            this.media_CB.Size = new System.Drawing.Size(140, 20);
            this.media_CB.TabIndex = 35;
            this.media_CB.Click += new System.EventHandler(this.media_CB_Click);
            this.media_CB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.media_CB_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 100);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 18;
            this.label10.Text = "   판   ";
            // 
            // pan_CB
            // 
            this.pan_CB.FormattingEnabled = true;
            this.pan_CB.Items.AddRange(new object[] {
            "선택"});
            this.pan_CB.Location = new System.Drawing.Point(80, 95);
            this.pan_CB.Name = "pan_CB";
            this.pan_CB.Size = new System.Drawing.Size(140, 20);
            this.pan_CB.TabIndex = 45;
            // 
            // jibang_CB
            // 
            this.jibang_CB.FormattingEnabled = true;
            this.jibang_CB.Items.AddRange(new object[] {
            "선택"});
            this.jibang_CB.Location = new System.Drawing.Point(310, 65);
            this.jibang_CB.Name = "jibang_CB";
            this.jibang_CB.Size = new System.Drawing.Size(140, 20);
            this.jibang_CB.TabIndex = 40;
            this.jibang_CB.Click += new System.EventHandler(this.jibang_CB_Click);
            this.jibang_CB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.jibang_CB_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(250, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "등록일";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "매   체";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(250, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "지   방";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "게재일";
            // 
            // paperDate_CAL
            // 
            this.paperDate_CAL.CustomFormat = "yyyy-MM-dd dddd";
            this.paperDate_CAL.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.paperDate_CAL.Location = new System.Drawing.Point(80, 35);
            this.paperDate_CAL.Name = "paperDate_CAL";
            this.paperDate_CAL.Size = new System.Drawing.Size(140, 21);
            this.paperDate_CAL.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "제   목";
            // 
            // title_TB
            // 
            this.title_TB.Location = new System.Drawing.Point(80, 50);
            this.title_TB.Name = "title_TB";
            this.title_TB.ReadOnly = true;
            this.title_TB.Size = new System.Drawing.Size(250, 21);
            this.title_TB.TabIndex = 1;
            this.title_TB.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.registrant_TB);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.title_TB);
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(470, 150);
            this.groupBox1.TabIndex = 71;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "등록 정보 1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(350, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 110);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // registrant_TB
            // 
            this.registrant_TB.Location = new System.Drawing.Point(80, 80);
            this.registrant_TB.Name = "registrant_TB";
            this.registrant_TB.ReadOnly = true;
            this.registrant_TB.Size = new System.Drawing.Size(250, 21);
            this.registrant_TB.TabIndex = 10;
            this.registrant_TB.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "등록자";
            // 
            // SJSendForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(490, 371);
            this.Controls.Add(this.ok_BTN);
            this.Controls.Add(this.cancel_BTN);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SJSendForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "상주화상 요소화";
            this.Load += new System.EventHandler(this.SJSendForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok_BTN;
        private System.Windows.Forms.Button cancel_BTN;
        private System.Windows.Forms.TextBox registerDate_TB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox myun_CB;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox media_CB;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox pan_CB;
        private System.Windows.Forms.ComboBox jibang_CB;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker paperDate_CAL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox title_TB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox registrant_TB;
        private System.Windows.Forms.Label label3;
    }
}