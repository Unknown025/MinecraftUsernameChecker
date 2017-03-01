namespace Minecraft_Username_Checker
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
            this.searchTextBox1 = new WindowsFormsAero.SearchTextBox();
            this.themedLabel1 = new WindowsFormsAero.ThemeText.ThemedLabel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // searchTextBox1
            // 
            this.searchTextBox1.ActiveFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextBox1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.searchTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.searchTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTextBox1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.searchTextBox1.InactiveFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic);
            this.searchTextBox1.InactiveText = "Search usernames";
            this.searchTextBox1.Location = new System.Drawing.Point(177, 12);
            this.searchTextBox1.Name = "searchTextBox1";
            this.searchTextBox1.Size = new System.Drawing.Size(495, 21);
            this.searchTextBox1.StartSearchAfterDelay = false;
            this.searchTextBox1.StartSearchOnEnter = true;
            this.searchTextBox1.TabIndex = 0;
            this.searchTextBox1.SearchStarted += new System.EventHandler(this.searchTextBox1_SearchStarted);
            this.searchTextBox1.SearchCancelled += new System.EventHandler(this.searchTextBox1_SearchCancelled);
            // 
            // themedLabel1
            // 
            this.themedLabel1.Location = new System.Drawing.Point(12, 12);
            this.themedLabel1.Name = "themedLabel1";
            this.themedLabel1.Size = new System.Drawing.Size(159, 23);
            this.themedLabel1.TabIndex = 1;
            this.themedLabel1.Text = "Search for Minecraft usernames:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 41);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(660, 333);
            this.dataGridView1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 386);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.themedLabel1);
            this.Controls.Add(this.searchTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(700, 400);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Minecraft Username Checker";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WindowsFormsAero.SearchTextBox searchTextBox1;
        private WindowsFormsAero.ThemeText.ThemedLabel themedLabel1;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

