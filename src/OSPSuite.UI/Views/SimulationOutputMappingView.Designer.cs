
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views
{
   partial class SimulationOutputMappingView
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
         DevExpress.XtraBars.Ribbon.Internal.RepositoryItemRibbonSearchEdit repositoryItemRibbonSearchEdit1 = new DevExpress.XtraBars.Ribbon.Internal.RepositoryItemRibbonSearchEdit();
         DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulationOutputMappingView));
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
         this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new OSPSuite.UI.Controls.UxGridView();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.ribbonGroupEdit = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
         this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
         this.ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
         this.barDockControlTop1 = new DevExpress.XtraBars.BarDockControl();
         this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
         this.btnPresent = new DevExpress.XtraBars.BarButtonItem();
         this.btnNotPresent = new DevExpress.XtraBars.BarButtonItem();
         this.btnAllowNegativeValues = new DevExpress.XtraBars.BarButtonItem();
         this.btnNotAllowNegativeValues = new DevExpress.XtraBars.BarButtonItem();
         this.ribbonPage = new DevExpress.XtraBars.Ribbon.RibbonPage();
         this.ribbonGroupEdit1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
         this.layoutItemRibbon = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(repositoryItemRibbonSearchEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRibbon)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Controls.Add(this.ribbonControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(920, 811);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl";
         // 
         // gridControl
         // 
         this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
         this.gridControl.Location = new System.Drawing.Point(6, 107);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(908, 698);
         this.gridControl.TabIndex = 4;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.ColumnPanelRowHeight = 0;
         this.gridView.DetailHeight = 170;
         this.gridView.EnableColumnContextMenu = true;
         this.gridView.FooterPanelHeight = 0;
         this.gridView.GridControl = this.gridControl;
         this.gridView.GroupRowHeight = 0;
         this.gridView.LevelIndent = 0;
         this.gridView.MultiSelect = false;
         this.gridView.Name = "gridView";
         this.gridView.PreviewIndent = 0;
         this.gridView.RowHeight = 0;
         this.gridView.ViewCaptionHeight = 0;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem,
            this.layoutItemRibbon});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(920, 811);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem
         // 
         this.layoutControlItem.Control = this.gridControl;
         this.layoutControlItem.Location = new System.Drawing.Point(0, 101);
         this.layoutControlItem.Name = "layoutControlItem";
         this.layoutControlItem.Size = new System.Drawing.Size(910, 700);
         this.layoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem.TextVisible = false;
         // 
         // barDockControlTop
         // 
         this.barDockControlTop.CausesValidation = false;
         this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop.Manager = null;
         this.barDockControlTop.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
         this.barDockControlTop.Size = new System.Drawing.Size(1898, 0);
         this.barDockControlTop.Visible = false;
         // 
         // ribbonGroupEdit
         // 
         this.ribbonGroupEdit.Name = "ribbonGroupEdit";
         this.ribbonGroupEdit.Text = "ribbonGroupEdit";
         // 
         // btnRefresh
         // 
         this.btnRefresh.Caption = "btnRefresh";
         this.btnRefresh.Id = 2;
         this.btnRefresh.Name = "btnRefresh";
         // 
         // ribbonControl
         // 
         this.ribbonControl.AllowMinimizeRibbon = false;
         this.ribbonControl.AllowTrimPageText = false;
         this.ribbonControl.ApplicationButtonDropDownControl = this.barDockControlTop1;
         this.ribbonControl.ApplicationButtonText = null;
         this.ribbonControl.AutoSizeItems = true;
         this.ribbonControl.Dock = System.Windows.Forms.DockStyle.None;
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl.ExpandCollapseItem,
            this.btnDelete,
            this.btnPresent,
            this.btnNotPresent,
            this.btnAllowNegativeValues,
            this.btnNotAllowNegativeValues});
         this.ribbonControl.Location = new System.Drawing.Point(6, 6);
         this.ribbonControl.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
         this.ribbonControl.MaxItemId = 8;
         this.ribbonControl.Name = "ribbonControl";
         this.ribbonControl.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage});
         repositoryItemRibbonSearchEdit1.AllowFocused = false;
         repositoryItemRibbonSearchEdit1.AutoHeight = false;
         repositoryItemRibbonSearchEdit1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         editorButtonImageOptions1.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
         editorButtonImageOptions1.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("editorButtonImageOptions1.SvgImage")));
         repositoryItemRibbonSearchEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, true, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Clear, "", -1, true, false, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
         repositoryItemRibbonSearchEdit1.NullText = "Search";
         this.ribbonControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            repositoryItemRibbonSearchEdit1});
         this.ribbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.MacOffice;
         // 
         // 
         // 
         this.ribbonControl.SearchEditItem.AccessibleName = "Search Item";
         this.ribbonControl.SearchEditItem.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
         this.ribbonControl.SearchEditItem.EditWidth = 150;
         this.ribbonControl.SearchEditItem.Id = -5000;
         this.ribbonControl.SearchEditItem.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
         this.ribbonControl.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
         this.ribbonControl.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
         this.ribbonControl.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
         this.ribbonControl.ShowItemCaptionsInCaptionBar = true;
         this.ribbonControl.ShowPageHeadersInFormCaption = DevExpress.Utils.DefaultBoolean.False;
         this.ribbonControl.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
         this.ribbonControl.ShowQatLocationSelector = false;
         this.ribbonControl.ShowToolbarCustomizeItem = false;
         this.ribbonControl.Size = new System.Drawing.Size(908, 100);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         this.ribbonControl.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
         // 
         // barDockControlTop1
         // 
         this.barDockControlTop1.CausesValidation = false;
         this.barDockControlTop1.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop1.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop1.Manager = null;
         this.barDockControlTop1.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
         this.barDockControlTop1.Size = new System.Drawing.Size(1898, 0);
         this.barDockControlTop1.Visible = false;
         // 
         // btnDelete
         // 
         this.btnDelete.Caption = "btnDelete";
         this.btnDelete.Id = 1;
         this.btnDelete.Name = "btnDelete";
         this.btnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDeleteItemClick);
         // 
         // btnPresent
         // 
         this.btnPresent.Caption = "btnPresent";
         this.btnPresent.Id = 3;
         this.btnPresent.Name = "btnPresent";
         // 
         // btnNotPresent
         // 
         this.btnNotPresent.Caption = "btnNotPresent";
         this.btnNotPresent.Id = 4;
         this.btnNotPresent.Name = "btnNotPresent";
         // 
         // btnAllowNegativeValues
         // 
         this.btnAllowNegativeValues.Caption = "btnAllowNegativeValues";
         this.btnAllowNegativeValues.Id = 5;
         this.btnAllowNegativeValues.Name = "btnAllowNegativeValues";
         // 
         // btnNotAllowNegativeValues
         // 
         this.btnNotAllowNegativeValues.Caption = "btnNotAllowNegativeValues";
         this.btnNotAllowNegativeValues.Id = 6;
         this.btnNotAllowNegativeValues.Name = "btnNotAllowNegativeValues";
         // 
         // ribbonPage
         // 
         this.ribbonPage.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonGroupEdit1});
         this.ribbonPage.Name = "ribbonPage";
         // 
         // ribbonGroupEdit1
         // 
         this.ribbonGroupEdit1.ItemLinks.Add(this.btnDelete);
         this.ribbonGroupEdit1.Name = "ribbonGroupEdit1";
         // 
         // layoutItemRibbon
         // 
         this.layoutItemRibbon.Control = this.ribbonControl;
         this.layoutItemRibbon.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
         this.layoutItemRibbon.CustomizationFormText = "layoutItemRibbon";
         this.layoutItemRibbon.Location = new System.Drawing.Point(0, 0);
         this.layoutItemRibbon.Name = "layoutItemRibbon";
         this.layoutItemRibbon.Size = new System.Drawing.Size(910, 101);
         this.layoutItemRibbon.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.SupportHorzAlignment;
         this.layoutItemRibbon.Text = "layoutItemRibbon";
         this.layoutItemRibbon.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemRibbon.TextVisible = false;
         // 
         // SimulationOutputMappingView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.Name = "SimulationOutputMappingView";
         this.Size = new System.Drawing.Size(920, 811);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         this.layoutControl.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(repositoryItemRibbonSearchEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRibbon)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private UxGridControl gridControl;
      private UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem;
      private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl;
      private DevExpress.XtraBars.BarDockControl barDockControlTop1;
      private DevExpress.XtraBars.BarButtonItem btnDelete;
      private DevExpress.XtraBars.BarButtonItem btnPresent;
      private DevExpress.XtraBars.BarButtonItem btnNotPresent;
      private DevExpress.XtraBars.BarButtonItem btnAllowNegativeValues;
      private DevExpress.XtraBars.BarButtonItem btnNotAllowNegativeValues;
      private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage;
      private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonGroupEdit1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemRibbon;
      private DevExpress.XtraBars.BarDockControl barDockControlTop;
      private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonGroupEdit;
      private DevExpress.XtraBars.BarButtonItem btnRefresh;
   }
}
