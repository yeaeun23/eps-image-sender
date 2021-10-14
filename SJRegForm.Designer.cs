namespace ImageMgr
{
    partial class SJRegForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.registerDate_TB = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.registrant_TB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.title_TB = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ok_BTN
            // 
            this.ok_BTN.BackColor = System.Drawing.Color.White;
            this.ok_BTN.Location = new System.Drawing.Point(163, 174);
            this.ok_BTN.Name = "ok_BTN";
            this.ok_BTN.Size = new System.Drawing.Size(75, 23);
            this.ok_BTN.TabIndex = 20;
            this.ok_BTN.Text = "등   록";
            this.ok_BTN.UseVisualStyleBackColor = false;
            this.ok_BTN.Click += new System.EventHandler(this.ok_BTN_Click);
            // 
            // cancel_BTN
            // 
            this.cancel_BTN.BackColor = System.Drawing.Color.White;
            this.cancel_BTN.Location = new System.Drawing.Point(258, 174);
            this.cancel_BTN.Name = "cancel_BTN";
            this.cancel_BTN.Size = new System.Drawing.Size(75, 23);
            this.cancel_BTN.TabIndex = 25;
            this.cancel_BTN.Text = "취  소";
            this.cancel_BTN.UseVisualStyleBackColor = false;
            this.cancel_BTN.Click += new System.EventHandler(this.cancel_BTN_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.registerDate_TB);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.registrant_TB);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.title_TB);
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(470, 150);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "등록 정보";
            // 
            // registerDate_TB
            // 
            this.registerDate_TB.Location = new System.Drawing.Point(80, 95);
            this.registerDate_TB.Name = "registerDate_TB";
            this.registerDate_TB.ReadOnly = true;
            this.registerDate_TB.Size = new System.Drawing.Size(250, 21);
            this.registerDate_TB.TabIndex = 5;
            this.registerDate_TB.TabStop = false;
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
            this.registrant_TB.Location = new System.Drawing.Point(80, 65);
            this.registrant_TB.Name = "registrant_TB";
            this.registrant_TB.ReadOnly = true;
            this.registrant_TB.Size = new System.Drawing.Size(250, 21);
            this.registrant_TB.TabIndex = 10;
            this.registrant_TB.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "등록일";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "등록자";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "제   목";
            // 
            // title_TB
            // 
            this.title_TB.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.title_TB.Location = new System.Drawing.Point(80, 35);
            this.title_TB.Name = "title_TB";
            this.title_TB.Size = new System.Drawing.Size(250, 21);
            this.title_TB.TabIndex = 1;
            this.title_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.title_TB_KeyDown);
            // 
            // SJRegForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(490, 215);
            this.Controls.Add(this.ok_BTN);
            this.Controls.Add(this.cancel_BTN);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SJRegForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "상주화상 등록";
            this.Load += new System.EventHandler(this.SJRegForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok_BTN;
        private System.Windows.Forms.Button cancel_BTN;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox registrant_TB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox title_TB;
        private System.Windows.Forms.TextBox registerDate_TB;
    }
}