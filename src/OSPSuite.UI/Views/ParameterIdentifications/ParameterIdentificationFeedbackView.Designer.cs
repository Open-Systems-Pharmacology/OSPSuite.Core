namespace OSPSuite.UI.Views.ParameterIdentifications
{
   partial class ParameterIdentificationFeedbackView
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
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.chkRefreshFeedback = new OSPSuite.UI.Controls.UxCheckEdit();
         this.panelContent = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemRefreshFeedback = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.chkRefreshFeedback.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelContent)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRefreshFeedback)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.chkRefreshFeedback);
         this.layoutControl.Controls.Add(this.panelContent);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(784, 522);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // chkRefreshFeedback
         // 
         this.chkRefreshFeedback.Location = new System.Drawing.Point(12, 12);
         this.chkRefreshFeedback.Name = "chkRefreshFeedback";
         this.chkRefreshFeedback.Properties.Caption = "chkRefreshFeedback";
         this.chkRefreshFeedback.Size = new System.Drawing.Size(760, 19);
         this.chkRefreshFeedback.StyleController = this.layoutControl;
         this.chkRefreshFeedback.TabIndex = 5;
         // 
         // panelContent
         // 
         this.panelContent.Location = new System.Drawing.Point(12, 35);
         this.panelContent.Name = "panelContent";
         this.panelContent.Size = new System.Drawing.Size(760, 475);
         this.panelContent.TabIndex = 4;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutItemRefreshFeedback});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Size = new System.Drawing.Size(784, 522);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.panelContent;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 23);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(764, 479);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutItemRefreshFeedback
         // 
         this.layoutItemRefreshFeedback.Control = this.chkRefreshFeedback;
         this.layoutItemRefreshFeedback.Location = new System.Drawing.Point(0, 0);
         this.layoutItemRefreshFeedback.Name = "layoutItemRefreshFeedback";
         this.layoutItemRefreshFeedback.Size = new System.Drawing.Size(764, 23);
         this.layoutItemRefreshFeedback.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemRefreshFeedback.TextVisible = false;
         // 
         // ParameterIdentificationFeedbackView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "ParameterIdentificationFeedbackView";
         this.ClientSize = new System.Drawing.Size(784, 522);
         this.Controls.Add(this.layoutControl);
         this.Name = "ParameterIdentificationFeedbackView";
         this.Text = "ParameterIdentificationFeedbackView";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.chkRefreshFeedback.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelContent)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRefreshFeedback)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelContent;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraEditors.CheckEdit chkRefreshFeedback;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemRefreshFeedback;
   }
}