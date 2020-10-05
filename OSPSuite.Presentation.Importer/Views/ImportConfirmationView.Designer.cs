using System.Windows.Forms;

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
         this.buttonAdd = new DevExpress.XtraEditors.SimpleButton();
         this.keysListBox = new DevExpress.XtraEditors.ListBoxControl();
         this.namingConventionComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.namingConventionLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.namesLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.keysLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.buttonAddLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.gridControl = new DevExpress.XtraGrid.GridControl();
         this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesListBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.keysListBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonAddLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         this.SuspendLayout();
         // 
         // NamesListBox
         // 
         this.namesListBox.Location = new System.Drawing.Point(12, 279);
         this.namesListBox.Name = "NamesListBox";
         this.namesListBox.Size = new System.Drawing.Size(578, 399);
         this.namesListBox.StyleController = this.layoutControl;
         this.namesListBox.TabIndex = 4;
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.buttonAdd);
         this.layoutControl.Controls.Add(this.keysListBox);
         this.layoutControl.Controls.Add(this.namingConventionComboBoxEdit);
         this.layoutControl.Controls.Add(this.namesListBox);
         this.layoutControl.Location = new System.Drawing.Point(7, 5);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(600, 690);
         this.layoutControl.TabIndex = 38;
         this.layoutControl.Text = "layoutControl";
         // 
         // simpleButton1
         // 
         this.buttonAdd.Location = new System.Drawing.Point(12, 38);
         this.buttonAdd.Name = "buttonAdd";
         this.buttonAdd.Size = new System.Drawing.Size(576, 27);
         this.buttonAdd.StyleController = this.layoutControl;
         this.buttonAdd.TabIndex = 10;
         this.buttonAdd.Text = "Add";
         // 
         // layoutControlItem
         // 
         this.keysLayout.Control = this.keysListBox;
         this.keysLayout.Location = new System.Drawing.Point(0, 0);
         this.keysLayout.Name = "namesLayout";
         this.keysLayout.Size = new System.Drawing.Size(580, 26);
         this.keysLayout.TextSize = new System.Drawing.Size(101, 16);
         // 
         // KeysListBox
         // 
         this.keysListBox.Location = new System.Drawing.Point(12, 82);
         this.keysListBox.Name = "KeysListBox";
         this.keysListBox.Size = new System.Drawing.Size(578, 190);
         this.keysListBox.StyleController = this.layoutControl;
         this.keysListBox.TabIndex = 9;
         this.keysLayout.TextVisible = false;
         this.keysListBox.SelectionMode = SelectionMode.MultiExtended;
         // 
         // namingConventionComboBoxEdit
         // 
         this.namingConventionComboBoxEdit.Location = new System.Drawing.Point(116, 12);
         this.namingConventionComboBoxEdit.Name = "namingConventionComboBoxEdit";
         this.namingConventionComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.namingConventionComboBoxEdit.Size = new System.Drawing.Size(472, 22);
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
            this.namesLayout
         });
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(600, 690);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem
         // 
         this.namingConventionLayout.Control = this.namingConventionComboBoxEdit;
         this.namingConventionLayout.Location = new System.Drawing.Point(0, 0);
         this.namingConventionLayout.Name = "namingConventionLayout";
         this.namingConventionLayout.Size = new System.Drawing.Size(580, 26);
         this.namingConventionLayout.TextSize = new System.Drawing.Size(101, 16);
         // 
         // layoutControlItem1
         // 
         this.buttonAddLayout.Control = this.buttonAdd;
         this.buttonAddLayout.Location = new System.Drawing.Point(0, 0);
         this.buttonAddLayout.Name = "";
         this.buttonAddLayout.Size = new System.Drawing.Size(50, 40);
         this.buttonAddLayout.TextSize = new System.Drawing.Size(0, 0);
         this.buttonAddLayout.TextVisible = false;
         // 
         // layoutControlItem
         // 
         this.namesLayout.Control = this.namesListBox;
         this.namesLayout.Location = new System.Drawing.Point(10, 100);
         this.namesLayout.Name = "namesLayout";
         this.namesLayout.Size = new System.Drawing.Size(580, 26);
         this.namesLayout.TextSize = new System.Drawing.Size(101, 16);
         this.namesLayout.TextVisible = false;
         // 
         // gridControl
         // 
         this.gridControl.Location = new System.Drawing.Point(618, 20);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(697, 665);
         this.gridControl.TabIndex = 39;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.GridControl = this.gridControl;
         this.gridView.Name = "gridView";
         // 
         // ImportConfirmationView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.gridControl);
         this.Controls.Add(this.layoutControl);
         this.Name = "ImportConfirmationView";
         this.Size = new System.Drawing.Size(1318, 688);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesListBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.keysListBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonAddLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private DevExpress.XtraEditors.ListBoxControl namesListBox;
      private DevExpress.XtraEditors.ComboBoxEdit namingConventionComboBoxEdit;
      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem namingConventionLayout;
      private DevExpress.XtraLayout.LayoutControlItem namesLayout;
      private DevExpress.XtraLayout.LayoutControlItem keysLayout;
      private DevExpress.XtraGrid.GridControl gridControl;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView;
      private DevExpress.XtraEditors.ListBoxControl keysListBox;
      private DevExpress.XtraEditors.SimpleButton buttonAdd;
      private DevExpress.XtraLayout.LayoutControlItem buttonAddLayout;
   }
}
