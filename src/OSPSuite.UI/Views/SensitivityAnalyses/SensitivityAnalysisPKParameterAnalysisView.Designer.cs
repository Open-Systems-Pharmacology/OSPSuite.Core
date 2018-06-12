using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.SensitivityAnalyses
{
   partial class SensitivityAnalysisPKParameterAnalysisView
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
         DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle2 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle3 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle4 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle5 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle6 = new DevExpress.XtraCharts.ChartTitle();
         this.chartControl = new UxChartControl();
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.cbOutputPathSelection = new DevExpress.XtraEditors.ComboBoxEdit();
         this.lblSensitivityNotCalculated = new DevExpress.XtraEditors.LabelControl();
         this.cbPKParameter = new DevExpress.XtraEditors.ComboBoxEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.labelLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.outputPathSelectionLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.pKParameterSelectionLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.chartLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.chartControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbOutputPathSelection.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbPKParameter.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.labelLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.outputPathSelectionLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pKParameterSelectionLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartLayoutControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.chartControl);
         this.layoutControl1.Controls.Add(this.cbOutputPathSelection);
         this.layoutControl1.Controls.Add(this.lblSensitivityNotCalculated);
         this.layoutControl1.Controls.Add(this.cbPKParameter);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1226, 293, 629, 542);
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(787, 437);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // chartControl
         // 
         this.chartControl.Description = "";
         this.chartControl.DiagramBackColor = System.Drawing.Color.Empty;
         this.chartControl.Location = new System.Drawing.Point(175, 60);
         this.chartControl.Name = "chartControl";
         this.chartControl.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
         this.chartControl.Size = new System.Drawing.Size(600, 348);
         this.chartControl.TabIndex = 8;
         this.chartControl.Title = "";
         chartTitle1.Font = new System.Drawing.Font("Arial", 16F);
         chartTitle1.Text = "";
         chartTitle1.WordWrap = true;
         chartTitle2.Alignment = System.Drawing.StringAlignment.Near;
         chartTitle2.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
         chartTitle2.Font = new System.Drawing.Font("Arial", 12F);
         chartTitle2.Text = "";
         chartTitle2.WordWrap = true;
         chartTitle3.Font = new System.Drawing.Font("Arial", 16F);
         chartTitle3.Text = "";
         chartTitle3.WordWrap = true;
         chartTitle4.Alignment = System.Drawing.StringAlignment.Near;
         chartTitle4.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
         chartTitle4.Font = new System.Drawing.Font("Arial", 12F);
         chartTitle4.Text = "";
         chartTitle4.WordWrap = true;
         chartTitle5.Font = new System.Drawing.Font("Arial", 16F);
         chartTitle5.Text = "";
         chartTitle5.WordWrap = true;
         chartTitle6.Alignment = System.Drawing.StringAlignment.Near;
         chartTitle6.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
         chartTitle6.Font = new System.Drawing.Font("Arial", 12F);
         chartTitle6.Text = "";
         chartTitle6.WordWrap = true;
         this.chartControl.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1,
            chartTitle2,
            chartTitle3,
            chartTitle4,
            chartTitle5,
            chartTitle6});
         // 
         // cbOutputPathSelection
         // 
         this.cbOutputPathSelection.Location = new System.Drawing.Point(175, 12);
         this.cbOutputPathSelection.Name = "cbOutputPathSelection";
         this.cbOutputPathSelection.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbOutputPathSelection.Size = new System.Drawing.Size(600, 20);
         this.cbOutputPathSelection.StyleController = this.layoutControl1;
         this.cbOutputPathSelection.TabIndex = 7;
         // 
         // lblSensitivityNotCalculated
         // 
         this.lblSensitivityNotCalculated.Location = new System.Drawing.Point(175, 412);
         this.lblSensitivityNotCalculated.Name = "lblSensitivityNotCalculated";
         this.lblSensitivityNotCalculated.Size = new System.Drawing.Size(63, 13);
         this.lblSensitivityNotCalculated.StyleController = this.layoutControl1;
         this.lblSensitivityNotCalculated.TabIndex = 6;
         this.lblSensitivityNotCalculated.Text = "labelControl1";
         // 
         // cbPKParameter
         // 
         this.cbPKParameter.Location = new System.Drawing.Point(175, 36);
         this.cbPKParameter.Name = "cbPKParameter";
         this.cbPKParameter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbPKParameter.Size = new System.Drawing.Size(600, 20);
         this.cbPKParameter.StyleController = this.layoutControl1;
         this.cbPKParameter.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.labelLayoutControlItem,
            this.pKParameterSelectionLayoutItem,
            this.chartLayoutControlItem,
            this.outputPathSelectionLayoutItem});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Size = new System.Drawing.Size(787, 437);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // labelLayoutControlItem
         // 
         this.labelLayoutControlItem.Control = this.lblSensitivityNotCalculated;
         this.labelLayoutControlItem.Location = new System.Drawing.Point(0, 400);
         this.labelLayoutControlItem.Name = "labelLayoutControlItem";
         this.labelLayoutControlItem.Size = new System.Drawing.Size(767, 17);
         this.labelLayoutControlItem.TextSize = new System.Drawing.Size(160, 13);
         // 
         // outputPathSelectionLayoutItem
         // 
         this.outputPathSelectionLayoutItem.Control = this.cbOutputPathSelection;
         this.outputPathSelectionLayoutItem.Location = new System.Drawing.Point(0, 0);
         this.outputPathSelectionLayoutItem.Name = "outputPathSelectionLayoutItem";
         this.outputPathSelectionLayoutItem.Size = new System.Drawing.Size(767, 24);
         this.outputPathSelectionLayoutItem.TextSize = new System.Drawing.Size(160, 13);
         // 
         // pKParameterSelectionLayoutItem
         // 
         this.pKParameterSelectionLayoutItem.Control = this.cbPKParameter;
         this.pKParameterSelectionLayoutItem.Location = new System.Drawing.Point(0, 24);
         this.pKParameterSelectionLayoutItem.Name = "pKParameterSelectionLayoutItem";
         this.pKParameterSelectionLayoutItem.Size = new System.Drawing.Size(767, 24);
         this.pKParameterSelectionLayoutItem.TextSize = new System.Drawing.Size(160, 13);
         // 
         // chartLayoutControlItem
         // 
         this.chartLayoutControlItem.Control = this.chartControl;
         this.chartLayoutControlItem.Location = new System.Drawing.Point(0, 48);
         this.chartLayoutControlItem.Name = "chartLayoutControlItem";
         this.chartLayoutControlItem.Size = new System.Drawing.Size(767, 352);
         this.chartLayoutControlItem.TextSize = new System.Drawing.Size(160, 13);
         // 
         // SensitivityAnalysisPKParameterAnalysisView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "SensitivityAnalysisPKParameterAnalysisView";
         this.Size = new System.Drawing.Size(787, 437);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.chartControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbOutputPathSelection.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbPKParameter.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.labelLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.outputPathSelectionLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pKParameterSelectionLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartLayoutControlItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.ComboBoxEdit cbPKParameter;
      private DevExpress.XtraLayout.LayoutControlItem pKParameterSelectionLayoutItem;
      private DevExpress.XtraEditors.LabelControl lblSensitivityNotCalculated;
      private DevExpress.XtraLayout.LayoutControlItem labelLayoutControlItem;
      private DevExpress.XtraEditors.ComboBoxEdit cbOutputPathSelection;
      private DevExpress.XtraLayout.LayoutControlItem outputPathSelectionLayoutItem;
      private UxChartControl chartControl;
      private DevExpress.XtraLayout.LayoutControlItem chartLayoutControlItem;
   }
}
