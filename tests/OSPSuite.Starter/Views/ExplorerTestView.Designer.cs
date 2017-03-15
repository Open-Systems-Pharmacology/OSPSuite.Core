using OSPSuite.UI.Controls;

namespace OSPSuite.Starter.Views
{
   partial class ExplorerTestView
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
         this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager();
         this.explorerPanel = new UxDockPanel();
         this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
         this.explorerPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // dockManager1
         // 
         this.dockManager1.Form = this;
         this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.explorerPanel});
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
         // explorerPanel
         // 
         this.explorerPanel.Controls.Add(this.dockPanel1_Container);
         this.explorerPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
         this.explorerPanel.ID = new System.Guid("48dc6053-a162-435c-b614-6a64234f488a");
         this.explorerPanel.Location = new System.Drawing.Point(0, 0);
         this.explorerPanel.Name = "explorerPanel";
         this.explorerPanel.OriginalSize = new System.Drawing.Size(380, 200);
         this.explorerPanel.Size = new System.Drawing.Size(380, 814);
         this.explorerPanel.Text = "explorerPanel";
         // 
         // dockPanel1_Container
         // 
         this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
         this.dockPanel1_Container.Name = "dockPanel1_Container";
         this.dockPanel1_Container.Size = new System.Drawing.Size(372, 787);
         this.dockPanel1_Container.TabIndex = 0;
         // 
         // ExplorerTestView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.explorerPanel);
         this.Name = "ExplorerTestView";
         this.Size = new System.Drawing.Size(862, 814);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
         this.explorerPanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraBars.Docking.DockManager dockManager1;
      private UxDockPanel explorerPanel;
      private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;

   }
}
