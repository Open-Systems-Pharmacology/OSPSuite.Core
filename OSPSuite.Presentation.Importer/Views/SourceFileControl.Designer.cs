namespace OSPSuite.Presentation.Importer.Views
{
   partial class SourceFileControl
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
         this.fileLabelControl = new DevExpress.XtraEditors.LabelControl();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         this.SuspendLayout();
         // 
         // fileLabelControl
         // 
         this.fileLabelControl.Location = new System.Drawing.Point(36, 54);
         this.fileLabelControl.Name = "fileLabelControl";
         this.fileLabelControl.Size = new System.Drawing.Size(156, 33);
         this.fileLabelControl.TabIndex = 0;
         this.fileLabelControl.Text = "File: ";
         // 
         // SourceFileControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.fileLabelControl);
         this.Name = "SourceFileControl";
         this.Size = new System.Drawing.Size(2589, 144);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.LabelControl fileLabelControl;
   }
}
