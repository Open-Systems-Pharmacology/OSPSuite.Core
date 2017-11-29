using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Charts
{
  internal partial class ChartSettingsView
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
       _settingsBinder.Dispose();
       _nameBinder.Dispose();
       _curveChartBinder.Dispose();
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
         this.titleTextBox = new DevExpress.XtraEditors.TextEdit();
         this.layoutControl = new UxLayoutControl();
         this.sideMarginsEnabledCheckEdit = new OSPSuite.UI.Controls.UxCheckEdit();
         this.legendPositionComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.diagramBackgroundColorColorEdit = new UxColorPickEditWithHistory();
         this.backgroundColorColorEdit = new UxColorPickEditWithHistory();
         this.descriptionTextBox = new DevExpress.XtraEditors.MemoEdit();
         this.nameTextBox = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.legendPositionControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.descriptionControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.titleControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.nameControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.chartColorControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.diagramBackgroundControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.titleTextBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.sideMarginsEnabledCheckEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.legendPositionComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.diagramBackgroundColorColorEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.backgroundColorColorEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.descriptionTextBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nameTextBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.legendPositionControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.descriptionControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.titleControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nameControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartColorControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.diagramBackgroundControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
         this.SuspendLayout();
         // 
         // titleTextBox
         // 
         this.titleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.titleTextBox.Location = new System.Drawing.Point(166, 36);
         this.titleTextBox.Name = "titleTextBox";
         this.titleTextBox.Size = new System.Drawing.Size(214, 20);
         this.titleTextBox.StyleController = this.layoutControl;
         this.titleTextBox.TabIndex = 0;
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.sideMarginsEnabledCheckEdit);
         this.layoutControl.Controls.Add(this.legendPositionComboBoxEdit);
         this.layoutControl.Controls.Add(this.diagramBackgroundColorColorEdit);
         this.layoutControl.Controls.Add(this.backgroundColorColorEdit);
         this.layoutControl.Controls.Add(this.descriptionTextBox);
         this.layoutControl.Controls.Add(this.titleTextBox);
         this.layoutControl.Controls.Add(this.nameTextBox);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(-1178, 339, 385, 511);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(392, 293);
         this.layoutControl.TabIndex = 12;
         this.layoutControl.Text = "layoutControl1";
         // 
         // sideMarginsEnabledCheckEdit
         // 
         this.sideMarginsEnabledCheckEdit.EditValue = true;
         this.sideMarginsEnabledCheckEdit.Location = new System.Drawing.Point(12, 262);
         this.sideMarginsEnabledCheckEdit.Name = "sideMarginsEnabledCheckEdit";
         this.sideMarginsEnabledCheckEdit.Properties.Caption = "sideMarginsEnabledCheckEdit";
         this.sideMarginsEnabledCheckEdit.Size = new System.Drawing.Size(368, 19);
         this.sideMarginsEnabledCheckEdit.StyleController = this.layoutControl;
         this.sideMarginsEnabledCheckEdit.TabIndex = 12;
         // 
         // legendPositionComboBoxEdit
         // 
         this.legendPositionComboBoxEdit.Location = new System.Drawing.Point(166, 190);
         this.legendPositionComboBoxEdit.Name = "legendPositionComboBoxEdit";
         this.legendPositionComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.legendPositionComboBoxEdit.Size = new System.Drawing.Size(214, 20);
         this.legendPositionComboBoxEdit.StyleController = this.layoutControl;
         this.legendPositionComboBoxEdit.TabIndex = 9;
         // 
         // diagramBackgroundColorColorEdit
         // 
         this.diagramBackgroundColorColorEdit.EditValue = System.Drawing.Color.Empty;
         this.diagramBackgroundColorColorEdit.Location = new System.Drawing.Point(166, 238);
         this.diagramBackgroundColorColorEdit.Name = "diagramBackgroundColorColorEdit";
         this.diagramBackgroundColorColorEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.diagramBackgroundColorColorEdit.Size = new System.Drawing.Size(214, 20);
         this.diagramBackgroundColorColorEdit.StyleController = this.layoutControl;
         this.diagramBackgroundColorColorEdit.TabIndex = 11;
         // 
         // backgroundColorColorEdit
         // 
         this.backgroundColorColorEdit.EditValue = System.Drawing.Color.Empty;
         this.backgroundColorColorEdit.Location = new System.Drawing.Point(166, 214);
         this.backgroundColorColorEdit.Name = "backgroundColorColorEdit";
         this.backgroundColorColorEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.backgroundColorColorEdit.Size = new System.Drawing.Size(214, 20);
         this.backgroundColorColorEdit.StyleController = this.layoutControl;
         this.backgroundColorColorEdit.TabIndex = 10;
         // 
         // descriptionTextBox
         // 
         this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.descriptionTextBox.Location = new System.Drawing.Point(166, 60);
         this.descriptionTextBox.Name = "descriptionTextBox";
         this.descriptionTextBox.Size = new System.Drawing.Size(214, 126);
         this.descriptionTextBox.StyleController = this.layoutControl;
         this.descriptionTextBox.TabIndex = 2;
         // 
         // nameTextBox
         // 
         this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.nameTextBox.Location = new System.Drawing.Point(166, 12);
         this.nameTextBox.Name = "nameTextBox";
         this.nameTextBox.Size = new System.Drawing.Size(214, 20);
         this.nameTextBox.StyleController = this.layoutControl;
         this.nameTextBox.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.legendPositionControlItem,
            this.descriptionControlItem,
            this.titleControlItem,
            this.nameControlItem,
            this.chartColorControlItem,
            this.diagramBackgroundControlItem,
            this.layoutControlItem7});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(392, 293);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         // 
         // legendPositionControlItem
         // 
         this.legendPositionControlItem.Control = this.legendPositionComboBoxEdit;
         this.legendPositionControlItem.CustomizationFormText = "Legend Position:";
         this.legendPositionControlItem.Location = new System.Drawing.Point(0, 178);
         this.legendPositionControlItem.Name = "legendPositionControlItem";
         this.legendPositionControlItem.Size = new System.Drawing.Size(372, 24);
         this.legendPositionControlItem.Text = "legendPositionControlItem";
         this.legendPositionControlItem.TextSize = new System.Drawing.Size(151, 13);
         // 
         // descriptionControlItem
         // 
         this.descriptionControlItem.Control = this.descriptionTextBox;
         this.descriptionControlItem.CustomizationFormText = "Description:";
         this.descriptionControlItem.Location = new System.Drawing.Point(0, 48);
         this.descriptionControlItem.Name = "descriptionControlItem";
         this.descriptionControlItem.Size = new System.Drawing.Size(372, 130);
         this.descriptionControlItem.Text = "descriptionControlItem";
         this.descriptionControlItem.TextSize = new System.Drawing.Size(151, 13);
         // 
         // titleControlItem
         // 
         this.titleControlItem.Control = this.titleTextBox;
         this.titleControlItem.CustomizationFormText = "Title:";
         this.titleControlItem.Location = new System.Drawing.Point(0, 24);
         this.titleControlItem.Name = "titleControlItem";
         this.titleControlItem.Size = new System.Drawing.Size(372, 24);
         this.titleControlItem.Text = "titleControlItem";
         this.titleControlItem.TextSize = new System.Drawing.Size(151, 13);
         // 
         // nameControlItem
         // 
         this.nameControlItem.Control = this.nameTextBox;
         this.nameControlItem.CustomizationFormText = "Name:";
         this.nameControlItem.Location = new System.Drawing.Point(0, 0);
         this.nameControlItem.Name = "nameControlItem";
         this.nameControlItem.Size = new System.Drawing.Size(372, 24);
         this.nameControlItem.Text = "nameControlItem";
         this.nameControlItem.TextSize = new System.Drawing.Size(151, 13);
         // 
         // chartColorControlItem
         // 
         this.chartColorControlItem.Control = this.backgroundColorColorEdit;
         this.chartColorControlItem.CustomizationFormText = "Chart Color:";
         this.chartColorControlItem.Location = new System.Drawing.Point(0, 202);
         this.chartColorControlItem.Name = "chartColorControlItem";
         this.chartColorControlItem.Size = new System.Drawing.Size(372, 24);
         this.chartColorControlItem.Text = "chartColorControlItem";
         this.chartColorControlItem.TextSize = new System.Drawing.Size(151, 13);
         // 
         // diagramBackgroundControlItem
         // 
         this.diagramBackgroundControlItem.Control = this.diagramBackgroundColorColorEdit;
         this.diagramBackgroundControlItem.CustomizationFormText = "Diagram Background:";
         this.diagramBackgroundControlItem.Location = new System.Drawing.Point(0, 226);
         this.diagramBackgroundControlItem.Name = "diagramBackgroundControlItem";
         this.diagramBackgroundControlItem.Size = new System.Drawing.Size(372, 24);
         this.diagramBackgroundControlItem.Text = "diagramBackgroundControlItem";
         this.diagramBackgroundControlItem.TextSize = new System.Drawing.Size(151, 13);
         // 
         // layoutControlItem7
         // 
         this.layoutControlItem7.Control = this.sideMarginsEnabledCheckEdit;
         this.layoutControlItem7.CustomizationFormText = "Side Margins Enabled:";
         this.layoutControlItem7.Location = new System.Drawing.Point(0, 250);
         this.layoutControlItem7.Name = "layoutControlItem7";
         this.layoutControlItem7.Size = new System.Drawing.Size(372, 23);
         this.layoutControlItem7.Text = "Side Margins Enabled:";
         this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem7.TextToControlDistance = 0;
         this.layoutControlItem7.TextVisible = false;
         // 
         // ChartSettingsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ChartSettingsView";
         this.Size = new System.Drawing.Size(392, 293);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.titleTextBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.sideMarginsEnabledCheckEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.legendPositionComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.diagramBackgroundColorColorEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.backgroundColorColorEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.descriptionTextBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nameTextBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.legendPositionControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.descriptionControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.titleControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nameControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartColorControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.diagramBackgroundControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
         this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraEditors.TextEdit titleTextBox;
    private DevExpress.XtraEditors.MemoEdit descriptionTextBox;
    private DevExpress.XtraEditors.TextEdit nameTextBox;
    private DevExpress.XtraEditors.ComboBoxEdit legendPositionComboBoxEdit;
    private UxColorPickEditWithHistory backgroundColorColorEdit;
    private UxColorPickEditWithHistory diagramBackgroundColorColorEdit;
    private UxLayoutControl layoutControl;
    private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
    private DevExpress.XtraLayout.LayoutControlItem legendPositionControlItem;
    private DevExpress.XtraLayout.LayoutControlItem chartColorControlItem;
    private DevExpress.XtraLayout.LayoutControlItem diagramBackgroundControlItem;
    private DevExpress.XtraLayout.LayoutControlItem descriptionControlItem;
    private DevExpress.XtraLayout.LayoutControlItem titleControlItem;
    private DevExpress.XtraLayout.LayoutControlItem nameControlItem;
    private DevExpress.XtraEditors.CheckEdit sideMarginsEnabledCheckEdit;
    private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;

  }
}