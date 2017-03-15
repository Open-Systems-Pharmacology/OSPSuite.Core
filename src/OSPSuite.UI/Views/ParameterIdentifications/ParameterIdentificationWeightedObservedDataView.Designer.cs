using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   partial class ParameterIdentificationWeightedObservedDataView
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
         this.layoutControl = new UxLayoutControl();
         this.panelChart = new DevExpress.XtraEditors.PanelControl();
         this.panelData = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutPanelChart = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutPanelData = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelChart)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelData)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutPanelChart)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutPanelData)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.panelChart);
         this.layoutControl.Controls.Add(this.panelData);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(709, 190, 559, 469);
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(881, 606);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // panelChart
         // 
         this.panelChart.Location = new System.Drawing.Point(12, 267);
         this.panelChart.Name = "panelChart";
         this.panelChart.Size = new System.Drawing.Size(857, 327);
         this.panelChart.TabIndex = 5;
         // 
         // panelData
         // 
         this.panelData.Location = new System.Drawing.Point(12, 12);
         this.panelData.Name = "panelData";
         this.panelData.Size = new System.Drawing.Size(857, 246);
         this.panelData.TabIndex = 4;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutPanelChart,
            this.layoutPanelData,
            this.splitterItem1});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "Root";
         this.layoutControlGroup.Size = new System.Drawing.Size(881, 606);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutPanelChart
         // 
         this.layoutPanelChart.Control = this.panelData;
         this.layoutPanelChart.Location = new System.Drawing.Point(0, 0);
         this.layoutPanelChart.Name = "layoutPanelChart";
         this.layoutPanelChart.Size = new System.Drawing.Size(861, 250);
         this.layoutPanelChart.TextSize = new System.Drawing.Size(0, 0);
         this.layoutPanelChart.TextVisible = false;
         // 
         // layoutPanelData
         // 
         this.layoutPanelData.Control = this.panelChart;
         this.layoutPanelData.Location = new System.Drawing.Point(0, 255);
         this.layoutPanelData.Name = "layoutPanelData";
         this.layoutPanelData.Size = new System.Drawing.Size(861, 331);
         this.layoutPanelData.TextSize = new System.Drawing.Size(0, 0);
         this.layoutPanelData.TextVisible = false;
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(0, 250);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(861, 5);
         // 
         // ParameterIdentificationWeightedObservedDataView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ParameterIdentificationWeightedObservedDataView";
         this.Size = new System.Drawing.Size(881, 606);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelChart)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelData)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutPanelChart)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutPanelData)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControlItem layoutPanelChart;
      private DevExpress.XtraLayout.LayoutControlItem layoutPanelData;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraEditors.PanelControl panelData;
      private DevExpress.XtraEditors.PanelControl panelChart;
      private UxLayoutControl layoutControl;
   }
}
