using System.Windows.Forms;
using OSPSuite.Assets;

namespace OSPSuite.UI.Views.Importer
{
   partial class ImporterReloadView
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
         this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.newListBoxControl = new DevExpress.XtraEditors.ListBoxControl();
         this.overwrittenListBoxControl = new DevExpress.XtraEditors.ListBoxControl();
         this.deletedListBoxControl = new DevExpress.XtraEditors.ListBoxControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.newListBoxControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.overwrittenListBoxControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.deletedListBoxControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.labelControl1);
         this.layoutControl1.Controls.Add(this.newListBoxControl);
         this.layoutControl1.Controls.Add(this.overwrittenListBoxControl);
         this.layoutControl1.Controls.Add(this.deletedListBoxControl);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.Root;
         this.layoutControl1.Size = new System.Drawing.Size(1453, 713);
         this.layoutControl1.TabIndex = 38;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // labelControl1
         // 
         this.labelControl1.Location = new System.Drawing.Point(12, 12);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(1073, 33);
         this.labelControl1.StyleController = this.layoutControl1;
         this.labelControl1.TabIndex = 7;
         this.labelControl1.Text = Captions.Importer.ReloadWillCauseChangeOfDataSets;
         // 
         // newListBoxControl
         // 
         this.newListBoxControl.Location = new System.Drawing.Point(1009, 103);
         this.newListBoxControl.Name = "newListBoxControl";
         this.newListBoxControl.Size = new System.Drawing.Size(432, 598);
         this.newListBoxControl.StyleController = this.layoutControl1;
         this.newListBoxControl.TabIndex = 6;
         // 
         // overwrittenListBoxControl
         // 
         this.overwrittenListBoxControl.Location = new System.Drawing.Point(489, 103);
         this.overwrittenListBoxControl.Name = "overwrittenListBoxControl";
         this.overwrittenListBoxControl.Size = new System.Drawing.Size(516, 598);
         this.overwrittenListBoxControl.StyleController = this.layoutControl1;
         this.overwrittenListBoxControl.TabIndex = 5;
         // 
         // deletedListBoxControl
         // 
         this.deletedListBoxControl.Location = new System.Drawing.Point(12, 103);
         this.deletedListBoxControl.Name = "deletedListBoxControl";
         this.deletedListBoxControl.Size = new System.Drawing.Size(473, 598);
         this.deletedListBoxControl.StyleController = this.layoutControl1;
         this.deletedListBoxControl.TabIndex = 4;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.emptySpaceItem1});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1453, 713);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.deletedListBoxControl;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 55);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(477, 638);
         this.layoutControlItem1.Text = "Datasets that will be deleted";
         this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutControlItem1.TextSize = new System.Drawing.Size(412, 33);
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.overwrittenListBoxControl;
         this.layoutControlItem2.Location = new System.Drawing.Point(477, 55);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(520, 638);
         this.layoutControlItem2.Text = Captions.Importer.DataSetsWillBeOverwritten;
         this.layoutControlItem2.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutControlItem2.TextSize = new System.Drawing.Size(412, 33);
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.newListBoxControl;
         this.layoutControlItem3.Location = new System.Drawing.Point(997, 55);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(436, 638);
         this.layoutControlItem3.Text = Captions.Importer.NewDataStetsWillBeImported;
         this.layoutControlItem3.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutControlItem3.TextSize = new System.Drawing.Size(412, 33);
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.labelControl1;
         this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Size = new System.Drawing.Size(1433, 37);
         this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem4.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 37);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(1433, 18);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // ImporterReloadView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = Captions.Importer.ReloadData;
         this.ClientSize = new System.Drawing.Size(1453, 830);
         this.Controls.Add(this.layoutControl1);
         this.Name = "ImporterReloadView";
         this.Text = Captions.Importer.ReloadData;
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.newListBoxControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.overwrittenListBoxControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.deletedListBoxControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraEditors.ListBoxControl newListBoxControl;
      private DevExpress.XtraEditors.ListBoxControl overwrittenListBoxControl;
      private DevExpress.XtraEditors.ListBoxControl deletedListBoxControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}