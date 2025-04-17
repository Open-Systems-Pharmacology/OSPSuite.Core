namespace OSPSuite.UI.Views
{
   partial class HeavyWorkView
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
         this.uxLayoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
         this.progressBar = new DevExpress.XtraEditors.MarqueeProgressBarControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemProgressBar = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemCancelButton = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).BeginInit();
         this.uxLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemProgressBar)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCancelButton)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // uxLayoutControl
         // 
         this.uxLayoutControl.AllowCustomization = false;
         this.uxLayoutControl.Controls.Add(this.btnCancel);
         this.uxLayoutControl.Controls.Add(this.progressBar);
         this.uxLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl.Margin = new System.Windows.Forms.Padding(0);
         this.uxLayoutControl.Name = "uxLayoutControl";
         this.uxLayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(599, -1065, 812, 500);
         this.uxLayoutControl.Root = this.Root;
         this.uxLayoutControl.Size = new System.Drawing.Size(172, 110);
         this.uxLayoutControl.TabIndex = 4;
         this.uxLayoutControl.Text = "uxLayoutControl1";
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(47, 68);
         this.btnCancel.MinimumSize = new System.Drawing.Size(0, 30);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(113, 30);
         this.btnCancel.StyleController = this.uxLayoutControl;
         this.btnCancel.TabIndex = 5;
         this.btnCancel.Text = "btnCancel";
         // 
         // progressBar
         // 
         this.progressBar.EditValue = 0;
         this.progressBar.Location = new System.Drawing.Point(12, 12);
         this.progressBar.MinimumSize = new System.Drawing.Size(0, 25);
         this.progressBar.Name = "progressBar";
         this.progressBar.Size = new System.Drawing.Size(148, 25);
         this.progressBar.StyleController = this.uxLayoutControl;
         this.progressBar.TabIndex = 4;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemProgressBar,
            this.layoutControlItemCancelButton,
            this.emptySpaceItem1,
            this.emptySpaceItem2});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(172, 110);
         this.Root.TextVisible = false;
         // 
         // layoutControlItemProgressBar
         // 
         this.layoutControlItemProgressBar.Control = this.progressBar;
         this.layoutControlItemProgressBar.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemProgressBar.Name = "layoutControlItemProgressBar";
         this.layoutControlItemProgressBar.Size = new System.Drawing.Size(152, 29);
         this.layoutControlItemProgressBar.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemProgressBar.TextVisible = false;
         // 
         // layoutControlItemCancelButton
         // 
         this.layoutControlItemCancelButton.Control = this.btnCancel;
         this.layoutControlItemCancelButton.Location = new System.Drawing.Point(35, 56);
         this.layoutControlItemCancelButton.Name = "layoutControlItemCancelButton";
         this.layoutControlItemCancelButton.Size = new System.Drawing.Size(117, 34);
         this.layoutControlItemCancelButton.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemCancelButton.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 29);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(35, 61);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(35, 29);
         this.emptySpaceItem2.MinSize = new System.Drawing.Size(104, 24);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(117, 27);
         this.emptySpaceItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // HeavyWorkView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "Processing...";
         this.ClientSize = new System.Drawing.Size(172, 110);
         this.Controls.Add(this.uxLayoutControl);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Margin = new System.Windows.Forms.Padding(5);
         this.MaximumSize = new System.Drawing.Size(250, 155);
         this.Name = "HeavyWorkView";
         this.Text = "Processing...";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).EndInit();
         this.uxLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemProgressBar)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCancelButton)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private Controls.UxLayoutControl uxLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.SimpleButton btnCancel;
      private DevExpress.XtraEditors.MarqueeProgressBarControl progressBar;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemProgressBar;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemCancelButton;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
   }
}