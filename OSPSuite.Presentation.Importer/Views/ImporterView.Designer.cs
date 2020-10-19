using OSPSuite.Assets;

namespace OSPSuite.Presentation.Importer.Views
{
   partial class ImporterView
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
         this.rootLayoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.btnImport = new DevExpress.XtraEditors.SimpleButton();
         this.btnImportAll = new DevExpress.XtraEditors.SimpleButton();
         this.columnMappingPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.ImporterTabControl = new DevExpress.XtraTab.XtraTabControl();
         this.formatComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.sourceFilePanelControl = new DevExpress.XtraEditors.PanelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.sourceFileLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.importerTabLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.columnMappingLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.importAllBtnLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.importBtnLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem5 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.formatLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem6 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ImporterTabControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.formatComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerTabLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.importAllBtnLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.importBtnLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.formatLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem6)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
         this.rootLayoutControl.Controls.Add(this.btnImport);
         this.rootLayoutControl.Controls.Add(this.btnImportAll);
         this.rootLayoutControl.Controls.Add(this.columnMappingPanelControl);
         this.rootLayoutControl.Controls.Add(this.ImporterTabControl);
         this.rootLayoutControl.Controls.Add(this.formatComboBoxEdit);
         this.rootLayoutControl.Controls.Add(this.sourceFilePanelControl);
         this.rootLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.rootLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.rootLayoutControl.Name = "rootLayoutControl";
         this.rootLayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(11551, 879, 1625, 1000);
         this.rootLayoutControl.Root = this.Root;
         this.rootLayoutControl.Size = new System.Drawing.Size(2414, 2122);
         this.rootLayoutControl.TabIndex = 0;
         this.rootLayoutControl.Text = "rootLayoutControl";
         // 
         // btnImport
         // 
         this.btnImport.Location = new System.Drawing.Point(1754, 2015);
         this.btnImport.Name = "btnImport";
         this.btnImport.Size = new System.Drawing.Size(294, 54);
         this.btnImport.StyleController = this.rootLayoutControl;
         this.btnImport.TabIndex = 10;
         this.btnImport.Text = Captions.Importer.Import;
         // 
         // btnImportAll
         // 
         this.btnImportAll.Location = new System.Drawing.Point(2113, 2015);
         this.btnImportAll.Name = "btnImportAll";
         this.btnImportAll.Size = new System.Drawing.Size(289, 54);
         this.btnImportAll.StyleController = this.rootLayoutControl;
         this.btnImportAll.TabIndex = 9;
         this.btnImportAll.Text = Captions.Importer.ImportAll;
         // 
         // columnMappingPanelControl
         // 
         this.columnMappingPanelControl.Location = new System.Drawing.Point(1131, 277);
         this.columnMappingPanelControl.Name = "columnMappingPanelControl";
         this.columnMappingPanelControl.Size = new System.Drawing.Size(1271, 1655);
         this.columnMappingPanelControl.TabIndex = 8;
         // 
         // ImporterTabControl
         // 
         this.ImporterTabControl.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPageHeaders;
         this.ImporterTabControl.Location = new System.Drawing.Point(12, 171);
         this.ImporterTabControl.Name = "ImporterTabControl";
         this.ImporterTabControl.Size = new System.Drawing.Size(1090, 1761);
         this.ImporterTabControl.TabIndex = 6;
         // 
         // formatComboBoxEdit
         // 
         this.formatComboBoxEdit.Location = new System.Drawing.Point(1237, 171);
         this.formatComboBoxEdit.Name = "formatComboBoxEdit";
         this.formatComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.formatComboBoxEdit.Size = new System.Drawing.Size(1165, 50);
         this.formatComboBoxEdit.StyleController = this.rootLayoutControl;
         this.formatComboBoxEdit.TabIndex = 4;
         // 
         // sourceFilePanelControl
         // 
         this.sourceFilePanelControl.Location = new System.Drawing.Point(12, 12);
         this.sourceFilePanelControl.Margin = new System.Windows.Forms.Padding(2);
         this.sourceFilePanelControl.Name = "sourceFilePanelControl";
         this.sourceFilePanelControl.Size = new System.Drawing.Size(2390, 121);
         this.sourceFilePanelControl.TabIndex = 5;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.sourceFileLayoutControlItem,
            this.importerTabLayoutControlItem,
            this.splitterItem1,
            this.columnMappingLayoutControlItem,
            this.importAllBtnLayoutControlItem,
            this.importBtnLayoutControlItem,
            this.emptySpaceItem2,
            this.emptySpaceItem3,
            this.emptySpaceItem4,
            this.emptySpaceItem5,
            this.formatLayoutControlItem,
            this.emptySpaceItem6});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(2414, 2122);
         this.Root.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 2003);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(1742, 58);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // sourceFileLayoutControlItem
         // 
         this.sourceFileLayoutControlItem.Control = this.sourceFilePanelControl;
         this.sourceFileLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.sourceFileLayoutControlItem.Name = "sourceFileLayoutControlItem";
         this.sourceFileLayoutControlItem.Size = new System.Drawing.Size(2394, 125);
         this.sourceFileLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.sourceFileLayoutControlItem.TextVisible = false;
         // 
         // importerTabLayoutControlItem
         // 
         this.importerTabLayoutControlItem.Control = this.ImporterTabControl;
         this.importerTabLayoutControlItem.Location = new System.Drawing.Point(0, 159);
         this.importerTabLayoutControlItem.Name = "importerTabLayoutControlItem";
         this.importerTabLayoutControlItem.Size = new System.Drawing.Size(1094, 1765);
         this.importerTabLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.importerTabLayoutControlItem.TextVisible = false;
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(1094, 159);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(25, 1765);
         // 
         // columnMappingLayoutControlItem
         // 
         this.columnMappingLayoutControlItem.Control = this.columnMappingPanelControl;
         this.columnMappingLayoutControlItem.Location = new System.Drawing.Point(1119, 265);
         this.columnMappingLayoutControlItem.Name = "columnMappingLayoutControlItem";
         this.columnMappingLayoutControlItem.Size = new System.Drawing.Size(1275, 1659);
         this.columnMappingLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.columnMappingLayoutControlItem.TextVisible = false;
         // 
         // importAllBtnLayoutControlItem
         // 
         this.importAllBtnLayoutControlItem.Control = this.btnImportAll;
         this.importAllBtnLayoutControlItem.Location = new System.Drawing.Point(2101, 2003);
         this.importAllBtnLayoutControlItem.Name = "importAllBtnLayoutControlItem";
         this.importAllBtnLayoutControlItem.Size = new System.Drawing.Size(293, 58);
         this.importAllBtnLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.importAllBtnLayoutControlItem.TextVisible = false;
         // 
         // importBtnLayoutControlItem
         // 
         this.importBtnLayoutControlItem.Control = this.btnImport;
         this.importBtnLayoutControlItem.Location = new System.Drawing.Point(1742, 2003);
         this.importBtnLayoutControlItem.Name = "importBtnLayoutControlItem";
         this.importBtnLayoutControlItem.Size = new System.Drawing.Size(298, 58);
         this.importBtnLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.importBtnLayoutControlItem.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 1924);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(2394, 79);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem3
         // 
         this.emptySpaceItem3.AllowHotTrack = false;
         this.emptySpaceItem3.Location = new System.Drawing.Point(0, 2061);
         this.emptySpaceItem3.Name = "emptySpaceItem3";
         this.emptySpaceItem3.Size = new System.Drawing.Size(2394, 41);
         this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem4
         // 
         this.emptySpaceItem4.AllowHotTrack = false;
         this.emptySpaceItem4.Location = new System.Drawing.Point(2040, 2003);
         this.emptySpaceItem4.Name = "emptySpaceItem4";
         this.emptySpaceItem4.Size = new System.Drawing.Size(61, 58);
         this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem5
         // 
         this.emptySpaceItem5.AllowHotTrack = false;
         this.emptySpaceItem5.Location = new System.Drawing.Point(0, 125);
         this.emptySpaceItem5.Name = "emptySpaceItem5";
         this.emptySpaceItem5.Size = new System.Drawing.Size(2394, 34);
         this.emptySpaceItem5.TextSize = new System.Drawing.Size(0, 0);
         // 
         // formatLayoutControlItem
         // 
         this.formatLayoutControlItem.Control = this.formatComboBoxEdit;
         this.formatLayoutControlItem.Location = new System.Drawing.Point(1119, 159);
         this.formatLayoutControlItem.Name = "Format: ";
         this.formatLayoutControlItem.Size = new System.Drawing.Size(1275, 54);
         this.formatLayoutControlItem.TextSize = new System.Drawing.Size(103, 33);
         // 
         // emptySpaceItem6
         // 
         this.emptySpaceItem6.AllowHotTrack = false;
         this.emptySpaceItem6.Location = new System.Drawing.Point(1119, 213);
         this.emptySpaceItem6.Name = "emptySpaceItem6";
         this.emptySpaceItem6.Size = new System.Drawing.Size(1275, 52);
         this.emptySpaceItem6.TextSize = new System.Drawing.Size(0, 0);
         // 
         // ImporterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "ImporterView";
         this.Controls.Add(this.rootLayoutControl);
         this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
         this.Name = "ImporterView";
         this.Size = new System.Drawing.Size(2414, 2122);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).EndInit();
         this.rootLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ImporterTabControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.formatComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerTabLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.importAllBtnLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.importBtnLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.formatLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem6)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl sourceFilePanelControl;
      private DevExpress.XtraLayout.LayoutControl rootLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.ComboBoxEdit formatComboBoxEdit;
      private DevExpress.XtraLayout.LayoutControlItem sourceFileLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraTab.XtraTabControl ImporterTabControl;
      private DevExpress.XtraLayout.LayoutControlItem importerTabLayoutControlItem;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraEditors.PanelControl columnMappingPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem columnMappingLayoutControlItem;
      private DevExpress.XtraEditors.SimpleButton btnImport;
      private DevExpress.XtraEditors.SimpleButton btnImportAll;
      private DevExpress.XtraLayout.LayoutControlItem importAllBtnLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem importBtnLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem5;
      private DevExpress.XtraLayout.LayoutControlItem formatLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem6;
   }
}