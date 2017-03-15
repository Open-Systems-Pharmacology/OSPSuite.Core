namespace PkModelCore.DataChart.Tests
{
   partial class ChartControlForm
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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
         DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
         DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel1 = new DevExpress.XtraCharts.PointSeriesLabel();
         DevExpress.XtraCharts.PointOptions pointOptions1 = new DevExpress.XtraCharts.PointOptions();
         DevExpress.XtraCharts.SeriesPoint seriesPoint1 = new DevExpress.XtraCharts.SeriesPoint(0D, new object[] {
            ((object)(0D))});
         DevExpress.XtraCharts.SeriesPoint seriesPoint2 = new DevExpress.XtraCharts.SeriesPoint(1E-05D, new object[] {
            ((object)(1D))});
         DevExpress.XtraCharts.SeriesPoint seriesPoint3 = new DevExpress.XtraCharts.SeriesPoint(2E-05D, new object[] {
            ((object)(1D))});
         DevExpress.XtraCharts.SeriesPoint seriesPoint4 = new DevExpress.XtraCharts.SeriesPoint(1D, new object[] {
            ((object)(1D))});
         DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
         DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
         DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel2 = new DevExpress.XtraCharts.PointSeriesLabel();
         DevExpress.XtraCharts.PointOptions pointOptions2 = new DevExpress.XtraCharts.PointOptions();
         DevExpress.XtraCharts.LineSeriesView lineSeriesView2 = new DevExpress.XtraCharts.LineSeriesView();
         DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel3 = new DevExpress.XtraCharts.PointSeriesLabel();
         DevExpress.XtraCharts.PointOptions pointOptions3 = new DevExpress.XtraCharts.PointOptions();
         DevExpress.XtraCharts.LineSeriesView lineSeriesView3 = new DevExpress.XtraCharts.LineSeriesView();
         this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
         ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).BeginInit();
         this.SuspendLayout();
         // 
         // chartControl1
         // 
         this.chartControl1.CrosshairOptions.ArgumentLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(57)))), ((int)(((byte)(205)))));
         this.chartControl1.CrosshairOptions.ValueLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(57)))), ((int)(((byte)(205)))));
         xyDiagram1.AxisX.NumericOptions.Format = DevExpress.XtraCharts.NumericFormat.General;
         xyDiagram1.AxisX.Range.ScrollingRange.SideMarginsEnabled = true;
         xyDiagram1.AxisX.Range.SideMarginsEnabled = true;
         xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
         xyDiagram1.AxisY.NumericOptions.Format = DevExpress.XtraCharts.NumericFormat.General;
         xyDiagram1.AxisY.Range.ScrollingRange.SideMarginsEnabled = true;
         xyDiagram1.AxisY.Range.SideMarginsEnabled = true;
         xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
         this.chartControl1.Diagram = xyDiagram1;
         this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.chartControl1.Location = new System.Drawing.Point(0, 0);
         this.chartControl1.Name = "chartControl1";
         pointSeriesLabel1.LineVisible = true;
         pointOptions1.ArgumentNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.General;
         pointOptions1.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.General;
         pointSeriesLabel1.PointOptions = pointOptions1;
         series1.Label = pointSeriesLabel1;
         series1.Name = "Series 1";
         series1.Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] {
            seriesPoint1,
            seriesPoint2,
            seriesPoint3,
            seriesPoint4});
         lineSeriesView1.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Dash;
         lineSeriesView1.LineStyle.Thickness = 1;
         series1.View = lineSeriesView1;
         pointSeriesLabel2.LineVisible = true;
         pointOptions2.ArgumentNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.General;
         pointOptions2.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.General;
         pointSeriesLabel2.PointOptions = pointOptions2;
         series2.Label = pointSeriesLabel2;
         series2.Name = "Series 2";
         series2.View = lineSeriesView2;
         this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2};
         pointSeriesLabel3.LineVisible = true;
         pointOptions3.ArgumentNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.General;
         pointOptions3.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.General;
         pointSeriesLabel3.PointOptions = pointOptions3;
         this.chartControl1.SeriesTemplate.Label = pointSeriesLabel3;
         this.chartControl1.SeriesTemplate.View = lineSeriesView3;
         this.chartControl1.Size = new System.Drawing.Size(284, 262);
         this.chartControl1.TabIndex = 0;
         // 
         // ChartControlForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(284, 262);
         this.Controls.Add(this.chartControl1);
         this.Name = "ChartControlForm";
         this.Text = "ChartControlForm";
         ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraCharts.ChartControl chartControl1;
   }
}