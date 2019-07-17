namespace OSPSuite.UI.Importer
{
   public partial class ImporterView
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
         cleanMemory();
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.xtraTabControl = new DevExpress.XtraTab.XtraTabControl();
         this.SourcePage = new DevExpress.XtraTab.XtraTabPage();
         this.layoutControlSourceTab = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnSelectRange = new DevExpress.XtraEditors.SimpleButton();
         this.openSourceFileControlPanel = new DevExpress.XtraEditors.PanelControl();
         this.columnMappingControlPanel = new DevExpress.XtraEditors.PanelControl();
         this.sourceFilePreviewControlPanel = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroupSourceTab = new DevExpress.XtraLayout.LayoutControlGroup();
         this.Mapping = new DevExpress.XtraLayout.LayoutControlItem();
         this.Preview = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem = new DevExpress.XtraLayout.SplitterItem();
         this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutItemSelectRange = new DevExpress.XtraLayout.LayoutControlItem();
         this.ImportsPage = new DevExpress.XtraTab.XtraTabPage();
         this.layoutControlTab = new OSPSuite.UI.Controls.UxLayoutControl();
         this.panelImportedTabs = new DevExpress.XtraEditors.PanelControl();
         this.namingImportPanel = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemNamingPanel = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
         this.btnImportAll = new DevExpress.XtraEditors.SimpleButton();
         this.layoutItemImportAll = new DevExpress.XtraLayout.LayoutControlItem();
         this.btnImport = new DevExpress.XtraEditors.SimpleButton();
         this.layoutItemImport = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).BeginInit();
         this.layoutControlBase.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl)).BeginInit();
         this.xtraTabControl.SuspendLayout();
         this.SourcePage.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlSourceTab)).BeginInit();
         this.layoutControlSourceTab.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.openSourceFileControlPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingControlPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePreviewControlPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupSourceTab)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Mapping)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Preview)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSelectRange)).BeginInit();
         this.ImportsPage.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlTab)).BeginInit();
         this.layoutControlTab.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelImportedTabs)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingImportPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNamingPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImportAll)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImport)).BeginInit();
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(625, 12);
         this.btnCancel.Size = new System.Drawing.Size(130, 22);
         // 
         // btnOk
         // 
         this.btnOk.Location = new System.Drawing.Point(459, 12);
         this.btnOk.Size = new System.Drawing.Size(162, 22);
         // 
         // layoutControlBase
         // 
         this.layoutControlBase.Location = new System.Drawing.Point(0, 583);
         this.layoutControlBase.Size = new System.Drawing.Size(767, 46);
         this.layoutControlBase.Controls.SetChildIndex(this.btnCancel, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnOk, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnExtra, 0);
         // 
         // btnExtra
         // 
         this.btnExtra.Size = new System.Drawing.Size(224, 22);
         // 
         // layoutControlGroupBase
         // 
         this.layoutControlGroupBase.Size = new System.Drawing.Size(767, 46);
         // 
         // layoutItemOK
         // 
         this.layoutItemOK.Location = new System.Drawing.Point(447, 0);
         this.layoutItemOK.Size = new System.Drawing.Size(166, 26);
         // 
         // layoutItemCancel
         // 
         this.layoutItemCancel.Location = new System.Drawing.Point(613, 0);
         this.layoutItemCancel.Size = new System.Drawing.Size(134, 26);
         // 
         // emptySpaceItemBase
         // 
         this.emptySpaceItemBase.Location = new System.Drawing.Point(228, 0);
         this.emptySpaceItemBase.Size = new System.Drawing.Size(219, 26);
         // 
         // layoutItemExtra
         // 
         this.layoutItemExtra.Size = new System.Drawing.Size(228, 26);
         // 
         // xtraTabControl
         // 
         this.xtraTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.xtraTabControl.Location = new System.Drawing.Point(12, 12);
         this.xtraTabControl.Name = "xtraTabControl";
         this.xtraTabControl.SelectedTabPage = this.SourcePage;
         this.xtraTabControl.Size = new System.Drawing.Size(743, 559);
         this.xtraTabControl.TabIndex = 0;
         this.xtraTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.SourcePage,
            this.ImportsPage});
         // 
         // SourcePage
         // 
         this.SourcePage.Controls.Add(this.layoutControlSourceTab);
         this.SourcePage.Name = "SourcePage";
         this.SourcePage.Size = new System.Drawing.Size(737, 531);
         this.SourcePage.Text = "Source";
         // 
         // layoutControlSourceTab
         // 
         this.layoutControlSourceTab.AllowCustomization = false;
         this.layoutControlSourceTab.Controls.Add(this.btnImport);
         this.layoutControlSourceTab.Controls.Add(this.btnImportAll);
         this.layoutControlSourceTab.Controls.Add(this.btnSelectRange);
         this.layoutControlSourceTab.Controls.Add(this.openSourceFileControlPanel);
         this.layoutControlSourceTab.Controls.Add(this.columnMappingControlPanel);
         this.layoutControlSourceTab.Controls.Add(this.sourceFilePreviewControlPanel);
         this.layoutControlSourceTab.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControlSourceTab.Location = new System.Drawing.Point(0, 0);
         this.layoutControlSourceTab.Name = "layoutControlSourceTab";
         this.layoutControlSourceTab.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1081, 366, 506, 554);
         this.layoutControlSourceTab.OptionsView.UseDefaultDragAndDropRendering = false;
         this.layoutControlSourceTab.Root = this.layoutControlGroupSourceTab;
         this.layoutControlSourceTab.Size = new System.Drawing.Size(737, 531);
         this.layoutControlSourceTab.TabIndex = 0;
         this.layoutControlSourceTab.Text = "layoutControl1";
         // 
         // btnSelectRange
         // 
         this.btnSelectRange.Location = new System.Drawing.Point(12, 497);
         this.btnSelectRange.Name = "btnSelectRange";
         this.btnSelectRange.Size = new System.Drawing.Size(285, 22);
         this.btnSelectRange.StyleController = this.layoutControlSourceTab;
         this.btnSelectRange.TabIndex = 9;
         this.btnSelectRange.Text = "btnSelectRange";
         // 
         // openSourceFileControlPanel
         // 
         this.openSourceFileControlPanel.Location = new System.Drawing.Point(24, 24);
         this.openSourceFileControlPanel.Name = "openSourceFileControlPanel";
         this.openSourceFileControlPanel.Size = new System.Drawing.Size(689, 26);
         this.openSourceFileControlPanel.TabIndex = 5;
         // 
         // columnMappingControlPanel
         // 
         this.columnMappingControlPanel.Location = new System.Drawing.Point(392, 82);
         this.columnMappingControlPanel.Name = "columnMappingControlPanel";
         this.columnMappingControlPanel.Size = new System.Drawing.Size(333, 411);
         this.columnMappingControlPanel.TabIndex = 7;
         // 
         // sourceFilePreviewControlPanel
         // 
         this.sourceFilePreviewControlPanel.Location = new System.Drawing.Point(12, 82);
         this.sourceFilePreviewControlPanel.Name = "sourceFilePreviewControlPanel";
         this.sourceFilePreviewControlPanel.Size = new System.Drawing.Size(371, 411);
         this.sourceFilePreviewControlPanel.TabIndex = 6;
         // 
         // layoutControlGroupSourceTab
         // 
         this.layoutControlGroupSourceTab.CustomizationFormText = "Root";
         this.layoutControlGroupSourceTab.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroupSourceTab.GroupBordersVisible = false;
         this.layoutControlGroupSourceTab.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.Mapping,
            this.Preview,
            this.splitterItem,
            this.layoutControlGroup2,
            this.emptySpaceItem1,
            this.layoutItemSelectRange,
            this.layoutItemImportAll,
            this.layoutItemImport});
         this.layoutControlGroupSourceTab.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroupSourceTab.Name = "Root";
         this.layoutControlGroupSourceTab.Size = new System.Drawing.Size(737, 531);
         this.layoutControlGroupSourceTab.TextVisible = false;
         // 
         // Mapping
         // 
         this.Mapping.Control = this.columnMappingControlPanel;
         this.Mapping.CustomizationFormText = "ColumnMappingCaption";
         this.Mapping.Location = new System.Drawing.Point(380, 54);
         this.Mapping.Name = "Mapping";
         this.Mapping.Size = new System.Drawing.Size(337, 431);
         this.Mapping.TextLocation = DevExpress.Utils.Locations.Top;
         this.Mapping.TextSize = new System.Drawing.Size(40, 13);
         // 
         // Preview
         // 
         this.Preview.Control = this.sourceFilePreviewControlPanel;
         this.Preview.CustomizationFormText = "layoutControlItem3";
         this.Preview.Location = new System.Drawing.Point(0, 54);
         this.Preview.Name = "Preview";
         this.Preview.Size = new System.Drawing.Size(375, 431);
         this.Preview.TextLocation = DevExpress.Utils.Locations.Top;
         this.Preview.TextSize = new System.Drawing.Size(40, 13);
         // 
         // splitterItem
         // 
         this.splitterItem.AllowHotTrack = true;
         this.splitterItem.CustomizationFormText = "splitterItem";
         this.splitterItem.Location = new System.Drawing.Point(375, 54);
         this.splitterItem.Name = "splitterItem";
         this.splitterItem.Size = new System.Drawing.Size(5, 431);
         // 
         // layoutControlGroup2
         // 
         this.layoutControlGroup2.CustomizationFormText = "layoutControlGroup2";
         this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
         this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup2.Name = "layoutControlGroup2";
         this.layoutControlGroup2.Size = new System.Drawing.Size(717, 54);
         this.layoutControlGroup2.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.openSourceFileControlPanel;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.MaxSize = new System.Drawing.Size(0, 30);
         this.layoutControlItem2.MinSize = new System.Drawing.Size(104, 30);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(693, 30);
         this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(289, 485);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(166, 26);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutItemSelectRange
         // 
         this.layoutItemSelectRange.Control = this.btnSelectRange;
         this.layoutItemSelectRange.Location = new System.Drawing.Point(0, 485);
         this.layoutItemSelectRange.Name = "layoutItemSelectRange";
         this.layoutItemSelectRange.Size = new System.Drawing.Size(289, 26);
         this.layoutItemSelectRange.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemSelectRange.TextVisible = false;
         // 
         // ImportsPage
         // 
         this.ImportsPage.Controls.Add(this.layoutControlTab);
         this.ImportsPage.Name = "ImportsPage";
         this.ImportsPage.Size = new System.Drawing.Size(737, 531);
         this.ImportsPage.Text = "Imports";
         // 
         // layoutControlTab
         // 
         this.layoutControlTab.Controls.Add(this.panelImportedTabs);
         this.layoutControlTab.Controls.Add(this.namingImportPanel);
         this.layoutControlTab.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControlTab.Location = new System.Drawing.Point(0, 0);
         this.layoutControlTab.Name = "layoutControlTab";
         this.layoutControlTab.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(666, 494, 250, 350);
         this.layoutControlTab.Root = this.layoutControlGroup1;
         this.layoutControlTab.Size = new System.Drawing.Size(737, 531);
         this.layoutControlTab.TabIndex = 0;
         this.layoutControlTab.Text = "layoutControl1";
         // 
         // panelImportedTabs
         // 
         this.panelImportedTabs.Location = new System.Drawing.Point(12, 106);
         this.panelImportedTabs.Name = "panelImportedTabs";
         this.panelImportedTabs.Size = new System.Drawing.Size(713, 413);
         this.panelImportedTabs.TabIndex = 5;
         // 
         // namingImportPanel
         // 
         this.namingImportPanel.Location = new System.Drawing.Point(24, 24);
         this.namingImportPanel.Name = "namingImportPanel";
         this.namingImportPanel.Size = new System.Drawing.Size(689, 66);
         this.namingImportPanel.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem7,
            this.layoutControlGroup3});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Size = new System.Drawing.Size(737, 531);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem7
         // 
         this.layoutControlItem7.Control = this.panelImportedTabs;
         this.layoutControlItem7.CustomizationFormText = "layoutControlItem7";
         this.layoutControlItem7.Location = new System.Drawing.Point(0, 94);
         this.layoutControlItem7.MinSize = new System.Drawing.Size(104, 24);
         this.layoutControlItem7.Name = "layoutControlItem7";
         this.layoutControlItem7.Size = new System.Drawing.Size(717, 417);
         this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem7.TextVisible = false;
         // 
         // layoutControlGroup3
         // 
         this.layoutControlGroup3.CustomizationFormText = "layoutControlGroup3";
         this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemNamingPanel});
         this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup3.Name = "layoutControlGroup3";
         this.layoutControlGroup3.Size = new System.Drawing.Size(717, 94);
         this.layoutControlGroup3.TextVisible = false;
         // 
         // layoutItemNamingPanel
         // 
         this.layoutItemNamingPanel.Control = this.namingImportPanel;
         this.layoutItemNamingPanel.CustomizationFormText = "layoutControlItem6";
         this.layoutItemNamingPanel.Location = new System.Drawing.Point(0, 0);
         this.layoutItemNamingPanel.MaxSize = new System.Drawing.Size(0, 70);
         this.layoutItemNamingPanel.MinSize = new System.Drawing.Size(104, 70);
         this.layoutItemNamingPanel.Name = "layoutItemNamingPanel";
         this.layoutItemNamingPanel.Size = new System.Drawing.Size(693, 70);
         this.layoutItemNamingPanel.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemNamingPanel.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemNamingPanel.TextVisible = false;
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.xtraTabControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1086, 328, 250, 350);
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(767, 583);
         this.layoutControl.TabIndex = 4;
         this.layoutControl.Text = "layoutControl1";
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.CustomizationFormText = "layoutControlGroup";
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem4});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Size = new System.Drawing.Size(767, 583);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.xtraTabControl;
         this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
         this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Size = new System.Drawing.Size(747, 563);
         this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem4.TextVisible = false;
         // 
         // btnImportAll
         // 
         this.btnImportAll.Location = new System.Drawing.Point(467, 497);
         this.btnImportAll.Name = "btnImportAll";
         this.btnImportAll.Size = new System.Drawing.Size(68, 22);
         this.btnImportAll.StyleController = this.layoutControlSourceTab;
         this.btnImportAll.TabIndex = 10;
         this.btnImportAll.Text = "btnImportAll";
         // 
         // layoutItemImportAll
         // 
         this.layoutItemImportAll.Control = this.btnImportAll;
         this.layoutItemImportAll.Location = new System.Drawing.Point(455, 485);
         this.layoutItemImportAll.Name = "layoutItemImportAll";
         this.layoutItemImportAll.Size = new System.Drawing.Size(72, 26);
         this.layoutItemImportAll.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemImportAll.TextVisible = false;
         // 
         // btnImport
         // 
         this.btnImport.Location = new System.Drawing.Point(539, 497);
         this.btnImport.Name = "btnImport";
         this.btnImport.Size = new System.Drawing.Size(186, 22);
         this.btnImport.StyleController = this.layoutControlSourceTab;
         this.btnImport.TabIndex = 11;
         this.btnImport.Text = "btnImport";
         // 
         // layoutItemImport
         // 
         this.layoutItemImport.Control = this.btnImport;
         this.layoutItemImport.Location = new System.Drawing.Point(527, 485);
         this.layoutItemImport.Name = "layoutItemImport";
         this.layoutItemImport.Size = new System.Drawing.Size(190, 26);
         this.layoutItemImport.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemImport.TextVisible = false;
         // 
         // ImporterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "ImporterView";
         this.ClientSize = new System.Drawing.Size(767, 629);
         this.Controls.Add(this.layoutControl);
         this.MinimumSize = new System.Drawing.Size(600, 400);
         this.Name = "ImporterView";
         this.Text = "ImporterView";
         this.Controls.SetChildIndex(this.layoutControlBase, 0);
         this.Controls.SetChildIndex(this.layoutControl, 0);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).EndInit();
         this.layoutControlBase.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl)).EndInit();
         this.xtraTabControl.ResumeLayout(false);
         this.SourcePage.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlSourceTab)).EndInit();
         this.layoutControlSourceTab.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.openSourceFileControlPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingControlPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePreviewControlPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupSourceTab)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Mapping)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Preview)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSelectRange)).EndInit();
         this.ImportsPage.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlTab)).EndInit();
         this.layoutControlTab.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelImportedTabs)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingImportPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNamingPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImportAll)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImport)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraTab.XtraTabControl xtraTabControl;
      private DevExpress.XtraTab.XtraTabPage SourcePage;
      private DevExpress.XtraTab.XtraTabPage ImportsPage;
      private DevExpress.XtraLayout.LayoutControl layoutControlSourceTab;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupSourceTab;
      private DevExpress.XtraLayout.SplitterItem splitterItem;
      private DevExpress.XtraEditors.PanelControl columnMappingControlPanel;
      private DevExpress.XtraEditors.PanelControl sourceFilePreviewControlPanel;
      private DevExpress.XtraEditors.PanelControl openSourceFileControlPanel;
      private DevExpress.XtraLayout.LayoutControlItem Preview;
      private DevExpress.XtraLayout.LayoutControlItem Mapping;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
      private DevExpress.XtraLayout.LayoutControl layoutControlTab;
      private DevExpress.XtraEditors.PanelControl panelImportedTabs;
      private DevExpress.XtraEditors.PanelControl namingImportPanel;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemNamingPanel;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.SimpleButton btnSelectRange;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSelectRange;
      private DevExpress.XtraEditors.SimpleButton btnImport;
      private DevExpress.XtraEditors.SimpleButton btnImportAll;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemImportAll;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemImport;
   }
}