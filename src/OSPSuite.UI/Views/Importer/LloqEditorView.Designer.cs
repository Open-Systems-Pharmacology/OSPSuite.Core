namespace OSPSuite.UI.Views.Importer
{
   partial class LloqEditorView
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
         this.rootLayoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.LloqDescriptionPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.LloqToggleSwitch = new DevExpress.XtraEditors.ToggleSwitch();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.LloqToggleSwitchLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.LloqDescriptionPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.LloqToggleSwitch.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.LloqToggleSwitchLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
         this.rootLayoutControl.Controls.Add(this.LloqDescriptionPanelControl);
         this.rootLayoutControl.Controls.Add(this.LloqToggleSwitch);
         this.rootLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.rootLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.rootLayoutControl.Margin = new System.Windows.Forms.Padding(6);
         this.rootLayoutControl.Name = "rootLayoutControl";
         this.rootLayoutControl.Root = this.Root;
         this.rootLayoutControl.Size = new System.Drawing.Size(1435, 349);
         this.rootLayoutControl.TabIndex = 38;
         this.rootLayoutControl.Text = "rootLayoutControl";
         // 
         // LloqDescriptionPanelControl
         // 
         this.LloqDescriptionPanelControl.Location = new System.Drawing.Point(12, 63);
         this.LloqDescriptionPanelControl.Name = "LloqDescriptionPanelControl";
         this.LloqDescriptionPanelControl.Size = new System.Drawing.Size(1411, 135);
         this.LloqDescriptionPanelControl.TabIndex = 5;
         // 
         // LloqToggleSwitch
         // 
         this.LloqToggleSwitch.Location = new System.Drawing.Point(443, 12);
         this.LloqToggleSwitch.Name = "LloqToggleSwitch";
         this.LloqToggleSwitch.Properties.OffText = "Off";
         this.LloqToggleSwitch.Properties.OnText = "On";
         this.LloqToggleSwitch.Size = new System.Drawing.Size(980, 47);
         this.LloqToggleSwitch.StyleController = this.rootLayoutControl;
         this.LloqToggleSwitch.TabIndex = 4;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.LloqToggleSwitchLayoutControlItem,
            this.layoutControlItem1});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1435, 349);
         this.Root.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 190);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(1415, 139);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // LloqToggleSwitchLayoutControlItem
         // 
         this.LloqToggleSwitchLayoutControlItem.Control = this.LloqToggleSwitch;
         this.LloqToggleSwitchLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.LloqToggleSwitchLayoutControlItem.Name = "LloqToggleSwitchLayoutControlItem";
         this.LloqToggleSwitchLayoutControlItem.Size = new System.Drawing.Size(1415, 51);
         this.LloqToggleSwitchLayoutControlItem.TextSize = new System.Drawing.Size(428, 33);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.LloqDescriptionPanelControl;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 51);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(1415, 139);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // LloqEditorView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "LLOQEditorViewNew";
         this.Controls.Add(this.rootLayoutControl);
         this.Margin = new System.Windows.Forms.Padding(43, 41, 43, 41);
         this.Name = "LloqEditorView";
         this.Size = new System.Drawing.Size(1435, 349);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).EndInit();
         this.rootLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.LloqDescriptionPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.LloqToggleSwitch.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.LloqToggleSwitchLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         this.ResumeLayout(false);

      }

      private DevExpress.XtraLayout.LayoutControl rootLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;

      
      #endregion
      private DevExpress.XtraEditors.ToggleSwitch LloqToggleSwitch;
      private DevExpress.XtraLayout.LayoutControlItem LloqToggleSwitchLayoutControlItem;
      private DevExpress.XtraEditors.PanelControl LloqDescriptionPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
   }
}
