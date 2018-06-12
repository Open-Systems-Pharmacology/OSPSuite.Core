namespace OSPSuite.UI.Views.ParameterIdentifications
{
   partial class ParameterIdentificationChartFeedbackView
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
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.panelChart = new DevExpress.XtraEditors.PanelControl();
         this.layoutItemChart = new DevExpress.XtraLayout.LayoutControlItem();
         this.cbOutputSelection = new DevExpress.XtraEditors.ComboBoxEdit();
         this.layoutItemOutputSelection = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelChart)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemChart)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbOutputSelection.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOutputSelection)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.cbOutputSelection);
         this.layoutControl.Controls.Add(this.panelChart);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(524, 549);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemChart,
            this.layoutItemOutputSelection});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(524, 549);
         this.layoutControlGroup.TextVisible = false;
         // 
         // panelChart
         // 
         this.panelChart.Location = new System.Drawing.Point(135, 26);
         this.panelChart.Name = "panelChart";
         this.panelChart.Size = new System.Drawing.Size(387, 521);
         this.panelChart.TabIndex = 4;
         // 
         // layoutItemChart
         // 
         this.layoutItemChart.Control = this.panelChart;
         this.layoutItemChart.Location = new System.Drawing.Point(0, 24);
         this.layoutItemChart.Name = "layoutItemChart";
         this.layoutItemChart.Size = new System.Drawing.Size(524, 525);
         this.layoutItemChart.TextSize = new System.Drawing.Size(129, 13);
         // 
         // cbOutputSelection
         // 
         this.cbOutputSelection.Location = new System.Drawing.Point(135, 2);
         this.cbOutputSelection.Name = "cbOutputSelection";
         this.cbOutputSelection.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbOutputSelection.Size = new System.Drawing.Size(387, 20);
         this.cbOutputSelection.StyleController = this.layoutControl;
         this.cbOutputSelection.TabIndex = 5;
         // 
         // layoutItemOutputSelection
         // 
         this.layoutItemOutputSelection.Control = this.cbOutputSelection;
         this.layoutItemOutputSelection.Location = new System.Drawing.Point(0, 0);
         this.layoutItemOutputSelection.Name = "layoutItemOutputSelection";
         this.layoutItemOutputSelection.Size = new System.Drawing.Size(524, 24);
         this.layoutItemOutputSelection.TextSize = new System.Drawing.Size(129, 13);
         // 
         // ParameterIdentificationTimeProfileFeedbackView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ParameterIdentificationChartFeedbackView";
         this.Size = new System.Drawing.Size(524, 549);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelChart)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemChart)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbOutputSelection.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOutputSelection)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraEditors.PanelControl panelChart;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemChart;
      private DevExpress.XtraEditors.ComboBoxEdit cbOutputSelection;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemOutputSelection;
   }
}
