using OSPSuite.UI.Controls;

namespace OSPSuite.Starter.Views
{
   partial class SimpleUIStarterView
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
         this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
         this.rightHandCaptionCheckBox = new UxCheckEdit();
         this.allowChecksOutsideControlAreaCheckBox = new UxCheckEdit();
         this.leftHandCaptionCheckBox = new UxCheckEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
         this.btnShowInputDialog = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.rightHandCaptionCheckBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.allowChecksOutsideControlAreaCheckBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.leftHandCaptionCheckBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.btnShowInputDialog);
         this.layoutControl1.Controls.Add(this.simpleButton1);
         this.layoutControl1.Controls.Add(this.rightHandCaptionCheckBox);
         this.layoutControl1.Controls.Add(this.allowChecksOutsideControlAreaCheckBox);
         this.layoutControl1.Controls.Add(this.leftHandCaptionCheckBox);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(284, 262);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // simpleButton1
         // 
         this.simpleButton1.Location = new System.Drawing.Point(12, 81);
         this.simpleButton1.Name = "simpleButton1";
         this.simpleButton1.Size = new System.Drawing.Size(260, 22);
         this.simpleButton1.StyleController = this.layoutControl1;
         this.simpleButton1.TabIndex = 7;
         this.simpleButton1.Text = "simpleButton1";
         this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
         // 
         // rightHandCaptionCheckBox
         // 
         this.rightHandCaptionCheckBox.AllowClicksOutsideControlArea = false;
         this.rightHandCaptionCheckBox.Location = new System.Drawing.Point(12, 35);
         this.rightHandCaptionCheckBox.Name = "rightHandCaptionCheckBox";
         this.rightHandCaptionCheckBox.Properties.Caption = "checkEdit3";
         this.rightHandCaptionCheckBox.Size = new System.Drawing.Size(260, 19);
         this.rightHandCaptionCheckBox.StyleController = this.layoutControl1;
         this.rightHandCaptionCheckBox.TabIndex = 6;
         // 
         // allowChecksOutsideControlAreaCheckBox
         // 
         this.allowChecksOutsideControlAreaCheckBox.AllowClicksOutsideControlArea = false;
         this.allowChecksOutsideControlAreaCheckBox.Location = new System.Drawing.Point(12, 12);
         this.allowChecksOutsideControlAreaCheckBox.Name = "allowChecksOutsideControlAreaCheckBox";
         this.allowChecksOutsideControlAreaCheckBox.Properties.Caption = "Allow Clicks Outside Control Area";
         this.allowChecksOutsideControlAreaCheckBox.Size = new System.Drawing.Size(260, 19);
         this.allowChecksOutsideControlAreaCheckBox.StyleController = this.layoutControl1;
         this.allowChecksOutsideControlAreaCheckBox.TabIndex = 5;
         // 
         // leftHandCaptionCheckBox
         // 
         this.leftHandCaptionCheckBox.AllowClicksOutsideControlArea = false;
         this.leftHandCaptionCheckBox.Location = new System.Drawing.Point(12, 58);
         this.leftHandCaptionCheckBox.Name = "leftHandCaptionCheckBox";
         this.leftHandCaptionCheckBox.Properties.Caption = "checkEdit1";
         this.leftHandCaptionCheckBox.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.leftHandCaptionCheckBox.Size = new System.Drawing.Size(260, 19);
         this.leftHandCaptionCheckBox.StyleController = this.layoutControl1;
         this.leftHandCaptionCheckBox.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(284, 262);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.leftHandCaptionCheckBox;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 46);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(264, 23);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.allowChecksOutsideControlAreaCheckBox;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(264, 23);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.rightHandCaptionCheckBox;
         this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 23);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(264, 23);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.simpleButton1;
         this.layoutControlItem4.Location = new System.Drawing.Point(0, 69);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Size = new System.Drawing.Size(264, 26);
         this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem4.TextVisible = false;
         // 
         // btnShowInputDialog
         // 
         this.btnShowInputDialog.Location = new System.Drawing.Point(12, 107);
         this.btnShowInputDialog.Name = "btnShowInputDialog";
         this.btnShowInputDialog.Size = new System.Drawing.Size(260, 22);
         this.btnShowInputDialog.StyleController = this.layoutControl1;
         this.btnShowInputDialog.TabIndex = 8;
         this.btnShowInputDialog.Text = "btnShowInputDialog";
         this.btnShowInputDialog.Click += new System.EventHandler(this.btnShowInputDialog_Click);
         // 
         // layoutControlItem5
         // 
         this.layoutControlItem5.Control = this.btnShowInputDialog;
         this.layoutControlItem5.Location = new System.Drawing.Point(0, 95);
         this.layoutControlItem5.Name = "layoutControlItem5";
         this.layoutControlItem5.Size = new System.Drawing.Size(264, 147);
         this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem5.TextVisible = false;
         // 
         // Starter
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(284, 262);
         this.Controls.Add(this.layoutControl1);
         this.Name = "Starter";
         this.Text = "Form1";
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.rightHandCaptionCheckBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.allowChecksOutsideControlAreaCheckBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.leftHandCaptionCheckBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private UxCheckEdit leftHandCaptionCheckBox;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private UxCheckEdit allowChecksOutsideControlAreaCheckBox;
      private UxCheckEdit rightHandCaptionCheckBox;
      private DevExpress.XtraEditors.SimpleButton simpleButton1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
      private DevExpress.XtraEditors.SimpleButton btnShowInputDialog;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;

   }
}

