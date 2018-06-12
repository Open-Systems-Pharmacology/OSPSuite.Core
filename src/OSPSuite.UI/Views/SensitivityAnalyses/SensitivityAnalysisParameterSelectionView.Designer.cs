namespace OSPSuite.UI.Views.SensitivityAnalyses
{
   partial class SensitivityAnalysisParameterSelectionView
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
         _screenBinder.Dispose();
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
         this.btnAddAllConstantParameters = new DevExpress.XtraEditors.SimpleButton();
         this.pnlParameterSelection = new DevExpress.XtraEditors.PanelControl();
         this.cbSimulationSelector = new DevExpress.XtraEditors.ImageComboBoxEdit();
         this.btnRemoveParameters = new DevExpress.XtraEditors.SimpleButton();
         this.btnAddParameters = new DevExpress.XtraEditors.SimpleButton();
         this.pnlSensitivityParameters = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.sensitivityParametersLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemAddParameters = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemRemoveParameters = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.parameterSelectionGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.simulationSelectionPanelLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemSelectSimulation = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutItemAddAllConstantParameters = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pnlParameterSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbSimulationSelector.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pnlSensitivityParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sensitivityParametersLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRemoveParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.parameterSelectionGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.simulationSelectionPanelLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSelectSimulation)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddAllConstantParameters)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.btnAddAllConstantParameters);
         this.layoutControl.Controls.Add(this.pnlParameterSelection);
         this.layoutControl.Controls.Add(this.cbSimulationSelector);
         this.layoutControl.Controls.Add(this.btnRemoveParameters);
         this.layoutControl.Controls.Add(this.btnAddParameters);
         this.layoutControl.Controls.Add(this.pnlSensitivityParameters);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1104, 117, 731, 696);
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(700, 439);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // btnAddAllConstantParameters
         // 
         this.btnAddAllConstantParameters.Location = new System.Drawing.Point(211, 99);
         this.btnAddAllConstantParameters.Name = "btnAddAllConstantParameters";
         this.btnAddAllConstantParameters.Size = new System.Drawing.Size(145, 22);
         this.btnAddAllConstantParameters.StyleController = this.layoutControl;
         this.btnAddAllConstantParameters.TabIndex = 9;
         this.btnAddAllConstantParameters.Text = "btnAddConstantParameters";
         // 
         // pnlParameterSelection
         // 
         this.pnlParameterSelection.Location = new System.Drawing.Point(10, 34);
         this.pnlParameterSelection.Name = "pnlParameterSelection";
         this.pnlParameterSelection.Size = new System.Drawing.Size(194, 395);
         this.pnlParameterSelection.TabIndex = 8;
         // 
         // cbSimulationSelector
         // 
         this.cbSimulationSelector.Location = new System.Drawing.Point(12, 12);
         this.cbSimulationSelector.Name = "cbSimulationSelector";
         this.cbSimulationSelector.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbSimulationSelector.Size = new System.Drawing.Size(190, 20);
         this.cbSimulationSelector.StyleController = this.layoutControl;
         this.cbSimulationSelector.TabIndex = 7;
         // 
         // btnRemoveParameters
         // 
         this.btnRemoveParameters.Location = new System.Drawing.Point(211, 232);
         this.btnRemoveParameters.Name = "btnRemoveParameters";
         this.btnRemoveParameters.Size = new System.Drawing.Size(145, 22);
         this.btnRemoveParameters.StyleController = this.layoutControl;
         this.btnRemoveParameters.TabIndex = 6;
         this.btnRemoveParameters.Text = "btnRemoveParameters";
         // 
         // btnAddParameters
         // 
         this.btnAddParameters.Location = new System.Drawing.Point(211, 206);
         this.btnAddParameters.Name = "btnAddParameters";
         this.btnAddParameters.Size = new System.Drawing.Size(145, 22);
         this.btnAddParameters.StyleController = this.layoutControl;
         this.btnAddParameters.TabIndex = 5;
         this.btnAddParameters.Text = "btnAddParameters";
         // 
         // pnlSensitivityParameters
         // 
         this.pnlSensitivityParameters.Location = new System.Drawing.Point(360, 12);
         this.pnlSensitivityParameters.Name = "pnlSensitivityParameters";
         this.pnlSensitivityParameters.Size = new System.Drawing.Size(328, 415);
         this.pnlSensitivityParameters.TabIndex = 0;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.sensitivityParametersLayoutItem,
            this.layoutItemAddParameters,
            this.layoutItemRemoveParameters,
            this.emptySpaceItem1,
            this.emptySpaceItem2,
            this.splitterItem1,
            this.parameterSelectionGroup,
            this.emptySpaceItem3,
            this.layoutItemAddAllConstantParameters});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "Root";
         this.layoutControlGroup.Size = new System.Drawing.Size(700, 439);
         this.layoutControlGroup.TextVisible = false;
         // 
         // sensitivityParametersLayoutItem
         // 
         this.sensitivityParametersLayoutItem.Control = this.pnlSensitivityParameters;
         this.sensitivityParametersLayoutItem.Location = new System.Drawing.Point(348, 0);
         this.sensitivityParametersLayoutItem.Name = "sensitivityParametersLayoutItem";
         this.sensitivityParametersLayoutItem.Size = new System.Drawing.Size(332, 419);
         this.sensitivityParametersLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.sensitivityParametersLayoutItem.TextVisible = false;
         // 
         // layoutItemAddParameters
         // 
         this.layoutItemAddParameters.Control = this.btnAddParameters;
         this.layoutItemAddParameters.Location = new System.Drawing.Point(199, 194);
         this.layoutItemAddParameters.Name = "layoutItemAddParameters";
         this.layoutItemAddParameters.Size = new System.Drawing.Size(149, 26);
         this.layoutItemAddParameters.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemAddParameters.TextVisible = false;
         // 
         // layoutItemRemoveParameters
         // 
         this.layoutItemRemoveParameters.Control = this.btnRemoveParameters;
         this.layoutItemRemoveParameters.Location = new System.Drawing.Point(199, 220);
         this.layoutItemRemoveParameters.Name = "layoutItemRemoveParameters";
         this.layoutItemRemoveParameters.Size = new System.Drawing.Size(149, 26);
         this.layoutItemRemoveParameters.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemRemoveParameters.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(199, 113);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(149, 81);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(199, 246);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(149, 173);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(194, 0);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(5, 419);
         // 
         // parameterSelectionGroup
         // 
         this.parameterSelectionGroup.GroupBordersVisible = false;
         this.parameterSelectionGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.simulationSelectionPanelLayoutItem,
            this.layoutItemSelectSimulation});
         this.parameterSelectionGroup.Location = new System.Drawing.Point(0, 0);
         this.parameterSelectionGroup.Name = "parameterSelectionGroup";
         this.parameterSelectionGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.parameterSelectionGroup.Size = new System.Drawing.Size(194, 419);
         this.parameterSelectionGroup.TextVisible = false;
         // 
         // simulationSelectionPanelLayoutItem
         // 
         this.simulationSelectionPanelLayoutItem.Control = this.pnlParameterSelection;
         this.simulationSelectionPanelLayoutItem.Location = new System.Drawing.Point(0, 24);
         this.simulationSelectionPanelLayoutItem.Name = "simulationSelectionPanelLayoutItem";
         this.simulationSelectionPanelLayoutItem.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.simulationSelectionPanelLayoutItem.Size = new System.Drawing.Size(194, 395);
         this.simulationSelectionPanelLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.simulationSelectionPanelLayoutItem.TextVisible = false;
         // 
         // layoutItemSelectSimulation
         // 
         this.layoutItemSelectSimulation.Control = this.cbSimulationSelector;
         this.layoutItemSelectSimulation.Location = new System.Drawing.Point(0, 0);
         this.layoutItemSelectSimulation.Name = "layoutItemSelectSimulation";
         this.layoutItemSelectSimulation.Size = new System.Drawing.Size(194, 24);
         this.layoutItemSelectSimulation.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemSelectSimulation.TextVisible = false;
         // 
         // emptySpaceItem3
         // 
         this.emptySpaceItem3.AllowHotTrack = false;
         this.emptySpaceItem3.Location = new System.Drawing.Point(199, 0);
         this.emptySpaceItem3.Name = "emptySpaceItem3";
         this.emptySpaceItem3.Size = new System.Drawing.Size(149, 87);
         this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutItemAddAllConstantParameters
         // 
         this.layoutItemAddAllConstantParameters.Control = this.btnAddAllConstantParameters;
         this.layoutItemAddAllConstantParameters.Location = new System.Drawing.Point(199, 87);
         this.layoutItemAddAllConstantParameters.Name = "layoutItemAddAllConstantParameters";
         this.layoutItemAddAllConstantParameters.Size = new System.Drawing.Size(149, 26);
         this.layoutItemAddAllConstantParameters.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemAddAllConstantParameters.TextVisible = false;
         // 
         // SensitivityAnalysisParameterSelectionView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "SensitivityAnalysisParameterSelectionView";
         this.Size = new System.Drawing.Size(700, 439);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.pnlParameterSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbSimulationSelector.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pnlSensitivityParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sensitivityParametersLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRemoveParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.parameterSelectionGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.simulationSelectionPanelLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSelectSimulation)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddAllConstantParameters)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraEditors.PanelControl pnlSensitivityParameters;
      private DevExpress.XtraLayout.LayoutControlItem sensitivityParametersLayoutItem;
      private DevExpress.XtraEditors.SimpleButton btnRemoveParameters;
      private DevExpress.XtraEditors.SimpleButton btnAddParameters;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemAddParameters;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemRemoveParameters;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.ImageComboBoxEdit cbSimulationSelector;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSelectSimulation;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraEditors.PanelControl pnlParameterSelection;
      private DevExpress.XtraLayout.LayoutControlItem simulationSelectionPanelLayoutItem;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraLayout.LayoutControlGroup parameterSelectionGroup;
      private DevExpress.XtraEditors.SimpleButton btnAddAllConstantParameters;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemAddAllConstantParameters;
   }
}
