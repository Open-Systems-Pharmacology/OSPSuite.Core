using DevExpress.XtraBars.Docking;
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
         DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
         DevExpress.XtraEditors.TileItemElement tileItemElement3 = new DevExpress.XtraEditors.TileItemElement();
         this.navigationTileBar = new DevExpress.XtraBars.Navigation.TileBar();
         this.tileBarGroup2 = new DevExpress.XtraBars.Navigation.TileBarGroup();
         this.tileBarItem1 = new DevExpress.XtraBars.Navigation.TileBarItem();
         this.tileBarItem2 = new DevExpress.XtraBars.Navigation.TileBarItem();
         this.tileBarItem3 = new DevExpress.XtraBars.Navigation.TileBarItem();
         this.centralPanelControl = new DevExpress.XtraEditors.PanelControl();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.centralPanelControl)).BeginInit();
         this.SuspendLayout();
         // 
         // navigationTileBar
         // 
         this.navigationTileBar.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
         this.navigationTileBar.Groups.Add(this.tileBarGroup2);
         this.navigationTileBar.Location = new System.Drawing.Point(13, 17);
         this.navigationTileBar.MaxId = 3;
         this.navigationTileBar.Name = "navigationTileBar";
         this.navigationTileBar.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
         this.navigationTileBar.Size = new System.Drawing.Size(2528, 130);
         this.navigationTileBar.TabIndex = 1;
         this.navigationTileBar.Text = "navigationTileBar";
         // 
         // tileBarGroup2
         // 
         this.tileBarGroup2.Items.Add(this.tileBarItem1);
         this.tileBarGroup2.Items.Add(this.tileBarItem2);
         this.tileBarGroup2.Items.Add(this.tileBarItem3);
         this.tileBarGroup2.Name = "tileBarGroup2";
         // 
         // tileBarItem1
         // 
         this.tileBarItem1.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
         tileItemElement1.Text = "tileBarItem1";
         this.tileBarItem1.Elements.Add(tileItemElement1);
         this.tileBarItem1.Id = 0;
         this.tileBarItem1.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Medium;
         this.tileBarItem1.Name = "tileBarItem1";
         // 
         // tileBarItem2
         // 
         this.tileBarItem2.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
         tileItemElement2.Text = "tileBarItem2";
         this.tileBarItem2.Elements.Add(tileItemElement2);
         this.tileBarItem2.Id = 1;
         this.tileBarItem2.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
         this.tileBarItem2.Name = "tileBarItem2";
         // 
         // tileBarItem3
         // 
         this.tileBarItem3.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
         tileItemElement3.Text = "tileBarItem3";
         this.tileBarItem3.Elements.Add(tileItemElement3);
         this.tileBarItem3.Id = 2;
         this.tileBarItem3.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Medium;
         this.tileBarItem3.Name = "tileBarItem3";
         // 
         // centralPanelControl
         // 
         this.centralPanelControl.Location = new System.Drawing.Point(13, 153);
         this.centralPanelControl.Name = "centralPanelControl";
         this.centralPanelControl.Size = new System.Drawing.Size(2528, 1125);
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
         this.Size = new System.Drawing.Size(2544, 1290);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.centralPanelControl)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private DevExpress.XtraBars.Navigation.TileBar navigationTileBar;
      private DevExpress.XtraEditors.PanelControl centralPanelControl;
      private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroup2;
      private DevExpress.XtraBars.Navigation.TileBarItem tileBarItem1;
      private DevExpress.XtraBars.Navigation.TileBarItem tileBarItem2;
      private DevExpress.XtraBars.Navigation.TileBarItem tileBarItem3;
   }
}