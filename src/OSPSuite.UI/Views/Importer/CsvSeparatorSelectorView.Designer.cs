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
         this.previewMemoEdit = new DevExpress.XtraEditors.MemoEdit();
         this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
         this.separatorComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.columnSeparatorLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.decimalSeparatorLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.fileNameLabel = new DevExpress.XtraEditors.LabelControl();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.mainLayoutControl)).BeginInit();
         this.mainLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.previewMemoEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnSeparatorLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.decimalSeparatorLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // tablePanel
         // 
         this.tablePanel.Location = new System.Drawing.Point(0, 209);
         this.tablePanel.Size = new System.Drawing.Size(562, 43);
         // 
         // mainLayoutControl
         // 
         this.mainLayoutControl.Controls.Add(this.fileNameLabel);
         this.mainLayoutControl.Controls.Add(this.previewMemoEdit);
         this.mainLayoutControl.Controls.Add(this.comboBoxEdit1);
         this.mainLayoutControl.Controls.Add(this.separatorComboBoxEdit);
         this.mainLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.mainLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.mainLayoutControl.Margin = new System.Windows.Forms.Padding(1);
         this.mainLayoutControl.Name = "mainLayoutControl";
         this.mainLayoutControl.Root = this.Root;
         this.mainLayoutControl.Size = new System.Drawing.Size(562, 209);
         this.mainLayoutControl.TabIndex = 38;
         this.mainLayoutControl.Text = "mainLayoutControl";
         // 
         // previewMemoEdit
         // 
         this.previewMemoEdit.EditValue = "preview\r\npreview\r\npreview";
         this.previewMemoEdit.Enabled = false;
         this.previewMemoEdit.Location = new System.Drawing.Point(12, 29);
         this.previewMemoEdit.Name = "previewMemoEdit";
         this.previewMemoEdit.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
         this.previewMemoEdit.Size = new System.Drawing.Size(538, 42);
         this.previewMemoEdit.StyleController = this.mainLayoutControl;
         this.previewMemoEdit.TabIndex = 7;
         // 
         // comboBoxEdit1
         // 
         this.comboBoxEdit1.Location = new System.Drawing.Point(197, 133);
         this.comboBoxEdit1.Margin = new System.Windows.Forms.Padding(1);
         this.comboBoxEdit1.Name = "comboBoxEdit1";
         this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.comboBoxEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
         this.comboBoxEdit1.Size = new System.Drawing.Size(353, 20);
         this.comboBoxEdit1.StyleController = this.mainLayoutControl;
         this.comboBoxEdit1.TabIndex = 6;
         // 
         // separatorComboBoxEdit
         // 
         this.separatorComboBoxEdit.Location = new System.Drawing.Point(197, 109);
         this.separatorComboBoxEdit.Margin = new System.Windows.Forms.Padding(1);
         this.separatorComboBoxEdit.Name = "separatorComboBoxEdit";
         this.separatorComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.separatorComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
         this.separatorComboBoxEdit.Size = new System.Drawing.Size(353, 20);
         this.separatorComboBoxEdit.StyleController = this.mainLayoutControl;
         this.separatorComboBoxEdit.TabIndex = 5;
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
            this.layoutControlItem2});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(562, 209);
         this.Root.TextVisible = false;
         // 
         // columnSeparatorLayoutControlItem
         // 
         this.columnSeparatorLayoutControlItem.Control = this.separatorComboBoxEdit;
         this.columnSeparatorLayoutControlItem.Location = new System.Drawing.Point(0, 97);
         this.columnSeparatorLayoutControlItem.Name = "columnSeparatorLayoutControlItem";
         this.columnSeparatorLayoutControlItem.Size = new System.Drawing.Size(542, 24);
         this.columnSeparatorLayoutControlItem.TextSize = new System.Drawing.Size(173, 13);
         // 
         // emptySpaceItem4
         // 
         this.emptySpaceItem4.AllowHotTrack = false;
         this.emptySpaceItem4.Location = new System.Drawing.Point(0, 63);
         this.emptySpaceItem4.Name = "emptySpaceItem4";
         this.emptySpaceItem4.Size = new System.Drawing.Size(542, 34);
         this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
         // 
         // decimalSeparatorLayoutControlItem
         // 
         this.decimalSeparatorLayoutControlItem.Control = this.comboBoxEdit1;
         this.decimalSeparatorLayoutControlItem.Location = new System.Drawing.Point(0, 121);
         this.decimalSeparatorLayoutControlItem.Name = "decimalSeparatorLayoutControlItem";
         this.decimalSeparatorLayoutControlItem.Size = new System.Drawing.Size(542, 24);
         this.decimalSeparatorLayoutControlItem.TextSize = new System.Drawing.Size(173, 13);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 145);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(542, 44);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.previewMemoEdit;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 17);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(542, 46);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
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
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.fileNameLabel;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(542, 17);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // CSVSeparatorSelectorView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "Separator Selection";
         this.ClientSize = new System.Drawing.Size(562, 252);
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
         ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnSeparatorLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.decimalSeparatorLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl mainLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.ComboBoxEdit separatorComboBoxEdit;
      private DevExpress.XtraLayout.LayoutControlItem columnSeparatorLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private DevExpress.XtraLayout.LayoutControlItem decimalSeparatorLayoutControlItem;
        private DevExpress.XtraEditors.MemoEdit previewMemoEdit;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.LabelControl fileNameLabel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
    }
}
