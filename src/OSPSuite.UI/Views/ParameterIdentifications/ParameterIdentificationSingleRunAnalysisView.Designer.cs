namespace OSPSuite.UI.Views.ParameterIdentifications
{
   partial class ParameterIdentificationSingleRunAnalysisView
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
         this.cbRunSelection = new DevExpress.XtraEditors.ComboBoxEdit();
         this.panelAnalysis = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemChartControl = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemRunSelection = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbRunSelection.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelAnalysis)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemChartControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRunSelection)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.cbRunSelection);
         this.layoutControl.Controls.Add(this.panelAnalysis);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(608, 528);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // cbRunSelection
         // 
         this.cbRunSelection.Location = new System.Drawing.Point(119, 2);
         this.cbRunSelection.Name = "cbRunSelection";
         this.cbRunSelection.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbRunSelection.Size = new System.Drawing.Size(487, 20);
         this.cbRunSelection.StyleController = this.layoutControl;
         this.cbRunSelection.TabIndex = 5;
         // 
         // panelAnalysis
         // 
         this.panelAnalysis.Location = new System.Drawing.Point(119, 26);
         this.panelAnalysis.Name = "panelAnalysis";
         this.panelAnalysis.Size = new System.Drawing.Size(487, 500);
         this.panelAnalysis.TabIndex = 4;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemChartControl,
            this.layoutItemRunSelection});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(608, 528);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemChartControl
         // 
         this.layoutItemChartControl.Control = this.panelAnalysis;
         this.layoutItemChartControl.Location = new System.Drawing.Point(0, 24);
         this.layoutItemChartControl.Name = "layoutItemChartControl";
         this.layoutItemChartControl.Size = new System.Drawing.Size(608, 504);
         this.layoutItemChartControl.TextSize = new System.Drawing.Size(114, 13);
         // 
         // layoutItemRunSelection
         // 
         this.layoutItemRunSelection.Control = this.cbRunSelection;
         this.layoutItemRunSelection.Location = new System.Drawing.Point(0, 0);
         this.layoutItemRunSelection.Name = "layoutItemRunSelection";
         this.layoutItemRunSelection.Size = new System.Drawing.Size(608, 24);
         this.layoutItemRunSelection.TextSize = new System.Drawing.Size(114, 13);
         // 
         // ParameterIdentificationSingleRunAnalysisView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ParameterIdentificationSingleRunAnalysisView";
         this.Size = new System.Drawing.Size(608, 528);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cbRunSelection.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelAnalysis)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemChartControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRunSelection)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraEditors.ComboBoxEdit cbRunSelection;
      private DevExpress.XtraEditors.PanelControl panelAnalysis;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemChartControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemRunSelection;
   }
}
