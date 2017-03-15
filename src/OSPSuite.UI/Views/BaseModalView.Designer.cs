
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views
{
   partial class BaseModalView
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
         _shortcutsManager.Dispose();
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.btnOk = new UxSimpleButton();
         this.layoutControlBase = new UxLayoutControl();
         this.btnExtra = new UxSimpleButton();
         this.btnCancel = new UxSimpleButton();
         this.layoutControlGroupBase = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemOK = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemCancel = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItemBase = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutItemExtra = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).BeginInit();
         this.layoutControlBase.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).BeginInit();
         this.SuspendLayout();
         // 
         // btnOk
         // 
         this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnOk.Location = new System.Drawing.Point(252, 12);
         this.btnOk.Manager = null;
         this.btnOk.Name = "btnOk";
         this.btnOk.Shortcut = System.Windows.Forms.Keys.None;
         this.btnOk.Size = new System.Drawing.Size(79, 22);
         this.btnOk.StyleController = this.layoutControlBase;
         this.btnOk.TabIndex = 30;
         this.btnOk.Text = "btnOk";
         // 
         // layoutControlBase
         // 
         this.layoutControlBase.AllowCustomization = false;
         this.layoutControlBase.Controls.Add(this.btnExtra);
         this.layoutControlBase.Controls.Add(this.btnOk);
         this.layoutControlBase.Controls.Add(this.btnCancel);
         this.layoutControlBase.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.layoutControlBase.Location = new System.Drawing.Point(0, 170);
         this.layoutControlBase.Name = "layoutControlBase";
         this.layoutControlBase.Root = this.layoutControlGroupBase;
         this.layoutControlBase.Size = new System.Drawing.Size(414, 46);
         this.layoutControlBase.TabIndex = 33;
         this.layoutControlBase.Text = "layoutControl1";
         // 
         // btnExtra
         // 
         this.btnExtra.Location = new System.Drawing.Point(12, 12);
         this.btnExtra.Manager = null;
         this.btnExtra.Name = "btnExtra";
         this.btnExtra.Shortcut = System.Windows.Forms.Keys.None;
         this.btnExtra.Size = new System.Drawing.Size(116, 22);
         this.btnExtra.StyleController = this.layoutControlBase;
         this.btnExtra.TabIndex = 32;
         this.btnExtra.Text = "btnExtra";
         // 
         // btnCancel
         // 
         this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancel.Location = new System.Drawing.Point(335, 12);
         this.btnCancel.Manager = null;
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Shortcut = System.Windows.Forms.Keys.None;
         this.btnCancel.Size = new System.Drawing.Size(67, 22);
         this.btnCancel.StyleController = this.layoutControlBase;
         this.btnCancel.TabIndex = 31;
         this.btnCancel.Text = "btnCancel";
         // 
         // layoutControlGroupBase
         // 
         this.layoutControlGroupBase.CustomizationFormText = "layoutControlGroup";
         this.layoutControlGroupBase.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroupBase.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemOK,
            this.layoutItemCancel,
            this.emptySpaceItemBase,
            this.layoutItemExtra});
         this.layoutControlGroupBase.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroupBase.Name = "layoutControlGroupBase";
         this.layoutControlGroupBase.Size = new System.Drawing.Size(414, 46);
         this.layoutControlGroupBase.Text = "layoutControlGroup";
         this.layoutControlGroupBase.TextVisible = false;
         // 
         // layoutItemOK
         // 
         this.layoutItemOK.Control = this.btnOk;
         this.layoutItemOK.CustomizationFormText = "layoutControlItem1";
         this.layoutItemOK.Location = new System.Drawing.Point(240, 0);
         this.layoutItemOK.Name = "layoutItemOK";
         this.layoutItemOK.Size = new System.Drawing.Size(83, 26);
         this.layoutItemOK.Text = "layoutItemOK";
         this.layoutItemOK.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemOK.TextToControlDistance = 0;
         this.layoutItemOK.TextVisible = false;
         // 
         // layoutItemCancel
         // 
         this.layoutItemCancel.Control = this.btnCancel;
         this.layoutItemCancel.CustomizationFormText = "layoutControlItem2";
         this.layoutItemCancel.Location = new System.Drawing.Point(323, 0);
         this.layoutItemCancel.Name = "layoutItemCancel";
         this.layoutItemCancel.Size = new System.Drawing.Size(71, 26);
         this.layoutItemCancel.Text = "layoutItemCancel";
         this.layoutItemCancel.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemCancel.TextToControlDistance = 0;
         this.layoutItemCancel.TextVisible = false;
         // 
         // emptySpaceItemBase
         // 
         this.emptySpaceItemBase.AllowHotTrack = false;
         this.emptySpaceItemBase.CustomizationFormText = "emptySpaceItemBase";
         this.emptySpaceItemBase.Location = new System.Drawing.Point(120, 0);
         this.emptySpaceItemBase.Name = "emptySpaceItemBase";
         this.emptySpaceItemBase.Size = new System.Drawing.Size(120, 26);
         this.emptySpaceItemBase.Text = "emptySpaceItemBase";
         this.emptySpaceItemBase.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutItemExtra
         // 
         this.layoutItemExtra.Control = this.btnExtra;
         this.layoutItemExtra.CustomizationFormText = "layoutItemExtra";
         this.layoutItemExtra.Location = new System.Drawing.Point(0, 0);
         this.layoutItemExtra.Name = "layoutItemExtra";
         this.layoutItemExtra.Size = new System.Drawing.Size(120, 26);
         this.layoutItemExtra.Text = "layoutItemExtra";
         this.layoutItemExtra.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemExtra.TextToControlDistance = 0;
         this.layoutItemExtra.TextVisible = false;
         // 
         // BaseModalView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnCancel;
         this.Caption = "BaseModalView";
         this.ClientSize = new System.Drawing.Size(414, 216);
         this.Controls.Add(this.layoutControlBase);
         this.Name = "BaseModalView";
         this.Text = "BaseModalView";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).EndInit();
         this.layoutControlBase.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      protected UxSimpleButton btnCancel;
      protected UxSimpleButton btnOk;
      protected UxLayoutControl layoutControlBase;
      protected UxSimpleButton btnExtra;
      protected DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupBase;
      protected DevExpress.XtraLayout.LayoutControlItem layoutItemOK;
      protected DevExpress.XtraLayout.LayoutControlItem layoutItemCancel;
      protected DevExpress.XtraLayout.EmptySpaceItem emptySpaceItemBase;
      protected DevExpress.XtraLayout.LayoutControlItem layoutItemExtra;
   }
}