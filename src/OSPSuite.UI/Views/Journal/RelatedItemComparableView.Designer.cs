using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Journal
{
   partial class RelatedItemComparableView
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
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnRunComparison = new DevExpress.XtraEditors.SimpleButton();
         this.gridControl = new UxGridControl();
         this.gridView = new UxGridView();
         this.lblWarning = new DevExpress.XtraEditors.LabelControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemWarning = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemComparableItems = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemRunComparison = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemWarning)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemComparableItems)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRunComparison)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.btnRunComparison);
         this.layoutControl1.Controls.Add(this.gridControl);
         this.layoutControl1.Controls.Add(this.lblWarning);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup;
         this.layoutControl1.Size = new System.Drawing.Size(175, 228);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // btnRunComparison
         // 
         this.btnRunComparison.Location = new System.Drawing.Point(71, 204);
         this.btnRunComparison.Name = "btnRunComparison";
         this.btnRunComparison.Size = new System.Drawing.Size(102, 22);
         this.btnRunComparison.StyleController = this.layoutControl1;
         this.btnRunComparison.TabIndex = 6;
         this.btnRunComparison.Text = "btnRunComparison";
         // 
         // gridControl
         // 
         this.gridControl.Location = new System.Drawing.Point(2, 35);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(171, 165);
         this.gridControl.TabIndex = 5;
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
         // 
         // lblWarning
         // 
         this.lblWarning.Location = new System.Drawing.Point(2, 2);
         this.lblWarning.Name = "lblWarning";
         this.lblWarning.Size = new System.Drawing.Size(50, 13);
         this.lblWarning.StyleController = this.layoutControl1;
         this.lblWarning.TabIndex = 4;
         this.lblWarning.Text = "lblWarning";
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemWarning,
            this.layoutItemComparableItems,
            this.layoutItemRunComparison,
            this.emptySpaceItem});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(175, 228);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemWarning
         // 
         this.layoutItemWarning.Control = this.lblWarning;
         this.layoutItemWarning.Location = new System.Drawing.Point(0, 0);
         this.layoutItemWarning.Name = "layoutItemWarning";
         this.layoutItemWarning.Size = new System.Drawing.Size(175, 17);
         this.layoutItemWarning.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemWarning.TextVisible = false;
         // 
         // layoutItemComparableItems
         // 
         this.layoutItemComparableItems.Control = this.gridControl;
         this.layoutItemComparableItems.Location = new System.Drawing.Point(0, 17);
         this.layoutItemComparableItems.Name = "layoutItemComparableItems";
         this.layoutItemComparableItems.Size = new System.Drawing.Size(175, 185);
         this.layoutItemComparableItems.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutItemComparableItems.TextSize = new System.Drawing.Size(136, 13);
         // 
         // layoutItemRunComparison
         // 
         this.layoutItemRunComparison.Control = this.btnRunComparison;
         this.layoutItemRunComparison.Location = new System.Drawing.Point(69, 202);
         this.layoutItemRunComparison.Name = "layoutItemRunComparison";
         this.layoutItemRunComparison.Size = new System.Drawing.Size(106, 26);
         this.layoutItemRunComparison.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemRunComparison.TextVisible = false;
         // 
         // emptySpaceItem
         // 
         this.emptySpaceItem.AllowHotTrack = false;
         this.emptySpaceItem.Location = new System.Drawing.Point(0, 202);
         this.emptySpaceItem.Name = "emptySpaceItem";
         this.emptySpaceItem.Size = new System.Drawing.Size(69, 26);
         this.emptySpaceItem.TextSize = new System.Drawing.Size(0, 0);
         // 
         // RelatedItemComparableView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "RelatedItemComparableView";
         this.Size = new System.Drawing.Size(175, 228);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemWarning)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemComparableItems)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRunComparison)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraEditors.SimpleButton btnRunComparison;
      private UxGridControl gridControl;
      private UxGridView gridView;
      private DevExpress.XtraEditors.LabelControl lblWarning;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemWarning;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemComparableItems;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemRunComparison;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem;
   }
}
