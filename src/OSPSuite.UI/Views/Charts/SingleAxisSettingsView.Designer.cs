using DevExpress.XtraEditors;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Charts
{
   partial class SingleAxisSettingsView
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
         this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.maxTextBox = new DevExpress.XtraEditors.TextEdit();
         this.minTextBox = new DevExpress.XtraEditors.TextEdit();
         this.captionTextBox = new DevExpress.XtraEditors.TextEdit();
         this.gridLinesCheckEdit = new OSPSuite.UI.Controls.UxCheckEdit();
         this.defaultColorColorEdit = new UxColorPickEditWithHistory();
         this.scalingComboBox = new UxScalingsEdit();
         this.unitComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
         this.dimensionComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
         this.numberRepresentationComboBox = new UxNumberModeEdit();
         this.defaultLineSytleComboBox = new UxLineTypeEdit();
         this.axisTypeTextBox = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.axisTypeLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.defaultLineStyleLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.numberRepresentationLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.dimensionLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.unitLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.scalingLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.captionLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.minLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.maxLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.gridLinesLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.defaultColorLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.maxTextBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.minTextBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.captionTextBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridLinesCheckEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.defaultColorColorEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.scalingComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.unitComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dimensionComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberRepresentationComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.defaultLineSytleComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.axisTypeTextBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.axisTypeLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.defaultLineStyleLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberRepresentationLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dimensionLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.unitLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.scalingLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.captionLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.minLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.maxLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridLinesLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.defaultColorLayoutControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.maxTextBox);
         this.layoutControl.Controls.Add(this.minTextBox);
         this.layoutControl.Controls.Add(this.captionTextBox);
         this.layoutControl.Controls.Add(this.gridLinesCheckEdit);
         this.layoutControl.Controls.Add(this.defaultColorColorEdit);
         this.layoutControl.Controls.Add(this.scalingComboBox);
         this.layoutControl.Controls.Add(this.unitComboBox);
         this.layoutControl.Controls.Add(this.dimensionComboBox);
         this.layoutControl.Controls.Add(this.numberRepresentationComboBox);
         this.layoutControl.Controls.Add(this.defaultLineSytleComboBox);
         this.layoutControl.Controls.Add(this.axisTypeTextBox);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(463, 265);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // maxTextBox
         // 
         this.maxTextBox.Location = new System.Drawing.Point(205, 170);
         this.maxTextBox.Name = "maxTextBox";
         this.maxTextBox.Size = new System.Drawing.Size(256, 20);
         this.maxTextBox.StyleController = this.layoutControl;
         this.maxTextBox.TabIndex = 14;
         // 
         // minTextBox
         // 
         this.minTextBox.Location = new System.Drawing.Point(205, 146);
         this.minTextBox.Name = "minTextBox";
         this.minTextBox.Size = new System.Drawing.Size(256, 20);
         this.minTextBox.StyleController = this.layoutControl;
         this.minTextBox.TabIndex = 13;
         // 
         // captionTextBox
         // 
         this.captionTextBox.Location = new System.Drawing.Point(205, 50);
         this.captionTextBox.Name = "captionTextBox";
         this.captionTextBox.Size = new System.Drawing.Size(256, 20);
         this.captionTextBox.StyleController = this.layoutControl;
         this.captionTextBox.TabIndex = 12;
         // 
         // gridLinesCheckEdit
         // 
         this.gridLinesCheckEdit.Location = new System.Drawing.Point(205, 194);
         this.gridLinesCheckEdit.Name = "gridLinesCheckEdit";
         this.gridLinesCheckEdit.Properties.Caption = "";
         this.gridLinesCheckEdit.Size = new System.Drawing.Size(256, 19);
         this.gridLinesCheckEdit.StyleController = this.layoutControl;
         this.gridLinesCheckEdit.TabIndex = 11;
         // 
         // defaultColorColorEdit
         // 
         this.defaultColorColorEdit.EditValue = System.Drawing.Color.Empty;
         this.defaultColorColorEdit.Location = new System.Drawing.Point(205, 217);
         this.defaultColorColorEdit.Name = "defaultColorColorEdit";
         this.defaultColorColorEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.defaultColorColorEdit.Size = new System.Drawing.Size(256, 20);
         this.defaultColorColorEdit.StyleController = this.layoutControl;
         this.defaultColorColorEdit.TabIndex = 10;
         // 
         // scalingComboBox
         // 
         this.scalingComboBox.Location = new System.Drawing.Point(205, 122);
         this.scalingComboBox.Name = "scalingComboBox";
         this.scalingComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.scalingComboBox.Size = new System.Drawing.Size(256, 20);
         this.scalingComboBox.StyleController = this.layoutControl;
         this.scalingComboBox.TabIndex = 9;
         // 
         // unitComboBox
         // 
         this.unitComboBox.Location = new System.Drawing.Point(205, 98);
         this.unitComboBox.Name = "unitComboBox";
         this.unitComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.unitComboBox.Size = new System.Drawing.Size(256, 20);
         this.unitComboBox.StyleController = this.layoutControl;
         this.unitComboBox.TabIndex = 8;
         // 
         // dimensionComboBox
         // 
         this.dimensionComboBox.Location = new System.Drawing.Point(205, 74);
         this.dimensionComboBox.Name = "dimensionComboBox";
         this.dimensionComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.dimensionComboBox.Size = new System.Drawing.Size(256, 20);
         this.dimensionComboBox.StyleController = this.layoutControl;
         this.dimensionComboBox.TabIndex = 7;
         // 
         // numberRepresentationComboBox
         // 
         this.numberRepresentationComboBox.Location = new System.Drawing.Point(205, 26);
         this.numberRepresentationComboBox.Name = "numberRepresentationComboBox";
         this.numberRepresentationComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.numberRepresentationComboBox.Size = new System.Drawing.Size(256, 20);
         this.numberRepresentationComboBox.StyleController = this.layoutControl;
         this.numberRepresentationComboBox.TabIndex = 6;
         // 
         // defaultLineSytleComboBox
         // 
         this.defaultLineSytleComboBox.Location = new System.Drawing.Point(205, 241);
         this.defaultLineSytleComboBox.Name = "defaultLineSytleComboBox";
         this.defaultLineSytleComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.defaultLineSytleComboBox.Size = new System.Drawing.Size(256, 20);
         this.defaultLineSytleComboBox.StyleController = this.layoutControl;
         this.defaultLineSytleComboBox.TabIndex = 5;
         // 
         // axisTypeTextBox
         // 
         this.axisTypeTextBox.Location = new System.Drawing.Point(205, 2);
         this.axisTypeTextBox.Name = "axisTypeTextBox";
         this.axisTypeTextBox.Size = new System.Drawing.Size(256, 20);
         this.axisTypeTextBox.StyleController = this.layoutControl;
         this.axisTypeTextBox.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.axisTypeLayoutControlItem,
            this.defaultLineStyleLayoutControlItem,
            this.numberRepresentationLayoutControlItem,
            this.dimensionLayoutControlItem,
            this.unitLayoutControlItem,
            this.scalingLayoutControlItem,
            this.captionLayoutControlItem,
            this.minLayoutControlItem,
            this.maxLayoutControlItem,
            this.gridLinesLayoutControlItem,
            this.defaultColorLayoutControlItem});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(463, 265);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // axisTypeLayoutControlItem
         // 
         this.axisTypeLayoutControlItem.Control = this.axisTypeTextBox;
         this.axisTypeLayoutControlItem.CustomizationFormText = "layoutControlItem1";
         this.axisTypeLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.axisTypeLayoutControlItem.Name = "axisTypeLayoutControlItem";
         this.axisTypeLayoutControlItem.Size = new System.Drawing.Size(463, 24);
         this.axisTypeLayoutControlItem.Text = "axisTypeLayoutControlItem";
         this.axisTypeLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // defaultLineStyleLayoutControlItem
         // 
         this.defaultLineStyleLayoutControlItem.Control = this.defaultLineSytleComboBox;
         this.defaultLineStyleLayoutControlItem.CustomizationFormText = "layoutControlItem2";
         this.defaultLineStyleLayoutControlItem.Location = new System.Drawing.Point(0, 239);
         this.defaultLineStyleLayoutControlItem.Name = "defaultLineStyleLayoutControlItem";
         this.defaultLineStyleLayoutControlItem.Size = new System.Drawing.Size(463, 26);
         this.defaultLineStyleLayoutControlItem.Text = "defaultLineStyleLayoutControlItem";
         this.defaultLineStyleLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // numberRepresentationLayoutControlItem
         // 
         this.numberRepresentationLayoutControlItem.Control = this.numberRepresentationComboBox;
         this.numberRepresentationLayoutControlItem.CustomizationFormText = "layoutControlItem3";
         this.numberRepresentationLayoutControlItem.Location = new System.Drawing.Point(0, 24);
         this.numberRepresentationLayoutControlItem.Name = "numberRepresentationLayoutControlItem";
         this.numberRepresentationLayoutControlItem.Size = new System.Drawing.Size(463, 24);
         this.numberRepresentationLayoutControlItem.Text = "numberRepresentationLayoutControlItem";
         this.numberRepresentationLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // dimensionLayoutControlItem
         // 
         this.dimensionLayoutControlItem.Control = this.dimensionComboBox;
         this.dimensionLayoutControlItem.CustomizationFormText = "layoutControlItem4";
         this.dimensionLayoutControlItem.Location = new System.Drawing.Point(0, 72);
         this.dimensionLayoutControlItem.Name = "dimensionLayoutControlItem";
         this.dimensionLayoutControlItem.Size = new System.Drawing.Size(463, 24);
         this.dimensionLayoutControlItem.Text = "dimensionLayoutControlItem";
         this.dimensionLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // unitLayoutControlItem
         // 
         this.unitLayoutControlItem.Control = this.unitComboBox;
         this.unitLayoutControlItem.CustomizationFormText = "layoutControlItem5";
         this.unitLayoutControlItem.Location = new System.Drawing.Point(0, 96);
         this.unitLayoutControlItem.Name = "unitLayoutControlItem";
         this.unitLayoutControlItem.Size = new System.Drawing.Size(463, 24);
         this.unitLayoutControlItem.Text = "unitLayoutControlItem";
         this.unitLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // scalingLayoutControlItem
         // 
         this.scalingLayoutControlItem.Control = this.scalingComboBox;
         this.scalingLayoutControlItem.CustomizationFormText = "layoutControlItem6";
         this.scalingLayoutControlItem.Location = new System.Drawing.Point(0, 120);
         this.scalingLayoutControlItem.Name = "scalingLayoutControlItem";
         this.scalingLayoutControlItem.Size = new System.Drawing.Size(463, 24);
         this.scalingLayoutControlItem.Text = "scalingLayoutControlItem";
         this.scalingLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // captionLayoutControlItem
         // 
         this.captionLayoutControlItem.Control = this.captionTextBox;
         this.captionLayoutControlItem.CustomizationFormText = "layoutControlItem9";
         this.captionLayoutControlItem.Location = new System.Drawing.Point(0, 48);
         this.captionLayoutControlItem.Name = "captionLayoutControlItem";
         this.captionLayoutControlItem.Size = new System.Drawing.Size(463, 24);
         this.captionLayoutControlItem.Text = "captionLayoutControlItem";
         this.captionLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // minLayoutControlItem
         // 
         this.minLayoutControlItem.Control = this.minTextBox;
         this.minLayoutControlItem.CustomizationFormText = "layoutControlItem10";
         this.minLayoutControlItem.Location = new System.Drawing.Point(0, 144);
         this.minLayoutControlItem.Name = "minLayoutControlItem";
         this.minLayoutControlItem.Size = new System.Drawing.Size(463, 24);
         this.minLayoutControlItem.Text = "minLayoutControlItem";
         this.minLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // maxLayoutControlItem
         // 
         this.maxLayoutControlItem.Control = this.maxTextBox;
         this.maxLayoutControlItem.CustomizationFormText = "layoutControlItem11";
         this.maxLayoutControlItem.Location = new System.Drawing.Point(0, 168);
         this.maxLayoutControlItem.Name = "maxLayoutControlItem";
         this.maxLayoutControlItem.Size = new System.Drawing.Size(463, 24);
         this.maxLayoutControlItem.Text = "maxLayoutControlItem";
         this.maxLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // gridLinesLayoutControlItem
         // 
         this.gridLinesLayoutControlItem.Control = this.gridLinesCheckEdit;
         this.gridLinesLayoutControlItem.CustomizationFormText = "layoutControlItem8";
         this.gridLinesLayoutControlItem.Location = new System.Drawing.Point(0, 192);
         this.gridLinesLayoutControlItem.Name = "gridLinesLayoutControlItem";
         this.gridLinesLayoutControlItem.Size = new System.Drawing.Size(463, 23);
         this.gridLinesLayoutControlItem.Text = "gridLinesLayoutControlItem";
         this.gridLinesLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // defaultColorLayoutControlItem
         // 
         this.defaultColorLayoutControlItem.Control = this.defaultColorColorEdit;
         this.defaultColorLayoutControlItem.CustomizationFormText = "layoutControlItem7";
         this.defaultColorLayoutControlItem.Location = new System.Drawing.Point(0, 215);
         this.defaultColorLayoutControlItem.Name = "defaultColorLayoutControlItem";
         this.defaultColorLayoutControlItem.Size = new System.Drawing.Size(463, 24);
         this.defaultColorLayoutControlItem.Text = "defaultColorLayoutControlItem";
         this.defaultColorLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // SingleAxisSettingsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Margin = new System.Windows.Forms.Padding(0);
         this.Name = "SingleAxisSettingsView";
         this.Size = new System.Drawing.Size(463, 265);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.maxTextBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.minTextBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.captionTextBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridLinesCheckEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.defaultColorColorEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.scalingComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.unitComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dimensionComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberRepresentationComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.defaultLineSytleComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.axisTypeTextBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.axisTypeLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.defaultLineStyleLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberRepresentationLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dimensionLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.unitLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.scalingLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.captionLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.minLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.maxLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridLinesLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.defaultColorLayoutControlItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.TextEdit maxTextBox;
      private DevExpress.XtraEditors.TextEdit minTextBox;
      private DevExpress.XtraEditors.TextEdit captionTextBox;
      private DevExpress.XtraEditors.CheckEdit gridLinesCheckEdit;
      private ColorEdit defaultColorColorEdit;
      private UxScalingsEdit scalingComboBox;
      private DevExpress.XtraEditors.ComboBoxEdit unitComboBox;
      private DevExpress.XtraEditors.ComboBoxEdit dimensionComboBox;
      private UxNumberModeEdit numberRepresentationComboBox;
      private UxLineTypeEdit defaultLineSytleComboBox;
      private DevExpress.XtraEditors.TextEdit axisTypeTextBox;
      private DevExpress.XtraLayout.LayoutControlItem axisTypeLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem defaultLineStyleLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem numberRepresentationLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem dimensionLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem unitLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem scalingLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem captionLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem minLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem maxLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem gridLinesLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem defaultColorLayoutControlItem;
   }
}
