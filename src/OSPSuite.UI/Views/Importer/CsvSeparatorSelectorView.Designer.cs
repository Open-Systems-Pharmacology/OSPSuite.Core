using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using OSPSuite.Assets;

namespace OSPSuite.UI.Views.Importer
{
   partial class CsvSeparatorSelectorView
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
         this.separatorComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.separatorDescriptionLabelControl = new DevExpress.XtraEditors.LabelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.separatorDescriptionLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.separatorLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).BeginInit();
         this.layoutControlBase.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.mainLayoutControl)).BeginInit();
         this.mainLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.separatorComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorDescriptionLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(491, 7);
         this.btnCancel.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
         this.btnCancel.Size = new System.Drawing.Size(158, 27);
         // 
         // btnOk
         // 
         this.btnOk.Location = new System.Drawing.Point(381, 7);
         this.btnOk.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
         this.btnOk.Size = new System.Drawing.Size(108, 27);
         // 
         // layoutControlBase
         // 
         this.layoutControlBase.Location = new System.Drawing.Point(0, 192);
         this.layoutControlBase.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
         this.layoutControlBase.Size = new System.Drawing.Size(656, 57);
         this.layoutControlBase.Controls.SetChildIndex(this.btnCancel, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnOk, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnExtra, 0);
         // 
         // btnExtra
         // 
         this.btnExtra.Location = new System.Drawing.Point(7, 7);
         this.btnExtra.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
         this.btnExtra.Size = new System.Drawing.Size(194, 27);
         // 
         // layoutControlGroupBase
         // 
         this.layoutControlGroupBase.Size = new System.Drawing.Size(656, 57);
         // 
         // layoutItemOK
         // 
         this.layoutItemOK.Location = new System.Drawing.Point(374, 0);
         this.layoutItemOK.Size = new System.Drawing.Size(110, 45);
         // 
         // layoutItemCancel
         // 
         this.layoutItemCancel.Location = new System.Drawing.Point(484, 0);
         this.layoutItemCancel.Size = new System.Drawing.Size(160, 45);
         // 
         // emptySpaceItemBase
         // 
         this.emptySpaceItemBase.Location = new System.Drawing.Point(196, 0);
         this.emptySpaceItemBase.Size = new System.Drawing.Size(178, 45);
         // 
         // layoutItemExtra
         // 
         this.layoutItemExtra.Size = new System.Drawing.Size(196, 45);
         // 
         // mainLayoutControl
         // 
         this.mainLayoutControl.Controls.Add(this.separatorComboBoxEdit);
         this.mainLayoutControl.Controls.Add(this.separatorDescriptionLabelControl);
         this.mainLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.mainLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.mainLayoutControl.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.mainLayoutControl.Name = "mainLayoutControl";
         this.mainLayoutControl.Root = this.Root;
         this.mainLayoutControl.Size = new System.Drawing.Size(656, 192);
         this.mainLayoutControl.TabIndex = 38;
         this.mainLayoutControl.Text = "mainLayoutControl";
         // 
         // separatorComboBoxEdit
         // 
         this.separatorComboBoxEdit.Location = new System.Drawing.Point(249, 56);
         this.separatorComboBoxEdit.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.separatorComboBoxEdit.Name = "separatorComboBoxEdit";
         this.separatorComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.separatorComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
         this.separatorComboBoxEdit.Size = new System.Drawing.Size(138, 22);
         this.separatorComboBoxEdit.StyleController = this.mainLayoutControl;
         this.separatorComboBoxEdit.TabIndex = 5;
         // 
         // separatorDescriptionLabelControl
         // 
         this.separatorDescriptionLabelControl.Location = new System.Drawing.Point(6, 6);
         this.separatorDescriptionLabelControl.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.separatorDescriptionLabelControl.Name = "separatorDescriptionLabelControl";
         this.separatorDescriptionLabelControl.Size = new System.Drawing.Size(644, 16);
         this.separatorDescriptionLabelControl.StyleController = this.mainLayoutControl;
         this.separatorDescriptionLabelControl.TabIndex = 4;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.separatorDescriptionLayoutControlItem,
            this.emptySpaceItem1,
            this.separatorLayoutControlItem,
            this.emptySpaceItem2,
            this.emptySpaceItem3,
            this.emptySpaceItem4});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(656, 192);
         this.Root.TextVisible = false;
         // 
         // separatorDescriptionLayoutControlItem
         // 
         this.separatorDescriptionLayoutControlItem.Control = this.separatorDescriptionLabelControl;
         this.separatorDescriptionLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.separatorDescriptionLayoutControlItem.Name = "separatorDescriptionLayoutControlItem";
         this.separatorDescriptionLayoutControlItem.Size = new System.Drawing.Size(646, 18);
         this.separatorDescriptionLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.separatorDescriptionLayoutControlItem.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 74);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(646, 108);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // separatorLayoutControlItem
         // 
         this.separatorLayoutControlItem.Control = this.separatorComboBoxEdit;
         this.separatorLayoutControlItem.Location = new System.Drawing.Point(243, 50);
         this.separatorLayoutControlItem.Name = "separatorLayoutControlItem";
         this.separatorLayoutControlItem.Size = new System.Drawing.Size(140, 24);
         this.separatorLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.separatorLayoutControlItem.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 50);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(243, 24);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem3
         // 
         this.emptySpaceItem3.AllowHotTrack = false;
         this.emptySpaceItem3.Location = new System.Drawing.Point(383, 50);
         this.emptySpaceItem3.Name = "emptySpaceItem3";
         this.emptySpaceItem3.Size = new System.Drawing.Size(263, 24);
         this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem4
         // 
         this.emptySpaceItem4.AllowHotTrack = false;
         this.emptySpaceItem4.Location = new System.Drawing.Point(0, 18);
         this.emptySpaceItem4.Name = "emptySpaceItem4";
         this.emptySpaceItem4.Size = new System.Drawing.Size(646, 32);
         this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
         // 
         // CsvSeparatorSelectorView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "Separator Selection";
         this.ClientSize = new System.Drawing.Size(656, 249);
         this.Controls.Add(this.mainLayoutControl);
         this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
         this.Name = "CsvSeparatorSelectorView";
         this.Text = "Separator Selection";
         this.Controls.SetChildIndex(this.layoutControlBase, 0);
         this.Controls.SetChildIndex(this.mainLayoutControl, 0);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).EndInit();
         this.layoutControlBase.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.mainLayoutControl)).EndInit();
         this.mainLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.separatorComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorDescriptionLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl mainLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.LabelControl separatorDescriptionLabelControl;
      private DevExpress.XtraLayout.LayoutControlItem separatorDescriptionLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.ComboBoxEdit separatorComboBoxEdit;
      private DevExpress.XtraLayout.LayoutControlItem separatorLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
   }
}
