namespace OSPSuite.UI.Views.Importer
{
   partial class OptionsEditorView
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
         this.comboBoxLayoutControl = new DevExpress.XtraLayout.LayoutControl();
         this._comboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.comboBoxLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.comboBoxLayoutControl)).BeginInit();
         this.comboBoxLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._comboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.comboBoxLayoutControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // comboBoxLayoutControl
         // 
         this.comboBoxLayoutControl.Controls.Add(this._comboBoxEdit);
         this.comboBoxLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.comboBoxLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.comboBoxLayoutControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.comboBoxLayoutControl.Name = "comboBoxLayoutControl";
         this.comboBoxLayoutControl.Root = this.Root;
         this.comboBoxLayoutControl.Size = new System.Drawing.Size(364, 37);
         this.comboBoxLayoutControl.TabIndex = 0;
         this.comboBoxLayoutControl.Text = "layoutControl1";
         // 
         // _comboBoxEdit
         // 
         this._comboBoxEdit.Location = new System.Drawing.Point(5, 5);
         this._comboBoxEdit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this._comboBoxEdit.Name = "_comboBoxEdit";
         this._comboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this._comboBoxEdit.Properties.Padding = new System.Windows.Forms.Padding(2);
         this._comboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
         this._comboBoxEdit.Size = new System.Drawing.Size(354, 24);
         this._comboBoxEdit.StyleController = this.comboBoxLayoutControl;
         this._comboBoxEdit.TabIndex = 4;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.comboBoxLayoutControlItem});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(364, 37);
         this.Root.TextVisible = false;
         // 
         // comboBoxLayoutControlItem
         // 
         this.comboBoxLayoutControlItem.Control = this._comboBoxEdit;
         this.comboBoxLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.comboBoxLayoutControlItem.Name = "comboBoxLayoutControlItem";
         this.comboBoxLayoutControlItem.Size = new System.Drawing.Size(356, 29);
         this.comboBoxLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.comboBoxLayoutControlItem.TextVisible = false;
         // 
         // OptionsEditorView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.comboBoxLayoutControl);
         this.Margin = new System.Windows.Forms.Padding(0);
         this.Name = "OptionsEditorView";
         this.Size = new System.Drawing.Size(364, 37);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.comboBoxLayoutControl)).EndInit();
         this.comboBoxLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._comboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.comboBoxLayoutControlItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl comboBoxLayoutControl;
      private DevExpress.XtraEditors.ComboBoxEdit _comboBoxEdit;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem comboBoxLayoutControlItem;
   }
}
