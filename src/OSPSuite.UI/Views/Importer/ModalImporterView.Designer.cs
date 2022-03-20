using OSPSuite.Assets;

namespace OSPSuite.UI.Views.Importer
{
   partial class ModalImporterView
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
         this.importerPanelControl = new DevExpress.XtraEditors.PanelControl();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerPanelControl)).BeginInit();
         this.SuspendLayout();
         this.Text = Captions.Importer.Title;
         // 
         // panelControl
         // 
         this.importerPanelControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.importerPanelControl.Location = new System.Drawing.Point(0, 0);
         this.importerPanelControl.Name = "panelControl";
         this.importerPanelControl.Size = new System.Drawing.Size(1050, 602);
         this.importerPanelControl.TabIndex = 38;
         // 
         // ModalImporterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1050, 648);
         this.Controls.Add(this.importerPanelControl);
         this.Name = "ModalImporterView";
         this.Controls.SetChildIndex(this.importerPanelControl, 0);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerPanelControl)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl importerPanelControl;
   }
}
