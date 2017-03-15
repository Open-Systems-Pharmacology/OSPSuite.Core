using DevExpress.XtraRichEdit;

namespace OSPSuite.Starter.Views
{
   partial class JournalRichEditFormView
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
         this.uxRichEditControl = new RichEditControl();
         this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
         this.undoItem1 = new DevExpress.XtraRichEdit.UI.UndoItem();
         this.redoItem1 = new DevExpress.XtraRichEdit.UI.RedoItem();
         this.fileNewItem1 = new DevExpress.XtraRichEdit.UI.FileNewItem();
         this.fileOpenItem1 = new DevExpress.XtraRichEdit.UI.FileOpenItem();
         this.fileSaveItem1 = new DevExpress.XtraRichEdit.UI.FileSaveItem();
         this.fileSaveAsItem1 = new DevExpress.XtraRichEdit.UI.FileSaveAsItem();
         this.quickPrintItem1 = new DevExpress.XtraRichEdit.UI.QuickPrintItem();
         this.printItem1 = new DevExpress.XtraRichEdit.UI.PrintItem();
         this.printPreviewItem1 = new DevExpress.XtraRichEdit.UI.PrintPreviewItem();
         this.fileRibbonPage1 = new DevExpress.XtraRichEdit.UI.FileRibbonPage();
         this.commonRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.CommonRibbonPageGroup();
         this.richEditBarController1 = new DevExpress.XtraRichEdit.UI.RichEditBarController();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).BeginInit();
         this.layoutControlBase.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.richEditBarController1)).BeginInit();
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(894, 12);
         this.btnCancel.Size = new System.Drawing.Size(190, 22);
         // 
         // btnOk
         // 
         this.btnOk.Location = new System.Drawing.Point(668, 12);
         this.btnOk.Size = new System.Drawing.Size(222, 22);
         // 
         // layoutControlBase
         // 
         this.layoutControlBase.Location = new System.Drawing.Point(0, 666);
         this.layoutControlBase.Size = new System.Drawing.Size(1096, 46);
         this.layoutControlBase.Controls.SetChildIndex(this.btnCancel, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnOk, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnExtra, 0);
         // 
         // btnExtra
         // 
         this.btnExtra.Size = new System.Drawing.Size(324, 22);
         // 
         // layoutControlGroupBase
         // 
         this.layoutControlGroupBase.Size = new System.Drawing.Size(1096, 46);
         // 
         // layoutItemOK
         // 
         this.layoutItemOK.Location = new System.Drawing.Point(656, 0);
         this.layoutItemOK.Size = new System.Drawing.Size(226, 26);
         // 
         // layoutItemCancel
         // 
         this.layoutItemCancel.Location = new System.Drawing.Point(882, 0);
         this.layoutItemCancel.Size = new System.Drawing.Size(194, 26);
         // 
         // emptySpaceItemBase
         // 
         this.emptySpaceItemBase.Location = new System.Drawing.Point(328, 0);
         this.emptySpaceItemBase.Size = new System.Drawing.Size(328, 26);
         // 
         // layoutItemExtra
         // 
         this.layoutItemExtra.Size = new System.Drawing.Size(328, 26);
         // 
         // uxRichEditControl
         // 
         this.uxRichEditControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxRichEditControl.EnableToolTips = true;
         this.uxRichEditControl.Location = new System.Drawing.Point(0, 141);
         this.uxRichEditControl.MenuManager = this.ribbonControl1;
         this.uxRichEditControl.Name = "uxRichEditControl";
         this.uxRichEditControl.Options.Fields.UpdateFieldsInTextBoxes = false;
         this.uxRichEditControl.Size = new System.Drawing.Size(1096, 525);
         this.uxRichEditControl.TabIndex = 38;
         this.uxRichEditControl.Text = "uxRichEditControl1";
         // 
         // ribbonControl1
         // 
         this.ribbonControl1.ExpandCollapseItem.Id = 0;
         this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.undoItem1,
            this.redoItem1,
            this.fileNewItem1,
            this.fileOpenItem1,
            this.fileSaveItem1,
            this.fileSaveAsItem1,
            this.quickPrintItem1,
            this.printItem1,
            this.printPreviewItem1});
         this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
         this.ribbonControl1.MaxItemId = 10;
         this.ribbonControl1.Name = "ribbonControl1";
         this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.fileRibbonPage1});
         this.ribbonControl1.Size = new System.Drawing.Size(1096, 141);
         // 
         // undoItem1
         // 
         this.undoItem1.Id = 1;
         this.undoItem1.Name = "undoItem1";
         // 
         // redoItem1
         // 
         this.redoItem1.Id = 2;
         this.redoItem1.Name = "redoItem1";
         // 
         // fileNewItem1
         // 
         this.fileNewItem1.Id = 3;
         this.fileNewItem1.Name = "fileNewItem1";
         // 
         // fileOpenItem1
         // 
         this.fileOpenItem1.Id = 4;
         this.fileOpenItem1.Name = "fileOpenItem1";
         // 
         // fileSaveItem1
         // 
         this.fileSaveItem1.Id = 5;
         this.fileSaveItem1.Name = "fileSaveItem1";
         // 
         // fileSaveAsItem1
         // 
         this.fileSaveAsItem1.Id = 6;
         this.fileSaveAsItem1.Name = "fileSaveAsItem1";
         // 
         // quickPrintItem1
         // 
         this.quickPrintItem1.Id = 7;
         this.quickPrintItem1.Name = "quickPrintItem1";
         // 
         // printItem1
         // 
         this.printItem1.Id = 8;
         this.printItem1.Name = "printItem1";
         // 
         // printPreviewItem1
         // 
         this.printPreviewItem1.Id = 9;
         this.printPreviewItem1.Name = "printPreviewItem1";
         // 
         // fileRibbonPage1
         // 
         this.fileRibbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.commonRibbonPageGroup1});
         this.fileRibbonPage1.Name = "fileRibbonPage1";
         // 
         // commonRibbonPageGroup1
         // 
         this.commonRibbonPageGroup1.ItemLinks.Add(this.undoItem1);
         this.commonRibbonPageGroup1.ItemLinks.Add(this.redoItem1);
         this.commonRibbonPageGroup1.ItemLinks.Add(this.fileNewItem1);
         this.commonRibbonPageGroup1.ItemLinks.Add(this.fileOpenItem1);
         this.commonRibbonPageGroup1.ItemLinks.Add(this.fileSaveItem1);
         this.commonRibbonPageGroup1.ItemLinks.Add(this.fileSaveAsItem1);
         this.commonRibbonPageGroup1.ItemLinks.Add(this.quickPrintItem1);
         this.commonRibbonPageGroup1.ItemLinks.Add(this.printItem1);
         this.commonRibbonPageGroup1.ItemLinks.Add(this.printPreviewItem1);
         this.commonRibbonPageGroup1.Name = "commonRibbonPageGroup1";
         // 
         // richEditBarController1
         // 
         this.richEditBarController1.BarItems.Add(this.undoItem1);
         this.richEditBarController1.BarItems.Add(this.redoItem1);
         this.richEditBarController1.BarItems.Add(this.fileNewItem1);
         this.richEditBarController1.BarItems.Add(this.fileOpenItem1);
         this.richEditBarController1.BarItems.Add(this.fileSaveItem1);
         this.richEditBarController1.BarItems.Add(this.fileSaveAsItem1);
         this.richEditBarController1.BarItems.Add(this.quickPrintItem1);
         this.richEditBarController1.BarItems.Add(this.printItem1);
         this.richEditBarController1.BarItems.Add(this.printPreviewItem1);
         this.richEditBarController1.Control = this.uxRichEditControl;
         // 
         // JournalRichEditFormView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1096, 712);
         this.Controls.Add(this.uxRichEditControl);
         this.Controls.Add(this.ribbonControl1);
         this.Name = "JournalRichEditFormView";
         this.Controls.SetChildIndex(this.ribbonControl1, 0);
         this.Controls.SetChildIndex(this.layoutControlBase, 0);
         this.Controls.SetChildIndex(this.uxRichEditControl, 0);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).EndInit();
         this.layoutControlBase.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.richEditBarController1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private RichEditControl uxRichEditControl;
      private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
      private DevExpress.XtraRichEdit.UI.UndoItem undoItem1;
      private DevExpress.XtraRichEdit.UI.RedoItem redoItem1;
      private DevExpress.XtraRichEdit.UI.FileNewItem fileNewItem1;
      private DevExpress.XtraRichEdit.UI.FileOpenItem fileOpenItem1;
      private DevExpress.XtraRichEdit.UI.FileSaveItem fileSaveItem1;
      private DevExpress.XtraRichEdit.UI.FileSaveAsItem fileSaveAsItem1;
      private DevExpress.XtraRichEdit.UI.QuickPrintItem quickPrintItem1;
      private DevExpress.XtraRichEdit.UI.PrintItem printItem1;
      private DevExpress.XtraRichEdit.UI.PrintPreviewItem printPreviewItem1;
      private DevExpress.XtraRichEdit.UI.FileRibbonPage fileRibbonPage1;
      private DevExpress.XtraRichEdit.UI.CommonRibbonPageGroup commonRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.RichEditBarController richEditBarController1;
   }
}
