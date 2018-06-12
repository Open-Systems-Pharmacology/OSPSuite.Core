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
         this.imageComboBoxValueOriginDeterminationMethod = new DevExpress.XtraEditors.ImageComboBoxEdit();
         this.mruDescription = new DevExpress.XtraEditors.MRUEdit();
         this.imageComboBoxValueOriginSource = new DevExpress.XtraEditors.ImageComboBoxEdit();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemValueOriginSource = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemValueOriginDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemValueOriginDeterminationMethod = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxValueOriginDeterminationMethod.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.mruDescription.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxValueOriginSource.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValueOriginSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValueOriginDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValueOriginDeterminationMethod)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.imageComboBoxValueOriginDeterminationMethod);
         this.layoutControl.Controls.Add(this.mruDescription);
         this.layoutControl.Controls.Add(this.imageComboBoxValueOriginSource);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(391, 142);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "uxLayoutControl1";
         // 
         // imageComboBoxValueOriginDeterminationMethod
         // 
         this.imageComboBoxValueOriginDeterminationMethod.Location = new System.Drawing.Point(12, 68);
         this.imageComboBoxValueOriginDeterminationMethod.Name = "imageComboBoxValueOriginDeterminationMethod";
         this.imageComboBoxValueOriginDeterminationMethod.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.imageComboBoxValueOriginDeterminationMethod.Size = new System.Drawing.Size(367, 20);
         this.imageComboBoxValueOriginDeterminationMethod.StyleController = this.layoutControl;
         this.imageComboBoxValueOriginDeterminationMethod.TabIndex = 7;
         // 
         // mruDescription
         // 
         this.mruDescription.Location = new System.Drawing.Point(12, 108);
         this.mruDescription.Name = "mruDescription";
         this.mruDescription.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.mruDescription.Size = new System.Drawing.Size(367, 20);
         this.mruDescription.StyleController = this.layoutControl;
         this.mruDescription.TabIndex = 6;
         // 
         // imageComboBoxValueOriginSource
         // 
         this.imageComboBoxValueOriginSource.Location = new System.Drawing.Point(12, 28);
         this.imageComboBoxValueOriginSource.Name = "imageComboBoxValueOriginSource";
         this.imageComboBoxValueOriginSource.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.imageComboBoxValueOriginSource.Size = new System.Drawing.Size(367, 20);
         this.imageComboBoxValueOriginSource.StyleController = this.layoutControl;
         this.imageComboBoxValueOriginSource.TabIndex = 4;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemValueOriginSource,
            this.layoutItemValueOriginDescription,
            this.layoutItemValueOriginDeterminationMethod});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "Root";
         this.layoutControlGroup.Size = new System.Drawing.Size(391, 142);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemValueOriginSource
         // 
         this.layoutItemValueOriginSource.Control = this.imageComboBoxValueOriginSource;
         this.layoutItemValueOriginSource.Location = new System.Drawing.Point(0, 0);
         this.layoutItemValueOriginSource.Name = "layoutItemValueOriginSource";
         this.layoutItemValueOriginSource.Size = new System.Drawing.Size(371, 40);
         this.layoutItemValueOriginSource.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutItemValueOriginSource.TextSize = new System.Drawing.Size(209, 13);
         // 
         // layoutItemValueOriginDescription
         // 
         this.layoutItemValueOriginDescription.Control = this.mruDescription;
         this.layoutItemValueOriginDescription.Location = new System.Drawing.Point(0, 80);
         this.layoutItemValueOriginDescription.Name = "layoutItemValueOriginDescription";
         this.layoutItemValueOriginDescription.Size = new System.Drawing.Size(371, 42);
         this.layoutItemValueOriginDescription.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutItemValueOriginDescription.TextSize = new System.Drawing.Size(209, 13);
         // 
         // layoutItemValueOriginDeterminationMethod
         // 
         this.layoutItemValueOriginDeterminationMethod.Control = this.imageComboBoxValueOriginDeterminationMethod;
         this.layoutItemValueOriginDeterminationMethod.Location = new System.Drawing.Point(0, 40);
         this.layoutItemValueOriginDeterminationMethod.Name = "layoutItemValueOriginDeterminationMethod";
         this.layoutItemValueOriginDeterminationMethod.Size = new System.Drawing.Size(371, 40);
         this.layoutItemValueOriginDeterminationMethod.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutItemValueOriginDeterminationMethod.TextSize = new System.Drawing.Size(209, 13);
         // 
         // ValueOriginView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ValueOriginView";
         this.Size = new System.Drawing.Size(391, 142);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxValueOriginDeterminationMethod.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.mruDescription.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxValueOriginSource.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValueOriginSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValueOriginDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValueOriginDeterminationMethod)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraEditors.ImageComboBoxEdit imageComboBoxValueOriginSource;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemValueOriginSource;
      private DevExpress.XtraEditors.MRUEdit mruDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemValueOriginDescription;
      private DevExpress.XtraEditors.ImageComboBoxEdit imageComboBoxValueOriginDeterminationMethod;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemValueOriginDeterminationMethod;
   }
}
