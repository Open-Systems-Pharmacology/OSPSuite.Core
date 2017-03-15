using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Charts
{
   partial class ChartTemplateDetailsView
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
         this.layoutControl1 = new UxLayoutControl();
         this.panelChartExportSettings = new DevExpress.XtraEditors.PanelControl();
         this.panelCurves = new DevExpress.XtraEditors.PanelControl();
         this.panelAxes = new DevExpress.XtraEditors.PanelControl();
         this.panelChartSettings = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.tabbedControlGroup1 = new DevExpress.XtraLayout.TabbedControlGroup();
         this.layoutGroupChartExportSettings = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupCurvesAndAxis = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemAxes = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemCurves = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.layoutGroupChartSettings = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelChartExportSettings)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelCurves)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelAxes)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelChartSettings)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupChartExportSettings)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupCurvesAndAxis)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAxes)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCurves)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupChartSettings)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.panelChartExportSettings);
         this.layoutControl1.Controls.Add(this.panelCurves);
         this.layoutControl1.Controls.Add(this.panelAxes);
         this.layoutControl1.Controls.Add(this.panelChartSettings);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(844, 342, 402, 415);
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(657, 531);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // panelChartExportSettings
         // 
         this.panelChartExportSettings.Location = new System.Drawing.Point(14, 36);
         this.panelChartExportSettings.Name = "panelChartExportSettings";
         this.panelChartExportSettings.Size = new System.Drawing.Size(629, 481);
         this.panelChartExportSettings.TabIndex = 7;
         // 
         // panelCurves
         // 
         this.panelCurves.Location = new System.Drawing.Point(14, 36);
         this.panelCurves.Name = "panelCurves";
         this.panelCurves.Size = new System.Drawing.Size(629, 252);
         this.panelCurves.TabIndex = 6;
         // 
         // panelAxes
         // 
         this.panelAxes.Location = new System.Drawing.Point(14, 297);
         this.panelAxes.Name = "panelAxes";
         this.panelAxes.Size = new System.Drawing.Size(629, 220);
         this.panelAxes.TabIndex = 5;
         // 
         // panelChartSettings
         // 
         this.panelChartSettings.Location = new System.Drawing.Point(14, 36);
         this.panelChartSettings.Name = "panelChartSettings";
         this.panelChartSettings.Size = new System.Drawing.Size(629, 481);
         this.panelChartSettings.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.tabbedControlGroup1});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(657, 531);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // tabbedControlGroup1
         // 
         this.tabbedControlGroup1.CustomizationFormText = "tabbedControlGroup1";
         this.tabbedControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.tabbedControlGroup1.Name = "tabbedControlGroup1";
         this.tabbedControlGroup1.SelectedTabPage = this.layoutGroupCurvesAndAxis;
         this.tabbedControlGroup1.SelectedTabPageIndex = 0;
         this.tabbedControlGroup1.Size = new System.Drawing.Size(657, 531);
         this.tabbedControlGroup1.TabPages.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutGroupCurvesAndAxis,
            this.layoutGroupChartSettings,
            this.layoutGroupChartExportSettings});
         // 
         // layoutGroupChartExportSettings
         // 
         this.layoutGroupChartExportSettings.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
         this.layoutGroupChartExportSettings.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupChartExportSettings.Name = "layoutGroupChartExportSettings";
         this.layoutGroupChartExportSettings.Size = new System.Drawing.Size(633, 485);
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.panelChartExportSettings;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(633, 485);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutGroupCurvesAndAxis
         // 
         this.layoutGroupCurvesAndAxis.CustomizationFormText = "layoutControlGroup3";
         this.layoutGroupCurvesAndAxis.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemAxes,
            this.layoutItemCurves,
            this.splitterItem1});
         this.layoutGroupCurvesAndAxis.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupCurvesAndAxis.Name = "layoutGroupCurvesAndAxis";
         this.layoutGroupCurvesAndAxis.Size = new System.Drawing.Size(633, 485);
         // 
         // layoutItemAxes
         // 
         this.layoutItemAxes.Control = this.panelAxes;
         this.layoutItemAxes.CustomizationFormText = "layoutItemAxes";
         this.layoutItemAxes.Location = new System.Drawing.Point(0, 261);
         this.layoutItemAxes.Name = "layoutItemAxes";
         this.layoutItemAxes.Size = new System.Drawing.Size(633, 224);
         this.layoutItemAxes.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemAxes.TextVisible = false;
         // 
         // layoutItemCurves
         // 
         this.layoutItemCurves.Control = this.panelCurves;
         this.layoutItemCurves.CustomizationFormText = "layoutItemCurves";
         this.layoutItemCurves.Location = new System.Drawing.Point(0, 0);
         this.layoutItemCurves.Name = "layoutItemCurves";
         this.layoutItemCurves.Size = new System.Drawing.Size(633, 256);
         this.layoutItemCurves.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemCurves.TextVisible = false;
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.CustomizationFormText = "splitterItem1";
         this.splitterItem1.Location = new System.Drawing.Point(0, 256);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(633, 5);
         // 
         // layoutGroupChartSettings
         // 
         this.layoutGroupChartSettings.CustomizationFormText = "layoutControlGroup2";
         this.layoutGroupChartSettings.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
         this.layoutGroupChartSettings.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupChartSettings.Name = "layoutGroupChartSettings";
         this.layoutGroupChartSettings.Size = new System.Drawing.Size(633, 485);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.panelChartSettings;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(633, 485);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // ChartTemplateDetailsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Margin = new System.Windows.Forms.Padding(0);
         this.Name = "ChartTemplateDetailsView";
         this.Size = new System.Drawing.Size(657, 531);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelChartExportSettings)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelCurves)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelAxes)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelChartSettings)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupChartExportSettings)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupCurvesAndAxis)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAxes)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCurves)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupChartSettings)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.PanelControl panelCurves;
      private DevExpress.XtraEditors.PanelControl panelAxes;
      private DevExpress.XtraEditors.PanelControl panelChartSettings;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.TabbedControlGroup tabbedControlGroup1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupCurvesAndAxis;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemAxes;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemCurves;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupChartSettings;
      private DevExpress.XtraEditors.PanelControl panelChartExportSettings;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupChartExportSettings;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
   }
}
