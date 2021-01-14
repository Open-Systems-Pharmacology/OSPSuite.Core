using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.UI.Views.Importer
{
   public class ModalImporterView : BaseModalView, IModalImporterView
   {
      private DevExpress.XtraLayout.LayoutControl importerLayoutControl;
      private DevExpress.XtraEditors.PanelControl importerPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlGroup Root;

      public override void InitializeResources()
      {
         base.InitializeResources();
         InitializeComponent();
         layoutControlBase.Visible = false;
         Text = Captions.Importer.Title;
      }

      public void FillImporterPanel(IView view)
      {
         importerPanelControl.FillWith(view);
      }

      public void AttachPresenter(IModalImporterPresenter presenter)
      {
      }

      public void AttachImporterPresenter(IImporterPresenter presenter)
      {
         presenter.OnTriggerImport += (s, d) =>
         {
            DialogResult = System.Windows.Forms.DialogResult.OK;
         };
      }

      private void InitializeComponent()
      {
         this.importerLayoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.importerPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).BeginInit();
         this.layoutControlBase.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerLayoutControl)).BeginInit();
         this.importerLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.importerPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(948, 14);
         this.btnCancel.Size = new System.Drawing.Size(200, 27);
         // 
         // btnOk
         // 
         this.btnOk.Location = new System.Drawing.Point(707, 14);
         this.btnOk.Size = new System.Drawing.Size(237, 27);
         // 
         // layoutControlBase
         // 
         this.layoutControlBase.Location = new System.Drawing.Point(0, 699);
         this.layoutControlBase.Size = new System.Drawing.Size(1161, 57);
         this.layoutControlBase.Controls.SetChildIndex(this.btnCancel, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnOk, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnExtra, 0);
         // 
         // btnExtra
         // 
         this.btnExtra.Size = new System.Drawing.Size(342, 27);
         // 
         // layoutControlGroupBase
         // 
         this.layoutControlGroupBase.Size = new System.Drawing.Size(1161, 57);
         // 
         // layoutItemOK
         // 
         this.layoutItemOK.Location = new System.Drawing.Point(694, 0);
         this.layoutItemOK.Size = new System.Drawing.Size(241, 33);
         // 
         // layoutItemCancel
         // 
         this.layoutItemCancel.Location = new System.Drawing.Point(935, 0);
         this.layoutItemCancel.Size = new System.Drawing.Size(204, 33);
         // 
         // emptySpaceItemBase
         // 
         this.emptySpaceItemBase.Location = new System.Drawing.Point(346, 0);
         this.emptySpaceItemBase.Size = new System.Drawing.Size(348, 33);
         // 
         // layoutItemExtra
         // 
         this.layoutItemExtra.Size = new System.Drawing.Size(346, 33);
         // 
         // importerLayoutControl
         // 
         this.importerLayoutControl.Controls.Add(this.importerPanelControl);
         this.importerLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.importerLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.importerLayoutControl.Name = "importerLayoutControl";
         this.importerLayoutControl.Root = this.Root;
         this.importerLayoutControl.Size = new System.Drawing.Size(1161, 699);
         this.importerLayoutControl.TabIndex = 38;
         this.importerLayoutControl.Text = "layoutControl1";
         // 
         // importerPanelControl
         // 
         this.importerPanelControl.Location = new System.Drawing.Point(12, 12);
         this.importerPanelControl.Name = "importerPanelControl";
         this.importerPanelControl.Size = new System.Drawing.Size(1137, 675);
         this.importerPanelControl.TabIndex = 4;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1161, 699);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.importerPanelControl;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(1141, 679);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // ModalImporterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.ClientSize = new System.Drawing.Size(1161, 756);
         this.Controls.Add(this.importerLayoutControl);
         this.Name = "ModalImporterView";
         this.Controls.SetChildIndex(this.layoutControlBase, 0);
         this.Controls.SetChildIndex(this.importerLayoutControl, 0);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).EndInit();
         this.layoutControlBase.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerLayoutControl)).EndInit();
         this.importerLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.importerPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }
   }
}
