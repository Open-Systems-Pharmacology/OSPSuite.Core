using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;

namespace OSPSuite.Presentation.Importer.Views
{
   partial class ImporterView
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
         this.columnMappingPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.formatComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.sourceFilePanelControl = new DevExpress.XtraEditors.PanelControl();
         this.TabControl = new DevExpress.XtraTab.XtraTabControl();
         this.dataViewingPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.btnImport = new DevExpress.XtraEditors.SimpleButton();
         this.btnImportAll = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingPanelControl)).BeginInit();
         this.columnMappingPanelControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.formatComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.TabControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // columnMappingPanelControl
         // 
         this.columnMappingPanelControl.Controls.Add(this.formatComboBoxEdit);
         this.columnMappingPanelControl.Controls.Add(this.labelControl1);
         this.columnMappingPanelControl.Location = new System.Drawing.Point(1431, 296);
         this.columnMappingPanelControl.Margin = new System.Windows.Forms.Padding(2);
         this.columnMappingPanelControl.Name = "columnMappingPanelControl";
         this.columnMappingPanelControl.Size = new System.Drawing.Size(1770, 1225);
         this.columnMappingPanelControl.TabIndex = 1;
         // 
         // formatComboBoxEdit
         // 
         this.formatComboBoxEdit.Location = new System.Drawing.Point(22, 399);
         this.formatComboBoxEdit.Margin = new System.Windows.Forms.Padding(6);
         this.formatComboBoxEdit.Name = "formatComboBoxEdit";
         this.formatComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.formatComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
         this.formatComboBoxEdit.Size = new System.Drawing.Size(1315, 50);
         this.formatComboBoxEdit.TabIndex = 3;
         // 
         // labelControl1
         // 
         this.labelControl1.Location = new System.Drawing.Point(349, 223);
         this.labelControl1.Margin = new System.Windows.Forms.Padding(6);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(85, 33);
         this.labelControl1.TabIndex = 4;
         this.labelControl1.Text = "Format";
         // 
         // sourceFilePanelControl
         // 
         this.sourceFilePanelControl.Location = new System.Drawing.Point(19, 52);
         this.sourceFilePanelControl.Margin = new System.Windows.Forms.Padding(2);
         this.sourceFilePanelControl.Name = "sourceFilePanelControl";
         this.sourceFilePanelControl.Size = new System.Drawing.Size(3182, 85);
         this.sourceFilePanelControl.TabIndex = 5;
         // 
         // TabControl
         // 
         this.TabControl.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPageHeaders;
         this.TabControl.Location = new System.Drawing.Point(19, 296);
         this.TabControl.Margin = new System.Windows.Forms.Padding(2);
         this.TabControl.Name = "TabControl";
         this.TabControl.Size = new System.Drawing.Size(1384, 1192);
         this.TabControl.TabIndex = 0;
         // 
         // dataViewingPanelControl
         // 
         this.dataViewingPanelControl.Location = new System.Drawing.Point(0, 83);
         this.dataViewingPanelControl.Name = "dataViewingPanelControl";
         this.dataViewingPanelControl.Size = new System.Drawing.Size(1385, 1109);
         this.dataViewingPanelControl.TabIndex = 0;
         // 
         // btnImport
         // 
         this.btnImport.Enabled = false;
         this.btnImport.Location = new System.Drawing.Point(2387, 1551);
         this.btnImport.Margin = new System.Windows.Forms.Padding(6);
         this.btnImport.Name = "btnImport";
         this.btnImport.Size = new System.Drawing.Size(401, 54);
         this.btnImport.TabIndex = 6;
         this.btnImport.Text = "Import current sheet";
         // 
         // btnImportAll
         // 
         this.btnImportAll.Enabled = false;
         this.btnImportAll.Location = new System.Drawing.Point(2801, 1551);
         this.btnImportAll.Margin = new System.Windows.Forms.Padding(6);
         this.btnImportAll.Name = "btnImportAll";
         this.btnImportAll.Size = new System.Drawing.Size(401, 54);
         this.btnImportAll.TabIndex = 7;
         this.btnImportAll.Text = "Import All";
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.comboBoxEdit1);
         this.layoutControl1.Controls.Add(this.labelControl2);
         this.layoutControl1.Location = new System.Drawing.Point(26, 156);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(953, 449, 1625, 1000);
         this.layoutControl1.Root = this.Root;
         this.layoutControl1.Size = new System.Drawing.Size(3174, 123);
         this.layoutControl1.TabIndex = 8;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem2});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(3174, 123);
         this.Root.TextVisible = false;
         // 
         // labelControl2
         // 
         this.labelControl2.Location = new System.Drawing.Point(12, 12);
         this.labelControl2.Name = "labelControl2";
         this.labelControl2.Size = new System.Drawing.Size(156, 33);
         this.labelControl2.StyleController = this.layoutControl1;
         this.labelControl2.TabIndex = 4;
         this.labelControl2.Text = "labelControl2";
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.labelControl2;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(3154, 37);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(1577, 37);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(1577, 66);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // comboBoxEdit1
         // 
         this.comboBoxEdit1.Location = new System.Drawing.Point(253, 49);
         this.comboBoxEdit1.Name = "comboBoxEdit1";
         this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.comboBoxEdit1.Size = new System.Drawing.Size(1332, 50);
         this.comboBoxEdit1.StyleController = this.layoutControl1;
         this.comboBoxEdit1.TabIndex = 5;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.comboBoxEdit1;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 37);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(1577, 66);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(229, 33);
         // 
         // ImporterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "ImporterView";
         this.Controls.Add(this.layoutControl1);
         this.Controls.Add(this.btnImportAll);
         this.Controls.Add(this.btnImport);
         this.Controls.Add(this.TabControl);
         this.Controls.Add(this.sourceFilePanelControl);
         this.Controls.Add(this.columnMappingPanelControl);
         this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
         this.Name = "ImporterView";
         this.Size = new System.Drawing.Size(3266, 1732);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingPanelControl)).EndInit();
         this.columnMappingPanelControl.ResumeLayout(false);
         this.columnMappingPanelControl.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.formatComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.TabControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl dataViewingPanelControl;
      private DevExpress.XtraEditors.PanelControl columnMappingPanelControl;
      private DevExpress.XtraEditors.ComboBoxEdit formatComboBoxEdit;
      private DevExpress.XtraEditors.PanelControl sourceFilePanelControl;
      private DevExpress.XtraTab.XtraTabControl TabControl;
      private DevExpress.XtraEditors.SimpleButton btnImport;
      private DevExpress.XtraEditors.SimpleButton btnImportAll;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
      private DevExpress.XtraEditors.LabelControl labelControl2;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
   }
}