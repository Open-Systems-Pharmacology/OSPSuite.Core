using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views
{
   partial class HeavyWorkCancellableView
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
         this.progressBar = new DevExpress.XtraEditors.MarqueeProgressBarControl();
         this.btnCancel = new OSPSuite.UI.Controls.UxSimpleButton();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // progressBar
         // 
         this.progressBar.EditValue = 0;
         this.progressBar.Location = new System.Drawing.Point(0, 0);
         this.progressBar.Margin = new System.Windows.Forms.Padding(4);
         this.progressBar.Name = "progressBar";
         this.progressBar.Size = new System.Drawing.Size(168, 26);
         this.progressBar.TabIndex = 1;
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(72, 37);
         this.btnCancel.Manager = null;
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Shortcut = System.Windows.Forms.Keys.None;
         this.btnCancel.Size = new System.Drawing.Size(94, 29);
         this.btnCancel.TabIndex = 2;
         this.btnCancel.Text = "simpleButton1";
         // 
         // HeavyWorkCancellableView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "HeavyWorkView";
         this.ClientSize = new System.Drawing.Size(168, 69);
         this.Controls.Add(this.btnCancel);
         this.Controls.Add(this.progressBar);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Margin = new System.Windows.Forms.Padding(5);
         this.MaximumSize = new System.Drawing.Size(168, 80);
         this.Name = "HeavyWorkCancellableView";
         this.Text = "HeavyWorkView";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.MarqueeProgressBarControl progressBar;
      private UxSimpleButton btnCancel;
   }
}