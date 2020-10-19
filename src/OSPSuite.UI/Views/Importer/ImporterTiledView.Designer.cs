using DevExpress.XtraBars.Navigation;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Importer
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
         this.rootLayoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.centralPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.navigationTileBar = new DevExpress.XtraBars.Navigation.TileBar();
         this.navigationTileBarGroup = new DevExpress.XtraBars.Navigation.TileBarGroup();
         this.dataMappingTile = new DevExpress.XtraBars.Navigation.TileBarItem();
         this.confirmationTile = new DevExpress.XtraBars.Navigation.TileBarItem();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.tileBarLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.centralPanelControlLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.centralPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tileBarLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.centralPanelControlLayoutControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
         this.rootLayoutControl.Controls.Add(this.centralPanelControl);
         this.rootLayoutControl.Controls.Add(this.navigationTileBar);
         this.rootLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.rootLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.rootLayoutControl.Name = "rootLayoutControl";
         this.rootLayoutControl.Root = this.Root;
         this.rootLayoutControl.Size = new System.Drawing.Size(1271, 1216);
         this.rootLayoutControl.TabIndex = 0;
         // 
         // centralPanelControl
         // 
         this.centralPanelControl.Location = new System.Drawing.Point(12, 245);
         this.centralPanelControl.Name = "centralPanelControl";
         this.centralPanelControl.Size = new System.Drawing.Size(1247, 959);
         this.centralPanelControl.TabIndex = 5;
         // 
         // navigationTileBar
         // 
         this.navigationTileBar.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
         this.navigationTileBar.Groups.Add(this.navigationTileBarGroup);
         this.navigationTileBar.ItemSize = 140;
         this.navigationTileBar.Location = new System.Drawing.Point(12, 12);
         this.navigationTileBar.Name = "navigationTileBar";
         this.navigationTileBar.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
         this.navigationTileBar.Size = new System.Drawing.Size(1247, 229);
         this.navigationTileBar.TabIndex = 4;
         this.navigationTileBar.WideTileWidth = 300;
         // 
         // navigationTileBarGroup
         // 
         this.navigationTileBarGroup.Items.Add(this.dataMappingTile);
         this.navigationTileBarGroup.Items.Add(this.confirmationTile);
         this.navigationTileBarGroup.Name = "navigationTileBarGroup";
         // 
         // dataMappingTile
         // 
         this.dataMappingTile.DropDownOptions.BeakColor = System.Drawing.Color.Coral;
         tileItemElement1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
         tileItemElement1.ImageOptions.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Stretch;
         tileItemElement1.ImageOptions.ImageSize = new System.Drawing.Size(80, 80);
         tileItemElement1.Text = Captions.Importer.DataMapping;
         this.dataMappingTile.Elements.Add(tileItemElement1);
         this.dataMappingTile.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
         this.dataMappingTile.Name = "dataMappingTile";
         // 
         // confirmationTile
         // 
         this.confirmationTile.DropDownOptions.BeakColor = System.Drawing.Color.Blue;
         tileItemElement2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
         tileItemElement2.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.TopLeft;
         tileItemElement2.ImageOptions.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Stretch;
         tileItemElement2.ImageOptions.ImageSize = new System.Drawing.Size(80, 80);
         tileItemElement2.Text = Captions.Importer.Confirmation;
         this.confirmationTile.Elements.Add(tileItemElement2);
         this.confirmationTile.Enabled = false;
         this.confirmationTile.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
         this.confirmationTile.Name = "confirmationTile";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.tileBarLayoutControlItem,
            this.centralPanelControlLayoutControlItem});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1271, 1216);
         this.Root.TextVisible = false;
         // 
         // tileBarLayoutControlItem
         // 
         this.tileBarLayoutControlItem.Control = this.navigationTileBar;
         this.tileBarLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.tileBarLayoutControlItem.Name = "tileBarLayoutControlItem";
         this.tileBarLayoutControlItem.Size = new System.Drawing.Size(1251, 233);
         this.tileBarLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.tileBarLayoutControlItem.TextVisible = false;
         // 
         // centralPanelControlLayoutControlItem
         // 
         this.centralPanelControlLayoutControlItem.Control = this.centralPanelControl;
         this.centralPanelControlLayoutControlItem.Location = new System.Drawing.Point(0, 233);
         this.centralPanelControlLayoutControlItem.Name = "centralPanelControlLayoutControlItem";
         this.centralPanelControlLayoutControlItem.Size = new System.Drawing.Size(1251, 963);
         this.centralPanelControlLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.centralPanelControlLayoutControlItem.TextVisible = false;
         // 
         // ImporterTiledView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.rootLayoutControl);
         this.Name = "ImporterTiledView";
         this.Size = new System.Drawing.Size(1271, 1216);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).EndInit();
         this.rootLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.centralPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tileBarLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.centralPanelControlLayoutControlItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl rootLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.PanelControl centralPanelControl;
      private TileBar navigationTileBar;
      private TileBarGroup navigationTileBarGroup;
      private TileBarItem dataMappingTile;
      private TileBarItem confirmationTile;
      private DevExpress.XtraLayout.LayoutControlItem tileBarLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem centralPanelControlLayoutControlItem;
   }
}