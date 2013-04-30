namespace Shane.Church.PhotoGallery.Publish
{
    partial class ConfigureForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigureForm));
			this.comboBoxServer = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxPages = new System.Windows.Forms.ComboBox();
			this.labelPage = new System.Windows.Forms.Label();
			this.buttonCreateNew = new System.Windows.Forms.Button();
			this.btnPublish = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.textBoxResizeSize = new System.Windows.Forms.TextBox();
			this.textBoxMaxPreviewHeight = new System.Windows.Forms.TextBox();
			this.textBoxMaxPreviewWidth = new System.Windows.Forms.TextBox();
			this.labelResizeSize = new System.Windows.Forms.Label();
			this.labelMaxPreviewHeight = new System.Windows.Forms.Label();
			this.labelMaxPreviewWidth = new System.Windows.Forms.Label();
			this.labelVersion = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// comboBoxServer
			// 
			this.comboBoxServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxServer.FormattingEnabled = true;
			this.comboBoxServer.Items.AddRange(new object[] {
            "Azure",
            "Local"});
			this.comboBoxServer.Location = new System.Drawing.Point(121, 12);
			this.comboBoxServer.Name = "comboBoxServer";
			this.comboBoxServer.Size = new System.Drawing.Size(184, 21);
			this.comboBoxServer.TabIndex = 0;
			this.comboBoxServer.SelectedIndexChanged += new System.EventHandler(this.comboBoxServer_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Server:";
			// 
			// comboBoxPages
			// 
			this.comboBoxPages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxPages.FormattingEnabled = true;
			this.comboBoxPages.Location = new System.Drawing.Point(121, 39);
			this.comboBoxPages.Name = "comboBoxPages";
			this.comboBoxPages.Size = new System.Drawing.Size(184, 21);
			this.comboBoxPages.TabIndex = 1;
			// 
			// labelPage
			// 
			this.labelPage.AutoSize = true;
			this.labelPage.Location = new System.Drawing.Point(12, 42);
			this.labelPage.Name = "labelPage";
			this.labelPage.Size = new System.Drawing.Size(35, 13);
			this.labelPage.TabIndex = 3;
			this.labelPage.Text = "Page:";
			// 
			// buttonCreateNew
			// 
			this.buttonCreateNew.Location = new System.Drawing.Point(311, 39);
			this.buttonCreateNew.Name = "buttonCreateNew";
			this.buttonCreateNew.Size = new System.Drawing.Size(113, 23);
			this.buttonCreateNew.TabIndex = 2;
			this.buttonCreateNew.Text = "Create New Page";
			this.buttonCreateNew.UseVisualStyleBackColor = true;
			this.buttonCreateNew.Click += new System.EventHandler(this.buttonCreateNew_Click);
			// 
			// btnPublish
			// 
			this.btnPublish.Location = new System.Drawing.Point(349, 152);
			this.btnPublish.Name = "btnPublish";
			this.btnPublish.Size = new System.Drawing.Size(75, 23);
			this.btnPublish.TabIndex = 7;
			this.btnPublish.Text = "Publish";
			this.btnPublish.UseVisualStyleBackColor = true;
			this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(268, 152);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// textBoxResizeSize
			// 
			this.textBoxResizeSize.Location = new System.Drawing.Point(121, 67);
			this.textBoxResizeSize.Name = "textBoxResizeSize";
			this.textBoxResizeSize.Size = new System.Drawing.Size(184, 20);
			this.textBoxResizeSize.TabIndex = 3;
			this.textBoxResizeSize.Text = "576";
			// 
			// textBoxMaxPreviewHeight
			// 
			this.textBoxMaxPreviewHeight.Location = new System.Drawing.Point(121, 119);
			this.textBoxMaxPreviewHeight.Name = "textBoxMaxPreviewHeight";
			this.textBoxMaxPreviewHeight.Size = new System.Drawing.Size(184, 20);
			this.textBoxMaxPreviewHeight.TabIndex = 5;
			this.textBoxMaxPreviewHeight.Text = "173";
			// 
			// textBoxMaxPreviewWidth
			// 
			this.textBoxMaxPreviewWidth.Location = new System.Drawing.Point(121, 93);
			this.textBoxMaxPreviewWidth.Name = "textBoxMaxPreviewWidth";
			this.textBoxMaxPreviewWidth.Size = new System.Drawing.Size(184, 20);
			this.textBoxMaxPreviewWidth.TabIndex = 4;
			this.textBoxMaxPreviewWidth.Text = "231";
			// 
			// labelResizeSize
			// 
			this.labelResizeSize.AutoSize = true;
			this.labelResizeSize.Location = new System.Drawing.Point(12, 70);
			this.labelResizeSize.Name = "labelResizeSize";
			this.labelResizeSize.Size = new System.Drawing.Size(103, 13);
			this.labelResizeSize.TabIndex = 10;
			this.labelResizeSize.Text = "Resized Image Size:";
			// 
			// labelMaxPreviewHeight
			// 
			this.labelMaxPreviewHeight.AutoSize = true;
			this.labelMaxPreviewHeight.Location = new System.Drawing.Point(12, 122);
			this.labelMaxPreviewHeight.Name = "labelMaxPreviewHeight";
			this.labelMaxPreviewHeight.Size = new System.Drawing.Size(105, 13);
			this.labelMaxPreviewHeight.TabIndex = 11;
			this.labelMaxPreviewHeight.Text = "Max Preview Height:";
			// 
			// labelMaxPreviewWidth
			// 
			this.labelMaxPreviewWidth.AutoSize = true;
			this.labelMaxPreviewWidth.Location = new System.Drawing.Point(12, 96);
			this.labelMaxPreviewWidth.Name = "labelMaxPreviewWidth";
			this.labelMaxPreviewWidth.Size = new System.Drawing.Size(102, 13);
			this.labelMaxPreviewWidth.TabIndex = 12;
			this.labelMaxPreviewWidth.Text = "Max Preview Width:";
			// 
			// labelVersion
			// 
			this.labelVersion.AutoSize = true;
			this.labelVersion.Location = new System.Drawing.Point(15, 161);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(0, 13);
			this.labelVersion.TabIndex = 13;
			// 
			// ConfigureForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(436, 185);
			this.Controls.Add(this.labelVersion);
			this.Controls.Add(this.labelMaxPreviewWidth);
			this.Controls.Add(this.labelMaxPreviewHeight);
			this.Controls.Add(this.labelResizeSize);
			this.Controls.Add(this.textBoxMaxPreviewWidth);
			this.Controls.Add(this.textBoxMaxPreviewHeight);
			this.Controls.Add(this.textBoxResizeSize);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnPublish);
			this.Controls.Add(this.buttonCreateNew);
			this.Controls.Add(this.labelPage);
			this.Controls.Add(this.comboBoxPages);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboBoxServer);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ConfigureForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Publish to s-church.net";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxPages;
        private System.Windows.Forms.Label labelPage;
        private System.Windows.Forms.Button buttonCreateNew;
        private System.Windows.Forms.Button btnPublish;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox textBoxResizeSize;
        private System.Windows.Forms.TextBox textBoxMaxPreviewHeight;
        private System.Windows.Forms.TextBox textBoxMaxPreviewWidth;
        private System.Windows.Forms.Label labelResizeSize;
        private System.Windows.Forms.Label labelMaxPreviewHeight;
        private System.Windows.Forms.Label labelMaxPreviewWidth;
		private System.Windows.Forms.Label labelVersion;
    }
}