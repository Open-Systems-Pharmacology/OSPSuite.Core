using OSPSuite.Assets;

namespace OSPSuite.UI.Views.Importer
{
   partial class ColumnMappingControl
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
         _gridViewBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.columnMappingGrid = new OSPSuite.UI.Controls.UxGridControl();
         this.columnMappingGridView = new OSPSuite.UI.Controls.UxGridView();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingGrid)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingGridView)).BeginInit();
         this.SuspendLayout();
         // 
         // columnMappingGrid
         // 
         this.columnMappingGrid.Dock = System.Windows.Forms.DockStyle.Fill;
         this.columnMappingGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.columnMappingGrid.Location = new System.Drawing.Point(0, 0);
         this.columnMappingGrid.MainView = this.columnMappingGridView;
         this.columnMappingGrid.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.columnMappingGrid.Name = "columnMappingGrid";
         this.columnMappingGrid.Size = new System.Drawing.Size(1078, 757);
         this.columnMappingGrid.TabIndex = 1;
         this.columnMappingGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.columnMappingGridView});
         // 
         // columnMappingGridView
         // 
         this.columnMappingGridView.AllowsFiltering = true;
         this.columnMappingGridView.ColumnPanelRowHeight = 0;
         this.columnMappingGridView.DetailHeight = 284;
         this.columnMappingGridView.EnableColumnContextMenu = true;
         this.columnMappingGridView.FooterPanelHeight = 0;
         this.columnMappingGridView.GridControl = this.columnMappingGrid;
         this.columnMappingGridView.GroupRowHeight = 0;
         this.columnMappingGridView.LevelIndent = 0;
         this.columnMappingGridView.MultiSelect = true;
         this.columnMappingGridView.Name = "columnMappingGridView";
         this.columnMappingGridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.columnMappingGridView.OptionsNavigation.AutoFocusNewRow = true;
         this.columnMappingGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.columnMappingGridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.columnMappingGridView.OptionsSelection.MultiSelect = true;
         this.columnMappingGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
         this.columnMappingGridView.PreviewIndent = 0;
         this.columnMappingGridView.RowHeight = 0;
         this.columnMappingGridView.ViewCaptionHeight = 0;
         // 
         // ColumnMappingControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.columnMappingGrid);
         this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
         this.Name = "ColumnMappingControl";
         this.Size = new System.Drawing.Size(1078, 757);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingGrid)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingGridView)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private UI.Controls.UxGridControl columnMappingGrid;
      private UI.Controls.UxGridView columnMappingGridView;
   }
}
