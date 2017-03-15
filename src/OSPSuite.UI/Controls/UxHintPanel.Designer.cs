namespace OSPSuite.UI.Controls
{
   partial class UxHintPanel
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
         this.groupControl = new DevExpress.XtraEditors.GroupControl();
         this.panelNote = new DevExpress.Utils.Frames.NotePanel8_1();
         ((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
         this.groupControl.SuspendLayout();
         this.SuspendLayout();
         // 
         // groupControl
         // 
         this.groupControl.Controls.Add(this.panelNote);
         this.groupControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.groupControl.Location = new System.Drawing.Point(0, 0);
         this.groupControl.Name = "groupControl";
         this.groupControl.ShowCaption = false;
         this.groupControl.Size = new System.Drawing.Size(518, 40);
         this.groupControl.TabIndex = 0;
         this.groupControl.Text = "groupControl1";
         // 
         // panelNote
         // 
         this.panelNote.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelNote.Location = new System.Drawing.Point(2, 2);
         this.panelNote.Name = "panelNote";
         this.panelNote.Size = new System.Drawing.Size(514, 36);
         this.panelNote.TabIndex = 0;
         this.panelNote.TabStop = false;
         // 
         // UxHintPanel
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.groupControl);
         this.MaximumSize = new System.Drawing.Size(1000000, 40);
         this.MinimumSize = new System.Drawing.Size(200, 40);
         this.Name = "UxHintPanel";
         this.Size = new System.Drawing.Size(518, 40);
         ((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
         this.groupControl.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.GroupControl groupControl;
      private DevExpress.Utils.Frames.NotePanel8_1 panelNote;
   }
}
