using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Charts
{
   partial class SingleCurveSettingsView
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
         _curveScreenBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.visibleInLegendCheckEdit = new OSPSuite.UI.Controls.UxCheckEdit();
         this.colorEdit = new UxColorPickEditWithHistory();
         this.visibleCheckEdit = new OSPSuite.UI.Controls.UxCheckEdit();
         this.lineThicknessComboBox = new UxLineThicknessEdit();
         this.symbolComboBox = new UxSymbolEdit();
         this.lineStyleComboBox = new UxLineTypeEdit();
         this.yAxisTypeComboBox = new UxYAxisTypeEdit();
         this.nameTextBox = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.nameControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.yAxisTypeControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.lineStyleControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.symbolControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.lineThicknessControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.visibleControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.colorControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.visibleInLegendControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.visibleInLegendCheckEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.colorEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleCheckEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.lineThicknessComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.symbolComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.lineStyleComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.yAxisTypeComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nameTextBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nameControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.yAxisTypeControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.lineStyleControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.symbolControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.lineThicknessControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.colorControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleInLegendControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.visibleInLegendCheckEdit);
         this.layoutControl.Controls.Add(this.colorEdit);
         this.layoutControl.Controls.Add(this.visibleCheckEdit);
         this.layoutControl.Controls.Add(this.lineThicknessComboBox);
         this.layoutControl.Controls.Add(this.symbolComboBox);
         this.layoutControl.Controls.Add(this.lineStyleComboBox);
         this.layoutControl.Controls.Add(this.yAxisTypeComboBox);
         this.layoutControl.Controls.Add(this.nameTextBox);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(392, 191);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // visibleInLegendCheckEdit
         // 
         this.visibleInLegendCheckEdit.Location = new System.Drawing.Point(136, 169);
         this.visibleInLegendCheckEdit.Name = "visibleInLegendCheckEdit";
         this.visibleInLegendCheckEdit.Properties.Caption = "";
         this.visibleInLegendCheckEdit.Size = new System.Drawing.Size(254, 19);
         this.visibleInLegendCheckEdit.StyleController = this.layoutControl;
         this.visibleInLegendCheckEdit.TabIndex = 14;
         // 
         // colorEdit
         // 
         this.colorEdit.EditValue = System.Drawing.Color.Empty;
         this.colorEdit.Location = new System.Drawing.Point(136, 50);
         this.colorEdit.Name = "colorEdit";
         this.colorEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.colorEdit.Size = new System.Drawing.Size(254, 20);
         this.colorEdit.StyleController = this.layoutControl;
         this.colorEdit.TabIndex = 13;
         // 
         // visibleCheckEdit
         // 
         this.visibleCheckEdit.Location = new System.Drawing.Point(136, 146);
         this.visibleCheckEdit.Name = "visibleCheckEdit";
         this.visibleCheckEdit.Properties.Caption = "";
         this.visibleCheckEdit.Size = new System.Drawing.Size(254, 19);
         this.visibleCheckEdit.StyleController = this.layoutControl;
         this.visibleCheckEdit.TabIndex = 12;
         // 
         // lineThicknessComboBox
         // 
         this.lineThicknessComboBox.Location = new System.Drawing.Point(136, 122);
         this.lineThicknessComboBox.Name = "lineThicknessComboBox";
         this.lineThicknessComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.lineThicknessComboBox.Size = new System.Drawing.Size(254, 20);
         this.lineThicknessComboBox.StyleController = this.layoutControl;
         this.lineThicknessComboBox.TabIndex = 11;
         // 
         // symbolComboBox
         // 
         this.symbolComboBox.Location = new System.Drawing.Point(136, 98);
         this.symbolComboBox.Name = "symbolComboBox";
         this.symbolComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.symbolComboBox.Size = new System.Drawing.Size(254, 20);
         this.symbolComboBox.StyleController = this.layoutControl;
         this.symbolComboBox.TabIndex = 10;
         // 
         // lineStyleComboBox
         // 
         this.lineStyleComboBox.Location = new System.Drawing.Point(136, 74);
         this.lineStyleComboBox.Name = "lineStyleComboBox";
         this.lineStyleComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.lineStyleComboBox.Size = new System.Drawing.Size(254, 20);
         this.lineStyleComboBox.StyleController = this.layoutControl;
         this.lineStyleComboBox.TabIndex = 9;
         // 
         // yAxisTypeComboBox
         // 
         this.yAxisTypeComboBox.Location = new System.Drawing.Point(136, 26);
         this.yAxisTypeComboBox.Name = "yAxisTypeComboBox";
         this.yAxisTypeComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.yAxisTypeComboBox.Size = new System.Drawing.Size(254, 20);
         this.yAxisTypeComboBox.StyleController = this.layoutControl;
         this.yAxisTypeComboBox.TabIndex = 7;
         // 
         // nameTextBox
         // 
         this.nameTextBox.Location = new System.Drawing.Point(136, 2);
         this.nameTextBox.Name = "nameTextBox";
         this.nameTextBox.Size = new System.Drawing.Size(254, 20);
         this.nameTextBox.StyleController = this.layoutControl;
         this.nameTextBox.TabIndex = 5;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.nameControlItem,
            this.yAxisTypeControlItem,
            this.lineStyleControlItem,
            this.symbolControlItem,
            this.lineThicknessControlItem,
            this.visibleControlItem,
            this.colorControlItem,
            this.visibleInLegendControlItem});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(392, 191);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // nameControlItem
         // 
         this.nameControlItem.Control = this.nameTextBox;
         this.nameControlItem.CustomizationFormText = "nameControlItem";
         this.nameControlItem.Location = new System.Drawing.Point(0, 0);
         this.nameControlItem.Name = "nameControlItem";
         this.nameControlItem.Size = new System.Drawing.Size(392, 24);
         this.nameControlItem.Text = "nameControlItem";
         this.nameControlItem.TextSize = new System.Drawing.Size(131, 13);
         // 
         // yAxisTypeControlItem
         // 
         this.yAxisTypeControlItem.Control = this.yAxisTypeComboBox;
         this.yAxisTypeControlItem.CustomizationFormText = "yAxisTypeControlItem";
         this.yAxisTypeControlItem.Location = new System.Drawing.Point(0, 24);
         this.yAxisTypeControlItem.Name = "yAxisTypeControlItem";
         this.yAxisTypeControlItem.Size = new System.Drawing.Size(392, 24);
         this.yAxisTypeControlItem.Text = "yAxisTypeControlItem";
         this.yAxisTypeControlItem.TextSize = new System.Drawing.Size(131, 13);
         // 
         // lineStyleControlItem
         // 
         this.lineStyleControlItem.Control = this.lineStyleComboBox;
         this.lineStyleControlItem.CustomizationFormText = "lineStyleControlItem";
         this.lineStyleControlItem.Location = new System.Drawing.Point(0, 72);
         this.lineStyleControlItem.Name = "lineStyleControlItem";
         this.lineStyleControlItem.Size = new System.Drawing.Size(392, 24);
         this.lineStyleControlItem.Text = "lineStyleControlItem";
         this.lineStyleControlItem.TextSize = new System.Drawing.Size(131, 13);
         // 
         // symbolControlItem
         // 
         this.symbolControlItem.Control = this.symbolComboBox;
         this.symbolControlItem.CustomizationFormText = "symbolControlItem";
         this.symbolControlItem.Location = new System.Drawing.Point(0, 96);
         this.symbolControlItem.Name = "symbolControlItem";
         this.symbolControlItem.Size = new System.Drawing.Size(392, 24);
         this.symbolControlItem.Text = "symbolControlItem";
         this.symbolControlItem.TextSize = new System.Drawing.Size(131, 13);
         // 
         // lineThicknessControlItem
         // 
         this.lineThicknessControlItem.Control = this.lineThicknessComboBox;
         this.lineThicknessControlItem.CustomizationFormText = "lineThicknessControlItem";
         this.lineThicknessControlItem.Location = new System.Drawing.Point(0, 120);
         this.lineThicknessControlItem.Name = "lineThicknessControlItem";
         this.lineThicknessControlItem.Size = new System.Drawing.Size(392, 24);
         this.lineThicknessControlItem.Text = "lineThicknessControlItem";
         this.lineThicknessControlItem.TextSize = new System.Drawing.Size(131, 13);
         // 
         // visibleControlItem
         // 
         this.visibleControlItem.Control = this.visibleCheckEdit;
         this.visibleControlItem.CustomizationFormText = "visibleControlItem";
         this.visibleControlItem.Location = new System.Drawing.Point(0, 144);
         this.visibleControlItem.Name = "visibleControlItem";
         this.visibleControlItem.Size = new System.Drawing.Size(392, 23);
         this.visibleControlItem.Text = "visibleControlItem";
         this.visibleControlItem.TextSize = new System.Drawing.Size(131, 13);
         // 
         // colorControlItem
         // 
         this.colorControlItem.Control = this.colorEdit;
         this.colorControlItem.CustomizationFormText = "colorControlItem";
         this.colorControlItem.Location = new System.Drawing.Point(0, 48);
         this.colorControlItem.Name = "colorControlItem";
         this.colorControlItem.Size = new System.Drawing.Size(392, 24);
         this.colorControlItem.Text = "colorControlItem";
         this.colorControlItem.TextSize = new System.Drawing.Size(131, 13);
         // 
         // visibleInLegendControlItem
         // 
         this.visibleInLegendControlItem.Control = this.visibleInLegendCheckEdit;
         this.visibleInLegendControlItem.CustomizationFormText = "visibleInLegendControlItem";
         this.visibleInLegendControlItem.Location = new System.Drawing.Point(0, 167);
         this.visibleInLegendControlItem.Name = "visibleInLegendControlItem";
         this.visibleInLegendControlItem.Size = new System.Drawing.Size(392, 24);
         this.visibleInLegendControlItem.Text = "visibleInLegendControlItem";
         this.visibleInLegendControlItem.TextSize = new System.Drawing.Size(131, 13);
         // 
         // layoutControlItem5
         // 
         this.layoutControlItem5.Control = this.yAxisTypeComboBox;
         this.layoutControlItem5.CustomizationFormText = "layoutControlItem4";
         this.layoutControlItem5.Location = new System.Drawing.Point(0, 72);
         this.layoutControlItem5.Name = "layoutControlItem4";
         this.layoutControlItem5.Size = new System.Drawing.Size(546, 254);
         this.layoutControlItem5.Text = "layoutControlItem4";
         this.layoutControlItem5.TextSize = new System.Drawing.Size(93, 13);
         this.layoutControlItem5.TextToControlDistance = 5;
         // 
         // SingleCurveSettingsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Margin = new System.Windows.Forms.Padding(0);
         this.Name = "SingleCurveSettingsView";
         this.Size = new System.Drawing.Size(392, 191);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.visibleInLegendCheckEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.colorEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleCheckEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.lineThicknessComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.symbolComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.lineStyleComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.yAxisTypeComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nameTextBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nameControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.yAxisTypeControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.lineStyleControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.symbolControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.lineThicknessControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.colorControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleInLegendControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.CheckEdit visibleCheckEdit;
      private UxLineThicknessEdit lineThicknessComboBox;
      private UxSymbolEdit symbolComboBox;
      private UxLineTypeEdit lineStyleComboBox;
      private UxYAxisTypeEdit yAxisTypeComboBox;
      private DevExpress.XtraEditors.TextEdit nameTextBox;
      private DevExpress.XtraLayout.LayoutControlItem nameControlItem;
      private DevExpress.XtraLayout.LayoutControlItem yAxisTypeControlItem;
      private DevExpress.XtraLayout.LayoutControlItem lineStyleControlItem;
      private DevExpress.XtraLayout.LayoutControlItem symbolControlItem;
      private DevExpress.XtraLayout.LayoutControlItem lineThicknessControlItem;
      private DevExpress.XtraLayout.LayoutControlItem visibleControlItem;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
      private DevExpress.XtraLayout.LayoutControlItem colorControlItem;
      private UxColorPickEditWithHistory colorEdit;
      private DevExpress.XtraEditors.CheckEdit visibleInLegendCheckEdit;
      private DevExpress.XtraLayout.LayoutControlItem visibleInLegendControlItem;



   }
}
