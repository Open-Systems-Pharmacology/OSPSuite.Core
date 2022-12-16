using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using OSPSuite.Assets;

namespace OSPSuite.UI.Views.Importer
{
   partial class CSVSeparatorSelectorView
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
         this.mainLayoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.fileNameLabel = new DevExpress.XtraEditors.LabelControl();
         this.previewMemoEdit = new DevExpress.XtraEditors.MemoEdit();
         this.decimalSeparatorComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.columnSeparatorComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.columnSeparatorLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.decimalSeparatorLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.mainLayoutControl)).BeginInit();
         this.mainLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.previewMemoEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.decimalSeparatorComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnSeparatorComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnSeparatorLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.decimalSeparatorLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
         this.SuspendLayout();
         // 
         // tablePanel
         // 
         this.tablePanel.Location = new System.Drawing.Point(0, 215);
         this.tablePanel.Size = new System.Drawing.Size(562, 43);
         // 
         // mainLayoutControl
         // 
         this.mainLayoutControl.Controls.Add(this.fileNameLabel);
         this.mainLayoutControl.Controls.Add(this.previewMemoEdit);
         this.mainLayoutControl.Controls.Add(this.decimalSeparatorComboBoxEdit);
         this.mainLayoutControl.Controls.Add(this.columnSeparatorComboBoxEdit);
         this.mainLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.mainLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.mainLayoutControl.Margin = new System.Windows.Forms.Padding(1);
         this.mainLayoutControl.Name = "mainLayoutControl";
         this.mainLayoutControl.Root = this.Root;
         this.mainLayoutControl.Size = new System.Drawing.Size(562, 215);
         this.mainLayoutControl.TabIndex = 38;
         this.mainLayoutControl.Text = "mainLayoutControl";
         // 
         // fileNameLabel
         // 
         this.fileNameLabel.Location = new System.Drawing.Point(12, 12);
         this.fileNameLabel.Name = "fileNameLabel";
         this.fileNameLabel.Size = new System.Drawing.Size(66, 13);
         this.fileNameLabel.StyleController = this.mainLayoutControl;
         this.fileNameLabel.TabIndex = 8;
         this.fileNameLabel.Text = "fileNameLabel";
         // 
         // previewMemoEdit
         // 
         this.previewMemoEdit.EditValue = "preview\r\npreview\r\npreviewpreview";
         this.previewMemoEdit.Enabled = false;
         this.previewMemoEdit.Location = new System.Drawing.Point(12, 29);
         this.previewMemoEdit.Name = "previewMemoEdit";
         this.previewMemoEdit.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
         this.previewMemoEdit.Size = new System.Drawing.Size(538, 106);
         this.previewMemoEdit.StyleController = this.mainLayoutControl;
         this.previewMemoEdit.TabIndex = 7;
         // 
         // decimalSeparatorComboBoxEdit
         // 
         this.decimalSeparatorComboBoxEdit.Location = new System.Drawing.Point(107, 173);
         this.decimalSeparatorComboBoxEdit.Margin = new System.Windows.Forms.Padding(1);
         this.decimalSeparatorComboBoxEdit.Name = "decimalSeparatorComboBoxEdit";
         this.decimalSeparatorComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.decimalSeparatorComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
         this.decimalSeparatorComboBoxEdit.Size = new System.Drawing.Size(62, 20);
         this.decimalSeparatorComboBoxEdit.StyleController = this.mainLayoutControl;
         this.decimalSeparatorComboBoxEdit.TabIndex = 6;
         // 
         // columnSeparatorComboBoxEdit
         // 
         this.columnSeparatorComboBoxEdit.Location = new System.Drawing.Point(107, 149);
         this.columnSeparatorComboBoxEdit.Margin = new System.Windows.Forms.Padding(1);
         this.columnSeparatorComboBoxEdit.Name = "columnSeparatorComboBoxEdit";
         this.columnSeparatorComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.columnSeparatorComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
         this.columnSeparatorComboBoxEdit.Size = new System.Drawing.Size(62, 20);
         this.columnSeparatorComboBoxEdit.StyleController = this.mainLayoutControl;
         this.columnSeparatorComboBoxEdit.TabIndex = 5;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.columnSeparatorLayoutControlItem,
            this.emptySpaceItem4,
            this.decimalSeparatorLayoutControlItem,
            this.emptySpaceItem1,
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.emptySpaceItem2,
            this.emptySpaceItem3});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(562, 215);
         this.Root.TextVisible = false;
         // 
         // columnSeparatorLayoutControlItem
         // 
         this.columnSeparatorLayoutControlItem.Control = this.columnSeparatorComboBoxEdit;
         this.columnSeparatorLayoutControlItem.Location = new System.Drawing.Point(0, 137);
         this.columnSeparatorLayoutControlItem.Name = "columnSeparatorLayoutControlItem";
         this.columnSeparatorLayoutControlItem.Size = new System.Drawing.Size(161, 24);
         this.columnSeparatorLayoutControlItem.Text = "columnSeparator";
         this.columnSeparatorLayoutControlItem.TextSize = new System.Drawing.Size(83, 13);
         // 
         // emptySpaceItem4
         // 
         this.emptySpaceItem4.AllowHotTrack = false;
         this.emptySpaceItem4.Location = new System.Drawing.Point(0, 127);
         this.emptySpaceItem4.Name = "emptySpaceItem4";
         this.emptySpaceItem4.Size = new System.Drawing.Size(542, 10);
         this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
         // 
         // decimalSeparatorLayoutControlItem
         // 
         this.decimalSeparatorLayoutControlItem.Control = this.decimalSeparatorComboBoxEdit;
         this.decimalSeparatorLayoutControlItem.Location = new System.Drawing.Point(0, 161);
         this.decimalSeparatorLayoutControlItem.Name = "decimalSeparatorLayoutControlItem";
         this.decimalSeparatorLayoutControlItem.Size = new System.Drawing.Size(161, 24);
         this.decimalSeparatorLayoutControlItem.Text = "decimalSeparator";
         this.decimalSeparatorLayoutControlItem.TextSize = new System.Drawing.Size(83, 13);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 185);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(542, 10);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.previewMemoEdit;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 17);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(542, 110);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.fileNameLabel;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(542, 17);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(161, 137);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(381, 24);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem3
         // 
         this.emptySpaceItem3.AllowHotTrack = false;
         this.emptySpaceItem3.Location = new System.Drawing.Point(161, 161);
         this.emptySpaceItem3.Name = "emptySpaceItem3";
         this.emptySpaceItem3.Size = new System.Drawing.Size(381, 24);
         this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
         // 
         // CSVSeparatorSelectorView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "Separator Selection";
         this.ClientSize = new System.Drawing.Size(562, 258);
         this.Controls.Add(this.mainLayoutControl);
         this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
         this.Name = "CSVSeparatorSelectorView";
         this.Text = "Separator Selection";
         this.Controls.SetChildIndex(this.tablePanel, 0);
         this.Controls.SetChildIndex(this.mainLayoutControl, 0);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.mainLayoutControl)).EndInit();
         this.mainLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.previewMemoEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.decimalSeparatorComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnSeparatorComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnSeparatorLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.decimalSeparatorLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl mainLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.ComboBoxEdit columnSeparatorComboBoxEdit;
      private DevExpress.XtraLayout.LayoutControlItem columnSeparatorLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
        private DevExpress.XtraEditors.ComboBoxEdit decimalSeparatorComboBoxEdit;
        private DevExpress.XtraLayout.LayoutControlItem decimalSeparatorLayoutControlItem;
        private DevExpress.XtraEditors.MemoEdit previewMemoEdit;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.LabelControl fileNameLabel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
    }
}
