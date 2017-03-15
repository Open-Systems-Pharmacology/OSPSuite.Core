using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views
{
   partial class BaseExplorerView
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
         this.treeView = new UxImageTreeView();
         this._toolTipController = new DevExpress.Utils.ToolTipController(this.components);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.treeView)).BeginInit();
         this.SuspendLayout();
         // 
         // treeView
         // 
         this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
         this.treeView.IsLatched = false;
         this.treeView.Location = new System.Drawing.Point(0, 0);
         this.treeView.Name = "treeView";
         this.treeView.OptionsBehavior.Editable = false;
         this.treeView.OptionsView.ShowColumns = false;
         this.treeView.OptionsView.ShowHorzLines = false;
         this.treeView.OptionsView.ShowIndicator = false;
         this.treeView.OptionsView.ShowVertLines = false;
         this.treeView.Size = new System.Drawing.Size(335, 370);
         this.treeView.TabIndex = 0;
         this.treeView.UseLazyLoading = false;
         // 
         // ExplorerView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.treeView);
         this.Name = "BaseExplorerView";
         this.Size = new System.Drawing.Size(335, 370);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.treeView)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      protected UxImageTreeView treeView;
      private DevExpress.Utils.ToolTipController _toolTipController;
   }
}