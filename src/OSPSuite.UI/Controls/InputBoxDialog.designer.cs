namespace OSPSuite.UI.Controls
{
   partial class InputBoxDialog
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
         this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.cbInput = new DevExpress.XtraEditors.MRUEdit();
         this.layoutItemInput = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.lblPrompt = new DevExpress.XtraEditors.LabelControl();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbInput.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemInput)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // tablePanel
         // 
         this.tablePanel.Location = new System.Drawing.Point(0, 89);
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.lblPrompt);
         this.layoutControl.Controls.Add(this.cbInput);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(580, 89);
         this.layoutControl.TabIndex = 39;
         this.layoutControl.Text = "layoutControl1";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemInput,
            this.emptySpaceItem1,
            this.layoutControlItem2});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(580, 89);
         this.Root.TextVisible = false;
         // 
         // cbInput
         // 
         this.cbInput.Location = new System.Drawing.Point(12, 29);
         this.cbInput.Name = "cbInput";
         this.cbInput.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbInput.Size = new System.Drawing.Size(556, 20);
         this.cbInput.StyleController = this.layoutControl;
         this.cbInput.TabIndex = 6;
         // 
         // layoutItemInput
         // 
         this.layoutItemInput.Control = this.cbInput;
         this.layoutItemInput.Location = new System.Drawing.Point(0, 17);
         this.layoutItemInput.Name = "layoutItemInput";
         this.layoutItemInput.Size = new System.Drawing.Size(560, 24);
         this.layoutItemInput.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemInput.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 41);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(560, 28);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // lblPrompt
         // 
         this.lblPrompt.Location = new System.Drawing.Point(12, 12);
         this.lblPrompt.Name = "lblPrompt";
         this.lblPrompt.Size = new System.Drawing.Size(44, 13);
         this.lblPrompt.StyleController = this.layoutControl;
         this.lblPrompt.TabIndex = 8;
         this.lblPrompt.Text = "lblPrompt";
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.lblPrompt;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(560, 17);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // InputBoxDialog
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "InputBoxDialog";
         this.ClientSize = new System.Drawing.Size(580, 132);
         this.Controls.Add(this.layoutControl);
         this.Name = "InputBoxDialog";
         this.Text = "InputBoxDialog";
         this.Controls.SetChildIndex(this.tablePanel, 0);
         this.Controls.SetChildIndex(this.layoutControl, 0);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbInput.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemInput)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.MRUEdit cbInput;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemInput;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.LabelControl lblPrompt;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
   }
}