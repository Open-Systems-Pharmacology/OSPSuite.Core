using OSPSuite.Assets;

namespace OSPSuite.Presentation.Importer.Views
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
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.mappingFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
         this.mappingsLabel = new System.Windows.Forms.Label();
         this.columnMappingGrid = new OSPSuite.UI.Controls.UxGridControl();
         this.columnMappingGridView = new OSPSuite.UI.Controls.UxGridView();
         this.formaLabelControl = new DevExpress.XtraEditors.LabelControl();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         this.mappingFlowLayoutPanel.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingGrid)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingGridView)).BeginInit();
         this.SuspendLayout();
         // 
         // mappingFlowLayoutPanel
         // 
         this.mappingFlowLayoutPanel.Controls.Add(this.mappingsLabel);
         this.mappingFlowLayoutPanel.Location = new System.Drawing.Point(-2, 0);
         this.mappingFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(6);
         this.mappingFlowLayoutPanel.Name = "mappingFlowLayoutPanel";
         this.mappingFlowLayoutPanel.Size = new System.Drawing.Size(1637, 825);
         this.mappingFlowLayoutPanel.TabIndex = 0;
         // 
         // mappingsLabel
         // 
         this.mappingsLabel.AutoSize = true;
         this.mappingsLabel.Location = new System.Drawing.Point(6, 0);
         this.mappingsLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
         this.mappingsLabel.Name = "mappingsLabel";
         this.mappingsLabel.Size = new System.Drawing.Size(132, 34);
         this.mappingsLabel.TabIndex = 0;
         this.mappingsLabel.Text = Captions.Importer.Mappings;
         // 
         // columnMappingGrid
         // 
         this.columnMappingGrid.Dock = System.Windows.Forms.DockStyle.Fill;
         this.columnMappingGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6);
         this.columnMappingGrid.Location = new System.Drawing.Point(0, 0);
         this.columnMappingGrid.MainView = this.columnMappingGridView;
         this.columnMappingGrid.Margin = new System.Windows.Forms.Padding(6);
         this.columnMappingGrid.Name = "columnMappingGrid";
         this.columnMappingGrid.Size = new System.Drawing.Size(1633, 923);
         this.columnMappingGrid.TabIndex = 1;
         this.columnMappingGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.columnMappingGridView});
         // 
         // columnMappingGridView
         // 
         this.columnMappingGridView.AllowsFiltering = true;
         this.columnMappingGridView.DetailHeight = 722;
         this.columnMappingGridView.EnableColumnContextMenu = true;
         this.columnMappingGridView.FixedLineWidth = 4;
         this.columnMappingGridView.GridControl = this.columnMappingGrid;
         this.columnMappingGridView.MultiSelect = true;
         this.columnMappingGridView.Name = "columnMappingGridView";
         this.columnMappingGridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.columnMappingGridView.OptionsNavigation.AutoFocusNewRow = true;
         this.columnMappingGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.columnMappingGridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.columnMappingGridView.OptionsSelection.MultiSelect = true;
         this.columnMappingGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
         // 
         // formaLabelControl
         // 
         this.formaLabelControl.Location = new System.Drawing.Point(9, 850);
         this.formaLabelControl.Margin = new System.Windows.Forms.Padding(6);
         this.formaLabelControl.Name = "formaLabelControl";
         this.formaLabelControl.Size = new System.Drawing.Size(85, 33);
         this.formaLabelControl.TabIndex = 2;
         this.formaLabelControl.Text = Captions.Importer.Format;
         // 
         // ColumnMappingControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.columnMappingGrid);
         this.Controls.Add(this.formaLabelControl);
         this.Controls.Add(this.mappingFlowLayoutPanel);
         this.Margin = new System.Windows.Forms.Padding(17, 16, 17, 16);
         this.Name = "ColumnMappingControl";
         this.Size = new System.Drawing.Size(1633, 923);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         this.mappingFlowLayoutPanel.ResumeLayout(false);
         this.mappingFlowLayoutPanel.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingGrid)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingGridView)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.FlowLayoutPanel mappingFlowLayoutPanel;
      private System.Windows.Forms.Label mappingsLabel;
      private UI.Controls.UxGridControl columnMappingGrid;
      private UI.Controls.UxGridView columnMappingGridView;
      private DevExpress.XtraEditors.LabelControl formaLabelControl;
   }
}
