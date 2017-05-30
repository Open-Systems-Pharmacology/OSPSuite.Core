namespace OSPSuite.Starter.Views
{
   partial class HistogramTestView
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
         DevExpress.XtraCharts.ChartTitle chartTitle19 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle20 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle21 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle22 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle23 = new DevExpress.XtraCharts.ChartTitle();
         DevExpress.XtraCharts.ChartTitle chartTitle24 = new DevExpress.XtraCharts.ChartTitle();
         this.uxLayoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.minTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.maxTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.binsTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.plotButton = new DevExpress.XtraEditors.SimpleButton();
         this.uxHistogramControl = new OSPSuite.UI.Controls.UxHistogramControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.plotButtonControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.binsLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.maxLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.minLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.valuesTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.valuesControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).BeginInit();
         this.uxLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.minTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.maxTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.binsTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxHistogramControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.plotButtonControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.binsLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.maxLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.minLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.valuesTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.valuesControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // uxLayoutControl
         // 
         this.uxLayoutControl.AllowCustomization = false;
         this.uxLayoutControl.Controls.Add(this.valuesTextEdit);
         this.uxLayoutControl.Controls.Add(this.minTextEdit);
         this.uxLayoutControl.Controls.Add(this.maxTextEdit);
         this.uxLayoutControl.Controls.Add(this.binsTextEdit);
         this.uxLayoutControl.Controls.Add(this.plotButton);
         this.uxLayoutControl.Controls.Add(this.uxHistogramControl);
         this.uxLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl.Name = "uxLayoutControl";
         this.uxLayoutControl.Root = this.layoutControlGroup;
         this.uxLayoutControl.Size = new System.Drawing.Size(855, 473);
         this.uxLayoutControl.TabIndex = 0;
         this.uxLayoutControl.Text = "uxLayoutControl1";
         // 
         // minTextEdit
         // 
         this.minTextEdit.Location = new System.Drawing.Point(226, 439);
         this.minTextEdit.Name = "minTextEdit";
         this.minTextEdit.Size = new System.Drawing.Size(116, 20);
         this.minTextEdit.StyleController = this.uxLayoutControl;
         this.minTextEdit.TabIndex = 7;
         // 
         // maxTextEdit
         // 
         this.maxTextEdit.Location = new System.Drawing.Point(393, 439);
         this.maxTextEdit.Name = "maxTextEdit";
         this.maxTextEdit.Size = new System.Drawing.Size(116, 20);
         this.maxTextEdit.StyleController = this.uxLayoutControl;
         this.maxTextEdit.TabIndex = 6;
         // 
         // binsTextEdit
         // 
         this.binsTextEdit.Location = new System.Drawing.Point(560, 439);
         this.binsTextEdit.Name = "binsTextEdit";
         this.binsTextEdit.Size = new System.Drawing.Size(116, 20);
         this.binsTextEdit.StyleController = this.uxLayoutControl;
         this.binsTextEdit.TabIndex = 5;
         // 
         // plotButton
         // 
         this.plotButton.Location = new System.Drawing.Point(12, 439);
         this.plotButton.Name = "plotButton";
         this.plotButton.Size = new System.Drawing.Size(163, 22);
         this.plotButton.StyleController = this.uxLayoutControl;
         this.plotButton.TabIndex = 1;
         this.plotButton.Text = "plotButton";
         // 
         // uxHistogramControl
         // 
         this.uxHistogramControl.DataBindings = null;
         this.uxHistogramControl.Description = "";
         this.uxHistogramControl.DiagramBackColor = System.Drawing.Color.Empty;
         this.uxHistogramControl.Legend.Name = "Default Legend";
         this.uxHistogramControl.Location = new System.Drawing.Point(12, 12);
         this.uxHistogramControl.Name = "uxHistogramControl";
         this.uxHistogramControl.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
         this.uxHistogramControl.Size = new System.Drawing.Size(831, 423);
         this.uxHistogramControl.TabIndex = 4;
         this.uxHistogramControl.Title = "";
         chartTitle19.Font = new System.Drawing.Font("Arial", 16F);
         chartTitle19.Text = "";
         chartTitle19.WordWrap = true;
         chartTitle20.Alignment = System.Drawing.StringAlignment.Near;
         chartTitle20.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
         chartTitle20.Font = new System.Drawing.Font("Arial", 12F);
         chartTitle20.Text = "";
         chartTitle20.WordWrap = true;
         chartTitle21.Font = new System.Drawing.Font("Arial", 16F);
         chartTitle21.Text = "";
         chartTitle21.WordWrap = true;
         chartTitle22.Alignment = System.Drawing.StringAlignment.Near;
         chartTitle22.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
         chartTitle22.Font = new System.Drawing.Font("Arial", 12F);
         chartTitle22.Text = "";
         chartTitle22.WordWrap = true;
         chartTitle23.Font = new System.Drawing.Font("Arial", 16F);
         chartTitle23.Text = "";
         chartTitle23.WordWrap = true;
         chartTitle24.Alignment = System.Drawing.StringAlignment.Near;
         chartTitle24.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
         chartTitle24.Font = new System.Drawing.Font("Arial", 12F);
         chartTitle24.Text = "";
         chartTitle24.WordWrap = true;
         this.uxHistogramControl.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle19,
            chartTitle20,
            chartTitle21,
            chartTitle22,
            chartTitle23,
            chartTitle24});
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem,
            this.plotButtonControlItem,
            this.binsLayoutItem,
            this.maxLayoutItem,
            this.minLayoutItem,
            this.valuesControlItem});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Size = new System.Drawing.Size(855, 473);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutControlItem
         // 
         this.layoutControlItem.Control = this.uxHistogramControl;
         this.layoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem.Name = "layoutControlItem";
         this.layoutControlItem.Size = new System.Drawing.Size(835, 427);
         this.layoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem.TextVisible = false;
         // 
         // plotButtonControlItem
         // 
         this.plotButtonControlItem.Control = this.plotButton;
         this.plotButtonControlItem.Location = new System.Drawing.Point(0, 427);
         this.plotButtonControlItem.Name = "plotButtonControlItem";
         this.plotButtonControlItem.Size = new System.Drawing.Size(167, 26);
         this.plotButtonControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.plotButtonControlItem.TextVisible = false;
         // 
         // binsLayoutItem
         // 
         this.binsLayoutItem.Control = this.binsTextEdit;
         this.binsLayoutItem.Location = new System.Drawing.Point(501, 427);
         this.binsLayoutItem.Name = "binsLayoutItem";
         this.binsLayoutItem.Size = new System.Drawing.Size(167, 26);
         this.binsLayoutItem.Text = "Bins";
         this.binsLayoutItem.TextSize = new System.Drawing.Size(44, 13);
         // 
         // maxLayoutItem
         // 
         this.maxLayoutItem.Control = this.maxTextEdit;
         this.maxLayoutItem.Location = new System.Drawing.Point(334, 427);
         this.maxLayoutItem.Name = "maxLayoutItem";
         this.maxLayoutItem.Size = new System.Drawing.Size(167, 26);
         this.maxLayoutItem.Text = "Maximum";
         this.maxLayoutItem.TextSize = new System.Drawing.Size(44, 13);
         // 
         // minLayoutItem
         // 
         this.minLayoutItem.Control = this.minTextEdit;
         this.minLayoutItem.Location = new System.Drawing.Point(167, 427);
         this.minLayoutItem.Name = "minLayoutItem";
         this.minLayoutItem.Size = new System.Drawing.Size(167, 26);
         this.minLayoutItem.Text = "Minimum";
         this.minLayoutItem.TextSize = new System.Drawing.Size(44, 13);
         // 
         // valuesTextEdit
         // 
         this.valuesTextEdit.Location = new System.Drawing.Point(727, 439);
         this.valuesTextEdit.Name = "valuesTextEdit";
         this.valuesTextEdit.Size = new System.Drawing.Size(116, 20);
         this.valuesTextEdit.StyleController = this.uxLayoutControl;
         this.valuesTextEdit.TabIndex = 8;
         // 
         // valuesControlItem
         // 
         this.valuesControlItem.Control = this.valuesTextEdit;
         this.valuesControlItem.Location = new System.Drawing.Point(668, 427);
         this.valuesControlItem.Name = "valuesControlItem";
         this.valuesControlItem.Size = new System.Drawing.Size(167, 26);
         this.valuesControlItem.Text = "Values";
         this.valuesControlItem.TextSize = new System.Drawing.Size(44, 13);
         // 
         // HistogramTestView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.uxLayoutControl);
         this.Name = "HistogramTestView";
         this.Size = new System.Drawing.Size(855, 473);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).EndInit();
         this.uxLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.minTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.maxTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.binsTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxHistogramControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.plotButtonControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.binsLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.maxLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.minLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.valuesTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.valuesControlItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private UI.Controls.UxLayoutControl uxLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private UI.Controls.UxHistogramControl uxHistogramControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem;
      private DevExpress.XtraEditors.TextEdit minTextEdit;
      private DevExpress.XtraEditors.TextEdit maxTextEdit;
      private DevExpress.XtraEditors.TextEdit binsTextEdit;
      private DevExpress.XtraEditors.SimpleButton plotButton;
      private DevExpress.XtraLayout.LayoutControlItem plotButtonControlItem;
      private DevExpress.XtraLayout.LayoutControlItem binsLayoutItem;
      private DevExpress.XtraLayout.LayoutControlItem maxLayoutItem;
      private DevExpress.XtraLayout.LayoutControlItem minLayoutItem;
      private DevExpress.XtraEditors.TextEdit valuesTextEdit;
      private DevExpress.XtraLayout.LayoutControlItem valuesControlItem;
   }
}
