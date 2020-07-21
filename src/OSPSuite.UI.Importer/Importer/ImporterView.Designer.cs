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
         this.btnImport = new DevExpress.XtraEditors.SimpleButton();
         this.btnImportAll = new DevExpress.XtraEditors.SimpleButton();
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
         this.layoutItemImportAll = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemImport = new DevExpress.XtraLayout.LayoutControlItem();
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
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImportAll)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImport)).BeginInit();
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
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(1514, 73);
         this.btnCancel.Size = new System.Drawing.Size(291, 54);
         // 
         // btnOk
         // 
         this.btnOk.Location = new System.Drawing.Point(1123, 73);
         this.btnOk.Size = new System.Drawing.Size(367, 54);
         // 
         // layoutControlBase
         // 
         this.layoutControlBase.Location = new System.Drawing.Point(0, 1480);
         this.layoutControlBase.Margin = new System.Windows.Forms.Padding(20, 20, 20, 20);
         this.layoutControlBase.Size = new System.Drawing.Size(1918, 117);
         this.layoutControlBase.Controls.SetChildIndex(this.btnCancel, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnOk, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnExtra, 0);
         // 
         // btnExtra
         // 
         this.btnExtra.Location = new System.Drawing.Point(70, 73);
         this.btnExtra.Size = new System.Drawing.Size(513, 54);
         // 
         // layoutControlGroupBase
         // 
         this.layoutControlGroupBase.Size = new System.Drawing.Size(1875, 200);
         // 
         // layoutItemOK
         // 
         this.layoutItemOK.Location = new System.Drawing.Point(1053, 0);
         this.layoutItemOK.Size = new System.Drawing.Size(391, 80);
         // 
         // layoutItemCancel
         // 
         this.layoutItemCancel.Location = new System.Drawing.Point(1444, 0);
         this.layoutItemCancel.Size = new System.Drawing.Size(315, 80);
         // 
         // emptySpaceItemBase
         // 
         this.emptySpaceItemBase.Location = new System.Drawing.Point(537, 0);
         this.emptySpaceItemBase.Size = new System.Drawing.Size(516, 80);
         // 
         // layoutItemExtra
         // 
         this.layoutItemExtra.Size = new System.Drawing.Size(537, 80);
         // 
         // xtraTabControl
         // 
         this.xtraTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.xtraTabControl.Location = new System.Drawing.Point(30, 30);
         this.xtraTabControl.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.xtraTabControl.Name = "xtraTabControl";
         this.xtraTabControl.SelectedTabPage = this.SourcePage;
         this.xtraTabControl.Size = new System.Drawing.Size(1858, 1420);
         this.xtraTabControl.TabIndex = 0;
         this.xtraTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.SourcePage,
            this.ImportsPage});
         // 
         // SourcePage
         // 
         this.SourcePage.Controls.Add(this.layoutControlSourceTab);
         this.SourcePage.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.SourcePage.Name = "SourcePage";
         this.SourcePage.Size = new System.Drawing.Size(1854, 1358);
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
         this.layoutControlSourceTab.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.layoutControlSourceTab.Name = "layoutControlSourceTab";
         this.layoutControlSourceTab.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1081, 366, 506, 554);
         this.layoutControlSourceTab.OptionsView.UseDefaultDragAndDropRendering = false;
         this.layoutControlSourceTab.Root = this.layoutControlGroupSourceTab;
         this.layoutControlSourceTab.Size = new System.Drawing.Size(1854, 1358);
         this.layoutControlSourceTab.TabIndex = 0;
         this.layoutControlSourceTab.Text = "layoutControl1";
         // 
         // btnImport
         // 
         this.btnImport.Location = new System.Drawing.Point(1356, 1274);
         this.btnImport.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.btnImport.Name = "btnImport";
         this.btnImport.Size = new System.Drawing.Size(468, 54);
         this.btnImport.StyleController = this.layoutControlSourceTab;
         this.btnImport.TabIndex = 11;
         this.btnImport.Text = "btnImport";
         // 
         // btnImportAll
         // 
         this.btnImportAll.Location = new System.Drawing.Point(1175, 1274);
         this.btnImportAll.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.btnImportAll.Name = "btnImportAll";
         this.btnImportAll.Size = new System.Drawing.Size(171, 54);
         this.btnImportAll.StyleController = this.layoutControlSourceTab;
         this.btnImportAll.TabIndex = 10;
         this.btnImportAll.Text = "btnImportAll";
         // 
         // btnSelectRange
         // 
         this.btnSelectRange.Location = new System.Drawing.Point(30, 1274);
         this.btnSelectRange.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.btnSelectRange.Name = "btnSelectRange";
         this.btnSelectRange.Size = new System.Drawing.Size(717, 54);
         this.btnSelectRange.StyleController = this.layoutControlSourceTab;
         this.btnSelectRange.TabIndex = 9;
         this.btnSelectRange.Text = "btnSelectRange";
         // 
         // openSourceFileControlPanel
         // 
         this.openSourceFileControlPanel.Location = new System.Drawing.Point(59, 60);
         this.openSourceFileControlPanel.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.openSourceFileControlPanel.Name = "openSourceFileControlPanel";
         this.openSourceFileControlPanel.Size = new System.Drawing.Size(1736, 66);
         this.openSourceFileControlPanel.TabIndex = 5;
         // 
         // columnMappingControlPanel
         // 
         this.columnMappingControlPanel.Location = new System.Drawing.Point(999, 207);
         this.columnMappingControlPanel.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.columnMappingControlPanel.Name = "columnMappingControlPanel";
         this.columnMappingControlPanel.Size = new System.Drawing.Size(825, 1057);
         this.columnMappingControlPanel.TabIndex = 7;
         // 
         // sourceFilePreviewControlPanel
         // 
         this.sourceFilePreviewControlPanel.Location = new System.Drawing.Point(30, 207);
         this.sourceFilePreviewControlPanel.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.sourceFilePreviewControlPanel.Name = "sourceFilePreviewControlPanel";
         this.sourceFilePreviewControlPanel.Size = new System.Drawing.Size(934, 1057);
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
         this.layoutControlGroupSourceTab.Name = "Root";
         this.layoutControlGroupSourceTab.Size = new System.Drawing.Size(1854, 1358);
         this.layoutControlGroupSourceTab.TextVisible = false;
         // 
         // Mapping
         // 
         this.Mapping.Control = this.columnMappingControlPanel;
         this.Mapping.CustomizationFormText = "ColumnMappingCaption";
         this.Mapping.Location = new System.Drawing.Point(969, 136);
         this.Mapping.Name = "Mapping";
         this.Mapping.Size = new System.Drawing.Size(835, 1108);
         this.Mapping.TextLocation = DevExpress.Utils.Locations.Top;
         this.Mapping.TextSize = new System.Drawing.Size(101, 33);
         // 
         // Preview
         // 
         this.Preview.Control = this.sourceFilePreviewControlPanel;
         this.Preview.CustomizationFormText = "layoutControlItem3";
         this.Preview.Location = new System.Drawing.Point(0, 136);
         this.Preview.Name = "Preview";
         this.Preview.Size = new System.Drawing.Size(944, 1108);
         this.Preview.TextLocation = DevExpress.Utils.Locations.Top;
         this.Preview.TextSize = new System.Drawing.Size(101, 33);
         // 
         // splitterItem
         // 
         this.splitterItem.AllowHotTrack = true;
         this.splitterItem.CustomizationFormText = "splitterItem";
         this.splitterItem.Location = new System.Drawing.Point(944, 136);
         this.splitterItem.Name = "splitterItem";
         this.splitterItem.Size = new System.Drawing.Size(25, 1108);
         // 
         // layoutControlGroup2
         // 
         this.layoutControlGroup2.CustomizationFormText = "layoutControlGroup2";
         this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
         this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup2.Name = "layoutControlGroup2";
         this.layoutControlGroup2.Size = new System.Drawing.Size(1804, 136);
         this.layoutControlGroup2.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.openSourceFileControlPanel;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.MaxSize = new System.Drawing.Size(0, 76);
         this.layoutControlItem2.MinSize = new System.Drawing.Size(260, 76);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(1746, 76);
         this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(727, 1244);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(418, 64);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutItemSelectRange
         // 
         this.layoutItemSelectRange.Control = this.btnSelectRange;
         this.layoutItemSelectRange.Location = new System.Drawing.Point(0, 1244);
         this.layoutItemSelectRange.Name = "layoutItemSelectRange";
         this.layoutItemSelectRange.Size = new System.Drawing.Size(727, 64);
         this.layoutItemSelectRange.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemSelectRange.TextVisible = false;
         // 
         // layoutItemImportAll
         // 
         this.layoutItemImportAll.Control = this.btnImportAll;
         this.layoutItemImportAll.Location = new System.Drawing.Point(1145, 1244);
         this.layoutItemImportAll.Name = "layoutItemImportAll";
         this.layoutItemImportAll.Size = new System.Drawing.Size(181, 64);
         this.layoutItemImportAll.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemImportAll.TextVisible = false;
         // 
         // layoutItemImport
         // 
         this.layoutItemImport.Control = this.btnImport;
         this.layoutItemImport.Location = new System.Drawing.Point(1326, 1244);
         this.layoutItemImport.Name = "layoutItemImport";
         this.layoutItemImport.Size = new System.Drawing.Size(478, 64);
         this.layoutItemImport.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemImport.TextVisible = false;
         // 
         // ImportsPage
         // 
         this.ImportsPage.Controls.Add(this.layoutControlTab);
         this.ImportsPage.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.ImportsPage.Name = "ImportsPage";
         this.ImportsPage.Size = new System.Drawing.Size(1854, 1358);
         this.ImportsPage.Text = "Imports";
         // 
         // layoutControlTab
         // 
         this.layoutControlTab.AllowCustomization = false;
         this.layoutControlTab.Controls.Add(this.panelImportedTabs);
         this.layoutControlTab.Controls.Add(this.namingImportPanel);
         this.layoutControlTab.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControlTab.Location = new System.Drawing.Point(0, 0);
         this.layoutControlTab.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.layoutControlTab.Name = "layoutControlTab";
         this.layoutControlTab.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(666, 494, 250, 350);
         this.layoutControlTab.Root = this.layoutControlGroup1;
         this.layoutControlTab.Size = new System.Drawing.Size(1854, 1358);
         this.layoutControlTab.TabIndex = 0;
         this.layoutControlTab.Text = "layoutControl1";
         // 
         // panelImportedTabs
         // 
         this.panelImportedTabs.Location = new System.Drawing.Point(30, 268);
         this.panelImportedTabs.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.panelImportedTabs.Name = "panelImportedTabs";
         this.panelImportedTabs.Size = new System.Drawing.Size(1794, 1060);
         this.panelImportedTabs.TabIndex = 5;
         // 
         // namingImportPanel
         // 
         this.namingImportPanel.Location = new System.Drawing.Point(59, 60);
         this.namingImportPanel.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.namingImportPanel.Name = "namingImportPanel";
         this.namingImportPanel.Size = new System.Drawing.Size(1736, 168);
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
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Size = new System.Drawing.Size(1854, 1358);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem7
         // 
         this.layoutControlItem7.Control = this.panelImportedTabs;
         this.layoutControlItem7.CustomizationFormText = "layoutControlItem7";
         this.layoutControlItem7.Location = new System.Drawing.Point(0, 238);
         this.layoutControlItem7.MinSize = new System.Drawing.Size(260, 61);
         this.layoutControlItem7.Name = "layoutControlItem7";
         this.layoutControlItem7.Size = new System.Drawing.Size(1804, 1070);
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
         this.layoutControlGroup3.Size = new System.Drawing.Size(1804, 238);
         this.layoutControlGroup3.TextVisible = false;
         // 
         // layoutItemNamingPanel
         // 
         this.layoutItemNamingPanel.Control = this.namingImportPanel;
         this.layoutItemNamingPanel.CustomizationFormText = "layoutControlItem6";
         this.layoutItemNamingPanel.Location = new System.Drawing.Point(0, 0);
         this.layoutItemNamingPanel.MaxSize = new System.Drawing.Size(0, 178);
         this.layoutItemNamingPanel.MinSize = new System.Drawing.Size(260, 178);
         this.layoutItemNamingPanel.Name = "layoutItemNamingPanel";
         this.layoutItemNamingPanel.Size = new System.Drawing.Size(1746, 178);
         this.layoutItemNamingPanel.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemNamingPanel.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemNamingPanel.TextVisible = false;
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.xtraTabControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1086, 328, 250, 350);
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(1918, 1480);
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
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Size = new System.Drawing.Size(1918, 1480);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.xtraTabControl;
         this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
         this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Size = new System.Drawing.Size(1868, 1430);
         this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem4.TextVisible = false;
         // 
         // ImporterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "ImporterView";
         this.ClientSize = new System.Drawing.Size(1918, 1597);
         this.Controls.Add(this.layoutControl);
         this.Margin = new System.Windows.Forms.Padding(50, 51, 50, 51);
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
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImportAll)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImport)).EndInit();
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
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraTab.XtraTabControl xtraTabControl;
      private DevExpress.XtraTab.XtraTabPage SourcePage;
      private DevExpress.XtraTab.XtraTabPage ImportsPage;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupSourceTab;
      private DevExpress.XtraLayout.SplitterItem splitterItem;
      private DevExpress.XtraEditors.PanelControl columnMappingControlPanel;
      private DevExpress.XtraEditors.PanelControl sourceFilePreviewControlPanel;
      private DevExpress.XtraEditors.PanelControl openSourceFileControlPanel;
      private DevExpress.XtraLayout.LayoutControlItem Preview;
      private DevExpress.XtraLayout.LayoutControlItem Mapping;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
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
      private Controls.UxLayoutControl layoutControlSourceTab;
      private Controls.UxLayoutControl layoutControl;
      private Controls.UxLayoutControl layoutControlTab;
   }
}