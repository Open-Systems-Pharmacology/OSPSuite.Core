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
         this.dataViewingPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.columnMappingPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.formatLabelControl = new DevExpress.XtraEditors.LabelControl();
         this.formatComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.sourceFilePanelControl = new DevExpress.XtraEditors.PanelControl();
         this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
         this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
         this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.formatComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
         this.xtraTabControl1.SuspendLayout();
         this.xtraTabPage1.SuspendLayout();
         this.SuspendLayout();
         // 
         // dataViewingPanelControl
         // 
         this.dataViewingPanelControl.Location = new System.Drawing.Point(3, 0);
         this.dataViewingPanelControl.Name = "dataViewingPanelControl";
         this.dataViewingPanelControl.Size = new System.Drawing.Size(1380, 1132);
         this.dataViewingPanelControl.TabIndex = 0;
         // 
         // columnMappingPanelControl
         // 
         this.columnMappingPanelControl.Location = new System.Drawing.Point(1431, 154);
         this.columnMappingPanelControl.Name = "columnMappingPanelControl";
         this.columnMappingPanelControl.Size = new System.Drawing.Size(1770, 1226);
         this.columnMappingPanelControl.TabIndex = 1;
         // 
         // formatLabelControl
         // 
         this.formatLabelControl.Location = new System.Drawing.Point(1379, 1442);
         this.formatLabelControl.Margin = new System.Windows.Forms.Padding(6);
         this.formatLabelControl.Name = "formatLabelControl";
         this.formatLabelControl.Size = new System.Drawing.Size(85, 33);
         this.formatLabelControl.TabIndex = 4;
         this.formatLabelControl.Text = "Format";
         // 
         // formatComboBoxEdit
         // 
         this.formatComboBoxEdit.Location = new System.Drawing.Point(1496, 1434);
         this.formatComboBoxEdit.Margin = new System.Windows.Forms.Padding(6);
         this.formatComboBoxEdit.Name = "formatComboBoxEdit";
         this.formatComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.formatComboBoxEdit.Size = new System.Drawing.Size(1514, 50);
         this.formatComboBoxEdit.TabIndex = 3;
         // 
         // sourceFilePanelControl
         // 
         this.sourceFilePanelControl.Location = new System.Drawing.Point(19, 51);
         this.sourceFilePanelControl.Name = "sourceFilePanelControl";
         this.sourceFilePanelControl.Size = new System.Drawing.Size(3182, 85);
         this.sourceFilePanelControl.TabIndex = 5;
         // 
         // xtraTabControl1
         // 
         this.xtraTabControl1.Location = new System.Drawing.Point(19, 154);
         this.xtraTabControl1.Name = "xtraTabControl1";
         this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
         this.xtraTabControl1.Size = new System.Drawing.Size(1385, 1192);
         this.xtraTabControl1.TabIndex = 0;
         this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
         this.xtraTabControl1.Click += new System.EventHandler(this.xtraTabControl1_Click);
         // 
         // xtraTabPage1
         // 
         this.xtraTabPage1.Controls.Add(this.dataViewingPanelControl);
         this.xtraTabPage1.Name = "xtraTabPage1";
         this.xtraTabPage1.Size = new System.Drawing.Size(1381, 1130);
         this.xtraTabPage1.Text = "xtraTabPage1";
         // 
         // xtraTabPage2
         // 
         this.xtraTabPage2.Name = "xtraTabPage2";
         this.xtraTabPage2.Size = new System.Drawing.Size(1381, 1130);
         this.xtraTabPage2.Text = "xtraTabPage2";
         // 
         // ImporterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "ImporterView";
         this.Controls.Add(this.xtraTabControl1);
         this.Controls.Add(this.sourceFilePanelControl);
         this.Controls.Add(this.formatLabelControl);
         this.Controls.Add(this.formatComboBoxEdit);
         this.Controls.Add(this.columnMappingPanelControl);
         this.Name = "ImporterView";
         this.Size = new System.Drawing.Size(3266, 1732);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.formatComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
         this.xtraTabControl1.ResumeLayout(false);
         this.xtraTabPage1.ResumeLayout(false);
         this.xtraTabPage2.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl dataViewingPanelControl;
      private DevExpress.XtraEditors.PanelControl columnMappingPanelControl;
      private DevExpress.XtraEditors.LabelControl formatLabelControl;
      private DevExpress.XtraEditors.ComboBoxEdit formatComboBoxEdit;
      private DevExpress.XtraEditors.PanelControl sourceFilePanelControl;
      private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
      private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
      private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
   }
}