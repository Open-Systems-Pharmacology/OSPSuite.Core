using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   partial class ParameterIdentificationParametersFeedbackView
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
         _parametersBinder.Dispose();
         _runPropertiesBinder.Dispose();
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
         this.gridProperties = new UxGridControl();
         this.gridViewProperties = new UxGridView();
         this.gridParameters = new UxGridControl();
         this.gridViewParameters = new UxGridView();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemParameters = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemRunProperties = new DevExpress.XtraLayout.LayoutControlItem();
         this.btnExportParametersHistory = new DevExpress.XtraEditors.SimpleButton();
         this.layoutItemExportParametersHistory = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRunProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExportParametersHistory)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.btnExportParametersHistory);
         this.layoutControl.Controls.Add(this.gridProperties);
         this.layoutControl.Controls.Add(this.gridParameters);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(618, 592);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // gridProperties
         // 
         this.gridProperties.Location = new System.Drawing.Point(125, 28);
         this.gridProperties.MainView = this.gridViewProperties;
         this.gridProperties.Name = "gridProperties";
         this.gridProperties.Size = new System.Drawing.Size(491, 20);
         this.gridProperties.TabIndex = 5;
         this.gridProperties.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewProperties});
         // 
         // gridViewProperties
         // 
         this.gridViewProperties.AllowsFiltering = true;
         this.gridViewProperties.EnableColumnContextMenu = true;
         this.gridViewProperties.GridControl = this.gridProperties;
         this.gridViewProperties.MultiSelect = false;
         this.gridViewProperties.Name = "gridViewProperties";
         this.gridViewProperties.OptionsSelection.EnableAppearanceFocusedRow = false;
         // 
         // gridParameters
         // 
         this.gridParameters.Location = new System.Drawing.Point(2, 52);
         this.gridParameters.MainView = this.gridViewParameters;
         this.gridParameters.Name = "gridParameters";
         this.gridParameters.Size = new System.Drawing.Size(614, 538);
         this.gridParameters.TabIndex = 4;
         this.gridParameters.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewParameters});
         // 
         // gridViewParameters
         // 
         this.gridViewParameters.AllowsFiltering = true;
         this.gridViewParameters.EnableColumnContextMenu = true;
         this.gridViewParameters.GridControl = this.gridParameters;
         this.gridViewParameters.MultiSelect = false;
         this.gridViewParameters.Name = "gridViewParameters";
         this.gridViewParameters.OptionsSelection.EnableAppearanceFocusedRow = false;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemParameters,
            this.layoutItemRunProperties,
            this.layoutItemExportParametersHistory,
            this.emptySpaceItem});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(618, 592);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemParameters
         // 
         this.layoutItemParameters.Control = this.gridParameters;
         this.layoutItemParameters.Location = new System.Drawing.Point(0, 50);
         this.layoutItemParameters.Name = "layoutItemParameters";
         this.layoutItemParameters.Size = new System.Drawing.Size(618, 542);
         this.layoutItemParameters.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemParameters.TextVisible = false;
         // 
         // layoutItemRunProperties
         // 
         this.layoutItemRunProperties.Control = this.gridProperties;
         this.layoutItemRunProperties.Location = new System.Drawing.Point(0, 26);
         this.layoutItemRunProperties.Name = "layoutItemRunProperties";
         this.layoutItemRunProperties.Size = new System.Drawing.Size(618, 24);
         this.layoutItemRunProperties.TextSize = new System.Drawing.Size(120, 13);
         // 
         // btnExportParametersHistory
         // 
         this.btnExportParametersHistory.Location = new System.Drawing.Point(311, 2);
         this.btnExportParametersHistory.Name = "btnExportParametersHistory";
         this.btnExportParametersHistory.Size = new System.Drawing.Size(305, 22);
         this.btnExportParametersHistory.StyleController = this.layoutControl;
         this.btnExportParametersHistory.TabIndex = 6;
         this.btnExportParametersHistory.Text = "btnExportParametersHistory";
         // 
         // layoutControlItem1
         // 
         this.layoutItemExportParametersHistory.Control = this.btnExportParametersHistory;
         this.layoutItemExportParametersHistory.Location = new System.Drawing.Point(309, 0);
         this.layoutItemExportParametersHistory.Name = "layoutItemExportParametersHistory";
         this.layoutItemExportParametersHistory.Size = new System.Drawing.Size(309, 26);
         this.layoutItemExportParametersHistory.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemExportParametersHistory.TextVisible = false;
         // 
         // emptySpaceItem
         // 
         this.emptySpaceItem.AllowHotTrack = false;
         this.emptySpaceItem.Location = new System.Drawing.Point(0, 0);
         this.emptySpaceItem.Name = "emptySpaceItem";
         this.emptySpaceItem.Size = new System.Drawing.Size(309, 26);
         this.emptySpaceItem.TextSize = new System.Drawing.Size(0, 0);
         // 
         // ParameterIdentificationParametersFeedbackView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ParameterIdentificationParametersFeedbackView";
         this.Size = new System.Drawing.Size(618, 592);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRunProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExportParametersHistory)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private UxGridControl gridParameters;
      private UxGridView gridViewParameters;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemParameters;
      private UxGridControl gridProperties;
      private UxGridView gridViewProperties;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemRunProperties;
      private DevExpress.XtraEditors.SimpleButton btnExportParametersHistory;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemExportParametersHistory;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem;
   }
}
