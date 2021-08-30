using OSPSuite.Assets;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Importer
{
   partial class MetaDataParameterEditorView : BaseUserControl
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
         this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.manualInput = new DevExpress.XtraEditors.TextEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.manualInputControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.manualnputLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.manualInputLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.manualInput.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.manualInputControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.manualnputLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.manualInputLayoutControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.manualInput);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(6);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(415, 82);
         this.layoutControl.TabIndex = 38;
         // 
         // manualInput
         // 
         this.manualInput.Location = new System.Drawing.Point(101, 24);
         this.manualInput.Name = "manualInput";
         this.manualInput.Size = new System.Drawing.Size(290, 22);
         this.manualInput.StyleController = this.layoutControl;
         this.manualInput.TabIndex = 0;
         // 
         // Root
         // 
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(415, 82);
         this.Root.TextVisible = false;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.manualInputControlGroup});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(395, 62);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 50);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(395, 12);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // manualInputControlGroup
         // 
         this.manualInputControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.manualnputLayoutControlItem});
         this.manualInputControlGroup.Location = new System.Drawing.Point(0, 0);
         this.manualInputControlGroup.Name = "ErrorLayoutControlGroup";
         this.manualInputControlGroup.Size = new System.Drawing.Size(395, 50);
         this.manualInputControlGroup.TextVisible = false;
         // 
         // manualnputLayoutControlItem
         // 
         this.manualnputLayoutControlItem.Control = this.manualInput;
         this.manualnputLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.manualnputLayoutControlItem.Name = "manual input";
         this.manualnputLayoutControlItem.Size = new System.Drawing.Size(371, 26);
         this.manualnputLayoutControlItem.TextSize = new System.Drawing.Size(74, 16);
         // 
         // manualInputLayoutControlItem
         // 
         this.manualInputLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.manualInputLayoutControlItem.Name = "manualInputLayoutControlItem";
         this.manualInputLayoutControlItem.Size = new System.Drawing.Size(0, 0);
         this.manualInputLayoutControlItem.TextSize = new System.Drawing.Size(50, 20);
         // 
         // MetaDataParameterEditorView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Margin = new System.Windows.Forms.Padding(8);
         this.Name = "MetaDataParameterEditorView";
         this.Size = new System.Drawing.Size(415, 82);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.manualInput.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.manualInputControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.manualnputLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.manualInputLayoutControlItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem manualInputLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlGroup manualInputControlGroup;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.TextEdit manualInput;
      private DevExpress.XtraLayout.LayoutControlItem manualnputLayoutControlItem;
   }
}
