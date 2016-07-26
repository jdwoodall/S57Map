namespace S57Map
{
    partial class S57Options
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise", false.</param>
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
            this.gBoxDebug = new System.Windows.Forms.GroupBox();
            this.cBoxConsoleOut = new System.Windows.Forms.CheckBox();
            this.cBoxTreeOut = new System.Windows.Forms.CheckBox();
            this.cBoxS57Out = new System.Windows.Forms.CheckBox();
            this.cBoxDebug = new System.Windows.Forms.CheckBox();
            this.btnOptionsOK = new System.Windows.Forms.Button();
            this.gBoxDebug.SuspendLayout();
            this.SuspendLayout();
            // 
            // gBoxDebug
            // 
            this.gBoxDebug.Controls.Add(this.cBoxConsoleOut);
            this.gBoxDebug.Controls.Add(this.cBoxTreeOut);
            this.gBoxDebug.Controls.Add(this.cBoxS57Out);
            this.gBoxDebug.Controls.Add(this.cBoxDebug);
            this.gBoxDebug.Location = new System.Drawing.Point(19, 15);
            this.gBoxDebug.Name = "gBoxDebug";
            this.gBoxDebug.Size = new System.Drawing.Size(161, 178);
            this.gBoxDebug.TabIndex = 0;
            this.gBoxDebug.TabStop = false;
            this.gBoxDebug.Text = "Debug Options";
            // 
            // cBoxConsoleOut
            // 
            this.cBoxConsoleOut.AutoSize = true;
            this.cBoxConsoleOut.Location = new System.Drawing.Point(45, 134);
            this.cBoxConsoleOut.Name = "cBoxConsoleOut";
            this.cBoxConsoleOut.Size = new System.Drawing.Size(99, 17);
            this.cBoxConsoleOut.TabIndex = 3;
            this.cBoxConsoleOut.Text = "Console Output";
            this.cBoxConsoleOut.UseVisualStyleBackColor = true;
            this.cBoxConsoleOut.CheckedChanged += new System.EventHandler(this.cBoxConsoleOut_CheckedChanged);
            // 
            // cBoxTreeOut
            // 
            this.cBoxTreeOut.AutoSize = true;
            this.cBoxTreeOut.Location = new System.Drawing.Point(45, 99);
            this.cBoxTreeOut.Name = "cBoxTreeOut";
            this.cBoxTreeOut.Size = new System.Drawing.Size(107, 17);
            this.cBoxTreeOut.TabIndex = 2;
            this.cBoxTreeOut.Text = "Tree Text Output";
            this.cBoxTreeOut.UseVisualStyleBackColor = true;
            this.cBoxTreeOut.CheckedChanged += new System.EventHandler(this.cBoxTreeOut_CheckedChanged);
            // 
            // cBoxS57Out
            // 
            this.cBoxS57Out.AutoSize = true;
            this.cBoxS57Out.Location = new System.Drawing.Point(45, 64);
            this.cBoxS57Out.Name = "cBoxS57Out";
            this.cBoxS57Out.Size = new System.Drawing.Size(107, 17);
            this.cBoxS57Out.TabIndex = 1;
            this.cBoxS57Out.Text = "S-57 Text Output";
            this.cBoxS57Out.UseVisualStyleBackColor = true;
            this.cBoxS57Out.CheckedChanged += new System.EventHandler(this.cBoxS57Out_CheckedChanged);
            // 
            // cBoxDebug
            // 
            this.cBoxDebug.AutoSize = true;
            this.cBoxDebug.Location = new System.Drawing.Point(17, 30);
            this.cBoxDebug.Name = "cBoxDebug";
            this.cBoxDebug.Size = new System.Drawing.Size(94, 17);
            this.cBoxDebug.TabIndex = 0;
            this.cBoxDebug.Text = "Debug Enable";
            this.cBoxDebug.UseVisualStyleBackColor = true;
            this.cBoxDebug.CheckedChanged += new System.EventHandler(this.cBoxDebug_CheckedChanged);
            // 
            // btnOptionsOK
            // 
            this.btnOptionsOK.Location = new System.Drawing.Point(590, 356);
            this.btnOptionsOK.Name = "btnOptionsOK";
            this.btnOptionsOK.Size = new System.Drawing.Size(75, 23);
            this.btnOptionsOK.TabIndex = 1;
            this.btnOptionsOK.Text = "Done";
            this.btnOptionsOK.UseVisualStyleBackColor = true;
            this.btnOptionsOK.Click += new System.EventHandler(this.btnOptionsOK_Click);
            // 
            // S57Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 400);
            this.Controls.Add(this.btnOptionsOK);
            this.Controls.Add(this.gBoxDebug);
            this.Name = "S57Options";
            this.Text = "SharpMap S-57 Options";
            this.gBoxDebug.ResumeLayout(false);
            this.gBoxDebug.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gBoxDebug;
        private System.Windows.Forms.CheckBox cBoxConsoleOut;
        private System.Windows.Forms.CheckBox cBoxTreeOut;
        private System.Windows.Forms.CheckBox cBoxS57Out;
        private System.Windows.Forms.CheckBox cBoxDebug;
        private System.Windows.Forms.Button btnOptionsOK;
    }
}