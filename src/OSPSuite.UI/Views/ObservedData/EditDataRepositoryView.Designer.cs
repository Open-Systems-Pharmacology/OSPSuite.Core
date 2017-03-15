namespace OSPSuite.UI.Views.ObservedData
{
   partial class EditDataRepositoryView

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
         this.splitControlMetaDataToData = new DevExpress.XtraEditors.SplitContainerControl();
         this.splitControlDataToChart = new DevExpress.XtraEditors.SplitContainerControl();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitControlMetaDataToData)).BeginInit();
         this.splitControlMetaDataToData.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitControlDataToChart)).BeginInit();
         this.splitControlDataToChart.SuspendLayout();
         this.SuspendLayout();
         // 
         // splitControlMetaDataToData
         // 
         this.splitControlMetaDataToData.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitControlMetaDataToData.Location = new System.Drawing.Point(0, 0);
         this.splitControlMetaDataToData.Name = "splitControlMetaDataToData";
         this.splitControlMetaDataToData.Panel1.Padding = new System.Windows.Forms.Padding(10);
         this.splitControlMetaDataToData.Panel1.Text = "Panel1";
         this.splitControlMetaDataToData.Panel2.Controls.Add(this.splitControlDataToChart);
         this.splitControlMetaDataToData.Panel2.Text = "Panel2";
         this.splitControlMetaDataToData.Size = new System.Drawing.Size(922, 633);
         this.splitControlMetaDataToData.SplitterPosition = 264;
         this.splitControlMetaDataToData.TabIndex = 0;
         this.splitControlMetaDataToData.Text = "splitContainerControl1";
         // 
         // splitControlDataToChart
         // 
         this.splitControlDataToChart.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitControlDataToChart.Horizontal = false;
         this.splitControlDataToChart.Location = new System.Drawing.Point(0, 0);
         this.splitControlDataToChart.Name = "splitControlDataToChart";
         this.splitControlDataToChart.Panel1.Padding = new System.Windows.Forms.Padding(10);
         this.splitControlDataToChart.Panel1.Text = "Panel1";
         this.splitControlDataToChart.Panel2.Padding = new System.Windows.Forms.Padding(10);
         this.splitControlDataToChart.Panel2.Text = "Panel2";
         this.splitControlDataToChart.Size = new System.Drawing.Size(653, 633);
         this.splitControlDataToChart.SplitterPosition = 313;
         this.splitControlDataToChart.TabIndex = 0;
         this.splitControlDataToChart.Text = "splitContainerControl2";
         // 
         // EditDataRepositoryView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "EditObservedDataView";
         this.ClientSize = new System.Drawing.Size(922, 633);
         this.Controls.Add(this.splitControlMetaDataToData);
         this.Name = "EditDataRepositoryView";
         this.Text = "EditObservedDataView";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitControlMetaDataToData)).EndInit();
         this.splitControlMetaDataToData.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitControlDataToChart)).EndInit();
         this.splitControlDataToChart.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SplitContainerControl splitControlMetaDataToData;
      private DevExpress.XtraEditors.SplitContainerControl splitControlDataToChart;

   }
}