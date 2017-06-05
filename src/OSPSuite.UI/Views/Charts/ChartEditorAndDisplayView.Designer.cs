namespace OSPSuite.UI.Views.Charts
{
   public partial class ChartEditorAndDisplayView
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
         this._dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
         this.hideContainerRight = new DevExpress.XtraBars.Docking.AutoHideContainer();
         this._pnlChartEditor = new DevExpress.XtraBars.Docking.DockPanel();
         this._contChartEditor = new DevExpress.XtraBars.Docking.ControlContainer();
         this._barManager = new DevExpress.XtraBars.BarManager(this.components);
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         this._pnlChartDisplay = new DevExpress.XtraEditors.PanelControl();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._dockManager)).BeginInit();
         this.hideContainerRight.SuspendLayout();
         this._pnlChartEditor.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._barManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._pnlChartDisplay)).BeginInit();
         this.SuspendLayout();
         // 
         // _dockManager
         // 
         this._dockManager.AutoHideContainers.AddRange(new DevExpress.XtraBars.Docking.AutoHideContainer[] {
            this.hideContainerRight});
         this._dockManager.Form = this;
         this._dockManager.MenuManager = this._barManager;
         this._dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
         // 
         // hideContainerRight
         // 
         this.hideContainerRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
         this.hideContainerRight.Controls.Add(this._pnlChartEditor);
         this.hideContainerRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.hideContainerRight.Location = new System.Drawing.Point(882, 0);
         this.hideContainerRight.Name = "hideContainerRight";
         this.hideContainerRight.Size = new System.Drawing.Size(19, 619);
         // 
         // _pnlChartEditor
         // 
         this._pnlChartEditor.Controls.Add(this._contChartEditor);
         this._pnlChartEditor.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
         this._pnlChartEditor.ID = new System.Guid("bc6dd20e-7f30-4e3b-b994-e34b2e328b35");
         this._pnlChartEditor.Location = new System.Drawing.Point(0, 0);
         this._pnlChartEditor.Name = "_pnlChartEditor";
         this._pnlChartEditor.Options.AllowFloating = false;
         this._pnlChartEditor.Options.ShowCloseButton = false;
         this._pnlChartEditor.OriginalSize = new System.Drawing.Size(411, 200);
         this._pnlChartEditor.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Right;
         this._pnlChartEditor.SavedIndex = 0;
         this._pnlChartEditor.Size = new System.Drawing.Size(411, 619);
         this._pnlChartEditor.Text = "Chart Editor";
         this._pnlChartEditor.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
         // 
         // _contChartEditor
         // 
         this._contChartEditor.Location = new System.Drawing.Point(4, 23);
         this._contChartEditor.Name = "_contChartEditor";
         this._contChartEditor.Size = new System.Drawing.Size(403, 592);
         this._contChartEditor.TabIndex = 0;
         // 
         // _barManager
         // 
         this._barManager.DockControls.Add(this.barDockControlTop);
         this._barManager.DockControls.Add(this.barDockControlBottom);
         this._barManager.DockControls.Add(this.barDockControlLeft);
         this._barManager.DockControls.Add(this.barDockControlRight);
         this._barManager.DockManager = this._dockManager;
         this._barManager.Form = this;
         this._barManager.MaxItemId = 3;
         // 
         // barDockControlTop
         // 
         this.barDockControlTop.CausesValidation = false;
         this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop.Size = new System.Drawing.Size(901, 0);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 619);
         this.barDockControlBottom.Size = new System.Drawing.Size(901, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 619);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(901, 0);
         this.barDockControlRight.Size = new System.Drawing.Size(0, 619);
         // 
         // _pnlChartDisplay
         // 
         this._pnlChartDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
         this._pnlChartDisplay.Location = new System.Drawing.Point(0, 0);
         this._pnlChartDisplay.Name = "_pnlChartDisplay";
         this._pnlChartDisplay.Size = new System.Drawing.Size(882, 619);
         this._pnlChartDisplay.TabIndex = 6;
         // 
         // ChartEditorAndDisplayControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this._pnlChartDisplay);
         this.Controls.Add(this.hideContainerRight);
         this.Controls.Add(this.barDockControlLeft);
         this.Controls.Add(this.barDockControlRight);
         this.Controls.Add(this.barDockControlBottom);
         this.Controls.Add(this.barDockControlTop);
         this.Name = "ChartEditorAndDisplayView";
         this.Size = new System.Drawing.Size(901, 619);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._dockManager)).EndInit();
         this.hideContainerRight.ResumeLayout(false);
         this._pnlChartEditor.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._barManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._pnlChartDisplay)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

    }

    #endregion

    private DevExpress.XtraBars.Docking.DockManager _dockManager;
    private DevExpress.XtraBars.BarDockControl barDockControlLeft;
    private DevExpress.XtraBars.BarDockControl barDockControlRight;
    private DevExpress.XtraBars.BarDockControl barDockControlBottom;
    private DevExpress.XtraBars.BarDockControl barDockControlTop;
    private DevExpress.XtraBars.BarManager _barManager;
    private DevExpress.XtraBars.Docking.DockPanel _pnlChartEditor;
    private DevExpress.XtraBars.Docking.ControlContainer _contChartEditor;
    private DevExpress.XtraBars.Docking.AutoHideContainer hideContainerRight;
    private DevExpress.XtraEditors.PanelControl _pnlChartDisplay;
  }
}
