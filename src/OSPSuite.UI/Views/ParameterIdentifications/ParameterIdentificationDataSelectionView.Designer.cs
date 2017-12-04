namespace OSPSuite.UI.Views.ParameterIdentifications
{
   partial class ParameterIdentificationDataSelectionView
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
         this.panelObservedData = new DevExpress.XtraEditors.PanelControl();
         this.panelOutputMapping = new DevExpress.XtraEditors.PanelControl();
         this.panelSimulationSelection = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemSimulationSelection = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemOutputMapping = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemObservedData = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.splitterItem2 = new DevExpress.XtraLayout.SplitterItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelObservedData)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelOutputMapping)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelSimulationSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSimulationSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOutputMapping)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemObservedData)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.panelObservedData);
         this.layoutControl.Controls.Add(this.panelOutputMapping);
         this.layoutControl.Controls.Add(this.panelSimulationSelection);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1012, 358, 250, 350);
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(959, 587);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // panelObservedData
         // 
         this.panelObservedData.Location = new System.Drawing.Point(283, 225);
         this.panelObservedData.Name = "panelObservedData";
         this.panelObservedData.Size = new System.Drawing.Size(664, 350);
         this.panelObservedData.TabIndex = 6;
         // 
         // panelOutputMapping
         // 
         this.panelOutputMapping.Location = new System.Drawing.Point(283, 12);
         this.panelOutputMapping.Name = "panelOutputMapping";
         this.panelOutputMapping.Size = new System.Drawing.Size(664, 204);
         this.panelOutputMapping.TabIndex = 5;
         // 
         // panelSimulationSelection
         // 
         this.panelSimulationSelection.Location = new System.Drawing.Point(12, 12);
         this.panelSimulationSelection.Name = "panelSimulationSelection";
         this.panelSimulationSelection.Size = new System.Drawing.Size(262, 563);
         this.panelSimulationSelection.TabIndex = 4;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemSimulationSelection,
            this.layoutItemOutputMapping,
            this.layoutItemObservedData,
            this.splitterItem1,
            this.splitterItem2});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "Root";
         this.layoutControlGroup.Size = new System.Drawing.Size(959, 587);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemSimulationSelection
         // 
         this.layoutItemSimulationSelection.Control = this.panelSimulationSelection;
         this.layoutItemSimulationSelection.Location = new System.Drawing.Point(0, 0);
         this.layoutItemSimulationSelection.Name = "layoutItemSimulationSelection";
         this.layoutItemSimulationSelection.Size = new System.Drawing.Size(266, 567);
         this.layoutItemSimulationSelection.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemSimulationSelection.TextVisible = false;
         // 
         // layoutItemOutputMapping
         // 
         this.layoutItemOutputMapping.Control = this.panelOutputMapping;
         this.layoutItemOutputMapping.Location = new System.Drawing.Point(271, 0);
         this.layoutItemOutputMapping.Name = "layoutItemOutputMapping";
         this.layoutItemOutputMapping.Size = new System.Drawing.Size(668, 208);
         this.layoutItemOutputMapping.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemOutputMapping.TextVisible = false;
         // 
         // layoutItemObservedData
         // 
         this.layoutItemObservedData.Control = this.panelObservedData;
         this.layoutItemObservedData.Location = new System.Drawing.Point(271, 213);
         this.layoutItemObservedData.Name = "layoutItemObservedData";
         this.layoutItemObservedData.Size = new System.Drawing.Size(668, 354);
         this.layoutItemObservedData.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemObservedData.TextVisible = false;
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(266, 0);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(5, 567);
         // 
         // splitterItem2
         // 
         this.splitterItem2.AllowHotTrack = true;
         this.splitterItem2.Location = new System.Drawing.Point(271, 208);
         this.splitterItem2.Name = "splitterItem2";
         this.splitterItem2.Size = new System.Drawing.Size(668, 5);
         // 
         // ParameterIdentificationDataSelectionView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ParameterIdentificationDataSelectionView";
         this.Size = new System.Drawing.Size(959, 587);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelObservedData)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelOutputMapping)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelSimulationSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSimulationSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOutputMapping)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemObservedData)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem2)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelObservedData;
      private DevExpress.XtraEditors.PanelControl panelOutputMapping;
      private DevExpress.XtraEditors.PanelControl panelSimulationSelection;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemObservedData;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemOutputMapping;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSimulationSelection;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraLayout.SplitterItem splitterItem2;
   }
}
