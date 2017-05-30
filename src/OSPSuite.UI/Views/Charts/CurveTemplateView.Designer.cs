
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Charts
{
   partial class CurveTemplateView
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
         _gridViewBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.gridControl = new UxGridControl();
         this.mainView = new UxGridView();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.mainView)).BeginInit();
         this.SuspendLayout();

         // 
         // gridControl
         // 
         this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gridControl.Location = new System.Drawing.Point(0, 0);
         this.gridControl.MainView = this.mainView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(634, 447);
         this.gridControl.TabIndex = 0;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.mainView});
         // 
         // mainView
         // 
         this.mainView.GridControl = this.gridControl;
         this.mainView.Name = "mainView";
         // 
         // CurveTemplateView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.gridControl);
         this.Name = "CurveTemplateView";
         this.Size = new System.Drawing.Size(634, 447);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.mainView)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private UxGridControl gridControl;
      private UxGridView mainView;
   }
}
