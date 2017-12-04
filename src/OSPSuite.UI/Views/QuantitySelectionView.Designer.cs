namespace OSPSuite.UI.Views
{
   partial class QuantitySelectionView
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
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnDeselectAll = new DevExpress.XtraEditors.SimpleButton();
         this.txtInfo = new DevExpress.XtraEditors.TextEdit();
         this.lblDescription = new DevExpress.XtraEditors.LabelControl();
         this.layoutControl = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemDeselectAll = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.panelQuantities = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.panelSelectedQuantities = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtInfo.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDeselectAll)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelQuantities)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelSelectedQuantities)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.panelSelectedQuantities);
         this.layoutControl1.Controls.Add(this.panelQuantities);
         this.layoutControl1.Controls.Add(this.btnDeselectAll);
         this.layoutControl1.Controls.Add(this.txtInfo);
         this.layoutControl1.Controls.Add(this.lblDescription);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(855, 250, 250, 350);
         this.layoutControl1.Root = this.layoutControl;
         this.layoutControl1.Size = new System.Drawing.Size(513, 512);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // btnDeselectAll
         // 
         this.btnDeselectAll.Location = new System.Drawing.Point(258, 19);
         this.btnDeselectAll.Name = "btnDeselectAll";
         this.btnDeselectAll.Size = new System.Drawing.Size(253, 22);
         this.btnDeselectAll.StyleController = this.layoutControl1;
         this.btnDeselectAll.TabIndex = 6;
         this.btnDeselectAll.Text = "btnDeselectAll";
         // 
         // txtInfo
         // 
         this.txtInfo.Location = new System.Drawing.Point(2, 19);
         this.txtInfo.Name = "txtInfo";
         this.txtInfo.Size = new System.Drawing.Size(252, 20);
         this.txtInfo.StyleController = this.layoutControl1;
         this.txtInfo.TabIndex = 5;
         // 
         // lblDescription
         // 
         this.lblDescription.Location = new System.Drawing.Point(2, 2);
         this.lblDescription.Name = "lblDescription";
         this.lblDescription.Size = new System.Drawing.Size(63, 13);
         this.lblDescription.StyleController = this.layoutControl1;
         this.lblDescription.TabIndex = 4;
         this.lblDescription.Text = "lblDescription";
         // 
         // layoutControl
         // 
         this.layoutControl.CustomizationFormText = "layoutControl";
         this.layoutControl.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControl.GroupBordersVisible = false;
         this.layoutControl.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutItemDeselectAll,
            this.splitterItem1,
            this.layoutControlItem3,
            this.layoutControlItem6});
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "Root";
         this.layoutControl.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControl.Size = new System.Drawing.Size(513, 512);
         this.layoutControl.Text = "Root";
         this.layoutControl.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.lblDescription;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(513, 17);
         this.layoutControlItem1.Text = "layoutControlItem1";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextToControlDistance = 0;
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.txtInfo;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 17);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(256, 26);
         this.layoutControlItem2.Text = "layoutControlItem2";
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextToControlDistance = 0;
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutItemDeselectAll
         // 
         this.layoutItemDeselectAll.Control = this.btnDeselectAll;
         this.layoutItemDeselectAll.CustomizationFormText = "layoutControlItem3";
         this.layoutItemDeselectAll.Location = new System.Drawing.Point(256, 17);
         this.layoutItemDeselectAll.Name = "layoutItemDeselectAll";
         this.layoutItemDeselectAll.Size = new System.Drawing.Size(257, 26);
         this.layoutItemDeselectAll.Text = "layoutItemDeselectAll";
         this.layoutItemDeselectAll.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemDeselectAll.TextToControlDistance = 0;
         this.layoutItemDeselectAll.TextVisible = false;
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.CustomizationFormText = "splitterItem1";
         this.splitterItem1.Location = new System.Drawing.Point(0, 301);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(513, 5);
         // 
         // panelQuantities
         // 
         this.panelQuantities.Location = new System.Drawing.Point(2, 45);
         this.panelQuantities.Name = "panelQuantities";
         this.panelQuantities.Size = new System.Drawing.Size(509, 254);
         this.panelQuantities.TabIndex = 10;
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.panelQuantities;
         this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 43);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(513, 258);
         this.layoutControlItem3.Text = "layoutControlItem3";
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextToControlDistance = 0;
         this.layoutControlItem3.TextVisible = false;
         // 
         // panelSelectedQuantities
         // 
         this.panelSelectedQuantities.Location = new System.Drawing.Point(2, 308);
         this.panelSelectedQuantities.Name = "panelSelectedQuantities";
         this.panelSelectedQuantities.Size = new System.Drawing.Size(509, 202);
         this.panelSelectedQuantities.TabIndex = 11;
         // 
         // layoutControlItem6
         // 
         this.layoutControlItem6.Control = this.panelSelectedQuantities;
         this.layoutControlItem6.CustomizationFormText = "layoutControlItem6";
         this.layoutControlItem6.Location = new System.Drawing.Point(0, 306);
         this.layoutControlItem6.Name = "layoutControlItem6";
         this.layoutControlItem6.Size = new System.Drawing.Size(513, 206);
         this.layoutControlItem6.Text = "layoutControlItem6";
         this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem6.TextToControlDistance = 0;
         this.layoutControlItem6.TextVisible = false;
         // 
         // QuantitySelectionView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "QuantitySelectionView";
         this.Size = new System.Drawing.Size(513, 512);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.txtInfo.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDeselectAll)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelQuantities)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelSelectedQuantities)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControl;
      private DevExpress.XtraEditors.LabelControl lblDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraEditors.TextEdit txtInfo;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraEditors.SimpleButton btnDeselectAll;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDeselectAll;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraEditors.PanelControl panelSelectedQuantities;
      private DevExpress.XtraEditors.PanelControl panelQuantities;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
   }
}
