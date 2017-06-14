using DevExpress.XtraEditors;

namespace OSPSuite.Starter.Views
{
   partial class ChartTestView
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
         this.components = new System.ComponentModel.Container();
         this.uxLayoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.numberOfObservationPointsTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.barManager = new DevExpress.XtraBars.BarManager(this.components);
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         this.addObservationsButton = new DevExpress.XtraEditors.DropDownButton();
         this.numberOfCalculationPointsTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.redrawChartButton = new DevExpress.XtraEditors.SimpleButton();
         this.clearChartButton = new DevExpress.XtraEditors.SimpleButton();
         this.numberOfObservationsTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.numberOfCalculationsTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.addCalculationsButton = new DevExpress.XtraEditors.DropDownButton();
         this.originalConfigurationButton = new DevExpress.XtraEditors.SimpleButton();
         this.removeDatalessCurvesButton = new DevExpress.XtraEditors.SimpleButton();
         this.reloadMenusButton = new DevExpress.XtraEditors.SimpleButton();
         this.refreshDisplayButton = new DevExpress.XtraEditors.SimpleButton();
         this.loadSettingsButton = new DevExpress.XtraEditors.SimpleButton();
         this.saveSettingsButton = new DevExpress.XtraEditors.SimpleButton();
         this.loadChartButton = new DevExpress.XtraEditors.SimpleButton();
         this.saveChartButton = new DevExpress.XtraEditors.SimpleButton();
         this.chartEditorPanel = new DevExpress.XtraEditors.PanelControl();
         this.chartDisplayPanel = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.chartDisplayLayoutControl = new DevExpress.XtraLayout.LayoutControlItem();
         this.chartEditorLayoutControl = new DevExpress.XtraLayout.LayoutControlItem();
         this.saveChartLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.loadChartLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.saveSettingsLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.loadSettingsLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.refreshDisplayLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.reloadMenusLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.removeDatalessCurvesLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.originalConfigurationLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.clearChartLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.redrawChartLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.calculationsLayoutGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.numberOfPointsLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.numberOfCalculationsLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.addCalculationsLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.observationsLayoutGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.addObservationsLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.numberOfObservationsLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.numberOfPointsPerObservation = new DevExpress.XtraLayout.LayoutControlItem();
         this.lloqForObservationsTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.lloqLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).BeginInit();
         this.uxLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.numberOfObservationPointsTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberOfCalculationPointsTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberOfObservationsTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberOfCalculationsTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartEditorPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartDisplayPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartDisplayLayoutControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartEditorLayoutControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.saveChartLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.loadChartLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.saveSettingsLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.loadSettingsLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.refreshDisplayLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.reloadMenusLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.removeDatalessCurvesLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.originalConfigurationLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.clearChartLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.redrawChartLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.calculationsLayoutGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberOfPointsLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberOfCalculationsLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.addCalculationsLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.observationsLayoutGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.addObservationsLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberOfObservationsLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberOfPointsPerObservation)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.lloqForObservationsTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.lloqLayoutItem)).BeginInit();
         this.SuspendLayout();
         // 
         // uxLayoutControl
         // 
         this.uxLayoutControl.AllowCustomization = false;
         this.uxLayoutControl.Controls.Add(this.lloqForObservationsTextEdit);
         this.uxLayoutControl.Controls.Add(this.numberOfObservationPointsTextEdit);
         this.uxLayoutControl.Controls.Add(this.addObservationsButton);
         this.uxLayoutControl.Controls.Add(this.numberOfCalculationPointsTextEdit);
         this.uxLayoutControl.Controls.Add(this.redrawChartButton);
         this.uxLayoutControl.Controls.Add(this.clearChartButton);
         this.uxLayoutControl.Controls.Add(this.numberOfObservationsTextEdit);
         this.uxLayoutControl.Controls.Add(this.numberOfCalculationsTextEdit);
         this.uxLayoutControl.Controls.Add(this.addCalculationsButton);
         this.uxLayoutControl.Controls.Add(this.originalConfigurationButton);
         this.uxLayoutControl.Controls.Add(this.removeDatalessCurvesButton);
         this.uxLayoutControl.Controls.Add(this.reloadMenusButton);
         this.uxLayoutControl.Controls.Add(this.refreshDisplayButton);
         this.uxLayoutControl.Controls.Add(this.loadSettingsButton);
         this.uxLayoutControl.Controls.Add(this.saveSettingsButton);
         this.uxLayoutControl.Controls.Add(this.loadChartButton);
         this.uxLayoutControl.Controls.Add(this.saveChartButton);
         this.uxLayoutControl.Controls.Add(this.chartEditorPanel);
         this.uxLayoutControl.Controls.Add(this.chartDisplayPanel);
         this.uxLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl.Name = "uxLayoutControl";
         this.uxLayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(701, 184, 939, 623);
         this.uxLayoutControl.Root = this.layoutControlGroup1;
         this.uxLayoutControl.Size = new System.Drawing.Size(1370, 686);
         this.uxLayoutControl.TabIndex = 0;
         this.uxLayoutControl.Text = "uxLayoutControl1";
         // 
         // numberOfObservationPointsTextEdit
         // 
         this.numberOfObservationPointsTextEdit.Location = new System.Drawing.Point(102, 107);
         this.numberOfObservationPointsTextEdit.MenuManager = this.barManager;
         this.numberOfObservationPointsTextEdit.Name = "numberOfObservationPointsTextEdit";
         this.numberOfObservationPointsTextEdit.Size = new System.Drawing.Size(145, 20);
         this.numberOfObservationPointsTextEdit.StyleController = this.uxLayoutControl;
         this.numberOfObservationPointsTextEdit.TabIndex = 30;
         // 
         // barManager
         // 
         this.barManager.DockControls.Add(this.barDockControlTop);
         this.barManager.DockControls.Add(this.barDockControlBottom);
         this.barManager.DockControls.Add(this.barDockControlLeft);
         this.barManager.DockControls.Add(this.barDockControlRight);
         this.barManager.Form = this;
         this.barManager.MaxItemId = 0;
         // 
         // barDockControlTop
         // 
         this.barDockControlTop.CausesValidation = false;
         this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop.Manager = this.barManager;
         this.barDockControlTop.Size = new System.Drawing.Size(1370, 0);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 686);
         this.barDockControlBottom.Manager = this.barManager;
         this.barDockControlBottom.Size = new System.Drawing.Size(1370, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
         this.barDockControlLeft.Manager = this.barManager;
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 686);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(1370, 0);
         this.barDockControlRight.Manager = this.barManager;
         this.barDockControlRight.Size = new System.Drawing.Size(0, 686);
         // 
         // addObservationsButton
         // 
         this.addObservationsButton.Location = new System.Drawing.Point(75, 155);
         this.addObservationsButton.Name = "addObservationsButton";
         this.addObservationsButton.Size = new System.Drawing.Size(172, 22);
         this.addObservationsButton.StyleController = this.uxLayoutControl;
         this.addObservationsButton.TabIndex = 29;
         this.addObservationsButton.Text = "Add Observations";
         // 
         // numberOfCalculationPointsTextEdit
         // 
         this.numberOfCalculationPointsTextEdit.Location = new System.Drawing.Point(102, 33);
         this.numberOfCalculationPointsTextEdit.Name = "numberOfCalculationPointsTextEdit";
         this.numberOfCalculationPointsTextEdit.Size = new System.Drawing.Size(145, 20);
         this.numberOfCalculationPointsTextEdit.StyleController = this.uxLayoutControl;
         this.numberOfCalculationPointsTextEdit.TabIndex = 28;
         // 
         // redrawChartButton
         // 
         this.redrawChartButton.Location = new System.Drawing.Point(12, 210);
         this.redrawChartButton.Name = "redrawChartButton";
         this.redrawChartButton.Size = new System.Drawing.Size(238, 22);
         this.redrawChartButton.StyleController = this.uxLayoutControl;
         this.redrawChartButton.TabIndex = 27;
         this.redrawChartButton.Text = "Redraw Chart";
         // 
         // clearChartButton
         // 
         this.clearChartButton.Location = new System.Drawing.Point(12, 184);
         this.clearChartButton.Name = "clearChartButton";
         this.clearChartButton.Size = new System.Drawing.Size(238, 22);
         this.clearChartButton.StyleController = this.uxLayoutControl;
         this.clearChartButton.TabIndex = 25;
         this.clearChartButton.Text = "Clear Repositories";
         // 
         // numberOfObservationsTextEdit
         // 
         this.numberOfObservationsTextEdit.Location = new System.Drawing.Point(15, 155);
         this.numberOfObservationsTextEdit.Name = "numberOfObservationsTextEdit";
         this.numberOfObservationsTextEdit.Size = new System.Drawing.Size(56, 20);
         this.numberOfObservationsTextEdit.StyleController = this.uxLayoutControl;
         this.numberOfObservationsTextEdit.TabIndex = 24;
         // 
         // numberOfCalculationsTextEdit
         // 
         this.numberOfCalculationsTextEdit.Location = new System.Drawing.Point(15, 57);
         this.numberOfCalculationsTextEdit.Name = "numberOfCalculationsTextEdit";
         this.numberOfCalculationsTextEdit.Size = new System.Drawing.Size(56, 20);
         this.numberOfCalculationsTextEdit.StyleController = this.uxLayoutControl;
         this.numberOfCalculationsTextEdit.TabIndex = 22;
         // 
         // addCalculationsButton
         // 
         this.addCalculationsButton.Location = new System.Drawing.Point(75, 57);
         this.addCalculationsButton.Name = "addCalculationsButton";
         this.addCalculationsButton.Size = new System.Drawing.Size(172, 22);
         this.addCalculationsButton.StyleController = this.uxLayoutControl;
         this.addCalculationsButton.TabIndex = 21;
         this.addCalculationsButton.Text = "Add Calculations";
         // 
         // originalConfigurationButton
         // 
         this.originalConfigurationButton.Location = new System.Drawing.Point(12, 236);
         this.originalConfigurationButton.Name = "originalConfigurationButton";
         this.originalConfigurationButton.Size = new System.Drawing.Size(238, 22);
         this.originalConfigurationButton.StyleController = this.uxLayoutControl;
         this.originalConfigurationButton.TabIndex = 20;
         this.originalConfigurationButton.Text = "Original Configuration";
         // 
         // removeDatalessCurvesButton
         // 
         this.removeDatalessCurvesButton.Location = new System.Drawing.Point(12, 418);
         this.removeDatalessCurvesButton.Name = "removeDatalessCurvesButton";
         this.removeDatalessCurvesButton.Size = new System.Drawing.Size(238, 22);
         this.removeDatalessCurvesButton.StyleController = this.uxLayoutControl;
         this.removeDatalessCurvesButton.TabIndex = 19;
         this.removeDatalessCurvesButton.Text = "Remove Dataless Curves";
         // 
         // reloadMenusButton
         // 
         this.reloadMenusButton.Location = new System.Drawing.Point(12, 392);
         this.reloadMenusButton.Name = "reloadMenusButton";
         this.reloadMenusButton.Size = new System.Drawing.Size(238, 22);
         this.reloadMenusButton.StyleController = this.uxLayoutControl;
         this.reloadMenusButton.TabIndex = 16;
         this.reloadMenusButton.Text = "Reload Menus";
         // 
         // refreshDisplayButton
         // 
         this.refreshDisplayButton.Location = new System.Drawing.Point(12, 366);
         this.refreshDisplayButton.Name = "refreshDisplayButton";
         this.refreshDisplayButton.Size = new System.Drawing.Size(238, 22);
         this.refreshDisplayButton.StyleController = this.uxLayoutControl;
         this.refreshDisplayButton.TabIndex = 13;
         this.refreshDisplayButton.Text = "Refresh Display";
         // 
         // loadSettingsButton
         // 
         this.loadSettingsButton.Location = new System.Drawing.Point(12, 340);
         this.loadSettingsButton.Name = "loadSettingsButton";
         this.loadSettingsButton.Size = new System.Drawing.Size(238, 22);
         this.loadSettingsButton.StyleController = this.uxLayoutControl;
         this.loadSettingsButton.TabIndex = 12;
         this.loadSettingsButton.Text = "Load Settings";
         // 
         // saveSettingsButton
         // 
         this.saveSettingsButton.Location = new System.Drawing.Point(12, 314);
         this.saveSettingsButton.Name = "saveSettingsButton";
         this.saveSettingsButton.Size = new System.Drawing.Size(238, 22);
         this.saveSettingsButton.StyleController = this.uxLayoutControl;
         this.saveSettingsButton.TabIndex = 11;
         this.saveSettingsButton.Text = "Save Settings";
         // 
         // loadChartButton
         // 
         this.loadChartButton.Location = new System.Drawing.Point(12, 288);
         this.loadChartButton.Name = "loadChartButton";
         this.loadChartButton.Size = new System.Drawing.Size(238, 22);
         this.loadChartButton.StyleController = this.uxLayoutControl;
         this.loadChartButton.TabIndex = 10;
         this.loadChartButton.Text = "Load Chart";
         // 
         // saveChartButton
         // 
         this.saveChartButton.Location = new System.Drawing.Point(12, 262);
         this.saveChartButton.Name = "saveChartButton";
         this.saveChartButton.Size = new System.Drawing.Size(238, 22);
         this.saveChartButton.StyleController = this.uxLayoutControl;
         this.saveChartButton.TabIndex = 9;
         this.saveChartButton.Text = "Save Chart";
         // 
         // chartEditorPanel
         // 
         this.chartEditorPanel.Location = new System.Drawing.Point(1026, 12);
         this.chartEditorPanel.Name = "chartEditorPanel";
         this.chartEditorPanel.Size = new System.Drawing.Size(332, 662);
         this.chartEditorPanel.TabIndex = 7;
         // 
         // chartDisplayPanel
         // 
         this.chartDisplayPanel.Location = new System.Drawing.Point(254, 12);
         this.chartDisplayPanel.Name = "chartDisplayPanel";
         this.chartDisplayPanel.Size = new System.Drawing.Size(763, 662);
         this.chartDisplayPanel.TabIndex = 5;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.chartDisplayLayoutControl,
            this.chartEditorLayoutControl,
            this.saveChartLayoutItem,
            this.loadChartLayoutItem,
            this.saveSettingsLayoutItem,
            this.loadSettingsLayoutItem,
            this.refreshDisplayLayoutItem,
            this.splitterItem1,
            this.reloadMenusLayoutItem,
            this.removeDatalessCurvesLayoutItem,
            this.originalConfigurationLayoutItem,
            this.clearChartLayoutItem,
            this.redrawChartLayoutItem,
            this.calculationsLayoutGroup,
            this.observationsLayoutGroup});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Size = new System.Drawing.Size(1370, 686);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // chartDisplayLayoutControl
         // 
         this.chartDisplayLayoutControl.Control = this.chartDisplayPanel;
         this.chartDisplayLayoutControl.Location = new System.Drawing.Point(242, 0);
         this.chartDisplayLayoutControl.Name = "chartDisplayLayoutControl";
         this.chartDisplayLayoutControl.Size = new System.Drawing.Size(767, 666);
         this.chartDisplayLayoutControl.TextSize = new System.Drawing.Size(0, 0);
         this.chartDisplayLayoutControl.TextVisible = false;
         // 
         // chartEditorLayoutControl
         // 
         this.chartEditorLayoutControl.Control = this.chartEditorPanel;
         this.chartEditorLayoutControl.Location = new System.Drawing.Point(1014, 0);
         this.chartEditorLayoutControl.Name = "chartEditorLayoutControl";
         this.chartEditorLayoutControl.Size = new System.Drawing.Size(336, 666);
         this.chartEditorLayoutControl.TextSize = new System.Drawing.Size(0, 0);
         this.chartEditorLayoutControl.TextVisible = false;
         // 
         // saveChartLayoutItem
         // 
         this.saveChartLayoutItem.Control = this.saveChartButton;
         this.saveChartLayoutItem.Location = new System.Drawing.Point(0, 250);
         this.saveChartLayoutItem.Name = "saveChartLayoutItem";
         this.saveChartLayoutItem.Size = new System.Drawing.Size(242, 26);
         this.saveChartLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.saveChartLayoutItem.TextVisible = false;
         // 
         // loadChartLayoutItem
         // 
         this.loadChartLayoutItem.Control = this.loadChartButton;
         this.loadChartLayoutItem.Location = new System.Drawing.Point(0, 276);
         this.loadChartLayoutItem.Name = "loadChartLayoutItem";
         this.loadChartLayoutItem.Size = new System.Drawing.Size(242, 26);
         this.loadChartLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.loadChartLayoutItem.TextVisible = false;
         // 
         // saveSettingsLayoutItem
         // 
         this.saveSettingsLayoutItem.Control = this.saveSettingsButton;
         this.saveSettingsLayoutItem.Location = new System.Drawing.Point(0, 302);
         this.saveSettingsLayoutItem.Name = "saveSettingsLayoutItem";
         this.saveSettingsLayoutItem.Size = new System.Drawing.Size(242, 26);
         this.saveSettingsLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.saveSettingsLayoutItem.TextVisible = false;
         // 
         // loadSettingsLayoutItem
         // 
         this.loadSettingsLayoutItem.Control = this.loadSettingsButton;
         this.loadSettingsLayoutItem.Location = new System.Drawing.Point(0, 328);
         this.loadSettingsLayoutItem.Name = "loadSettingsLayoutItem";
         this.loadSettingsLayoutItem.Size = new System.Drawing.Size(242, 26);
         this.loadSettingsLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.loadSettingsLayoutItem.TextVisible = false;
         // 
         // refreshDisplayLayoutItem
         // 
         this.refreshDisplayLayoutItem.Control = this.refreshDisplayButton;
         this.refreshDisplayLayoutItem.Location = new System.Drawing.Point(0, 354);
         this.refreshDisplayLayoutItem.Name = "refreshDisplayLayoutItem";
         this.refreshDisplayLayoutItem.Size = new System.Drawing.Size(242, 26);
         this.refreshDisplayLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.refreshDisplayLayoutItem.TextVisible = false;
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(1009, 0);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(5, 666);
         // 
         // reloadMenusLayoutItem
         // 
         this.reloadMenusLayoutItem.Control = this.reloadMenusButton;
         this.reloadMenusLayoutItem.Location = new System.Drawing.Point(0, 380);
         this.reloadMenusLayoutItem.Name = "reloadMenusLayoutItem";
         this.reloadMenusLayoutItem.Size = new System.Drawing.Size(242, 26);
         this.reloadMenusLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.reloadMenusLayoutItem.TextVisible = false;
         // 
         // removeDatalessCurvesLayoutItem
         // 
         this.removeDatalessCurvesLayoutItem.Control = this.removeDatalessCurvesButton;
         this.removeDatalessCurvesLayoutItem.Location = new System.Drawing.Point(0, 406);
         this.removeDatalessCurvesLayoutItem.Name = "removeDatalessCurvesLayoutItem";
         this.removeDatalessCurvesLayoutItem.Size = new System.Drawing.Size(242, 260);
         this.removeDatalessCurvesLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.removeDatalessCurvesLayoutItem.TextVisible = false;
         // 
         // originalConfigurationLayoutItem
         // 
         this.originalConfigurationLayoutItem.Control = this.originalConfigurationButton;
         this.originalConfigurationLayoutItem.Location = new System.Drawing.Point(0, 224);
         this.originalConfigurationLayoutItem.Name = "originalConfigurationLayoutItem";
         this.originalConfigurationLayoutItem.Size = new System.Drawing.Size(242, 26);
         this.originalConfigurationLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.originalConfigurationLayoutItem.TextVisible = false;
         // 
         // clearChartLayoutItem
         // 
         this.clearChartLayoutItem.Control = this.clearChartButton;
         this.clearChartLayoutItem.Location = new System.Drawing.Point(0, 172);
         this.clearChartLayoutItem.Name = "clearChartLayoutItem";
         this.clearChartLayoutItem.Size = new System.Drawing.Size(242, 26);
         this.clearChartLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.clearChartLayoutItem.TextVisible = false;
         // 
         // redrawChartLayoutItem
         // 
         this.redrawChartLayoutItem.Control = this.redrawChartButton;
         this.redrawChartLayoutItem.Location = new System.Drawing.Point(0, 198);
         this.redrawChartLayoutItem.Name = "redrawChartLayoutItem";
         this.redrawChartLayoutItem.Size = new System.Drawing.Size(242, 26);
         this.redrawChartLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.redrawChartLayoutItem.TextVisible = false;
         // 
         // calculationsLayoutGroup
         // 
         this.calculationsLayoutGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.numberOfPointsLayoutItem,
            this.numberOfCalculationsLayoutItem,
            this.addCalculationsLayoutItem});
         this.calculationsLayoutGroup.Location = new System.Drawing.Point(0, 0);
         this.calculationsLayoutGroup.Name = "calculationsLayoutGroup";
         this.calculationsLayoutGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.calculationsLayoutGroup.Size = new System.Drawing.Size(242, 74);
         this.calculationsLayoutGroup.Text = "Calculations";
         // 
         // numberOfPointsLayoutItem
         // 
         this.numberOfPointsLayoutItem.Control = this.numberOfCalculationPointsTextEdit;
         this.numberOfPointsLayoutItem.Location = new System.Drawing.Point(0, 0);
         this.numberOfPointsLayoutItem.Name = "numberOfPointsLayoutItem";
         this.numberOfPointsLayoutItem.Size = new System.Drawing.Size(236, 24);
         this.numberOfPointsLayoutItem.Text = "Number Of Points";
         this.numberOfPointsLayoutItem.TextSize = new System.Drawing.Size(84, 13);
         // 
         // numberOfCalculationsLayoutItem
         // 
         this.numberOfCalculationsLayoutItem.Control = this.numberOfCalculationsTextEdit;
         this.numberOfCalculationsLayoutItem.Location = new System.Drawing.Point(0, 24);
         this.numberOfCalculationsLayoutItem.Name = "numberOfCalculationsLayoutItem";
         this.numberOfCalculationsLayoutItem.Size = new System.Drawing.Size(60, 26);
         this.numberOfCalculationsLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.numberOfCalculationsLayoutItem.TextVisible = false;
         // 
         // addCalculationsLayoutItem
         // 
         this.addCalculationsLayoutItem.Control = this.addCalculationsButton;
         this.addCalculationsLayoutItem.Location = new System.Drawing.Point(60, 24);
         this.addCalculationsLayoutItem.Name = "addCalculationsLayoutItem";
         this.addCalculationsLayoutItem.Size = new System.Drawing.Size(176, 26);
         this.addCalculationsLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.addCalculationsLayoutItem.TextVisible = false;
         // 
         // observationsLayoutGroup
         // 
         this.observationsLayoutGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.addObservationsLayoutItem,
            this.numberOfObservationsLayoutItem,
            this.numberOfPointsPerObservation,
            this.lloqLayoutItem});
         this.observationsLayoutGroup.Location = new System.Drawing.Point(0, 74);
         this.observationsLayoutGroup.Name = "observationsLayoutGroup";
         this.observationsLayoutGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.observationsLayoutGroup.Size = new System.Drawing.Size(242, 98);
         this.observationsLayoutGroup.Text = "Observations";
         // 
         // addObservationsLayoutItem
         // 
         this.addObservationsLayoutItem.Control = this.addObservationsButton;
         this.addObservationsLayoutItem.Location = new System.Drawing.Point(60, 48);
         this.addObservationsLayoutItem.Name = "addObservationsLayoutItem";
         this.addObservationsLayoutItem.Size = new System.Drawing.Size(176, 26);
         this.addObservationsLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.addObservationsLayoutItem.TextVisible = false;
         // 
         // numberOfObservationsLayoutItem
         // 
         this.numberOfObservationsLayoutItem.Control = this.numberOfObservationsTextEdit;
         this.numberOfObservationsLayoutItem.Location = new System.Drawing.Point(0, 48);
         this.numberOfObservationsLayoutItem.Name = "numberOfObservationsLayoutItem";
         this.numberOfObservationsLayoutItem.Size = new System.Drawing.Size(60, 26);
         this.numberOfObservationsLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.numberOfObservationsLayoutItem.TextVisible = false;
         // 
         // numberOfPointsPerObservation
         // 
         this.numberOfPointsPerObservation.Control = this.numberOfObservationPointsTextEdit;
         this.numberOfPointsPerObservation.Location = new System.Drawing.Point(0, 0);
         this.numberOfPointsPerObservation.Name = "numberOfPointsPerObservation";
         this.numberOfPointsPerObservation.Size = new System.Drawing.Size(236, 24);
         this.numberOfPointsPerObservation.Text = "Number of Points";
         this.numberOfPointsPerObservation.TextSize = new System.Drawing.Size(84, 13);
         // 
         // lloqForObservationsTextEdit
         // 
         this.lloqForObservationsTextEdit.Location = new System.Drawing.Point(102, 131);
         this.lloqForObservationsTextEdit.MenuManager = this.barManager;
         this.lloqForObservationsTextEdit.Name = "lloqForObservationsTextEdit";
         this.lloqForObservationsTextEdit.Size = new System.Drawing.Size(145, 20);
         this.lloqForObservationsTextEdit.StyleController = this.uxLayoutControl;
         this.lloqForObservationsTextEdit.TabIndex = 31;
         // 
         // lloqLayoutItem
         // 
         this.lloqLayoutItem.Control = this.lloqForObservationsTextEdit;
         this.lloqLayoutItem.Location = new System.Drawing.Point(0, 24);
         this.lloqLayoutItem.Name = "lloqLayoutItem";
         this.lloqLayoutItem.Size = new System.Drawing.Size(236, 24);
         this.lloqLayoutItem.Text = "LLOQ";
         this.lloqLayoutItem.TextSize = new System.Drawing.Size(84, 13);
         // 
         // ChartTestView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.uxLayoutControl);
         this.Controls.Add(this.barDockControlLeft);
         this.Controls.Add(this.barDockControlRight);
         this.Controls.Add(this.barDockControlBottom);
         this.Controls.Add(this.barDockControlTop);
         this.Name = "ChartTestView";
         this.Size = new System.Drawing.Size(1370, 686);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).EndInit();
         this.uxLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.numberOfObservationPointsTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberOfCalculationPointsTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberOfObservationsTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberOfCalculationsTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartEditorPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartDisplayPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartDisplayLayoutControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartEditorLayoutControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.saveChartLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.loadChartLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.saveSettingsLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.loadSettingsLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.refreshDisplayLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.reloadMenusLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.removeDatalessCurvesLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.originalConfigurationLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.clearChartLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.redrawChartLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.calculationsLayoutGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberOfPointsLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberOfCalculationsLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.addCalculationsLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.observationsLayoutGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.addObservationsLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberOfObservationsLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numberOfPointsPerObservation)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.lloqForObservationsTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.lloqLayoutItem)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private UI.Controls.UxLayoutControl uxLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.PanelControl chartDisplayPanel;
      private DevExpress.XtraLayout.LayoutControlItem chartDisplayLayoutControl;
      private DevExpress.XtraEditors.PanelControl chartEditorPanel;
      private DevExpress.XtraLayout.LayoutControlItem chartEditorLayoutControl;
      private DevExpress.XtraEditors.SimpleButton loadChartButton;
      private DevExpress.XtraEditors.SimpleButton saveChartButton;
      private DevExpress.XtraLayout.LayoutControlItem saveChartLayoutItem;
      private DevExpress.XtraLayout.LayoutControlItem loadChartLayoutItem;
      private DevExpress.XtraEditors.SimpleButton loadSettingsButton;
      private DevExpress.XtraEditors.SimpleButton saveSettingsButton;
      private DevExpress.XtraLayout.LayoutControlItem saveSettingsLayoutItem;
      private DevExpress.XtraLayout.LayoutControlItem loadSettingsLayoutItem;
      private DevExpress.XtraEditors.SimpleButton refreshDisplayButton;
      private DevExpress.XtraLayout.LayoutControlItem refreshDisplayLayoutItem;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraEditors.SimpleButton reloadMenusButton;
      private DevExpress.XtraLayout.LayoutControlItem reloadMenusLayoutItem;
      private DevExpress.XtraEditors.SimpleButton removeDatalessCurvesButton;
      private DevExpress.XtraLayout.LayoutControlItem removeDatalessCurvesLayoutItem;
      private DevExpress.XtraEditors.SimpleButton originalConfigurationButton;
      private DevExpress.XtraLayout.LayoutControlItem originalConfigurationLayoutItem;
      private DevExpress.XtraEditors.DropDownButton addCalculationsButton;
      private DevExpress.XtraEditors.TextEdit numberOfCalculationsTextEdit;
      private DevExpress.XtraEditors.TextEdit numberOfObservationsTextEdit;
      private DevExpress.XtraLayout.LayoutControlItem addCalculationsLayoutItem;
      private DevExpress.XtraLayout.LayoutControlItem numberOfCalculationsLayoutItem;
      private DevExpress.XtraLayout.LayoutControlItem numberOfObservationsLayoutItem;
      private DevExpress.XtraEditors.SimpleButton clearChartButton;
      private DevExpress.XtraLayout.LayoutControlItem clearChartLayoutItem;
      private DevExpress.XtraEditors.SimpleButton redrawChartButton;
      private DevExpress.XtraLayout.LayoutControlItem redrawChartLayoutItem;
      private TextEdit numberOfCalculationPointsTextEdit;
      private DropDownButton addObservationsButton;
      private DevExpress.XtraLayout.LayoutControlItem addObservationsLayoutItem;
      private DevExpress.XtraBars.BarManager barManager;
      private DevExpress.XtraBars.BarDockControl barDockControlTop;
      private DevExpress.XtraBars.BarDockControl barDockControlBottom;
      private DevExpress.XtraBars.BarDockControl barDockControlLeft;
      private DevExpress.XtraBars.BarDockControl barDockControlRight;
      private TextEdit numberOfObservationPointsTextEdit;
      private DevExpress.XtraLayout.LayoutControlGroup calculationsLayoutGroup;
      private DevExpress.XtraLayout.LayoutControlItem numberOfPointsLayoutItem;
      private DevExpress.XtraLayout.LayoutControlItem numberOfPointsPerObservation;
      private DevExpress.XtraLayout.LayoutControlGroup observationsLayoutGroup;
      private TextEdit lloqForObservationsTextEdit;
      private DevExpress.XtraLayout.LayoutControlItem lloqLayoutItem;
   }
}
