namespace OSPSuite.UI.Views.Importer
{
   partial class NamingPatternView
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
         this.cbPatternComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemNamingPattern = new DevExpress.XtraLayout.LayoutControlItem();
         this.lblNamingPatternDescription = new DevExpress.XtraEditors.LabelControl();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbPatternComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNamingPattern)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.lblNamingPatternDescription);
         this.layoutControl1.Controls.Add(this.cbPatternComboBox);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(493, 49);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // cbPatternComboBox
         // 
         this.cbPatternComboBox.Location = new System.Drawing.Point(128, 2);
         this.cbPatternComboBox.Name = "cbPatternComboBox";
         this.cbPatternComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbPatternComboBox.Size = new System.Drawing.Size(363, 20);
         this.cbPatternComboBox.StyleController = this.layoutControl1;
         this.cbPatternComboBox.TabIndex = 7;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemNamingPattern,
            this.layoutControlItem1});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(493, 49);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemNamingPattern
         // 
         this.layoutItemNamingPattern.Control = this.cbPatternComboBox;
         this.layoutItemNamingPattern.CustomizationFormText = "layoutItemNamingPattern";
         this.layoutItemNamingPattern.Location = new System.Drawing.Point(0, 0);
         this.layoutItemNamingPattern.Name = "layoutItemNamingPattern";
         this.layoutItemNamingPattern.Size = new System.Drawing.Size(493, 24);
         this.layoutItemNamingPattern.TextSize = new System.Drawing.Size(123, 13);
         // 
         // lblNamingPatternDescription
         // 
         this.lblNamingPatternDescription.Location = new System.Drawing.Point(2, 26);
         this.lblNamingPatternDescription.Name = "lblNamingPatternDescription";
         this.lblNamingPatternDescription.Size = new System.Drawing.Size(489, 21);
         this.lblNamingPatternDescription.StyleController = this.layoutControl1;
         this.lblNamingPatternDescription.TabIndex = 8;
         this.lblNamingPatternDescription.Text = "lblNamingPatternDescription";
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.lblNamingPatternDescription;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItem1.MinSize = new System.Drawing.Size(138, 17);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(493, 25);
         this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // NamingPatternView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "NamingPatternView";
         this.Size = new System.Drawing.Size(493, 49);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cbPatternComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNamingPattern)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.ComboBoxEdit cbPatternComboBox;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemNamingPattern;
      private DevExpress.XtraEditors.LabelControl lblNamingPatternDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
   }
}
