using DevExpress.Utils;
using OSPSuite.Assets;

namespace OSPSuite.UI.Views.Importer
{
   partial class ImporterDataView
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
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImporterDataView));
         this.rootLayoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.labelControlError = new DevExpress.XtraEditors.LabelControl();
         this.useForImportCheckEdit = new DevExpress.XtraEditors.CheckEdit();
         this.btnImport = new DevExpress.XtraEditors.SimpleButton();
         this.btnImportAll = new DevExpress.XtraEditors.SimpleButton();
         this.importerTabControl = new DevExpress.XtraTab.XtraTabControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.importerLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemImportAll = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemImportCurrent = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.dataViewingGridControl = new DevExpress.XtraGrid.GridControl();
         this.dataViewingGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
         this.dpiAwareImageCollection1 = new DevExpress.Utils.DPIAwareImageCollection(this.components);
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.useForImportCheckEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerTabControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImportAll)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImportCurrent)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingGridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dpiAwareImageCollection1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
         this.rootLayoutControl.Controls.Add(this.labelControlError);
         this.rootLayoutControl.Controls.Add(this.useForImportCheckEdit);
         this.rootLayoutControl.Controls.Add(this.btnImport);
         this.rootLayoutControl.Controls.Add(this.btnImportAll);
         this.rootLayoutControl.Controls.Add(this.importerTabControl);
         this.rootLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.rootLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.rootLayoutControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
         this.rootLayoutControl.Name = "rootLayoutControl";
         this.rootLayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(3197, 116, 650, 400);
         this.rootLayoutControl.Root = this.Root;
         this.rootLayoutControl.Size = new System.Drawing.Size(1457, 1050);
         this.rootLayoutControl.TabIndex = 0;
         this.rootLayoutControl.Text = "rootLayoutControl";
         // 
         // labelControlError
         // 
         this.labelControlError.Appearance.ForeColor = System.Drawing.Color.Red;
         this.labelControlError.Appearance.Options.UseForeColor = true;
         this.labelControlError.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
         this.labelControlError.Location = new System.Drawing.Point(2, 951);
         this.labelControlError.Name = "labelControlError";
         this.labelControlError.Size = new System.Drawing.Size(1453, 16);
         this.labelControlError.StyleController = this.rootLayoutControl;
         this.labelControlError.TabIndex = 12;
         // 
         // useForImportCheckEdit
         // 
         this.useForImportCheckEdit.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.True;
         this.useForImportCheckEdit.Location = new System.Drawing.Point(2, 1022);
         this.useForImportCheckEdit.Name = "useForImportCheckEdit";
         this.useForImportCheckEdit.Properties.Caption = "Use filters for importing data";
         this.useForImportCheckEdit.Size = new System.Drawing.Size(478, 24);
         this.useForImportCheckEdit.StyleController = this.rootLayoutControl;
         this.useForImportCheckEdit.TabIndex = 11;
         this.useForImportCheckEdit.ToolTip = resources.GetString("useForImportCheckEdit.ToolTip");
         this.useForImportCheckEdit.ToolTipAnchor = DevExpress.Utils.ToolTipAnchor.Cursor;
         // 
         // btnImport
         // 
         this.btnImport.Location = new System.Drawing.Point(1271, 1021);
         this.btnImport.Margin = new System.Windows.Forms.Padding(1);
         this.btnImport.Name = "btnImport";
         this.btnImport.Size = new System.Drawing.Size(103, 27);
         this.btnImport.StyleController = this.rootLayoutControl;
         this.btnImport.TabIndex = 10;
         this.btnImport.Text = "btnImport";
         // 
         // btnImportAll
         // 
         this.btnImportAll.Location = new System.Drawing.Point(1378, 1021);
         this.btnImportAll.Margin = new System.Windows.Forms.Padding(1);
         this.btnImportAll.Name = "btnImportAll";
         this.btnImportAll.Size = new System.Drawing.Size(77, 27);
         this.btnImportAll.StyleController = this.rootLayoutControl;
         this.btnImportAll.TabIndex = 9;
         this.btnImportAll.Text = "btnImportAll";
         // 
         // importerTabControl
         // 
         this.importerTabControl.Location = new System.Drawing.Point(2, 2);
         this.importerTabControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
         this.importerTabControl.Name = "importerTabControl";
         this.importerTabControl.Size = new System.Drawing.Size(1453, 945);
         this.importerTabControl.TabIndex = 5;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.importerLayoutControlItem,
            this.layoutItemImportAll,
            this.layoutItemImportCurrent,
            this.emptySpaceItem1,
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.emptySpaceItem2});
         this.Root.Name = "Root";
         this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.Root.Size = new System.Drawing.Size(1457, 1050);
         this.Root.TextVisible = false;
         // 
         // importerLayoutControlItem
         // 
         this.importerLayoutControlItem.Control = this.importerTabControl;
         this.importerLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.importerLayoutControlItem.Name = "importerLayoutControlItem";
         this.importerLayoutControlItem.Size = new System.Drawing.Size(1457, 949);
         this.importerLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.importerLayoutControlItem.TextVisible = false;
         // 
         // layoutItemImportAll
         // 
         this.layoutItemImportAll.Control = this.btnImportAll;
         this.layoutItemImportAll.Location = new System.Drawing.Point(1376, 1019);
         this.layoutItemImportAll.Name = "layoutItemImportAll";
         this.layoutItemImportAll.Size = new System.Drawing.Size(81, 31);
         this.layoutItemImportAll.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemImportAll.TextVisible = false;
         // 
         // layoutItemImportCurrent
         // 
         this.layoutItemImportCurrent.Control = this.btnImport;
         this.layoutItemImportCurrent.Location = new System.Drawing.Point(1269, 1019);
         this.layoutItemImportCurrent.Name = "layoutItemImportCurrent";
         this.layoutItemImportCurrent.Size = new System.Drawing.Size(107, 31);
         this.layoutItemImportCurrent.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemImportCurrent.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(482, 1019);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(787, 31);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
         this.layoutControlItem1.Control = this.useForImportCheckEdit;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 1019);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(482, 31);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.labelControlError;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 949);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(1457, 20);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         this.layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
         // 
         // dataViewingGridControl
         // 
         this.dataViewingGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.dataViewingGridControl.Location = new System.Drawing.Point(222, 713);
         this.dataViewingGridControl.MainView = this.dataViewingGridView;
         this.dataViewingGridControl.Name = "dataViewingGridControl";
         this.dataViewingGridControl.Size = new System.Drawing.Size(400, 200);
         this.dataViewingGridControl.TabIndex = 0;
         this.dataViewingGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dataViewingGridView});
         // 
         // dataViewingGridView
         // 
         this.dataViewingGridView.GridControl = this.dataViewingGridControl;
         this.dataViewingGridView.Name = "dataViewingGridView";
         this.dataViewingGridView.OptionsView.ColumnAutoWidth = false;
         // 
         // imageCollection1
         // 
         this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
         this.imageCollection1.Images.SetKeyName(0, "OK.ico");
         this.imageCollection1.Images.SetKeyName(1, "Cancel.ico");
         // 
         // dpiAwareImageCollection1
         // 
         this.dpiAwareImageCollection1.Owner = this;
         this.dpiAwareImageCollection1.Stream = ((DevExpress.Utils.DPIAwareImageCollectionStreamer)(resources.GetObject("dpiAwareImageCollection1.Stream")));
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 969);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(1457, 50);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // ImporterDataView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.rootLayoutControl);
         this.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
         this.Name = "ImporterDataView";
         this.Size = new System.Drawing.Size(1457, 1050);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).EndInit();
         this.rootLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.useForImportCheckEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerTabControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImportAll)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImportCurrent)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingGridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dpiAwareImageCollection1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl rootLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.SimpleButton btnImport;
      private DevExpress.XtraEditors.SimpleButton btnImportAll;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemImportAll;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemImportCurrent;
      private DevExpress.XtraGrid.GridControl dataViewingGridControl;
      private DevExpress.XtraGrid.Views.Grid.GridView dataViewingGridView;
      private DevExpress.XtraTab.XtraTabControl importerTabControl;
      private DevExpress.XtraLayout.LayoutControlItem importerLayoutControlItem;
      private DevExpress.XtraEditors.CheckEdit useForImportCheckEdit;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private ImageCollection imageCollection1;
      private DPIAwareImageCollection dpiAwareImageCollection1;
      private DevExpress.XtraEditors.LabelControl labelControlError;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
   }
}
