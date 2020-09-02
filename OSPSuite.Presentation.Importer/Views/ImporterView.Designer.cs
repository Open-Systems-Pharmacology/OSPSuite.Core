using OSPSuite.Utility.Extensions;

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
         this.formatLabelControl = new DevExpress.XtraEditors.LabelControl();
         this.formatComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.sourceFilePanelControl = new DevExpress.XtraEditors.PanelControl();
         this.TabControl = new DevExpress.XtraTab.XtraTabControl();
         this.dataViewingPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.btnImport = new DevExpress.XtraEditors.SimpleButton();
         this.btnImportAll = new DevExpress.XtraEditors.SimpleButton();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.formatComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.TabControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingPanelControl)).BeginInit();
         this.SuspendLayout();
         // 
         // columnMappingPanelControl
         // 
         this.columnMappingPanelControl.Location = new System.Drawing.Point(1431, 155);
         this.columnMappingPanelControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.columnMappingPanelControl.Name = "columnMappingPanelControl";
         this.columnMappingPanelControl.Size = new System.Drawing.Size(1770, 1225);
         this.columnMappingPanelControl.TabIndex = 1;
         // 
         // formatLabelControl
         // 
         this.formatLabelControl.Location = new System.Drawing.Point(1380, 1442);
         this.formatLabelControl.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
         this.formatLabelControl.Name = "formatLabelControl";
         this.formatLabelControl.Size = new System.Drawing.Size(85, 33);
         this.formatLabelControl.TabIndex = 4;
         this.formatLabelControl.Text = "Format";
         // 
         // formatComboBoxEdit
         // 
         this.formatComboBoxEdit.Location = new System.Drawing.Point(1496, 1433);
         this.formatComboBoxEdit.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
         this.formatComboBoxEdit.Name = "formatComboBoxEdit";
         this.formatComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.formatComboBoxEdit.Size = new System.Drawing.Size(1515, 50);
         this.formatComboBoxEdit.TabIndex = 3;
         // 
         // sourceFilePanelControl
         // 
         this.sourceFilePanelControl.Location = new System.Drawing.Point(19, 52);
         this.sourceFilePanelControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.sourceFilePanelControl.Name = "sourceFilePanelControl";
         this.sourceFilePanelControl.Size = new System.Drawing.Size(3182, 85);
         this.sourceFilePanelControl.TabIndex = 5;
         // 
         // TabControl
         // 
         this.TabControl.Location = new System.Drawing.Point(19, 155);
         this.TabControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
         this.btnImport.Location = new System.Drawing.Point(2387, 1550);
         this.btnImport.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
         this.btnImport.Name = "btnImport";
         this.btnImport.Size = new System.Drawing.Size(401, 118);
         this.btnImport.TabIndex = 6;
         this.btnImport.Text = "Import current sheet";
         // 
         // btnImportAll
         // 
         this.btnImportAll.Location = new System.Drawing.Point(2800, 1550);
         this.btnImportAll.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
         this.btnImportAll.Name = "btnImportAll";
         this.btnImportAll.Size = new System.Drawing.Size(401, 118);
         this.btnImportAll.TabIndex = 7;
         this.btnImportAll.Text = "Import All";
         // 
         // ImporterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "ImporterView";
         this.Controls.Add(this.btnImportAll);
         this.Controls.Add(this.btnImport);
         this.Controls.Add(this.TabControl);
         this.Controls.Add(this.sourceFilePanelControl);
         this.Controls.Add(this.formatLabelControl);
         this.Controls.Add(this.formatComboBoxEdit);
         this.Controls.Add(this.columnMappingPanelControl);
         this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
         this.Name = "ImporterView";
         this.Size = new System.Drawing.Size(3266, 1732);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.formatComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.TabControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingPanelControl)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl dataViewingPanelControl;
      private DevExpress.XtraEditors.PanelControl columnMappingPanelControl;
      private DevExpress.XtraEditors.LabelControl formatLabelControl;
      private DevExpress.XtraEditors.ComboBoxEdit formatComboBoxEdit;
      private DevExpress.XtraEditors.PanelControl sourceFilePanelControl;
      private DevExpress.XtraTab.XtraTabControl TabControl;
      private DevExpress.XtraEditors.SimpleButton btnImport;
      private DevExpress.XtraEditors.SimpleButton btnImportAll;
   }
}