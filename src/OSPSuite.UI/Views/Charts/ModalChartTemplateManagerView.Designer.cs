namespace OSPSuite.UI.Views.Charts
{
   partial class ModalChartTemplateManagerView
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
         this.panelControl = new DevExpress.XtraEditors.PanelControl();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
         this.SuspendLayout();
         // 
         // panelControl
         // 
         this.panelControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl.Location = new System.Drawing.Point(0, 0);
         this.panelControl.Name = "panelControl";
         this.panelControl.Size = new System.Drawing.Size(1050, 602);
         this.panelControl.TabIndex = 38;
         // 
         // ModalChartTemplateManagerView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1050, 648);
         this.Controls.Add(this.panelControl);
         this.Name = "ModalChartTemplateManagerView";
         this.Controls.SetChildIndex(this.panelControl, 0);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl;
   }
}
