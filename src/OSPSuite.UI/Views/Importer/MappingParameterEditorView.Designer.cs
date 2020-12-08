using OSPSuite.Assets;

namespace OSPSuite.UI.Views.Importer
{
   partial class MappingParameterEditorView
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
         this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.errorTypePanelControl = new DevExpress.XtraEditors.PanelControl();
         this.lloqPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.unitsPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.unitsLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.lloqLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.errorTypeLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.errorTypePanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.lloqPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.unitsPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.unitsLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.lloqLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.errorTypeLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.errorTypePanelControl);
         this.layoutControl.Controls.Add(this.lloqPanelControl);
         this.layoutControl.Controls.Add(this.unitsPanelControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(6);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(538, 125);
         this.layoutControl.TabIndex = 38;
         // 
         // errorTypePanelControl
         // 
         this.errorTypePanelControl.Location = new System.Drawing.Point(72, 76);
         this.errorTypePanelControl.Name = "errorTypePanelControl";
         this.errorTypePanelControl.Size = new System.Drawing.Size(454, 27);
         this.errorTypePanelControl.TabIndex = 7;
         // 
         // lloqPanelControl
         // 
         this.lloqPanelControl.Location = new System.Drawing.Point(72, 46);
         this.lloqPanelControl.Name = "lloqPanelControl";
         this.lloqPanelControl.Size = new System.Drawing.Size(454, 26);
         this.lloqPanelControl.TabIndex = 6;
         // 
         // unitsPanelControl
         // 
         this.unitsPanelControl.Location = new System.Drawing.Point(72, 12);
         this.unitsPanelControl.Margin = new System.Windows.Forms.Padding(0);
         this.unitsPanelControl.Name = "unitsPanelControl";
         this.unitsPanelControl.Size = new System.Drawing.Size(454, 30);
         this.unitsPanelControl.TabIndex = 5;
         // 
         // Root
         // 
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.unitsLayoutControlItem,
            this.lloqLayoutControlItem,
            this.errorTypeLayoutControlItem,
            this.emptySpaceItem1});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(538, 125);
         this.Root.TextVisible = false;
         // 
         // unitsLayoutControlItem
         // 
         this.unitsLayoutControlItem.Control = this.unitsPanelControl;
         this.unitsLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.unitsLayoutControlItem.Name = "Units";
         this.unitsLayoutControlItem.Size = new System.Drawing.Size(518, 34);
         this.unitsLayoutControlItem.TextSize = new System.Drawing.Size(57, 16);
         // 
         // lloqLayoutControlItem
         // 
         this.lloqLayoutControlItem.Control = this.lloqPanelControl;
         this.lloqLayoutControlItem.Location = new System.Drawing.Point(0, 34);
         this.lloqLayoutControlItem.Name = "Lloq";
         this.lloqLayoutControlItem.Size = new System.Drawing.Size(518, 30);
         this.lloqLayoutControlItem.TextSize = new System.Drawing.Size(57, 16);
         // 
         // errorTypeLayoutControlItem
         // 
         this.errorTypeLayoutControlItem.Control = this.errorTypePanelControl;
         this.errorTypeLayoutControlItem.Location = new System.Drawing.Point(0, 64);
         this.errorTypeLayoutControlItem.Name = "Error type";
         this.errorTypeLayoutControlItem.Size = new System.Drawing.Size(518, 31);
         this.errorTypeLayoutControlItem.TextSize = new System.Drawing.Size(57, 16);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 95);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(518, 10);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // MappingParameterEditorView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Margin = new System.Windows.Forms.Padding(8);
         this.Name = "MappingParameterEditorView";
         this.Size = new System.Drawing.Size(538, 125);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.errorTypePanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.lloqPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.unitsPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.unitsLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.lloqLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.errorTypeLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.PanelControl errorTypePanelControl;
      private DevExpress.XtraEditors.PanelControl lloqPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem lloqLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem errorTypeLayoutControlItem;
      private DevExpress.XtraEditors.PanelControl unitsPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem unitsLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
