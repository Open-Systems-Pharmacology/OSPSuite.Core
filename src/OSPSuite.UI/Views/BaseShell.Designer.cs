namespace OSPSuite.UI.Views
{
   partial class BaseShell
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
         this.components = new System.ComponentModel.Container();
         this.xtraTabbedMdiManager = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
         this.defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
         this.defaultToolTipController = new DevExpress.Utils.DefaultToolTipController(this.components);
         this.alertControl = new DevExpress.XtraBars.Alerter.AlertControl(this.components);
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager)).BeginInit();
         this.SuspendLayout();
         // 
         // xtraTabbedMdiManager
         // 
         this.xtraTabbedMdiManager.MdiParent = this;
         // 
         // BaseShell
         // 
         this.defaultToolTipController.SetAllowHtmlText(this, DevExpress.Utils.DefaultBoolean.Default);
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(740, 698);
         this.IsMdiContainer = true;
         this.Name = "BaseShell";
         this.Text = "BaseShell";
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      protected DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager;
      protected DevExpress.Utils.DefaultToolTipController defaultToolTipController;
      protected DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel;
      protected DevExpress.XtraBars.Alerter.AlertControl alertControl;
   }
}