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
         this.chartPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.dataPanelControl = new DevExpress.XtraEditors.PanelControl();
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
         this.importButtonLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.chartPanelLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.dataPanelLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem = new DevExpress.XtraLayout.SplitterItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesListBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.chartPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataPanelControl)).BeginInit();
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
         ((System.ComponentModel.ISupportInitialize)(this.importButtonLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartPanelLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataPanelLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // namesListBox
         // 
         this.namesListBox.Location = new System.Drawing.Point(5, 378);
         this.namesListBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.namesListBox.Name = "namesListBox";
         this.namesListBox.Size = new System.Drawing.Size(770, 667);
         this.namesListBox.StyleController = this.layoutControl;
         this.namesListBox.TabIndex = 4;
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.chartPanelControl);
         this.layoutControl.Controls.Add(this.dataPanelControl);
         this.layoutControl.Controls.Add(this.importButton);
         this.layoutControl.Controls.Add(this.buttonAdd);
         this.layoutControl.Controls.Add(this.keysListBox);
         this.layoutControl.Controls.Add(this.namingConventionComboBoxEdit);
         this.layoutControl.Controls.Add(this.namesListBox);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(1465, 1074);
         this.layoutControl.TabIndex = 38;
         // 
         // chartPanelControl
         // 
         this.chartPanelControl.Location = new System.Drawing.Point(787, 485);
         this.chartPanelControl.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.chartPanelControl.Name = "chartPanelControl";
         this.chartPanelControl.Size = new System.Drawing.Size(673, 560);
         this.chartPanelControl.TabIndex = 41;
         // 
         // dataPanelControl
         // 
         this.dataPanelControl.Location = new System.Drawing.Point(787, 5);
         this.dataPanelControl.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.dataPanelControl.Name = "dataPanelControl";
         this.dataPanelControl.Size = new System.Drawing.Size(673, 468);
         this.dataPanelControl.TabIndex = 41;
         // 
         // importButton
         // 
         this.importButton.Location = new System.Drawing.Point(1302, 1047);
         this.importButton.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.importButton.Name = "importButton";
         this.importButton.Size = new System.Drawing.Size(158, 22);
         this.importButton.StyleController = this.layoutControl;
         this.importButton.TabIndex = 40;
         this.importButton.Text = "Import";
         // 
         // buttonAdd
         // 
         this.buttonAdd.Location = new System.Drawing.Point(633, 266);
         this.buttonAdd.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.buttonAdd.Name = "buttonAdd";
         this.buttonAdd.Size = new System.Drawing.Size(142, 22);
         this.buttonAdd.StyleController = this.layoutControl;
         this.buttonAdd.TabIndex = 10;
         this.buttonAdd.Text = "Add keys";
         // 
         // keysListBox
         // 
         this.keysListBox.Location = new System.Drawing.Point(5, 5);
         this.keysListBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.keysListBox.Name = "keysListBox";
         this.keysListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.keysListBox.Size = new System.Drawing.Size(770, 249);
         this.keysListBox.StyleController = this.layoutControl;
         this.keysListBox.TabIndex = 9;
         // 
         // namingConventionComboBoxEdit
         // 
         this.namingConventionComboBoxEdit.Location = new System.Drawing.Point(128, 266);
         this.namingConventionComboBoxEdit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.namingConventionComboBoxEdit.Name = "namingConventionComboBoxEdit";
         this.namingConventionComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.namingConventionComboBoxEdit.Size = new System.Drawing.Size(468, 20);
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
            this.importButtonLayoutControlItem,
            this.emptySpaceItem4,
            this.chartPanelLayoutControlItem,
            this.dataPanelLayoutControlItem,
            this.splitterItem,
            this.splitterItem1});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1465, 1074);
         this.Root.TextVisible = false;
         // 
         // keysLayout
         // 
         this.keysLayout.Control = this.keysListBox;
         this.keysLayout.Location = new System.Drawing.Point(0, 0);
         this.keysLayout.Name = "namesLayout";
         this.keysLayout.Size = new System.Drawing.Size(772, 251);
         this.keysLayout.TextSize = new System.Drawing.Size(0, 0);
         this.keysLayout.TextVisible = false;
         // 
         // namesLayout
         // 
         this.namesLayout.Control = this.namesListBox;
         this.namesLayout.Location = new System.Drawing.Point(0, 373);
         this.namesLayout.Name = "namesLayout";
         this.namesLayout.Size = new System.Drawing.Size(772, 669);
         this.namesLayout.TextSize = new System.Drawing.Size(0, 0);
         this.namesLayout.TextVisible = false;
         // 
         // namingConventionLayout
         // 
         this.namingConventionLayout.Control = this.namingConventionComboBoxEdit;
         this.namingConventionLayout.Location = new System.Drawing.Point(0, 261);
         this.namingConventionLayout.Name = "namingConventionLayout";
         this.namingConventionLayout.Size = new System.Drawing.Size(593, 24);
         this.namingConventionLayout.TextSize = new System.Drawing.Size(122, 13);
         // 
         // buttonAddLayout
         // 
         this.buttonAddLayout.Control = this.buttonAdd;
         this.buttonAddLayout.Location = new System.Drawing.Point(628, 261);
         this.buttonAddLayout.Name = "buttonAddLayout";
         this.buttonAddLayout.Size = new System.Drawing.Size(144, 24);
         this.buttonAddLayout.TextSize = new System.Drawing.Size(0, 0);
         this.buttonAddLayout.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(593, 261);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(35, 24);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 251);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(772, 10);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem3
         // 
         this.emptySpaceItem3.AllowHotTrack = false;
         this.emptySpaceItem3.Location = new System.Drawing.Point(0, 285);
         this.emptySpaceItem3.Name = "emptySpaceItem3";
         this.emptySpaceItem3.Size = new System.Drawing.Size(772, 88);
         this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
         // 
         // importButtonLayoutControlItem
         // 
         this.importButtonLayoutControlItem.Control = this.importButton;
         this.importButtonLayoutControlItem.Location = new System.Drawing.Point(1297, 1042);
         this.importButtonLayoutControlItem.Name = "importButtonLayoutControlItem";
         this.importButtonLayoutControlItem.Size = new System.Drawing.Size(160, 24);
         this.importButtonLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.importButtonLayoutControlItem.TextVisible = false;
         // 
         // emptySpaceItem4
         // 
         this.emptySpaceItem4.AllowHotTrack = false;
         this.emptySpaceItem4.Location = new System.Drawing.Point(0, 1042);
         this.emptySpaceItem4.Name = "emptySpaceItem4";
         this.emptySpaceItem4.Size = new System.Drawing.Size(1297, 24);
         this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
         // 
         // chartPanelLayoutControlItem
         // 
         this.chartPanelLayoutControlItem.Control = this.chartPanelControl;
         this.chartPanelLayoutControlItem.Location = new System.Drawing.Point(782, 480);
         this.chartPanelLayoutControlItem.Name = "chartPanelLayoutControlItem";
         this.chartPanelLayoutControlItem.Size = new System.Drawing.Size(675, 562);
         this.chartPanelLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.chartPanelLayoutControlItem.TextVisible = false;
         // 
         // dataPanelLayoutControlItem
         // 
         this.dataPanelLayoutControlItem.Control = this.dataPanelControl;
         this.dataPanelLayoutControlItem.Location = new System.Drawing.Point(782, 0);
         this.dataPanelLayoutControlItem.Name = "dataPanelLayoutControlItem";
         this.dataPanelLayoutControlItem.Size = new System.Drawing.Size(675, 470);
         this.dataPanelLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.dataPanelLayoutControlItem.TextVisible = false;
         // 
         // splitterItem
         // 
         this.splitterItem.AllowHotTrack = true;
         this.splitterItem.Location = new System.Drawing.Point(772, 0);
         this.splitterItem.Name = "splitterItem1";
         this.splitterItem.Size = new System.Drawing.Size(10, 1042);
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(782, 470);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(675, 10);
         // 
         // ImportConfirmationView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
         this.Name = "ImportConfirmationView";
         this.Size = new System.Drawing.Size(1465, 1074);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesListBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.chartPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataPanelControl)).EndInit();
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
         ((System.ComponentModel.ISupportInitialize)(this.importButtonLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartPanelLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataPanelLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem)).EndInit();
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
      private DevExpress.XtraLayout.LayoutControlItem importButtonLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
      private DevExpress.XtraEditors.PanelControl chartPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem chartPanelLayoutControlItem;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraEditors.PanelControl dataPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem dataPanelLayoutControlItem;
   }
}
