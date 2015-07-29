namespace eClipx.Controles
{
    partial class ItemControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rootPanel = new System.Windows.Forms.Panel();
            this.lblText = new System.Windows.Forms.Label();
            this.iconPicture = new System.Windows.Forms.PictureBox();
            this.rootPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // rootPanel
            // 
            this.rootPanel.Controls.Add(this.lblText);
            this.rootPanel.Controls.Add(this.iconPicture);
            this.rootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootPanel.Location = new System.Drawing.Point(0, 0);
            this.rootPanel.Margin = new System.Windows.Forms.Padding(0);
            this.rootPanel.Name = "rootPanel";
            this.rootPanel.Size = new System.Drawing.Size(598, 32);
            this.rootPanel.TabIndex = 0;
            // 
            // lblText
            // 
            this.lblText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblText.Location = new System.Drawing.Point(32, 0);
            this.lblText.Margin = new System.Windows.Forms.Padding(0);
            this.lblText.Name = "lblText";
            this.lblText.Padding = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblText.Size = new System.Drawing.Size(566, 32);
            this.lblText.TabIndex = 1;
            this.lblText.Text = "label1";
            this.lblText.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblText_MouseClick);
            this.lblText.MouseEnter += new System.EventHandler(this.controlMouseEnter);
            this.lblText.MouseLeave += new System.EventHandler(this.controlMouseLeave);
            // 
            // iconPicture
            // 
            this.iconPicture.BackColor = System.Drawing.Color.Transparent;
            this.iconPicture.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconPicture.Location = new System.Drawing.Point(0, 0);
            this.iconPicture.Name = "iconPicture";
            this.iconPicture.Size = new System.Drawing.Size(32, 32);
            this.iconPicture.TabIndex = 0;
            this.iconPicture.TabStop = false;
            this.iconPicture.MouseEnter += new System.EventHandler(this.controlMouseEnter);
            this.iconPicture.MouseLeave += new System.EventHandler(this.controlMouseLeave);
            // 
            // ItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.rootPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ItemControl";
            this.Size = new System.Drawing.Size(598, 32);
            this.rootPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel rootPanel;
        private System.Windows.Forms.PictureBox iconPicture;
        private System.Windows.Forms.Label lblText;
    }
}
