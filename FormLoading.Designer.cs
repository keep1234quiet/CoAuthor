namespace CoAuthor
{
    partial class FormLoading
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tableLayoutPanel_loading = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox_loading = new System.Windows.Forms.PictureBox();
            this.label_tips = new System.Windows.Forms.Label();
            this.tableLayoutPanel_loading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_loading)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel_loading
            // 
            this.tableLayoutPanel_loading.ColumnCount = 1;
            this.tableLayoutPanel_loading.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_loading.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_loading.Controls.Add(this.pictureBox_loading, 0, 0);
            this.tableLayoutPanel_loading.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel_loading.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_loading.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel_loading.Name = "tableLayoutPanel_loading";
            this.tableLayoutPanel_loading.RowCount = 1;
            this.tableLayoutPanel_loading.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_loading.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_loading.Size = new System.Drawing.Size(100, 100);
            this.tableLayoutPanel_loading.TabIndex = 0;
            // 
            // pictureBox_loading
            // 
            this.pictureBox_loading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_loading.Image = global::CoAuthor.Properties.Resources.loading_state;
            this.pictureBox_loading.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_loading.Name = "pictureBox_loading";
            this.pictureBox_loading.Size = new System.Drawing.Size(94, 94);
            this.pictureBox_loading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_loading.TabIndex = 3;
            this.pictureBox_loading.TabStop = false;
            this.pictureBox_loading.Visible = false;
            // 
            // label_tips
            // 
            this.label_tips.AutoSize = true;
            this.label_tips.Location = new System.Drawing.Point(29, 102);
            this.label_tips.Name = "label_tips";
            this.label_tips.Size = new System.Drawing.Size(0, 13);
            this.label_tips.TabIndex = 1;
            // 
            // FormLoading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(100, 131);
            this.Controls.Add(this.label_tips);
            this.Controls.Add(this.tableLayoutPanel_loading);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormLoading";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "插件窗口";
            this.TopMost = true;
            this.tableLayoutPanel_loading.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_loading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_loading;
        private System.Windows.Forms.PictureBox pictureBox_loading;
        private System.Windows.Forms.Label label_tips;
    }
}