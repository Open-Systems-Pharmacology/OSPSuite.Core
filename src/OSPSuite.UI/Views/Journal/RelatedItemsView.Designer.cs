namespace OSPSuite.UI.Views.Journal
{
   partial class RelatedItemsView
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
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGrid)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutItemGrid
         // 
         this.layoutItemGrid.MinSize = new System.Drawing.Size(1, 1);
         this.layoutItemGrid.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         // 
         // RelatedItemsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Name = "RelatedItemsView";
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGrid)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
   }
}
