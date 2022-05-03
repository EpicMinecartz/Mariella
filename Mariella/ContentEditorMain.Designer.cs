namespace Mariella {
    partial class ContentEditorMain {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.contentUp = new System.Windows.Forms.Button();
            this.contentDown = new System.Windows.Forms.Button();
            this.contentRemove = new System.Windows.Forms.Button();
            this.contentAdd = new System.Windows.Forms.Button();
            this.listBoxContent = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.contentUp);
            this.splitContainer1.Panel1.Controls.Add(this.contentDown);
            this.splitContainer1.Panel1.Controls.Add(this.contentRemove);
            this.splitContainer1.Panel1.Controls.Add(this.contentAdd);
            this.splitContainer1.Panel1.Controls.Add(this.listBoxContent);
            this.splitContainer1.Size = new System.Drawing.Size(1043, 496);
            this.splitContainer1.SplitterDistance = 330;
            this.splitContainer1.TabIndex = 1;
            // 
            // contentUp
            // 
            this.contentUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.contentUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.contentUp.ForeColor = System.Drawing.Color.Silver;
            this.contentUp.Location = new System.Drawing.Point(247, 468);
            this.contentUp.Name = "contentUp";
            this.contentUp.Size = new System.Drawing.Size(75, 23);
            this.contentUp.TabIndex = 4;
            this.contentUp.Text = "Move Up";
            this.contentUp.UseVisualStyleBackColor = true;
            this.contentUp.Visible = false;
            // 
            // contentDown
            // 
            this.contentDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.contentDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.contentDown.ForeColor = System.Drawing.Color.Silver;
            this.contentDown.Location = new System.Drawing.Point(166, 468);
            this.contentDown.Name = "contentDown";
            this.contentDown.Size = new System.Drawing.Size(75, 23);
            this.contentDown.TabIndex = 3;
            this.contentDown.Text = "Move Down";
            this.contentDown.UseVisualStyleBackColor = true;
            this.contentDown.Visible = false;
            // 
            // contentRemove
            // 
            this.contentRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.contentRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.contentRemove.ForeColor = System.Drawing.Color.Red;
            this.contentRemove.Location = new System.Drawing.Point(85, 468);
            this.contentRemove.Name = "contentRemove";
            this.contentRemove.Size = new System.Drawing.Size(75, 23);
            this.contentRemove.TabIndex = 2;
            this.contentRemove.Text = "Remove";
            this.contentRemove.UseVisualStyleBackColor = true;
            this.contentRemove.Visible = false;
            this.contentRemove.Click += new System.EventHandler(this.contentRemove_Click);
            // 
            // contentAdd
            // 
            this.contentAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.contentAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.contentAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.contentAdd.Location = new System.Drawing.Point(4, 468);
            this.contentAdd.Name = "contentAdd";
            this.contentAdd.Size = new System.Drawing.Size(75, 23);
            this.contentAdd.TabIndex = 1;
            this.contentAdd.Text = "Add";
            this.contentAdd.UseVisualStyleBackColor = true;
            this.contentAdd.Click += new System.EventHandler(this.contentAdd_Click);
            // 
            // listBoxContent
            // 
            this.listBoxContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.listBoxContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxContent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.listBoxContent.FormattingEnabled = true;
            this.listBoxContent.IntegralHeight = false;
            this.listBoxContent.Location = new System.Drawing.Point(0, 0);
            this.listBoxContent.Name = "listBoxContent";
            this.listBoxContent.Size = new System.Drawing.Size(328, 462);
            this.listBoxContent.TabIndex = 0;
            this.listBoxContent.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // ContentEditorMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ContentEditorMain";
            this.Size = new System.Drawing.Size(1043, 496);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button contentUp;
        private System.Windows.Forms.Button contentDown;
        private System.Windows.Forms.Button contentRemove;
        private System.Windows.Forms.Button contentAdd;
        private System.Windows.Forms.ListBox listBoxContent;
    }
}
