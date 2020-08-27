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
         this.openSourceFileButton = new DevExpress.XtraEditors.SimpleButton();
         this.sourceFileTextEdit = new DevExpress.XtraEditors.TextEdit();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileTextEdit.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // fileLabelControl
         // 
         this.fileLabelControl.Location = new System.Drawing.Point(35, 27);
         this.fileLabelControl.Name = "fileLabelControl";
         this.fileLabelControl.Size = new System.Drawing.Size(58, 33);
         this.fileLabelControl.TabIndex = 0;
         this.fileLabelControl.Text = "File: ";
         // 
         // openSourceFileButton
         // 
         this.openSourceFileButton.Location = new System.Drawing.Point(2494, 27);
         this.openSourceFileButton.Name = "openSourceFileButton";
         this.openSourceFileButton.Size = new System.Drawing.Size(60, 46);
         this.openSourceFileButton.TabIndex = 1;
         this.openSourceFileButton.Text = "...";
         // 
         // sourceFileTextEdit
         // 
         this.sourceFileTextEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left));
         this.sourceFileTextEdit.Location = new System.Drawing.Point(208, 22);
         this.sourceFileTextEdit.Margin = new System.Windows.Forms.Padding(8);
         this.sourceFileTextEdit.Properties.ReadOnly = true;
         this.sourceFileTextEdit.Name = "sourceFileTextEdit";
         this.sourceFileTextEdit.Size = new System.Drawing.Size(2229, 50);
         this.sourceFileTextEdit.TabIndex = 2;

         // 
         // SourceFileControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.sourceFileTextEdit);
         this.Controls.Add(this.openSourceFileButton);
         this.Controls.Add(this.fileLabelControl);
         this.Name = "SourceFileControl";
         this.Size = new System.Drawing.Size(2589, 105);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileTextEdit.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.LabelControl fileLabelControl;
      private DevExpress.XtraEditors.SimpleButton openSourceFileButton;
      private DevExpress.XtraEditors.TextEdit sourceFileTextEdit;
   }
}
