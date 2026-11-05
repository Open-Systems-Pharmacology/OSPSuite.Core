namespace OSPSuite.UI.Views.Importer
{
   partial class ErrorTypeColumnEditorView
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
         this.errorTypeColumnDescriptionLabelControl = new DevExpress.XtraEditors.LabelControl();
         this.errorTypeColumnPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.errorTypeColumnToggleSwitch = new DevExpress.XtraEditors.ToggleSwitch();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.errorTypeColumnToggleLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.errorTypeColumnLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.errorTypeColumnDescriptionLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.errorTypeColumnPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.errorTypeColumnToggleSwitch.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.errorTypeColumnToggleLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.errorTypeColumnLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.errorTypeColumnDescriptionLayoutControlItem)).BeginInit();
         this.SuspendLayout();
         //
         // layoutControl
         //
         this.layoutControl.Controls.Add(this.errorTypeColumnDescriptionLabelControl);
         this.layoutControl.Controls.Add(this.errorTypeColumnPanelControl);
         this.layoutControl.Controls.Add(this.errorTypeColumnToggleSwitch);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(560, 76);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl";
         //
         // errorTypeColumnDescriptionLabelControl
         //
         this.errorTypeColumnDescriptionLabelControl.Location = new System.Drawing.Point(2, 56);
         this.errorTypeColumnDescriptionLabelControl.Name = "errorTypeColumnDescriptionLabelControl";
         this.errorTypeColumnDescriptionLabelControl.Size = new System.Drawing.Size(240, 16);
         this.errorTypeColumnDescriptionLabelControl.StyleController = this.layoutControl;
         this.errorTypeColumnDescriptionLabelControl.TabIndex = 6;
         this.errorTypeColumnDescriptionLabelControl.Text = "errorTypeColumnDescriptionLabelControl";
         //
         // errorTypeColumnPanelControl
         //
         this.errorTypeColumnPanelControl.Location = new System.Drawing.Point(173, 26);
         this.errorTypeColumnPanelControl.Margin = new System.Windows.Forms.Padding(0);
         this.errorTypeColumnPanelControl.Name = "errorTypeColumnPanelControl";
         this.errorTypeColumnPanelControl.Size = new System.Drawing.Size(387, 28);
         this.errorTypeColumnPanelControl.TabIndex = 5;
         //
         // errorTypeColumnToggleSwitch
         //
         this.errorTypeColumnToggleSwitch.Location = new System.Drawing.Point(173, 2);
         this.errorTypeColumnToggleSwitch.Name = "errorTypeColumnToggleSwitch";
         this.errorTypeColumnToggleSwitch.Properties.OffText = "Off";
         this.errorTypeColumnToggleSwitch.Properties.OnText = "On";
         this.errorTypeColumnToggleSwitch.Size = new System.Drawing.Size(387, 24);
         this.errorTypeColumnToggleSwitch.StyleController = this.layoutControl;
         this.errorTypeColumnToggleSwitch.TabIndex = 4;
         //
         // Root
         //
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.errorTypeColumnToggleLayoutControlItem,
            this.errorTypeColumnLayoutControlItem,
            this.errorTypeColumnDescriptionLayoutControlItem});
         this.Root.Name = "Root";
         this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 2, 2);
         this.Root.Size = new System.Drawing.Size(560, 76);
         this.Root.TextVisible = false;
         //
         // errorTypeColumnToggleLayoutControlItem
         //
         this.errorTypeColumnToggleLayoutControlItem.Control = this.errorTypeColumnToggleSwitch;
         this.errorTypeColumnToggleLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.errorTypeColumnToggleLayoutControlItem.Name = "errorTypeColumnToggleLayoutControlItem";
         this.errorTypeColumnToggleLayoutControlItem.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.errorTypeColumnToggleLayoutControlItem.Size = new System.Drawing.Size(560, 24);
         this.errorTypeColumnToggleLayoutControlItem.TextSize = new System.Drawing.Size(170, 16);
         //
         // errorTypeColumnLayoutControlItem
         //
         this.errorTypeColumnLayoutControlItem.Control = this.errorTypeColumnPanelControl;
         this.errorTypeColumnLayoutControlItem.Location = new System.Drawing.Point(0, 24);
         this.errorTypeColumnLayoutControlItem.Name = "errorTypeColumnLayoutControlItem";
         this.errorTypeColumnLayoutControlItem.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.errorTypeColumnLayoutControlItem.Size = new System.Drawing.Size(560, 28);
         this.errorTypeColumnLayoutControlItem.TextSize = new System.Drawing.Size(170, 16);
         //
         // errorTypeColumnDescriptionLayoutControlItem
         //
         this.errorTypeColumnDescriptionLayoutControlItem.Control = this.errorTypeColumnDescriptionLabelControl;
         this.errorTypeColumnDescriptionLayoutControlItem.Location = new System.Drawing.Point(0, 52);
         this.errorTypeColumnDescriptionLayoutControlItem.Name = "errorTypeColumnDescriptionLayoutControlItem";
         this.errorTypeColumnDescriptionLayoutControlItem.Size = new System.Drawing.Size(560, 20);
         this.errorTypeColumnDescriptionLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.errorTypeColumnDescriptionLayoutControlItem.TextVisible = false;
         //
         // ErrorTypeColumnEditorView
         //
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ErrorTypeColumnEditorView";
         this.Size = new System.Drawing.Size(560, 76);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.errorTypeColumnPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.errorTypeColumnToggleSwitch.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.errorTypeColumnToggleLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.errorTypeColumnLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.errorTypeColumnDescriptionLayoutControlItem)).EndInit();
         this.ResumeLayout(false);
      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.LabelControl errorTypeColumnDescriptionLabelControl;
      private DevExpress.XtraEditors.PanelControl errorTypeColumnPanelControl;
      private DevExpress.XtraEditors.ToggleSwitch errorTypeColumnToggleSwitch;
      private DevExpress.XtraLayout.LayoutControlItem errorTypeColumnToggleLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem errorTypeColumnLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem errorTypeColumnDescriptionLayoutControlItem;
   }
}
