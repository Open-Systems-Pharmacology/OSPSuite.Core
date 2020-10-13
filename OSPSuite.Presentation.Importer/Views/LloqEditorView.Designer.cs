using DevExpress.XtraEditors.Controls;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.UI.Views;
using OSPSuite.Assets;

namespace OSPSuite.Presentation.Importer.Views
{
   partial class LloqEditorView : BaseModalView
   {
      private DevExpress.XtraLayout.LayoutControl rootLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.ComboBoxEdit ColumnsComboBox;
      private DevExpress.XtraLayout.LayoutControlItem rootLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private ILloqEditorPresenter _presenter;
      public void AttachPresenter(ILloqEditorPresenter presenter)
      {
         _presenter = presenter;
      }

      private void InitializeComponent()
      {
         this.rootLayoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.ColumnsComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
         this.rootLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).BeginInit();
         this.layoutControlBase.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ColumnsComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(428, 14);
         this.btnCancel.Size = new System.Drawing.Size(87, 27);
         // 
         // btnOk
         // 
         this.btnOk.Location = new System.Drawing.Point(321, 14);
         this.btnOk.Size = new System.Drawing.Size(103, 27);
         // 
         // layoutControlBase
         // 
         this.layoutControlBase.Location = new System.Drawing.Point(0, 59);
         this.layoutControlBase.Size = new System.Drawing.Size(528, 57);
         this.layoutControlBase.Controls.SetChildIndex(this.btnCancel, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnOk, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnExtra, 0);
         // 
         // btnExtra
         // 
         this.btnExtra.Size = new System.Drawing.Size(149, 27);
         // 
         // layoutControlGroupBase
         // 
         this.layoutControlGroupBase.Size = new System.Drawing.Size(528, 57);
         // 
         // layoutItemOK
         // 
         this.layoutItemOK.Location = new System.Drawing.Point(308, 0);
         this.layoutItemOK.Size = new System.Drawing.Size(107, 33);
         // 
         // layoutItemCancel
         // 
         this.layoutItemCancel.Location = new System.Drawing.Point(415, 0);
         this.layoutItemCancel.Size = new System.Drawing.Size(91, 33);
         // 
         // emptySpaceItemBase
         // 
         this.emptySpaceItemBase.Location = new System.Drawing.Point(153, 0);
         this.emptySpaceItemBase.Size = new System.Drawing.Size(155, 33);
         // 
         // layoutItemExtra
         // 
         this.layoutItemExtra.Size = new System.Drawing.Size(153, 33);
         // 
         // rootLayoutControl
         // 
         this.rootLayoutControl.Controls.Add(this.ColumnsComboBox);
         this.rootLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.rootLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.rootLayoutControl.Name = "rootLayoutControl";
         this.rootLayoutControl.Root = this.Root;
         this.rootLayoutControl.Size = new System.Drawing.Size(528, 59);
         this.rootLayoutControl.TabIndex = 38;
         this.rootLayoutControl.Text = "rootLayoutControl";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.rootLayoutControlItem,
            this.emptySpaceItem1});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(528, 59);
         this.Root.TextVisible = false;
         // 
         // ColumnsComboBox
         // 
         this.ColumnsComboBox.Location = new System.Drawing.Point(123, 12);
         this.ColumnsComboBox.Name = "ColumnsComboBox";
         this.ColumnsComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ColumnsComboBox.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
         this.ColumnsComboBox.Size = new System.Drawing.Size(393, 22);
         this.ColumnsComboBox.StyleController = this.rootLayoutControl;
         this.ColumnsComboBox.TabIndex = 4;
         // 
         // rootLayoutControlItem
         // 
         this.rootLayoutControlItem.Control = this.ColumnsComboBox;
         this.rootLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.rootLayoutControlItem.Name = Captions.Importer.Columns;
         this.rootLayoutControlItem.Size = new System.Drawing.Size(508, 26);
         this.rootLayoutControlItem.TextSize = new System.Drawing.Size(108, 16);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 26);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(508, 13);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // LloqEditorView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.ClientSize = new System.Drawing.Size(528, 116);
         this.Controls.Add(this.rootLayoutControl);
         this.Name = "LloqEditorView";
         this.Controls.SetChildIndex(this.layoutControlBase, 0);
         this.Controls.SetChildIndex(this.rootLayoutControl, 0);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).EndInit();
         this.layoutControlBase.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).EndInit();
         this.rootLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ColumnsComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }
   }
}
