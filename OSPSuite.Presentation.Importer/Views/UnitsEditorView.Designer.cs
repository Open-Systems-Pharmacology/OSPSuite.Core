namespace OSPSuite.Presentation.Importer.Views
{
   partial class UnitsEditorView
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
         this.panel1 = new System.Windows.Forms.Panel();
         this.btnOK = new DevExpress.XtraEditors.SimpleButton();
         this.btnAllOK = new DevExpress.XtraEditors.SimpleButton();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.Location = new System.Drawing.Point(7, 24);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(482, 85);
         this.panel1.TabIndex = 0;
         // 
         // btnOK
         // 
         this.btnOK.Location = new System.Drawing.Point(373, 120);
         this.btnOK.Name = "btnOK";
         this.btnOK.Size = new System.Drawing.Size(102, 31);
         this.btnOK.TabIndex = 2;
         this.btnOK.Text = "OK";
         // 
         // btnAllOK
         // 
         this.btnAllOK.Location = new System.Drawing.Point(263, 120);
         this.btnAllOK.Name = "btnAllOK";
         this.btnAllOK.Size = new System.Drawing.Size(102, 31);
         this.btnAllOK.TabIndex = 3;
         this.btnAllOK.Text = "OK to All";
         // 
         // UnitsEditorView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.btnAllOK);
         this.Controls.Add(this.btnOK);
         this.Controls.Add(this.panel1);
         this.Name = "UnitsEditorView";
         this.Size = new System.Drawing.Size(500, 168);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Panel panel1;
      private DevExpress.XtraEditors.SimpleButton btnOK;
      private DevExpress.XtraEditors.SimpleButton btnAllOK;
   }
}
