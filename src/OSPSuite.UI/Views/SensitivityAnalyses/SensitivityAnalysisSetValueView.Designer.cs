using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.SensitivityAnalyses
{
   partial class SensitivityAnalysisSetValueView
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
         this.uxLayoutControl = new UxLayoutControl();
         this.textEdit = new DevExpress.XtraEditors.TextEdit();
         this.btnApplySelection = new DevExpress.XtraEditors.SimpleButton();
         this.btnApplyAll = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemValueText = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemAllButton = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemSelectionButton = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).BeginInit();
         this.uxLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.textEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemValueText)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAllButton)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSelectionButton)).BeginInit();
         this.SuspendLayout();
         // 
         // uxLayoutControl
         // 
         this.uxLayoutControl.AllowCustomization = false;
         this.uxLayoutControl.Controls.Add(this.textEdit);
         this.uxLayoutControl.Controls.Add(this.btnApplySelection);
         this.uxLayoutControl.Controls.Add(this.btnApplyAll);
         this.uxLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl.Name = "uxLayoutControl";
         this.uxLayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(388, 229, 1090, 626);
         this.uxLayoutControl.Root = this.layoutControlGroup;
         this.uxLayoutControl.Size = new System.Drawing.Size(381, 26);
         this.uxLayoutControl.TabIndex = 0;
         this.uxLayoutControl.Text = "uxLayoutControl1";
         // 
         // textEdit
         // 
         this.textEdit.Location = new System.Drawing.Point(140, 2);
         this.textEdit.MaximumSize = new System.Drawing.Size(100, 0);
         this.textEdit.Name = "textEdit";
         this.textEdit.Size = new System.Drawing.Size(69, 20);
         this.textEdit.StyleController = this.uxLayoutControl;
         this.textEdit.TabIndex = 6;
         // 
         // btnApplySelection
         // 
         this.btnApplySelection.Location = new System.Drawing.Point(282, 2);
         this.btnApplySelection.Name = "btnApplySelection";
         this.btnApplySelection.Size = new System.Drawing.Size(97, 22);
         this.btnApplySelection.StyleController = this.uxLayoutControl;
         this.btnApplySelection.TabIndex = 5;
         this.btnApplySelection.Text = "btnApplySelection";
         // 
         // btnApplyAll
         // 
         this.btnApplyAll.Location = new System.Drawing.Point(213, 2);
         this.btnApplyAll.Name = "btnApplyAll";
         this.btnApplyAll.Size = new System.Drawing.Size(65, 22);
         this.btnApplyAll.StyleController = this.uxLayoutControl;
         this.btnApplyAll.TabIndex = 4;
         this.btnApplyAll.Text = "btnApplyAll";
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemValueText,
            this.layoutControlItemAllButton,
            this.layoutControlItemSelectionButton});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "Root";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(381, 26);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutControlItemValueText
         // 
         this.layoutControlItemValueText.Control = this.textEdit;
         this.layoutControlItemValueText.ControlAlignment = System.Drawing.ContentAlignment.MiddleLeft;
         this.layoutControlItemValueText.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemValueText.Name = "layoutControlItemValueText";
         this.layoutControlItemValueText.Size = new System.Drawing.Size(211, 26);
         this.layoutControlItemValueText.TextSize = new System.Drawing.Size(135, 13);
         // 
         // layoutControlItemAllButton
         // 
         this.layoutControlItemAllButton.Control = this.btnApplyAll;
         this.layoutControlItemAllButton.Location = new System.Drawing.Point(211, 0);
         this.layoutControlItemAllButton.Name = "layoutControlItemAllButton";
         this.layoutControlItemAllButton.Size = new System.Drawing.Size(69, 26);
         this.layoutControlItemAllButton.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemAllButton.TextVisible = false;
         // 
         // layoutControlItemSelectionButton
         // 
         this.layoutControlItemSelectionButton.Control = this.btnApplySelection;
         this.layoutControlItemSelectionButton.Location = new System.Drawing.Point(280, 0);
         this.layoutControlItemSelectionButton.Name = "layoutControlItemSelectionButton";
         this.layoutControlItemSelectionButton.Size = new System.Drawing.Size(101, 26);
         this.layoutControlItemSelectionButton.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemSelectionButton.TextVisible = false;
         // 
         // SensitivityAnalysisSetValueView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.uxLayoutControl);
         this.Margin = new System.Windows.Forms.Padding(0);
         this.Name = "SensitivityAnalysisSetValueView";
         this.Size = new System.Drawing.Size(381, 26);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).EndInit();
         this.uxLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.textEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemValueText)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAllButton)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSelectionButton)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private UxLayoutControl uxLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraEditors.SimpleButton btnApplySelection;
      private DevExpress.XtraEditors.SimpleButton btnApplyAll;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAllButton;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSelectionButton;
      private DevExpress.XtraEditors.TextEdit textEdit;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemValueText;
   }
}
