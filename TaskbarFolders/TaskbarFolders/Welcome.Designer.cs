namespace TaskbarFolders
{
    partial class Welcome
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Welcome));
            this.bottomPanel1 = new AeroSuite.Controls.BottomPanel();
            this.aeroLinkLabel2 = new AeroSuite.Controls.AeroLinkLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.aeroLinkLabel1 = new AeroSuite.Controls.AeroLinkLabel();
            this.captionLabel1 = new AeroSuite.Controls.CaptionLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.bottomPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bottomPanel1
            // 
            this.bottomPanel1.Controls.Add(this.aeroLinkLabel2);
            this.bottomPanel1.Controls.Add(this.tableLayoutPanel1);
            this.bottomPanel1.Controls.Add(this.aeroLinkLabel1);
            this.bottomPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel1.Location = new System.Drawing.Point(0, 139);
            this.bottomPanel1.Margin = new System.Windows.Forms.Padding(5);
            this.bottomPanel1.Name = "bottomPanel1";
            this.bottomPanel1.Size = new System.Drawing.Size(763, 40);
            this.bottomPanel1.TabIndex = 15;
            // 
            // aeroLinkLabel2
            // 
            this.aeroLinkLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.aeroLinkLabel2.AutoSize = true;
            this.aeroLinkLabel2.Location = new System.Drawing.Point(58, 14);
            this.aeroLinkLabel2.Name = "aeroLinkLabel2";
            this.aeroLinkLabel2.Size = new System.Drawing.Size(46, 13);
            this.aeroLinkLabel2.TabIndex = 20;
            this.aeroLinkLabel2.TabStop = true;
            this.aeroLinkLabel2.Text = "Website";
            this.aeroLinkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.aeroLinkLabel2_LinkClicked);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.button2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(668, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(95, 40);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Location = new System.Drawing.Point(8, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 24);
            this.button2.TabIndex = 1;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // aeroLinkLabel1
            // 
            this.aeroLinkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.aeroLinkLabel1.AutoSize = true;
            this.aeroLinkLabel1.Location = new System.Drawing.Point(12, 14);
            this.aeroLinkLabel1.Name = "aeroLinkLabel1";
            this.aeroLinkLabel1.Size = new System.Drawing.Size(40, 13);
            this.aeroLinkLabel1.TabIndex = 19;
            this.aeroLinkLabel1.TabStop = true;
            this.aeroLinkLabel1.Text = "GitHub";
            this.aeroLinkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.aeroLinkLabel1_LinkClicked);
            // 
            // captionLabel1
            // 
            this.captionLabel1.AutoSize = true;
            this.captionLabel1.Location = new System.Drawing.Point(8, 9);
            this.captionLabel1.Name = "captionLabel1";
            this.captionLabel1.Size = new System.Drawing.Size(203, 21);
            this.captionLabel1.TabIndex = 16;
            this.captionLabel1.Text = "Welcome to Taskbar Folders";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(735, 32);
            this.label1.TabIndex = 18;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // Welcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 179);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.captionLabel1);
            this.Controls.Add(this.bottomPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Welcome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Welcome";
            this.bottomPanel1.ResumeLayout(false);
            this.bottomPanel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AeroSuite.Controls.BottomPanel bottomPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button2;
        private AeroSuite.Controls.CaptionLabel captionLabel1;
        private System.Windows.Forms.Label label1;
        private AeroSuite.Controls.AeroLinkLabel aeroLinkLabel2;
        private AeroSuite.Controls.AeroLinkLabel aeroLinkLabel1;
    }
}