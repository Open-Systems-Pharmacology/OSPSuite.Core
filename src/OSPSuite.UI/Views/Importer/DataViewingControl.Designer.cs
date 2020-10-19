namespace OSPSuite.UI.Views.Importer
{
   partial class DataViewingControl
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
         this.dataViewingGridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.dataViewingGridView = new OSPSuite.UI.Controls.UxGridView();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingGridView)).BeginInit();
         this.SuspendLayout();
         // 
         // dataViewingGridControl
         // 
         this.dataViewingGridControl.Location = new System.Drawing.Point(16, 25);
         this.dataViewingGridControl.MainView = this.dataViewingGridView;
         this.dataViewingGridControl.Name = "dataViewingGridControl";
         this.dataViewingGridControl.Size = new System.Drawing.Size(1969, 1304);
         this.dataViewingGridControl.TabIndex = 0;
         this.dataViewingGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dataViewingGridView});
         this.dataViewingGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         // 
         // dataViewingGridView
         // 
         this.dataViewingGridView.GridControl = this.dataViewingGridControl;
         this.dataViewingGridView.OptionsView.ShowGroupPanel = false;
         this.dataViewingGridView.Name = "dataViewingGridView";
         this.dataViewingGridView.OptionsBehavior.Editable = false;
         // 
         // DataViewingControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "DataViewingControl";
         this.Controls.Add(this.dataViewingGridControl);
         this.Name = "DataViewingControl";
         this.Size = new System.Drawing.Size(2023, 1351);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingGridView)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private UI.Controls.UxGridControl dataViewingGridControl;
      private UI.Controls.UxGridView dataViewingGridView;
   }
}