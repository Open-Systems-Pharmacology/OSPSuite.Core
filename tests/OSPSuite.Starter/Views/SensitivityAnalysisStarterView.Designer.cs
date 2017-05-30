namespace OSPSuite.Starter.Views
{
   partial class SensitivityAnalysisStarterView
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
         this.btnSensitivityAnalysis = new DevExpress.XtraEditors.SimpleButton();
         this.uxLayoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).BeginInit();
         this.uxLayoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // btnSensitivityAnalysis
         // 
         this.btnSensitivityAnalysis.Location = new System.Drawing.Point(12, 12);
         this.btnSensitivityAnalysis.Name = "btnSensitivityAnalysis";
         this.btnSensitivityAnalysis.Size = new System.Drawing.Size(329, 22);
         this.btnSensitivityAnalysis.StyleController = this.uxLayoutControl1;
         this.btnSensitivityAnalysis.TabIndex = 0;
         this.btnSensitivityAnalysis.Text = "btnSensitivityAnalysis";
         // 
         // uxLayoutControl1
         // 
         this.uxLayoutControl1.AllowCustomization = false;
         this.uxLayoutControl1.Controls.Add(this.btnSensitivityAnalysis);
         this.uxLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl1.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl1.Name = "uxLayoutControl1";
         this.uxLayoutControl1.Root = this.layoutControlGroup1;
         this.uxLayoutControl1.Size = new System.Drawing.Size(353, 414);
         this.uxLayoutControl1.TabIndex = 1;
         this.uxLayoutControl1.Text = "uxLayoutControl1";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(353, 414);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.btnSensitivityAnalysis;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(333, 26);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 26);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(333, 368);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // SensitivityAnalysisStarterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.uxLayoutControl1);
         this.Name = "SensitivityAnalysisStarterView";
         this.Size = new System.Drawing.Size(353, 414);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).EndInit();
         this.uxLayoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton btnSensitivityAnalysis;
      private UI.Controls.UxLayoutControl uxLayoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
