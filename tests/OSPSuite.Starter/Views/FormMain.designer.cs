namespace OSPSuite.Starter.Views
{
    partial class FormMain
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
         this.components = new System.ComponentModel.Container();
         this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
         this.tbValue = new DevExpress.XtraEditors.TextEdit();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         ((System.ComponentModel.ISupportInitialize)(this.tbValue.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // defaultLookAndFeel1
         // 
         this.defaultLookAndFeel1.LookAndFeel.SkinName = "Black";
         // 
         // tbValue
         // 
         this.tbValue.Location = new System.Drawing.Point(170, 44);
         this.tbValue.Name = "tbValue";
         this.tbValue.Size = new System.Drawing.Size(100, 20);
         this.tbValue.TabIndex = 2;
         // 
         // labelControl1
         // 
         this.labelControl1.Location = new System.Drawing.Point(13, 47);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(83, 13);
         this.labelControl1.TabIndex = 3;
         this.labelControl1.Text = "Parameter Value:";
         // 
         // FormMain
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(418, 346);
         this.Controls.Add(this.labelControl1);
         this.Controls.Add(this.tbValue);
         this.Name = "FormMain";
         this.Text = "Form1";
         ((System.ComponentModel.ISupportInitialize)(this.tbValue.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraEditors.TextEdit tbValue;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}

