using OSPSuite.UI.Controls;

namespace OSPSuite.Starter.Views
{
   partial class PivotGridTestView
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
         this.pivotGridControl = new UxPivotGrid();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl)).BeginInit();
         this.SuspendLayout();
         // 
         // pivotGridControl1
         // 
         this.pivotGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.pivotGridControl.Location = new System.Drawing.Point(0, 0);
         this.pivotGridControl.Name = "pivotGridControl";
         this.pivotGridControl.Size = new System.Drawing.Size(591, 375);
         this.pivotGridControl.TabIndex = 0;
         // 
         // PivotGridTestView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.pivotGridControl);
         this.Name = "PivotGridTestView";
         this.Size = new System.Drawing.Size(591, 375);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private UxPivotGrid pivotGridControl;
   }
}
