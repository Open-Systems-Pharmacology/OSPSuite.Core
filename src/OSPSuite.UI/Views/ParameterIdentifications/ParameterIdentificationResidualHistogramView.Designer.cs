using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   partial class ParameterIdentificationResidualHistogramView
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
         DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle2 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle3 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle4 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle5 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle6 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle7 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle8 = new DevExpress.XtraCharts.ChartTitle();
         this.chart = new UxHistogramControl();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
         this.SuspendLayout();
         // 
         // chart
         // 
         this.chart.Description = "";
         this.chart.DiagramBackColor = System.Drawing.Color.Empty;
         this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
         this.chart.Location = new System.Drawing.Point(0, 0);
         this.chart.Name = "chart";
         this.chart.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
         this.chart.Size = new System.Drawing.Size(770, 652);
         this.chart.TabIndex = 5;
         this.chart.Title = "";
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
         chartTitle7.Font = new System.Drawing.Font("Arial", 16F);
         chartTitle7.Text = "";
         chartTitle7.WordWrap = true;
         chartTitle8.Alignment = System.Drawing.StringAlignment.Near;
         chartTitle8.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
         chartTitle8.Font = new System.Drawing.Font("Arial", 12F);
         chartTitle8.Text = "";
         chartTitle8.WordWrap = true;
         this.chart.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1,
            chartTitle2,
            chartTitle3,
            chartTitle4,
            chartTitle5,
            chartTitle6,
            chartTitle7,
            chartTitle8});
         // 
         // ParameterIdentificationResidualHistogramView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.chart);
         this.Name = "ParameterIdentificationResidualHistogramView";
         this.Size = new System.Drawing.Size(770, 652);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private Controls.UxHistogramControl chart;
   }
}
