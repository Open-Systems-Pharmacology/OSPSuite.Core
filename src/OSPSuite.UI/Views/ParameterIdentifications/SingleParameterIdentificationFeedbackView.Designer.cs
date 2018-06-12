namespace OSPSuite.UI.Views.ParameterIdentifications
{
   partial class SingleParameterIdentificationFeedbackView
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
         this.panelParameters = new DevExpress.XtraEditors.PanelControl();
         this.panelErrorHistory = new DevExpress.XtraEditors.PanelControl();
         this.panelTimeProfile = new DevExpress.XtraEditors.PanelControl();
         this.panelPredictedVsObserved = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemParameters = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemErrorHistory = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemTimeProfile = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemPredictedVsObserved = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelErrorHistory)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelTimeProfile)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelPredictedVsObserved)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemErrorHistory)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTimeProfile)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPredictedVsObserved)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         this.SuspendLayout();
         // 
         // panelParameters
         // 
         this.panelParameters.Location = new System.Drawing.Point(2, 2);
         this.panelParameters.Name = "panelParameters";
         this.panelParameters.Size = new System.Drawing.Size(453, 348);
         this.panelParameters.TabIndex = 4;
         // 
         // panelErrorHistory
         // 
         this.panelErrorHistory.Location = new System.Drawing.Point(459, 2);
         this.panelErrorHistory.Name = "panelErrorHistory";
         this.panelErrorHistory.Size = new System.Drawing.Size(465, 348);
         this.panelErrorHistory.TabIndex = 5;
         // 
         // panelTimeProfile
         // 
         this.panelTimeProfile.Location = new System.Drawing.Point(459, 354);
         this.panelTimeProfile.Name = "panelTimeProfile";
         this.panelTimeProfile.Size = new System.Drawing.Size(465, 350);
         this.panelTimeProfile.TabIndex = 6;
         // 
         // panelPredictedVsObserved
         // 
         this.panelPredictedVsObserved.Location = new System.Drawing.Point(2, 354);
         this.panelPredictedVsObserved.Name = "panelPredictedVsObserved";
         this.panelPredictedVsObserved.Size = new System.Drawing.Size(453, 350);
         this.panelPredictedVsObserved.TabIndex = 7;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemParameters,
            this.layoutItemErrorHistory,
            this.layoutItemTimeProfile,
            this.layoutItemPredictedVsObserved});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "Root";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(926, 706);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemParameters
         // 
         this.layoutItemParameters.Control = this.panelParameters;
         this.layoutItemParameters.Location = new System.Drawing.Point(0, 0);
         this.layoutItemParameters.Name = "layoutItemParameters";
         this.layoutItemParameters.Size = new System.Drawing.Size(457, 352);
         this.layoutItemParameters.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemParameters.TextVisible = false;
         // 
         // layoutItemErrorHistory
         // 
         this.layoutItemErrorHistory.Control = this.panelErrorHistory;
         this.layoutItemErrorHistory.Location = new System.Drawing.Point(457, 0);
         this.layoutItemErrorHistory.Name = "layoutItemErrorHistory";
         this.layoutItemErrorHistory.Size = new System.Drawing.Size(469, 352);
         this.layoutItemErrorHistory.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemErrorHistory.TextVisible = false;
         // 
         // layoutItemTimeProfile
         // 
         this.layoutItemTimeProfile.Control = this.panelTimeProfile;
         this.layoutItemTimeProfile.Location = new System.Drawing.Point(457, 352);
         this.layoutItemTimeProfile.Name = "layoutItemTimeProfile";
         this.layoutItemTimeProfile.Size = new System.Drawing.Size(469, 354);
         this.layoutItemTimeProfile.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemTimeProfile.TextVisible = false;
         // 
         // layoutItemPredictedVsObserved
         // 
         this.layoutItemPredictedVsObserved.Control = this.panelPredictedVsObserved;
         this.layoutItemPredictedVsObserved.Location = new System.Drawing.Point(0, 352);
         this.layoutItemPredictedVsObserved.Name = "layoutItemPredictedVsObserved";
         this.layoutItemPredictedVsObserved.Size = new System.Drawing.Size(457, 354);
         this.layoutItemPredictedVsObserved.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemPredictedVsObserved.TextVisible = false;
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.panelPredictedVsObserved);
         this.layoutControl.Controls.Add(this.panelTimeProfile);
         this.layoutControl.Controls.Add(this.panelErrorHistory);
         this.layoutControl.Controls.Add(this.panelParameters);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(926, 706);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // SingleParameterIdentificationFeedbackView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "SingleParameterIdentificationFeedbackView";
         this.Controls.Add(this.layoutControl);
         this.Name = "SingleParameterIdentificationFeedbackView";
         this.Size = new System.Drawing.Size(926, 706);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelErrorHistory)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelTimeProfile)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelPredictedVsObserved)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemErrorHistory)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTimeProfile)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPredictedVsObserved)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemPredictedVsObserved;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemTimeProfile;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemErrorHistory;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemParameters;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraEditors.PanelControl panelPredictedVsObserved;
      private DevExpress.XtraEditors.PanelControl panelTimeProfile;
      private DevExpress.XtraEditors.PanelControl panelErrorHistory;
      private DevExpress.XtraEditors.PanelControl panelParameters;
   }
}
