
namespace OSPSuite.UI.Views.SensitivityAnalyses
{
   partial class SensitivityAnalysisFeedbackView
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
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.progressBarControl = new DevExpress.XtraEditors.ProgressBarControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemProgressBar = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.progressBarControl.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemProgressBar)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.progressBarControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(533, 108);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // progressBarControl
         // 
         this.progressBarControl.Location = new System.Drawing.Point(160, 12);
         this.progressBarControl.Name = "progressBarControl";
         this.progressBarControl.Properties.ShowTitle = true;
         this.progressBarControl.Size = new System.Drawing.Size(361, 18);
         this.progressBarControl.StyleController = this.layoutControl;
         this.progressBarControl.TabIndex = 4;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemProgressBar});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Size = new System.Drawing.Size(533, 108);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutControlItemProgressBar
         // 
         this.layoutControlItemProgressBar.Control = this.progressBarControl;
         this.layoutControlItemProgressBar.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemProgressBar.Name = "layoutControlItemProgressBar";
         this.layoutControlItemProgressBar.Size = new System.Drawing.Size(513, 88);
         this.layoutControlItemProgressBar.TextSize = new System.Drawing.Size(145, 13);
         // 
         // SensitivityAnalysisFeedbackView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "SensitivityAnalysisFeedbackView";
         this.ClientSize = new System.Drawing.Size(533, 108);
         this.Controls.Add(this.layoutControl);
         this.Name = "SensitivityAnalysisFeedbackView";
         this.Text = "SensitivityAnalysisFeedbackView";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.progressBarControl.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemProgressBar)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraEditors.ProgressBarControl progressBarControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemProgressBar;
   }
}