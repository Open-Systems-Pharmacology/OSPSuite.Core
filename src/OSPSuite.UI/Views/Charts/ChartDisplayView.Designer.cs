namespace OSPSuite.UI.Views.Charts
{
   partial class ChartDisplayView
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
         this.components = new System.ComponentModel.Container();
         this._chartControl = new OSPSuite.UI.Controls.UxChartControl(useDefaultPopupMechanism:false);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._chartControl)).BeginInit();
         this.SuspendLayout();
         // 
         // _chartControl
         // 
         this._chartControl.DataBindings = null;
         this._chartControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this._chartControl.Legend.Name = "Default Legend";
         this._chartControl.Location = new System.Drawing.Point(0, 0);
         this._chartControl.Name = "_chartControl";
         this._chartControl.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
         this._chartControl.Size = new System.Drawing.Size(380, 430);
         this._chartControl.TabIndex = 0;
         // 
         // ChartDisplayView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this._chartControl);
         this.Name = "ChartDisplayView";
         this.Size = new System.Drawing.Size(380, 430);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._chartControl)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxChartControl _chartControl;
   }
}
