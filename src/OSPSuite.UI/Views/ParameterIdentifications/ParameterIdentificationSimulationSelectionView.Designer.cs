using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   partial class ParameterIdentificationSimulationSelectionView
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
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParameterIdentificationSimulationSelectionView));
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnAddSimulation = new DevExpress.XtraEditors.SimpleButton();
         this.treeView = new UxImageTreeView();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemTreeView = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemAddSimulation = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.treeView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTreeView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddSimulation)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.btnAddSimulation);
         this.layoutControl.Controls.Add(this.treeView);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(469, 510);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // btnAddSimulation
         // 
         this.btnAddSimulation.Location = new System.Drawing.Point(236, 2);
         this.btnAddSimulation.Name = "btnAddSimulation";
         this.btnAddSimulation.Size = new System.Drawing.Size(231, 22);
         this.btnAddSimulation.StyleController = this.layoutControl;
         this.btnAddSimulation.TabIndex = 5;
         this.btnAddSimulation.Text = "btnAddSimulation";
         // 
         // treeView
         // 
         this.treeView.IsLatched = false;
         this.treeView.Location = new System.Drawing.Point(101, 28);
         this.treeView.Name = "treeView";
         this.treeView.Size = new System.Drawing.Size(366, 482);
         this.treeView.TabIndex = 4;
         this.treeView.ToolTipForNode = ((System.Func<ITreeNode, System.Collections.Generic.IEnumerable<ToolTipPart>>)(resources.GetObject("treeView.ToolTipForNode")));
         this.treeView.UseLazyLoading = false;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemTreeView,
            this.layoutItemAddSimulation,
            this.emptySpaceItem1});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(469, 510);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemTreeView
         // 
         this.layoutItemTreeView.Control = this.treeView;
         this.layoutItemTreeView.Location = new System.Drawing.Point(0, 26);
         this.layoutItemTreeView.Name = "layoutItemTreeView";
         this.layoutItemTreeView.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 0);
         this.layoutItemTreeView.Size = new System.Drawing.Size(469, 484);
         this.layoutItemTreeView.TextSize = new System.Drawing.Size(96, 13);
         // 
         // layoutItemAddSimulation
         // 
         this.layoutItemAddSimulation.Control = this.btnAddSimulation;
         this.layoutItemAddSimulation.Location = new System.Drawing.Point(234, 0);
         this.layoutItemAddSimulation.Name = "layoutItemAddSimulation";
         this.layoutItemAddSimulation.Size = new System.Drawing.Size(235, 26);
         this.layoutItemAddSimulation.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemAddSimulation.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(234, 26);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // ParameterIdentificationSimulationSelectionView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ParameterIdentificationSimulationSelectionView";
         this.Size = new System.Drawing.Size(469, 510);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.treeView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTreeView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddSimulation)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraEditors.SimpleButton btnAddSimulation;
      private UxImageTreeView treeView;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemTreeView;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemAddSimulation;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
