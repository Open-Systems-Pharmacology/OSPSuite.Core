using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Charts
{
   partial class ChartTemplateManagerView
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
         DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
         this.layoutControl = new UxLayoutControl();
         this.loadTemplateButton = new DevExpress.XtraEditors.SimpleButton();
         this.gridTemplates = new UxGridControl();
         this.gridViewTemplates = new UxGridView();
         this.panelTemplateView = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.loadTemplateControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridTemplates)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewTemplates)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelTemplateView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.loadTemplateControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.loadTemplateButton);
         this.layoutControl.Controls.Add(this.gridTemplates);
         this.layoutControl.Controls.Add(this.panelTemplateView);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(787, 263, 250, 350);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(972, 641);
         this.layoutControl.TabIndex = 39;
         this.layoutControl.Text = "layoutControl1";
         // 
         // loadTemplateButton
         // 
         this.loadTemplateButton.Location = new System.Drawing.Point(706, 12);
         this.loadTemplateButton.Name = "loadTemplateButton";
         this.loadTemplateButton.Size = new System.Drawing.Size(254, 22);
         this.loadTemplateButton.StyleController = this.layoutControl;
         this.loadTemplateButton.TabIndex = 7;
         this.loadTemplateButton.Text = "loadTemplateButton";
         // 
         // gridTemplates
         // 
         gridLevelNode1.RelationName = "Level1";
         this.gridTemplates.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
         this.gridTemplates.Location = new System.Drawing.Point(12, 12);
         this.gridTemplates.MainView = this.gridViewTemplates;
         this.gridTemplates.Name = "gridTemplates";
         this.gridTemplates.Size = new System.Drawing.Size(259, 617);
         this.gridTemplates.TabIndex = 6;
         this.gridTemplates.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTemplates});
         // 
         // gridViewTemplates
         // 
         this.gridViewTemplates.AllowsFiltering = true;
         this.gridViewTemplates.GridControl = this.gridTemplates;
         this.gridViewTemplates.Name = "gridViewTemplates";
         // 
         // panelTemplateView
         // 
         this.panelTemplateView.Location = new System.Drawing.Point(278, 36);
         this.panelTemplateView.Margin = new System.Windows.Forms.Padding(0);
         this.panelTemplateView.Name = "panelTemplateView";
         this.panelTemplateView.Size = new System.Drawing.Size(684, 595);
         this.panelTemplateView.TabIndex = 5;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.splitterItem1,
            this.emptySpaceItem1,
            this.loadTemplateControlItem});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Size = new System.Drawing.Size(972, 641);
         this.layoutControlGroup1.Text = "Root";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.gridTemplates;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(263, 621);
         this.layoutControlItem1.Text = "layoutControlItem1";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextToControlDistance = 0;
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.panelTemplateView;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(268, 26);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlItem2.Size = new System.Drawing.Size(684, 595);
         this.layoutControlItem2.Text = "layoutControlItem2";
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextToControlDistance = 0;
         this.layoutControlItem2.TextVisible = false;
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.CustomizationFormText = "splitterItem1";
         this.splitterItem1.Location = new System.Drawing.Point(263, 0);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(5, 621);
         // 
         // loadTemplateControlItem
         // 
         this.loadTemplateControlItem.Control = this.loadTemplateButton;
         this.loadTemplateControlItem.CustomizationFormText = "layoutControlItem3";
         this.loadTemplateControlItem.Location = new System.Drawing.Point(694, 0);
         this.loadTemplateControlItem.Name = "loadTemplateControlItem";
         this.loadTemplateControlItem.Size = new System.Drawing.Size(258, 26);
         this.loadTemplateControlItem.Text = "loadTemplateControlItem";
         this.loadTemplateControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.loadTemplateControlItem.TextToControlDistance = 0;
         this.loadTemplateControlItem.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
         this.emptySpaceItem1.Location = new System.Drawing.Point(268, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(426, 26);
         this.emptySpaceItem1.Text = "emptySpaceItem1";
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // ChartTemplateManagerView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoSize = true;
         this.Controls.Add(this.layoutControl);
         this.Name = "ChartTemplateManagerView";
         this.Size = new System.Drawing.Size(972, 641);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridTemplates)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewTemplates)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelTemplateView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.loadTemplateControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private UxLayoutControl layoutControl;
      private UxGridControl gridTemplates;
      private UxGridView gridViewTemplates;
      private DevExpress.XtraEditors.PanelControl panelTemplateView;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraEditors.SimpleButton loadTemplateButton;
      private DevExpress.XtraLayout.LayoutControlItem loadTemplateControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
