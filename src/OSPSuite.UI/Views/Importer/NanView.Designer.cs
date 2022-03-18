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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NanView));
         this.rootLayoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.actionImageComboBoxEdit = new DevExpress.XtraEditors.ImageComboBoxEdit();
         this.indicatorTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.indicatorLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.actionLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.actionImageComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.indicatorTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.indicatorLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.actionLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
         this.rootLayoutControl.Controls.Add(this.actionImageComboBoxEdit);
         this.rootLayoutControl.Controls.Add(this.indicatorTextEdit);
         this.rootLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.rootLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.rootLayoutControl.Margin = new System.Windows.Forms.Padding(8, 5, 8, 5);
         this.rootLayoutControl.Name = "rootLayoutControl";
         this.rootLayoutControl.Root = this.Root;
         this.rootLayoutControl.Size = new System.Drawing.Size(1390, 201);
         this.rootLayoutControl.TabIndex = 0;
         this.rootLayoutControl.Text = "rootLayoutControl";
         // 
         // actionImageComboBoxEdit
         // 
         this.actionImageComboBoxEdit.Location = new System.Drawing.Point(337, 54);
         this.actionImageComboBoxEdit.Margin = new System.Windows.Forms.Padding(8, 5, 8, 5);
         this.actionImageComboBoxEdit.Name = "actionImageComboBoxEdit";
         this.actionImageComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.actionImageComboBoxEdit.Size = new System.Drawing.Size(1051, 48);
         this.actionImageComboBoxEdit.StyleController = this.rootLayoutControl;
         this.actionImageComboBoxEdit.TabIndex = 5;
         this.actionImageComboBoxEdit.ToolTip = resources.GetString("actionImageComboBoxEdit.ToolTip");
         // 
         // indicatorTextEdit
         // 
         this.indicatorTextEdit.Location = new System.Drawing.Point(337, 2);
         this.indicatorTextEdit.Margin = new System.Windows.Forms.Padding(8, 5, 8, 5);
         this.indicatorTextEdit.Name = "indicatorTextEdit";
         this.indicatorTextEdit.Size = new System.Drawing.Size(1051, 48);
         this.indicatorTextEdit.StyleController = this.rootLayoutControl;
         this.indicatorTextEdit.TabIndex = 4;
         this.indicatorTextEdit.ToolTip = "Type a number that will be interpreted as NaN(Not a Number). Text in numerical co" +
    "lumns is interpreted as NaN anyway.";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.indicatorLayoutControlItem,
            this.actionLayoutControlItem,
            this.emptySpaceItem1});
         this.Root.Name = "Root";
         this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.Root.Size = new System.Drawing.Size(1390, 201);
         this.Root.TextVisible = false;
         // 
         // indicatorLayoutControlItem
         // 
         this.indicatorLayoutControlItem.Control = this.indicatorTextEdit;
         this.indicatorLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.indicatorLayoutControlItem.Name = "indicatorLayoutControlItem";
         this.indicatorLayoutControlItem.Size = new System.Drawing.Size(1390, 52);
         this.indicatorLayoutControlItem.TextSize = new System.Drawing.Size(323, 33);
         // 
         // actionLayoutControlItem
         // 
         this.actionLayoutControlItem.Control = this.actionImageComboBoxEdit;
         this.actionLayoutControlItem.Location = new System.Drawing.Point(0, 52);
         this.actionLayoutControlItem.Name = "actionLayoutControlItem";
         this.actionLayoutControlItem.Size = new System.Drawing.Size(1390, 52);
         this.actionLayoutControlItem.TextSize = new System.Drawing.Size(323, 33);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 104);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(1390, 97);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // NanView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.rootLayoutControl);
         this.Margin = new System.Windows.Forms.Padding(8, 5, 8, 5);
         this.Name = "NanView";
         this.Size = new System.Drawing.Size(1390, 201);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).EndInit();
         this.rootLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.actionImageComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.indicatorTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.indicatorLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.actionLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl rootLayoutControl;
      private DevExpress.XtraEditors.ImageComboBoxEdit actionImageComboBoxEdit;
      private DevExpress.XtraEditors.TextEdit indicatorTextEdit;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem indicatorLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem actionLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
