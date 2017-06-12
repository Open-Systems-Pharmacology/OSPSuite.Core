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
         _screenBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.uxLayoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.nameTextBox = new DevExpress.XtraEditors.TextEdit();
         this.nameControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.yAxisTypeComboBox = new OSPSuite.UI.Controls.UxYAxisTypeEdit();
         this.yAxisTypeControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.lineStyleComboBox = new OSPSuite.UI.Controls.UxLineTypeEdit();
         this.lineStyleControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.symbolComboBox = new OSPSuite.UI.Controls.UxSymbolEdit();
         this.symbolControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.lineThicknessComboBox = new OSPSuite.UI.Controls.UxLineThicknessEdit();
         this.lineThicknessControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.visibleCheckEdit = new OSPSuite.UI.Controls.UxCheckEdit();
         this.visibleControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.visibleInLegendCheckEdit = new OSPSuite.UI.Controls.UxCheckEdit();
         this.visibleInLegendControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.colorEdit = new OSPSuite.UI.Controls.UxColorPickEditWithHistory();
         this.colorControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).BeginInit();
         this.layoutControlBase.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).BeginInit();
         this.uxLayoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nameTextBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nameControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.yAxisTypeComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.yAxisTypeControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.lineStyleComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.lineStyleControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.symbolComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.symbolControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.lineThicknessComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.lineThicknessControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleCheckEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleInLegendCheckEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleInLegendControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.colorEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.colorControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(293, 12);
         this.btnCancel.Size = new System.Drawing.Size(57, 22);
         // 
         // btnOk
         // 
         this.btnOk.Location = new System.Drawing.Point(221, 12);
         this.btnOk.Size = new System.Drawing.Size(68, 22);
         // 
         // layoutControlBase
         // 
         this.layoutControlBase.Location = new System.Drawing.Point(0, 257);
         this.layoutControlBase.Size = new System.Drawing.Size(362, 46);
         this.layoutControlBase.Controls.SetChildIndex(this.btnCancel, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnOk, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnExtra, 0);
         // 
         // btnExtra
         // 
         this.btnExtra.Size = new System.Drawing.Size(100, 22);
         // 
         // layoutControlGroupBase
         // 
         this.layoutControlGroupBase.Size = new System.Drawing.Size(362, 46);
         // 
         // layoutItemOK
         // 
         this.layoutItemOK.Location = new System.Drawing.Point(209, 0);
         this.layoutItemOK.Size = new System.Drawing.Size(72, 26);
         // 
         // layoutItemCancel
         // 
         this.layoutItemCancel.Location = new System.Drawing.Point(281, 0);
         this.layoutItemCancel.Size = new System.Drawing.Size(61, 26);
         // 
         // emptySpaceItemBase
         // 
         this.emptySpaceItemBase.Location = new System.Drawing.Point(104, 0);
         this.emptySpaceItemBase.Size = new System.Drawing.Size(105, 26);
         // 
         // layoutItemExtra
         // 
         this.layoutItemExtra.Size = new System.Drawing.Size(104, 26);
         // 
         // uxLayoutControl1
         // 
         this.uxLayoutControl1.AllowCustomization = false;
         this.uxLayoutControl1.Controls.Add(this.colorEdit);
         this.uxLayoutControl1.Controls.Add(this.nameTextBox);
         this.uxLayoutControl1.Controls.Add(this.yAxisTypeComboBox);
         this.uxLayoutControl1.Controls.Add(this.lineStyleComboBox);
         this.uxLayoutControl1.Controls.Add(this.symbolComboBox);
         this.uxLayoutControl1.Controls.Add(this.lineThicknessComboBox);
         this.uxLayoutControl1.Controls.Add(this.visibleCheckEdit);
         this.uxLayoutControl1.Controls.Add(this.visibleInLegendCheckEdit);
         this.uxLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl1.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl1.Name = "uxLayoutControl1";
         this.uxLayoutControl1.Root = this.layoutControlGroup2;
         this.uxLayoutControl1.Size = new System.Drawing.Size(362, 257);
         this.uxLayoutControl1.TabIndex = 38;
         this.uxLayoutControl1.Text = "uxLayoutControl1";
         // 
         // layoutControlGroup2
         // 
         this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup2.GroupBordersVisible = false;
         this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.nameControlItem,
            this.yAxisTypeControlItem,
            this.lineStyleControlItem,
            this.symbolControlItem,
            this.lineThicknessControlItem,
            this.visibleControlItem,
            this.visibleInLegendControlItem,
            this.colorControlItem});
         this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup2.Name = "layoutControlGroup2";
         this.layoutControlGroup2.Size = new System.Drawing.Size(362, 257);
         this.layoutControlGroup2.TextVisible = false;
         // 
         // nameTextBox
         // 
         this.nameTextBox.Location = new System.Drawing.Point(147, 12);
         this.nameTextBox.Name = "nameTextBox";
         this.nameTextBox.Size = new System.Drawing.Size(203, 20);
         this.nameTextBox.StyleController = this.uxLayoutControl1;
         this.nameTextBox.TabIndex = 5;
         // 
         // nameControlItem
         // 
         this.nameControlItem.Control = this.nameTextBox;
         this.nameControlItem.CustomizationFormText = "nameControlItem";
         this.nameControlItem.Location = new System.Drawing.Point(0, 0);
         this.nameControlItem.Name = "nameControlItem";
         this.nameControlItem.Size = new System.Drawing.Size(342, 24);
         this.nameControlItem.Text = "nameControlItem";
         this.nameControlItem.TextSize = new System.Drawing.Size(131, 13);
         // 
         // yAxisTypeComboBox
         // 
         this.yAxisTypeComboBox.Location = new System.Drawing.Point(147, 36);
         this.yAxisTypeComboBox.Name = "yAxisTypeComboBox";
         this.yAxisTypeComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.yAxisTypeComboBox.Size = new System.Drawing.Size(203, 20);
         this.yAxisTypeComboBox.StyleController = this.uxLayoutControl1;
         this.yAxisTypeComboBox.TabIndex = 7;
         // 
         // yAxisTypeControlItem
         // 
         this.yAxisTypeControlItem.Control = this.yAxisTypeComboBox;
         this.yAxisTypeControlItem.CustomizationFormText = "yAxisTypeControlItem";
         this.yAxisTypeControlItem.Location = new System.Drawing.Point(0, 24);
         this.yAxisTypeControlItem.Name = "yAxisTypeControlItem";
         this.yAxisTypeControlItem.Size = new System.Drawing.Size(342, 24);
         this.yAxisTypeControlItem.Text = "yAxisTypeControlItem";
         this.yAxisTypeControlItem.TextSize = new System.Drawing.Size(131, 13);
         // 
         // lineStyleComboBox
         // 
         this.lineStyleComboBox.Location = new System.Drawing.Point(147, 84);
         this.lineStyleComboBox.Name = "lineStyleComboBox";
         this.lineStyleComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.lineStyleComboBox.Size = new System.Drawing.Size(203, 20);
         this.lineStyleComboBox.StyleController = this.uxLayoutControl1;
         this.lineStyleComboBox.TabIndex = 9;
         // 
         // lineStyleControlItem
         // 
         this.lineStyleControlItem.Control = this.lineStyleComboBox;
         this.lineStyleControlItem.CustomizationFormText = "lineStyleControlItem";
         this.lineStyleControlItem.Location = new System.Drawing.Point(0, 72);
         this.lineStyleControlItem.Name = "lineStyleControlItem";
         this.lineStyleControlItem.Size = new System.Drawing.Size(342, 24);
         this.lineStyleControlItem.Text = "lineStyleControlItem";
         this.lineStyleControlItem.TextSize = new System.Drawing.Size(131, 13);
         // 
         // symbolComboBox
         // 
         this.symbolComboBox.Location = new System.Drawing.Point(147, 108);
         this.symbolComboBox.Name = "symbolComboBox";
         this.symbolComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.symbolComboBox.Size = new System.Drawing.Size(203, 20);
         this.symbolComboBox.StyleController = this.uxLayoutControl1;
         this.symbolComboBox.TabIndex = 10;
         // 
         // symbolControlItem
         // 
         this.symbolControlItem.Control = this.symbolComboBox;
         this.symbolControlItem.CustomizationFormText = "symbolControlItem";
         this.symbolControlItem.Location = new System.Drawing.Point(0, 96);
         this.symbolControlItem.Name = "symbolControlItem";
         this.symbolControlItem.Size = new System.Drawing.Size(342, 24);
         this.symbolControlItem.Text = "symbolControlItem";
         this.symbolControlItem.TextSize = new System.Drawing.Size(131, 13);
         // 
         // lineThicknessComboBox
         // 
         this.lineThicknessComboBox.Location = new System.Drawing.Point(147, 132);
         this.lineThicknessComboBox.Name = "lineThicknessComboBox";
         this.lineThicknessComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.lineThicknessComboBox.Size = new System.Drawing.Size(203, 20);
         this.lineThicknessComboBox.StyleController = this.uxLayoutControl1;
         this.lineThicknessComboBox.TabIndex = 11;
         // 
         // lineThicknessControlItem
         // 
         this.lineThicknessControlItem.Control = this.lineThicknessComboBox;
         this.lineThicknessControlItem.CustomizationFormText = "lineThicknessControlItem";
         this.lineThicknessControlItem.Location = new System.Drawing.Point(0, 120);
         this.lineThicknessControlItem.Name = "lineThicknessControlItem";
         this.lineThicknessControlItem.Size = new System.Drawing.Size(342, 24);
         this.lineThicknessControlItem.Text = "lineThicknessControlItem";
         this.lineThicknessControlItem.TextSize = new System.Drawing.Size(131, 13);
         // 
         // visibleCheckEdit
         // 
         this.visibleCheckEdit.AllowClicksOutsideControlArea = false;
         this.visibleCheckEdit.Location = new System.Drawing.Point(147, 156);
         this.visibleCheckEdit.Name = "visibleCheckEdit";
         this.visibleCheckEdit.Properties.AllowFocused = false;
         this.visibleCheckEdit.Properties.Caption = "";
         this.visibleCheckEdit.Size = new System.Drawing.Size(203, 19);
         this.visibleCheckEdit.StyleController = this.uxLayoutControl1;
         this.visibleCheckEdit.TabIndex = 12;
         // 
         // visibleControlItem
         // 
         this.visibleControlItem.Control = this.visibleCheckEdit;
         this.visibleControlItem.CustomizationFormText = "visibleControlItem";
         this.visibleControlItem.Location = new System.Drawing.Point(0, 144);
         this.visibleControlItem.Name = "visibleControlItem";
         this.visibleControlItem.Size = new System.Drawing.Size(342, 23);
         this.visibleControlItem.Text = "visibleControlItem";
         this.visibleControlItem.TextSize = new System.Drawing.Size(131, 13);
         // 
         // visibleInLegendCheckEdit
         // 
         this.visibleInLegendCheckEdit.AllowClicksOutsideControlArea = false;
         this.visibleInLegendCheckEdit.Location = new System.Drawing.Point(147, 179);
         this.visibleInLegendCheckEdit.Name = "visibleInLegendCheckEdit";
         this.visibleInLegendCheckEdit.Properties.AllowFocused = false;
         this.visibleInLegendCheckEdit.Properties.Caption = "";
         this.visibleInLegendCheckEdit.Size = new System.Drawing.Size(203, 19);
         this.visibleInLegendCheckEdit.StyleController = this.uxLayoutControl1;
         this.visibleInLegendCheckEdit.TabIndex = 14;
         // 
         // visibleInLegendControlItem
         // 
         this.visibleInLegendControlItem.Control = this.visibleInLegendCheckEdit;
         this.visibleInLegendControlItem.CustomizationFormText = "visibleInLegendControlItem";
         this.visibleInLegendControlItem.Location = new System.Drawing.Point(0, 167);
         this.visibleInLegendControlItem.Name = "visibleInLegendControlItem";
         this.visibleInLegendControlItem.Size = new System.Drawing.Size(342, 70);
         this.visibleInLegendControlItem.Text = "visibleInLegendControlItem";
         this.visibleInLegendControlItem.TextSize = new System.Drawing.Size(131, 13);
         // 
         // colorEdit
         // 
         this.colorEdit.EditValue = System.Drawing.Color.Empty;
         this.colorEdit.Location = new System.Drawing.Point(147, 60);
         this.colorEdit.Name = "colorEdit";
         this.colorEdit.Properties.AutomaticColor = System.Drawing.Color.Black;
         this.colorEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.colorEdit.Properties.ShowSystemColors = false;
         this.colorEdit.Properties.ShowWebColors = false;
         this.colorEdit.Size = new System.Drawing.Size(203, 20);
         this.colorEdit.StyleController = this.uxLayoutControl1;
         this.colorEdit.TabIndex = 15;
         // 
         // colorControlItem
         // 
         this.colorControlItem.Control = this.colorEdit;
         this.colorControlItem.CustomizationFormText = "colorControlItem";
         this.colorControlItem.Location = new System.Drawing.Point(0, 48);
         this.colorControlItem.Name = "colorControlItem";
         this.colorControlItem.Size = new System.Drawing.Size(342, 24);
         this.colorControlItem.TextSize = new System.Drawing.Size(131, 13);
         // 
         // SingleCurveSettingsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(362, 303);
         this.Controls.Add(this.uxLayoutControl1);
         this.Margin = new System.Windows.Forms.Padding(0);
         this.Name = "SingleCurveSettingsView";
         this.Controls.SetChildIndex(this.layoutControlBase, 0);
         this.Controls.SetChildIndex(this.uxLayoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).EndInit();
         this.layoutControlBase.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).EndInit();
         this.uxLayoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nameTextBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nameControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.yAxisTypeComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.yAxisTypeControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.lineStyleComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.lineStyleControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.symbolComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.symbolControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.lineThicknessComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.lineThicknessControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleCheckEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleInLegendCheckEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.visibleInLegendControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.colorEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.colorControlItem)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private UxLayoutControl uxLayoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
      private DevExpress.XtraEditors.TextEdit nameTextBox;
      private DevExpress.XtraLayout.LayoutControlItem nameControlItem;
      private UxYAxisTypeEdit yAxisTypeComboBox;
      private DevExpress.XtraLayout.LayoutControlItem yAxisTypeControlItem;
      private UxLineTypeEdit lineStyleComboBox;
      private DevExpress.XtraLayout.LayoutControlItem lineStyleControlItem;
      private UxSymbolEdit symbolComboBox;
      private DevExpress.XtraLayout.LayoutControlItem symbolControlItem;
      private UxLineThicknessEdit lineThicknessComboBox;
      private DevExpress.XtraLayout.LayoutControlItem lineThicknessControlItem;
      private UxCheckEdit visibleCheckEdit;
      private DevExpress.XtraLayout.LayoutControlItem visibleControlItem;
      private UxCheckEdit visibleInLegendCheckEdit;
      private DevExpress.XtraLayout.LayoutControlItem visibleInLegendControlItem;
      private UxColorPickEditWithHistory colorEdit;
      private DevExpress.XtraLayout.LayoutControlItem colorControlItem;
   }
}
