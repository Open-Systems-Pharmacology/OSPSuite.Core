namespace OSPSuite.Presentation.Importer.Views
{
   partial class XtraUserControl1
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
         this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.dataLayout = new DevExpress.XtraLayout.LayoutControlItem();
         this.sourceFileLayout = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataLayout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileLayout)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Location = new System.Drawing.Point(16, 15);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.Root;
         this.layoutControl1.Size = new System.Drawing.Size(1186, 1239);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.dataLayout,
            this.sourceFileLayout});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1186, 1239);
         this.Root.TextVisible = false;
         // 
         // dataLayout
         // 
         this.dataLayout.Location = new System.Drawing.Point(0, 0);
         this.dataLayout.Name = "namesLayout";
         this.dataLayout.Size = new System.Drawing.Size(1166, 609);
         this.dataLayout.TextSize = new System.Drawing.Size(157, 33);
         // 
         // sourceFileLayout
         // 
         this.sourceFileLayout.Location = new System.Drawing.Point(0, 609);
         this.sourceFileLayout.Name = "namesLayout";
         this.sourceFileLayout.Size = new System.Drawing.Size(1166, 610);
         this.sourceFileLayout.TextSize = new System.Drawing.Size(157, 33);
         // 
         // XtraUserControl1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "XtraUserControl1";
         this.Size = new System.Drawing.Size(1223, 1278);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataLayout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileLayout)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControlItem dataLayout;
      private DevExpress.XtraLayout.LayoutControlItem sourceFileLayout;
      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
   }
}
