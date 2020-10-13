using OSPSuite.Assets;

namespace OSPSuite.Presentation.Importer.Views
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
         this.openSourceFileButton = new DevExpress.XtraEditors.SimpleButton();
         this.sourceFileTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.centralLayoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemExcelFile = new DevExpress.XtraLayout.LayoutControlItem();
         this.buttonLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.centralLayoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExcelFile)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonLayoutControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
         this.rootLayoutControl.AllowCustomization = false;
         this.rootLayoutControl.Controls.Add(this.openSourceFileButton);
         this.rootLayoutControl.Controls.Add(this.sourceFileTextEdit);
         this.rootLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.rootLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.rootLayoutControl.Margin = new System.Windows.Forms.Padding(8);
         this.rootLayoutControl.Name = "rootLayoutControl";
         this.rootLayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(226, 121, 250, 350);
         this.rootLayoutControl.Root = this.centralLayoutControlGroup;
         this.rootLayoutControl.Size = new System.Drawing.Size(1150, 66);
         this.rootLayoutControl.TabIndex = 6;
         // 
         // openSourceFileButton
         // 
         this.openSourceFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.openSourceFileButton.Location = new System.Drawing.Point(1085, 2);
         this.openSourceFileButton.Margin = new System.Windows.Forms.Padding(8);
         this.openSourceFileButton.Name = "openSourceFileButton";
         this.openSourceFileButton.Size = new System.Drawing.Size(63, 62);
         this.openSourceFileButton.StyleController = this.rootLayoutControl;
         this.openSourceFileButton.TabIndex = 1;
         this.openSourceFileButton.Text = Captions.Importer.ThreeDots;
         // 
         // sourceFileTextEdit
         // 
         this.sourceFileTextEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.sourceFileTextEdit.Location = new System.Drawing.Point(74, 8);
         this.sourceFileTextEdit.Margin = new System.Windows.Forms.Padding(8);
         this.sourceFileTextEdit.Name = "sourceFileTextEdit";
         this.sourceFileTextEdit.Properties.ReadOnly = true;
         this.sourceFileTextEdit.Size = new System.Drawing.Size(1004, 50);
         this.sourceFileTextEdit.StyleController = this.rootLayoutControl;
         this.sourceFileTextEdit.TabIndex = 2;
         // 
         // centralLayoutControlGroup
         // 
         this.centralLayoutControlGroup.CustomizationFormText = "centralLayoutControlGroup";
         this.centralLayoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.centralLayoutControlGroup.GroupBordersVisible = false;
         this.centralLayoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemExcelFile,
            this.buttonLayoutControlItem});
         this.centralLayoutControlGroup.Name = "centralLayoutControlGroup";
         this.centralLayoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.centralLayoutControlGroup.Size = new System.Drawing.Size(1150, 66);
         this.centralLayoutControlGroup.TextVisible = false;
         // 
         // layoutItemExcelFile
         // 
         this.layoutItemExcelFile.Control = this.sourceFileTextEdit;
         this.layoutItemExcelFile.CustomizationFormText = Captions.Importer.File;
         this.layoutItemExcelFile.Location = new System.Drawing.Point(0, 0);
         this.layoutItemExcelFile.Name = "layoutItemExcelFile";
         this.layoutItemExcelFile.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 8, 5);
         this.layoutItemExcelFile.Size = new System.Drawing.Size(1083, 66);
         this.layoutItemExcelFile.Text = Captions.Importer.File;
         this.layoutItemExcelFile.TextSize = new System.Drawing.Size(66, 33);
         // 
         // buttonLayoutControlItem
         // 
         this.buttonLayoutControlItem.Control = this.openSourceFileButton;
         this.buttonLayoutControlItem.CustomizationFormText = "buttonLayoutControlItem";
         this.buttonLayoutControlItem.Location = new System.Drawing.Point(1083, 0);
         this.buttonLayoutControlItem.MaxSize = new System.Drawing.Size(67, 66);
         this.buttonLayoutControlItem.MinSize = new System.Drawing.Size(67, 66);
         this.buttonLayoutControlItem.Name = "buttonLayoutControlItem";
         this.buttonLayoutControlItem.Size = new System.Drawing.Size(67, 66);
         this.buttonLayoutControlItem.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.buttonLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.buttonLayoutControlItem.TextVisible = false;
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
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.centralLayoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExcelFile)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonLayoutControlItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton openSourceFileButton;
      private DevExpress.XtraEditors.TextEdit sourceFileTextEdit;
      private DevExpress.XtraLayout.LayoutControlGroup centralLayoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemExcelFile;
      private DevExpress.XtraLayout.LayoutControlItem buttonLayoutControlItem;
      private UI.Controls.UxLayoutControl rootLayoutControl;
   }
}
