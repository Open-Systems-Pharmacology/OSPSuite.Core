using DevExpress.XtraGrid;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   partial class ParameterIdentificationMatrixView
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
         this.matrixGridControl = new UxGridControl();
         this.matrixGridView = new UxGridView();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.legendGridControl = new UxGridControl();
         this.legendGridView = new UxGridView();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemMatrixGrid = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemLegendGrid = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.matrixGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.matrixGridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.legendGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.legendGridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMatrixGrid)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLegendGrid)).BeginInit();
         this.SuspendLayout();
         // 
         // matrixGridControl
         // 
         this.matrixGridControl.Location = new System.Drawing.Point(111, 26);
         this.matrixGridControl.MainView = this.matrixGridView;
         this.matrixGridControl.Name = "matrixGridControl";
         this.matrixGridControl.Size = new System.Drawing.Size(691, 379);
         this.matrixGridControl.TabIndex = 0;
         this.matrixGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.matrixGridView});
         // 
         // matrixGridView
         // 
         this.matrixGridView.AllowsFiltering = true;
         this.matrixGridView.EnableColumnContextMenu = true;
         this.matrixGridView.GridControl = this.matrixGridControl;
         this.matrixGridView.MultiSelect = false;
         this.matrixGridView.Name = "matrixGridView";
         this.matrixGridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         // 
         // layoutControl
         // 
         this.layoutControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
         this.layoutControl.Controls.Add(this.legendGridControl);
         this.layoutControl.Controls.Add(this.matrixGridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1135, 200, 895, 605);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(804, 407);
         this.layoutControl.TabIndex = 1;
         this.layoutControl.Text = "layoutControl1";
         // 
         // legendGridControl
         // 
         this.legendGridControl.Location = new System.Drawing.Point(111, 2);
         this.legendGridControl.MainView = this.legendGridView;
         this.legendGridControl.MaximumSize = new System.Drawing.Size(0, 20);
         this.legendGridControl.Name = "legendGridControl";
         this.legendGridControl.Size = new System.Drawing.Size(691, 20);
         this.legendGridControl.TabIndex = 3;
         this.legendGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.legendGridView});
         // 
         // legendGridView
         // 
         this.legendGridView.AllowsFiltering = true;
         this.legendGridView.EnableColumnContextMenu = true;
         this.legendGridView.GridControl = this.legendGridControl;
         this.legendGridView.MultiSelect = false;
         this.legendGridView.Name = "legendGridView";
         this.legendGridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemMatrixGrid,
            this.layoutItemLegendGrid});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(804, 407);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemMatrixGrid
         // 
         this.layoutItemMatrixGrid.Control = this.matrixGridControl;
         this.layoutItemMatrixGrid.Location = new System.Drawing.Point(0, 24);
         this.layoutItemMatrixGrid.MinSize = new System.Drawing.Size(213, 100);
         this.layoutItemMatrixGrid.Name = "layoutItemMatrixGrid";
         this.layoutItemMatrixGrid.Size = new System.Drawing.Size(804, 383);
         this.layoutItemMatrixGrid.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemMatrixGrid.TextSize = new System.Drawing.Size(106, 13);
         // 
         // layoutItemLegendGrid
         // 
         this.layoutItemLegendGrid.Control = this.legendGridControl;
         this.layoutItemLegendGrid.Location = new System.Drawing.Point(0, 0);
         this.layoutItemLegendGrid.Name = "layoutItemLegendGrid";
         this.layoutItemLegendGrid.Size = new System.Drawing.Size(804, 24);
         this.layoutItemLegendGrid.TextSize = new System.Drawing.Size(106, 13);
         // 
         // ParameterIdentificationMatrixView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.Controls.Add(this.layoutControl);
         this.Name = "ParameterIdentificationMatrixView";
         this.Size = new System.Drawing.Size(804, 407);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.matrixGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.matrixGridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.legendGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.legendGridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMatrixGrid)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLegendGrid)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private UxGridControl matrixGridControl;
      private UxGridView matrixGridView;
      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private UxGridControl legendGridControl;
      private UxGridView legendGridView;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemMatrixGrid;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemLegendGrid;
   }
}
