using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Importer
{
   partial class ExcelPreviewView
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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.lblRangeSelectHint = new DevExpress.XtraEditors.LabelControl();
         this.excelGridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.excelGridView = new OSPSuite.UI.Controls.UxGridView();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).BeginInit();
         this.layoutControlBase.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.excelGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.excelGridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(1327, 30);
         this.btnCancel.Size = new System.Drawing.Size(276, 54);
         // 
         // btnOk
         // 
         this.btnOk.Location = new System.Drawing.Point(993, 30);
         this.btnOk.Size = new System.Drawing.Size(324, 54);
         // 
         // layoutControlBase
         // 
         this.layoutControlBase.Location = new System.Drawing.Point(0, 766);
         this.layoutControlBase.Size = new System.Drawing.Size(1632, 117);
         this.layoutControlBase.Controls.SetChildIndex(this.btnCancel, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnOk, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnExtra, 0);
         // 
         // btnExtra
         // 
         this.btnExtra.Size = new System.Drawing.Size(473, 54);
         // 
         // layoutControlGroupBase
         // 
         this.layoutControlGroupBase.Size = new System.Drawing.Size(1632, 117);
         // 
         // layoutItemOK
         // 
         this.layoutItemOK.Location = new System.Drawing.Point(964, 0);
         this.layoutItemOK.Size = new System.Drawing.Size(334, 67);
         // 
         // layoutItemCancel
         // 
         this.layoutItemCancel.Location = new System.Drawing.Point(1298, 0);
         this.layoutItemCancel.Size = new System.Drawing.Size(286, 67);
         // 
         // emptySpaceItemBase
         // 
         this.emptySpaceItemBase.Location = new System.Drawing.Point(483, 0);
         this.emptySpaceItemBase.Size = new System.Drawing.Size(481, 67);
         // 
         // layoutItemExtra
         // 
         this.layoutItemExtra.Size = new System.Drawing.Size(483, 67);
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.lblRangeSelectHint);
         this.layoutControl1.Controls.Add(this.excelGridControl);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(1632, 766);
         this.layoutControl1.TabIndex = 38;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // lblRangeSelectHint
         // 
         this.lblRangeSelectHint.Location = new System.Drawing.Point(30, 30);
         this.lblRangeSelectHint.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.lblRangeSelectHint.Name = "lblRangeSelectHint";
         this.lblRangeSelectHint.Size = new System.Drawing.Size(220, 33);
         this.lblRangeSelectHint.StyleController = this.layoutControl1;
         this.lblRangeSelectHint.TabIndex = 5;
         this.lblRangeSelectHint.Text = "lblRangeSelectHint";
         // 
         // excelGridControl
         // 
         this.excelGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(20, 20, 20, 20);
         this.excelGridControl.Location = new System.Drawing.Point(30, 73);
         this.excelGridControl.MainView = this.excelGridView;
         this.excelGridControl.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.excelGridControl.Name = "excelGridControl";
         this.excelGridControl.Size = new System.Drawing.Size(1572, 663);
         this.excelGridControl.TabIndex = 4;
         this.excelGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.excelGridView});
         // 
         // excelGridView
         // 
         this.excelGridView.AllowsFiltering = true;
         this.excelGridView.DetailHeight = 888;
         this.excelGridView.EnableColumnContextMenu = true;
         this.excelGridView.FixedLineWidth = 5;
         this.excelGridView.GridControl = this.excelGridControl;
         this.excelGridView.MultiSelect = false;
         this.excelGridView.Name = "excelGridView";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(1632, 766);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.excelGridControl;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 43);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(1582, 673);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.lblRangeSelectHint;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(1582, 43);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // ExcelPreviewView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "ExcelPreviewView";
         this.ClientSize = new System.Drawing.Size(1632, 883);
         this.Controls.Add(this.layoutControl1);
         this.Margin = new System.Windows.Forms.Padding(50, 51, 50, 51);
         this.Name = "ExcelPreviewView";
         this.Text = "ExcelPreviewView";
         this.Controls.SetChildIndex(this.layoutControlBase, 0);
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).EndInit();
         this.layoutControlBase.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.excelGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.excelGridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private UxGridControl excelGridControl;
      private UxGridView excelGridView;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraEditors.LabelControl lblRangeSelectHint;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private UxLayoutControl layoutControl1;
   }
}