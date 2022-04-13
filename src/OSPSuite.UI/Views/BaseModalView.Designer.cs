
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views
{
   partial class BaseModalView
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
         _shortcutsManager.Dispose();
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.btnOk = new OSPSuite.UI.Controls.UxSimpleButton();
         this.btnExtra = new OSPSuite.UI.Controls.UxSimpleButton();
         this.btnCancel = new OSPSuite.UI.Controls.UxSimpleButton();
         this.tablePanel = new DevExpress.Utils.Layout.TablePanel();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();   
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         this.tablePanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // btnOk
         // 
         this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.tablePanel.SetColumn(this.btnOk, 2);
         this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnOk.Location = new System.Drawing.Point(300, 10);
         this.btnOk.Manager = null;
         this.btnOk.Name = "btnOk";
         this.tablePanel.SetRow(this.btnOk, 0);
         this.btnOk.Shortcut = System.Windows.Forms.Keys.None;
         this.btnOk.Size = new System.Drawing.Size(135, 22);
         this.btnOk.TabIndex = 30;
         this.btnOk.Text = "btnOk";
         // 
         // btnExtra
         // 
         this.tablePanel.SetColumn(this.btnExtra, 0);
         this.btnExtra.Location = new System.Drawing.Point(3, 10);
         this.btnExtra.Manager = null;
         this.btnExtra.Name = "btnExtra";
         this.tablePanel.SetRow(this.btnExtra, 0);
         this.btnExtra.Shortcut = System.Windows.Forms.Keys.None;
         this.btnExtra.Size = new System.Drawing.Size(150, 22);
         this.btnExtra.TabIndex = 32;
         this.btnExtra.Text = "btnExtra";
         // 
         // btnCancel
         // 
         this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.tablePanel.SetColumn(this.btnCancel, 3);
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancel.Location = new System.Drawing.Point(442, 10);
         this.btnCancel.Manager = null;
         this.btnCancel.Name = "btnCancel";
         this.tablePanel.SetRow(this.btnCancel, 0);
         this.btnCancel.Shortcut = System.Windows.Forms.Keys.None;
         this.btnCancel.Size = new System.Drawing.Size(135, 22);
         this.btnCancel.TabIndex = 31;
         this.btnCancel.Text = "btnCancel";
         // 
         // tablePanel
         // 
         this.tablePanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.tablePanel.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 55F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
         this.tablePanel.Controls.Add(this.btnExtra);
         this.tablePanel.Controls.Add(this.btnOk);
         this.tablePanel.Controls.Add(this.btnCancel);
         this.tablePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.tablePanel.Location = new System.Drawing.Point(0, 309);
         this.tablePanel.Name = "tablePanel";
         this.tablePanel.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
         this.tablePanel.Size = new System.Drawing.Size(580, 43);
         this.tablePanel.TabIndex = 34;
         // 
         // BaseModalView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnCancel;
         this.Caption = "BaseModalView";
         this.ClientSize = new System.Drawing.Size(580, 352);
         this.Controls.Add(this.tablePanel);
         this.Name = "BaseModalView";
         this.Text = "BaseModalView";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         this.tablePanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private UxSimpleButton btnCancel;
      private UxSimpleButton btnOk;
      private UxSimpleButton btnExtra;
      protected DevExpress.Utils.Layout.TablePanel tablePanel;
   }
}