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
         this.uxLayoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.scalingComboBox = new OSPSuite.UI.Controls.UxScalingsEdit();
         this.minTextBox = new DevExpress.XtraEditors.TextEdit();
         this.numberRepresentationComboBox = new OSPSuite.UI.Controls.UxNumberModeEdit();
         this.gridLinesCheckEdit = new OSPSuite.UI.Controls.UxCheckEdit();
         this.defaultLineSytleComboBox = new OSPSuite.UI.Controls.UxLineTypeEdit();
         this.captionTextBox = new DevExpress.XtraEditors.TextEdit();
         this.unitComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
         this.defaultColorColorEdit = new OSPSuite.UI.Controls.UxColorPickEditWithHistory();
         this.axisTypeTextBox = new DevExpress.XtraEditors.TextEdit();
         this.maxTextBox = new DevExpress.XtraEditors.TextEdit();
         this.dimensionComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
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
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
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
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.scalingComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.minTextBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberRepresentationComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridLinesCheckEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.defaultLineSytleComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.captionTextBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.unitComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.defaultColorColorEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.axisTypeTextBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.maxTextBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dimensionComboBox.Properties)).BeginInit();
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
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(268, 12);
         this.btnCancel.Size = new System.Drawing.Size(57, 22);
         // 
         // btnOk
         // 
         this.btnOk.Location = new System.Drawing.Point(205, 12);
         this.btnOk.Size = new System.Drawing.Size(59, 22);
         // 
         // layoutControlBase
         // 
         this.layoutControlBase.Location = new System.Drawing.Point(0, 321);
         this.layoutControlBase.Size = new System.Drawing.Size(337, 46);
         this.layoutControlBase.Controls.SetChildIndex(this.btnCancel, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnOk, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnExtra, 0);
         // 
         // btnExtra
         // 
         this.btnExtra.Size = new System.Drawing.Size(93, 22);
         // 
         // layoutControlGroupBase
         // 
         this.layoutControlGroupBase.Size = new System.Drawing.Size(337, 46);
         // 
         // layoutItemOK
         // 
         this.layoutItemOK.Location = new System.Drawing.Point(193, 0);
         this.layoutItemOK.Size = new System.Drawing.Size(63, 26);
         // 
         // layoutItemCancel
         // 
         this.layoutItemCancel.Location = new System.Drawing.Point(256, 0);
         this.layoutItemCancel.Size = new System.Drawing.Size(61, 26);
         // 
         // emptySpaceItemBase
         // 
         this.emptySpaceItemBase.Location = new System.Drawing.Point(97, 0);
         this.emptySpaceItemBase.Size = new System.Drawing.Size(96, 26);
         // 
         // layoutItemExtra
         // 
         this.layoutItemExtra.Size = new System.Drawing.Size(97, 26);
         // 
         // uxLayoutControl1
         // 
         this.uxLayoutControl1.AllowCustomization = false;
         this.uxLayoutControl1.Controls.Add(this.axisTypeTextBox);
         this.uxLayoutControl1.Controls.Add(this.defaultLineSytleComboBox);
         this.uxLayoutControl1.Controls.Add(this.numberRepresentationComboBox);
         this.uxLayoutControl1.Controls.Add(this.dimensionComboBox);
         this.uxLayoutControl1.Controls.Add(this.unitComboBox);
         this.uxLayoutControl1.Controls.Add(this.scalingComboBox);
         this.uxLayoutControl1.Controls.Add(this.captionTextBox);
         this.uxLayoutControl1.Controls.Add(this.minTextBox);
         this.uxLayoutControl1.Controls.Add(this.maxTextBox);
         this.uxLayoutControl1.Controls.Add(this.gridLinesCheckEdit);
         this.uxLayoutControl1.Controls.Add(this.defaultColorColorEdit);
         this.uxLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl1.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl1.Name = "uxLayoutControl1";
         this.uxLayoutControl1.Root = this.layoutControlGroup1;
         this.uxLayoutControl1.Size = new System.Drawing.Size(337, 321);
         this.uxLayoutControl1.TabIndex = 38;
         this.uxLayoutControl1.Text = "uxLayoutControl1";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.defaultLineStyleLayoutControlItem,
            this.defaultColorLayoutControlItem,
            this.gridLinesLayoutControlItem,
            this.maxLayoutControlItem,
            this.minLayoutControlItem,
            this.scalingLayoutControlItem,
            this.unitLayoutControlItem,
            this.dimensionLayoutControlItem,
            this.captionLayoutControlItem,
            this.numberRepresentationLayoutControlItem,
            this.axisTypeLayoutControlItem,
            this.emptySpaceItem1});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(337, 321);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // scalingComboBox
         // 
         this.scalingComboBox.Location = new System.Drawing.Point(216, 132);
         this.scalingComboBox.Name = "scalingComboBox";
         this.scalingComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.scalingComboBox.Size = new System.Drawing.Size(109, 20);
         this.scalingComboBox.StyleController = this.uxLayoutControl1;
         this.scalingComboBox.TabIndex = 9;
         // 
         // minTextBox
         // 
         this.minTextBox.Location = new System.Drawing.Point(216, 156);
         this.minTextBox.Name = "minTextBox";
         this.minTextBox.Size = new System.Drawing.Size(109, 20);
         this.minTextBox.StyleController = this.uxLayoutControl1;
         this.minTextBox.TabIndex = 13;
         // 
         // numberRepresentationComboBox
         // 
         this.numberRepresentationComboBox.Location = new System.Drawing.Point(216, 36);
         this.numberRepresentationComboBox.Name = "numberRepresentationComboBox";
         this.numberRepresentationComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.numberRepresentationComboBox.Size = new System.Drawing.Size(109, 20);
         this.numberRepresentationComboBox.StyleController = this.uxLayoutControl1;
         this.numberRepresentationComboBox.TabIndex = 6;
         // 
         // gridLinesCheckEdit
         // 
         this.gridLinesCheckEdit.AllowClicksOutsideControlArea = false;
         this.gridLinesCheckEdit.Location = new System.Drawing.Point(216, 204);
         this.gridLinesCheckEdit.Name = "gridLinesCheckEdit";
         this.gridLinesCheckEdit.Properties.AllowFocused = false;
         this.gridLinesCheckEdit.Properties.Caption = "";
         this.gridLinesCheckEdit.Size = new System.Drawing.Size(109, 19);
         this.gridLinesCheckEdit.StyleController = this.uxLayoutControl1;
         this.gridLinesCheckEdit.TabIndex = 11;
         // 
         // defaultLineSytleComboBox
         // 
         this.defaultLineSytleComboBox.Location = new System.Drawing.Point(216, 251);
         this.defaultLineSytleComboBox.Name = "defaultLineSytleComboBox";
         this.defaultLineSytleComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.defaultLineSytleComboBox.Size = new System.Drawing.Size(109, 20);
         this.defaultLineSytleComboBox.StyleController = this.uxLayoutControl1;
         this.defaultLineSytleComboBox.TabIndex = 5;
         // 
         // captionTextBox
         // 
         this.captionTextBox.Location = new System.Drawing.Point(216, 60);
         this.captionTextBox.Name = "captionTextBox";
         this.captionTextBox.Size = new System.Drawing.Size(109, 20);
         this.captionTextBox.StyleController = this.uxLayoutControl1;
         this.captionTextBox.TabIndex = 12;
         // 
         // unitComboBox
         // 
         this.unitComboBox.Location = new System.Drawing.Point(216, 108);
         this.unitComboBox.Name = "unitComboBox";
         this.unitComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.unitComboBox.Size = new System.Drawing.Size(109, 20);
         this.unitComboBox.StyleController = this.uxLayoutControl1;
         this.unitComboBox.TabIndex = 8;
         // 
         // defaultColorColorEdit
         // 
         this.defaultColorColorEdit.EditValue = System.Drawing.Color.Empty;
         this.defaultColorColorEdit.Location = new System.Drawing.Point(216, 227);
         this.defaultColorColorEdit.Name = "defaultColorColorEdit";
         this.defaultColorColorEdit.Properties.AutomaticColor = System.Drawing.Color.Black;
         this.defaultColorColorEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.defaultColorColorEdit.Properties.ShowSystemColors = false;
         this.defaultColorColorEdit.Properties.ShowWebColors = false;
         this.defaultColorColorEdit.Size = new System.Drawing.Size(109, 20);
         this.defaultColorColorEdit.StyleController = this.uxLayoutControl1;
         this.defaultColorColorEdit.TabIndex = 10;
         // 
         // axisTypeTextBox
         // 
         this.axisTypeTextBox.Location = new System.Drawing.Point(216, 12);
         this.axisTypeTextBox.Name = "axisTypeTextBox";
         this.axisTypeTextBox.Size = new System.Drawing.Size(109, 20);
         this.axisTypeTextBox.StyleController = this.uxLayoutControl1;
         this.axisTypeTextBox.TabIndex = 4;
         // 
         // maxTextBox
         // 
         this.maxTextBox.Location = new System.Drawing.Point(216, 180);
         this.maxTextBox.Name = "maxTextBox";
         this.maxTextBox.Size = new System.Drawing.Size(109, 20);
         this.maxTextBox.StyleController = this.uxLayoutControl1;
         this.maxTextBox.TabIndex = 14;
         // 
         // dimensionComboBox
         // 
         this.dimensionComboBox.Location = new System.Drawing.Point(216, 84);
         this.dimensionComboBox.Name = "dimensionComboBox";
         this.dimensionComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.dimensionComboBox.Size = new System.Drawing.Size(109, 20);
         this.dimensionComboBox.StyleController = this.uxLayoutControl1;
         this.dimensionComboBox.TabIndex = 7;
         // 
         // axisTypeLayoutControlItem
         // 
         this.axisTypeLayoutControlItem.Control = this.axisTypeTextBox;
         this.axisTypeLayoutControlItem.CustomizationFormText = "axisTypeLayoutControlItem";
         this.axisTypeLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.axisTypeLayoutControlItem.Name = "axisTypeLayoutControlItem";
         this.axisTypeLayoutControlItem.Size = new System.Drawing.Size(317, 24);
         this.axisTypeLayoutControlItem.Text = "axisTypeLayoutControlItem";
         this.axisTypeLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // defaultLineStyleLayoutControlItem
         // 
         this.defaultLineStyleLayoutControlItem.Control = this.defaultLineSytleComboBox;
         this.defaultLineStyleLayoutControlItem.CustomizationFormText = "layoutControlItem2";
         this.defaultLineStyleLayoutControlItem.Location = new System.Drawing.Point(0, 239);
         this.defaultLineStyleLayoutControlItem.Name = "defaultLineStyleLayoutControlItem";
         this.defaultLineStyleLayoutControlItem.Size = new System.Drawing.Size(317, 24);
         this.defaultLineStyleLayoutControlItem.Text = "defaultLineStyleLayoutControlItem";
         this.defaultLineStyleLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // numberRepresentationLayoutControlItem
         // 
         this.numberRepresentationLayoutControlItem.Control = this.numberRepresentationComboBox;
         this.numberRepresentationLayoutControlItem.CustomizationFormText = "numberRepresentationLayoutControlItem";
         this.numberRepresentationLayoutControlItem.Location = new System.Drawing.Point(0, 24);
         this.numberRepresentationLayoutControlItem.Name = "numberRepresentationLayoutControlItem";
         this.numberRepresentationLayoutControlItem.Size = new System.Drawing.Size(317, 24);
         this.numberRepresentationLayoutControlItem.Text = "numberRepresentationLayoutControlItem";
         this.numberRepresentationLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // dimensionLayoutControlItem
         // 
         this.dimensionLayoutControlItem.Control = this.dimensionComboBox;
         this.dimensionLayoutControlItem.CustomizationFormText = "layoutControlItem4";
         this.dimensionLayoutControlItem.Location = new System.Drawing.Point(0, 72);
         this.dimensionLayoutControlItem.Name = "dimensionLayoutControlItem";
         this.dimensionLayoutControlItem.Size = new System.Drawing.Size(317, 24);
         this.dimensionLayoutControlItem.Text = "dimensionLayoutControlItem";
         this.dimensionLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // unitLayoutControlItem
         // 
         this.unitLayoutControlItem.Control = this.unitComboBox;
         this.unitLayoutControlItem.CustomizationFormText = "layoutControlItem5";
         this.unitLayoutControlItem.Location = new System.Drawing.Point(0, 96);
         this.unitLayoutControlItem.Name = "unitLayoutControlItem";
         this.unitLayoutControlItem.Size = new System.Drawing.Size(317, 24);
         this.unitLayoutControlItem.Text = "unitLayoutControlItem";
         this.unitLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // scalingLayoutControlItem
         // 
         this.scalingLayoutControlItem.Control = this.scalingComboBox;
         this.scalingLayoutControlItem.CustomizationFormText = "layoutControlItem6";
         this.scalingLayoutControlItem.Location = new System.Drawing.Point(0, 120);
         this.scalingLayoutControlItem.Name = "scalingLayoutControlItem";
         this.scalingLayoutControlItem.Size = new System.Drawing.Size(317, 24);
         this.scalingLayoutControlItem.Text = "scalingLayoutControlItem";
         this.scalingLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // captionLayoutControlItem
         // 
         this.captionLayoutControlItem.Control = this.captionTextBox;
         this.captionLayoutControlItem.CustomizationFormText = "captionLayoutControlItem";
         this.captionLayoutControlItem.Location = new System.Drawing.Point(0, 48);
         this.captionLayoutControlItem.Name = "captionLayoutControlItem";
         this.captionLayoutControlItem.Size = new System.Drawing.Size(317, 24);
         this.captionLayoutControlItem.Text = "captionLayoutControlItem";
         this.captionLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // minLayoutControlItem
         // 
         this.minLayoutControlItem.Control = this.minTextBox;
         this.minLayoutControlItem.CustomizationFormText = "layoutControlItem10";
         this.minLayoutControlItem.Location = new System.Drawing.Point(0, 144);
         this.minLayoutControlItem.Name = "minLayoutControlItem";
         this.minLayoutControlItem.Size = new System.Drawing.Size(317, 24);
         this.minLayoutControlItem.Text = "minLayoutControlItem";
         this.minLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // maxLayoutControlItem
         // 
         this.maxLayoutControlItem.Control = this.maxTextBox;
         this.maxLayoutControlItem.CustomizationFormText = "layoutControlItem11";
         this.maxLayoutControlItem.Location = new System.Drawing.Point(0, 168);
         this.maxLayoutControlItem.Name = "maxLayoutControlItem";
         this.maxLayoutControlItem.Size = new System.Drawing.Size(317, 24);
         this.maxLayoutControlItem.Text = "maxLayoutControlItem";
         this.maxLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // gridLinesLayoutControlItem
         // 
         this.gridLinesLayoutControlItem.Control = this.gridLinesCheckEdit;
         this.gridLinesLayoutControlItem.CustomizationFormText = "layoutControlItem8";
         this.gridLinesLayoutControlItem.Location = new System.Drawing.Point(0, 192);
         this.gridLinesLayoutControlItem.Name = "gridLinesLayoutControlItem";
         this.gridLinesLayoutControlItem.Size = new System.Drawing.Size(317, 23);
         this.gridLinesLayoutControlItem.Text = "gridLinesLayoutControlItem";
         this.gridLinesLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // defaultColorLayoutControlItem
         // 
         this.defaultColorLayoutControlItem.Control = this.defaultColorColorEdit;
         this.defaultColorLayoutControlItem.CustomizationFormText = "layoutControlItem7";
         this.defaultColorLayoutControlItem.Location = new System.Drawing.Point(0, 215);
         this.defaultColorLayoutControlItem.Name = "defaultColorLayoutControlItem";
         this.defaultColorLayoutControlItem.Size = new System.Drawing.Size(317, 24);
         this.defaultColorLayoutControlItem.Text = "defaultColorLayoutControlItem";
         this.defaultColorLayoutControlItem.TextSize = new System.Drawing.Size(200, 13);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 263);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(317, 38);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // SingleAxisSettingsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(337, 367);
         this.Controls.Add(this.uxLayoutControl1);
         this.Margin = new System.Windows.Forms.Padding(0);
         this.Name = "SingleAxisSettingsView";
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
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.scalingComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.minTextBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberRepresentationComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridLinesCheckEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.defaultLineSytleComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.captionTextBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.unitComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.defaultColorColorEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.axisTypeTextBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.maxTextBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dimensionComboBox.Properties)).EndInit();
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
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private UxLayoutControl uxLayoutControl1;
      private TextEdit axisTypeTextBox;
      private UxLineTypeEdit defaultLineSytleComboBox;
      private UxNumberModeEdit numberRepresentationComboBox;
      private ComboBoxEdit dimensionComboBox;
      private ComboBoxEdit unitComboBox;
      private UxScalingsEdit scalingComboBox;
      private TextEdit captionTextBox;
      private TextEdit minTextBox;
      private TextEdit maxTextBox;
      private UxCheckEdit gridLinesCheckEdit;
      private UxColorPickEditWithHistory defaultColorColorEdit;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem defaultLineStyleLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem defaultColorLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem gridLinesLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem maxLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem minLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem scalingLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem unitLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem dimensionLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem captionLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem numberRepresentationLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem axisTypeLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
