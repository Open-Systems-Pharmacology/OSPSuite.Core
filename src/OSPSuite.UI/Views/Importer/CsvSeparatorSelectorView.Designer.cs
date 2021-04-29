using System.Windows.Forms;

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
         this.separatorDescriptionLabelControl = new DevExpress.XtraEditors.LabelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.separatorDescriptionLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.separatorComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.separatorLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
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
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorDescriptionLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorLayoutControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(817, 30);
         this.btnCancel.Size = new System.Drawing.Size(163, 54);
         // 
         // btnOk
         // 
         this.btnOk.Location = new System.Drawing.Point(615, 30);
         this.btnOk.Size = new System.Drawing.Size(192, 54);
         // 
         // layoutControlBase
         // 
         this.layoutControlBase.Location = new System.Drawing.Point(0, 506);
         this.layoutControlBase.Size = new System.Drawing.Size(1009, 117);
         this.layoutControlBase.Controls.SetChildIndex(this.btnCancel, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnOk, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnExtra, 0);
         // 
         // btnExtra
         // 
         this.btnExtra.Size = new System.Drawing.Size(283, 54);
         // 
         // layoutControlGroupBase
         // 
         this.layoutControlGroupBase.Size = new System.Drawing.Size(1009, 117);
         // 
         // layoutItemOK
         // 
         this.layoutItemOK.Location = new System.Drawing.Point(586, 0);
         this.layoutItemOK.Size = new System.Drawing.Size(202, 67);
         // 
         // layoutItemCancel
         // 
         this.layoutItemCancel.Location = new System.Drawing.Point(788, 0);
         this.layoutItemCancel.Size = new System.Drawing.Size(173, 67);
         // 
         // emptySpaceItemBase
         // 
         this.emptySpaceItemBase.Location = new System.Drawing.Point(293, 0);
         this.emptySpaceItemBase.Size = new System.Drawing.Size(293, 67);
         // 
         // layoutItemExtra
         // 
         this.layoutItemExtra.Size = new System.Drawing.Size(293, 67);
         // 
         // mainLayoutControl
         // 
         this.mainLayoutControl.Controls.Add(this.separatorComboBoxEdit);
         this.mainLayoutControl.Controls.Add(this.separatorDescriptionLabelControl);
         this.mainLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.mainLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.mainLayoutControl.Name = "mainLayoutControl";
         this.mainLayoutControl.Root = this.Root;
         this.mainLayoutControl.Size = new System.Drawing.Size(1009, 506);
         this.mainLayoutControl.TabIndex = 38;
         this.mainLayoutControl.Text = "mainLayoutControl";
         // 
         // separatorDescriptionLabelControl
         // 
         this.separatorDescriptionLabelControl.Location = new System.Drawing.Point(12, 12);
         this.separatorDescriptionLabelControl.Name = "separatorDescriptionLabelControl";
         this.separatorDescriptionLabelControl.Size = new System.Drawing.Size(393, 33);
         this.separatorDescriptionLabelControl.StyleController = this.mainLayoutControl;
         this.separatorDescriptionLabelControl.TabIndex = 4;
         this.separatorDescriptionLabelControl.Text = "separatorDescriptionLabelControl";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.separatorDescriptionLayoutControlItem,
            this.emptySpaceItem1,
            this.separatorLayoutControlItem});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1009, 506);
         this.Root.TextVisible = false;
         // 
         // separatorDescriptionLayoutControlItem
         // 
         this.separatorDescriptionLayoutControlItem.Control = this.separatorDescriptionLabelControl;
         this.separatorDescriptionLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.separatorDescriptionLayoutControlItem.Name = "separatorDescriptionLayoutControlItem";
         this.separatorDescriptionLayoutControlItem.Size = new System.Drawing.Size(989, 37);
         this.separatorDescriptionLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.separatorDescriptionLayoutControlItem.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 89);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(989, 397);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // separatorComboBoxEdit
         // 
         this.separatorComboBoxEdit.Location = new System.Drawing.Point(12, 49);
         this.separatorComboBoxEdit.Name = "separatorComboBoxEdit";
         this.separatorComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.separatorComboBoxEdit.Size = new System.Drawing.Size(985, 48);
         this.separatorComboBoxEdit.StyleController = this.mainLayoutControl;
         this.separatorComboBoxEdit.TabIndex = 5;
         // 
         // separatorLayoutControlItem
         // 
         this.separatorLayoutControlItem.Control = this.separatorComboBoxEdit;
         this.separatorLayoutControlItem.Location = new System.Drawing.Point(0, 37);
         this.separatorLayoutControlItem.Name = "separatorLayoutControlItem";
         this.separatorLayoutControlItem.Size = new System.Drawing.Size(989, 52);
         this.separatorLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.separatorLayoutControlItem.TextVisible = false;
         // 
         // CsvSeparatorSelectorView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1009, 623);
         this.Controls.Add(this.mainLayoutControl);
         this.Name = "CsvSeparatorSelectorView";
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
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorDescriptionLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.separatorLayoutControlItem)).EndInit();
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
   }
}
