
namespace YoutubeDownloader
{
    partial class Form1
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
            this.downloadBtn = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.txtYoutubeUrl = new System.Windows.Forms.TextBox();
            this.isim = new System.Windows.Forms.Label();
            this.durum = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // downloadBtn
            // 
            this.downloadBtn.Location = new System.Drawing.Point(170, 114);
            this.downloadBtn.Name = "downloadBtn";
            this.downloadBtn.Size = new System.Drawing.Size(75, 23);
            this.downloadBtn.TabIndex = 0;
            this.downloadBtn.Text = "indir";
            this.downloadBtn.UseVisualStyleBackColor = true;
            this.downloadBtn.Click += new System.EventHandler(this.downloadBtn_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(17, 85);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(391, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // txtYoutubeUrl
            // 
            this.txtYoutubeUrl.Location = new System.Drawing.Point(17, 59);
            this.txtYoutubeUrl.Name = "txtYoutubeUrl";
            this.txtYoutubeUrl.Size = new System.Drawing.Size(391, 20);
            this.txtYoutubeUrl.TabIndex = 2;
            // 
            // isim
            // 
            this.isim.AutoSize = true;
            this.isim.Location = new System.Drawing.Point(14, 43);
            this.isim.Name = "isim";
            this.isim.Size = new System.Drawing.Size(0, 13);
            this.isim.TabIndex = 3;
            // 
            // durum
            // 
            this.durum.AutoSize = true;
            this.durum.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.durum.Location = new System.Drawing.Point(13, 154);
            this.durum.Name = "durum";
            this.durum.Size = new System.Drawing.Size(0, 24);
            this.durum.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 187);
            this.Controls.Add(this.durum);
            this.Controls.Add(this.isim);
            this.Controls.Add(this.txtYoutubeUrl);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.downloadBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Youtube Downloader";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button downloadBtn;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox txtYoutubeUrl;
        private System.Windows.Forms.Label isim;
        private System.Windows.Forms.Label durum;
    }
}

