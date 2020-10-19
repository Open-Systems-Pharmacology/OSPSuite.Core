using OSPSuite.Assets;

namespace OSPSuite.UI.Views.Importer
{
   partial class ImportConfirmationView
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
         this.namesListBox = new DevExpress.XtraEditors.ListBoxControl();
         this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.gridControl = new DevExpress.XtraGrid.GridControl();
         this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.buttonAdd = new DevExpress.XtraEditors.SimpleButton();
         this.keysListBox = new DevExpress.XtraEditors.ListBoxControl();
         this.namingConventionComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.keysLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.namesLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.gridLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem = new DevExpress.XtraLayout.SplitterItem();
         this.namingConventionLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.buttonAddLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.importButton = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesListBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysListBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonAddLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
         this.SuspendLayout();
         // 
         // namesListBox
         // 
         this.namesListBox.Location = new System.Drawing.Point(25, 859);
         this.namesListBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
         this.namesListBox.Name = "namesListBox";
         this.namesListBox.Size = new System.Drawing.Size(1910, 1781);
         this.namesListBox.StyleController = this.layoutControl;
         this.namesListBox.TabIndex = 4;
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.importButton);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Controls.Add(this.buttonAdd);
         this.layoutControl.Controls.Add(this.keysListBox);
         this.layoutControl.Controls.Add(this.namingConventionComboBoxEdit);
         this.layoutControl.Controls.Add(this.namesListBox);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(3662, 2727);
         this.layoutControl.TabIndex = 38;
         // 
         // gridControl
         // 
         this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.gridControl.Location = new System.Drawing.Point(1968, 25);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(1669, 2615);
         this.gridControl.TabIndex = 39;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.ColumnPanelRowHeight = 0;
         this.gridView.DetailHeight = 722;
         this.gridView.FixedLineWidth = 4;
         this.gridView.FooterPanelHeight = 0;
         this.gridView.GridControl = this.gridControl;
         this.gridView.GroupRowHeight = 0;
         this.gridView.LevelIndent = 0;
         this.gridView.Name = "gridView";
         this.gridView.PreviewIndent = 0;
         this.gridView.RowHeight = 0;
         this.gridView.ViewCaptionHeight = 0;
         // 
         // buttonAdd
         // 
         this.buttonAdd.Location = new System.Drawing.Point(1585, 719);
         this.buttonAdd.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
         this.buttonAdd.Name = "buttonAdd";
         this.buttonAdd.Size = new System.Drawing.Size(350, 54);
         this.buttonAdd.StyleController = this.layoutControl;
         this.buttonAdd.TabIndex = 10;
         this.buttonAdd.Text = "Add keys";
         // 
         // keysListBox
         // 
         this.keysListBox.Location = new System.Drawing.Point(25, 25);
         this.keysListBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
         this.keysListBox.Name = "keysListBox";
         this.keysListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.keysListBox.Size = new System.Drawing.Size(1910, 660);
         this.keysListBox.StyleController = this.layoutControl;
         this.keysListBox.TabIndex = 9;
         // 
         // namingConventionComboBoxEdit
         // 
         this.namingConventionComboBoxEdit.Location = new System.Drawing.Point(331, 719);
         this.namingConventionComboBoxEdit.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
         this.namingConventionComboBoxEdit.Name = "namingConventionComboBoxEdit";
         this.namingConventionComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.namingConventionComboBoxEdit.Size = new System.Drawing.Size(1161, 50);
         this.namingConventionComboBoxEdit.StyleController = this.layoutControl;
         this.namingConventionComboBoxEdit.TabIndex = 8;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.keysLayout,
            this.namesLayout,
            this.gridLayoutControlItem,
            this.splitterItem,
            this.namingConventionLayout,
            this.buttonAddLayout,
            this.emptySpaceItem1,
            this.emptySpaceItem2,
            this.emptySpaceItem3,
            this.layoutControlItem1,
            this.emptySpaceItem4});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(3662, 2727);
         this.Root.TextVisible = false;
         // 
         // keysLayout
         // 
         this.keysLayout.Control = this.keysListBox;
         this.keysLayout.Location = new System.Drawing.Point(0, 0);
         this.keysLayout.Name = "namesLayout";
         this.keysLayout.Size = new System.Drawing.Size(1918, 668);
         this.keysLayout.TextSize = new System.Drawing.Size(0, 0);
         this.keysLayout.TextVisible = false;
         // 
         // namesLayout
         // 
         this.namesLayout.Control = this.namesListBox;
         this.namesLayout.Location = new System.Drawing.Point(0, 834);
         this.namesLayout.Name = "namesLayout";
         this.namesLayout.Size = new System.Drawing.Size(1918, 1789);
         this.namesLayout.TextSize = new System.Drawing.Size(0, 0);
         this.namesLayout.TextVisible = false;
         // 
         // gridLayoutControlItem
         // 
         this.gridLayoutControlItem.Control = this.gridControl;
         this.gridLayoutControlItem.Location = new System.Drawing.Point(1943, 0);
         this.gridLayoutControlItem.Name = "gridLayoutControlItem";
         this.gridLayoutControlItem.Size = new System.Drawing.Size(1677, 2623);
         this.gridLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.gridLayoutControlItem.TextVisible = false;
         // 
         // splitterItem
         // 
         this.splitterItem.AllowHotTrack = true;
         this.splitterItem.Location = new System.Drawing.Point(1918, 0);
         this.splitterItem.Name = "splitterItem1";
         this.splitterItem.Size = new System.Drawing.Size(25, 2623);
         // 
         // namingConventionLayout
         // 
         this.namingConventionLayout.Control = this.namingConventionComboBoxEdit;
         this.namingConventionLayout.Location = new System.Drawing.Point(0, 694);
         this.namingConventionLayout.Name = "namingConventionLayout";
         this.namingConventionLayout.Size = new System.Drawing.Size(1475, 62);
         this.namingConventionLayout.TextSize = new System.Drawing.Size(300, 33);
         // 
         // buttonAddLayout
         // 
         this.buttonAddLayout.Control = this.buttonAdd;
         this.buttonAddLayout.Location = new System.Drawing.Point(1560, 694);
         this.buttonAddLayout.Name = "buttonAddLayout";
         this.buttonAddLayout.Size = new System.Drawing.Size(358, 62);
         this.buttonAddLayout.TextSize = new System.Drawing.Size(0, 0);
         this.buttonAddLayout.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(1475, 694);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(85, 62);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 668);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(1918, 26);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem3
         // 
         this.emptySpaceItem3.AllowHotTrack = false;
         this.emptySpaceItem3.Location = new System.Drawing.Point(0, 756);
         this.emptySpaceItem3.Name = "emptySpaceItem3";
         this.emptySpaceItem3.Size = new System.Drawing.Size(1918, 78);
         this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
         // 
         // importButton
         // 
         this.importButton.Location = new System.Drawing.Point(3249, 2648);
         this.importButton.Name = "importButton";
         this.importButton.Size = new System.Drawing.Size(388, 54);
         this.importButton.StyleController = this.layoutControl;
         this.importButton.TabIndex = 40;
         this.importButton.Text = Captions.Importer.ConfirmationImport;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.importButton;
         this.layoutControlItem1.Location = new System.Drawing.Point(3224, 2623);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(396, 62);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // emptySpaceItem4
         // 
         this.emptySpaceItem4.AllowHotTrack = false;
         this.emptySpaceItem4.Location = new System.Drawing.Point(0, 2623);
         this.emptySpaceItem4.Name = "emptySpaceItem4";
         this.emptySpaceItem4.Size = new System.Drawing.Size(3224, 62);
         this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
         // 
         // ImportConfirmationView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Margin = new System.Windows.Forms.Padding(17, 16, 17, 16);
         this.Name = "ImportConfirmationView";
         this.Size = new System.Drawing.Size(3662, 2727);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesListBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysListBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonAddLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private DevExpress.XtraEditors.ListBoxControl namesListBox;
      private DevExpress.XtraEditors.ComboBoxEdit namingConventionComboBoxEdit;
      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem namingConventionLayout;
      private DevExpress.XtraLayout.LayoutControlItem keysLayout;
      private DevExpress.XtraGrid.GridControl gridControl;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView;
      private DevExpress.XtraEditors.ListBoxControl keysListBox;
      private DevExpress.XtraEditors.SimpleButton buttonAdd;
      private DevExpress.XtraLayout.LayoutControlItem buttonAddLayout;
      private DevExpress.XtraLayout.LayoutControlItem namesLayout;
      private DevExpress.XtraLayout.LayoutControlItem gridLayoutControlItem;
      private DevExpress.XtraLayout.SplitterItem splitterItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
      private DevExpress.XtraEditors.SimpleButton importButton;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
   }
}
