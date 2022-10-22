
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views
{
   partial class SimulationOutputMappingView
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
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.gridControl = new UxGridControl();
         this.gridView = new UxGridView();
         this.layoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Location = new System.Drawing.Point(19, 22);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(1919, 1634);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1919, 1634);
         this.Root.TextVisible = false;
         // 
         // gridControl
         // 
         this.gridControl.Location = new System.Drawing.Point(12, 12);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(1895, 1610);
         this.gridControl.TabIndex = 4;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.GridControl = this.gridControl;
         this.gridView.Name = "gridView";
         // 
         // layoutControlItem
         // 
         this.layoutControlItem.Control = this.gridControl;
         this.layoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem.Name = "layoutControlItem";
         this.layoutControlItem.Size = new System.Drawing.Size(1899, 1614);
         this.layoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem.TextVisible = false;
         // 
         // SimulationOutputMappingView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "SimulationOutputMappingView";
         this.Size = new System.Drawing.Size(1972, 1672);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private UxGridControl gridControl;
      private UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem;
   }
}
