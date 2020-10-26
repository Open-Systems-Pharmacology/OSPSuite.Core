using System.Windows.Forms;
using OSPSuite.Assets;

namespace OSPSuite.UI.Views.Importer
{
   partial class NanView
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
         this.rootLayoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.actionImageComboBoxEdit = new DevExpress.XtraEditors.ImageComboBoxEdit();
         this.indicatorTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.indicatorLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.actionLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.actionImageComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.indicatorTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.indicatorLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.actionLayoutControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
         this.rootLayoutControl.Controls.Add(this.actionImageComboBoxEdit);
         this.rootLayoutControl.Controls.Add(this.indicatorTextEdit);
         this.rootLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.rootLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.rootLayoutControl.Name = "rootLayoutControl";
         this.rootLayoutControl.Root = this.Root;
         this.rootLayoutControl.Size = new System.Drawing.Size(642, 86);
         this.rootLayoutControl.TabIndex = 0;
         this.rootLayoutControl.Text = "rootLayoutControl";
         // 
         // actionImageComboBoxEdit
         // 
         this.actionImageComboBoxEdit.Location = new System.Drawing.Point(123, 52);
         this.actionImageComboBoxEdit.Name = "actionImageComboBoxEdit";
         this.indicatorTextEdit.ToolTip = Captions.Importer.NanIndicatorHint;
         this.actionImageComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.actionImageComboBoxEdit.Size = new System.Drawing.Size(507, 22);
         this.actionImageComboBoxEdit.StyleController = this.rootLayoutControl;
         this.actionImageComboBoxEdit.TabIndex = 5;
         // 
         // indicatorTextEdit
         // 
         this.indicatorTextEdit.Location = new System.Drawing.Point(123, 12);
         this.indicatorTextEdit.Name = "indicatorTextEdit";
         this.indicatorTextEdit.Size = new System.Drawing.Size(507, 22);
         this.indicatorTextEdit.StyleController = this.rootLayoutControl;
         this.indicatorTextEdit.TabIndex = 4;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.indicatorLayoutControlItem,
            this.emptySpaceItem1,
            this.actionLayoutControlItem});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(642, 86);
         this.Root.TextVisible = false;
         // 
         // indicatorLayoutControlItem
         // 
         this.indicatorLayoutControlItem.Control = this.indicatorTextEdit;
         this.indicatorLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.indicatorLayoutControlItem.Name = Captions.Importer.NanIndicator;
         this.indicatorLayoutControlItem.Size = new System.Drawing.Size(622, 26);
         this.indicatorLayoutControlItem.TextSize = new System.Drawing.Size(108, 16);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 26);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(622, 14);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // actionLayoutControlItem
         // 
         this.actionLayoutControlItem.Control = this.actionImageComboBoxEdit;
         this.actionLayoutControlItem.Location = new System.Drawing.Point(0, 40);
         this.actionLayoutControlItem.Name = Captions.Importer.NanAction;
         this.actionLayoutControlItem.Size = new System.Drawing.Size(622, 26);
         this.actionLayoutControlItem.TextSize = new System.Drawing.Size(108, 16);
         // 
         // NanView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.rootLayoutControl);
         this.Name = "NanView";
         this.Size = new System.Drawing.Size(642, 86);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).EndInit();
         this.rootLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.actionImageComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.indicatorTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.indicatorLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.actionLayoutControlItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl rootLayoutControl;
      private DevExpress.XtraEditors.ImageComboBoxEdit actionImageComboBoxEdit;
      private DevExpress.XtraEditors.TextEdit indicatorTextEdit;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem indicatorLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.LayoutControlItem actionLayoutControlItem;
   }
}
