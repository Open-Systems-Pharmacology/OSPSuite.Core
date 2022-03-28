using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Charts
{
   partial class CurveMultiItemEditorView
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
         _screenBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.colorEditLayoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.inLegendComboBoxEdit = new UxComboBoxEdit();
         this.visibleComboBoxEdit = new UxComboBoxEdit();
         this.symbolComboBoxEdit = new UxComboBoxEdit();
         this.styleComboBoxEdit = new UxComboBoxEdit();
         this.colorPickEdit = new UxColorPickEditWithHistory();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.colorLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.styleLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.symbolLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.visibleLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.inLegendLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.colorEditLayoutControl)).BeginInit();
         this.colorEditLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.inLegendComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.symbolComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.styleComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.colorPickEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.colorLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.styleLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.symbolLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.inLegendLayoutControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // colorEditLayoutControl
         // 
         this.colorEditLayoutControl.Controls.Add(this.inLegendComboBoxEdit);
         this.colorEditLayoutControl.Controls.Add(this.visibleComboBoxEdit);
         this.colorEditLayoutControl.Controls.Add(this.symbolComboBoxEdit);
         this.colorEditLayoutControl.Controls.Add(this.styleComboBoxEdit);
         this.colorEditLayoutControl.Controls.Add(this.colorPickEdit);
         this.colorEditLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.colorEditLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.colorEditLayoutControl.Name = "colorEditLayoutControl";
         this.colorEditLayoutControl.Root = this.Root;
         this.colorEditLayoutControl.Size = new System.Drawing.Size(1035, 431);
         this.colorEditLayoutControl.TabIndex = 38;
         // 
         // inLegendComboBoxEdit
         // 
         this.inLegendComboBoxEdit.Location = new System.Drawing.Point(134, 220);
         this.inLegendComboBoxEdit.Name = "inLegendComboBoxEdit";
         this.inLegendComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.inLegendComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
         this.inLegendComboBoxEdit.Size = new System.Drawing.Size(889, 48);
         this.inLegendComboBoxEdit.StyleController = this.colorEditLayoutControl;
         this.inLegendComboBoxEdit.TabIndex = 9;
         // 
         // visibleComboBoxEdit
         // 
         this.visibleComboBoxEdit.Location = new System.Drawing.Point(134, 168);
         this.visibleComboBoxEdit.Name = "visibleComboBoxEdit";
         this.visibleComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.visibleComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
         this.visibleComboBoxEdit.Size = new System.Drawing.Size(889, 48);
         this.visibleComboBoxEdit.StyleController = this.colorEditLayoutControl;
         this.visibleComboBoxEdit.TabIndex = 8;
         // 
         // symbolComboBoxEdit
         // 
         this.symbolComboBoxEdit.Location = new System.Drawing.Point(134, 116);
         this.symbolComboBoxEdit.Name = "symbolComboBoxEdit";
         this.symbolComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.symbolComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
         this.symbolComboBoxEdit.Size = new System.Drawing.Size(889, 48);
         this.symbolComboBoxEdit.StyleController = this.colorEditLayoutControl;
         this.symbolComboBoxEdit.TabIndex = 7;
         // 
         // styleComboBoxEdit
         // 
         this.styleComboBoxEdit.Location = new System.Drawing.Point(134, 64);
         this.styleComboBoxEdit.Name = "styleComboBoxEdit";
         this.styleComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.styleComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
         this.styleComboBoxEdit.Size = new System.Drawing.Size(889, 48);
         this.styleComboBoxEdit.StyleController = this.colorEditLayoutControl;
         this.styleComboBoxEdit.TabIndex = 6;
         // 
         // colorPickEdit1
         // 
         this.colorPickEdit.EditValue = System.Drawing.Color.Empty;
         this.colorPickEdit.Location = new System.Drawing.Point(134, 12);
         this.colorPickEdit.Name = "colorPickEdit";
         this.colorPickEdit.Properties.AutomaticColor = System.Drawing.Color.Black;
         this.colorPickEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.colorPickEdit.Size = new System.Drawing.Size(889, 48);
         this.colorPickEdit.StyleController = this.colorEditLayoutControl;
         this.colorPickEdit.TabIndex = 5;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.colorLayoutControlItem,
            this.styleLayoutControlItem,
            this.symbolLayoutControlItem,
            this.visibleLayoutControlItem,
            this.inLegendLayoutControlItem});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1035, 431);
         this.Root.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 260);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(1015, 151);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // colorLayoutControlItem
         // 
         this.colorLayoutControlItem.Control = this.colorPickEdit;
         this.colorLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.colorLayoutControlItem.Name = "colorLayoutControlItem";
         this.colorLayoutControlItem.Size = new System.Drawing.Size(1015, 52);
         this.colorLayoutControlItem.Text = "Color";
         this.colorLayoutControlItem.TextSize = new System.Drawing.Size(119, 33);
         // 
         // styleLayoutControlItem
         // 
         this.styleLayoutControlItem.Control = this.styleComboBoxEdit;
         this.styleLayoutControlItem.Location = new System.Drawing.Point(0, 52);
         this.styleLayoutControlItem.Name = "styleLayoutControlItem";
         this.styleLayoutControlItem.Size = new System.Drawing.Size(1015, 52);
         this.styleLayoutControlItem.Text = "Style";
         this.styleLayoutControlItem.TextSize = new System.Drawing.Size(119, 33);
         // 
         // symbolLayoutControlItem
         // 
         this.symbolLayoutControlItem.Control = this.symbolComboBoxEdit;
         this.symbolLayoutControlItem.Location = new System.Drawing.Point(0, 104);
         this.symbolLayoutControlItem.Name = "symbolLayoutControlItem";
         this.symbolLayoutControlItem.Size = new System.Drawing.Size(1015, 52);
         this.symbolLayoutControlItem.Text = "Symbol";
         this.symbolLayoutControlItem.TextSize = new System.Drawing.Size(119, 33);
         // 
         // visibleLayoutControlItem
         // 
         this.visibleLayoutControlItem.Control = this.visibleComboBoxEdit;
         this.visibleLayoutControlItem.Location = new System.Drawing.Point(0, 156);
         this.visibleLayoutControlItem.Name = "visibleLayoutControlItem";
         this.visibleLayoutControlItem.Size = new System.Drawing.Size(1015, 52);
         this.visibleLayoutControlItem.Text = "Visible";
         this.visibleLayoutControlItem.TextSize = new System.Drawing.Size(119, 33);
         // 
         // inLegendLayoutControlItem
         // 
         this.inLegendLayoutControlItem.Control = this.inLegendComboBoxEdit;
         this.inLegendLayoutControlItem.Location = new System.Drawing.Point(0, 208);
         this.inLegendLayoutControlItem.Name = "inLegendLayoutControlItem";
         this.inLegendLayoutControlItem.Size = new System.Drawing.Size(1015, 52);
         this.inLegendLayoutControlItem.Text = "In Legend";
         this.inLegendLayoutControlItem.TextSize = new System.Drawing.Size(119, 33);
         // 
         // CurveMultiItemEditorView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "Edit Options";
         this.ClientSize = new System.Drawing.Size(1035, 548);
         this.Controls.Add(this.colorEditLayoutControl);
         this.Name = "CurveMultiItemEditorView";
         this.Text = "Edit Options";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.colorEditLayoutControl)).EndInit();
         this.colorEditLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.inLegendComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.symbolComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.styleComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.colorPickEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.colorLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.styleLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.symbolLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.inLegendLayoutControlItem)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl colorEditLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private OSPSuite.UI.Controls.UxColorPickEditWithHistory colorPickEdit;
      private DevExpress.XtraLayout.LayoutControlItem colorLayoutControlItem;
      private OSPSuite.UI.Controls.UxComboBoxEdit symbolComboBoxEdit;
      private OSPSuite.UI.Controls.UxComboBoxEdit styleComboBoxEdit;
      private DevExpress.XtraLayout.LayoutControlItem styleLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem symbolLayoutControlItem;
      private OSPSuite.UI.Controls.UxComboBoxEdit inLegendComboBoxEdit;
      private OSPSuite.UI.Controls.UxComboBoxEdit visibleComboBoxEdit;
      private DevExpress.XtraLayout.LayoutControlItem visibleLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem inLegendLayoutControlItem;
   }
}