using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Journal
{
   partial class JournalView
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
         _gridViewBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.gridControl = new CustomGridControl();
         this.gridView = new CustomGridView();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.panelSearch = new DevExpress.XtraEditors.PanelControl();
         this.panelPreview = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemGridView = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemPreview = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.layoutGroupSearch = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutitemSearch = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelSearch)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelPreview)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPreview)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupSearch)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutitemSearch)).BeginInit();
         this.SuspendLayout();
         // 
         // gridControl
         // 
         this.gridControl.Cursor = System.Windows.Forms.Cursors.Default;
         this.gridControl.Location = new System.Drawing.Point(98, 73);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(430, 232);
         this.gridControl.TabIndex = 0;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.EnableColumnContextMenu = true;
         this.gridView.GridControl = this.gridControl;
         this.gridView.MultiSelect = false;
         this.gridView.Name = "gridView";
         this.gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.gridView.PreviewRowEdit = null;
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.panelSearch);
         this.layoutControl.Controls.Add(this.panelPreview);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(507, 196, 585, 470);
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(530, 425);
         this.layoutControl.TabIndex = 1;
         this.layoutControl.Text = "layoutControl1";
         // 
         // panelSearch
         // 
         this.panelSearch.Location = new System.Drawing.Point(110, 14);
         this.panelSearch.Name = "panelSearch";
         this.panelSearch.Size = new System.Drawing.Size(406, 43);
         this.panelSearch.TabIndex = 5;
         // 
         // panelPreview
         // 
         this.panelPreview.Location = new System.Drawing.Point(98, 314);
         this.panelPreview.Margin = new System.Windows.Forms.Padding(0);
         this.panelPreview.Name = "panelPreview";
         this.panelPreview.Size = new System.Drawing.Size(430, 109);
         this.panelPreview.TabIndex = 4;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.CustomizationFormText = "layoutControlGroup";
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemGridView,
            this.layoutItemPreview,
            this.splitterItem1,
            this.layoutGroupSearch});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "Root";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(530, 425);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemGridView
         // 
         this.layoutItemGridView.Control = this.gridControl;
         this.layoutItemGridView.CustomizationFormText = "layoutItemGridView";
         this.layoutItemGridView.Location = new System.Drawing.Point(0, 71);
         this.layoutItemGridView.MinSize = new System.Drawing.Size(200, 24);
         this.layoutItemGridView.Name = "layoutItemGridView";
         this.layoutItemGridView.Size = new System.Drawing.Size(530, 236);
         this.layoutItemGridView.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemGridView.TextSize = new System.Drawing.Size(93, 13);
         // 
         // layoutItemPreview
         // 
         this.layoutItemPreview.Control = this.panelPreview;
         this.layoutItemPreview.CustomizationFormText = "layoutItemPreview";
         this.layoutItemPreview.Location = new System.Drawing.Point(0, 312);
         this.layoutItemPreview.MinSize = new System.Drawing.Size(101, 17);
         this.layoutItemPreview.Name = "layoutItemPreview";
         this.layoutItemPreview.Size = new System.Drawing.Size(530, 113);
         this.layoutItemPreview.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemPreview.TextSize = new System.Drawing.Size(93, 13);
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(0, 307);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(530, 5);
         // 
         // layoutGroupSearch
         // 
         this.layoutGroupSearch.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutitemSearch});
         this.layoutGroupSearch.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupSearch.Name = "layoutGroupSearch";
         this.layoutGroupSearch.Size = new System.Drawing.Size(530, 71);
         this.layoutGroupSearch.TextVisible = false;
         // 
         // layoutitemSearch
         // 
         this.layoutitemSearch.Control = this.panelSearch;
         this.layoutitemSearch.Location = new System.Drawing.Point(0, 0);
         this.layoutitemSearch.Name = "layoutitemSearch";
         this.layoutitemSearch.Size = new System.Drawing.Size(506, 47);
         this.layoutitemSearch.TextSize = new System.Drawing.Size(93, 13);
         // 
         // JournalView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "JournalView";
         this.Size = new System.Drawing.Size(530, 425);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelSearch)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelPreview)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPreview)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupSearch)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutitemSearch)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private CustomGridControl gridControl;
      private CustomGridView gridView;
      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraEditors.PanelControl panelPreview;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemGridView;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemPreview;
      private DevExpress.XtraEditors.PanelControl panelSearch;
      private DevExpress.XtraLayout.LayoutControlItem layoutitemSearch;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupSearch;
   }
}
