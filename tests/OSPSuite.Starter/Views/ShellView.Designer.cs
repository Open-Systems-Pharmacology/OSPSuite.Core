namespace OSPSuite.Starter.Views
{
   partial class ShellView
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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
         this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
         ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
         this.SuspendLayout();
         // 
         // ribbon
         // 
         this.ribbon.ExpandCollapseItem.Id = 0;
         this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem});
         this.ribbon.Location = new System.Drawing.Point(0, 0);
         this.ribbon.MaxItemId = 1;
         this.ribbon.Name = "ribbon";
         this.ribbon.Size = new System.Drawing.Size(1069, 49);
         this.ribbon.StatusBar = this.ribbonStatusBar;
         // 
         // ribbonStatusBar
         // 
         this.ribbonStatusBar.Location = new System.Drawing.Point(0, 582);
         this.ribbonStatusBar.Name = "ribbonStatusBar";
         this.ribbonStatusBar.Ribbon = this.ribbon;
         this.ribbonStatusBar.Size = new System.Drawing.Size(1069, 31);
         // 
         // ShellView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1069, 613);
         this.Controls.Add(this.ribbonStatusBar);
         this.Controls.Add(this.ribbon);
         this.Name = "ShellView";
         this.Ribbon = this.ribbon;
         this.StatusBar = this.ribbonStatusBar;
         this.Text = "Shell";
         ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
      private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
   }
}