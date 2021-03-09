using OSPSuite.UI.Controls;

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
         this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
         this.keysListBox = new DevExpress.XtraEditors.ListBoxControl();
         this.importButton = new OSPSuite.UI.Controls.UxSimpleButton();
         this.separatorComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.namingConventionComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.buttonAdd = new OSPSuite.UI.Controls.UxSimpleButton();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
         this.importButtonLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
         this.namingConventionLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.buttonAddLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.separatorControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.dataPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.chartPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.splitterItem2 = new DevExpress.XtraLayout.SplitterItem();
         this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesListBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.keysListBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.importButtonLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonAddLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dataPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // namesListBox
         // 
         this.namesListBox.Location = new System.Drawing.Point(12, 1087);
         this.namesListBox.Margin = new System.Windows.Forms.Padding(4);
         this.namesListBox.Name = "namesListBox";
         this.namesListBox.Size = new System.Drawing.Size(1480, 432);
         this.namesListBox.StyleController = this.layoutControl1;
         this.namesListBox.TabIndex = 4;
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.keysListBox);
         this.layoutControl1.Controls.Add(this.importButton);
         this.layoutControl1.Controls.Add(this.namesListBox);
         this.layoutControl1.Controls.Add(this.separatorComboBoxEdit);
         this.layoutControl1.Controls.Add(this.namingConventionComboBoxEdit);
         this.layoutControl1.Controls.Add(this.buttonAdd);
         this.layoutControl1.Location = new System.Drawing.Point(12, 12);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(1504, 1589);
         this.layoutControl1.TabIndex = 45;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // keysListBox
         // 
         this.keysListBox.Location = new System.Drawing.Point(12, 12);
         this.keysListBox.Margin = new System.Windows.Forms.Padding(4);
         this.keysListBox.Name = "keysListBox";
         this.keysListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.keysListBox.Size = new System.Drawing.Size(1480, 909);
         this.keysListBox.StyleController = this.layoutControl1;
         this.keysListBox.TabIndex = 9;
         // 
         // importButton
         // 
         this.importButton.Location = new System.Drawing.Point(965, 1523);
         this.importButton.Manager = null;
         this.importButton.Margin = new System.Windows.Forms.Padding(2);
         this.importButton.Name = "importButton";
         this.importButton.Shortcut = System.Windows.Forms.Keys.None;
         this.importButton.Size = new System.Drawing.Size(79, 22);
         this.importButton.StyleController = this.layoutControl1;
         this.importButton.TabIndex = 40;
         this.importButton.Text = "Import";
         // 
         // separatorComboBoxEdit
         // 
         this.separatorComboBoxEdit.Location = new System.Drawing.Point(315, 925);
         this.separatorComboBoxEdit.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
         this.separatorComboBoxEdit.Name = "separatorComboBoxEdit";
         this.separatorComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.separatorComboBoxEdit.Size = new System.Drawing.Size(1177, 48);
         this.separatorComboBoxEdit.StyleController = this.layoutControl1;
         this.separatorComboBoxEdit.TabIndex = 42;
         // 
         // namingConventionComboBoxEdit
         // 
         this.namingConventionComboBoxEdit.Location = new System.Drawing.Point(315, 1035);
         this.namingConventionComboBoxEdit.Margin = new System.Windows.Forms.Padding(4);
         this.namingConventionComboBoxEdit.Name = "namingConventionComboBoxEdit";
         this.namingConventionComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.namingConventionComboBoxEdit.Size = new System.Drawing.Size(1177, 48);
         this.namingConventionComboBoxEdit.StyleController = this.layoutControl1;
         this.namingConventionComboBoxEdit.TabIndex = 8;
         // 
         // buttonAdd
         // 
         this.buttonAdd.Location = new System.Drawing.Point(956, 977);
         this.buttonAdd.Manager = null;
         this.buttonAdd.Margin = new System.Windows.Forms.Padding(4);
         this.buttonAdd.Name = "buttonAdd";
         this.buttonAdd.Shortcut = System.Windows.Forms.Keys.None;
         this.buttonAdd.Size = new System.Drawing.Size(79, 22);
         this.buttonAdd.StyleController = this.layoutControl1;
         this.buttonAdd.TabIndex = 10;
         this.buttonAdd.Text = "Add keys";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem5,
            this.importButtonLayoutControlItem,
            this.emptySpaceItem2,
            this.layoutControlItem7,
            this.namingConventionLayout,
            this.buttonAddLayoutControlItem,
            this.separatorControlItem,
            this.emptySpaceItem1});
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(1504, 1589);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem5
         // 
         this.layoutControlItem5.Control = this.keysListBox;
         this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem5.Name = "layoutControlItem5";
         this.layoutControlItem5.Size = new System.Drawing.Size(1484, 913);
         this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem5.TextVisible = false;
         // 
         // importButtonLayoutControlItem
         // 
         this.importButtonLayoutControlItem.Control = this.importButton;
         this.importButtonLayoutControlItem.Location = new System.Drawing.Point(953, 1511);
         this.importButtonLayoutControlItem.Name = "importButtonLayoutControlItem";
         this.importButtonLayoutControlItem.Size = new System.Drawing.Size(531, 58);
         this.importButtonLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.importButtonLayoutControlItem.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 1511);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(953, 58);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItem7
         // 
         this.layoutControlItem7.Control = this.namesListBox;
         this.layoutControlItem7.Location = new System.Drawing.Point(0, 1075);
         this.layoutControlItem7.Name = "layoutControlItem7";
         this.layoutControlItem7.Size = new System.Drawing.Size(1484, 436);
         this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem7.TextVisible = false;
         // 
         // namingConventionLayout
         // 
         this.namingConventionLayout.Control = this.namingConventionComboBoxEdit;
         this.namingConventionLayout.Location = new System.Drawing.Point(0, 1023);
         this.namingConventionLayout.Name = "namingConventionLayout";
         this.namingConventionLayout.Size = new System.Drawing.Size(1484, 52);
         this.namingConventionLayout.TextSize = new System.Drawing.Size(300, 33);
         // 
         // buttonAddLayoutControlItem
         // 
         this.buttonAddLayoutControlItem.Control = this.buttonAdd;
         this.buttonAddLayoutControlItem.Location = new System.Drawing.Point(944, 965);
         this.buttonAddLayoutControlItem.Name = "buttonAddLayoutControlItem";
         this.buttonAddLayoutControlItem.Size = new System.Drawing.Size(540, 58);
         this.buttonAddLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.buttonAddLayoutControlItem.TextVisible = false;
         // 
         // separatorControlItem
         // 
         this.separatorControlItem.Control = this.separatorComboBoxEdit;
         this.separatorControlItem.Location = new System.Drawing.Point(0, 913);
         this.separatorControlItem.Name = "separatorControlItem";
         this.separatorControlItem.Size = new System.Drawing.Size(1484, 52);
         this.separatorControlItem.TextSize = new System.Drawing.Size(300, 33);
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.layoutControl1);
         this.layoutControl.Controls.Add(this.dataPanelControl);
         this.layoutControl.Controls.Add(this.chartPanelControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(4);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1252, 575, 650, 400);
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(3184, 1613);
         this.layoutControl.TabIndex = 38;
         // 
         // dataPanelControl
         // 
         this.dataPanelControl.Location = new System.Drawing.Point(1545, 12);
         this.dataPanelControl.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
         this.dataPanelControl.Name = "dataPanelControl";
         this.dataPanelControl.Size = new System.Drawing.Size(1627, 702);
         this.dataPanelControl.TabIndex = 44;
         // 
         // chartPanelControl
         // 
         this.chartPanelControl.Location = new System.Drawing.Point(1545, 743);
         this.chartPanelControl.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
         this.chartPanelControl.Name = "chartPanelControl";
         this.chartPanelControl.Size = new System.Drawing.Size(1627, 858);
         this.chartPanelControl.TabIndex = 43;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.splitterItem1,
            this.splitterItem2,
            this.layoutControlItem4});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(3184, 1613);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.chartPanelControl;
         this.layoutControlItem2.Location = new System.Drawing.Point(1533, 731);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(1631, 862);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.dataPanelControl;
         this.layoutControlItem3.Location = new System.Drawing.Point(1533, 0);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(1631, 706);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(1533, 706);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.ShowSplitGlyph = DevExpress.Utils.DefaultBoolean.True;
         this.splitterItem1.Size = new System.Drawing.Size(1631, 25);
         // 
         // splitterItem2
         // 
         this.splitterItem2.AllowHotTrack = true;
         this.splitterItem2.Location = new System.Drawing.Point(1508, 0);
         this.splitterItem2.Name = "splitterItem2";
         this.splitterItem2.ShowSplitGlyph = DevExpress.Utils.DefaultBoolean.True;
         this.splitterItem2.Size = new System.Drawing.Size(25, 1593);
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.layoutControl1;
         this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Size = new System.Drawing.Size(1508, 1593);
         this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem4.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 965);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(944, 58);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // ImportConfirmationView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Margin = new System.Windows.Forms.Padding(17, 14, 17, 14);
         this.Name = "ImportConfirmationView";
         this.Size = new System.Drawing.Size(3184, 1613);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namesListBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.keysListBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.importButtonLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.namingConventionLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonAddLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.dataPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private DevExpress.XtraEditors.ListBoxControl namesListBox;
      private DevExpress.XtraEditors.ComboBoxEdit namingConventionComboBoxEdit;
      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.ListBoxControl keysListBox;
      private OSPSuite.UI.Controls.UxSimpleButton buttonAdd;
      private OSPSuite.UI.Controls.UxSimpleButton importButton;
      private DevExpress.XtraEditors.ComboBoxEdit separatorComboBoxEdit;
      private DevExpress.XtraEditors.PanelControl dataPanelControl;
      private DevExpress.XtraEditors.PanelControl chartPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraLayout.SplitterItem splitterItem2;
      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
      private DevExpress.XtraLayout.LayoutControlItem importButtonLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
      private DevExpress.XtraLayout.LayoutControlItem namingConventionLayout;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
      private DevExpress.XtraLayout.LayoutControlItem buttonAddLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem separatorControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
