namespace Jukebox
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
            this.PlayBtn = new System.Windows.Forms.Button();
            this.SongSelector = new System.Windows.Forms.ComboBox();
            this.RepeatBox = new System.Windows.Forms.CheckBox();
            this.StopBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PlayBtn
            // 
            this.PlayBtn.Location = new System.Drawing.Point(239, 74);
            this.PlayBtn.Name = "PlayBtn";
            this.PlayBtn.Size = new System.Drawing.Size(75, 23);
            this.PlayBtn.TabIndex = 0;
            this.PlayBtn.Text = "Play";
            this.PlayBtn.UseVisualStyleBackColor = true;
            this.PlayBtn.Click += new System.EventHandler(this.PlayBtn_Click);
            // 
            // SongSelector
            // 
            this.SongSelector.FormattingEnabled = true;
            this.SongSelector.Location = new System.Drawing.Point(498, 12);
            this.SongSelector.Name = "SongSelector";
            this.SongSelector.Size = new System.Drawing.Size(403, 21);
            this.SongSelector.TabIndex = 1;
            // 
            // RepeatBox
            // 
            this.RepeatBox.AutoSize = true;
            this.RepeatBox.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.RepeatBox.Location = new System.Drawing.Point(320, 74);
            this.RepeatBox.Name = "RepeatBox";
            this.RepeatBox.Size = new System.Drawing.Size(46, 31);
            this.RepeatBox.TabIndex = 2;
            this.RepeatBox.Text = "Repeat";
            this.RepeatBox.UseVisualStyleBackColor = true;
            // 
            // StopBtn
            // 
            this.StopBtn.Location = new System.Drawing.Point(372, 74);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(75, 23);
            this.StopBtn.TabIndex = 3;
            this.StopBtn.Text = "Stop";
            this.StopBtn.UseVisualStyleBackColor = true;
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 449);
            this.Controls.Add(this.StopBtn);
            this.Controls.Add(this.RepeatBox);
            this.Controls.Add(this.SongSelector);
            this.Controls.Add(this.PlayBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PlayBtn;
        private System.Windows.Forms.ComboBox SongSelector;
        private System.Windows.Forms.CheckBox RepeatBox;
        private System.Windows.Forms.Button StopBtn;
    }
}

