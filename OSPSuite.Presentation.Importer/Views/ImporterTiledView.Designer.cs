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
         DevExpress.XtraEditors.TileItemElement tileItemElement4 = new DevExpress.XtraEditors.TileItemElement();
         DevExpress.XtraEditors.TileItemElement tileItemElement5 = new DevExpress.XtraEditors.TileItemElement();
         DevExpress.XtraEditors.TileItemElement tileItemElement6 = new DevExpress.XtraEditors.TileItemElement();
         this.navigationTileBar = new DevExpress.XtraBars.Navigation.TileBar();
         this.tileBarGroup2 = new DevExpress.XtraBars.Navigation.TileBarGroup();
         this.tileBarItem1 = new DevExpress.XtraBars.Navigation.TileBarItem();
         this.tileBarItem2 = new DevExpress.XtraBars.Navigation.TileBarItem();
         this.tileBarItem3 = new DevExpress.XtraBars.Navigation.TileBarItem();
         this.centralPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
         this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.centralPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
         this.dataLayoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         this.SuspendLayout();
         // 
         // navigationTileBar
         // 
         this.navigationTileBar.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
         this.navigationTileBar.Groups.Add(this.tileBarGroup2);
         this.navigationTileBar.Location = new System.Drawing.Point(244, 12);
         this.navigationTileBar.MaxId = 3;
         this.navigationTileBar.Name = "navigationTileBar";
         this.navigationTileBar.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
         this.navigationTileBar.Size = new System.Drawing.Size(100, 33);
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
         tileItemElement4.Text = "Source File";
         this.tileBarItem1.Elements.Add(tileItemElement4);
         this.tileBarItem1.Enabled = false;
         this.tileBarItem1.Id = 0;
         this.tileBarItem1.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
         this.tileBarItem1.Name = "tileBarItem1";
         // 
         // tileBarItem2
         // 
         this.tileBarItem2.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
         tileItemElement5.Text = "Import Configuration";
         this.tileBarItem2.Elements.Add(tileItemElement5);
         this.tileBarItem2.Id = 1;
         this.tileBarItem2.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
         this.tileBarItem2.Name = "tileBarItem2";
         // 
         // tileBarItem3
         // 
         this.tileBarItem3.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
         tileItemElement6.Text = "Confirmation";
         tileItemElement6.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleLeft;
         this.tileBarItem3.Elements.Add(tileItemElement6);
         this.tileBarItem3.Id = 2;
         this.tileBarItem3.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
         this.tileBarItem3.Name = "tileBarItem3";
         // 
         // centralPanelControl
         // 
         this.centralPanelControl.Location = new System.Drawing.Point(12, 12);
         this.centralPanelControl.Name = "centralPanelControl";
         this.centralPanelControl.Size = new System.Drawing.Size(694, 336);
         this.centralPanelControl.TabIndex = 2;
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.layoutControl2);
         this.layoutControl1.Controls.Add(this.navigationTileBar);
         this.layoutControl1.Location = new System.Drawing.Point(158, 56);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.Root;
         this.layoutControl1.Size = new System.Drawing.Size(180, 120);
         this.layoutControl1.TabIndex = 3;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(356, 81);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.navigationTileBar;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(336, 37);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(229, 33);
         // 
         // layoutControl2
         // 
         this.layoutControl2.Location = new System.Drawing.Point(12, 49);
         this.layoutControl2.Name = "layoutControl2";
         this.layoutControl2.Root = this.layoutControlGroup1;
         this.layoutControl2.Size = new System.Drawing.Size(332, 20);
         this.layoutControl2.TabIndex = 4;
         this.layoutControl2.Text = "layoutControl2";
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.layoutControl2;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 37);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(336, 24);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(332, 20);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // dataLayoutControl1
         // 
         this.dataLayoutControl1.Controls.Add(this.centralPanelControl);
         this.dataLayoutControl1.Location = new System.Drawing.Point(144, 424);
         this.dataLayoutControl1.Name = "dataLayoutControl1";
         this.dataLayoutControl1.Root = this.layoutControlGroup2;
         this.dataLayoutControl1.Size = new System.Drawing.Size(718, 360);
         this.dataLayoutControl1.TabIndex = 4;
         this.dataLayoutControl1.Text = "dataLayoutControl1";
         // 
         // layoutControlGroup2
         // 
         this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup2.GroupBordersVisible = false;
         this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3});
         this.layoutControlGroup2.Name = "layoutControlGroup2";
         this.layoutControlGroup2.Size = new System.Drawing.Size(718, 360);
         this.layoutControlGroup2.TextVisible = false;
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.centralPanelControl;
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(698, 340);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // ImporterTiledView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "ImporterTiledView";
         this.Controls.Add(this.dataLayoutControl1);
         this.Controls.Add(this.layoutControl1);
         this.Name = "ImporterTiledView";
         this.Size = new System.Drawing.Size(927, 795);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.centralPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
         this.dataLayoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private DevExpress.XtraBars.Navigation.TileBar navigationTileBar;
      private DevExpress.XtraEditors.PanelControl centralPanelControl;
      private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroup2;
      private DevExpress.XtraBars.Navigation.TileBarItem tileBarItem1;
      private DevExpress.XtraBars.Navigation.TileBarItem tileBarItem2;
      private DevExpress.XtraBars.Navigation.TileBarItem tileBarItem3;
      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControl layoutControl2;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraDataLayout.DataLayoutControl dataLayoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
   }
}