namespace OSPSuite.Presentation.Importer.Views
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
         this.namingConventionLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.buttonAddLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.keysLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.namesLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.gridLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem = new DevExpress.XtraLayout.SplitterItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesListBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysListBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonAddLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem)).BeginInit();
         this.SuspendLayout();
         // 
         // namesListBox
         // 
         this.namesListBox.Location = new System.Drawing.Point(12, 792);
         this.namesListBox.Margin = new System.Windows.Forms.Padding(6);
         this.namesListBox.Name = "namesListBox";
         this.namesListBox.Size = new System.Drawing.Size(1926, 1922);
         this.namesListBox.StyleController = this.layoutControl;
         this.namesListBox.TabIndex = 4;
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Controls.Add(this.buttonAdd);
         this.layoutControl.Controls.Add(this.keysListBox);
         this.layoutControl.Controls.Add(this.namingConventionComboBoxEdit);
         this.layoutControl.Controls.Add(this.namesListBox);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(6);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(3662, 2726);
         this.layoutControl.TabIndex = 38;
         this.layoutControl.Text = "layoutControl";
         // 
         // gridControl
         // 
         this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6);
         this.gridControl.Location = new System.Drawing.Point(1967, 12);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Margin = new System.Windows.Forms.Padding(6);
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(1683, 2702);
         this.gridControl.TabIndex = 39;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.DetailHeight = 722;
         this.gridView.FixedLineWidth = 4;
         this.gridView.GridControl = this.gridControl;
         this.gridView.Name = "gridView";
         // 
         // buttonAdd
         // 
         this.buttonAdd.Location = new System.Drawing.Point(12, 734);
         this.buttonAdd.Margin = new System.Windows.Forms.Padding(6);
         this.buttonAdd.Name = "buttonAdd";
         this.buttonAdd.Size = new System.Drawing.Size(1926, 54);
         this.buttonAdd.StyleController = this.layoutControl;
         this.buttonAdd.TabIndex = 10;
         this.buttonAdd.Text = "Add";
         // 
         // keysListBox
         // 
         this.keysListBox.Location = new System.Drawing.Point(12, 66);
         this.keysListBox.Margin = new System.Windows.Forms.Padding(6);
         this.keysListBox.Name = "keysListBox";
         this.keysListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.keysListBox.Size = new System.Drawing.Size(1926, 664);
         this.keysListBox.StyleController = this.layoutControl;
         this.keysListBox.TabIndex = 9;
         // 
         // namingConventionComboBoxEdit
         // 
         this.namingConventionComboBoxEdit.Location = new System.Drawing.Point(315, 12);
         this.namingConventionComboBoxEdit.Margin = new System.Windows.Forms.Padding(6);
         this.namingConventionComboBoxEdit.Name = "namingConventionComboBoxEdit";
         this.namingConventionComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.namingConventionComboBoxEdit.Size = new System.Drawing.Size(1623, 50);
         this.namingConventionComboBoxEdit.StyleController = this.layoutControl;
         this.namingConventionComboBoxEdit.TabIndex = 8;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.namingConventionLayout,
            this.buttonAddLayout,
            this.keysLayout,
            this.namesLayout,
            this.gridLayoutControlItem,
            this.splitterItem});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(3662, 2726);
         this.Root.TextVisible = false;
         // 
         // namingConventionLayout
         // 
         this.namingConventionLayout.Control = this.namingConventionComboBoxEdit;
         this.namingConventionLayout.Location = new System.Drawing.Point(0, 0);
         this.namingConventionLayout.Name = "namingConventionLayout";
         this.namingConventionLayout.Size = new System.Drawing.Size(1930, 54);
         this.namingConventionLayout.TextSize = new System.Drawing.Size(300, 33);
         // 
         // buttonAddLayout
         // 
         this.buttonAddLayout.Control = this.buttonAdd;
         this.buttonAddLayout.Location = new System.Drawing.Point(0, 722);
         this.buttonAddLayout.Name = "buttonAddLayout";
         this.buttonAddLayout.Size = new System.Drawing.Size(1930, 58);
         this.buttonAddLayout.TextSize = new System.Drawing.Size(0, 0);
         this.buttonAddLayout.TextVisible = false;
         // 
         // keysLayout
         // 
         this.keysLayout.Control = this.keysListBox;
         this.keysLayout.Location = new System.Drawing.Point(0, 54);
         this.keysLayout.Name = "namesLayout";
         this.keysLayout.Size = new System.Drawing.Size(1930, 668);
         this.keysLayout.TextSize = new System.Drawing.Size(0, 0);
         this.keysLayout.TextVisible = false;
         // 
         // namesLayout
         // 
         this.namesLayout.Control = this.namesListBox;
         this.namesLayout.Location = new System.Drawing.Point(0, 780);
         this.namesLayout.Name = "namesLayout";
         this.namesLayout.Size = new System.Drawing.Size(1930, 1926);
         this.namesLayout.TextSize = new System.Drawing.Size(0, 0);
         this.namesLayout.TextVisible = false;
         // 
         // gridLayoutControlItem
         // 
         this.gridLayoutControlItem.Control = this.gridControl;
         this.gridLayoutControlItem.Location = new System.Drawing.Point(1955, 0);
         this.gridLayoutControlItem.Name = "gridLayoutControlItem";
         this.gridLayoutControlItem.Size = new System.Drawing.Size(1687, 2706);
         this.gridLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.gridLayoutControlItem.TextVisible = false;
         // 
         // splitterItem
         // 
         this.splitterItem.AllowHotTrack = true;
         this.splitterItem.Location = new System.Drawing.Point(1930, 0);
         this.splitterItem.Name = "splitterItem1";
         this.splitterItem.Size = new System.Drawing.Size(25, 2706);
         // 
         // ImportConfirmationView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Margin = new System.Windows.Forms.Padding(17, 16, 17, 16);
         this.Name = "ImportConfirmationView";
         this.Size = new System.Drawing.Size(3662, 2726);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesListBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysListBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonAddLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem)).EndInit();
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
   }
}
