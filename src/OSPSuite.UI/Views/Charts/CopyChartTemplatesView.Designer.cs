using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Charts
{
   partial class CopyChartTemplatesView
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
         _gridViewBinder.Dispose();
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.cbSource = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.gridControl = new UxGridControl();
         this.gridView = new UxGridView();
         this.layoutItemSource = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemGridTemplates = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbSource.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridTemplates)).BeginInit();
         this.SuspendLayout();
         //
         // layoutControl
         //
         this.layoutControl.Controls.Add(this.cbSource);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(477, 414);
         this.layoutControl.TabIndex = 38;
         this.layoutControl.Text = "layoutControl1";
         //
         // layoutControlGroup
         //
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemSource,
            this.layoutItemGridTemplates});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Size = new System.Drawing.Size(477, 414);
         this.layoutControlGroup.TextVisible = false;
         //
         // cbSource
         //
         this.cbSource.Location = new System.Drawing.Point(109, 12);
         this.cbSource.Name = "cbSource";
         this.cbSource.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbSource.Size = new System.Drawing.Size(356, 20);
         this.cbSource.StyleController = this.layoutControl;
         this.cbSource.TabIndex = 4;
         //
         // gridControl
         //
         this.gridControl.Location = new System.Drawing.Point(12, 36);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(453, 366);
         this.gridControl.TabIndex = 5;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         //
         // gridView
         //
         this.gridView.GridControl = this.gridControl;
         this.gridView.Name = "gridView";
         //
         // layoutItemSource
         //
         this.layoutItemSource.Control = this.cbSource;
         this.layoutItemSource.Location = new System.Drawing.Point(0, 0);
         this.layoutItemSource.Name = "layoutItemSource";
         this.layoutItemSource.Size = new System.Drawing.Size(457, 24);
         this.layoutItemSource.TextSize = new System.Drawing.Size(93, 13);
         //
         // layoutItemGridTemplates
         //
         this.layoutItemGridTemplates.Control = this.gridControl;
         this.layoutItemGridTemplates.Location = new System.Drawing.Point(0, 24);
         this.layoutItemGridTemplates.Name = "layoutItemGridTemplates";
         this.layoutItemGridTemplates.Size = new System.Drawing.Size(457, 370);
         this.layoutItemGridTemplates.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemGridTemplates.TextVisible = false;
         //
         // CopyChartTemplatesView
         //
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "CopyChartTemplatesView";
         this.ClientSize = new System.Drawing.Size(477, 460);
         this.Controls.Add(this.layoutControl);
         this.Name = "CopyChartTemplatesView";
         this.Text = "CopyChartTemplatesView";
         this.Controls.SetChildIndex(this.layoutControl, 0);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbSource.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridTemplates)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private UxComboBoxEdit cbSource;
      private UxGridControl gridControl;
      private UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSource;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemGridTemplates;
   }
}
