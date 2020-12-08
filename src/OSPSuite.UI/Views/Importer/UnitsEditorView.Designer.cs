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
         this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
         this._unitComboBox = new DevExpress.XtraEditors.CheckedComboBoxEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._unitComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this._unitComboBox);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.Root;
         this.layoutControl1.Size = new System.Drawing.Size(379, 46);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // _unitComboBox
         // 
         this._unitComboBox.Location = new System.Drawing.Point(37, 12);
         this._unitComboBox.Name = "_unitComboBox";
         this._unitComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this._unitComboBox.Size = new System.Drawing.Size(330, 22);
         this._unitComboBox.StyleController = this.layoutControl1;
         this._unitComboBox.TabIndex = 5;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(379, 46);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this._unitComboBox;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(359, 26);
         this.layoutControlItem2.Text = "Unit";
         this.layoutControlItem2.TextSize = new System.Drawing.Size(22, 16);
         // 
         // UnitsEditorView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "UnitsEditorView";
         this.Size = new System.Drawing.Size(379, 46);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._unitComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraEditors.CheckedComboBoxEdit _unitComboBox;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
   }
}
