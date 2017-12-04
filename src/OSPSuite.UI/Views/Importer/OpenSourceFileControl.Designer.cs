namespace OSPSuite.UI.Views.Importer
{
   partial class OpenSourceFileControl
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
         cleanMemory();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.btnSelectExcelFile = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.txtExcelFile = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemExcelFile = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtExcelFile.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExcelFile)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // btnSelectExcelFile
         // 
         this.btnSelectExcelFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnSelectExcelFile.Location = new System.Drawing.Point(435, 2);
         this.btnSelectExcelFile.Name = "btnSelectExcelFile";
         this.btnSelectExcelFile.Size = new System.Drawing.Size(23, 22);
         this.btnSelectExcelFile.StyleController = this.layoutControl1;
         this.btnSelectExcelFile.TabIndex = 1;
         this.btnSelectExcelFile.Text = "...";
         this.btnSelectExcelFile.Click += new System.EventHandler(this.btnSelectExcelFileClick);
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.btnSelectExcelFile);
         this.layoutControl1.Controls.Add(this.txtExcelFile);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(226, 121, 250, 350);
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(460, 26);
         this.layoutControl1.TabIndex = 6;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // txtExcelFile
         // 
         this.txtExcelFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.txtExcelFile.Location = new System.Drawing.Point(98, 3);
         this.txtExcelFile.Name = "txtExcelFile";
         this.txtExcelFile.Properties.ReadOnly = true;
         this.txtExcelFile.Size = new System.Drawing.Size(333, 20);
         this.txtExcelFile.StyleController = this.layoutControl1;
         this.txtExcelFile.TabIndex = 2;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemExcelFile,
            this.layoutControlItem2});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(460, 26);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemExcelFile
         // 
         this.layoutItemExcelFile.Control = this.txtExcelFile;
         this.layoutItemExcelFile.CustomizationFormText = "layoutItemExcelFile";
         this.layoutItemExcelFile.Location = new System.Drawing.Point(0, 0);
         this.layoutItemExcelFile.Name = "layoutItemExcelFile";
         this.layoutItemExcelFile.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 2);
         this.layoutItemExcelFile.Size = new System.Drawing.Size(433, 26);
         this.layoutItemExcelFile.Text = "layoutItemExcelFile";
         this.layoutItemExcelFile.TextSize = new System.Drawing.Size(93, 13);
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.btnSelectExcelFile;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(433, 0);
         this.layoutControlItem2.MaxSize = new System.Drawing.Size(27, 26);
         this.layoutControlItem2.MinSize = new System.Drawing.Size(27, 26);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(27, 26);
         this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem2.Text = "layoutControlItem2";
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextToControlDistance = 0;
         this.layoutControlItem2.TextVisible = false;
         // 
         // OpenSourceFileControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "OpenSourceFileControl";
         this.Size = new System.Drawing.Size(460, 26);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.txtExcelFile.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExcelFile)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton btnSelectExcelFile;
      private DevExpress.XtraEditors.TextEdit txtExcelFile;
      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemExcelFile;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;


   }
}
