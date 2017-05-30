using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.ObservedData
{
   partial class DataRepositoryMetaDataView
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
         _molWeightBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.gridControl = new UxGridControl();
         this.gridView = new UxGridView();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemAddRow = new DevExpress.XtraLayout.LayoutControlItem();
         this.btnAddRow = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControl1 = new UxLayoutControl();
         this.tbLowerLimitOfQuantification = new DevExpress.XtraEditors.TextEdit();
         this.tbMolWeight = new DevExpress.XtraEditors.TextEdit();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutItemMolWeight = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemLowerLimitOfQuantification = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddRow)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbLowerLimitOfQuantification.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbMolWeight.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMolWeight)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLowerLimitOfQuantification)).BeginInit();
         this.SuspendLayout();
         // 
         // gridControl
         // 
         this.gridControl.Location = new System.Drawing.Point(2, 76);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(725, 530);
         this.gridControl.TabIndex = 0;
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
         this.gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutItemAddRow,
            this.emptySpaceItem1,
            this.layoutItemMolWeight,
            this.layoutItemLowerLimitOfQuantification});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(729, 608);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.gridControl;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 74);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(729, 534);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutItemAddRow
         // 
         this.layoutItemAddRow.Control = this.btnAddRow;
         this.layoutItemAddRow.CustomizationFormText = "layoutControlItem2";
         this.layoutItemAddRow.Location = new System.Drawing.Point(364, 48);
         this.layoutItemAddRow.Name = "layoutItemAddRow";
         this.layoutItemAddRow.Size = new System.Drawing.Size(365, 26);
         this.layoutItemAddRow.Text = "layoutControlItem2";
         this.layoutItemAddRow.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemAddRow.TextVisible = false;
         // 
         // btnAddRow
         // 
         this.btnAddRow.Location = new System.Drawing.Point(366, 50);
         this.btnAddRow.Name = "btnAddRow";
         this.btnAddRow.Size = new System.Drawing.Size(361, 22);
         this.btnAddRow.StyleController = this.layoutControl1;
         this.btnAddRow.TabIndex = 4;
         this.btnAddRow.Text = "btnAddRow";
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.tbLowerLimitOfQuantification);
         this.layoutControl1.Controls.Add(this.tbMolWeight);
         this.layoutControl1.Controls.Add(this.btnAddRow);
         this.layoutControl1.Controls.Add(this.gridControl);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(790, 387, 250, 350);
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(729, 608);
         this.layoutControl1.TabIndex = 1;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // tbLowerLimitOfQuantification
         // 
         this.tbLowerLimitOfQuantification.Enabled = false;
         this.tbLowerLimitOfQuantification.Location = new System.Drawing.Point(186, 26);
         this.tbLowerLimitOfQuantification.Name = "tbLowerLimitOfQuantification";
         this.tbLowerLimitOfQuantification.Size = new System.Drawing.Size(541, 20);
         this.tbLowerLimitOfQuantification.StyleController = this.layoutControl1;
         this.tbLowerLimitOfQuantification.TabIndex = 6;
         // 
         // tbMolWeight
         // 
         this.tbMolWeight.Location = new System.Drawing.Point(186, 2);
         this.tbMolWeight.Name = "tbMolWeight";
         this.tbMolWeight.Size = new System.Drawing.Size(541, 20);
         this.tbMolWeight.StyleController = this.layoutControl1;
         this.tbMolWeight.TabIndex = 5;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 48);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(364, 26);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutItemMolWeight
         // 
         this.layoutItemMolWeight.Control = this.tbMolWeight;
         this.layoutItemMolWeight.Location = new System.Drawing.Point(0, 0);
         this.layoutItemMolWeight.Name = "layoutItemMolWeight";
         this.layoutItemMolWeight.Size = new System.Drawing.Size(729, 24);
         this.layoutItemMolWeight.TextSize = new System.Drawing.Size(181, 13);
         // 
         // layoutItemLowerLimitOfQuantification
         // 
         this.layoutItemLowerLimitOfQuantification.Control = this.tbLowerLimitOfQuantification;
         this.layoutItemLowerLimitOfQuantification.Location = new System.Drawing.Point(0, 24);
         this.layoutItemLowerLimitOfQuantification.Name = "layoutItemLowerLimitOfQuantification";
         this.layoutItemLowerLimitOfQuantification.Size = new System.Drawing.Size(729, 24);
         this.layoutItemLowerLimitOfQuantification.TextSize = new System.Drawing.Size(181, 13);
         this.layoutItemLowerLimitOfQuantification.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
         // 
         // DataRepositoryMetaDataView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "DataRepositoryMetaDataView";
         this.Size = new System.Drawing.Size(729, 608);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddRow)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tbLowerLimitOfQuantification.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbMolWeight.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMolWeight)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLowerLimitOfQuantification)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private UxGridControl gridControl;
      private UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemAddRow;
      private DevExpress.XtraEditors.SimpleButton btnAddRow;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.TextEdit tbMolWeight;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemMolWeight;
      private DevExpress.XtraEditors.TextEdit tbLowerLimitOfQuantification;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemLowerLimitOfQuantification;
   }
}
