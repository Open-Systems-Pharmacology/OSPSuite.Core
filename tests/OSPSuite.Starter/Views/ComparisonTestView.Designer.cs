using OSPSuite.UI.Controls;

namespace OSPSuite.Starter.Views
{
   partial class ComparisonTestView
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
         this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
         this.comparisonPanel = new OSPSuite.UI.Controls.UxDockPanel();
         this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
         this.comparisonPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // dockManager1
         // 
         this.dockManager1.Form = this;
         this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.comparisonPanel});
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
         // comparisonPanel
         // 
         this.comparisonPanel.Controls.Add(this.dockPanel1_Container);
         this.comparisonPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Top;
         this.comparisonPanel.FloatVertical = true;
         this.comparisonPanel.ID = new System.Guid("89933267-46a1-4f24-8305-b49b304bf4ff");
         this.comparisonPanel.Location = new System.Drawing.Point(0, 0);
         this.comparisonPanel.Name = "comparisonPanel";
         this.comparisonPanel.OriginalSize = new System.Drawing.Size(200, 535);
         this.comparisonPanel.Size = new System.Drawing.Size(1098, 535);
         this.comparisonPanel.Text = "panelComparison";
         // 
         // dockPanel1_Container
         // 
         this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
         this.dockPanel1_Container.Name = "dockPanel1_Container";
         this.dockPanel1_Container.Size = new System.Drawing.Size(1090, 507);
         this.dockPanel1_Container.TabIndex = 0;
         // 
         // ComparisonTestView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.comparisonPanel);
         this.Name = "ComparisonTestView";
         this.Size = new System.Drawing.Size(1098, 535);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
         this.comparisonPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraBars.Docking.DockManager dockManager1;
      private UxDockPanel comparisonPanel;
      private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
   }
}
