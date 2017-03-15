namespace OSPSuite.UI.Controls
{
   partial class FilterTreeView
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
         this.layoutControl1 = new UxLayoutControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.treeView = new UxImageTreeView();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.btnFilter = new DevExpress.XtraEditors.ButtonEdit();
         this.layoutItemFilter = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.treeView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btnFilter.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFilter)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.btnFilter);
         this.layoutControl1.Controls.Add(this.treeView);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(283, 419);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutItemFilter});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(283, 419);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // treeView
         // 
         this.treeView.Location = new System.Drawing.Point(2, 26);
         this.treeView.Name = "treeView";
         this.treeView.Size = new System.Drawing.Size(279, 391);
         this.treeView.TabIndex = 4;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.treeView;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(283, 395);
         this.layoutControlItem1.Text = "layoutControlItem1";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextToControlDistance = 0;
         this.layoutControlItem1.TextVisible = false;
         // 
         // buttonFilter
         // 
         this.btnFilter.Location = new System.Drawing.Point(82, 2);
         this.btnFilter.Name = "btnFilter";
         this.btnFilter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btnFilter.Size = new System.Drawing.Size(199, 20);
         this.btnFilter.StyleController = this.layoutControl1;
         this.btnFilter.TabIndex = 5;
         // 
         // layoutItemFilter
         // 
         this.layoutItemFilter.Control = this.btnFilter;
         this.layoutItemFilter.CustomizationFormText = "layoutItemFilter";
         this.layoutItemFilter.Location = new System.Drawing.Point(0, 0);
         this.layoutItemFilter.Name = "layoutItemFilter";
         this.layoutItemFilter.Size = new System.Drawing.Size(283, 24);
         this.layoutItemFilter.Text = "layoutItemFilter";
         this.layoutItemFilter.TextSize = new System.Drawing.Size(76, 13);
         // 
         // UxFilterTreeView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "FilterTreeView";
         this.Size = new System.Drawing.Size(283, 419);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.treeView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btnFilter.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFilter)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      
      private UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.ButtonEdit btnFilter;
      private UxImageTreeView treeView;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFilter;
   }
}
