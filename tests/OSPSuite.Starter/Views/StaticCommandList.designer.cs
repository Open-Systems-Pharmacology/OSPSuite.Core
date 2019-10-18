namespace OSPSuite.Starter.Views
{
    partial class StaticCommandList
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
           this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
           this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
           this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
           this.btnCreateReport = new DevExpress.XtraEditors.SimpleButton();
           ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
           this.SuspendLayout();
           // 
           // simpleButton1
           // 
           this.simpleButton1.Location = new System.Drawing.Point(25, 23);
           this.simpleButton1.Name = "simpleButton1";
           this.simpleButton1.Size = new System.Drawing.Size(75, 23);
           this.simpleButton1.TabIndex = 0;
           this.simpleButton1.Text = "Show Browser";
           this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
           // 
           // panelControl1
           // 
           this.panelControl1.Location = new System.Drawing.Point(25, 80);
           this.panelControl1.Name = "panelControl1";
           this.panelControl1.Size = new System.Drawing.Size(542, 301);
           this.panelControl1.TabIndex = 1;
           // 
           // simpleButton2
           // 
           this.simpleButton2.Location = new System.Drawing.Point(129, 23);
           this.simpleButton2.Name = "simpleButton2";
           this.simpleButton2.Size = new System.Drawing.Size(75, 23);
           this.simpleButton2.TabIndex = 2;
           this.simpleButton2.Text = "Undo";
           this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
           // 
           // StaticCommandList
           // 
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.ClientSize = new System.Drawing.Size(579, 393);
           this.Controls.Add(this.btnCreateReport);
           this.Controls.Add(this.simpleButton2);
           this.Controls.Add(this.panelControl1);
           this.Controls.Add(this.simpleButton1);
           this.Name = "StaticCommandList";
           this.Text = "StaticCommandList";
           ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
           this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton btnCreateReport;
    }
}