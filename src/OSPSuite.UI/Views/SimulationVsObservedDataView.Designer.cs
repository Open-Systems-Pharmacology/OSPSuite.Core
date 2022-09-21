namespace OSPSuite.UI.Views
{
   partial class SimulationVsObservedDataView
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
         this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
         this.totalErrorTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.panelChart = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.panelChartLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.totalErrorLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
         this.layoutControl2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.totalErrorTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelChart)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelChartLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.totalErrorLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         this.SuspendLayout();
         // 
         // layoutControl2
         // 
         this.layoutControl2.Controls.Add(this.panelChart);
         this.layoutControl2.Controls.Add(this.totalErrorTextEdit);
         this.layoutControl2.Location = new System.Drawing.Point(12, 12);
         this.layoutControl2.Name = "layoutControl2";
         this.layoutControl2.Root = this.layoutControlGroup1;
         this.layoutControl2.Size = new System.Drawing.Size(2235, 1757);
         this.layoutControl2.TabIndex = 4;
         this.layoutControl2.Text = "layoutControl2";
         // 
         // totalErrorTextEdit
         // 
         this.totalErrorTextEdit.Location = new System.Drawing.Point(358, 12);
         this.totalErrorTextEdit.Name = "totalErrorTextEdit";
         this.totalErrorTextEdit.Size = new System.Drawing.Size(1865, 48);
         this.totalErrorTextEdit.StyleController = this.layoutControl2;
         this.totalErrorTextEdit.TabIndex = 4;
         // 
         // panelChart
         // 
         this.panelChart.Location = new System.Drawing.Point(12, 64);
         this.panelChart.Name = "panelChart";
         this.panelChart.Size = new System.Drawing.Size(2211, 1681);
         this.panelChart.TabIndex = 5;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.totalErrorLayoutControlItem,
            this.panelChartLayoutControlItem});
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(2235, 1757);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // panelChartLayoutControlItem
         // 
         this.panelChartLayoutControlItem.Control = this.panelChart;
         this.panelChartLayoutControlItem.Location = new System.Drawing.Point(0, 52);
         this.panelChartLayoutControlItem.Name = "panelChartLayoutControlItem";
         this.panelChartLayoutControlItem.Size = new System.Drawing.Size(2215, 1685);
         this.panelChartLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.panelChartLayoutControlItem.TextVisible = false;
         // 
         // totalErrorLayoutControlItem
         // 
         this.totalErrorLayoutControlItem.Control = this.totalErrorTextEdit;
         this.totalErrorLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.totalErrorLayoutControlItem.Name = "totalErrorLayoutControlItem";
         this.totalErrorLayoutControlItem.Size = new System.Drawing.Size(2215, 52);
         this.totalErrorLayoutControlItem.TextSize = new System.Drawing.Size(334, 33);
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(2259, 1781);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.layoutControl2;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(2239, 1761);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.layoutControl2);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.Root;
         this.layoutControl1.Size = new System.Drawing.Size(2259, 1781);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // SimulationVsObservedDataView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "SimulationVsObservedDataView";
         this.Size = new System.Drawing.Size(2259, 1781);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
         this.layoutControl2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.totalErrorTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelChart)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelChartLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.totalErrorLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl2;
      private DevExpress.XtraEditors.PanelControl panelChart;
      private DevExpress.XtraEditors.TextEdit totalErrorTextEdit;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem totalErrorLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem panelChartLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControl layoutControl1;
   }
}
