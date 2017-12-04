namespace OSPSuite.UI.Views.ParameterIdentifications
{
   partial class ParameterIdentificationConfigurationView
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
         _presenterBinder.Dispose();
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
         this.chkCalculateJacobian = new OSPSuite.UI.Controls.UxCheckEdit();
         this.lblLLOQUsageDescription = new DevExpress.XtraEditors.LabelControl();
         this.lblLLOQModeDescription = new DevExpress.XtraEditors.LabelControl();
         this.cbOptionSelection = new DevExpress.XtraEditors.ComboBoxEdit();
         this.panelOptions = new DevExpress.XtraEditors.PanelControl();
         this.cbAlgorithm = new DevExpress.XtraEditors.ComboBoxEdit();
         this.cbLLOQMode = new DevExpress.XtraEditors.ComboBoxEdit();
         this.cbLLOQUsage = new DevExpress.XtraEditors.ComboBoxEdit();
         this.panelAlgorithmProperties = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutGroupGeneral = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemLLOQMode = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemRemoveLLOQMode = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemAlgorithmSelection = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemAlgorithmOptions = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemLLOQModeDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemLLOQUsageDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemCalculateJacobian = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupOptions = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemOptionSelection = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemOptionPanel = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.chkCalculateJacobian.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbOptionSelection.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelOptions)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbAlgorithm.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbLLOQMode.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbLLOQUsage.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelAlgorithmProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupGeneral)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLLOQMode)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRemoveLLOQMode)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAlgorithmSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAlgorithmOptions)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLLOQModeDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLLOQUsageDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCalculateJacobian)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupOptions)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOptionSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOptionPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.chkCalculateJacobian);
         this.layoutControl.Controls.Add(this.lblLLOQUsageDescription);
         this.layoutControl.Controls.Add(this.lblLLOQModeDescription);
         this.layoutControl.Controls.Add(this.cbOptionSelection);
         this.layoutControl.Controls.Add(this.panelOptions);
         this.layoutControl.Controls.Add(this.cbAlgorithm);
         this.layoutControl.Controls.Add(this.cbLLOQMode);
         this.layoutControl.Controls.Add(this.cbLLOQUsage);
         this.layoutControl.Controls.Add(this.panelAlgorithmProperties);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(667, 356, 250, 350);
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(1155, 807);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // chkCalculateJacobian
         // 
         this.chkCalculateJacobian.Location = new System.Drawing.Point(24, 148);
         this.chkCalculateJacobian.Name = "chkCalculateJacobian";
         this.chkCalculateJacobian.Properties.Caption = "chkCalculateJacobian";
         this.chkCalculateJacobian.Size = new System.Drawing.Size(1107, 19);
         this.chkCalculateJacobian.StyleController = this.layoutControl;
         this.chkCalculateJacobian.TabIndex = 11;
         // 
         // lblLLOQUsageDescription
         // 
         this.lblLLOQUsageDescription.Location = new System.Drawing.Point(24, 107);
         this.lblLLOQUsageDescription.Name = "lblLLOQUsageDescription";
         this.lblLLOQUsageDescription.Size = new System.Drawing.Size(119, 13);
         this.lblLLOQUsageDescription.StyleController = this.layoutControl;
         this.lblLLOQUsageDescription.TabIndex = 10;
         this.lblLLOQUsageDescription.Text = "lblLLOQUsageDescription";
         // 
         // lblLLOQModeDescription
         // 
         this.lblLLOQModeDescription.Location = new System.Drawing.Point(24, 66);
         this.lblLLOQModeDescription.Name = "lblLLOQModeDescription";
         this.lblLLOQModeDescription.Size = new System.Drawing.Size(115, 13);
         this.lblLLOQModeDescription.StyleController = this.layoutControl;
         this.lblLLOQModeDescription.TabIndex = 9;
         this.lblLLOQModeDescription.Text = "lblLLOQModeDescription";
         // 
         // cbOptionSelection
         // 
         this.cbOptionSelection.Location = new System.Drawing.Point(170, 499);
         this.cbOptionSelection.Name = "cbOptionSelection";
         this.cbOptionSelection.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbOptionSelection.Size = new System.Drawing.Size(961, 20);
         this.cbOptionSelection.StyleController = this.layoutControl;
         this.cbOptionSelection.TabIndex = 4;
         // 
         // panelOptions
         // 
         this.panelOptions.Location = new System.Drawing.Point(170, 523);
         this.panelOptions.Name = "panelOptions";
         this.panelOptions.Size = new System.Drawing.Size(961, 260);
         this.panelOptions.TabIndex = 5;
         // 
         // cbAlgorithm
         // 
         this.cbAlgorithm.Location = new System.Drawing.Point(170, 124);
         this.cbAlgorithm.Name = "cbAlgorithm";
         this.cbAlgorithm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbAlgorithm.Size = new System.Drawing.Size(961, 20);
         this.cbAlgorithm.StyleController = this.layoutControl;
         this.cbAlgorithm.TabIndex = 7;
         // 
         // cbLLOQMode
         // 
         this.cbLLOQMode.Location = new System.Drawing.Point(170, 42);
         this.cbLLOQMode.Name = "cbLLOQMode";
         this.cbLLOQMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbLLOQMode.Size = new System.Drawing.Size(961, 20);
         this.cbLLOQMode.StyleController = this.layoutControl;
         this.cbLLOQMode.TabIndex = 4;
         // 
         // cbLLOQUsage
         // 
         this.cbLLOQUsage.Location = new System.Drawing.Point(170, 83);
         this.cbLLOQUsage.Name = "cbLLOQUsage";
         this.cbLLOQUsage.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbLLOQUsage.Size = new System.Drawing.Size(961, 20);
         this.cbLLOQUsage.StyleController = this.layoutControl;
         this.cbLLOQUsage.TabIndex = 5;
         // 
         // panelAlgorithmProperties
         // 
         this.panelAlgorithmProperties.Location = new System.Drawing.Point(170, 171);
         this.panelAlgorithmProperties.Name = "panelAlgorithmProperties";
         this.panelAlgorithmProperties.Size = new System.Drawing.Size(961, 277);
         this.panelAlgorithmProperties.TabIndex = 8;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutGroupGeneral,
            this.layoutGroupOptions,
            this.splitterItem1});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "Root";
         this.layoutControlGroup.Size = new System.Drawing.Size(1155, 807);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutGroupGeneral
         // 
         this.layoutGroupGeneral.ExpandButtonVisible = true;
         this.layoutGroupGeneral.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemLLOQMode,
            this.layoutItemRemoveLLOQMode,
            this.layoutItemAlgorithmSelection,
            this.layoutItemAlgorithmOptions,
            this.layoutItemLLOQModeDescription,
            this.layoutItemLLOQUsageDescription,
            this.layoutItemCalculateJacobian});
         this.layoutGroupGeneral.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupGeneral.Name = "layoutGroupGeneral";
         this.layoutGroupGeneral.Size = new System.Drawing.Size(1135, 452);
         // 
         // layoutItemLLOQMode
         // 
         this.layoutItemLLOQMode.Control = this.cbLLOQMode;
         this.layoutItemLLOQMode.CustomizationFormText = "LLOQModeLayoutItem";
         this.layoutItemLLOQMode.Location = new System.Drawing.Point(0, 0);
         this.layoutItemLLOQMode.Name = "layoutItemLLOQMode";
         this.layoutItemLLOQMode.Size = new System.Drawing.Size(1111, 24);
         this.layoutItemLLOQMode.TextSize = new System.Drawing.Size(143, 13);
         // 
         // layoutItemRemoveLLOQMode
         // 
         this.layoutItemRemoveLLOQMode.Control = this.cbLLOQUsage;
         this.layoutItemRemoveLLOQMode.CustomizationFormText = "LLOQUsageLayoutItem";
         this.layoutItemRemoveLLOQMode.Location = new System.Drawing.Point(0, 41);
         this.layoutItemRemoveLLOQMode.Name = "layoutItemRemoveLLOQMode";
         this.layoutItemRemoveLLOQMode.Size = new System.Drawing.Size(1111, 24);
         this.layoutItemRemoveLLOQMode.TextSize = new System.Drawing.Size(143, 13);
         // 
         // layoutItemAlgorithmSelection
         // 
         this.layoutItemAlgorithmSelection.Control = this.cbAlgorithm;
         this.layoutItemAlgorithmSelection.CustomizationFormText = "algorithmLayoutItem";
         this.layoutItemAlgorithmSelection.Location = new System.Drawing.Point(0, 82);
         this.layoutItemAlgorithmSelection.Name = "layoutItemAlgorithmSelection";
         this.layoutItemAlgorithmSelection.Size = new System.Drawing.Size(1111, 24);
         this.layoutItemAlgorithmSelection.TextSize = new System.Drawing.Size(143, 13);
         // 
         // layoutItemAlgorithmOptions
         // 
         this.layoutItemAlgorithmOptions.Control = this.panelAlgorithmProperties;
         this.layoutItemAlgorithmOptions.CustomizationFormText = "algorithmOptionsLayoutItem";
         this.layoutItemAlgorithmOptions.Location = new System.Drawing.Point(0, 129);
         this.layoutItemAlgorithmOptions.Name = "layoutItemAlgorithmOptions";
         this.layoutItemAlgorithmOptions.Size = new System.Drawing.Size(1111, 281);
         this.layoutItemAlgorithmOptions.TextSize = new System.Drawing.Size(143, 13);
         // 
         // layoutItemLLOQModeDescription
         // 
         this.layoutItemLLOQModeDescription.Control = this.lblLLOQModeDescription;
         this.layoutItemLLOQModeDescription.Location = new System.Drawing.Point(0, 24);
         this.layoutItemLLOQModeDescription.Name = "layoutItemLLOQModeDescription";
         this.layoutItemLLOQModeDescription.Size = new System.Drawing.Size(1111, 17);
         this.layoutItemLLOQModeDescription.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemLLOQModeDescription.TextVisible = false;
         // 
         // layoutItemLLOQUsageDescription
         // 
         this.layoutItemLLOQUsageDescription.Control = this.lblLLOQUsageDescription;
         this.layoutItemLLOQUsageDescription.Location = new System.Drawing.Point(0, 65);
         this.layoutItemLLOQUsageDescription.Name = "layoutItemLLOQUsageDescription";
         this.layoutItemLLOQUsageDescription.Size = new System.Drawing.Size(1111, 17);
         this.layoutItemLLOQUsageDescription.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemLLOQUsageDescription.TextVisible = false;
         // 
         // layoutItemCalculateJacobian
         // 
         this.layoutItemCalculateJacobian.Control = this.chkCalculateJacobian;
         this.layoutItemCalculateJacobian.Location = new System.Drawing.Point(0, 106);
         this.layoutItemCalculateJacobian.Name = "layoutItemCalculateJacobian";
         this.layoutItemCalculateJacobian.Size = new System.Drawing.Size(1111, 23);
         this.layoutItemCalculateJacobian.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemCalculateJacobian.TextVisible = false;
         // 
         // layoutGroupOptions
         // 
         this.layoutGroupOptions.ExpandButtonVisible = true;
         this.layoutGroupOptions.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemOptionSelection,
            this.layoutItemOptionPanel});
         this.layoutGroupOptions.Location = new System.Drawing.Point(0, 457);
         this.layoutGroupOptions.Name = "layoutGroupOptions";
         this.layoutGroupOptions.Size = new System.Drawing.Size(1135, 330);
         // 
         // layoutItemOptionSelection
         // 
         this.layoutItemOptionSelection.Control = this.cbOptionSelection;
         this.layoutItemOptionSelection.CustomizationFormText = "optionsSelectorLayoutItem";
         this.layoutItemOptionSelection.Location = new System.Drawing.Point(0, 0);
         this.layoutItemOptionSelection.Name = "layoutItemOptionSelection";
         this.layoutItemOptionSelection.Size = new System.Drawing.Size(1111, 24);
         this.layoutItemOptionSelection.TextSize = new System.Drawing.Size(143, 13);
         // 
         // layoutItemOptionPanel
         // 
         this.layoutItemOptionPanel.Control = this.panelOptions;
         this.layoutItemOptionPanel.CustomizationFormText = "layoutItemOptionPanel";
         this.layoutItemOptionPanel.Location = new System.Drawing.Point(0, 24);
         this.layoutItemOptionPanel.Name = "layoutItemOptionPanel";
         this.layoutItemOptionPanel.Size = new System.Drawing.Size(1111, 264);
         this.layoutItemOptionPanel.TextSize = new System.Drawing.Size(143, 13);
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(0, 452);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(1135, 5);
         // 
         // ParameterIdentificationConfigurationView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ParameterIdentificationConfigurationView";
         this.Size = new System.Drawing.Size(1155, 807);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.chkCalculateJacobian.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbOptionSelection.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelOptions)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbAlgorithm.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbLLOQMode.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbLLOQUsage.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelAlgorithmProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupGeneral)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLLOQMode)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRemoveLLOQMode)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAlgorithmSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAlgorithmOptions)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLLOQModeDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLLOQUsageDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCalculateJacobian)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupOptions)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOptionSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOptionPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemOptionPanel;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemOptionSelection;
      private DevExpress.XtraEditors.PanelControl panelOptions;
      private DevExpress.XtraEditors.ComboBoxEdit cbOptionSelection;
      private DevExpress.XtraEditors.ComboBoxEdit cbLLOQUsage;
      private DevExpress.XtraEditors.PanelControl panelAlgorithmProperties;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemAlgorithmOptions;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemAlgorithmSelection;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemRemoveLLOQMode;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemLLOQMode;
      private DevExpress.XtraEditors.ComboBoxEdit cbLLOQMode;
      private DevExpress.XtraEditors.ComboBoxEdit cbAlgorithm;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupGeneral;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupOptions;
      private DevExpress.XtraEditors.LabelControl lblLLOQUsageDescription;
      private DevExpress.XtraEditors.LabelControl lblLLOQModeDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemLLOQModeDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemLLOQUsageDescription;
      private DevExpress.XtraEditors.CheckEdit chkCalculateJacobian;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemCalculateJacobian;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
   }
}
