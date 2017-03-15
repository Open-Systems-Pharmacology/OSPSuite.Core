using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views
{
   partial class ObjectBaseView
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
         _screenBinder.Dispose();
         base.Dispose(disposing);
      } 

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl1 = new UxLayoutControl();
         this.tbDescription = new DevExpress.XtraEditors.MemoEdit();
         this.tbName = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).BeginInit();
         this.layoutControlBase.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbDescription.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).BeginInit();
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(341, 12);
         this.btnCancel.Size = new System.Drawing.Size(68, 22);
         // 
         // btnOk
         // 
         this.btnOk.Location = new System.Drawing.Point(257, 12);
         this.btnOk.Size = new System.Drawing.Size(80, 22);
         // 
         // layoutControlBase
         // 
         this.layoutControlBase.Location = new System.Drawing.Point(0, 217);
         this.layoutControlBase.Size = new System.Drawing.Size(421, 46);
         this.layoutControlBase.Controls.SetChildIndex(this.btnCancel, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnOk, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnExtra, 0);
         // 
         // btnExtra
         // 
         this.btnExtra.Size = new System.Drawing.Size(118, 22);
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.tbDescription);
         this.layoutControl1.Controls.Add(this.tbName);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(421, 217);
         this.layoutControl1.TabIndex = 34;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // tbDescription
         // 
         this.tbDescription.Location = new System.Drawing.Point(12, 68);
         this.tbDescription.Name = "tbDescription";
         this.tbDescription.Size = new System.Drawing.Size(397, 137);
         this.tbDescription.StyleController = this.layoutControl1;
         this.tbDescription.TabIndex = 5;
         // 
         // tbName
         // 
         this.tbName.Location = new System.Drawing.Point(12, 28);
         this.tbName.Name = "tbName";
         this.tbName.Size = new System.Drawing.Size(397, 20);
         this.tbName.StyleController = this.layoutControl1;
         this.tbName.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemName,
            this.layoutItemDescription});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(421, 217);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemName
         // 
         this.layoutItemName.Control = this.tbName;
         this.layoutItemName.CustomizationFormText = "layoutItemName";
         this.layoutItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutItemName.Name = "layoutItemName";
         this.layoutItemName.Size = new System.Drawing.Size(401, 40);
         this.layoutItemName.Text = "layoutItemName";
         this.layoutItemName.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutItemName.TextSize = new System.Drawing.Size(105, 13);
         // 
         // layoutItemDescription
         // 
         this.layoutItemDescription.Control = this.tbDescription;
         this.layoutItemDescription.CustomizationFormText = "layoutItemDescription";
         this.layoutItemDescription.Location = new System.Drawing.Point(0, 40);
         this.layoutItemDescription.Name = "layoutItemDescription";
         this.layoutItemDescription.Size = new System.Drawing.Size(401, 157);
         this.layoutItemDescription.Text = "layoutItemDescription";
         this.layoutItemDescription.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutItemDescription.TextSize = new System.Drawing.Size(105, 13);
         // 
         // ObjectBaseView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "ObjectBaseView";
         this.ClientSize = new System.Drawing.Size(421, 263);
         this.Controls.Add(this.layoutControl1);
         this.Name = "ObjectBaseView";
         this.Text = "ObjectBaseView";
         this.Controls.SetChildIndex(this.layoutControlBase, 0);
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).EndInit();
         this.layoutControlBase.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tbDescription.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      protected DevExpress.XtraEditors.MemoEdit tbDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemName;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDescription;
      protected DevExpress.XtraEditors.TextEdit tbName;

   }
}