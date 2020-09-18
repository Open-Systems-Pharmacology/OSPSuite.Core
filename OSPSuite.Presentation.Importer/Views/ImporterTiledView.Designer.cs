using DevExpress.XtraBars.Navigation;
using DevExpress.XtraRichEdit.Model;
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
         DevExpress.XtraEditors.TileItemElement tileItemElement1 = new DevExpress.XtraEditors.TileItemElement();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImporterTiledView));
         DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
         this.navigationTileBar = new DevExpress.XtraBars.Navigation.TileBar();
         this.group1 = new DevExpress.XtraBars.Navigation.TileBarGroup();
         this.dataMappingTile = new DevExpress.XtraBars.Navigation.TileBarItem();
         this.confirmationTile = new DevExpress.XtraBars.Navigation.TileBarItem();
         this.centralPanelControl = new DevExpress.XtraEditors.PanelControl();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.centralPanelControl)).BeginInit();
         this.SuspendLayout();
         // 
         // navigationTileBar
         // 
         this.navigationTileBar.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
         this.navigationTileBar.Groups.Add(this.group1);
         this.navigationTileBar.ItemSize = 140;
         this.navigationTileBar.Location = new System.Drawing.Point(12, 3);
         this.navigationTileBar.Name = "navigationTileBar";
         this.navigationTileBar.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
         this.navigationTileBar.Size = new System.Drawing.Size(4371, 271);
         this.navigationTileBar.TabIndex = 1;
         this.navigationTileBar.Text = "navigationTileBar";
         this.navigationTileBar.WideTileWidth = 300;
         //this.navigationTileBar.AllowSelectedItem = true;
         // 
         // group1
         // 
         this.group1.Items.Add(this.dataMappingTile);
         this.group1.Items.Add(this.confirmationTile);
         this.group1.Name = "group1";
         // 
         // dataMappingTile
         // 
         this.dataMappingTile.DropDownOptions.BeakColor = System.Drawing.Color.Coral;
         tileItemElement1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
         tileItemElement1.Text = "Data Mapping";
         this.dataMappingTile.Elements.Add(tileItemElement1);
         this.dataMappingTile.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
         this.dataMappingTile.Name = "dataMappingTile";
         // 
         // confirmationTile
         // 
         this.confirmationTile.DropDownOptions.BeakColor = System.Drawing.Color.Blue;
         tileItemElement2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
         tileItemElement2.Text = "Confirmation";
         this.confirmationTile.Elements.Add(tileItemElement2);
         this.confirmationTile.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
         this.confirmationTile.Name = "confirmationTile";
         // 
         // centralPanelControl
         // 
         this.centralPanelControl.Location = new System.Drawing.Point(13, 280);
         this.centralPanelControl.Name = "centralPanelControl";
         this.centralPanelControl.Size = new System.Drawing.Size(4370, 2426);
         this.centralPanelControl.TabIndex = 2;
         // 
         // ImporterTiledView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "ImporterTiledView";
         this.Controls.Add(this.centralPanelControl);
         this.Controls.Add(this.navigationTileBar);
         this.Name = "ImporterTiledView";
         this.Size = new System.Drawing.Size(4395, 2722);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.centralPanelControl)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private DevExpress.XtraBars.Navigation.TileBar navigationTileBar;
      private DevExpress.XtraEditors.PanelControl centralPanelControl;
      private TileBarGroup group1;
      private TileBarItem dataMappingTile;
      private TileBarItem confirmationTile;
   }
}