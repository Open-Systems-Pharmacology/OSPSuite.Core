
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Charts
{
  partial class ChartDisplayView 
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
         this.components = new System.ComponentModel.Container();
         DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle2 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle3 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle4 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle5 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle6 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle7 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle8 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle9 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle10 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle11 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle12 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle13 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle14 = new DevExpress.XtraCharts.ChartTitle();
         this._toolTipController = new DevExpress.Utils.ToolTipController(this.components);
         this._barManager = new DevExpress.XtraBars.BarManager(this.components);
         this._toolBar = new DevExpress.XtraBars.Bar();
         this._menuBar = new DevExpress.XtraBars.Bar();
         this._StatusBar = new DevExpress.XtraBars.Bar();
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         this._chartControl = new UxChartControl(useDefaultPopupMechanism: false);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._barManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._chartControl)).BeginInit();
         this._chartControl.SuspendLayout();
         this.SuspendLayout();
         // 
         // _barManager
         // 
         this._barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this._toolBar,
            this._menuBar,
            this._StatusBar});
         this._barManager.DockControls.Add(this.barDockControlTop);
         this._barManager.DockControls.Add(this.barDockControlBottom);
         this._barManager.DockControls.Add(this.barDockControlLeft);
         this._barManager.DockControls.Add(this.barDockControlRight);
         this._barManager.Form = this;
         this._barManager.MaxItemId = 9;
         // 
         // _toolBar
         // 
         this._toolBar.BarName = "Tools";
         this._toolBar.DockCol = 0;
         this._toolBar.DockRow = 1;
         this._toolBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
         this._toolBar.Text = "Tools";
         this._toolBar.Visible = false;
         // 
         // _menuBar
         // 
         this._menuBar.BarName = "Main menu";
         this._menuBar.DockCol = 0;
         this._menuBar.DockRow = 0;
         this._menuBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
         this._menuBar.FloatLocation = new System.Drawing.Point(132, 277);
         this._menuBar.OptionsBar.MultiLine = true;
         this._menuBar.OptionsBar.UseWholeRow = true;
         this._menuBar.Text = "Main menu";
         this._menuBar.Visible = false;
         // 
         // _StatusBar
         // 
         this._StatusBar.BarName = "Status bar";
         this._StatusBar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
         this._StatusBar.DockCol = 0;
         this._StatusBar.DockRow = 0;
         this._StatusBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
         this._StatusBar.OptionsBar.AllowQuickCustomization = false;
         this._StatusBar.OptionsBar.DrawDragBorder = false;
         this._StatusBar.OptionsBar.MultiLine = true;
         this._StatusBar.OptionsBar.UseWholeRow = true;
         this._StatusBar.Text = "Status bar";
         this._StatusBar.Visible = false;
         // 
         // barDockControlTop
         // 
         this.barDockControlTop.CausesValidation = false;
         this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop.Size = new System.Drawing.Size(389, 58);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 292);
         this.barDockControlBottom.Size = new System.Drawing.Size(389, 29);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 58);
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 234);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(389, 58);
         this.barDockControlRight.Size = new System.Drawing.Size(0, 234);
         // 
         // chartControl
         // 
         this._chartControl.BackColor = System.Drawing.Color.Transparent;
         this._chartControl.Controls.Add(this.barDockControlLeft);
         this._chartControl.Controls.Add(this.barDockControlRight);
         this._chartControl.Controls.Add(this.barDockControlBottom);
         this._chartControl.Controls.Add(this.barDockControlTop);
         this._chartControl.Description = "";
         this._chartControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this._chartControl.EmptyChartText.Text = "";
         this._chartControl.Location = new System.Drawing.Point(0, 0);
         this._chartControl.Name = "_chartControl";
         this._chartControl.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
         this._chartControl.Size = new System.Drawing.Size(389, 321);
         this._chartControl.SmallChartText.Text = "Increase the chart\'s size,\r\nto view its layout.\r\n    ";
         this._chartControl.TabIndex = 4;
         this._chartControl.Title = "";
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
         chartTitle9.Font = new System.Drawing.Font("Arial", 16F);
         chartTitle9.Text = "";
         chartTitle9.WordWrap = true;
         chartTitle10.Alignment = System.Drawing.StringAlignment.Near;
         chartTitle10.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
         chartTitle10.Font = new System.Drawing.Font("Arial", 12F);
         chartTitle10.Text = "";
         chartTitle10.WordWrap = true;
         chartTitle11.Font = new System.Drawing.Font("Arial", 16F);
         chartTitle11.Text = "";
         chartTitle11.WordWrap = true;
         chartTitle12.Alignment = System.Drawing.StringAlignment.Near;
         chartTitle12.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
         chartTitle12.Font = new System.Drawing.Font("Arial", 12F);
         chartTitle12.Text = "";
         chartTitle12.WordWrap = true;
         chartTitle13.Font = new System.Drawing.Font("Arial", 16F);
         chartTitle13.Text = "";
         chartTitle13.WordWrap = true;
         chartTitle14.Alignment = System.Drawing.StringAlignment.Near;
         chartTitle14.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
         chartTitle14.Font = new System.Drawing.Font("Arial", 12F);
         chartTitle14.Text = "";
         chartTitle14.WordWrap = true;
         this._chartControl.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1,
            chartTitle2,
            chartTitle3,
            chartTitle4,
            chartTitle5,
            chartTitle6,
            chartTitle7,
            chartTitle8,
            chartTitle9,
            chartTitle10,
            chartTitle11,
            chartTitle12,
            chartTitle13,
            chartTitle14});
         // 
         // ChartDisplayView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.Controls.Add(this._chartControl);
         this.Name = "ChartDisplayView";
         this.Size = new System.Drawing.Size(389, 321);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._barManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._chartControl)).EndInit();
         this._chartControl.ResumeLayout(false);
         this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.Utils.ToolTipController _toolTipController;
    private DevExpress.XtraBars.BarManager _barManager;
    private DevExpress.XtraBars.Bar _toolBar;
    private DevExpress.XtraBars.Bar _menuBar;
    private DevExpress.XtraBars.Bar _StatusBar;
    private DevExpress.XtraBars.BarDockControl barDockControlTop;
    private DevExpress.XtraBars.BarDockControl barDockControlBottom;
    private DevExpress.XtraBars.BarDockControl barDockControlLeft;
    private DevExpress.XtraBars.BarDockControl barDockControlRight;
    private UxChartControl _chartControl;
  }
}
