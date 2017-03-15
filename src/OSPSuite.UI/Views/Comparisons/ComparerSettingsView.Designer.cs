namespace OSPSuite.UI.Views.Comparisons
{
   partial class ComparerSettingsView
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
         this.lblFormulaComparisonModeDescription = new DevExpress.XtraEditors.LabelControl();
         this.lblRelativeToleranceDescription = new DevExpress.XtraEditors.LabelControl();
         this.cbFormulaComparisonMode = new DevExpress.XtraEditors.ComboBoxEdit();
         this.chkOnlyComputeModelRelevantProperties = new DevExpress.XtraEditors.CheckEdit();
         this.tbRelativeTolerance = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemOnlyComputeModelRelevantProperties = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutGroupFormulaComparisonMode = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemFormulaComparisonMode = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupRelativeTolerance = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemRelativeTolerance = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbFormulaComparisonMode.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkOnlyComputeModelRelevantProperties.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbRelativeTolerance.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOnlyComputeModelRelevantProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupFormulaComparisonMode)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaComparisonMode)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupRelativeTolerance)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRelativeTolerance)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.lblFormulaComparisonModeDescription);
         this.layoutControl.Controls.Add(this.lblRelativeToleranceDescription);
         this.layoutControl.Controls.Add(this.cbFormulaComparisonMode);
         this.layoutControl.Controls.Add(this.chkOnlyComputeModelRelevantProperties);
         this.layoutControl.Controls.Add(this.tbRelativeTolerance);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(285, 192);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // lblFormulaComparisonModeDescription
         // 
         this.lblFormulaComparisonModeDescription.Location = new System.Drawing.Point(14, 103);
         this.lblFormulaComparisonModeDescription.Name = "lblFormulaComparisonModeDescription";
         this.lblFormulaComparisonModeDescription.Size = new System.Drawing.Size(183, 13);
         this.lblFormulaComparisonModeDescription.StyleController = this.layoutControl;
         this.lblFormulaComparisonModeDescription.TabIndex = 8;
         this.lblFormulaComparisonModeDescription.Text = "lblFormulaComparisonModeDescription";
         // 
         // lblRelativeToleranceDescription
         // 
         this.lblRelativeToleranceDescription.Location = new System.Drawing.Point(14, 38);
         this.lblRelativeToleranceDescription.Name = "lblRelativeToleranceDescription";
         this.lblRelativeToleranceDescription.Size = new System.Drawing.Size(149, 13);
         this.lblRelativeToleranceDescription.StyleController = this.layoutControl;
         this.lblRelativeToleranceDescription.TabIndex = 7;
         this.lblRelativeToleranceDescription.Text = "lblRelativeToleranceDescription";
         // 
         // cbFormulaComparisonMode
         // 
         this.cbFormulaComparisonMode.Location = new System.Drawing.Point(189, 79);
         this.cbFormulaComparisonMode.Name = "cbFormulaComparisonMode";
         this.cbFormulaComparisonMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbFormulaComparisonMode.Size = new System.Drawing.Size(82, 20);
         this.cbFormulaComparisonMode.StyleController = this.layoutControl;
         this.cbFormulaComparisonMode.TabIndex = 6;
         // 
         // chkOnlyComputeModelRelevantProperties
         // 
         this.chkOnlyComputeModelRelevantProperties.Location = new System.Drawing.Point(2, 132);
         this.chkOnlyComputeModelRelevantProperties.Name = "chkOnlyComputeModelRelevantProperties";
         this.chkOnlyComputeModelRelevantProperties.Properties.Caption = "chkOnlyComputeModelRelevantProperties";
         this.chkOnlyComputeModelRelevantProperties.Size = new System.Drawing.Size(281, 19);
         this.chkOnlyComputeModelRelevantProperties.StyleController = this.layoutControl;
         this.chkOnlyComputeModelRelevantProperties.TabIndex = 5;
         // 
         // tbRelativeTolerance
         // 
         this.tbRelativeTolerance.Location = new System.Drawing.Point(189, 14);
         this.tbRelativeTolerance.Name = "tbRelativeTolerance";
         this.tbRelativeTolerance.Size = new System.Drawing.Size(82, 20);
         this.tbRelativeTolerance.StyleController = this.layoutControl;
         this.tbRelativeTolerance.TabIndex = 4;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.CustomizationFormText = "layoutControlGroup";
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemOnlyComputeModelRelevantProperties,
            this.emptySpaceItem1,
            this.layoutGroupFormulaComparisonMode,
            this.layoutGroupRelativeTolerance});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(285, 192);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemOnlyComputeModelRelevantProperties
         // 
         this.layoutItemOnlyComputeModelRelevantProperties.Control = this.chkOnlyComputeModelRelevantProperties;
         this.layoutItemOnlyComputeModelRelevantProperties.CustomizationFormText = "layoutItemOnlyComputeModelRelevantProperties";
         this.layoutItemOnlyComputeModelRelevantProperties.Location = new System.Drawing.Point(0, 130);
         this.layoutItemOnlyComputeModelRelevantProperties.Name = "layoutItemOnlyComputeModelRelevantProperties";
         this.layoutItemOnlyComputeModelRelevantProperties.Size = new System.Drawing.Size(285, 23);
         this.layoutItemOnlyComputeModelRelevantProperties.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemOnlyComputeModelRelevantProperties.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 153);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(285, 39);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutGroupFormulaComparisonMode
         // 
         this.layoutGroupFormulaComparisonMode.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemFormulaComparisonMode,
            this.layoutControlItem2});
         this.layoutGroupFormulaComparisonMode.Location = new System.Drawing.Point(0, 65);
         this.layoutGroupFormulaComparisonMode.Name = "layoutGroupFormulaComparisonMode";
         this.layoutGroupFormulaComparisonMode.Size = new System.Drawing.Size(285, 65);
         this.layoutGroupFormulaComparisonMode.TextVisible = false;
         // 
         // layoutItemFormulaComparisonMode
         // 
         this.layoutItemFormulaComparisonMode.Control = this.cbFormulaComparisonMode;
         this.layoutItemFormulaComparisonMode.CustomizationFormText = "layoutItemFormulaComparisonMode";
         this.layoutItemFormulaComparisonMode.Location = new System.Drawing.Point(0, 0);
         this.layoutItemFormulaComparisonMode.Name = "layoutItemFormulaComparisonMode";
         this.layoutItemFormulaComparisonMode.Size = new System.Drawing.Size(261, 24);
         this.layoutItemFormulaComparisonMode.TextSize = new System.Drawing.Size(172, 13);
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.lblFormulaComparisonModeDescription;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(261, 17);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutGroupRelativeTolerance
         // 
         this.layoutGroupRelativeTolerance.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemRelativeTolerance,
            this.layoutControlItem1});
         this.layoutGroupRelativeTolerance.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupRelativeTolerance.Name = "layoutGroupRelativeTolerance";
         this.layoutGroupRelativeTolerance.Size = new System.Drawing.Size(285, 65);
         this.layoutGroupRelativeTolerance.TextVisible = false;
         // 
         // layoutItemRelativeTolerance
         // 
         this.layoutItemRelativeTolerance.Control = this.tbRelativeTolerance;
         this.layoutItemRelativeTolerance.CustomizationFormText = "layoutItemRelativeTolerance";
         this.layoutItemRelativeTolerance.Location = new System.Drawing.Point(0, 0);
         this.layoutItemRelativeTolerance.Name = "layoutItemRelativeTolerance";
         this.layoutItemRelativeTolerance.Size = new System.Drawing.Size(261, 24);
         this.layoutItemRelativeTolerance.TextSize = new System.Drawing.Size(172, 13);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.lblRelativeToleranceDescription;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(261, 17);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // ComparerSettingsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ComparerSettingsView";
         this.Size = new System.Drawing.Size(285, 192);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cbFormulaComparisonMode.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkOnlyComputeModelRelevantProperties.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbRelativeTolerance.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOnlyComputeModelRelevantProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupFormulaComparisonMode)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaComparisonMode)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupRelativeTolerance)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRelativeTolerance)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraEditors.ComboBoxEdit cbFormulaComparisonMode;
      private DevExpress.XtraEditors.CheckEdit chkOnlyComputeModelRelevantProperties;
      private DevExpress.XtraEditors.TextEdit tbRelativeTolerance;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemRelativeTolerance;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemOnlyComputeModelRelevantProperties;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFormulaComparisonMode;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.LabelControl lblFormulaComparisonModeDescription;
      private DevExpress.XtraEditors.LabelControl lblRelativeToleranceDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupFormulaComparisonMode;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupRelativeTolerance;
   }
}
