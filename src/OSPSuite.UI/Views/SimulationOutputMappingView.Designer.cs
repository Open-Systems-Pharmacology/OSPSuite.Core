
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
         this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.deleteButton = new DevExpress.XtraEditors.SimpleButton();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new OSPSuite.UI.Controls.UxGridView();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.deleteButtonControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.barDockControlTop1 = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.ribbonGroupEdit = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
         this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.deleteButtonControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.deleteButton);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(1);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1414, -904, 812, 500);
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(789, 659);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl";
         // 
         // deleteButton
         // 
         this.deleteButton.Location = new System.Drawing.Point(11, 10);
         this.deleteButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.deleteButton.Name = "deleteButton";
         this.deleteButton.Size = new System.Drawing.Size(71, 22);
         this.deleteButton.StyleController = this.layoutControl;
         this.deleteButton.TabIndex = 5;
         this.deleteButton.Text = "deleteButton";
         // 
         // gridControl
         // 
         this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(0);
         this.gridControl.Location = new System.Drawing.Point(11, 36);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Margin = new System.Windows.Forms.Padding(1);
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(767, 613);
         this.gridControl.TabIndex = 4;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.ColumnPanelRowHeight = 0;
         this.gridView.DetailHeight = 138;
         this.gridView.EnableColumnContextMenu = true;
         this.gridView.FooterPanelHeight = 0;
         this.gridView.GridControl = this.gridControl;
         this.gridView.GroupRowHeight = 0;
         this.gridView.LevelIndent = 0;
         this.gridView.MultiSelect = false;
         this.gridView.Name = "gridView";
         this.gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
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
            this.deleteButtonControlItem,
            this.emptySpaceItem1});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(789, 659);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem
         // 
         this.layoutControlItem.Control = this.gridControl;
         this.layoutControlItem.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItem.Name = "layoutControlItem";
         this.layoutControlItem.Size = new System.Drawing.Size(771, 617);
         this.layoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem.TextVisible = false;
         // 
         // deleteButtonControlItem
         // 
         this.deleteButtonControlItem.Control = this.deleteButton;
         this.deleteButtonControlItem.Location = new System.Drawing.Point(0, 0);
         this.deleteButtonControlItem.Name = "deleteButtonControlItem";
         this.deleteButtonControlItem.Size = new System.Drawing.Size(75, 26);
         this.deleteButtonControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.deleteButtonControlItem.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(75, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(696, 26);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
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
         // SimulationOutputMappingView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Margin = new System.Windows.Forms.Padding(2);
         this.Name = "SimulationOutputMappingView";
         this.Size = new System.Drawing.Size(789, 659);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.deleteButtonControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private UxGridControl gridControl;
      private UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem;
      private DevExpress.XtraBars.BarDockControl barDockControlTop1;
      private DevExpress.XtraBars.BarDockControl barDockControlTop;
      private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonGroupEdit;
      private DevExpress.XtraBars.BarButtonItem btnRefresh;
      private DevExpress.XtraEditors.SimpleButton deleteButton;
      private DevExpress.XtraLayout.LayoutControlItem deleteButtonControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
