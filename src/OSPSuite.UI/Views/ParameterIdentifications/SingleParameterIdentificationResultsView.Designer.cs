using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   partial class SingleParameterIdentificationResultsView
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
         _gridViewBinder.Dispose();
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnTransferToSimulations = new DevExpress.XtraEditors.SimpleButton();
         this.panelProperties = new DevExpress.XtraEditors.PanelControl();
         this.gridParameters = new UxGridControl();
         this.gridView = new UxGridView();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemOptimizedParameters = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupRunResultProperties = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemProperties = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemButtonTransfer = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOptimizedParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupRunResultProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonTransfer)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.btnTransferToSimulations);
         this.layoutControl.Controls.Add(this.panelProperties);
         this.layoutControl.Controls.Add(this.gridParameters);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(737, 207, 250, 350);
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(434, 436);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl";
         // 
         // btnTransferToSimulations
         // 
         this.btnTransferToSimulations.Location = new System.Drawing.Point(219, 2);
         this.btnTransferToSimulations.Name = "btnTransferToSimulations";
         this.btnTransferToSimulations.Size = new System.Drawing.Size(213, 22);
         this.btnTransferToSimulations.StyleController = this.layoutControl;
         this.btnTransferToSimulations.TabIndex = 6;
         this.btnTransferToSimulations.Text = "btnTransferToSimulations";
         // 
         // panelProperties
         // 
         this.panelProperties.Location = new System.Drawing.Point(14, 58);
         this.panelProperties.Name = "panelProperties";
         this.panelProperties.Size = new System.Drawing.Size(406, 98);
         this.panelProperties.TabIndex = 5;
         // 
         // gridParameters
         // 
         this.gridParameters.Location = new System.Drawing.Point(159, 172);
         this.gridParameters.MainView = this.gridView;
         this.gridParameters.Name = "gridParameters";
         this.gridParameters.Size = new System.Drawing.Size(273, 262);
         this.gridParameters.TabIndex = 4;
         this.gridParameters.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.EnableColumnContextMenu = true;
         this.gridView.GridControl = this.gridParameters;
         this.gridView.MultiSelect = false;
         this.gridView.Name = "gridView";
         this.gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemOptimizedParameters,
            this.layoutGroupRunResultProperties,
            this.layoutItemButtonTransfer,
            this.emptySpaceItem1});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "Root";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(434, 436);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemOptimizedParameters
         // 
         this.layoutItemOptimizedParameters.Control = this.gridParameters;
         this.layoutItemOptimizedParameters.Location = new System.Drawing.Point(0, 170);
         this.layoutItemOptimizedParameters.Name = "layoutItemOptimizedParameters";
         this.layoutItemOptimizedParameters.Size = new System.Drawing.Size(434, 266);
         this.layoutItemOptimizedParameters.TextSize = new System.Drawing.Size(154, 13);
         // 
         // layoutGroupRunResultProperties
         // 
         this.layoutGroupRunResultProperties.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemProperties});
         this.layoutGroupRunResultProperties.Location = new System.Drawing.Point(0, 26);
         this.layoutGroupRunResultProperties.Name = "layoutGroupRunResultProperties";
         this.layoutGroupRunResultProperties.Size = new System.Drawing.Size(434, 144);
         // 
         // layoutItemProperties
         // 
         this.layoutItemProperties.Control = this.panelProperties;
         this.layoutItemProperties.Location = new System.Drawing.Point(0, 0);
         this.layoutItemProperties.Name = "layoutItemProperties";
         this.layoutItemProperties.Size = new System.Drawing.Size(410, 102);
         this.layoutItemProperties.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemProperties.TextVisible = false;
         // 
         // layoutItemButtonTransfer
         // 
         this.layoutItemButtonTransfer.Control = this.btnTransferToSimulations;
         this.layoutItemButtonTransfer.Location = new System.Drawing.Point(217, 0);
         this.layoutItemButtonTransfer.Name = "layoutItemButtonTransfer";
         this.layoutItemButtonTransfer.Size = new System.Drawing.Size(217, 26);
         this.layoutItemButtonTransfer.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemButtonTransfer.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem";
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(217, 26);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // SingleParameterIdentificationResultsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "SingleParameterIdentificationResultsView";
         this.Size = new System.Drawing.Size(434, 436);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOptimizedParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupRunResultProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonTransfer)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private UxGridControl gridParameters;
      private UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemOptimizedParameters;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupRunResultProperties;
      private DevExpress.XtraEditors.PanelControl panelProperties;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemProperties;
      private DevExpress.XtraEditors.SimpleButton btnTransferToSimulations;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemButtonTransfer;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
