using OSPSuite.UI.Controls;


namespace OSPSuite.Presentation.Importer.Views
{
   partial class ImporterTiledView : BaseUserControl, IImporterTiledView
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
         this.navigationTileBar = new DevExpress.XtraBars.Navigation.TileBar();
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.SuspendLayout();
         // 
         // navigationTileBar
         // 
         this.navigationTileBar.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
         this.navigationTileBar.Location = new System.Drawing.Point(12, 3);
         this.navigationTileBar.Name = "navigationTileBar";
         this.navigationTileBar.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
         this.navigationTileBar.Size = new System.Drawing.Size(1423, 143);
         this.navigationTileBar.TabIndex = 1;
         this.navigationTileBar.Text = "navigationTileBar";
         // 
         // panelControl1
         // 
         this.panelControl1.Location = new System.Drawing.Point(13, 153);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(1422, 704);
         this.panelControl1.TabIndex = 2;
         // 
         // ImporterTiledView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1447, 869);
         this.Controls.Add(this.panelControl1);
         this.Controls.Add(this.navigationTileBar);
         this.Name = "ImporterTiledView";
         this.Text = "ImporterTiledView";
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private DevExpress.XtraBars.Navigation.TileBar navigationTileBar;
      private DevExpress.XtraEditors.PanelControl panelControl1;
   }
}