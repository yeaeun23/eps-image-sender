namespace ImageMgr
{
    partial class FolderSetForm
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
            this.workFolderBrowse_BTN = new System.Windows.Forms.Button();
            this.workFolderPath_TB = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.ok_BTN = new System.Windows.Forms.Button();
            this.cancel_BTN = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.downloadFolderPath_TB = new System.Windows.Forms.TextBox();
            this.downloadFolderBrowse_BTN = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tempFolderPath_TB = new System.Windows.Forms.TextBox();
            this.tempFolderBrowse_BTN = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pdfDistFilePath_TB = new System.Windows.Forms.TextBox();
            this.pdfDistFileBrowse_BTN = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // workFolderBrowse_BTN
            // 
            this.workFolderBrowse_BTN.BackColor = System.Drawing.Color.White;
            this.workFolderBrowse_BTN.Location = new System.Drawing.Point(426, 30);
            this.workFolderBrowse_BTN.Name = "workFolderBrowse_BTN";
            this.workFolderBrowse_BTN.Size = new System.Drawing.Size(30, 21);
            this.workFolderBrowse_BTN.TabIndex = 5;
            this.workFolderBrowse_BTN.Text = "...";
            this.workFolderBrowse_BTN.UseVisualStyleBackColor = false;
            this.workFolderBrowse_BTN.Click += new System.EventHandler(this.workFolderBrowse_BTN_Click);
            // 
            // workFolderPath_TB
            // 
            this.workFolderPath_TB.Location = new System.Drawing.Point(20, 30);
            this.workFolderPath_TB.Name = "workFolderPath_TB";
            this.workFolderPath_TB.Size = new System.Drawing.Size(400, 21);
            this.workFolderPath_TB.TabIndex = 1;
            // 
            // ok_BTN
            // 
            this.ok_BTN.BackColor = System.Drawing.Color.White;
            this.ok_BTN.Location = new System.Drawing.Point(169, 375);
            this.ok_BTN.Name = "ok_BTN";
            this.ok_BTN.Size = new System.Drawing.Size(75, 23);
            this.ok_BTN.TabIndex = 30;
            this.ok_BTN.Text = "확   인";
            this.ok_BTN.UseVisualStyleBackColor = false;
            this.ok_BTN.Click += new System.EventHandler(this.ok_BTN_Click);
            // 
            // cancel_BTN
            // 
            this.cancel_BTN.BackColor = System.Drawing.Color.White;
            this.cancel_BTN.Location = new System.Drawing.Point(264, 375);
            this.cancel_BTN.Name = "cancel_BTN";
            this.cancel_BTN.Size = new System.Drawing.Size(75, 23);
            this.cancel_BTN.TabIndex = 35;
            this.cancel_BTN.Text = "취   소";
            this.cancel_BTN.UseVisualStyleBackColor = false;
            this.cancel_BTN.Click += new System.EventHandler(this.cancel_BTN_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.workFolderPath_TB);
            this.groupBox1.Controls.Add(this.workFolderBrowse_BTN);
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(475, 75);
            this.groupBox1.TabIndex = 40;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "작업 폴더 *";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.downloadFolderPath_TB);
            this.groupBox2.Controls.Add(this.downloadFolderBrowse_BTN);
            this.groupBox2.Location = new System.Drawing.Point(10, 91);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(475, 75);
            this.groupBox2.TabIndex = 45;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "다운로드 폴더 *";
            // 
            // downloadFolderPath_TB
            // 
            this.downloadFolderPath_TB.Location = new System.Drawing.Point(20, 30);
            this.downloadFolderPath_TB.Name = "downloadFolderPath_TB";
            this.downloadFolderPath_TB.Size = new System.Drawing.Size(400, 21);
            this.downloadFolderPath_TB.TabIndex = 10;
            // 
            // downloadFolderBrowse_BTN
            // 
            this.downloadFolderBrowse_BTN.BackColor = System.Drawing.Color.White;
            this.downloadFolderBrowse_BTN.Location = new System.Drawing.Point(426, 30);
            this.downloadFolderBrowse_BTN.Name = "downloadFolderBrowse_BTN";
            this.downloadFolderBrowse_BTN.Size = new System.Drawing.Size(30, 21);
            this.downloadFolderBrowse_BTN.TabIndex = 15;
            this.downloadFolderBrowse_BTN.Text = "...";
            this.downloadFolderBrowse_BTN.UseVisualStyleBackColor = false;
            this.downloadFolderBrowse_BTN.Click += new System.EventHandler(this.downloadFolderBrowse_BTN_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tempFolderPath_TB);
            this.groupBox3.Controls.Add(this.tempFolderBrowse_BTN);
            this.groupBox3.Location = new System.Drawing.Point(10, 172);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(475, 75);
            this.groupBox3.TabIndex = 50;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "EPS 파일 변환 폴더 **";
            // 
            // tempFolderPath_TB
            // 
            this.tempFolderPath_TB.Location = new System.Drawing.Point(20, 30);
            this.tempFolderPath_TB.Name = "tempFolderPath_TB";
            this.tempFolderPath_TB.Size = new System.Drawing.Size(400, 21);
            this.tempFolderPath_TB.TabIndex = 20;
            // 
            // tempFolderBrowse_BTN
            // 
            this.tempFolderBrowse_BTN.BackColor = System.Drawing.Color.White;
            this.tempFolderBrowse_BTN.Location = new System.Drawing.Point(426, 30);
            this.tempFolderBrowse_BTN.Name = "tempFolderBrowse_BTN";
            this.tempFolderBrowse_BTN.Size = new System.Drawing.Size(30, 21);
            this.tempFolderBrowse_BTN.TabIndex = 25;
            this.tempFolderBrowse_BTN.Text = "...";
            this.tempFolderBrowse_BTN.UseVisualStyleBackColor = false;
            this.tempFolderBrowse_BTN.Click += new System.EventHandler(this.tempFolderBrowse_BTN_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Exe Files (*.exe, *.EXE)|*.exe;*.EXE";
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.DarkGray;
            this.label1.Location = new System.Drawing.Point(35, 331);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(450, 11);
            this.label1.TabIndex = 8;
            this.label1.Text = "* 마지막으로 선택/다운로드한 폴더로 자동 설정됩니다.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.pdfDistFilePath_TB);
            this.groupBox4.Controls.Add(this.pdfDistFileBrowse_BTN);
            this.groupBox4.Location = new System.Drawing.Point(10, 253);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(475, 75);
            this.groupBox4.TabIndex = 51;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "PDF 변환 프로그램 (acrodist.exe) **";
            // 
            // pdfDistFilePath_TB
            // 
            this.pdfDistFilePath_TB.Location = new System.Drawing.Point(20, 30);
            this.pdfDistFilePath_TB.Name = "pdfDistFilePath_TB";
            this.pdfDistFilePath_TB.Size = new System.Drawing.Size(400, 21);
            this.pdfDistFilePath_TB.TabIndex = 20;
            // 
            // pdfDistFileBrowse_BTN
            // 
            this.pdfDistFileBrowse_BTN.BackColor = System.Drawing.Color.White;
            this.pdfDistFileBrowse_BTN.Location = new System.Drawing.Point(426, 30);
            this.pdfDistFileBrowse_BTN.Name = "pdfDistFileBrowse_BTN";
            this.pdfDistFileBrowse_BTN.Size = new System.Drawing.Size(30, 21);
            this.pdfDistFileBrowse_BTN.TabIndex = 25;
            this.pdfDistFileBrowse_BTN.Text = "...";
            this.pdfDistFileBrowse_BTN.UseVisualStyleBackColor = false;
            this.pdfDistFileBrowse_BTN.Click += new System.EventHandler(this.pdfDistFileBrowse_BTN_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.DarkGray;
            this.label3.Location = new System.Drawing.Point(35, 347);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(450, 11);
            this.label3.TabIndex = 53;
            this.label3.Text = "** 프로그램 종료 시 변환된 파일은 자동 삭제됩니다.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 300;
            // 
            // FolderSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(494, 421);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancel_BTN);
            this.Controls.Add(this.ok_BTN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FolderSetForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "폴더/파일 설정";
            this.Load += new System.EventHandler(this.FolderSetForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button workFolderBrowse_BTN;
        private System.Windows.Forms.TextBox workFolderPath_TB;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button ok_BTN;
        private System.Windows.Forms.Button cancel_BTN;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox downloadFolderPath_TB;
        private System.Windows.Forms.Button downloadFolderBrowse_BTN;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tempFolderPath_TB;
        private System.Windows.Forms.Button tempFolderBrowse_BTN;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox pdfDistFilePath_TB;
        private System.Windows.Forms.Button pdfDistFileBrowse_BTN;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}