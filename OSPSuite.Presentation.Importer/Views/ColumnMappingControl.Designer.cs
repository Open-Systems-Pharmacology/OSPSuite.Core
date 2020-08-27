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
         this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
         this.label1 = new System.Windows.Forms.Label();
         this.uxGrid = new OSPSuite.UI.Controls.UxGridControl();
         this.uxGridView = new OSPSuite.UI.Controls.UxGridView();
         this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         this.flowLayoutPanel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.uxGrid)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxGridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // flowLayoutPanel1
         // 
         this.flowLayoutPanel1.Controls.Add(this.label1);
         this.flowLayoutPanel1.Location = new System.Drawing.Point(-2, 0);
         this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
         this.flowLayoutPanel1.Name = "flowLayoutPanel1";
         this.flowLayoutPanel1.Size = new System.Drawing.Size(1637, 825);
         this.flowLayoutPanel1.TabIndex = 0;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(6, 0);
         this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(132, 34);
         this.label1.TabIndex = 0;
         this.label1.Text = "Mappings";
         // 
         // uxGrid
         // 
         this.uxGrid.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6);
         this.uxGrid.Location = new System.Drawing.Point(0, 0);
         this.uxGrid.MainView = this.uxGridView;
         this.uxGrid.Margin = new System.Windows.Forms.Padding(6);
         this.uxGrid.Name = "uxGrid";
         this.uxGrid.Size = new System.Drawing.Size(1633, 923);
         this.uxGrid.TabIndex = 1;
         this.uxGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.uxGridView});
         // 
         // uxGridView
         // 
         this.uxGridView.AllowsFiltering = true;
         this.uxGridView.DetailHeight = 722;
         this.uxGridView.EnableColumnContextMenu = true;
         this.uxGridView.FixedLineWidth = 4;
         this.uxGridView.GridControl = this.uxGrid;
         this.uxGridView.MultiSelect = true;
         this.uxGridView.Name = "uxGridView";
         this.uxGridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.uxGridView.OptionsNavigation.AutoFocusNewRow = true;
         this.uxGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.uxGridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.uxGridView.OptionsSelection.MultiSelect = true;
         this.uxGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
         // 
         // comboBoxEdit1
         // 
         this.comboBoxEdit1.Location = new System.Drawing.Point(120, 844);
         this.comboBoxEdit1.Margin = new System.Windows.Forms.Padding(6);
         this.comboBoxEdit1.Name = "comboBoxEdit1";
         this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.comboBoxEdit1.Size = new System.Drawing.Size(1506, 50);
         this.comboBoxEdit1.TabIndex = 1;
         // 
         // labelControl1
         // 
         this.labelControl1.Location = new System.Drawing.Point(9, 850);
         this.labelControl1.Margin = new System.Windows.Forms.Padding(6);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(85, 33);
         this.labelControl1.TabIndex = 2;
         this.labelControl1.Text = "Format";
         // 
         // ColumnMappingControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.uxGrid);
         this.Controls.Add(this.labelControl1);
         this.Controls.Add(this.comboBoxEdit1);
         this.Controls.Add(this.flowLayoutPanel1);
         this.Margin = new System.Windows.Forms.Padding(17, 16, 17, 16);
         this.Name = "ColumnMappingControl";
         this.Size = new System.Drawing.Size(1633, 923);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         this.flowLayoutPanel1.ResumeLayout(false);
         this.flowLayoutPanel1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.uxGrid)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxGridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
      private System.Windows.Forms.Label label1;
      private UI.Controls.UxGridControl uxGrid;
      private UI.Controls.UxGridView uxGridView;
      private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
      private DevExpress.XtraEditors.LabelControl labelControl1;
   }
}
