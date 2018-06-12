namespace OSPSuite.UI.Views.Diagram
{
   partial class BaseDiagramView
   {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      override protected void Dispose(bool disposing)
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
         this.PopupBarManager = new DevExpress.XtraBars.BarManager(this.components);
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         this._goView = new DiagramBaseView();
         ((System.ComponentModel.ISupportInitialize)(this.PopupBarManager)).BeginInit();
         this.SuspendLayout();
         // 
         // _barManager
         // 
         this.PopupBarManager.DockControls.Add(this.barDockControlTop);
         this.PopupBarManager.DockControls.Add(this.barDockControlBottom);
         this.PopupBarManager.DockControls.Add(this.barDockControlLeft);
         this.PopupBarManager.DockControls.Add(this.barDockControlRight);
         this.PopupBarManager.Form = this;
         this.PopupBarManager.MaxItemId = 0;
         // 
         // _goView
         // 
         this._goView.ArrowMoveLarge = 10F;
         this._goView.ArrowMoveSmall = 1F;
         this._goView.BackColor = System.Drawing.Color.White;
         this._goView.Dock = System.Windows.Forms.DockStyle.Fill;
         this._goView.Location = new System.Drawing.Point(0, 0);
         this._goView.Name = "_goView";
         this._goView.Size = new System.Drawing.Size(436, 357);
         this._goView.TabIndex = 8;
         this._goView.Text = "goView";
         // 
         // ReactionDiagramView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this._goView);
         this.Controls.Add(this.barDockControlLeft);
         this.Controls.Add(this.barDockControlRight);
         this.Controls.Add(this.barDockControlBottom);
         this.Controls.Add(this.barDockControlTop);
         this.Name = "ReactionDiagramView";
         this.Size = new System.Drawing.Size(436, 357);
         ((System.ComponentModel.ISupportInitialize)(this.PopupBarManager)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraBars.BarDockControl barDockControlTop;
      private DevExpress.XtraBars.BarDockControl barDockControlBottom;
      private DevExpress.XtraBars.BarDockControl barDockControlLeft;
      private DevExpress.XtraBars.BarDockControl barDockControlRight;
      protected DiagramBaseView _goView;
   }
}