namespace OSPSuite.UI.Views
{
   partial class ExceptionView
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
         this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.btnSendEmail = new DevExpress.XtraEditors.SimpleButton();
         this.btnCopyToClipboard = new DevExpress.XtraEditors.SimpleButton();
         this.tbException = new DevExpress.XtraEditors.MemoEdit();
         this.tbFullException = new DevExpress.XtraEditors.MemoEdit();
         this.btnOk = new DevExpress.XtraEditors.SimpleButton();
         this.lblDescription = new DevExpress.XtraEditors.LabelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemOk = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutGroupStackTraceException = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemFullException = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupException = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemException = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemCopyToClipbord = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemSendEmail = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbException.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbFullException.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOk)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupStackTraceException)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFullException)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupException)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemException)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCopyToClipbord)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSendEmail)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.btnSendEmail);
         this.layoutControl.Controls.Add(this.btnCopyToClipboard);
         this.layoutControl.Controls.Add(this.tbException);
         this.layoutControl.Controls.Add(this.tbFullException);
         this.layoutControl.Controls.Add(this.btnOk);
         this.layoutControl.Controls.Add(this.lblDescription);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(482, 203, 250, 350);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(663, 522);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // btnSendEmail
         // 
         this.btnSendEmail.Location = new System.Drawing.Point(378, 29);
         this.btnSendEmail.Name = "btnSendEmail";
         this.btnSendEmail.Size = new System.Drawing.Size(273, 22);
         this.btnSendEmail.StyleController = this.layoutControl;
         this.btnSendEmail.TabIndex = 9;
         this.btnSendEmail.Text = "btnSendEmail";
         // 
         // btnCopyToClipboard
         // 
         this.btnCopyToClipboard.Location = new System.Drawing.Point(195, 29);
         this.btnCopyToClipboard.Name = "btnCopyToClipboard";
         this.btnCopyToClipboard.Size = new System.Drawing.Size(179, 22);
         this.btnCopyToClipboard.StyleController = this.layoutControl;
         this.btnCopyToClipboard.TabIndex = 8;
         this.btnCopyToClipboard.Text = "btnCopyToClipboard";
         // 
         // tbException
         // 
         this.tbException.Location = new System.Drawing.Point(142, 86);
         this.tbException.Name = "tbException";
         this.tbException.Size = new System.Drawing.Size(497, 152);
         this.tbException.StyleController = this.layoutControl;
         this.tbException.TabIndex = 6;
         // 
         // tbFullException
         // 
         this.tbFullException.Location = new System.Drawing.Point(142, 285);
         this.tbFullException.Name = "tbFullException";
         this.tbFullException.Size = new System.Drawing.Size(497, 187);
         this.tbFullException.StyleController = this.layoutControl;
         this.tbFullException.TabIndex = 5;
         // 
         // btnOk
         // 
         this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnOk.Location = new System.Drawing.Point(305, 488);
         this.btnOk.Name = "btnOk";
         this.btnOk.Size = new System.Drawing.Size(346, 22);
         this.btnOk.StyleController = this.layoutControl;
         this.btnOk.TabIndex = 4;
         this.btnOk.Text = "OK";
         // 
         // lblDescription
         // 
         this.lblDescription.Location = new System.Drawing.Point(12, 12);
         this.lblDescription.Name = "lblDescription";
         this.lblDescription.Size = new System.Drawing.Size(63, 13);
         this.lblDescription.StyleController = this.layoutControl;
         this.lblDescription.TabIndex = 7;
         this.lblDescription.Text = "lblDescription";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemOk,
            this.emptySpaceItem1,
            this.layoutGroupStackTraceException,
            this.layoutGroupException,
            this.layoutItemCopyToClipbord,
            this.layoutItemSendEmail,
            this.layoutItemDescription,
            this.emptySpaceItem2});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(663, 522);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemOk
         // 
         this.layoutItemOk.Control = this.btnOk;
         this.layoutItemOk.CustomizationFormText = "layoutControlItem1";
         this.layoutItemOk.Location = new System.Drawing.Point(293, 476);
         this.layoutItemOk.Name = "layoutItemOk";
         this.layoutItemOk.Size = new System.Drawing.Size(350, 26);
         this.layoutItemOk.Text = "layoutItemOk";
         this.layoutItemOk.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemOk.TextToControlDistance = 0;
         this.layoutItemOk.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 476);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(293, 26);
         this.emptySpaceItem1.Text = "emptySpaceItem1";
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutGroupStackTraceException
         // 
         this.layoutGroupStackTraceException.CustomizationFormText = "layoutControlGroup2";
         this.layoutGroupStackTraceException.ExpandButtonVisible = true;
         this.layoutGroupStackTraceException.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemFullException});
         this.layoutGroupStackTraceException.Location = new System.Drawing.Point(0, 242);
         this.layoutGroupStackTraceException.Name = "layoutGroupFullException";
         this.layoutGroupStackTraceException.Size = new System.Drawing.Size(643, 234);
         this.layoutGroupStackTraceException.Text = "layoutGroupFullException";
         // 
         // layoutItemFullException
         // 
         this.layoutItemFullException.Control = this.tbFullException;
         this.layoutItemFullException.CustomizationFormText = "layoutItemFullException";
         this.layoutItemFullException.Location = new System.Drawing.Point(0, 0);
         this.layoutItemFullException.Name = "layoutItemFullException";
         this.layoutItemFullException.Size = new System.Drawing.Size(619, 191);
         this.layoutItemFullException.Text = "layoutItemFullException";
         this.layoutItemFullException.TextSize = new System.Drawing.Size(115, 13);
         // 
         // layoutGroupException
         // 
         this.layoutGroupException.CustomizationFormText = "layoutControlGroup2";
         this.layoutGroupException.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemException});
         this.layoutGroupException.Location = new System.Drawing.Point(0, 43);
         this.layoutGroupException.Name = "layoutGroupException";
         this.layoutGroupException.Size = new System.Drawing.Size(643, 199);
         this.layoutGroupException.Text = "layoutGroupException";
         // 
         // layoutItemException
         // 
         this.layoutItemException.Control = this.tbException;
         this.layoutItemException.CustomizationFormText = "layoutItemException";
         this.layoutItemException.Location = new System.Drawing.Point(0, 0);
         this.layoutItemException.Name = "layoutItemException";
         this.layoutItemException.Size = new System.Drawing.Size(619, 156);
         this.layoutItemException.Text = "layoutItemException";
         this.layoutItemException.TextLocation = DevExpress.Utils.Locations.Default;
         this.layoutItemException.TextSize = new System.Drawing.Size(115, 13);
         // 
         // layoutItemCopyToClipbord
         // 
         this.layoutItemCopyToClipbord.Control = this.btnCopyToClipboard;
         this.layoutItemCopyToClipbord.CustomizationFormText = "layoutItemCopyToClipbord";
         this.layoutItemCopyToClipbord.Location = new System.Drawing.Point(183, 17);
         this.layoutItemCopyToClipbord.Name = "layoutItemCopyToClipbord";
         this.layoutItemCopyToClipbord.Size = new System.Drawing.Size(183, 26);
         this.layoutItemCopyToClipbord.Text = "layoutItemCopyToClipbord";
         this.layoutItemCopyToClipbord.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemCopyToClipbord.TextToControlDistance = 0;
         this.layoutItemCopyToClipbord.TextVisible = false;
         // 
         // layoutItemSendEmail
         // 
         this.layoutItemSendEmail.Control = this.btnSendEmail;
         this.layoutItemSendEmail.CustomizationFormText = "layoutItemSendEmail";
         this.layoutItemSendEmail.Location = new System.Drawing.Point(366, 17);
         this.layoutItemSendEmail.Name = "layoutItemSendEmail";
         this.layoutItemSendEmail.Size = new System.Drawing.Size(277, 26);
         this.layoutItemSendEmail.Text = "layoutItemSendEmail";
         this.layoutItemSendEmail.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemSendEmail.TextToControlDistance = 0;
         this.layoutItemSendEmail.TextVisible = false;
         // 
         // layoutItemDescription
         // 
         this.layoutItemDescription.Control = this.lblDescription;
         this.layoutItemDescription.CustomizationFormText = "layoutItemDescription";
         this.layoutItemDescription.Location = new System.Drawing.Point(0, 0);
         this.layoutItemDescription.Name = "layoutItemDescription";
         this.layoutItemDescription.Size = new System.Drawing.Size(643, 17);
         this.layoutItemDescription.Text = "layoutItemDescription";
         this.layoutItemDescription.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemDescription.TextToControlDistance = 0;
         this.layoutItemDescription.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 17);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(183, 26);
         this.emptySpaceItem2.Text = "emptySpaceItem2";
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // ExceptionView
         // 
         this.AcceptButton = this.btnOk;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnOk;
         this.ClientSize = new System.Drawing.Size(663, 522);
         this.Controls.Add(this.layoutControl);
         this.Name = "ExceptionView";
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "ExceptionView";
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tbException.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbFullException.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOk)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupStackTraceException)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFullException)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupException)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemException)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCopyToClipbord)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSendEmail)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraEditors.MemoEdit tbException;
      private DevExpress.XtraEditors.MemoEdit tbFullException;
      private DevExpress.XtraEditors.SimpleButton btnOk;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemOk;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupStackTraceException;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFullException;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupException;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemException;
      private DevExpress.XtraEditors.LabelControl lblDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDescription;
      private DevExpress.XtraEditors.SimpleButton btnSendEmail;
      private DevExpress.XtraEditors.SimpleButton btnCopyToClipboard;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemCopyToClipbord;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSendEmail;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
   }
}