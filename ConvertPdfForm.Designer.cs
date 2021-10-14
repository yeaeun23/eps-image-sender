namespace ImageMgr
{
    partial class ConvertPdfForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ok_BTN = new System.Windows.Forms.Button();
            this.open_BTN = new System.Windows.Forms.Button();
            this.cancel_BTN = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "PDF 변환 진행중 ...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(363, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "00:00:00.00";
            // 
            // ok_BTN
            // 
            this.ok_BTN.BackColor = System.Drawing.Color.White;
            this.ok_BTN.Enabled = false;
            this.ok_BTN.Location = new System.Drawing.Point(385, 90);
            this.ok_BTN.Name = "ok_BTN";
            this.ok_BTN.Size = new System.Drawing.Size(75, 23);
            this.ok_BTN.TabIndex = 10;
            this.ok_BTN.Text = "닫   기";
            this.ok_BTN.UseVisualStyleBackColor = false;
            this.ok_BTN.Click += new System.EventHandler(this.ok_BTN_Click);
            // 
            // open_BTN
            // 
            this.open_BTN.BackColor = System.Drawing.Color.White;
            this.open_BTN.Enabled = false;
            this.open_BTN.Location = new System.Drawing.Point(304, 90);
            this.open_BTN.Name = "open_BTN";
            this.open_BTN.Size = new System.Drawing.Size(75, 23);
            this.open_BTN.TabIndex = 5;
            this.open_BTN.Text = "파일 열기";
            this.open_BTN.UseVisualStyleBackColor = false;
            this.open_BTN.Click += new System.EventHandler(this.open_BTN_Click);
            // 
            // cancel_BTN
            // 
            this.cancel_BTN.BackColor = System.Drawing.Color.White;
            this.cancel_BTN.Location = new System.Drawing.Point(10, 90);
            this.cancel_BTN.Name = "cancel_BTN";
            this.cancel_BTN.Size = new System.Drawing.Size(75, 23);
            this.cancel_BTN.TabIndex = 1;
            this.cancel_BTN.Text = "취   소";
            this.cancel_BTN.UseVisualStyleBackColor = false;
            this.cancel_BTN.Click += new System.EventHandler(this.cancel_BTN_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(11, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(450, 65);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 10;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ConvertEpsToPdf);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // ConvertPdfForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(469, 126);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancel_BTN);
            this.Controls.Add(this.open_BTN);
            this.Controls.Add(this.ok_BTN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ConvertPdfForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDF 변환";
            this.Load += new System.EventHandler(this.ConvertPdfForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ok_BTN;
        private System.Windows.Forms.Button open_BTN;
        private System.Windows.Forms.Button cancel_BTN;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}