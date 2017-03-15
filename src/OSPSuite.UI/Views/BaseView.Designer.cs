namespace OSPSuite.UI.Views
{
   partial class BaseView
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
         _basePresenter = null;
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         this._helpProvider = new System.Windows.Forms.HelpProvider();
         this._errorProvider = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         this.SuspendLayout();
         // 
         // _errorProvider
         // 
         this._errorProvider.ContainerControl = this;
         // 
         // BaseView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(526, 428);
         this._helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.TableOfContents);
         this.Name = "BaseView";
         this._helpProvider.SetShowHelp(this, true);
         this.ShowInTaskbar = false;
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "BaseView";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.HelpProvider _helpProvider;
      protected DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider _errorProvider;
   }
}