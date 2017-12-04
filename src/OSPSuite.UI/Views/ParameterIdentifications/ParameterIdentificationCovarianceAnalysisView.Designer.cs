namespace OSPSuite.UI.Views.ParameterIdentifications
{
   partial class ParameterIdentificationCovarianceAnalysisView
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
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.panelConfidenceInterval = new DevExpress.XtraEditors.PanelControl();
         this.panelMatrix = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemMatrix = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemConfidenceInterval = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelConfidenceInterval)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelMatrix)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMatrix)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemConfidenceInterval)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.panelConfidenceInterval);
         this.layoutControl.Controls.Add(this.panelMatrix);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(696, 570);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // panelConfidenceInterval
         // 
         this.panelConfidenceInterval.Location = new System.Drawing.Point(0, 304);
         this.panelConfidenceInterval.Name = "panelConfidenceInterval";
         this.panelConfidenceInterval.Size = new System.Drawing.Size(696, 266);
         this.panelConfidenceInterval.TabIndex = 5;
         // 
         // panelMatrix
         // 
         this.panelMatrix.Location = new System.Drawing.Point(0, 0);
         this.panelMatrix.Name = "panelMatrix";
         this.panelMatrix.Size = new System.Drawing.Size(696, 304);
         this.panelMatrix.TabIndex = 4;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemMatrix,
            this.layoutItemConfidenceInterval});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(696, 570);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemMatrix
         // 
         this.layoutItemMatrix.Control = this.panelMatrix;
         this.layoutItemMatrix.Location = new System.Drawing.Point(0, 0);
         this.layoutItemMatrix.Name = "layoutItemMatrix";
         this.layoutItemMatrix.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutItemMatrix.Size = new System.Drawing.Size(696, 304);
         this.layoutItemMatrix.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemMatrix.TextVisible = false;
         // 
         // layoutItemConfidenceInterval
         // 
         this.layoutItemConfidenceInterval.Control = this.panelConfidenceInterval;
         this.layoutItemConfidenceInterval.Location = new System.Drawing.Point(0, 304);
         this.layoutItemConfidenceInterval.Name = "layoutItemConfidenceInterval";
         this.layoutItemConfidenceInterval.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutItemConfidenceInterval.Size = new System.Drawing.Size(696, 266);
         this.layoutItemConfidenceInterval.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemConfidenceInterval.TextVisible = false;
         // 
         // ParameterIdentificationCovarianceAnalysisView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ParameterIdentificationCovarianceAnalysisView";
         this.Size = new System.Drawing.Size(696, 570);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelConfidenceInterval)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelMatrix)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMatrix)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemConfidenceInterval)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraEditors.PanelControl panelConfidenceInterval;
      private DevExpress.XtraEditors.PanelControl panelMatrix;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemMatrix;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemConfidenceInterval;
   }
}
