using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views
{
   partial class WizardView
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
         WizardPresenter = null;
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
         this.btnCancel = new UxSimpleButton();
         this.btnNext = new UxSimpleButton();
         this.btnPrevious = new UxSimpleButton();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemCancel = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemOK = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemNext = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemPrevious = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItemBase = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).BeginInit();
         this.layoutControlBase.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNext)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPrevious)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).BeginInit();
         this.SuspendLayout();
         // 
         // btnOk
         // 
         this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnOk.Location = new System.Drawing.Point(376, 12);
         this.btnOk.Manager = null;
         this.btnOk.Name = "btnOk";
         this.btnOk.Shortcut = System.Windows.Forms.Keys.None;
         this.btnOk.Size = new System.Drawing.Size(40, 22);
         this.btnOk.StyleController = this.layoutControlBase;
         this.btnOk.TabIndex = 38;
         this.btnOk.Text = "btnOk";
         // 
         // layoutControlBase
         // 
         this.layoutControlBase.AllowCustomization = false;
         this.layoutControlBase.Controls.Add(this.btnCancel);
         this.layoutControlBase.Controls.Add(this.btnOk);
         this.layoutControlBase.Controls.Add(this.btnNext);
         this.layoutControlBase.Controls.Add(this.btnPrevious);
         this.layoutControlBase.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.layoutControlBase.Location = new System.Drawing.Point(0, 451);
         this.layoutControlBase.Name = "layoutControlBase";
         this.layoutControlBase.Root = this.layoutControlGroup;
         this.layoutControlBase.Size = new System.Drawing.Size(526, 46);
         this.layoutControlBase.TabIndex = 36;
         this.layoutControlBase.Text = "layoutControl1";
         // 
         // btnCancel
         // 
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancel.Location = new System.Drawing.Point(420, 12);
         this.btnCancel.Manager = null;
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Shortcut = System.Windows.Forms.Keys.None;
         this.btnCancel.Size = new System.Drawing.Size(94, 22);
         this.btnCancel.StyleController = this.layoutControlBase;
         this.btnCancel.TabIndex = 37;
         this.btnCancel.Text = "btnCancel";
         // 
         // btnNext
         // 
         this.btnNext.Location = new System.Drawing.Point(322, 12);
         this.btnNext.Manager = null;
         this.btnNext.Name = "btnNext";
         this.btnNext.Shortcut = System.Windows.Forms.Keys.None;
         this.btnNext.Size = new System.Drawing.Size(50, 22);
         this.btnNext.StyleController = this.layoutControlBase;
         this.btnNext.TabIndex = 39;
         this.btnNext.Text = "btnNext";
         // 
         // btnPrevious
         // 
         this.btnPrevious.Location = new System.Drawing.Point(250, 12);
         this.btnPrevious.Manager = null;
         this.btnPrevious.Name = "btnPrevious";
         this.btnPrevious.Shortcut = System.Windows.Forms.Keys.None;
         this.btnPrevious.Size = new System.Drawing.Size(68, 22);
         this.btnPrevious.StyleController = this.layoutControlBase;
         this.btnPrevious.TabIndex = 40;
         this.btnPrevious.Text = "btnPrevious";
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.CustomizationFormText = "Root";
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemCancel,
            this.layoutItemOK,
            this.layoutItemNext,
            this.layoutItemPrevious,
            this.emptySpaceItemBase});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "Root";
         this.layoutControlGroup.Size = new System.Drawing.Size(526, 46);
         this.layoutControlGroup.Text = "Root";
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemCancel
         // 
         this.layoutItemCancel.Control = this.btnCancel;
         this.layoutItemCancel.CustomizationFormText = "layoutControlItem1";
         this.layoutItemCancel.Location = new System.Drawing.Point(408, 0);
         this.layoutItemCancel.Name = "layoutItemCancel";
         this.layoutItemCancel.Size = new System.Drawing.Size(98, 26);
         this.layoutItemCancel.Text = "layoutItemCancel";
         this.layoutItemCancel.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemCancel.TextToControlDistance = 0;
         this.layoutItemCancel.TextVisible = false;
         // 
         // layoutItemOK
         // 
         this.layoutItemOK.Control = this.btnOk;
         this.layoutItemOK.CustomizationFormText = "layoutControlItem2";
         this.layoutItemOK.Location = new System.Drawing.Point(364, 0);
         this.layoutItemOK.Name = "layoutItemOK";
         this.layoutItemOK.Size = new System.Drawing.Size(44, 26);
         this.layoutItemOK.Text = "layoutItemOK";
         this.layoutItemOK.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemOK.TextToControlDistance = 0;
         this.layoutItemOK.TextVisible = false;
         // 
         // layoutItemNext
         // 
         this.layoutItemNext.Control = this.btnNext;
         this.layoutItemNext.CustomizationFormText = "layoutControlItem3";
         this.layoutItemNext.Location = new System.Drawing.Point(310, 0);
         this.layoutItemNext.Name = "layoutItemNext";
         this.layoutItemNext.Size = new System.Drawing.Size(54, 26);
         this.layoutItemNext.Text = "layoutItemNext";
         this.layoutItemNext.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemNext.TextToControlDistance = 0;
         this.layoutItemNext.TextVisible = false;
         // 
         // layoutItemPrevious
         // 
         this.layoutItemPrevious.Control = this.btnPrevious;
         this.layoutItemPrevious.CustomizationFormText = "layoutControlItem4";
         this.layoutItemPrevious.Location = new System.Drawing.Point(238, 0);
         this.layoutItemPrevious.Name = "layoutItemPrevious";
         this.layoutItemPrevious.Size = new System.Drawing.Size(72, 26);
         this.layoutItemPrevious.Text = "layoutItemPrevious";
         this.layoutItemPrevious.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemPrevious.TextToControlDistance = 0;
         this.layoutItemPrevious.TextVisible = false;
         // 
         // emptySpaceItemBase
         // 
         this.emptySpaceItemBase.AllowHotTrack = false;
         this.emptySpaceItemBase.CustomizationFormText = "emptySpaceItemBase";
         this.emptySpaceItemBase.Location = new System.Drawing.Point(0, 0);
         this.emptySpaceItemBase.Name = "emptySpaceItemBase";
         this.emptySpaceItemBase.Size = new System.Drawing.Size(238, 26);
         this.emptySpaceItemBase.Text = "emptySpaceItemBase";
         this.emptySpaceItemBase.TextSize = new System.Drawing.Size(0, 0);
         // 
         // WizardView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnCancel;
         this.Caption = "WizardView";
         this.ClientSize = new System.Drawing.Size(526, 497);
         this.Controls.Add(this.layoutControlBase);
         this.Name = "WizardView";
         this.Text = "WizardView";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).EndInit();
         this.layoutControlBase.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNext)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPrevious)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      protected UxSimpleButton btnPrevious;
      protected UxSimpleButton btnNext;
      protected UxSimpleButton btnOk;
      protected UxSimpleButton btnCancel;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemCancel;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemOK;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemNext;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemPrevious;
      protected UxLayoutControl layoutControlBase;
      protected DevExpress.XtraLayout.EmptySpaceItem emptySpaceItemBase;

   }
}