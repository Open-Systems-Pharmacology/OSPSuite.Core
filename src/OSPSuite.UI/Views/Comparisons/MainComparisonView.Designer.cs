namespace OSPSuite.UI.Views.Comparisons
{
   partial class MainComparisonView
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
         this.barManager = new DevExpress.XtraBars.BarManager(this.components);
         this.toolBar = new DevExpress.XtraBars.Bar();
         this.btnExportToExcel = new DevExpress.XtraBars.BarButtonItem();
         this.btnRunComparison = new DevExpress.XtraBars.BarButtonItem();
         this.btnSettings = new DevExpress.XtraBars.BarButtonItem();
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.panelSettings = new DevExpress.XtraEditors.PanelControl();
         this.panelComparison = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemComparison = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupSettings = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemSettings = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem = new DevExpress.XtraLayout.SplitterItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelSettings)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelComparison)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemComparison)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupSettings)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSettings)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem)).BeginInit();
         this.SuspendLayout();
         // 
         // barManager
         // 
         this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.toolBar});
         this.barManager.DockControls.Add(this.barDockControlTop);
         this.barManager.DockControls.Add(this.barDockControlBottom);
         this.barManager.DockControls.Add(this.barDockControlLeft);
         this.barManager.DockControls.Add(this.barDockControlRight);
         this.barManager.Form = this;
         this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnExportToExcel,
            this.btnRunComparison,
            this.btnSettings});
         this.barManager.MaxItemId = 3;
         // 
         // toolBar
         // 
         this.toolBar.BarName = "Tools";
         this.toolBar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
         this.toolBar.DockCol = 0;
         this.toolBar.DockRow = 0;
         this.toolBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
         this.toolBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnExportToExcel),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnRunComparison),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnSettings)});
         this.toolBar.OptionsBar.AllowQuickCustomization = false;
         this.toolBar.OptionsBar.DisableClose = true;
         this.toolBar.OptionsBar.DisableCustomization = true;
         this.toolBar.OptionsBar.UseWholeRow = true;
         this.toolBar.Text = "Tools";
         // 
         // btnExportToExcel
         // 
         this.btnExportToExcel.Caption = "btnExportToExcel";
         this.btnExportToExcel.Id = 0;
         this.btnExportToExcel.Name = "btnExportToExcel";
         // 
         // btnRunComparison
         // 
         this.btnRunComparison.Caption = "btnRunComparison";
         this.btnRunComparison.Id = 1;
         this.btnRunComparison.Name = "btnRunComparison";
         // 
         // btnSettings
         // 
         this.btnSettings.Caption = "btnSettings";
         this.btnSettings.Id = 2;
         this.btnSettings.Name = "btnSettings";
         // 
         // barDockControlTop
         // 
         this.barDockControlTop.CausesValidation = false;
         this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop.Size = new System.Drawing.Size(569, 29);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 287);
         this.barDockControlBottom.Size = new System.Drawing.Size(569, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 258);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(569, 29);
         this.barDockControlRight.Size = new System.Drawing.Size(0, 258);
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.panelSettings);
         this.layoutControl.Controls.Add(this.panelComparison);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 29);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(862, 87, 250, 350);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(569, 258);
         this.layoutControl.TabIndex = 4;
         this.layoutControl.Text = "layoutControl1";
         // 
         // panelSettings
         // 
         this.panelSettings.Location = new System.Drawing.Point(453, 16);
         this.panelSettings.Name = "panelSettings";
         this.panelSettings.Size = new System.Drawing.Size(100, 226);
         this.panelSettings.TabIndex = 5;
         // 
         // panelComparison
         // 
         this.panelComparison.Location = new System.Drawing.Point(113, 2);
         this.panelComparison.Name = "panelComparison";
         this.panelComparison.Size = new System.Drawing.Size(206, 254);
         this.panelComparison.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemComparison,
            this.layoutGroupSettings,
            this.splitterItem});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(569, 258);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemComparison
         // 
         this.layoutItemComparison.Control = this.panelComparison;
         this.layoutItemComparison.CustomizationFormText = "layoutItemComparison";
         this.layoutItemComparison.Location = new System.Drawing.Point(0, 0);
         this.layoutItemComparison.Name = "layoutItemComparison";
         this.layoutItemComparison.Size = new System.Drawing.Size(321, 258);
         this.layoutItemComparison.TextSize = new System.Drawing.Size(108, 13);
         // 
         // layoutGroupSettings
         // 
         this.layoutGroupSettings.CustomizationFormText = "layoutGroupSettings";
         this.layoutGroupSettings.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemSettings});
         this.layoutGroupSettings.Location = new System.Drawing.Point(326, 0);
         this.layoutGroupSettings.Name = "layoutGroupSettings";
         this.layoutGroupSettings.Size = new System.Drawing.Size(243, 258);
         this.layoutGroupSettings.Spacing = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
         this.layoutGroupSettings.TextVisible = false;
         // 
         // layoutItemSettings
         // 
         this.layoutItemSettings.Control = this.panelSettings;
         this.layoutItemSettings.CustomizationFormText = "layoutItemSettings";
         this.layoutItemSettings.Location = new System.Drawing.Point(0, 0);
         this.layoutItemSettings.Name = "layoutItemSettings";
         this.layoutItemSettings.Size = new System.Drawing.Size(215, 230);
         this.layoutItemSettings.TextSize = new System.Drawing.Size(108, 13);
         // 
         // splitterItem
         // 
         this.splitterItem.AllowHotTrack = true;
         this.splitterItem.Location = new System.Drawing.Point(321, 0);
         this.splitterItem.Name = "splitterItem";
         this.splitterItem.Size = new System.Drawing.Size(5, 258);
         // 
         // MainComparisonView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Controls.Add(this.barDockControlLeft);
         this.Controls.Add(this.barDockControlRight);
         this.Controls.Add(this.barDockControlBottom);
         this.Controls.Add(this.barDockControlTop);
         this.Name = "MainComparisonView";
         this.Size = new System.Drawing.Size(569, 287);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelSettings)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelComparison)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemComparison)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupSettings)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSettings)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraBars.BarManager barManager;
      private DevExpress.XtraBars.BarDockControl barDockControlTop;
      private DevExpress.XtraBars.BarDockControl barDockControlBottom;
      private DevExpress.XtraBars.BarDockControl barDockControlLeft;
      private DevExpress.XtraBars.BarDockControl barDockControlRight;
      private DevExpress.XtraBars.Bar toolBar;
      private DevExpress.XtraBars.BarButtonItem btnExportToExcel;
      private DevExpress.XtraBars.BarButtonItem btnRunComparison;
      private DevExpress.XtraBars.BarButtonItem btnSettings;
      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraEditors.PanelControl panelSettings;
      private DevExpress.XtraEditors.PanelControl panelComparison;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemComparison;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSettings;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupSettings;
      private DevExpress.XtraLayout.SplitterItem splitterItem;
   }
}
