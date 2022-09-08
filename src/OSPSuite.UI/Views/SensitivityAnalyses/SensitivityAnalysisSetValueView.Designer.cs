using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.SensitivityAnalyses
{
   partial class SensitivityAnalysisSetValueView
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
         this.btnApplyAll = new DevExpress.XtraEditors.SimpleButton();
         this.btnApplySelection = new DevExpress.XtraEditors.SimpleButton();
         this.tbValue = new DevExpress.XtraEditors.TextEdit();
         this.tablePanel = new DevExpress.Utils.Layout.TablePanel();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbValue.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         this.tablePanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // btnApplyAll
         // 
         this.tablePanel.SetColumn(this.btnApplyAll, 1);
         this.btnApplyAll.Location = new System.Drawing.Point(163, 5);
         this.btnApplyAll.Name = "btnApplyAll";
         this.tablePanel.SetRow(this.btnApplyAll, 0);
         this.btnApplyAll.Size = new System.Drawing.Size(260, 20);
         this.btnApplyAll.TabIndex = 4;
         this.btnApplyAll.Text = "btnApplyAll";
         // 
         // btnApplySelection
         // 
         this.tablePanel.SetColumn(this.btnApplySelection, 2);
         this.btnApplySelection.Location = new System.Drawing.Point(428, 5);
         this.btnApplySelection.Name = "btnApplySelection";
         this.tablePanel.SetRow(this.btnApplySelection, 0);
         this.btnApplySelection.Size = new System.Drawing.Size(260, 20);
         this.btnApplySelection.TabIndex = 5;
         this.btnApplySelection.Text = "btnApplySelection";
         // 
         // tbValue
         // 
         this.tablePanel.SetColumn(this.tbValue, 0);
         this.tbValue.Location = new System.Drawing.Point(3, 5);
         this.tbValue.Name = "tbValue";
         this.tablePanel.SetRow(this.tbValue, 0);
         this.tbValue.Size = new System.Drawing.Size(154, 20);
         this.tbValue.TabIndex = 6;
         // 
         // tablePanel
         // 
         this.tablePanel.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 30.04F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
         this.tablePanel.Controls.Add(this.tbValue);
         this.tablePanel.Controls.Add(this.btnApplySelection);
         this.tablePanel.Controls.Add(this.btnApplyAll);
         this.tablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tablePanel.Location = new System.Drawing.Point(0, 0);
         this.tablePanel.Name = "tablePanel";
         this.tablePanel.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
         this.tablePanel.Size = new System.Drawing.Size(691, 31);
         this.tablePanel.TabIndex = 1;
         // 
         // SensitivityAnalysisSetValueView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tablePanel);
         this.Margin = new System.Windows.Forms.Padding(0);
         this.Name = "SensitivityAnalysisSetValueView";
         this.Size = new System.Drawing.Size(691, 31);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbValue.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         this.tablePanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton btnApplyAll;
      private DevExpress.Utils.Layout.TablePanel tablePanel;
      private DevExpress.XtraEditors.TextEdit tbValue;
      private DevExpress.XtraEditors.SimpleButton btnApplySelection;
   }
}
