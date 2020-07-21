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
         this.uxGridView1 = new OSPSuite.UI.Controls.UxGridView();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         this.flowLayoutPanel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.uxGrid)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxGridView1)).BeginInit();
         this.SuspendLayout();
         // 
         // flowLayoutPanel1
         // 
         this.flowLayoutPanel1.Controls.Add(this.label1);
         this.flowLayoutPanel1.Controls.Add(this.uxGrid);
         this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
         this.flowLayoutPanel1.Name = "flowLayoutPanel1";
         this.flowLayoutPanel1.Size = new System.Drawing.Size(763, 400);
         this.flowLayoutPanel1.TabIndex = 0;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(3, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(65, 17);
         this.label1.TabIndex = 0;
         this.label1.Text = "Mappings";
         // 
         // uxGrid
         // 
         this.uxGrid.Location = new System.Drawing.Point(3, 20);
         this.uxGrid.MainView = this.uxGridView1;
         this.uxGrid.Name = "uxGrid";
         this.uxGrid.Size = new System.Drawing.Size(757, 380);
         this.uxGrid.TabIndex = 1;
         this.uxGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.uxGridView1});
         // 
         // uxGridView1
         // 
         this.uxGridView1.AllowsFiltering = true;
         this.uxGridView1.EnableColumnContextMenu = true;
         this.uxGridView1.GridControl = this.uxGrid;
         this.uxGridView1.MultiSelect = true;
         this.uxGridView1.Name = "uxGridView1";
         this.uxGridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.uxGridView1.OptionsNavigation.AutoFocusNewRow = true;
         this.uxGridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.uxGridView1.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.uxGridView1.OptionsSelection.MultiSelect = true;
         this.uxGridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
         // 
         // ColumnMappingControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.flowLayoutPanel1);
         this.Name = "ColumnMappingControl";
         this.Size = new System.Drawing.Size(769, 406);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         this.flowLayoutPanel1.ResumeLayout(false);
         this.flowLayoutPanel1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.uxGrid)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxGridView1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
      private System.Windows.Forms.Label label1;
      private UI.Controls.UxGridControl uxGrid;
      private UI.Controls.UxGridView uxGridView1;
   }
}
