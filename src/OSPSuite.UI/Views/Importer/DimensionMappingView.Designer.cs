namespace OSPSuite.UI.Views.Importer
{
   partial class DimensionMappingView
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
            disposeBinders();
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
         this.dimensionSelectionGrid = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new OSPSuite.UI.Controls.UxGridView();
         this.uxLayoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.descriptionLabel = new DevExpress.XtraEditors.LabelControl();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dimensionSelectionGrid)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).BeginInit();
         this.uxLayoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         this.SuspendLayout();
         // 
         // dimensionSelectionGrid
         // 
         this.dimensionSelectionGrid.Location = new System.Drawing.Point(12, 29);
         this.dimensionSelectionGrid.MainView = this.gridView;
         this.dimensionSelectionGrid.Name = "dimensionSelectionGrid";
         this.dimensionSelectionGrid.Size = new System.Drawing.Size(556, 268);
         this.dimensionSelectionGrid.TabIndex = 39;
         this.dimensionSelectionGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.EnableColumnContextMenu = true;
         this.gridView.GridControl = this.dimensionSelectionGrid;
         this.gridView.MultiSelect = true;
         this.gridView.Name = "gridView";
         this.gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.gridView.OptionsNavigation.AutoFocusNewRow = true;
         this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.gridView.OptionsSelection.MultiSelect = true;
         this.gridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
         // 
         // uxLayoutControl1
         // 
         this.uxLayoutControl1.AllowCustomization = false;
         this.uxLayoutControl1.Controls.Add(this.descriptionLabel);
         this.uxLayoutControl1.Controls.Add(this.dimensionSelectionGrid);
         this.uxLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl1.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl1.Name = "uxLayoutControl1";
         this.uxLayoutControl1.Root = this.Root;
         this.uxLayoutControl1.Size = new System.Drawing.Size(580, 309);
         this.uxLayoutControl1.TabIndex = 41;
         this.uxLayoutControl1.Text = "uxLayoutControl1";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem3});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(580, 309);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.dimensionSelectionGrid;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 17);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(560, 272);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // descriptionLabel
         // 
         this.descriptionLabel.Location = new System.Drawing.Point(12, 12);
         this.descriptionLabel.Name = "descriptionLabel";
         this.descriptionLabel.Size = new System.Drawing.Size(63, 13);
         this.descriptionLabel.StyleController = this.uxLayoutControl1;
         this.descriptionLabel.TabIndex = 41;
         this.descriptionLabel.Text = "labelControl1";
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.descriptionLabel;
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(560, 17);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // DimensionMappingView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(580, 352);
         this.Controls.Add(this.uxLayoutControl1);
         this.Name = "DimensionMappingView";
         this.Controls.SetChildIndex(this.tablePanel, 0);
         this.Controls.SetChildIndex(this.uxLayoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dimensionSelectionGrid)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).EndInit();
         this.uxLayoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private Controls.UxGridControl dimensionSelectionGrid;
      private Controls.UxGridView gridView;
      private Controls.UxLayoutControl uxLayoutControl1;
      private DevExpress.XtraEditors.LabelControl descriptionLabel;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
   }
}
