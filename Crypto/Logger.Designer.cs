namespace Crypto
{
    partial class Logger
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
            richTextBox1 = new System.Windows.Forms.RichTextBox();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = System.Drawing.Color.Black;
            richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            richTextBox1.ForeColor = System.Drawing.Color.White;
            richTextBox1.Location = new System.Drawing.Point(0, 0);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(616, 251);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // Logger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 251);
            this.Controls.Add(richTextBox1);
            this.Name = "Logger";
            this.ShowIcon = false;
            this.Text = "Logger";
            this.Load += new System.EventHandler(this.Logger_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private static System.Windows.Forms.RichTextBox richTextBox1;
    }
}