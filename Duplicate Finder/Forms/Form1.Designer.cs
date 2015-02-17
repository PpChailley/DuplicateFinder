namespace Gbd.Sandbox.DuplicateFinder.Forms
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
            this.cbSearchPath = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lbSearchMethod = new System.Windows.Forms.ListBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // cbSearchPath
            // 
            this.cbSearchPath.FormattingEnabled = true;
            this.cbSearchPath.Location = new System.Drawing.Point(11, 24);
            this.cbSearchPath.Name = "cbSearchPath";
            this.cbSearchPath.Size = new System.Drawing.Size(432, 21);
            this.cbSearchPath.TabIndex = 0;
            this.cbSearchPath.Text = "S:\\Dropbox\\visual studio\\sandboxes\\duplicate finder\\TestDataSet";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(449, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 22);
            this.button1.TabIndex = 1;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbSearchMethod
            // 
            this.lbSearchMethod.FormattingEnabled = true;
            this.lbSearchMethod.Location = new System.Drawing.Point(11, 51);
            this.lbSearchMethod.Name = "lbSearchMethod";
            this.lbSearchMethod.Size = new System.Drawing.Size(278, 17);
            this.lbSearchMethod.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 121);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(782, 480);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 613);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lbSearchMethod);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbSearchPath);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbSearchPath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lbSearchMethod;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

