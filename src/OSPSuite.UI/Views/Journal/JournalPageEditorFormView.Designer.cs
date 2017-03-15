namespace OSPSuite.UI.Views.Journal
{
   partial class JournalPageEditorFormView
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
         this.panelControl = new DevExpress.XtraEditors.PanelControl();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
         this.SuspendLayout();
         // 
         // panelControl
         // 
         this.panelControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl.Location = new System.Drawing.Point(0, 0);
         this.panelControl.Name = "panelControl";
         this.panelControl.Size = new System.Drawing.Size(570, 403);
         this.panelControl.TabIndex = 0;
         // 
         // WorkingJournalItemEditorForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(570, 403);
         this.Controls.Add(this.panelControl);
         this.Name = "JournalPageEditorFormView";
         this.Text = "WorkingJournalItemEditorForm";
         ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl;
   }
}