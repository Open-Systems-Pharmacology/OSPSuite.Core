namespace OSPSuite.UI.Views.Charts
{
   partial class SimpleChartView
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
         this.logLinToggleSwitch = new DevExpress.XtraEditors.ToggleSwitch();
         this.chartPanel = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemChart = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemLogScaling = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.logLinToggleSwitch.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemChart)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLogScaling)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.logLinToggleSwitch);
         this.layoutControl.Controls.Add(this.chartPanel);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(516, 488);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // logLinToggleSwitch
         // 
         this.logLinToggleSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.logLinToggleSwitch.Location = new System.Drawing.Point(2, 462);
         this.logLinToggleSwitch.MaximumSize = new System.Drawing.Size(171, 0);
         this.logLinToggleSwitch.MinimumSize = new System.Drawing.Size(171, 0);
         this.logLinToggleSwitch.Name = "logLinToggleSwitch";
         this.logLinToggleSwitch.Properties.AllowFocused = false;
         this.logLinToggleSwitch.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.logLinToggleSwitch.Properties.Appearance.Options.UseBackColor = true;
         this.logLinToggleSwitch.Properties.OffText = "Off";
         this.logLinToggleSwitch.Properties.OnText = "On";
         this.logLinToggleSwitch.Size = new System.Drawing.Size(171, 24);
         this.logLinToggleSwitch.StyleController = this.layoutControl;
         this.logLinToggleSwitch.TabIndex = 5;
         // 
         // chartPanel
         // 
         this.chartPanel.Location = new System.Drawing.Point(2, 2);
         this.chartPanel.Name = "chartPanel";
         this.chartPanel.Size = new System.Drawing.Size(512, 456);
         this.chartPanel.TabIndex = 4;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemChart,
            this.layoutItemLogScaling});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(516, 488);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemChart
         // 
         this.layoutItemChart.Control = this.chartPanel;
         this.layoutItemChart.CustomizationFormText = "layoutControlItem1";
         this.layoutItemChart.Location = new System.Drawing.Point(0, 0);
         this.layoutItemChart.Name = "layoutItemChart";
         this.layoutItemChart.Size = new System.Drawing.Size(516, 460);
         this.layoutItemChart.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemChart.TextVisible = false;
         // 
         // layoutItemLogScaling
         // 
         this.layoutItemLogScaling.Control = this.logLinToggleSwitch;
         this.layoutItemLogScaling.CustomizationFormText = "layoutControlItem2";
         this.layoutItemLogScaling.Location = new System.Drawing.Point(0, 460);
         this.layoutItemLogScaling.Name = "layoutItemLogScaling";
         this.layoutItemLogScaling.Size = new System.Drawing.Size(516, 28);
         this.layoutItemLogScaling.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemLogScaling.TextVisible = false;
         // 
         // SimpleChartView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "SimpleChartView";
         this.Size = new System.Drawing.Size(516, 488);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.logLinToggleSwitch.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chartPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemChart)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLogScaling)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraEditors.ToggleSwitch logLinToggleSwitch;
      private DevExpress.XtraEditors.PanelControl chartPanel;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemChart;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemLogScaling;
   }
}
