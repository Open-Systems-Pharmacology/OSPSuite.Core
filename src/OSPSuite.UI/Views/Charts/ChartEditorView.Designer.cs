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
         this.btnApplyChartUpdates = new OSPSuite.UI.Controls.UxSimpleButton();
         this.chkAutoUpdateCharts = new OSPSuite.UI.Controls.UxCheckEdit();
         this.chkLinkSimulationObserved = new OSPSuite.UI.Controls.UxCheckEdit();
         this.chkUsedIn = new OSPSuite.UI.Controls.UxCheckEdit();
         this.panelCurveColorGrouping = new DevExpress.XtraEditors.PanelControl();
         this.panelChartSettings = new DevExpress.XtraEditors.PanelControl();
         this.panelChartExportSettings = new DevExpress.XtraEditors.PanelControl();
         this.panelDataBrowser = new DevExpress.XtraEditors.PanelControl();
         this.panelAxisOptions = new DevExpress.XtraEditors.PanelControl();
         this.panelCurveOptions = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.tabbedControlGroup1 = new DevExpress.XtraLayout.TabbedControlGroup();
         this.layoutCurveAndChartSettings = new DevExpress.XtraLayout.LayoutControlGroup();
         this.splitterItem2 = new DevExpress.XtraLayout.SplitterItem();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutChartOptions = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutChartExportOptions = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutColorGrouping = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemUsedIn = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemLink = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemAutoUpdateCharts = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemApplyButton = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._dockManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._barManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.chkAutoUpdateCharts.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkLinkSimulationObserved.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkUsedIn.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelCurveColorGrouping)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelChartSettings)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelChartExportSettings)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelDataBrowser)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelAxisOptions)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelCurveOptions)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutCurveAndChartSettings)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutChartOptions)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutChartExportOptions)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutColorGrouping)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemUsedIn)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemLink)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAutoUpdateCharts)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemApplyButton)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
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
         this.barDockControlTop.Manager = this._barManager;
         this.barDockControlTop.Size = new System.Drawing.Size(558, 20);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 619);
         this.barDockControlBottom.Manager = this._barManager;
         this.barDockControlBottom.Size = new System.Drawing.Size(558, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 20);
         this.barDockControlLeft.Manager = this._barManager;
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 599);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(558, 20);
         this.barDockControlRight.Manager = this._barManager;
         this.barDockControlRight.Size = new System.Drawing.Size(0, 599);
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.btnApplyChartUpdates);
         this.layoutControl.Controls.Add(this.chkAutoUpdateCharts);
         this.layoutControl.Controls.Add(this.chkLinkSimulationObserved);
         this.layoutControl.Controls.Add(this.chkUsedIn);
         this.layoutControl.Controls.Add(this.panelCurveColorGrouping);
         this.layoutControl.Controls.Add(this.panelChartSettings);
         this.layoutControl.Controls.Add(this.panelChartExportSettings);
         this.layoutControl.Controls.Add(this.panelDataBrowser);
         this.layoutControl.Controls.Add(this.panelAxisOptions);
         this.layoutControl.Controls.Add(this.panelCurveOptions);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 20);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1206, 243, 1216, 971);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(558, 599);
         this.layoutControl.TabIndex = 19;
         this.layoutControl.Text = "layoutControl1";
         // 
         // btnApplyChartUpdates
         // 
         this.btnApplyChartUpdates.Location = new System.Drawing.Point(276, 26);
         this.btnApplyChartUpdates.Manager = null;
         this.btnApplyChartUpdates.Name = "btnApplyChartUpdates";
         this.btnApplyChartUpdates.Shortcut = System.Windows.Forms.Keys.None;
         this.btnApplyChartUpdates.Size = new System.Drawing.Size(270, 22);
         this.btnApplyChartUpdates.StyleController = this.layoutControl;
         this.btnApplyChartUpdates.TabIndex = 27;
         this.btnApplyChartUpdates.Text = "btnApplyChartUpdates";
         // 
         // chkAutoUpdateCharts
         // 
         this.chkAutoUpdateCharts.AllowClicksOutsideControlArea = false;
         this.chkAutoUpdateCharts.Location = new System.Drawing.Point(2, 26);
         this.chkAutoUpdateCharts.MenuManager = this._barManager;
         this.chkAutoUpdateCharts.Name = "chkAutoUpdateCharts";
         this.chkAutoUpdateCharts.Properties.AllowFocused = false;
         this.chkAutoUpdateCharts.Properties.Caption = "chkAutoUpdateCharts";
         this.chkAutoUpdateCharts.Size = new System.Drawing.Size(270, 20);
         this.chkAutoUpdateCharts.StyleController = this.layoutControl;
         this.chkAutoUpdateCharts.TabIndex = 26;
         // 
         // chkLinkSimulationObserved
         // 
         this.chkLinkSimulationObserved.AllowClicksOutsideControlArea = false;
         this.chkLinkSimulationObserved.Location = new System.Drawing.Point(276, 2);
         this.chkLinkSimulationObserved.MenuManager = this._barManager;
         this.chkLinkSimulationObserved.Name = "chkLinkSimulationObserved";
         this.chkLinkSimulationObserved.Properties.AllowFocused = false;
         this.chkLinkSimulationObserved.Properties.Caption = "chkLink";
         this.chkLinkSimulationObserved.Size = new System.Drawing.Size(270, 20);
         this.chkLinkSimulationObserved.StyleController = this.layoutControl;
         this.chkLinkSimulationObserved.TabIndex = 25;
         // 
         // chkUsedIn
         // 
         this.chkUsedIn.AllowClicksOutsideControlArea = false;
         this.chkUsedIn.Location = new System.Drawing.Point(2, 2);
         this.chkUsedIn.MenuManager = this._barManager;
         this.chkUsedIn.Name = "chkUsedIn";
         this.chkUsedIn.Properties.AllowFocused = false;
         this.chkUsedIn.Properties.Caption = "chkUsedIn";
         this.chkUsedIn.Size = new System.Drawing.Size(270, 20);
         this.chkUsedIn.StyleController = this.layoutControl;
         this.chkUsedIn.TabIndex = 24;
         // 
         // panelCurveColorGrouping
         // 
         this.panelCurveColorGrouping.Location = new System.Drawing.Point(14, 315);
         this.panelCurveColorGrouping.Margin = new System.Windows.Forms.Padding(1);
         this.panelCurveColorGrouping.Name = "panelCurveColorGrouping";
         this.panelCurveColorGrouping.Size = new System.Drawing.Size(530, 270);
         this.panelCurveColorGrouping.TabIndex = 4;
         // 
         // panelChartSettings
         // 
         this.panelChartSettings.Location = new System.Drawing.Point(14, 315);
         this.panelChartSettings.Name = "panelChartSettings";
         this.panelChartSettings.Size = new System.Drawing.Size(530, 270);
         this.panelChartSettings.TabIndex = 22;
         // 
         // panelChartExportSettings
         // 
         this.panelChartExportSettings.Location = new System.Drawing.Point(14, 315);
         this.panelChartExportSettings.Name = "panelChartExportSettings";
         this.panelChartExportSettings.Size = new System.Drawing.Size(530, 270);
         this.panelChartExportSettings.TabIndex = 23;
         // 
         // panelDataBrowser
         // 
         this.panelDataBrowser.Location = new System.Drawing.Point(2, 52);
         this.panelDataBrowser.Name = "panelDataBrowser";
         this.panelDataBrowser.Size = new System.Drawing.Size(554, 214);
         this.panelDataBrowser.TabIndex = 21;
         // 
         // panelAxisOptions
         // 
         this.panelAxisOptions.Location = new System.Drawing.Point(14, 476);
         this.panelAxisOptions.Name = "panelAxisOptions";
         this.panelAxisOptions.Size = new System.Drawing.Size(530, 109);
         this.panelAxisOptions.TabIndex = 20;
         // 
         // panelCurveOptions
         // 
         this.panelCurveOptions.Location = new System.Drawing.Point(14, 315);
         this.panelCurveOptions.Name = "panelCurveOptions";
         this.panelCurveOptions.Size = new System.Drawing.Size(530, 147);
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
            this.layoutControlItem1,
            this.layoutControlItemUsedIn,
            this.layoutControlItemLink,
            this.layoutControlItemAutoUpdateCharts,
            this.layoutControlItemApplyButton,
            this.emptySpaceItem1});
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(558, 599);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // tabbedControlGroup1
         // 
         this.tabbedControlGroup1.CustomizationFormText = "Options";
         this.tabbedControlGroup1.Location = new System.Drawing.Point(0, 278);
         this.tabbedControlGroup1.Name = "tabbedControlGroup1";
         this.tabbedControlGroup1.SelectedTabPage = this.layoutCurveAndChartSettings;
         this.tabbedControlGroup1.Size = new System.Drawing.Size(558, 321);
         this.tabbedControlGroup1.TabPages.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutCurveAndChartSettings,
            this.layoutChartOptions,
            this.layoutChartExportOptions,
            this.layoutColorGrouping});
         this.tabbedControlGroup1.Text = "Options";
         // 
         // layoutCurveAndChartSettings
         // 
         this.layoutCurveAndChartSettings.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.splitterItem2,
            this.layoutControlItem3,
            this.layoutControlItem4});
         this.layoutCurveAndChartSettings.Location = new System.Drawing.Point(0, 0);
         this.layoutCurveAndChartSettings.Name = "layoutCurveAndChartSettings";
         this.layoutCurveAndChartSettings.Size = new System.Drawing.Size(534, 274);
         // 
         // splitterItem2
         // 
         this.splitterItem2.AllowHotTrack = true;
         this.splitterItem2.CustomizationFormText = "splitterItem2";
         this.splitterItem2.Location = new System.Drawing.Point(0, 151);
         this.splitterItem2.Name = "splitterItem2";
         this.splitterItem2.Size = new System.Drawing.Size(534, 10);
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.panelCurveOptions;
         this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem3.MinSize = new System.Drawing.Size(5, 5);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(534, 151);
         this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.panelAxisOptions;
         this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
         this.layoutControlItem4.Location = new System.Drawing.Point(0, 161);
         this.layoutControlItem4.MinSize = new System.Drawing.Size(5, 5);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Size = new System.Drawing.Size(534, 113);
         this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem4.TextVisible = false;
         // 
         // layoutChartOptions
         // 
         this.layoutChartOptions.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
         this.layoutChartOptions.Location = new System.Drawing.Point(0, 0);
         this.layoutChartOptions.Name = "layoutChartOptions";
         this.layoutChartOptions.Size = new System.Drawing.Size(534, 274);
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.panelChartSettings;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.MinSize = new System.Drawing.Size(5, 5);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(534, 274);
         this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutChartExportOptions
         // 
         this.layoutChartExportOptions.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem5});
         this.layoutChartExportOptions.Location = new System.Drawing.Point(0, 0);
         this.layoutChartExportOptions.Name = "layoutChartExportOptions";
         this.layoutChartExportOptions.Size = new System.Drawing.Size(534, 274);
         // 
         // layoutControlItem5
         // 
         this.layoutControlItem5.Control = this.panelChartExportSettings;
         this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem5.MinSize = new System.Drawing.Size(5, 5);
         this.layoutControlItem5.Name = "layoutControlItem5";
         this.layoutControlItem5.Size = new System.Drawing.Size(534, 274);
         this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem5.TextVisible = false;
         // 
         // layoutColorGrouping
         // 
         this.layoutColorGrouping.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem6});
         this.layoutColorGrouping.Location = new System.Drawing.Point(0, 0);
         this.layoutColorGrouping.Name = "layoutColorGrouping";
         this.layoutColorGrouping.Size = new System.Drawing.Size(534, 274);
         // 
         // layoutControlItem6
         // 
         this.layoutControlItem6.Control = this.panelCurveColorGrouping;
         this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem6.Name = "layoutControlItem6";
         this.layoutControlItem6.Size = new System.Drawing.Size(534, 274);
         this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem6.TextVisible = false;
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.CustomizationFormText = "splitterItem1";
         this.splitterItem1.Location = new System.Drawing.Point(0, 268);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(558, 10);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.panelDataBrowser;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 50);
         this.layoutControlItem1.MinSize = new System.Drawing.Size(5, 5);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(558, 218);
         this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItemUsedIn
         // 
         this.layoutControlItemUsedIn.Control = this.chkUsedIn;
         this.layoutControlItemUsedIn.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemUsedIn.Name = "layoutControlItemUsedIn";
         this.layoutControlItemUsedIn.Size = new System.Drawing.Size(274, 24);
         this.layoutControlItemUsedIn.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemUsedIn.TextVisible = false;
         // 
         // layoutControlItemLink
         // 
         this.layoutControlItemLink.Control = this.chkLinkSimulationObserved;
         this.layoutControlItemLink.Location = new System.Drawing.Point(274, 0);
         this.layoutControlItemLink.Name = "layoutControlItemLink";
         this.layoutControlItemLink.Size = new System.Drawing.Size(274, 24);
         this.layoutControlItemLink.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemLink.TextVisible = false;
         // 
         // layoutControlItemAutoUpdateCharts
         // 
         this.layoutControlItemAutoUpdateCharts.Control = this.chkAutoUpdateCharts;
         this.layoutControlItemAutoUpdateCharts.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItemAutoUpdateCharts.Name = "layoutControlItemAutoUpdateCharts";
         this.layoutControlItemAutoUpdateCharts.Size = new System.Drawing.Size(274, 26);
         this.layoutControlItemAutoUpdateCharts.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemAutoUpdateCharts.TextVisible = false;
         // 
         // layoutControlItemApplyButton
         // 
         this.layoutControlItemApplyButton.Control = this.btnApplyChartUpdates;
         this.layoutControlItemApplyButton.Location = new System.Drawing.Point(274, 24);
         this.layoutControlItemApplyButton.Name = "layoutControlItemApplyButton";
         this.layoutControlItemApplyButton.Size = new System.Drawing.Size(274, 26);
         this.layoutControlItemApplyButton.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemApplyButton.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(548, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(10, 50);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
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
         this.Margin = new System.Windows.Forms.Padding(8);
         this.Name = "ChartEditorView";
         this.Size = new System.Drawing.Size(558, 619);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._dockManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._barManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.chkAutoUpdateCharts.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkLinkSimulationObserved.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkUsedIn.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelCurveColorGrouping)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelChartSettings)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelChartExportSettings)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelDataBrowser)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelAxisOptions)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelCurveOptions)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutCurveAndChartSettings)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutChartOptions)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutChartExportOptions)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutColorGrouping)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemUsedIn)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemLink)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAutoUpdateCharts)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemApplyButton)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
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
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraLayout.SplitterItem splitterItem2;
      private DevExpress.XtraLayout.TabbedControlGroup tabbedControlGroup1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutCurveAndChartSettings;
      private DevExpress.XtraLayout.LayoutControlGroup layoutChartOptions;
      private DevExpress.XtraEditors.PanelControl panelAxisOptions;
      private DevExpress.XtraEditors.PanelControl panelCurveOptions;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
      private DevExpress.XtraEditors.PanelControl panelDataBrowser;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraEditors.PanelControl panelChartSettings;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraEditors.PanelControl panelChartExportSettings;
      private DevExpress.XtraLayout.LayoutControlGroup layoutChartExportOptions;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
      private Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraEditors.PanelControl panelCurveColorGrouping;
      private DevExpress.XtraLayout.LayoutControlGroup layoutColorGrouping;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
      private Controls.UxSimpleButton btnApplyChartUpdates;
      private Controls.UxCheckEdit chkAutoUpdateCharts;
      private Controls.UxCheckEdit chkLinkSimulationObserved;
      private Controls.UxCheckEdit chkUsedIn;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemUsedIn;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemLink;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAutoUpdateCharts;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemApplyButton;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
