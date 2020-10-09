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
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.openSourceFileButton = new DevExpress.XtraEditors.SimpleButton();
         this.sourceFileTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemExcelFile = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExcelFile)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.openSourceFileButton);
         this.layoutControl1.Controls.Add(this.sourceFileTextEdit);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Margin = new System.Windows.Forms.Padding(8);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(226, 121, 250, 350);
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(1150, 66);
         this.layoutControl1.TabIndex = 6;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // openSourceFileButton
         // 
         this.openSourceFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.openSourceFileButton.Location = new System.Drawing.Point(1085, 2);
         this.openSourceFileButton.Margin = new System.Windows.Forms.Padding(8);
         this.openSourceFileButton.Name = "openSourceFileButton";
         this.openSourceFileButton.Size = new System.Drawing.Size(63, 62);
         this.openSourceFileButton.StyleController = this.layoutControl1;
         this.openSourceFileButton.TabIndex = 1;
         this.openSourceFileButton.Text = "...";
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
         this.sourceFileTextEdit.StyleController = this.layoutControl1;
         this.sourceFileTextEdit.TabIndex = 2;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemExcelFile,
            this.layoutControlItem2});
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(1150, 66);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemExcelFile
         // 
         this.layoutItemExcelFile.Control = this.sourceFileTextEdit;
         this.layoutItemExcelFile.CustomizationFormText = "File: ";
         this.layoutItemExcelFile.Location = new System.Drawing.Point(0, 0);
         this.layoutItemExcelFile.Name = "layoutItemExcelFile";
         this.layoutItemExcelFile.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 8, 5);
         this.layoutItemExcelFile.Size = new System.Drawing.Size(1083, 66);
         this.layoutItemExcelFile.Text = "File:  ";
         this.layoutItemExcelFile.TextSize = new System.Drawing.Size(66, 33);
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.openSourceFileButton;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(1083, 0);
         this.layoutControlItem2.MaxSize = new System.Drawing.Size(67, 66);
         this.layoutControlItem2.MinSize = new System.Drawing.Size(67, 66);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(67, 66);
         this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // SourceFileControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "SourceFileControl";
         this.Size = new System.Drawing.Size(1150, 66);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExcelFile)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton openSourceFileButton;
      private DevExpress.XtraEditors.TextEdit sourceFileTextEdit;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemExcelFile;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private UI.Controls.UxLayoutControl layoutControl1;
   }
}
