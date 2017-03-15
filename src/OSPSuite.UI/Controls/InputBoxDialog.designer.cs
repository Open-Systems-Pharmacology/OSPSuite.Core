namespace OSPSuite.UI.Controls
{
   partial class InputBoxDialog
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
         this.components = new System.ComponentModel.Container();
         this.errorProvider = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
         this.btnOk = new UxSimpleButton();
         this.layoutControl1 = new UxLayoutControl();
         this.lblPrompt = new DevExpress.XtraEditors.LabelControl();
         this.cbInput = new DevExpress.XtraEditors.MRUEdit();
         this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutItemOk = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutItemCancel = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbInput.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOk)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).BeginInit();
         this.SuspendLayout();
         // 
         // errorProvider
         // 
         this.errorProvider.ContainerControl = this;
         // 
         // btnOk
         // 
         this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnOk.Location = new System.Drawing.Point(232, 87);
         this.btnOk.Name = "btnOk";
         this.btnOk.Size = new System.Drawing.Size(149, 22);
         this.btnOk.StyleController = this.layoutControl1;
         this.btnOk.TabIndex = 5;
         this.btnOk.Text = "btnOK";
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.lblPrompt);
         this.layoutControl1.Controls.Add(this.cbInput);
         this.layoutControl1.Controls.Add(this.btnCancel);
         this.layoutControl1.Controls.Add(this.btnOk);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(705, 325, 250, 350);
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(461, 121);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // lblPrompt
         // 
         this.lblPrompt.Location = new System.Drawing.Point(12, 12);
         this.lblPrompt.Name = "lblPrompt";
         this.lblPrompt.Size = new System.Drawing.Size(44, 13);
         this.lblPrompt.StyleController = this.layoutControl1;
         this.lblPrompt.TabIndex = 7;
         this.lblPrompt.Text = "lblPrompt";
         // 
         // cbInput
         // 
         this.cbInput.Location = new System.Drawing.Point(12, 29);
         this.cbInput.Name = "cbInput";
         this.cbInput.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbInput.Size = new System.Drawing.Size(437, 20);
         this.cbInput.StyleController = this.layoutControl1;
         this.cbInput.TabIndex = 6;
         // 
         // btnCancel
         // 
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancel.Location = new System.Drawing.Point(385, 87);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(64, 22);
         this.btnCancel.StyleController = this.layoutControl1;
         this.btnCancel.TabIndex = 4;
         this.btnCancel.Text = "btnCancel";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.emptySpaceItem1,
            this.layoutItemOk,
            this.emptySpaceItem2,
            this.layoutItemCancel});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(461, 121);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.cbInput;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 17);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(441, 24);
         this.layoutControlItem1.Text = "layoutControlItem1";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextToControlDistance = 0;
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.lblPrompt;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(441, 17);
         this.layoutControlItem2.Text = "layoutControlItem2";
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextToControlDistance = 0;
         this.layoutControlItem2.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 41);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(441, 34);
         this.emptySpaceItem1.Text = "emptySpaceItem1";
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutItemOk
         // 
         this.layoutItemOk.Control = this.btnOk;
         this.layoutItemOk.CustomizationFormText = "layoutControlItem2";
         this.layoutItemOk.Location = new System.Drawing.Point(220, 75);
         this.layoutItemOk.Name = "layoutItemOk";
         this.layoutItemOk.Size = new System.Drawing.Size(153, 26);
         this.layoutItemOk.Text = "layoutItemOk";
         this.layoutItemOk.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemOk.TextToControlDistance = 0;
         this.layoutItemOk.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 75);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(220, 26);
         this.emptySpaceItem2.Text = "emptySpaceItem2";
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutItemCancel
         // 
         this.layoutItemCancel.Control = this.btnCancel;
         this.layoutItemCancel.CustomizationFormText = "layoutItemCancel";
         this.layoutItemCancel.Location = new System.Drawing.Point(373, 75);
         this.layoutItemCancel.Name = "layoutItemCancel";
         this.layoutItemCancel.Size = new System.Drawing.Size(68, 26);
         this.layoutItemCancel.Text = "layoutItemCancel";
         this.layoutItemCancel.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemCancel.TextToControlDistance = 0;
         this.layoutItemCancel.TextVisible = false;
         // 
         // InputBoxDialog
         // 
         this.AcceptButton = this.btnOk;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoSize = true;
         this.CancelButton = this.btnCancel;
         this.ClientSize = new System.Drawing.Size(461, 121);
         this.Controls.Add(this.layoutControl1);
         this.Name = "InputBoxDialog";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "InputBoxDialog";
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cbInput.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOk)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private UxLayoutControl layoutControl1;
      private UxSimpleButton btnOk;
      private DevExpress.XtraEditors.SimpleButton btnCancel;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemCancel;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemOk;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.MRUEdit cbInput;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraEditors.LabelControl lblPrompt;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errorProvider;
   }
}