using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views
{
   partial class DisplayUnitsView
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
         this.layoutControl = new UxLayoutControl();
         this.btnSaveUnits = new DevExpress.XtraEditors.SimpleButton();
         this.btnLoadUnits = new DevExpress.XtraEditors.SimpleButton();
         this.btnAddUnitMap = new DevExpress.XtraEditors.SimpleButton();
         this.gridControl = new UxGridControl();
         this.gridView = new UxGridView();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemAddUnitMap = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutItemLoadUnits = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemSaveUnits = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddUnitMap)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLoadUnits)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSaveUnits)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.btnSaveUnits);
         this.layoutControl.Controls.Add(this.btnLoadUnits);
         this.layoutControl.Controls.Add(this.btnAddUnitMap);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(507, 402);
         this.layoutControl.TabIndex = 38;
         this.layoutControl.Text = "layoutControl1";
         // 
         // btnSaveUnits
         // 
         this.btnSaveUnits.Location = new System.Drawing.Point(128, 2);
         this.btnSaveUnits.Name = "btnSaveUnits";
         this.btnSaveUnits.Size = new System.Drawing.Size(75, 22);
         this.btnSaveUnits.StyleController = this.layoutControl;
         this.btnSaveUnits.TabIndex = 7;
         this.btnSaveUnits.Text = "btnSaveUnits";
         // 
         // btnLoadUnits
         // 
         this.btnLoadUnits.Location = new System.Drawing.Point(2, 2);
         this.btnLoadUnits.Name = "btnLoadUnits";
         this.btnLoadUnits.Size = new System.Drawing.Size(122, 22);
         this.btnLoadUnits.StyleController = this.layoutControl;
         this.btnLoadUnits.TabIndex = 6;
         this.btnLoadUnits.Text = "btnLoadUnits";
         // 
         // btnAddUnitMap
         // 
         this.btnAddUnitMap.Location = new System.Drawing.Point(268, 2);
         this.btnAddUnitMap.Name = "btnAddUnitMap";
         this.btnAddUnitMap.Size = new System.Drawing.Size(237, 22);
         this.btnAddUnitMap.StyleController = this.layoutControl;
         this.btnAddUnitMap.TabIndex = 5;
         this.btnAddUnitMap.Text = "btnAddUnitMap";
         // 
         // gridControl
         // 
         this.gridControl.Location = new System.Drawing.Point(2, 28);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(503, 372);
         this.gridControl.TabIndex = 4;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.GridControl = this.gridControl;
         this.gridView.Name = "gridView";
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.CustomizationFormText = "layoutControlGroup";
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutItemAddUnitMap,
            this.emptySpaceItem1,
            this.layoutItemLoadUnits,
            this.layoutItemSaveUnits});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(507, 402);
         this.layoutControlGroup.Text = "layoutControlGroup";
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.gridControl;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(507, 376);
         this.layoutControlItem1.Text = "layoutControlItem1";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextToControlDistance = 0;
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutItemAddUnitMap
         // 
         this.layoutItemAddUnitMap.Control = this.btnAddUnitMap;
         this.layoutItemAddUnitMap.CustomizationFormText = "layoutControlItem2";
         this.layoutItemAddUnitMap.Location = new System.Drawing.Point(266, 0);
         this.layoutItemAddUnitMap.Name = "layoutItemAddUnitMap";
         this.layoutItemAddUnitMap.Size = new System.Drawing.Size(241, 26);
         this.layoutItemAddUnitMap.Text = "layoutControlItem2";
         this.layoutItemAddUnitMap.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemAddUnitMap.TextToControlDistance = 0;
         this.layoutItemAddUnitMap.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
         this.emptySpaceItem1.Location = new System.Drawing.Point(205, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(61, 26);
         this.emptySpaceItem1.Text = "emptySpaceItem1";
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutItemLoadUnits
         // 
         this.layoutItemLoadUnits.Control = this.btnLoadUnits;
         this.layoutItemLoadUnits.CustomizationFormText = "layoutItemLoadUnits";
         this.layoutItemLoadUnits.Location = new System.Drawing.Point(0, 0);
         this.layoutItemLoadUnits.Name = "layoutItemLoadUnits";
         this.layoutItemLoadUnits.Size = new System.Drawing.Size(126, 26);
         this.layoutItemLoadUnits.Text = "layoutItemLoadUnits";
         this.layoutItemLoadUnits.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemLoadUnits.TextToControlDistance = 0;
         this.layoutItemLoadUnits.TextVisible = false;
         // 
         // layoutItemSaveUnits
         // 
         this.layoutItemSaveUnits.Control = this.btnSaveUnits;
         this.layoutItemSaveUnits.CustomizationFormText = "layoutItemSaveUnits";
         this.layoutItemSaveUnits.Location = new System.Drawing.Point(126, 0);
         this.layoutItemSaveUnits.Name = "layoutItemSaveUnits";
         this.layoutItemSaveUnits.Size = new System.Drawing.Size(79, 26);
         this.layoutItemSaveUnits.Text = "layoutItemSaveUnits";
         this.layoutItemSaveUnits.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemSaveUnits.TextToControlDistance = 0;
         this.layoutItemSaveUnits.TextVisible = false;
         // 
         // DisplayUnitsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "DisplayUnitsView";
         this.Size = new System.Drawing.Size(507, 402);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddUnitMap)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLoadUnits)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSaveUnits)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private UxGridControl gridControl;
      private UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraEditors.SimpleButton btnAddUnitMap;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemAddUnitMap;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.SimpleButton btnSaveUnits;
      private DevExpress.XtraEditors.SimpleButton btnLoadUnits;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemLoadUnits;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSaveUnits;
   }
}
