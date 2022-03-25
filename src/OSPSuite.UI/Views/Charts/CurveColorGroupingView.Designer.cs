
namespace OSPSuite.UI.Views.Charts
{
   partial class CurveColorGroupingView
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
         this.metaDataCheckedListBoxControl = new DevExpress.XtraEditors.CheckedListBoxControl();
         this.colorGroupingDescriptionLabelControl = new DevExpress.XtraEditors.LabelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.applyColorGroupingButton = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.metaDataCheckedListBoxControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.applyColorGroupingButton);
         this.layoutControl1.Controls.Add(this.metaDataCheckedListBoxControl);
         this.layoutControl1.Controls.Add(this.colorGroupingDescriptionLabelControl);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.Root;
         this.layoutControl1.Size = new System.Drawing.Size(1035, 548);
         this.layoutControl1.TabIndex = 38;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // metaDataCheckedListBoxControl
         // 
         this.metaDataCheckedListBoxControl.Location = new System.Drawing.Point(12, 49);
         this.metaDataCheckedListBoxControl.Name = "metaDataCheckedListBoxControl";
         this.metaDataCheckedListBoxControl.Size = new System.Drawing.Size(1011, 409);
         this.metaDataCheckedListBoxControl.StyleController = this.layoutControl1;
         this.metaDataCheckedListBoxControl.TabIndex = 5;
         // 
         // colorGroupingDescriptionLabelControl
         // 
         this.colorGroupingDescriptionLabelControl.Location = new System.Drawing.Point(12, 12);
         this.colorGroupingDescriptionLabelControl.Name = "colorGroupingDescriptionLabelControl";
         this.colorGroupingDescriptionLabelControl.Size = new System.Drawing.Size(447, 33);
         this.colorGroupingDescriptionLabelControl.StyleController = this.layoutControl1;
         this.colorGroupingDescriptionLabelControl.TabIndex = 4;
         this.colorGroupingDescriptionLabelControl.Text = "colorGroupingDescriptionLabelControl";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.emptySpaceItem1,
            this.layoutControlItem3,
            this.emptySpaceItem2});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1035, 548);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.colorGroupingDescriptionLabelControl;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(1015, 37);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.metaDataCheckedListBoxControl;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 37);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(1015, 413);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 508);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(1015, 20);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // applyColorGroupingButton
         // 
         this.applyColorGroupingButton.Location = new System.Drawing.Point(519, 462);
         this.applyColorGroupingButton.Name = "applyColorGroupingButton";
         this.applyColorGroupingButton.Size = new System.Drawing.Size(504, 54);
         this.applyColorGroupingButton.StyleController = this.layoutControl1;
         this.applyColorGroupingButton.TabIndex = 6;
         this.applyColorGroupingButton.Text = "applyColorGroupingButton";
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.applyColorGroupingButton;
         this.layoutControlItem3.Location = new System.Drawing.Point(507, 450);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(508, 58);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 450);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(507, 58);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // CurveColorGroupingView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "CurveColorGroupingView";
         this.Controls.Add(this.layoutControl1);
         this.Name = "CurveColorGroupingView";
         this.Size = new System.Drawing.Size(1035, 548);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.metaDataCheckedListBoxControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraEditors.CheckedListBoxControl metaDataCheckedListBoxControl;
      private DevExpress.XtraEditors.LabelControl colorGroupingDescriptionLabelControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraEditors.SimpleButton applyColorGroupingButton;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
   }
}