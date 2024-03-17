namespace ImageDownsizer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOpenFileDialog = new System.Windows.Forms.Button();
            this.pictureBoxOriginalImage = new System.Windows.Forms.PictureBox();
            this.pictureBoxDownsizedImage = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.downscaleFactor = new System.Windows.Forms.NumericUpDown();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.lblOriginalImage = new System.Windows.Forms.Label();
            this.lblDownsizedImage = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblTimeWithThreads = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginalImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDownsizedImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downscaleFactor)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpenFileDialog
            // 
            this.btnOpenFileDialog.Location = new System.Drawing.Point(38, 62);
            this.btnOpenFileDialog.Name = "btnOpenFileDialog";
            this.btnOpenFileDialog.Size = new System.Drawing.Size(218, 35);
            this.btnOpenFileDialog.TabIndex = 0;
            this.btnOpenFileDialog.Text = "Select file";
            this.btnOpenFileDialog.UseVisualStyleBackColor = true;
            this.btnOpenFileDialog.Click += new System.EventHandler(this.btnOpenFileDialog_Click);
            // 
            // pictureBoxOriginalImage
            // 
            this.pictureBoxOriginalImage.Location = new System.Drawing.Point(12, 175);
            this.pictureBoxOriginalImage.Name = "pictureBoxOriginalImage";
            this.pictureBoxOriginalImage.Size = new System.Drawing.Size(377, 263);
            this.pictureBoxOriginalImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxOriginalImage.TabIndex = 1;
            this.pictureBoxOriginalImage.TabStop = false;
            // 
            // pictureBoxDownsizedImage
            // 
            this.pictureBoxDownsizedImage.Location = new System.Drawing.Point(406, 175);
            this.pictureBoxDownsizedImage.Name = "pictureBoxDownsizedImage";
            this.pictureBoxDownsizedImage.Size = new System.Drawing.Size(382, 263);
            this.pictureBoxDownsizedImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxDownsizedImage.TabIndex = 2;
            this.pictureBoxDownsizedImage.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(314, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select down-scaling factor in %";
            // 
            // downscaleFactor
            // 
            this.downscaleFactor.Location = new System.Drawing.Point(335, 75);
            this.downscaleFactor.Name = "downscaleFactor";
            this.downscaleFactor.Size = new System.Drawing.Size(120, 23);
            this.downscaleFactor.TabIndex = 5;
            this.downscaleFactor.Tag = "";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(531, 61);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(218, 37);
            this.btnSubmit.TabIndex = 6;
            this.btnSubmit.Text = "Proceed";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // lblOriginalImage
            // 
            this.lblOriginalImage.AutoSize = true;
            this.lblOriginalImage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblOriginalImage.Location = new System.Drawing.Point(132, 151);
            this.lblOriginalImage.Name = "lblOriginalImage";
            this.lblOriginalImage.Size = new System.Drawing.Size(124, 21);
            this.lblOriginalImage.TabIndex = 7;
            this.lblOriginalImage.Text = "Original Image";
            // 
            // lblDownsizedImage
            // 
            this.lblDownsizedImage.AutoSize = true;
            this.lblDownsizedImage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblDownsizedImage.Location = new System.Drawing.Point(531, 151);
            this.lblDownsizedImage.Name = "lblDownsizedImage";
            this.lblDownsizedImage.Size = new System.Drawing.Size(146, 21);
            this.lblDownsizedImage.TabIndex = 8;
            this.lblDownsizedImage.Text = "Downsized Image";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(531, 9);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(39, 15);
            this.lblTime.TabIndex = 9;
            this.lblTime.Text = "Time: ";
            // 
            // lblTimeWithThreads
            // 
            this.lblTimeWithThreads.AutoSize = true;
            this.lblTimeWithThreads.Location = new System.Drawing.Point(531, 33);
            this.lblTimeWithThreads.Name = "lblTimeWithThreads";
            this.lblTimeWithThreads.Size = new System.Drawing.Size(107, 15);
            this.lblTimeWithThreads.TabIndex = 10;
            this.lblTimeWithThreads.Text = "Time with threads: ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblTimeWithThreads);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblDownsizedImage);
            this.Controls.Add(this.lblOriginalImage);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.downscaleFactor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBoxDownsizedImage);
            this.Controls.Add(this.pictureBoxOriginalImage);
            this.Controls.Add(this.btnOpenFileDialog);
            this.Name = "Form1";
            this.Text = "Image Downsizer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginalImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDownsizedImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downscaleFactor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnOpenFileDialog;
        private PictureBox pictureBoxOriginalImage;
        private PictureBox pictureBoxDownsizedImage;
        private Label label1;
        private NumericUpDown downscaleFactor;
        private Button btnSubmit;
        private Label lblOriginalImage;
        private Label lblDownsizedImage;
        private Label lblTime;
        private Label lblTimeWithThreads;
    }
}