namespace OSPSuite.UI.Views.Importer
{
   partial class UnitsEditorView
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

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this._layoutControl = new DevExpress.XtraLayout.LayoutControl();
         this._columnComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
         this._columnsToggleSwitch = new DevExpress.XtraEditors.ToggleSwitch();
         this._dimensionsComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
         this._unitComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
         this._Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this._unitLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this._dimensionsLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this._columnsToogleLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this._columnLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._layoutControl)).BeginInit();
         this._layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._columnComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._columnsToggleSwitch.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._dimensionsComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._unitComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._unitLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._dimensionsLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._columnsToogleLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._columnLayoutControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // _layoutControl
         // 
         this._layoutControl.Controls.Add(this._columnComboBox);
         this._layoutControl.Controls.Add(this._columnsToggleSwitch);
         this._layoutControl.Controls.Add(this._dimensionsComboBox);
         this._layoutControl.Controls.Add(this._unitComboBox);
         this._layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this._layoutControl.Location = new System.Drawing.Point(0, 0);
         this._layoutControl.Name = "_layoutControl";
         this._layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(3541, -85, 812, 500);
         this._layoutControl.Root = this._Root;
         this._layoutControl.Size = new System.Drawing.Size(379, 136);
         this._layoutControl.TabIndex = 0;
         this._layoutControl.Text = "_layoutControl";
         // 
         // _columnComboBox
         // 
         this._columnComboBox.Location = new System.Drawing.Point(212, 92);
         this._columnComboBox.Name = "_columnComboBox";
         this._columnComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this._columnComboBox.Size = new System.Drawing.Size(155, 22);
         this._columnComboBox.StyleController = this._layoutControl;
         this._columnComboBox.TabIndex = 8;
         // 
         // _columnsToggleSwitch
         // 
         this._columnsToggleSwitch.Location = new System.Drawing.Point(212, 12);
         this._columnsToggleSwitch.Name = "_columnsToggleSwitch";
         this._columnsToggleSwitch.Properties.OffText = "Off";
         this._columnsToggleSwitch.Properties.OnText = "On";
         this._columnsToggleSwitch.Size = new System.Drawing.Size(155, 24);
         this._columnsToggleSwitch.StyleController = this._layoutControl;
         this._columnsToggleSwitch.TabIndex = 7;
         // 
         // _dimensionsComboBox
         // 
         this._dimensionsComboBox.Location = new System.Drawing.Point(212, 40);
         this._dimensionsComboBox.Name = "_dimensionsComboBox";
         this._dimensionsComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this._dimensionsComboBox.Size = new System.Drawing.Size(155, 22);
         this._dimensionsComboBox.StyleController = this._layoutControl;
         this._dimensionsComboBox.TabIndex = 6;
         // 
         // _unitComboBox
         // 
         this._unitComboBox.Location = new System.Drawing.Point(212, 66);
         this._unitComboBox.Name = "_unitComboBox";
         this._unitComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this._unitComboBox.Size = new System.Drawing.Size(155, 22);
         this._unitComboBox.StyleController = this._layoutControl;
         this._unitComboBox.TabIndex = 5;
         // 
         // _Root
         // 
         this._Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this._Root.GroupBordersVisible = false;
         this._Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this._unitLayoutControlItem,
            this._dimensionsLayoutControlItem,
            this._columnsToogleLayoutControlItem,
            this._columnLayoutControlItem});
         this._Root.Name = "Root";
         this._Root.Size = new System.Drawing.Size(379, 136);
         this._Root.TextVisible = false;
         // 
         // _unitLayoutControlItem
         // 
         this._unitLayoutControlItem.Control = this._unitComboBox;
         this._unitLayoutControlItem.Location = new System.Drawing.Point(0, 54);
         this._unitLayoutControlItem.Name = "_unitLayoutControlItem";
         this._unitLayoutControlItem.Size = new System.Drawing.Size(359, 26);
         this._unitLayoutControlItem.Text = "Unit";
         this._unitLayoutControlItem.TextSize = new System.Drawing.Size(197, 16);
         // 
         // _dimensionsLayoutControlItem
         // 
         this._dimensionsLayoutControlItem.Control = this._dimensionsComboBox;
         this._dimensionsLayoutControlItem.Location = new System.Drawing.Point(0, 28);
         this._dimensionsLayoutControlItem.Name = "_dimensionsLayoutControlItem";
         this._dimensionsLayoutControlItem.Size = new System.Drawing.Size(359, 26);
         this._dimensionsLayoutControlItem.TextSize = new System.Drawing.Size(197, 16);
         // 
         // _columnsToogleLayoutControlItem
         // 
         this._columnsToogleLayoutControlItem.Control = this._columnsToggleSwitch;
         this._columnsToogleLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this._columnsToogleLayoutControlItem.Name = "_columnsToogleLayoutControlItem";
         this._columnsToogleLayoutControlItem.Size = new System.Drawing.Size(359, 28);
         this._columnsToogleLayoutControlItem.TextSize = new System.Drawing.Size(197, 16);
         // 
         // _columnLayoutControlItem
         // 
         this._columnLayoutControlItem.Control = this._columnComboBox;
         this._columnLayoutControlItem.Location = new System.Drawing.Point(0, 80);
         this._columnLayoutControlItem.Name = "_columnLayoutControlItem";
         this._columnLayoutControlItem.Size = new System.Drawing.Size(359, 36);
         this._columnLayoutControlItem.TextSize = new System.Drawing.Size(197, 16);
         // 
         // UnitsEditorView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this._layoutControl);
         this.Margin = new System.Windows.Forms.Padding(8);
         this.Name = "UnitsEditorView";
         this.Size = new System.Drawing.Size(379, 136);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._layoutControl)).EndInit();
         this._layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._columnComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._columnsToggleSwitch.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._dimensionsComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._unitComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._unitLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._dimensionsLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._columnsToogleLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._columnLayoutControlItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl _layoutControl;
      private DevExpress.XtraEditors.ComboBoxEdit _unitComboBox;
      private DevExpress.XtraLayout.LayoutControlGroup _Root;
      private DevExpress.XtraLayout.LayoutControlItem _unitLayoutControlItem;
      private DevExpress.XtraEditors.ComboBoxEdit _dimensionsComboBox;
      private DevExpress.XtraLayout.LayoutControlItem _dimensionsLayoutControlItem;
      private DevExpress.XtraEditors.ToggleSwitch _columnsToggleSwitch;
      private DevExpress.XtraLayout.LayoutControlItem _columnsToogleLayoutControlItem;
      private DevExpress.XtraEditors.ComboBoxEdit _columnComboBox;
      private DevExpress.XtraLayout.LayoutControlItem _columnLayoutControlItem;
   }
}
