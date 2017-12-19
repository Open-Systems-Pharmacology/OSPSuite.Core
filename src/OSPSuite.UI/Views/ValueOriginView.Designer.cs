namespace OSPSuite.UI.Views
{
   partial class ValueOriginView
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
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.mruDescription = new DevExpress.XtraEditors.MRUEdit();
         this.imageComboBoxValueOriginType = new DevExpress.XtraEditors.ImageComboBoxEdit();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemValueOriginType = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemValueOriginDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.mruDescription.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxValueOriginType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValueOriginType)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValueOriginDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.mruDescription);
         this.layoutControl.Controls.Add(this.imageComboBoxValueOriginType);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(378, 95);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "uxLayoutControl1";
         // 
         // mruDescription
         // 
         this.mruDescription.Location = new System.Drawing.Point(12, 63);
         this.mruDescription.Name = "mruDescription";
         this.mruDescription.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.mruDescription.Size = new System.Drawing.Size(354, 20);
         this.mruDescription.StyleController = this.layoutControl;
         this.mruDescription.TabIndex = 6;
         // 
         // imageComboBoxValueOriginType
         // 
         this.imageComboBoxValueOriginType.Location = new System.Drawing.Point(12, 12);
         this.imageComboBoxValueOriginType.Name = "imageComboBoxValueOriginType";
         this.imageComboBoxValueOriginType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.imageComboBoxValueOriginType.Size = new System.Drawing.Size(354, 20);
         this.imageComboBoxValueOriginType.StyleController = this.layoutControl;
         this.imageComboBoxValueOriginType.TabIndex = 4;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemValueOriginType,
            this.layoutItemValueOriginDescription,
            this.emptySpaceItem1});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "Root";
         this.layoutControlGroup.Size = new System.Drawing.Size(378, 95);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemValueOriginType
         // 
         this.layoutItemValueOriginType.Control = this.imageComboBoxValueOriginType;
         this.layoutItemValueOriginType.Location = new System.Drawing.Point(0, 0);
         this.layoutItemValueOriginType.Name = "layoutItemValueOriginType";
         this.layoutItemValueOriginType.Size = new System.Drawing.Size(358, 24);
         this.layoutItemValueOriginType.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemValueOriginType.TextVisible = false;
         // 
         // layoutItemValueOriginDescription
         // 
         this.layoutItemValueOriginDescription.Control = this.mruDescription;
         this.layoutItemValueOriginDescription.Location = new System.Drawing.Point(0, 35);
         this.layoutItemValueOriginDescription.Name = "layoutItemValueOriginDescription";
         this.layoutItemValueOriginDescription.Size = new System.Drawing.Size(358, 40);
         this.layoutItemValueOriginDescription.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutItemValueOriginDescription.TextSize = new System.Drawing.Size(159, 13);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 24);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(358, 11);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // ValueOriginView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ValueOriginView";
         this.Size = new System.Drawing.Size(378, 95);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.mruDescription.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxValueOriginType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValueOriginType)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValueOriginDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraEditors.ImageComboBoxEdit imageComboBoxValueOriginType;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemValueOriginType;
      private DevExpress.XtraEditors.MRUEdit mruDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemValueOriginDescription;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
