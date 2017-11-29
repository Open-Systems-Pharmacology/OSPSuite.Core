namespace OSPSuite.UI.Views
{
   partial class LogView
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

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.tbLog = new DevExpress.XtraEditors.MemoEdit();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.chkInfo = new OSPSuite.UI.Controls.UxCheckEdit();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.chkWarning = new OSPSuite.UI.Controls.UxCheckEdit();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.chkError = new OSPSuite.UI.Controls.UxCheckEdit();
         this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
         this.chkDebug = new OSPSuite.UI.Controls.UxCheckEdit();
         this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbLog.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkInfo.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkWarning.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkError.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkDebug.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.chkDebug);
         this.layoutControl1.Controls.Add(this.chkError);
         this.layoutControl1.Controls.Add(this.chkWarning);
         this.layoutControl1.Controls.Add(this.chkInfo);
         this.layoutControl1.Controls.Add(this.tbLog);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(744, 523);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // layoutControlGroup1
         // 
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
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(744, 523);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // tbLog
         // 
         this.tbLog.Location = new System.Drawing.Point(2, 25);
         this.tbLog.Name = "tbLog";
         this.tbLog.Size = new System.Drawing.Size(740, 496);
         this.tbLog.StyleController = this.layoutControl1;
         this.tbLog.TabIndex = 9;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.tbLog;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 23);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(744, 500);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // chkInfo
         // 
         this.chkInfo.Location = new System.Drawing.Point(2, 2);
         this.chkInfo.Name = "chkInfo";
         this.chkInfo.Properties.Caption = "chkInfo";
         this.chkInfo.Size = new System.Drawing.Size(182, 19);
         this.chkInfo.StyleController = this.layoutControl1;
         this.chkInfo.TabIndex = 10;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.chkInfo;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(186, 23);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // chkWarning
         // 
         this.chkWarning.Location = new System.Drawing.Point(374, 2);
         this.chkWarning.Name = "chkWarning";
         this.chkWarning.Properties.Caption = "chkWarning";
         this.chkWarning.Size = new System.Drawing.Size(182, 19);
         this.chkWarning.StyleController = this.layoutControl1;
         this.chkWarning.TabIndex = 11;
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.chkWarning;
         this.layoutControlItem3.Location = new System.Drawing.Point(372, 0);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(186, 23);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // chkError
         // 
         this.chkError.Location = new System.Drawing.Point(188, 2);
         this.chkError.Name = "chkError";
         this.chkError.Properties.Caption = "chkError";
         this.chkError.Size = new System.Drawing.Size(182, 19);
         this.chkError.StyleController = this.layoutControl1;
         this.chkError.TabIndex = 12;
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.chkError;
         this.layoutControlItem4.Location = new System.Drawing.Point(186, 0);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Size = new System.Drawing.Size(186, 23);
         this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem4.TextVisible = false;
         // 
         // chkDebug
         // 
         this.chkDebug.Location = new System.Drawing.Point(560, 2);
         this.chkDebug.Name = "chkDebug";
         this.chkDebug.Properties.Caption = "chkDebug";
         this.chkDebug.Size = new System.Drawing.Size(182, 19);
         this.chkDebug.StyleController = this.layoutControl1;
         this.chkDebug.TabIndex = 13;
         // 
         // layoutControlItem5
         // 
         this.layoutControlItem5.Control = this.chkDebug;
         this.layoutControlItem5.Location = new System.Drawing.Point(558, 0);
         this.layoutControlItem5.Name = "layoutControlItem5";
         this.layoutControlItem5.Size = new System.Drawing.Size(186, 23);
         this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem5.TextVisible = false;
         // 
         // LogView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "LogView";
         this.Size = new System.Drawing.Size(744, 523);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbLog.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkInfo.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkWarning.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkError.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkDebug.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.MemoEdit tbLog;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraEditors.CheckEdit chkDebug;
      private DevExpress.XtraEditors.CheckEdit chkError;
      private DevExpress.XtraEditors.CheckEdit chkWarning;
      private DevExpress.XtraEditors.CheckEdit chkInfo;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
   }
}
