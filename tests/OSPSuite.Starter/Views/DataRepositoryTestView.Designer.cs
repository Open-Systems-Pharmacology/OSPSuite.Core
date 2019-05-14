namespace OSPSuite.Starter.Views
{
   partial class DataRepositoryTestView
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
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.metaDataPanel = new DevExpress.XtraEditors.PanelControl();
         this.splitContainerControl = new DevExpress.XtraEditors.SplitContainerControl();
         this.dataPanel = new DevExpress.XtraEditors.PanelControl();
         this.chartPanel = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.splitContainerControlLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.btnExportToCSV = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.metaDataPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).BeginInit();
         this.splitContainerControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dataPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.btnExportToCSV);
         this.layoutControl.Controls.Add(this.metaDataPanel);
         this.layoutControl.Controls.Add(this.splitContainerControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(812, 451);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // metaDataPanel
         // 
         this.metaDataPanel.Location = new System.Drawing.Point(2, 28);
         this.metaDataPanel.Name = "metaDataPanel";
         this.metaDataPanel.Size = new System.Drawing.Size(154, 421);
         this.metaDataPanel.TabIndex = 5;
         // 
         // splitContainerControl
         // 
         this.splitContainerControl.Horizontal = false;
         this.splitContainerControl.Location = new System.Drawing.Point(160, 2);
         this.splitContainerControl.Name = "splitContainerControl";
         this.splitContainerControl.Panel1.Controls.Add(this.dataPanel);
         this.splitContainerControl.Panel1.Text = "Panel1";
         this.splitContainerControl.Panel2.Controls.Add(this.chartPanel);
         this.splitContainerControl.Panel2.Text = "Panel2";
         this.splitContainerControl.Size = new System.Drawing.Size(650, 447);
         this.splitContainerControl.TabIndex = 4;
         this.splitContainerControl.Text = "splitContainerControl";
         // 
         // dataPanel
         // 
         this.dataPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.dataPanel.Location = new System.Drawing.Point(0, 0);
         this.dataPanel.Name = "dataPanel";
         this.dataPanel.Size = new System.Drawing.Size(650, 100);
         this.dataPanel.TabIndex = 0;
         // 
         // chartPanel
         // 
         this.chartPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.chartPanel.Location = new System.Drawing.Point(0, 0);
         this.chartPanel.Name = "chartPanel";
         this.chartPanel.Size = new System.Drawing.Size(650, 342);
         this.chartPanel.TabIndex = 0;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.splitContainerControlLayoutItem,
            this.layoutControlItem1,
            this.layoutControlItem3});
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(812, 451);
         this.layoutControlGroup.TextVisible = false;
         // 
         // splitContainerControlLayoutItem
         // 
         this.splitContainerControlLayoutItem.Control = this.splitContainerControl;
         this.splitContainerControlLayoutItem.Location = new System.Drawing.Point(158, 0);
         this.splitContainerControlLayoutItem.Name = "splitContainerControlLayoutItem";
         this.splitContainerControlLayoutItem.Size = new System.Drawing.Size(654, 451);
         this.splitContainerControlLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.splitContainerControlLayoutItem.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.metaDataPanel;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(158, 425);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // btnExportToCSV
         // 
         this.btnExportToCSV.Location = new System.Drawing.Point(2, 2);
         this.btnExportToCSV.Name = "btnExportToCSV";
         this.btnExportToCSV.Size = new System.Drawing.Size(154, 22);
         this.btnExportToCSV.StyleController = this.layoutControl;
         this.btnExportToCSV.TabIndex = 6;
         this.btnExportToCSV.Text = "btnExportToCSV";
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.btnExportToCSV;
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(158, 26);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // DataRepositoryTestView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "DataRepositoryTestView";
         this.Size = new System.Drawing.Size(812, 451);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.metaDataPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).EndInit();
         this.splitContainerControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.dataPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraEditors.SplitContainerControl splitContainerControl;
      private DevExpress.XtraLayout.LayoutControlItem splitContainerControlLayoutItem;
      private DevExpress.XtraEditors.PanelControl dataPanel;
      private DevExpress.XtraEditors.PanelControl chartPanel;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraEditors.PanelControl metaDataPanel;
      private UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraEditors.SimpleButton btnExportToCSV;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
   }
}
