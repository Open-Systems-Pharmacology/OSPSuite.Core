using OSPSuite.Utility.Extensions;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   partial class MultipleParameterIdentificationResultsView
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
         _optimizedParametersBinderCache.DisposeAll();
         _optimizedParametersBinderCache.Clear();
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
         
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.optimizedParametersView = new UxGridView(gridControl);
         this.mainView = new OSPSuite.UI.Controls.UxGridView();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemResults = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.optimizedParametersView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.mainView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemResults)).BeginInit();
         this.SuspendLayout();
         // 
         // optimizedParametersView
         // 
         this.optimizedParametersView.AllowsFiltering = true;
         this.optimizedParametersView.EnableColumnContextMenu = true;
         this.optimizedParametersView.GridControl = this.gridControl;
         this.optimizedParametersView.MultiSelect = false;
         this.optimizedParametersView.Name = "optimizedParametersView";
         this.optimizedParametersView.OptionsSelection.EnableAppearanceFocusedRow = false;
         // 
         // gridControl
         // 
         gridLevelNode1.LevelTemplate = this.optimizedParametersView;
         gridLevelNode1.RelationName = "OptimizedParameters";
         this.gridControl.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
         this.gridControl.Location = new System.Drawing.Point(92, 2);
         this.gridControl.MainView = this.mainView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(648, 582);
         this.gridControl.TabIndex = 4;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.mainView,
            this.optimizedParametersView});
         // 
         // mainView
         // 
         this.mainView.AllowsFiltering = true;
         this.mainView.EnableColumnContextMenu = true;
         this.mainView.GridControl = this.gridControl;
         this.mainView.MultiSelect = false;
         this.mainView.Name = "mainView";
         this.mainView.OptionsSelection.EnableAppearanceFocusedRow = false;
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(742, 586);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemResults});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(742, 586);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemResults
         // 
         this.layoutItemResults.Control = this.gridControl;
         this.layoutItemResults.Location = new System.Drawing.Point(0, 0);
         this.layoutItemResults.Name = "layoutItemResults";
         this.layoutItemResults.Size = new System.Drawing.Size(742, 586);
         this.layoutItemResults.TextSize = new System.Drawing.Size(87, 13);
         // 
         // MultipleParameterIdentificationResultsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "MultipleParameterIdentificationResultsView";
         this.Size = new System.Drawing.Size(742, 586);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.optimizedParametersView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.mainView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemResults)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private UxGridControl gridControl;
      private UxGridView mainView;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemResults;
      private UxGridView optimizedParametersView;
   }
}
