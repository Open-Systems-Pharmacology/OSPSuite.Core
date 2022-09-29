using DevExpress.Utils;

namespace OSPSuite.UI.Views.Charts
{
   partial class DeviationLinesView
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
         this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
         this.deviationLineDescriptionLabelControl = new DevExpress.XtraEditors.LabelControl();
         this.foldValueTextLabelControl = new DevExpress.XtraEditors.LabelControl();
         this.foldValueTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.foldValueInputControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.foldValueTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.foldValueInputControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // tablePanel
         // 
         this.tablePanel.Location = new System.Drawing.Point(0, 286);
         this.tablePanel.Size = new System.Drawing.Size(1260, 110);
         this.tablePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.tablePanel_Paint);
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.deviationLineDescriptionLabelControl);
         this.layoutControl1.Controls.Add(this.foldValueTextLabelControl);
         this.layoutControl1.Controls.Add(this.foldValueTextEdit);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.Root;
         this.layoutControl1.Size = new System.Drawing.Size(1260, 286);
         this.layoutControl1.TabIndex = 39;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // deviationLineDescriptionLabelControl
         // 
         this.deviationLineDescriptionLabelControl.Location = new System.Drawing.Point(12, 101);
         this.deviationLineDescriptionLabelControl.Name = "deviationLineDescriptionLabelControl";
         this.deviationLineDescriptionLabelControl.Size = new System.Drawing.Size(435, 33);
         this.deviationLineDescriptionLabelControl.StyleController = this.layoutControl1;
         this.deviationLineDescriptionLabelControl.TabIndex = 6;
         this.deviationLineDescriptionLabelControl.Text = "deviationLineDescriptionLabelControl";
         // 
         // foldValueTextLabelControl
         // 
         this.foldValueTextLabelControl.Location = new System.Drawing.Point(12, 12);
         this.foldValueTextLabelControl.Name = "foldValueTextLabelControl";
         this.foldValueTextLabelControl.Size = new System.Drawing.Size(310, 33);
         this.foldValueTextLabelControl.StyleController = this.layoutControl1;
         this.foldValueTextLabelControl.TabIndex = 5;
         this.foldValueTextLabelControl.Text = "foldValueTextLabelControl";
         // 
         // foldValueTextEdit
         // 
         this.foldValueTextEdit.Location = new System.Drawing.Point(340, 49);
         this.foldValueTextEdit.Name = "foldValueTextEdit";
         this.foldValueTextEdit.Size = new System.Drawing.Size(908, 48);
         this.foldValueTextEdit.StyleController = this.layoutControl1;
         this.foldValueTextEdit.TabIndex = 4;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.foldValueInputControlItem,
            this.layoutControlItem2,
            this.emptySpaceItem1,
            this.layoutControlItem1});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1260, 286);
         this.Root.TextVisible = false;
         // 
         // foldValueInputControlItem
         // 
         this.foldValueInputControlItem.Control = this.foldValueTextEdit;
         this.foldValueInputControlItem.Location = new System.Drawing.Point(0, 37);
         this.foldValueInputControlItem.Name = "foldValueInputControlItem";
         this.foldValueInputControlItem.Size = new System.Drawing.Size(1240, 52);
         this.foldValueInputControlItem.TextSize = new System.Drawing.Size(316, 33);
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.foldValueTextLabelControl;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(1240, 37);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 126);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(1240, 140);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.deviationLineDescriptionLabelControl;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 89);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(1240, 37);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // DeviationLinesView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "New Deviation Line";
         this.ClientSize = new System.Drawing.Size(1260, 396);
         this.Controls.Add(this.layoutControl1);
         this.Name = "DeviationLinesView";
         this.Text = "New Deviation Line";
         this.Controls.SetChildIndex(this.tablePanel, 0);
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.foldValueTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.foldValueInputControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.TextEdit foldValueTextEdit;
      private DevExpress.XtraLayout.LayoutControlItem foldValueInputControlItem;
      private DevExpress.XtraEditors.LabelControl foldValueTextLabelControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraEditors.LabelControl deviationLineDescriptionLabelControl;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
   }
}