namespace OSPSuite.Starter.Views
{
   partial class DynamicTestView
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
         layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
         btnLoadViews = new DevExpress.XtraEditors.SimpleButton();
         panel = new DevExpress.XtraEditors.PanelControl();
         Root = new DevExpress.XtraLayout.LayoutControlGroup();
         layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)tablePanel).BeginInit();
         ((System.ComponentModel.ISupportInitialize)_errorProvider).BeginInit();
         ((System.ComponentModel.ISupportInitialize)layoutControl1).BeginInit();
         layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)panel).BeginInit();
         ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
         ((System.ComponentModel.ISupportInitialize)layoutControlItem1).BeginInit();
         ((System.ComponentModel.ISupportInitialize)layoutControlItem2).BeginInit();
         SuspendLayout();
         // 
         // tablePanel
         // 
         tablePanel.Location = new System.Drawing.Point(0, 552);
         tablePanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         tablePanel.Size = new System.Drawing.Size(585, 35);
         // 
         // layoutControl1
         // 
         layoutControl1.Controls.Add(btnLoadViews);
         layoutControl1.Controls.Add(panel);
         layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         layoutControl1.Location = new System.Drawing.Point(0, 0);
         layoutControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         layoutControl1.Name = "layoutControl1";
         layoutControl1.Root = Root;
         layoutControl1.Size = new System.Drawing.Size(585, 552);
         layoutControl1.TabIndex = 38;
         layoutControl1.Text = "layoutControl1";
         // 
         // btnLoadViews
         // 
         btnLoadViews.Location = new System.Drawing.Point(12, 12);
         btnLoadViews.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         btnLoadViews.Name = "btnLoadViews";
         btnLoadViews.Size = new System.Drawing.Size(561, 22);
         btnLoadViews.StyleController = layoutControl1;
         btnLoadViews.TabIndex = 0;
         btnLoadViews.Text = "btnLoadViews";
         // 
         // panel
         // 
         panel.Location = new System.Drawing.Point(12, 38);
         panel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         panel.Name = "panel";
         panel.Size = new System.Drawing.Size(561, 502);
         panel.TabIndex = 4;
         // 
         // Root
         // 
         Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         Root.GroupBordersVisible = false;
         Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem1, layoutControlItem2 });
         Root.Name = "Root";
         Root.Size = new System.Drawing.Size(585, 552);
         Root.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         layoutControlItem1.Control = panel;
         layoutControlItem1.Location = new System.Drawing.Point(0, 26);
         layoutControlItem1.Name = "layoutControlItem1";
         layoutControlItem1.Size = new System.Drawing.Size(565, 506);
         layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         layoutControlItem2.Control = btnLoadViews;
         layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         layoutControlItem2.Name = "layoutControlItem2";
         layoutControlItem2.Size = new System.Drawing.Size(565, 26);
         layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         layoutControlItem2.TextVisible = false;
         // 
         // DynamicTestView
         // 
         AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         Caption = "DynamicTestView";
         ClientSize = new System.Drawing.Size(585, 587);
         Controls.Add(layoutControl1);
         Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         Name = "DynamicTestView";
         Text = "DynamicTestView";
         Controls.SetChildIndex(tablePanel, 0);
         Controls.SetChildIndex(layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)tablePanel).EndInit();
         ((System.ComponentModel.ISupportInitialize)_errorProvider).EndInit();
         ((System.ComponentModel.ISupportInitialize)layoutControl1).EndInit();
         layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)panel).EndInit();
         ((System.ComponentModel.ISupportInitialize)Root).EndInit();
         ((System.ComponentModel.ISupportInitialize)layoutControlItem1).EndInit();
         ((System.ComponentModel.ISupportInitialize)layoutControlItem2).EndInit();
         ResumeLayout(false);
         PerformLayout();

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraEditors.PanelControl panel;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraEditors.SimpleButton btnLoadViews;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
   }
}