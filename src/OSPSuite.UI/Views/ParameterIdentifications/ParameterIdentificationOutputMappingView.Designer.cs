using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   partial class ParameterIdentificationOutputMappingView
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

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnAddOutput = new DevExpress.XtraEditors.SimpleButton();
         this.gridControl = new UxGridControl();
         this.gridView = new UxGridView();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemGridOutputs = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemAddOutput = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridOutputs)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddOutput)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.btnAddOutput);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(773, 499);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // btnAddOutput
         // 
         this.btnAddOutput.Location = new System.Drawing.Point(274, 2);
         this.btnAddOutput.Name = "btnAddOutput";
         this.btnAddOutput.Size = new System.Drawing.Size(497, 22);
         this.btnAddOutput.StyleController = this.layoutControl;
         this.btnAddOutput.TabIndex = 5;
         this.btnAddOutput.Text = "btnAddOutput";
         // 
         // gridControl
         // 
         this.gridControl.Location = new System.Drawing.Point(115, 28);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(656, 469);
         this.gridControl.TabIndex = 4;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.EnableColumnContextMenu = true;
         this.gridView.GridControl = this.gridControl;
         this.gridView.MultiSelect = false;
         this.gridView.Name = "gridView";
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemGridOutputs,
            this.layoutItemAddOutput,
            this.emptySpaceItem});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(773, 499);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemGridOutputs
         // 
         this.layoutItemGridOutputs.Control = this.gridControl;
         this.layoutItemGridOutputs.Location = new System.Drawing.Point(0, 26);
         this.layoutItemGridOutputs.Name = "layoutItemGridOutputs";
         this.layoutItemGridOutputs.Size = new System.Drawing.Size(773, 473);
         this.layoutItemGridOutputs.TextSize = new System.Drawing.Size(110, 13);
         // 
         // layoutItemAddOutput
         // 
         this.layoutItemAddOutput.Control = this.btnAddOutput;
         this.layoutItemAddOutput.Location = new System.Drawing.Point(272, 0);
         this.layoutItemAddOutput.Name = "layoutItemAddOutput";
         this.layoutItemAddOutput.Size = new System.Drawing.Size(501, 26);
         this.layoutItemAddOutput.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemAddOutput.TextVisible = false;
         // 
         // emptySpaceItem
         // 
         this.emptySpaceItem.AllowHotTrack = false;
         this.emptySpaceItem.Location = new System.Drawing.Point(0, 0);
         this.emptySpaceItem.Name = "emptySpaceItem";
         this.emptySpaceItem.Size = new System.Drawing.Size(272, 26);
         this.emptySpaceItem.TextSize = new System.Drawing.Size(0, 0);
         // 
         // ParameterIdentificationOutputMappingView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ParameterIdentificationOutputMappingView";
         this.Size = new System.Drawing.Size(773, 499);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridOutputs)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddOutput)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraEditors.SimpleButton btnAddOutput;
      private UxGridControl gridControl;
      private UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemGridOutputs;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemAddOutput;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem;
   }
}
