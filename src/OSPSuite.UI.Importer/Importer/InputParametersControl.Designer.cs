namespace OSPSuite.UI.Importer
{
   partial class InputParametersControl
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
         cleanMemory();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         ((System.ComponentModel.ISupportInitialize)(layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControlGroup
         // 
         layoutControlGroup.CustomizationFormText = "layoutControlGroup";
         layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         layoutControlGroup.GroupBordersVisible = false;
         layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         layoutControlGroup.Name = "layoutControlGroup";
         layoutControlGroup.Size = new System.Drawing.Size(403, 154);
         layoutControlGroup.Text = "layoutControlGroup";
         layoutControlGroup.TextVisible = false;
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(403, 154);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // InputParametersControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "InputParametersControl";
         this.Size = new System.Drawing.Size(403, 154);
         ((System.ComponentModel.ISupportInitialize)(layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
   }
}
