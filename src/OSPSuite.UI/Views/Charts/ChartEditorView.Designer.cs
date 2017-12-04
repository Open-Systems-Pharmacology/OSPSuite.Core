namespace OSPSuite.UI.Views.Charts
{
   public partial class ChartEditorView
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
         this._barManager = new DevExpress.XtraBars.BarManager(this.components);
         this._barMenu = new DevExpress.XtraBars.Bar();
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.panelChartSettings = new DevExpress.XtraEditors.PanelControl();
         this.panelChartExportSettings = new DevExpress.XtraEditors.PanelControl();
         this.panelDataBrowser = new DevExpress.XtraEditors.PanelControl();
         this.panelAxisOptions = new DevExpress.XtraEditors.PanelControl();
         this.panelCurveOptions = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.tabbedControlGroup1 = new DevExpress.XtraLayout.TabbedControlGroup();
         this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.splitterItem2 = new DevExpress.XtraLayout.SplitterItem();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._dockManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._barManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelChartSettings)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelChartExportSettings)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelDataBrowser)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelAxisOptions)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelCurveOptions)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // _dockManager
         // 
         this._dockManager.Form = this;
         this._dockManager.MenuManager = this._barManager;
         this._dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
         // 
         // _barManager
         // 
         this._barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this._barMenu});
         this._barManager.DockControls.Add(this.barDockControlTop);
         this._barManager.DockControls.Add(this.barDockControlBottom);
         this._barManager.DockControls.Add(this.barDockControlLeft);
         this._barManager.DockControls.Add(this.barDockControlRight);
         this._barManager.DockManager = this._dockManager;
         this._barManager.Form = this;
         this._barManager.MaxItemId = 3;
         // 
         // _barMenu
         // 
         this._barMenu.BarName = "Main menu";
         this._barMenu.DockCol = 0;
         this._barMenu.DockRow = 0;
         this._barMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
         this._barMenu.FloatLocation = new System.Drawing.Point(298, 139);
         this._barMenu.FloatSize = new System.Drawing.Size(46, 22);
         this._barMenu.OptionsBar.MultiLine = true;
         this._barMenu.OptionsBar.UseWholeRow = true;
         this._barMenu.Text = "Main menu";
         // 
         // barDockControlTop
         // 
         this.barDockControlTop.CausesValidation = false;
         this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop.Size = new System.Drawing.Size(558, 29);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 619);
         this.barDockControlBottom.Size = new System.Drawing.Size(558, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 590);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(558, 29);
         this.barDockControlRight.Size = new System.Drawing.Size(0, 590);
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.panelChartSettings);
         this.layoutControl.Controls.Add(this.panelChartExportSettings);
         this.layoutControl.Controls.Add(this.panelDataBrowser);
         this.layoutControl.Controls.Add(this.panelAxisOptions);
         this.layoutControl.Controls.Add(this.panelCurveOptions);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 29);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(907, 351, 328, 350);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(558, 590);
         this.layoutControl.TabIndex = 19;
         this.layoutControl.Text = "layoutControl1";
         // 
         // panelChartSettings
         // 
         this.panelChartSettings.Location = new System.Drawing.Point(14, 302);
         this.panelChartSettings.Name = "panelChartSettings";
         this.panelChartSettings.Size = new System.Drawing.Size(530, 274);
         this.panelChartSettings.TabIndex = 22;
         // 
         // panelChartExportSettings
         // 
         this.panelChartExportSettings.Location = new System.Drawing.Point(14, 302);
         this.panelChartExportSettings.Name = "panelChartExportSettings";
         this.panelChartExportSettings.Size = new System.Drawing.Size(530, 274);
         this.panelChartExportSettings.TabIndex = 23;
         // 
         // panelDataBrowser
         // 
         this.panelDataBrowser.Location = new System.Drawing.Point(2, 2);
         this.panelDataBrowser.Name = "panelDataBrowser";
         this.panelDataBrowser.Size = new System.Drawing.Size(554, 257);
         this.panelDataBrowser.TabIndex = 21;
         // 
         // panelAxisOptions
         // 
         this.panelAxisOptions.Location = new System.Drawing.Point(14, 459);
         this.panelAxisOptions.Name = "panelAxisOptions";
         this.panelAxisOptions.Size = new System.Drawing.Size(530, 117);
         this.panelAxisOptions.TabIndex = 20;
         // 
         // panelCurveOptions
         // 
         this.panelCurveOptions.Location = new System.Drawing.Point(14, 302);
         this.panelCurveOptions.Name = "panelCurveOptions";
         this.panelCurveOptions.Size = new System.Drawing.Size(530, 148);
         this.panelCurveOptions.TabIndex = 19;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "Root";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.tabbedControlGroup1,
            this.splitterItem1,
            this.layoutControlItem1});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(558, 590);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // tabbedControlGroup1
         // 
         this.tabbedControlGroup1.CustomizationFormText = "Options";
         this.tabbedControlGroup1.Location = new System.Drawing.Point(0, 266);
         this.tabbedControlGroup1.Name = "tabbedControlGroup1";
         this.tabbedControlGroup1.SelectedTabPage = this.layoutControlGroup3;
         this.tabbedControlGroup1.SelectedTabPageIndex = 0;
         this.tabbedControlGroup1.Size = new System.Drawing.Size(558, 324);
         this.tabbedControlGroup1.TabPages.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup3,
            this.layoutControlGroup2,
            this.layoutControlGroup4});
         this.tabbedControlGroup1.Text = "Options";
         // 
         // layoutControlGroup4
         // 
         this.layoutControlGroup4.CustomizationFormText = "Chart Export Options";
         this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem5});
         this.layoutControlGroup4.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup4.Name = "layoutControlGroup4";
         this.layoutControlGroup4.Size = new System.Drawing.Size(534, 278);
         this.layoutControlGroup4.Text = "Chart Export Options";
         // 
         // layoutControlItem5
         // 
         this.layoutControlItem5.Control = this.panelChartExportSettings;
         this.layoutControlItem5.CustomizationFormText = "layoutControlItem5";
         this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem5.MinSize = new System.Drawing.Size(5, 5);
         this.layoutControlItem5.Name = "layoutControlItem5";
         this.layoutControlItem5.Size = new System.Drawing.Size(534, 278);
         this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem5.TextVisible = false;
         // 
         // layoutControlGroup3
         // 
         this.layoutControlGroup3.CustomizationFormText = "Curves and Axis Options";
         this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.splitterItem2,
            this.layoutControlItem3,
            this.layoutControlItem4});
         this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup3.Name = "layoutControlGroup3";
         this.layoutControlGroup3.Size = new System.Drawing.Size(534, 278);
         this.layoutControlGroup3.Text = "Curves and Axis Options";
         // 
         // splitterItem2
         // 
         this.splitterItem2.AllowHotTrack = true;
         this.splitterItem2.CustomizationFormText = "splitterItem2";
         this.splitterItem2.Location = new System.Drawing.Point(0, 152);
         this.splitterItem2.Name = "splitterItem2";
         this.splitterItem2.Size = new System.Drawing.Size(534, 5);
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.panelCurveOptions;
         this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem3.MinSize = new System.Drawing.Size(5, 5);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(534, 152);
         this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.panelAxisOptions;
         this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
         this.layoutControlItem4.Location = new System.Drawing.Point(0, 157);
         this.layoutControlItem4.MinSize = new System.Drawing.Size(5, 5);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Size = new System.Drawing.Size(534, 121);
         this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem4.TextVisible = false;
         // 
         // layoutControlGroup2
         // 
         this.layoutControlGroup2.CustomizationFormText = "Chart Options";
         this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
         this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup2.Name = "layoutControlGroup2";
         this.layoutControlGroup2.Size = new System.Drawing.Size(534, 278);
         this.layoutControlGroup2.Text = "Chart Options";
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.panelChartSettings;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.MinSize = new System.Drawing.Size(5, 5);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(534, 278);
         this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.CustomizationFormText = "splitterItem1";
         this.splitterItem1.Location = new System.Drawing.Point(0, 261);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(558, 5);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.panelDataBrowser;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.MinSize = new System.Drawing.Size(5, 5);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(558, 261);
         this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // ChartEditorView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Controls.Add(this.barDockControlLeft);
         this.Controls.Add(this.barDockControlRight);
         this.Controls.Add(this.barDockControlBottom);
         this.Controls.Add(this.barDockControlTop);
         this.Name = "ChartEditorView";
         this.Size = new System.Drawing.Size(558, 619);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._dockManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._barManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelChartSettings)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelChartExportSettings)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelDataBrowser)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelAxisOptions)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelCurveOptions)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
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
      private DevExpress.XtraBars.Bar _barMenu;
      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraLayout.SplitterItem splitterItem2;
      private DevExpress.XtraLayout.TabbedControlGroup tabbedControlGroup1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
      private DevExpress.XtraEditors.PanelControl panelAxisOptions;
      private DevExpress.XtraEditors.PanelControl panelCurveOptions;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
      private DevExpress.XtraEditors.PanelControl panelDataBrowser;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraEditors.PanelControl panelChartSettings;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraEditors.PanelControl panelChartExportSettings;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup4;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
   }
}
