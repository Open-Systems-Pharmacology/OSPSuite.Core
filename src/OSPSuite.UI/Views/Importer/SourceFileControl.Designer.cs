using OSPSuite.Assets;

namespace OSPSuite.UI.Views.Importer
{
   partial class SourceFileControl
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
         this.rootLayoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.openSourceFileButtonEdit = new DevExpress.XtraEditors.ButtonEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.OpenSourceFileLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.openSourceFileButtonEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.OpenSourceFileLayoutControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
         this.rootLayoutControl.AllowCustomization = false;
         this.rootLayoutControl.Controls.Add(this.openSourceFileButtonEdit);
         this.rootLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.rootLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.rootLayoutControl.Margin = new System.Windows.Forms.Padding(8);
         this.rootLayoutControl.Name = "rootLayoutControl";
         this.rootLayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(226, 121, 250, 350);
         this.rootLayoutControl.Root = this.Root;
         this.rootLayoutControl.Size = new System.Drawing.Size(1150, 66);
         this.rootLayoutControl.TabIndex = 6;
         // 
         // openSourceFileButtonEdit
         // 
         this.openSourceFileButtonEdit.Location = new System.Drawing.Point(419, 2);
         this.openSourceFileButtonEdit.Name = "openSourceFileButtonEdit";
         this.openSourceFileButtonEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.openSourceFileButtonEdit.Size = new System.Drawing.Size(729, 48);
         this.openSourceFileButtonEdit.StyleController = this.rootLayoutControl;
         this.openSourceFileButtonEdit.TabIndex = 4;
         // 
         // Root
         // 
         this.Root.CustomizationFormText = "Root";
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.OpenSourceFileLayoutControlItem});
         this.Root.Name = "Root";
         this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.Root.Size = new System.Drawing.Size(1150, 66);
         this.Root.TextVisible = false;
         // 
         // OpenSourceFileLayoutControlItem
         // 
         this.OpenSourceFileLayoutControlItem.Control = this.openSourceFileButtonEdit;
         this.OpenSourceFileLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.OpenSourceFileLayoutControlItem.Name = "OpenSourceFileLayoutControlItem";
         this.OpenSourceFileLayoutControlItem.Size = new System.Drawing.Size(1150, 66);
         this.OpenSourceFileLayoutControlItem.TextSize = new System.Drawing.Size(405, 33);
         // 
         // SourceFileControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.rootLayoutControl);
         this.Name = "SourceFileControl";
         this.Size = new System.Drawing.Size(1150, 66);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).EndInit();
         this.rootLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.openSourceFileButtonEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.OpenSourceFileLayoutControlItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private UI.Controls.UxLayoutControl rootLayoutControl;
      private DevExpress.XtraEditors.ButtonEdit openSourceFileButtonEdit;
      private DevExpress.XtraLayout.LayoutControlItem OpenSourceFileLayoutControlItem;
   }
}
