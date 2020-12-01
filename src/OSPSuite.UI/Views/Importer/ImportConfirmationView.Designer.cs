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
         this.buttonAdd = new DevExpress.XtraEditors.SimpleButton();
         this.separatorComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.chartPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.dataPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.importButton = new DevExpress.XtraEditors.SimpleButton();
         this.keysListBox = new DevExpress.XtraEditors.ListBoxControl();
         this.namingConventionComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.keysLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.namesLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.importButtonLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.chartPanelLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.dataPanelLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem = new DevExpress.XtraLayout.SplitterItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.separatorControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem5 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.namingConventionLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem6 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesListBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.separatorComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysListBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.importButtonLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartPanelLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataPanelLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem6)).BeginInit();
         this.SuspendLayout();
         // 
         // namesListBox
         // 
         this.namesListBox.Location = new System.Drawing.Point(12, 384);
         this.namesListBox.Margin = new System.Windows.Forms.Padding(2);
         this.namesListBox.Name = "namesListBox";
         this.namesListBox.Size = new System.Drawing.Size(910, 640);
         this.namesListBox.StyleController = this.layoutControl;
         this.namesListBox.TabIndex = 4;
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.buttonAdd);
         this.layoutControl.Controls.Add(this.separatorComboBoxEdit);
         this.layoutControl.Controls.Add(this.chartPanelControl);
         this.layoutControl.Controls.Add(this.dataPanelControl);
         this.layoutControl.Controls.Add(this.importButton);
         this.layoutControl.Controls.Add(this.keysListBox);
         this.layoutControl.Controls.Add(this.namingConventionComboBoxEdit);
         this.layoutControl.Controls.Add(this.namesListBox);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(2);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(1745, 1067);
         this.layoutControl.TabIndex = 38;
         // 
         // buttonAdd
         // 
         this.buttonAdd.Location = new System.Drawing.Point(754, 303);
         this.buttonAdd.Margin = new System.Windows.Forms.Padding(2);
         this.buttonAdd.Name = "buttonAdd";
         this.buttonAdd.Size = new System.Drawing.Size(168, 27);
         this.buttonAdd.StyleController = this.layoutControl;
         this.buttonAdd.TabIndex = 10;
         this.buttonAdd.Text = "Add keys";
         // 
         // separatorComboBoxEdit
         // 
         this.separatorComboBoxEdit.Location = new System.Drawing.Point(157, 267);
         this.separatorComboBoxEdit.Name = "separatorComboBoxEdit";
         this.separatorComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.separatorComboBoxEdit.Size = new System.Drawing.Size(765, 22);
         this.separatorComboBoxEdit.StyleController = this.layoutControl;
         this.separatorComboBoxEdit.TabIndex = 42;
         // 
         // chartPanelControl
         // 
         this.chartPanelControl.Location = new System.Drawing.Point(938, 482);
         this.chartPanelControl.Margin = new System.Windows.Forms.Padding(1);
         this.chartPanelControl.Name = "chartPanelControl";
         this.chartPanelControl.Size = new System.Drawing.Size(795, 542);
         this.chartPanelControl.TabIndex = 41;
         // 
         // dataPanelControl
         // 
         this.dataPanelControl.Location = new System.Drawing.Point(938, 12);
         this.dataPanelControl.Margin = new System.Windows.Forms.Padding(1);
         this.dataPanelControl.Name = "dataPanelControl";
         this.dataPanelControl.Size = new System.Drawing.Size(795, 454);
         this.dataPanelControl.TabIndex = 41;
         // 
         // importButton
         // 
         this.importButton.Location = new System.Drawing.Point(1548, 1028);
         this.importButton.Margin = new System.Windows.Forms.Padding(1);
         this.importButton.Name = "importButton";
         this.importButton.Size = new System.Drawing.Size(185, 27);
         this.importButton.StyleController = this.layoutControl;
         this.importButton.TabIndex = 40;
         this.importButton.Text = "Import";
         // 
         // keysListBox
         // 
         this.keysListBox.Location = new System.Drawing.Point(12, 12);
         this.keysListBox.Margin = new System.Windows.Forms.Padding(2);
         this.keysListBox.Name = "keysListBox";
         this.keysListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.keysListBox.Size = new System.Drawing.Size(910, 241);
         this.keysListBox.StyleController = this.layoutControl;
         this.keysListBox.TabIndex = 9;
         // 
         // namingConventionComboBoxEdit
         // 
         this.namingConventionComboBoxEdit.Location = new System.Drawing.Point(157, 344);
         this.namingConventionComboBoxEdit.Margin = new System.Windows.Forms.Padding(2);
         this.namingConventionComboBoxEdit.Name = "namingConventionComboBoxEdit";
         this.namingConventionComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.namingConventionComboBoxEdit.Size = new System.Drawing.Size(765, 22);
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
            this.emptySpaceItem2,
            this.emptySpaceItem3,
            this.importButtonLayoutControlItem,
            this.emptySpaceItem4,
            this.chartPanelLayoutControlItem,
            this.dataPanelLayoutControlItem,
            this.splitterItem,
            this.splitterItem1,
            this.separatorControlItem,
            this.emptySpaceItem1,
            this.layoutControlItem1,
            this.emptySpaceItem5,
            this.namingConventionLayout,
            this.emptySpaceItem6});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1745, 1067);
         this.Root.TextVisible = false;
         // 
         // keysLayout
         // 
         this.keysLayout.Control = this.keysListBox;
         this.keysLayout.Location = new System.Drawing.Point(0, 0);
         this.keysLayout.Name = "namesLayout";
         this.keysLayout.Size = new System.Drawing.Size(914, 245);
         this.keysLayout.TextSize = new System.Drawing.Size(0, 0);
         this.keysLayout.TextVisible = false;
         // 
         // namesLayout
         // 
         this.namesLayout.Control = this.namesListBox;
         this.namesLayout.Location = new System.Drawing.Point(0, 372);
         this.namesLayout.Name = "item0";
         this.namesLayout.Size = new System.Drawing.Size(914, 644);
         this.namesLayout.TextSize = new System.Drawing.Size(0, 0);
         this.namesLayout.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 245);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(914, 10);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem3
         // 
         this.emptySpaceItem3.AllowHotTrack = false;
         this.emptySpaceItem3.Location = new System.Drawing.Point(0, 358);
         this.emptySpaceItem3.Name = "emptySpaceItem3";
         this.emptySpaceItem3.Size = new System.Drawing.Size(914, 14);
         this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
         // 
         // importButtonLayoutControlItem
         // 
         this.importButtonLayoutControlItem.Control = this.importButton;
         this.importButtonLayoutControlItem.Location = new System.Drawing.Point(1536, 1016);
         this.importButtonLayoutControlItem.Name = "importButtonLayoutControlItem";
         this.importButtonLayoutControlItem.Size = new System.Drawing.Size(189, 31);
         this.importButtonLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.importButtonLayoutControlItem.TextVisible = false;
         // 
         // emptySpaceItem4
         // 
         this.emptySpaceItem4.AllowHotTrack = false;
         this.emptySpaceItem4.Location = new System.Drawing.Point(0, 1016);
         this.emptySpaceItem4.Name = "emptySpaceItem4";
         this.emptySpaceItem4.Size = new System.Drawing.Size(1536, 31);
         this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
         // 
         // chartPanelLayoutControlItem
         // 
         this.chartPanelLayoutControlItem.Control = this.chartPanelControl;
         this.chartPanelLayoutControlItem.Location = new System.Drawing.Point(926, 470);
         this.chartPanelLayoutControlItem.Name = "chartPanelLayoutControlItem";
         this.chartPanelLayoutControlItem.Size = new System.Drawing.Size(799, 546);
         this.chartPanelLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.chartPanelLayoutControlItem.TextVisible = false;
         // 
         // dataPanelLayoutControlItem
         // 
         this.dataPanelLayoutControlItem.Control = this.dataPanelControl;
         this.dataPanelLayoutControlItem.Location = new System.Drawing.Point(926, 0);
         this.dataPanelLayoutControlItem.Name = "dataPanelLayoutControlItem";
         this.dataPanelLayoutControlItem.Size = new System.Drawing.Size(799, 458);
         this.dataPanelLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.dataPanelLayoutControlItem.TextVisible = false;
         // 
         // splitterItem
         // 
         this.splitterItem.AllowHotTrack = true;
         this.splitterItem.Location = new System.Drawing.Point(914, 0);
         this.splitterItem.Name = "splitterItem1";
         this.splitterItem.Size = new System.Drawing.Size(12, 1016);
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(926, 458);
         this.splitterItem1.Name = "item1";
         this.splitterItem1.Size = new System.Drawing.Size(799, 12);
         // 
         // separatorControlItem
         // 
         this.separatorControlItem.Control = this.separatorComboBoxEdit;
         this.separatorControlItem.Location = new System.Drawing.Point(0, 255);
         this.separatorControlItem.Name = "separatorControlItem";
         this.separatorControlItem.Size = new System.Drawing.Size(914, 26);
         this.separatorControlItem.Text = "Separator";
         this.separatorControlItem.TextSize = new System.Drawing.Size(142, 16);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 291);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(742, 31);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.buttonAdd;
         this.layoutControlItem1.Location = new System.Drawing.Point(742, 291);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(172, 31);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // emptySpaceItem5
         // 
         this.emptySpaceItem5.AllowHotTrack = false;
         this.emptySpaceItem5.Location = new System.Drawing.Point(0, 281);
         this.emptySpaceItem5.Name = "emptySpaceItem5";
         this.emptySpaceItem5.Size = new System.Drawing.Size(914, 10);
         this.emptySpaceItem5.TextSize = new System.Drawing.Size(0, 0);
         // 
         // namingConventionLayout
         // 
         this.namingConventionLayout.Control = this.namingConventionComboBoxEdit;
         this.namingConventionLayout.Location = new System.Drawing.Point(0, 332);
         this.namingConventionLayout.Name = "namingConventionLayout";
         this.namingConventionLayout.Size = new System.Drawing.Size(914, 26);
         this.namingConventionLayout.TextSize = new System.Drawing.Size(142, 16);
         // 
         // emptySpaceItem6
         // 
         this.emptySpaceItem6.AllowHotTrack = false;
         this.emptySpaceItem6.Location = new System.Drawing.Point(0, 322);
         this.emptySpaceItem6.Name = "emptySpaceItem6";
         this.emptySpaceItem6.Size = new System.Drawing.Size(914, 10);
         this.emptySpaceItem6.TextSize = new System.Drawing.Size(0, 0);
         // 
         // ImportConfirmationView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
         this.Name = "ImportConfirmationView";
         this.Size = new System.Drawing.Size(1745, 1067);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesListBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.separatorComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysListBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.keysLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.importButtonLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartPanelLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataPanelLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem6)).EndInit();
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
      private DevExpress.XtraLayout.LayoutControlItem namesLayout;
      private DevExpress.XtraLayout.SplitterItem splitterItem;
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
      private DevExpress.XtraEditors.ComboBoxEdit separatorComboBoxEdit;
      private DevExpress.XtraLayout.LayoutControlItem separatorControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem5;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem6;
   }
}
