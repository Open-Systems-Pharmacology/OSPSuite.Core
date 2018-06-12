using OSPSuite.UI.Controls;

namespace OSPSuite.Starter.Views
{
   partial class JournalTestView
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
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         this.controlsLayoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnResetOrdering = new DevExpress.XtraEditors.SimpleButton();
         this.searchButton = new DevExpress.XtraEditors.SimpleButton();
         this.saveDiagramButton = new DevExpress.XtraEditors.SimpleButton();
         this.exportJournalButton = new DevExpress.XtraEditors.SimpleButton();
         this.selectJournalButton = new DevExpress.XtraEditors.SimpleButton();
         this.addPageButton = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
         this.diagramPanel = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.diagramLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.controlsLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
         this.journalPanel = new UxDockPanel();
         this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
         this.btnView = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.controlsLayoutControl)).BeginInit();
         this.controlsLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.diagramPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.diagramLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.controlsLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
         this.journalPanel.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.panelControl1);
         this.layoutControl.Controls.Add(this.diagramPanel);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(785, 625);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // panelControl1
         // 
         this.panelControl1.Controls.Add(this.controlsLayoutControl);
         this.panelControl1.Location = new System.Drawing.Point(12, 12);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(144, 601);
         this.panelControl1.TabIndex = 0;
         // 
         // controlsLayoutControl
         // 
         this.controlsLayoutControl.Controls.Add(this.btnView);
         this.controlsLayoutControl.Controls.Add(this.btnResetOrdering);
         this.controlsLayoutControl.Controls.Add(this.searchButton);
         this.controlsLayoutControl.Controls.Add(this.saveDiagramButton);
         this.controlsLayoutControl.Controls.Add(this.exportJournalButton);
         this.controlsLayoutControl.Controls.Add(this.selectJournalButton);
         this.controlsLayoutControl.Controls.Add(this.addPageButton);
         this.controlsLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.controlsLayoutControl.Location = new System.Drawing.Point(2, 2);
         this.controlsLayoutControl.Name = "controlsLayoutControl";
         this.controlsLayoutControl.Root = this.layoutControlGroup2;
         this.controlsLayoutControl.Size = new System.Drawing.Size(140, 597);
         this.controlsLayoutControl.TabIndex = 0;
         this.controlsLayoutControl.Text = "layoutControl1";
         // 
         // btnResetOrdering
         // 
         this.btnResetOrdering.Location = new System.Drawing.Point(2, 132);
         this.btnResetOrdering.Name = "btnResetOrdering";
         this.btnResetOrdering.Size = new System.Drawing.Size(136, 22);
         this.btnResetOrdering.StyleController = this.controlsLayoutControl;
         this.btnResetOrdering.TabIndex = 9;
         this.btnResetOrdering.Text = "Reset Ordering";
         // 
         // searchButton
         // 
         this.searchButton.Location = new System.Drawing.Point(2, 80);
         this.searchButton.Name = "searchButton";
         this.searchButton.Size = new System.Drawing.Size(136, 22);
         this.searchButton.StyleController = this.controlsLayoutControl;
         this.searchButton.TabIndex = 8;
         this.searchButton.Text = "Search";
         // 
         // saveDiagramButton
         // 
         this.saveDiagramButton.Location = new System.Drawing.Point(2, 28);
         this.saveDiagramButton.Name = "saveDiagramButton";
         this.saveDiagramButton.Size = new System.Drawing.Size(136, 22);
         this.saveDiagramButton.StyleController = this.controlsLayoutControl;
         this.saveDiagramButton.TabIndex = 7;
         this.saveDiagramButton.Text = "Save Diagram";
         // 
         // exportJournalButton
         // 
         this.exportJournalButton.Location = new System.Drawing.Point(2, 106);
         this.exportJournalButton.Name = "exportJournalButton";
         this.exportJournalButton.Size = new System.Drawing.Size(136, 22);
         this.exportJournalButton.StyleController = this.controlsLayoutControl;
         this.exportJournalButton.TabIndex = 6;
         this.exportJournalButton.Text = "Export Journal";
         // 
         // selectJournalButton
         // 
         this.selectJournalButton.Location = new System.Drawing.Point(2, 2);
         this.selectJournalButton.Name = "selectJournalButton";
         this.selectJournalButton.Size = new System.Drawing.Size(136, 22);
         this.selectJournalButton.StyleController = this.controlsLayoutControl;
         this.selectJournalButton.TabIndex = 5;
         this.selectJournalButton.Text = "Select Journal";
         // 
         // addPageButton
         // 
         this.addPageButton.Location = new System.Drawing.Point(2, 54);
         this.addPageButton.Name = "addPageButton";
         this.addPageButton.Size = new System.Drawing.Size(136, 22);
         this.addPageButton.StyleController = this.controlsLayoutControl;
         this.addPageButton.TabIndex = 4;
         this.addPageButton.Text = "Add Page";
         // 
         // layoutControlGroup2
         // 
         this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup2.GroupBordersVisible = false;
         this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7});
         this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup2.Name = "layoutControlGroup2";
         this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup2.Size = new System.Drawing.Size(140, 597);
         this.layoutControlGroup2.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.addPageButton;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 52);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(140, 26);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.selectJournalButton;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(140, 26);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.exportJournalButton;
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 104);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(140, 26);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.saveDiagramButton;
         this.layoutControlItem4.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Size = new System.Drawing.Size(140, 26);
         this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem4.TextVisible = false;
         // 
         // layoutControlItem5
         // 
         this.layoutControlItem5.Control = this.searchButton;
         this.layoutControlItem5.Location = new System.Drawing.Point(0, 78);
         this.layoutControlItem5.Name = "layoutControlItem5";
         this.layoutControlItem5.Size = new System.Drawing.Size(140, 26);
         this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem5.TextVisible = false;
         // 
         // layoutControlItem6
         // 
         this.layoutControlItem6.Control = this.btnResetOrdering;
         this.layoutControlItem6.Location = new System.Drawing.Point(0, 130);
         this.layoutControlItem6.Name = "layoutControlItem6";
         this.layoutControlItem6.Size = new System.Drawing.Size(140, 26);
         this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem6.TextVisible = false;
         // 
         // diagramPanel
         // 
         this.diagramPanel.Location = new System.Drawing.Point(160, 12);
         this.diagramPanel.Name = "diagramPanel";
         this.diagramPanel.Size = new System.Drawing.Size(613, 601);
         this.diagramPanel.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.diagramLayoutItem,
            this.controlsLayoutItem});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(785, 625);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // diagramLayoutItem
         // 
         this.diagramLayoutItem.Control = this.diagramPanel;
         this.diagramLayoutItem.Location = new System.Drawing.Point(148, 0);
         this.diagramLayoutItem.Name = "diagramLayoutItem";
         this.diagramLayoutItem.Size = new System.Drawing.Size(617, 605);
         this.diagramLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.diagramLayoutItem.TextVisible = false;
         // 
         // controlsLayoutItem
         // 
         this.controlsLayoutItem.Control = this.panelControl1;
         this.controlsLayoutItem.Location = new System.Drawing.Point(0, 0);
         this.controlsLayoutItem.Name = "controlsLayoutItem";
         this.controlsLayoutItem.Size = new System.Drawing.Size(148, 605);
         this.controlsLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.controlsLayoutItem.TextVisible = false;
         // 
         // dockManager1
         // 
         this.dockManager1.Form = this;
         this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.journalPanel});
         this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane"});
         // 
         // journalPanel
         // 
         this.journalPanel.Controls.Add(this.dockPanel1_Container);
         this.journalPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
         this.journalPanel.ID = new System.Guid("6323e354-3fef-48ee-abb2-66ae22cf0d30");
         this.journalPanel.Location = new System.Drawing.Point(785, 0);
         this.journalPanel.Name = "journalPanel";
         this.journalPanel.OriginalSize = new System.Drawing.Size(200, 200);
         this.journalPanel.Size = new System.Drawing.Size(200, 625);
         this.journalPanel.Text = "dockPanel1";
         // 
         // dockPanel1_Container
         // 
         this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
         this.dockPanel1_Container.Name = "dockPanel1_Container";
         this.dockPanel1_Container.Size = new System.Drawing.Size(192, 598);
         this.dockPanel1_Container.TabIndex = 0;
         // 
         // btnView
         // 
         this.btnView.Location = new System.Drawing.Point(2, 158);
         this.btnView.Name = "btnView";
         this.btnView.Size = new System.Drawing.Size(136, 22);
         this.btnView.StyleController = this.controlsLayoutControl;
         this.btnView.TabIndex = 10;
         this.btnView.Text = "View";
         // 
         // layoutControlItem7
         // 
         this.layoutControlItem7.Control = this.btnView;
         this.layoutControlItem7.Location = new System.Drawing.Point(0, 156);
         this.layoutControlItem7.Name = "layoutControlItem7";
         this.layoutControlItem7.Size = new System.Drawing.Size(140, 441);
         this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem7.TextVisible = false;
         // 
         // JournalTestView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Controls.Add(this.journalPanel);
         this.Name = "JournalTestView";
         this.Size = new System.Drawing.Size(985, 625);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.controlsLayoutControl)).EndInit();
         this.controlsLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.diagramPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.diagramLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.controlsLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
         this.journalPanel.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.PanelControl diagramPanel;
      private DevExpress.XtraLayout.LayoutControlItem diagramLayoutItem;
      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraLayout.LayoutControlItem controlsLayoutItem;
      private DevExpress.XtraLayout.LayoutControl controlsLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
      private DevExpress.XtraEditors.SimpleButton addPageButton;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraEditors.SimpleButton selectJournalButton;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraEditors.SimpleButton exportJournalButton;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private DevExpress.XtraBars.Docking.DockManager dockManager1;
      private UxDockPanel journalPanel;
      private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
      private DevExpress.XtraEditors.SimpleButton saveDiagramButton;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
      private DevExpress.XtraEditors.SimpleButton searchButton;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
      private DevExpress.XtraEditors.SimpleButton btnResetOrdering;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
      private DevExpress.XtraEditors.SimpleButton btnView;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
   }
}
