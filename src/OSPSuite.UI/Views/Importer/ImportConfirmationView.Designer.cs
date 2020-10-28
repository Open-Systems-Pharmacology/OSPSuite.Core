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
         this.datagridControl = new DevExpress.XtraGrid.GridControl();
         this.dataGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.chartPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.importButton = new DevExpress.XtraEditors.SimpleButton();
         this.buttonAdd = new DevExpress.XtraEditors.SimpleButton();
         this.keysListBox = new DevExpress.XtraEditors.ListBoxControl();
         this.namingConventionComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.keysLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.namesLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.namingConventionLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.buttonAddLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem = new DevExpress.XtraLayout.SplitterItem();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesListBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.datagridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysListBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonAddLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // namesListBox
         // 
         this.namesListBox.Location = new System.Drawing.Point(6, 464);
         this.namesListBox.Name = "namesListBox";
         this.namesListBox.Size = new System.Drawing.Size(898, 823);
         this.namesListBox.StyleController = this.layoutControl;
         this.namesListBox.TabIndex = 4;
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.datagridControl);
         this.layoutControl.Controls.Add(this.chartPanelControl);
         this.layoutControl.Controls.Add(this.importButton);
         this.layoutControl.Controls.Add(this.buttonAdd);
         this.layoutControl.Controls.Add(this.keysListBox);
         this.layoutControl.Controls.Add(this.namingConventionComboBoxEdit);
         this.layoutControl.Controls.Add(this.namesListBox);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(1709, 1322);
         this.layoutControl.TabIndex = 38;
         // 
         // datagridControl
         // 
         this.datagridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
         this.datagridControl.Location = new System.Drawing.Point(918, 6);
         this.datagridControl.MainView = this.dataGridView;
         this.datagridControl.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.datagridControl.Name = "datagridControl";
         this.datagridControl.Size = new System.Drawing.Size(785, 576);
         this.datagridControl.TabIndex = 42;
         this.datagridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dataGridView});
         // 
         // dataGridView
         // 
         this.dataGridView.ColumnPanelRowHeight = 0;
         this.dataGridView.DetailHeight = 170;
         this.dataGridView.FixedLineWidth = 1;
         this.dataGridView.FooterPanelHeight = 0;
         this.dataGridView.GridControl = this.datagridControl;
         this.dataGridView.GroupRowHeight = 0;
         this.dataGridView.LevelIndent = 0;
         this.dataGridView.Name = "dataGridView";
         this.dataGridView.OptionsBehavior.Editable = false;
         this.dataGridView.PreviewIndent = 0;
         this.dataGridView.RowHeight = 0;
         this.dataGridView.ViewCaptionHeight = 0;
         // 
         // chartPanelControl
         // 
         this.chartPanelControl.Location = new System.Drawing.Point(918, 596);
         this.chartPanelControl.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.chartPanelControl.Name = "chartPanelControl";
         this.chartPanelControl.Size = new System.Drawing.Size(785, 691);
         this.chartPanelControl.TabIndex = 41;
         // 
         // importButton
         // 
         this.importButton.Location = new System.Drawing.Point(1519, 1289);
         this.importButton.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.importButton.Name = "importButton";
         this.importButton.Size = new System.Drawing.Size(184, 27);
         this.importButton.StyleController = this.layoutControl;
         this.importButton.TabIndex = 40;
         this.importButton.Text = "Import";
         // 
         // buttonAdd
         // 
         this.buttonAdd.Location = new System.Drawing.Point(738, 327);
         this.buttonAdd.Name = "buttonAdd";
         this.buttonAdd.Size = new System.Drawing.Size(166, 27);
         this.buttonAdd.StyleController = this.layoutControl;
         this.buttonAdd.TabIndex = 10;
         this.buttonAdd.Text = "Add keys";
         // 
         // keysListBox
         // 
         this.keysListBox.Location = new System.Drawing.Point(6, 6);
         this.keysListBox.Name = "keysListBox";
         this.keysListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.keysListBox.Size = new System.Drawing.Size(898, 307);
         this.keysListBox.StyleController = this.layoutControl;
         this.keysListBox.TabIndex = 9;
         // 
         // namingConventionComboBoxEdit
         // 
         this.namingConventionComboBoxEdit.Location = new System.Drawing.Point(149, 327);
         this.namingConventionComboBoxEdit.Name = "namingConventionComboBoxEdit";
         this.namingConventionComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.namingConventionComboBoxEdit.Size = new System.Drawing.Size(547, 22);
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
            this.namingConventionLayout,
            this.buttonAddLayout,
            this.emptySpaceItem1,
            this.emptySpaceItem2,
            this.emptySpaceItem3,
            this.layoutControlItem1,
            this.emptySpaceItem4,
            this.layoutControlItem2,
            this.splitterItem,
            this.layoutControlItem3,
            this.splitterItem1});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1709, 1322);
         this.Root.TextVisible = false;
         // 
         // keysLayout
         // 
         this.keysLayout.Control = this.keysListBox;
         this.keysLayout.Location = new System.Drawing.Point(0, 0);
         this.keysLayout.Name = "namesLayout";
         this.keysLayout.Size = new System.Drawing.Size(900, 309);
         this.keysLayout.TextSize = new System.Drawing.Size(0, 0);
         this.keysLayout.TextVisible = false;
         // 
         // namesLayout
         // 
         this.namesLayout.Control = this.namesListBox;
         this.namesLayout.Location = new System.Drawing.Point(0, 458);
         this.namesLayout.Name = "namesLayout";
         this.namesLayout.Size = new System.Drawing.Size(900, 825);
         this.namesLayout.TextSize = new System.Drawing.Size(0, 0);
         this.namesLayout.TextVisible = false;
         // 
         // namingConventionLayout
         // 
         this.namingConventionLayout.Control = this.namingConventionComboBoxEdit;
         this.namingConventionLayout.Location = new System.Drawing.Point(0, 321);
         this.namingConventionLayout.Name = "namingConventionLayout";
         this.namingConventionLayout.Size = new System.Drawing.Size(692, 29);
         this.namingConventionLayout.TextSize = new System.Drawing.Size(142, 16);
         // 
         // buttonAddLayout
         // 
         this.buttonAddLayout.Control = this.buttonAdd;
         this.buttonAddLayout.Location = new System.Drawing.Point(732, 321);
         this.buttonAddLayout.Name = "buttonAddLayout";
         this.buttonAddLayout.Size = new System.Drawing.Size(168, 29);
         this.buttonAddLayout.TextSize = new System.Drawing.Size(0, 0);
         this.buttonAddLayout.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(692, 321);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(40, 29);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 309);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(900, 12);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem3
         // 
         this.emptySpaceItem3.AllowHotTrack = false;
         this.emptySpaceItem3.Location = new System.Drawing.Point(0, 350);
         this.emptySpaceItem3.Name = "emptySpaceItem3";
         this.emptySpaceItem3.Size = new System.Drawing.Size(900, 108);
         this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.importButton;
         this.layoutControlItem1.Location = new System.Drawing.Point(1513, 1283);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(186, 29);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // emptySpaceItem4
         // 
         this.emptySpaceItem4.AllowHotTrack = false;
         this.emptySpaceItem4.Location = new System.Drawing.Point(0, 1283);
         this.emptySpaceItem4.Name = "emptySpaceItem4";
         this.emptySpaceItem4.Size = new System.Drawing.Size(1513, 29);
         this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.chartPanelControl;
         this.layoutControlItem2.Location = new System.Drawing.Point(912, 590);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(787, 693);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // splitterItem
         // 
         this.splitterItem.AllowHotTrack = true;
         this.splitterItem.Location = new System.Drawing.Point(900, 0);
         this.splitterItem.Name = "splitterItem1";
         this.splitterItem.Size = new System.Drawing.Size(12, 1283);
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.datagridControl;
         this.layoutControlItem3.Location = new System.Drawing.Point(912, 0);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(787, 578);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(912, 578);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(787, 12);
         // 
         // ImportConfirmationView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.Name = "ImportConfirmationView";
         this.Size = new System.Drawing.Size(1709, 1322);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesListBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.datagridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysListBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonAddLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private DevExpress.XtraEditors.ListBoxControl namesListBox;
      private DevExpress.XtraEditors.ComboBoxEdit namingConventionComboBoxEdit;
      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem namingConventionLayout;
      private DevExpress.XtraLayout.LayoutControlItem keysLayout;
      private DevExpress.XtraEditors.ListBoxControl keysListBox;
      private DevExpress.XtraEditors.SimpleButton buttonAdd;
      private DevExpress.XtraLayout.LayoutControlItem buttonAddLayout;
      private DevExpress.XtraLayout.LayoutControlItem namesLayout;
      private DevExpress.XtraLayout.SplitterItem splitterItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
      private DevExpress.XtraEditors.SimpleButton importButton;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
      private DevExpress.XtraEditors.PanelControl chartPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraGrid.GridControl datagridControl;
      private DevExpress.XtraGrid.Views.Grid.GridView dataGridView;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
   }
}
