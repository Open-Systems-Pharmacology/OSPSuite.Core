using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Charts
{
   internal partial class ChartExportSettingsView
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
       _screenBinderForExportSettings.Dispose();
       _screenBinderForFonts.Dispose();
       _screenBinderForChartManagement.Dispose();
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
         this.tbHeight = new DevExpress.XtraEditors.TextEdit();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnResetValues = new DevExpress.XtraEditors.SimpleButton();
         this.cbFontSizeOrigin = new DevExpress.XtraEditors.ComboBoxEdit();
         this.includeOriginDataInChartCheckEdit = new OSPSuite.UI.Controls.UxCheckEdit();
         this.cePreviewSettings = new OSPSuite.UI.Controls.UxCheckEdit();
         this.cbFontSizeDescription = new DevExpress.XtraEditors.ComboBoxEdit();
         this.cbFontSizeTitle = new DevExpress.XtraEditors.ComboBoxEdit();
         this.cbFontSizeLegend = new DevExpress.XtraEditors.ComboBoxEdit();
         this.cbFontSizeAxis = new DevExpress.XtraEditors.ComboBoxEdit();
         this.cbFontFamily = new DevExpress.XtraEditors.ComboBoxEdit();
         this.tbWidth = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemHeight = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemWidth = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemFontSizeAxis = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemFontSizeLegend = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemFontSizeTitle = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemFontSizeDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemFontSizeOrigin = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.cbFontSizeWatermark = new DevExpress.XtraEditors.ComboBoxEdit();
         this.layoutItemFontSizeWatermark = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbHeight.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbFontSizeOrigin.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.includeOriginDataInChartCheckEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cePreviewSettings.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFontSizeDescription.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFontSizeTitle.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFontSizeLegend.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFontSizeAxis.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFontFamily.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbWidth.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemHeight)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemWidth)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFontSizeAxis)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFontSizeLegend)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFontSizeTitle)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFontSizeDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFontSizeOrigin)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFontSizeWatermark.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFontSizeWatermark)).BeginInit();
         this.SuspendLayout();
         // 
         // tbHeight
         // 
         this.tbHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.tbHeight.Location = new System.Drawing.Point(161, 36);
         this.tbHeight.Name = "tbHeight";
         this.tbHeight.Size = new System.Drawing.Size(222, 20);
         this.tbHeight.StyleController = this.layoutControl;
         this.tbHeight.TabIndex = 0;
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.cbFontSizeWatermark);
         this.layoutControl.Controls.Add(this.btnResetValues);
         this.layoutControl.Controls.Add(this.cbFontSizeOrigin);
         this.layoutControl.Controls.Add(this.includeOriginDataInChartCheckEdit);
         this.layoutControl.Controls.Add(this.cePreviewSettings);
         this.layoutControl.Controls.Add(this.cbFontSizeDescription);
         this.layoutControl.Controls.Add(this.cbFontSizeTitle);
         this.layoutControl.Controls.Add(this.cbFontSizeLegend);
         this.layoutControl.Controls.Add(this.cbFontSizeAxis);
         this.layoutControl.Controls.Add(this.cbFontFamily);
         this.layoutControl.Controls.Add(this.tbHeight);
         this.layoutControl.Controls.Add(this.tbWidth);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(-1178, 339, 385, 511);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(395, 309);
         this.layoutControl.TabIndex = 12;
         this.layoutControl.Text = "layoutControl1";
         // 
         // btnResetValues
         // 
         this.btnResetValues.Location = new System.Drawing.Point(12, 274);
         this.btnResetValues.Name = "btnResetValues";
         this.btnResetValues.Size = new System.Drawing.Size(118, 22);
         this.btnResetValues.StyleController = this.layoutControl;
         this.btnResetValues.TabIndex = 21;
         this.btnResetValues.Text = "btnResetValues";
         // 
         // fontSizeOriginComboBox
         // 
         this.cbFontSizeOrigin.Location = new System.Drawing.Point(161, 180);
         this.cbFontSizeOrigin.Name = "cbFontSizeOrigin";
         this.cbFontSizeOrigin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbFontSizeOrigin.Size = new System.Drawing.Size(222, 20);
         this.cbFontSizeOrigin.StyleController = this.layoutControl;
         this.cbFontSizeOrigin.TabIndex = 20;
         // 
         // includeOriginDataInChartCheckEdit
         // 
         this.includeOriginDataInChartCheckEdit.AllowClicksOutsideControlArea = false;
         this.includeOriginDataInChartCheckEdit.Location = new System.Drawing.Point(12, 251);
         this.includeOriginDataInChartCheckEdit.Name = "includeOriginDataInChartCheckEdit";
         this.includeOriginDataInChartCheckEdit.Properties.Caption = "includeOriginDataInChart";
         this.includeOriginDataInChartCheckEdit.Size = new System.Drawing.Size(371, 19);
         this.includeOriginDataInChartCheckEdit.StyleController = this.layoutControl;
         this.includeOriginDataInChartCheckEdit.TabIndex = 19;
         // 
         // cePreviewSettings
         // 
         this.cePreviewSettings.AllowClicksOutsideControlArea = false;
         this.cePreviewSettings.Location = new System.Drawing.Point(12, 228);
         this.cePreviewSettings.Name = "cePreviewSettings";
         this.cePreviewSettings.Properties.Caption = "cePreviewSettings";
         this.cePreviewSettings.Size = new System.Drawing.Size(371, 19);
         this.cePreviewSettings.StyleController = this.layoutControl;
         this.cePreviewSettings.TabIndex = 18;
         // 
         // cbFontSizeDescription
         // 
         this.cbFontSizeDescription.Location = new System.Drawing.Point(161, 156);
         this.cbFontSizeDescription.Name = "cbFontSizeDescription";
         this.cbFontSizeDescription.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbFontSizeDescription.Size = new System.Drawing.Size(222, 20);
         this.cbFontSizeDescription.StyleController = this.layoutControl;
         this.cbFontSizeDescription.TabIndex = 17;
         // 
         // cbFontSizeTitle
         // 
         this.cbFontSizeTitle.Location = new System.Drawing.Point(161, 132);
         this.cbFontSizeTitle.Name = "cbFontSizeTitle";
         this.cbFontSizeTitle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbFontSizeTitle.Size = new System.Drawing.Size(222, 20);
         this.cbFontSizeTitle.StyleController = this.layoutControl;
         this.cbFontSizeTitle.TabIndex = 16;
         // 
         // cbFontSizeLegend
         // 
         this.cbFontSizeLegend.Location = new System.Drawing.Point(161, 108);
         this.cbFontSizeLegend.Name = "cbFontSizeLegend";
         this.cbFontSizeLegend.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbFontSizeLegend.Size = new System.Drawing.Size(222, 20);
         this.cbFontSizeLegend.StyleController = this.layoutControl;
         this.cbFontSizeLegend.TabIndex = 15;
         // 
         // cbFontSizeAxis
         // 
         this.cbFontSizeAxis.Location = new System.Drawing.Point(161, 84);
         this.cbFontSizeAxis.Name = "cbFontSizeAxis";
         this.cbFontSizeAxis.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbFontSizeAxis.Size = new System.Drawing.Size(222, 20);
         this.cbFontSizeAxis.StyleController = this.layoutControl;
         this.cbFontSizeAxis.TabIndex = 14;
         // 
         // cbFontFamily
         // 
         this.cbFontFamily.Location = new System.Drawing.Point(161, 60);
         this.cbFontFamily.Name = "cbFontFamily";
         this.cbFontFamily.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbFontFamily.Size = new System.Drawing.Size(222, 20);
         this.cbFontFamily.StyleController = this.layoutControl;
         this.cbFontFamily.TabIndex = 9;
         // 
         // tbWidth
         // 
         this.tbWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.tbWidth.Location = new System.Drawing.Point(161, 12);
         this.tbWidth.Name = "tbWidth";
         this.tbWidth.Size = new System.Drawing.Size(222, 20);
         this.tbWidth.StyleController = this.layoutControl;
         this.tbWidth.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutItemHeight,
            this.layoutItemWidth,
            this.layoutItemFontSizeAxis,
            this.layoutItemFontSizeLegend,
            this.layoutItemFontSizeTitle,
            this.layoutItemFontSizeDescription,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutItemFontSizeOrigin,
            this.layoutControlItem4,
            this.emptySpaceItem1,
            this.layoutItemFontSizeWatermark});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(395, 309);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.cbFontFamily;
         this.layoutControlItem1.CustomizationFormText = "Font:";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 48);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(375, 24);
         this.layoutControlItem1.Text = "Font:";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(146, 13);
         // 
         // layoutItemHeight
         // 
         this.layoutItemHeight.Control = this.tbHeight;
         this.layoutItemHeight.CustomizationFormText = "Title:";
         this.layoutItemHeight.Location = new System.Drawing.Point(0, 24);
         this.layoutItemHeight.Name = "layoutItemHeight";
         this.layoutItemHeight.Size = new System.Drawing.Size(375, 24);
         this.layoutItemHeight.Text = "Height:";
         this.layoutItemHeight.TextSize = new System.Drawing.Size(146, 13);
         // 
         // layoutItemWidth
         // 
         this.layoutItemWidth.Control = this.tbWidth;
         this.layoutItemWidth.CustomizationFormText = "layoutItemWidth";
         this.layoutItemWidth.Location = new System.Drawing.Point(0, 0);
         this.layoutItemWidth.Name = "layoutItemWidth";
         this.layoutItemWidth.Size = new System.Drawing.Size(375, 24);
         this.layoutItemWidth.TextSize = new System.Drawing.Size(146, 13);
         // 
         // layoutItemFontSizeAxis
         // 
         this.layoutItemFontSizeAxis.Control = this.cbFontSizeAxis;
         this.layoutItemFontSizeAxis.CustomizationFormText = "layoutItemFontSizeAxis";
         this.layoutItemFontSizeAxis.Location = new System.Drawing.Point(0, 72);
         this.layoutItemFontSizeAxis.Name = "layoutItemFontSizeAxis";
         this.layoutItemFontSizeAxis.Size = new System.Drawing.Size(375, 24);
         this.layoutItemFontSizeAxis.TextSize = new System.Drawing.Size(146, 13);
         // 
         // layoutItemFontSizeLegend
         // 
         this.layoutItemFontSizeLegend.Control = this.cbFontSizeLegend;
         this.layoutItemFontSizeLegend.CustomizationFormText = "layoutItemFontSizeLegend";
         this.layoutItemFontSizeLegend.Location = new System.Drawing.Point(0, 96);
         this.layoutItemFontSizeLegend.Name = "layoutItemFontSizeLegend";
         this.layoutItemFontSizeLegend.Size = new System.Drawing.Size(375, 24);
         this.layoutItemFontSizeLegend.TextSize = new System.Drawing.Size(146, 13);
         // 
         // layoutItemFontSizeTitle
         // 
         this.layoutItemFontSizeTitle.Control = this.cbFontSizeTitle;
         this.layoutItemFontSizeTitle.CustomizationFormText = "layoutItemFontSizeTitle";
         this.layoutItemFontSizeTitle.Location = new System.Drawing.Point(0, 120);
         this.layoutItemFontSizeTitle.Name = "layoutItemFontSizeTitle";
         this.layoutItemFontSizeTitle.Size = new System.Drawing.Size(375, 24);
         this.layoutItemFontSizeTitle.TextSize = new System.Drawing.Size(146, 13);
         // 
         // layoutItemFontSizeDescription
         // 
         this.layoutItemFontSizeDescription.Control = this.cbFontSizeDescription;
         this.layoutItemFontSizeDescription.CustomizationFormText = "layoutItemFontSizeDescription";
         this.layoutItemFontSizeDescription.Location = new System.Drawing.Point(0, 144);
         this.layoutItemFontSizeDescription.Name = "layoutItemFontSizeDescription";
         this.layoutItemFontSizeDescription.Size = new System.Drawing.Size(375, 24);
         this.layoutItemFontSizeDescription.TextSize = new System.Drawing.Size(146, 13);
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.cePreviewSettings;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 216);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(375, 23);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.includeOriginDataInChartCheckEdit;
         this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 239);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(375, 23);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // layoutItemFontSizeOrigin
         // 
         this.layoutItemFontSizeOrigin.Control = this.cbFontSizeOrigin;
         this.layoutItemFontSizeOrigin.CustomizationFormText = "layoutItemFontSizeOrigin";
         this.layoutItemFontSizeOrigin.Location = new System.Drawing.Point(0, 168);
         this.layoutItemFontSizeOrigin.Name = "layoutItemFontSizeOrigin";
         this.layoutItemFontSizeOrigin.Size = new System.Drawing.Size(375, 24);
         this.layoutItemFontSizeOrigin.TextSize = new System.Drawing.Size(146, 13);
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.btnResetValues;
         this.layoutControlItem4.Location = new System.Drawing.Point(0, 262);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Size = new System.Drawing.Size(122, 27);
         this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem4.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(122, 262);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(253, 27);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // fontSizeWatermarkComboBox
         // 
         this.cbFontSizeWatermark.Location = new System.Drawing.Point(161, 204);
         this.cbFontSizeWatermark.Name = "cbFontSizeWatermark";
         this.cbFontSizeWatermark.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbFontSizeWatermark.Size = new System.Drawing.Size(222, 20);
         this.cbFontSizeWatermark.StyleController = this.layoutControl;
         this.cbFontSizeWatermark.TabIndex = 22;
         // 
         // layoutItemFontSizeWatermark
         // 
         this.layoutItemFontSizeWatermark.Control = this.cbFontSizeWatermark;
         this.layoutItemFontSizeWatermark.Location = new System.Drawing.Point(0, 192);
         this.layoutItemFontSizeWatermark.Name = "layoutItemFontSizeWatermark";
         this.layoutItemFontSizeWatermark.Size = new System.Drawing.Size(375, 24);
         this.layoutItemFontSizeWatermark.TextSize = new System.Drawing.Size(146, 13);
         // 
         // ChartExportSettingsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ChartExportSettingsView";
         this.Size = new System.Drawing.Size(395, 309);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbHeight.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cbFontSizeOrigin.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.includeOriginDataInChartCheckEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cePreviewSettings.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFontSizeDescription.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFontSizeTitle.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFontSizeLegend.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFontSizeAxis.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFontFamily.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbWidth.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemHeight)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemWidth)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFontSizeAxis)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFontSizeLegend)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFontSizeTitle)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFontSizeDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFontSizeOrigin)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFontSizeWatermark.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFontSizeWatermark)).EndInit();
         this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraEditors.TextEdit tbHeight;
    private DevExpress.XtraEditors.TextEdit tbWidth;
    private DevExpress.XtraEditors.ComboBoxEdit cbFontFamily;
    private UxLayoutControl layoutControl;
    private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
    private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    private DevExpress.XtraLayout.LayoutControlItem layoutItemHeight;
    private DevExpress.XtraLayout.LayoutControlItem layoutItemWidth;
    private DevExpress.XtraEditors.ComboBoxEdit cbFontSizeDescription;
    private DevExpress.XtraEditors.ComboBoxEdit cbFontSizeTitle;
    private DevExpress.XtraEditors.ComboBoxEdit cbFontSizeLegend;
    private DevExpress.XtraEditors.ComboBoxEdit cbFontSizeAxis;
    private DevExpress.XtraLayout.LayoutControlItem layoutItemFontSizeAxis;
    private DevExpress.XtraLayout.LayoutControlItem layoutItemFontSizeLegend;
    private DevExpress.XtraLayout.LayoutControlItem layoutItemFontSizeTitle;
    private DevExpress.XtraLayout.LayoutControlItem layoutItemFontSizeDescription;
    private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
    private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
    private DevExpress.XtraEditors.ComboBoxEdit cbFontSizeOrigin;
    private DevExpress.XtraLayout.LayoutControlItem layoutItemFontSizeOrigin;
    private DevExpress.XtraEditors.SimpleButton btnResetValues;
    private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
    private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.ComboBoxEdit cbFontSizeWatermark;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFontSizeWatermark;
      private UxCheckEdit cePreviewSettings;
      private UxCheckEdit includeOriginDataInChartCheckEdit;
   }
}