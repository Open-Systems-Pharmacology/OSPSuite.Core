using DevExpress.XtraEditors;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Journal
{
   partial class JournalPageEditorView
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
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JournalPageEditorView));
         DevExpress.XtraBars.Ribbon.GalleryItemGroup galleryItemGroup1 = new DevExpress.XtraBars.Ribbon.GalleryItemGroup();
         DevExpress.XtraBars.Ribbon.GalleryItemGroup galleryItemGroup2 = new DevExpress.XtraBars.Ribbon.GalleryItemGroup();
         DevExpress.XtraBars.Ribbon.ReduceOperation reduceOperation1 = new DevExpress.XtraBars.Ribbon.ReduceOperation();
         this.stylesRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.StylesRibbonPageGroup();
         this.galleryChangeStyleItem1 = new DevExpress.XtraRichEdit.UI.GalleryChangeStyleItem();
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.tbTitle = new DevExpress.XtraEditors.TextEdit();
         this.ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
         this.uxRichEditControl = new OSPSuite.UI.Controls.UxRichEditControl();
         this.undoItem1 = new DevExpress.XtraRichEdit.UI.UndoItem();
         this.redoItem1 = new DevExpress.XtraRichEdit.UI.RedoItem();
         this.fileNewItem1 = new DevExpress.XtraRichEdit.UI.FileNewItem();
         this.fileOpenItem1 = new DevExpress.XtraRichEdit.UI.FileOpenItem();
         this.fileSaveItem1 = new DevExpress.XtraRichEdit.UI.FileSaveItem();
         this.fileSaveAsItem1 = new DevExpress.XtraRichEdit.UI.FileSaveAsItem();
         this.quickPrintItem1 = new DevExpress.XtraRichEdit.UI.QuickPrintItem();
         this.printItem1 = new DevExpress.XtraRichEdit.UI.PrintItem();
         this.printPreviewItem1 = new DevExpress.XtraRichEdit.UI.PrintPreviewItem();
         this.insertPageBreakItem21 = new DevExpress.XtraRichEdit.UI.InsertPageBreakItem2();
         this.insertTableItem1 = new DevExpress.XtraRichEdit.UI.InsertTableItem();
         this.insertPictureItem1 = new DevExpress.XtraRichEdit.UI.InsertPictureItem();
         this.insertFloatingPictureItem1 = new DevExpress.XtraRichEdit.UI.InsertFloatingPictureItem();
         this.insertBookmarkItem1 = new DevExpress.XtraRichEdit.UI.InsertBookmarkItem();
         this.insertHyperlinkItem1 = new DevExpress.XtraRichEdit.UI.InsertHyperlinkItem();
         this.editPageHeaderItem1 = new DevExpress.XtraRichEdit.UI.EditPageHeaderItem();
         this.editPageFooterItem1 = new DevExpress.XtraRichEdit.UI.EditPageFooterItem();
         this.insertPageNumberItem1 = new DevExpress.XtraRichEdit.UI.InsertPageNumberItem();
         this.insertPageCountItem1 = new DevExpress.XtraRichEdit.UI.InsertPageCountItem();
         this.insertTextBoxItem1 = new DevExpress.XtraRichEdit.UI.InsertTextBoxItem();
         this.insertSymbolItem1 = new DevExpress.XtraRichEdit.UI.InsertSymbolItem();
         this.toggleFirstRowItem1 = new DevExpress.XtraRichEdit.UI.ToggleFirstRowItem();
         this.toggleLastRowItem1 = new DevExpress.XtraRichEdit.UI.ToggleLastRowItem();
         this.toggleBandedRowsItem1 = new DevExpress.XtraRichEdit.UI.ToggleBandedRowsItem();
         this.toggleFirstColumnItem1 = new DevExpress.XtraRichEdit.UI.ToggleFirstColumnItem();
         this.toggleLastColumnItem1 = new DevExpress.XtraRichEdit.UI.ToggleLastColumnItem();
         this.toggleBandedColumnsItem1 = new DevExpress.XtraRichEdit.UI.ToggleBandedColumnsItem();
         this.galleryChangeTableStyleItem1 = new DevExpress.XtraRichEdit.UI.GalleryChangeTableStyleItem();
         this.changeTableBorderLineStyleItem1 = new DevExpress.XtraRichEdit.UI.ChangeTableBorderLineStyleItem();
         this.repositoryItemBorderLineStyle1 = new DevExpress.XtraRichEdit.Forms.Design.RepositoryItemBorderLineStyle();
         this.changeTableBorderLineWeightItem1 = new DevExpress.XtraRichEdit.UI.ChangeTableBorderLineWeightItem();
         this.repositoryItemBorderLineWeight1 = new DevExpress.XtraRichEdit.Forms.Design.RepositoryItemBorderLineWeight();
         this.changeTableBorderColorItem1 = new DevExpress.XtraRichEdit.UI.ChangeTableBorderColorItem();
         this.changeTableBordersItem1 = new DevExpress.XtraRichEdit.UI.ChangeTableBordersItem();
         this.toggleTableCellsBottomBorderItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsBottomBorderItem();
         this.toggleTableCellsTopBorderItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsTopBorderItem();
         this.toggleTableCellsLeftBorderItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsLeftBorderItem();
         this.toggleTableCellsRightBorderItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsRightBorderItem();
         this.resetTableCellsAllBordersItem1 = new DevExpress.XtraRichEdit.UI.ResetTableCellsAllBordersItem();
         this.toggleTableCellsAllBordersItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsAllBordersItem();
         this.toggleTableCellsOutsideBorderItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsOutsideBorderItem();
         this.toggleTableCellsInsideBorderItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsInsideBorderItem();
         this.toggleTableCellsInsideHorizontalBorderItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsInsideHorizontalBorderItem();
         this.toggleTableCellsInsideVerticalBorderItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsInsideVerticalBorderItem();
         this.toggleShowTableGridLinesItem1 = new DevExpress.XtraRichEdit.UI.ToggleShowTableGridLinesItem();
         this.changeTableCellsShadingItem1 = new DevExpress.XtraRichEdit.UI.ChangeTableCellsShadingItem();
         this.selectTableElementsItem1 = new DevExpress.XtraRichEdit.UI.SelectTableElementsItem();
         this.selectTableCellItem1 = new DevExpress.XtraRichEdit.UI.SelectTableCellItem();
         this.selectTableColumnItem1 = new DevExpress.XtraRichEdit.UI.SelectTableColumnItem();
         this.selectTableRowItem1 = new DevExpress.XtraRichEdit.UI.SelectTableRowItem();
         this.selectTableItem1 = new DevExpress.XtraRichEdit.UI.SelectTableItem();
         this.showTablePropertiesFormItem1 = new DevExpress.XtraRichEdit.UI.ShowTablePropertiesFormItem();
         this.deleteTableElementsItem1 = new DevExpress.XtraRichEdit.UI.DeleteTableElementsItem();
         this.showDeleteTableCellsFormItem1 = new DevExpress.XtraRichEdit.UI.ShowDeleteTableCellsFormItem();
         this.deleteTableColumnsItem1 = new DevExpress.XtraRichEdit.UI.DeleteTableColumnsItem();
         this.deleteTableRowsItem1 = new DevExpress.XtraRichEdit.UI.DeleteTableRowsItem();
         this.deleteTableItem1 = new DevExpress.XtraRichEdit.UI.DeleteTableItem();
         this.insertTableRowAboveItem1 = new DevExpress.XtraRichEdit.UI.InsertTableRowAboveItem();
         this.insertTableRowBelowItem1 = new DevExpress.XtraRichEdit.UI.InsertTableRowBelowItem();
         this.insertTableColumnToLeftItem1 = new DevExpress.XtraRichEdit.UI.InsertTableColumnToLeftItem();
         this.insertTableColumnToRightItem1 = new DevExpress.XtraRichEdit.UI.InsertTableColumnToRightItem();
         this.mergeTableCellsItem1 = new DevExpress.XtraRichEdit.UI.MergeTableCellsItem();
         this.showSplitTableCellsForm1 = new DevExpress.XtraRichEdit.UI.ShowSplitTableCellsForm();
         this.splitTableItem1 = new DevExpress.XtraRichEdit.UI.SplitTableItem();
         this.toggleTableAutoFitItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableAutoFitItem();
         this.toggleTableAutoFitContentsItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableAutoFitContentsItem();
         this.toggleTableAutoFitWindowItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableAutoFitWindowItem();
         this.toggleTableFixedColumnWidthItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableFixedColumnWidthItem();
         this.toggleTableCellsTopLeftAlignmentItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsTopLeftAlignmentItem();
         this.toggleTableCellsMiddleLeftAlignmentItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsMiddleLeftAlignmentItem();
         this.toggleTableCellsBottomLeftAlignmentItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsBottomLeftAlignmentItem();
         this.toggleTableCellsTopCenterAlignmentItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsTopCenterAlignmentItem();
         this.toggleTableCellsMiddleCenterAlignmentItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsMiddleCenterAlignmentItem();
         this.toggleTableCellsBottomCenterAlignmentItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsBottomCenterAlignmentItem();
         this.toggleTableCellsTopRightAlignmentItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsTopRightAlignmentItem();
         this.toggleTableCellsMiddleRightAlignmentItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsMiddleRightAlignmentItem();
         this.toggleTableCellsBottomRightAlignmentItem1 = new DevExpress.XtraRichEdit.UI.ToggleTableCellsBottomRightAlignmentItem();
         this.showTableOptionsFormItem1 = new DevExpress.XtraRichEdit.UI.ShowTableOptionsFormItem();
         this.pasteItem1 = new DevExpress.XtraRichEdit.UI.PasteItem();
         this.cutItem1 = new DevExpress.XtraRichEdit.UI.CutItem();
         this.copyItem1 = new DevExpress.XtraRichEdit.UI.CopyItem();
         this.pasteSpecialItem1 = new DevExpress.XtraRichEdit.UI.PasteSpecialItem();
         this.barButtonGroup1 = new DevExpress.XtraBars.BarButtonGroup();
         this.changeFontNameItem1 = new DevExpress.XtraRichEdit.UI.ChangeFontNameItem();
         this.repositoryItemFontEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemFontEdit();
         this.changeFontSizeItem1 = new DevExpress.XtraRichEdit.UI.ChangeFontSizeItem();
         this.repositoryItemRichEditFontSizeEdit1 = new DevExpress.XtraRichEdit.Design.RepositoryItemRichEditFontSizeEdit();
         this.fontSizeIncreaseItem1 = new DevExpress.XtraRichEdit.UI.FontSizeIncreaseItem();
         this.fontSizeDecreaseItem1 = new DevExpress.XtraRichEdit.UI.FontSizeDecreaseItem();
         this.barButtonGroup2 = new DevExpress.XtraBars.BarButtonGroup();
         this.toggleFontBoldItem1 = new DevExpress.XtraRichEdit.UI.ToggleFontBoldItem();
         this.toggleFontItalicItem1 = new DevExpress.XtraRichEdit.UI.ToggleFontItalicItem();
         this.toggleFontUnderlineItem1 = new DevExpress.XtraRichEdit.UI.ToggleFontUnderlineItem();
         this.toggleFontDoubleUnderlineItem1 = new DevExpress.XtraRichEdit.UI.ToggleFontDoubleUnderlineItem();
         this.toggleFontStrikeoutItem1 = new DevExpress.XtraRichEdit.UI.ToggleFontStrikeoutItem();
         this.toggleFontDoubleStrikeoutItem1 = new DevExpress.XtraRichEdit.UI.ToggleFontDoubleStrikeoutItem();
         this.toggleFontSuperscriptItem1 = new DevExpress.XtraRichEdit.UI.ToggleFontSuperscriptItem();
         this.toggleFontSubscriptItem1 = new DevExpress.XtraRichEdit.UI.ToggleFontSubscriptItem();
         this.barButtonGroup3 = new DevExpress.XtraBars.BarButtonGroup();
         this.changeFontColorItem1 = new DevExpress.XtraRichEdit.UI.ChangeFontColorItem();
         this.changeFontBackColorItem1 = new DevExpress.XtraRichEdit.UI.ChangeFontBackColorItem();
         this.changeTextCaseItem1 = new DevExpress.XtraRichEdit.UI.ChangeTextCaseItem();
         this.makeTextUpperCaseItem1 = new DevExpress.XtraRichEdit.UI.MakeTextUpperCaseItem();
         this.makeTextLowerCaseItem1 = new DevExpress.XtraRichEdit.UI.MakeTextLowerCaseItem();
         this.capitalizeEachWordCaseItem1 = new DevExpress.XtraRichEdit.UI.CapitalizeEachWordCaseItem();
         this.toggleTextCaseItem1 = new DevExpress.XtraRichEdit.UI.ToggleTextCaseItem();
         this.clearFormattingItem1 = new DevExpress.XtraRichEdit.UI.ClearFormattingItem();
         this.barButtonGroup4 = new DevExpress.XtraBars.BarButtonGroup();
         this.toggleBulletedListItem1 = new DevExpress.XtraRichEdit.UI.ToggleBulletedListItem();
         this.toggleNumberingListItem1 = new DevExpress.XtraRichEdit.UI.ToggleNumberingListItem();
         this.toggleMultiLevelListItem1 = new DevExpress.XtraRichEdit.UI.ToggleMultiLevelListItem();
         this.barButtonGroup5 = new DevExpress.XtraBars.BarButtonGroup();
         this.decreaseIndentItem1 = new DevExpress.XtraRichEdit.UI.DecreaseIndentItem();
         this.increaseIndentItem1 = new DevExpress.XtraRichEdit.UI.IncreaseIndentItem();
         this.toggleShowWhitespaceItem1 = new DevExpress.XtraRichEdit.UI.ToggleShowWhitespaceItem();
         this.barButtonGroup6 = new DevExpress.XtraBars.BarButtonGroup();
         this.toggleParagraphAlignmentLeftItem1 = new DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentLeftItem();
         this.toggleParagraphAlignmentCenterItem1 = new DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentCenterItem();
         this.toggleParagraphAlignmentRightItem1 = new DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentRightItem();
         this.toggleParagraphAlignmentJustifyItem1 = new DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentJustifyItem();
         this.barButtonGroup7 = new DevExpress.XtraBars.BarButtonGroup();
         this.changeParagraphLineSpacingItem1 = new DevExpress.XtraRichEdit.UI.ChangeParagraphLineSpacingItem();
         this.setSingleParagraphSpacingItem1 = new DevExpress.XtraRichEdit.UI.SetSingleParagraphSpacingItem();
         this.setSesquialteralParagraphSpacingItem1 = new DevExpress.XtraRichEdit.UI.SetSesquialteralParagraphSpacingItem();
         this.setDoubleParagraphSpacingItem1 = new DevExpress.XtraRichEdit.UI.SetDoubleParagraphSpacingItem();
         this.showLineSpacingFormItem1 = new DevExpress.XtraRichEdit.UI.ShowLineSpacingFormItem();
         this.addSpacingBeforeParagraphItem1 = new DevExpress.XtraRichEdit.UI.AddSpacingBeforeParagraphItem();
         this.removeSpacingBeforeParagraphItem1 = new DevExpress.XtraRichEdit.UI.RemoveSpacingBeforeParagraphItem();
         this.addSpacingAfterParagraphItem1 = new DevExpress.XtraRichEdit.UI.AddSpacingAfterParagraphItem();
         this.removeSpacingAfterParagraphItem1 = new DevExpress.XtraRichEdit.UI.RemoveSpacingAfterParagraphItem();
         this.changeParagraphBackColorItem1 = new DevExpress.XtraRichEdit.UI.ChangeParagraphBackColorItem();
         this.findItem1 = new DevExpress.XtraRichEdit.UI.FindItem();
         this.replaceItem1 = new DevExpress.XtraRichEdit.UI.ReplaceItem();
         this.tableToolsRibbonPageCategory1 = new DevExpress.XtraRichEdit.UI.TableToolsRibbonPageCategory();
         this.tableDesignRibbonPage1 = new DevExpress.XtraRichEdit.UI.TableDesignRibbonPage();
         this.tableStyleOptionsRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.TableStyleOptionsRibbonPageGroup();
         this.tableStylesRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.TableStylesRibbonPageGroup();
         this.tableDrawBordersRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.TableDrawBordersRibbonPageGroup();
         this.tableLayoutRibbonPage1 = new DevExpress.XtraRichEdit.UI.TableLayoutRibbonPage();
         this.tableTableRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.TableTableRibbonPageGroup();
         this.tableRowsAndColumnsRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.TableRowsAndColumnsRibbonPageGroup();
         this.tableMergeRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.TableMergeRibbonPageGroup();
         this.tableCellSizeRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.TableCellSizeRibbonPageGroup();
         this.tableAlignmentRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.TableAlignmentRibbonPageGroup();
         this.homeRibbonPage1 = new DevExpress.XtraRichEdit.UI.HomeRibbonPage();
         this.clipboardRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.ClipboardRibbonPageGroup();
         this.fontRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.FontRibbonPageGroup();
         this.paragraphRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.ParagraphRibbonPageGroup();
         this.editingRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.EditingRibbonPageGroup();
         this.fileRibbonPage1 = new DevExpress.XtraRichEdit.UI.FileRibbonPage();
         this.commonRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.CommonRibbonPageGroup();
         this.insertRibbonPage1 = new DevExpress.XtraRichEdit.UI.InsertRibbonPage();
         this.pagesRibbonPageGroup = new DevExpress.XtraRichEdit.UI.PagesRibbonPageGroup();
         this.tablesRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.TablesRibbonPageGroup();
         this.illustrationsRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.IllustrationsRibbonPageGroup();
         this.linksRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.LinksRibbonPageGroup();
         this.headerFooterRibbonPageGroup = new DevExpress.XtraRichEdit.UI.HeaderFooterRibbonPageGroup();
         this.textRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.TextRibbonPageGroup();
         this.symbolsRibbonPageGroup1 = new DevExpress.XtraRichEdit.UI.SymbolsRibbonPageGroup();
         this.cbOrigin = new DevExpress.XtraEditors.ImageComboBoxEdit();
         this.tagEdit = new DevExpress.XtraEditors.TokenEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.richEditControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemTokenEdit = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemSource = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemTitle = new DevExpress.XtraLayout.LayoutControlItem();
         this.richEditBarController1 = new DevExpress.XtraRichEdit.UI.RichEditBarController();
         this.buttonNextPage = new DevExpress.XtraEditors.SimpleButton();
         this.layoutItemButtonNextPage = new DevExpress.XtraLayout.LayoutControlItem();
         this.buttonPreviousPage = new DevExpress.XtraEditors.SimpleButton();
         this.layoutItemButtonPreviousPage = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbTitle.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemBorderLineStyle1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemBorderLineWeight1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemFontEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichEditFontSizeEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbOrigin.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tagEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.richEditControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTokenEdit)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTitle)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.richEditBarController1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonNextPage)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonPreviousPage)).BeginInit();
         this.SuspendLayout();
         // 
         // stylesRibbonPageGroup1
         // 
         this.stylesRibbonPageGroup1.Glyph = ((System.Drawing.Image)(resources.GetObject("stylesRibbonPageGroup1.Glyph")));
         this.stylesRibbonPageGroup1.ItemLinks.Add(this.galleryChangeStyleItem1);
         this.stylesRibbonPageGroup1.Name = "stylesRibbonPageGroup1";
         // 
         // galleryChangeStyleItem1
         // 
         // 
         // 
         // 
         this.galleryChangeStyleItem1.Gallery.ColumnCount = 10;
         this.galleryChangeStyleItem1.Gallery.Groups.AddRange(new DevExpress.XtraBars.Ribbon.GalleryItemGroup[] {
            galleryItemGroup1});
         this.galleryChangeStyleItem1.Gallery.ImageSize = new System.Drawing.Size(65, 46);
         this.galleryChangeStyleItem1.Id = 128;
         this.galleryChangeStyleItem1.Name = "galleryChangeStyleItem1";
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.tbTitle);
         this.layoutControl1.Controls.Add(this.cbOrigin);
         this.layoutControl1.Controls.Add(this.tagEdit);
         this.layoutControl1.Controls.Add(this.uxRichEditControl);
         this.layoutControl1.Controls.Add(this.buttonNextPage);
         this.layoutControl1.Controls.Add(this.buttonPreviousPage);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 141);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1167, 385, 450, 400);
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(940, 403);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // tbTitle
         // 
         this.tbTitle.Location = new System.Drawing.Point(79, 2);
         this.tbTitle.MenuManager = this.ribbonControl;
         this.tbTitle.Name = "tbTitle";
         this.tbTitle.Size = new System.Drawing.Size(664, 20);
         this.tbTitle.StyleController = this.layoutControl1;
         this.tbTitle.TabIndex = 8;
         // 
         // ribbonControl
         // 
         this.ribbonControl.ApplicationButtonDropDownControl = this.uxRichEditControl;
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl.ExpandCollapseItem,
            this.undoItem1,
            this.redoItem1,
            this.fileNewItem1,
            this.fileOpenItem1,
            this.fileSaveItem1,
            this.fileSaveAsItem1,
            this.quickPrintItem1,
            this.printItem1,
            this.printPreviewItem1,
            this.insertPageBreakItem21,
            this.insertTableItem1,
            this.insertPictureItem1,
            this.insertFloatingPictureItem1,
            this.insertBookmarkItem1,
            this.insertHyperlinkItem1,
            this.editPageHeaderItem1,
            this.editPageFooterItem1,
            this.insertPageNumberItem1,
            this.insertPageCountItem1,
            this.insertTextBoxItem1,
            this.insertSymbolItem1,
            this.toggleFirstRowItem1,
            this.toggleLastRowItem1,
            this.toggleBandedRowsItem1,
            this.toggleFirstColumnItem1,
            this.toggleLastColumnItem1,
            this.toggleBandedColumnsItem1,
            this.galleryChangeTableStyleItem1,
            this.changeTableBorderLineStyleItem1,
            this.changeTableBorderLineWeightItem1,
            this.changeTableBorderColorItem1,
            this.changeTableBordersItem1,
            this.toggleTableCellsBottomBorderItem1,
            this.toggleTableCellsTopBorderItem1,
            this.toggleTableCellsLeftBorderItem1,
            this.toggleTableCellsRightBorderItem1,
            this.resetTableCellsAllBordersItem1,
            this.toggleTableCellsAllBordersItem1,
            this.toggleTableCellsOutsideBorderItem1,
            this.toggleTableCellsInsideBorderItem1,
            this.toggleTableCellsInsideHorizontalBorderItem1,
            this.toggleTableCellsInsideVerticalBorderItem1,
            this.toggleShowTableGridLinesItem1,
            this.changeTableCellsShadingItem1,
            this.selectTableElementsItem1,
            this.selectTableCellItem1,
            this.selectTableColumnItem1,
            this.selectTableRowItem1,
            this.selectTableItem1,
            this.showTablePropertiesFormItem1,
            this.deleteTableElementsItem1,
            this.showDeleteTableCellsFormItem1,
            this.deleteTableColumnsItem1,
            this.deleteTableRowsItem1,
            this.deleteTableItem1,
            this.insertTableRowAboveItem1,
            this.insertTableRowBelowItem1,
            this.insertTableColumnToLeftItem1,
            this.insertTableColumnToRightItem1,
            this.mergeTableCellsItem1,
            this.showSplitTableCellsForm1,
            this.splitTableItem1,
            this.toggleTableAutoFitItem1,
            this.toggleTableAutoFitContentsItem1,
            this.toggleTableAutoFitWindowItem1,
            this.toggleTableFixedColumnWidthItem1,
            this.toggleTableCellsTopLeftAlignmentItem1,
            this.toggleTableCellsMiddleLeftAlignmentItem1,
            this.toggleTableCellsBottomLeftAlignmentItem1,
            this.toggleTableCellsTopCenterAlignmentItem1,
            this.toggleTableCellsMiddleCenterAlignmentItem1,
            this.toggleTableCellsBottomCenterAlignmentItem1,
            this.toggleTableCellsTopRightAlignmentItem1,
            this.toggleTableCellsMiddleRightAlignmentItem1,
            this.toggleTableCellsBottomRightAlignmentItem1,
            this.showTableOptionsFormItem1,
            this.pasteItem1,
            this.cutItem1,
            this.copyItem1,
            this.pasteSpecialItem1,
            this.barButtonGroup1,
            this.changeFontNameItem1,
            this.changeFontSizeItem1,
            this.fontSizeIncreaseItem1,
            this.fontSizeDecreaseItem1,
            this.barButtonGroup2,
            this.toggleFontBoldItem1,
            this.toggleFontItalicItem1,
            this.toggleFontUnderlineItem1,
            this.toggleFontDoubleUnderlineItem1,
            this.toggleFontStrikeoutItem1,
            this.toggleFontDoubleStrikeoutItem1,
            this.toggleFontSuperscriptItem1,
            this.toggleFontSubscriptItem1,
            this.barButtonGroup3,
            this.changeFontColorItem1,
            this.changeFontBackColorItem1,
            this.changeTextCaseItem1,
            this.makeTextUpperCaseItem1,
            this.makeTextLowerCaseItem1,
            this.capitalizeEachWordCaseItem1,
            this.toggleTextCaseItem1,
            this.clearFormattingItem1,
            this.barButtonGroup4,
            this.toggleBulletedListItem1,
            this.toggleNumberingListItem1,
            this.toggleMultiLevelListItem1,
            this.barButtonGroup5,
            this.decreaseIndentItem1,
            this.increaseIndentItem1,
            this.barButtonGroup6,
            this.toggleParagraphAlignmentLeftItem1,
            this.toggleParagraphAlignmentCenterItem1,
            this.toggleParagraphAlignmentRightItem1,
            this.toggleParagraphAlignmentJustifyItem1,
            this.toggleShowWhitespaceItem1,
            this.barButtonGroup7,
            this.changeParagraphLineSpacingItem1,
            this.setSingleParagraphSpacingItem1,
            this.setSesquialteralParagraphSpacingItem1,
            this.setDoubleParagraphSpacingItem1,
            this.showLineSpacingFormItem1,
            this.addSpacingBeforeParagraphItem1,
            this.removeSpacingBeforeParagraphItem1,
            this.addSpacingAfterParagraphItem1,
            this.removeSpacingAfterParagraphItem1,
            this.changeParagraphBackColorItem1,
            this.galleryChangeStyleItem1,
            this.findItem1,
            this.replaceItem1});
         this.ribbonControl.Location = new System.Drawing.Point(0, 0);
         this.ribbonControl.MaxItemId = 1;
         this.ribbonControl.Name = "ribbonControl";
         this.ribbonControl.PageCategories.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageCategory[] {
            this.tableToolsRibbonPageCategory1});
         this.ribbonControl.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.homeRibbonPage1,
            this.fileRibbonPage1,
            this.insertRibbonPage1});
         this.ribbonControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemBorderLineStyle1,
            this.repositoryItemBorderLineWeight1,
            this.repositoryItemFontEdit1,
            this.repositoryItemRichEditFontSizeEdit1});
         this.ribbonControl.Size = new System.Drawing.Size(940, 141);
         this.ribbonControl.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Above;
         // 
         // uxRichEditControl
         // 
         this.uxRichEditControl.Location = new System.Drawing.Point(2, 26);
         this.uxRichEditControl.MenuManager = this.ribbonControl;
         this.uxRichEditControl.Name = "uxRichEditControl";
         this.uxRichEditControl.Size = new System.Drawing.Size(936, 349);
         this.uxRichEditControl.TabIndex = 4;
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
         // insertPageBreakItem21
         // 
         this.insertPageBreakItem21.Id = 10;
         this.insertPageBreakItem21.Name = "insertPageBreakItem21";
         // 
         // insertTableItem1
         // 
         this.insertTableItem1.Id = 11;
         this.insertTableItem1.Name = "insertTableItem1";
         // 
         // insertPictureItem1
         // 
         this.insertPictureItem1.Id = 12;
         this.insertPictureItem1.Name = "insertPictureItem1";
         // 
         // insertFloatingPictureItem1
         // 
         this.insertFloatingPictureItem1.Id = 13;
         this.insertFloatingPictureItem1.Name = "insertFloatingPictureItem1";
         // 
         // insertBookmarkItem1
         // 
         this.insertBookmarkItem1.Id = 14;
         this.insertBookmarkItem1.Name = "insertBookmarkItem1";
         // 
         // insertHyperlinkItem1
         // 
         this.insertHyperlinkItem1.Id = 15;
         this.insertHyperlinkItem1.Name = "insertHyperlinkItem1";
         // 
         // editPageHeaderItem1
         // 
         this.editPageHeaderItem1.Id = 16;
         this.editPageHeaderItem1.Name = "editPageHeaderItem1";
         // 
         // editPageFooterItem1
         // 
         this.editPageFooterItem1.Id = 17;
         this.editPageFooterItem1.Name = "editPageFooterItem1";
         // 
         // insertPageNumberItem1
         // 
         this.insertPageNumberItem1.Id = 18;
         this.insertPageNumberItem1.Name = "insertPageNumberItem1";
         // 
         // insertPageCountItem1
         // 
         this.insertPageCountItem1.Id = 19;
         this.insertPageCountItem1.Name = "insertPageCountItem1";
         // 
         // insertTextBoxItem1
         // 
         this.insertTextBoxItem1.Id = 20;
         this.insertTextBoxItem1.Name = "insertTextBoxItem1";
         // 
         // insertSymbolItem1
         // 
         this.insertSymbolItem1.Id = 21;
         this.insertSymbolItem1.Name = "insertSymbolItem1";
         // 
         // toggleFirstRowItem1
         // 
         this.toggleFirstRowItem1.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
         this.toggleFirstRowItem1.Id = 22;
         this.toggleFirstRowItem1.Name = "toggleFirstRowItem1";
         // 
         // toggleLastRowItem1
         // 
         this.toggleLastRowItem1.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
         this.toggleLastRowItem1.Id = 23;
         this.toggleLastRowItem1.Name = "toggleLastRowItem1";
         // 
         // toggleBandedRowsItem1
         // 
         this.toggleBandedRowsItem1.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
         this.toggleBandedRowsItem1.Id = 24;
         this.toggleBandedRowsItem1.Name = "toggleBandedRowsItem1";
         // 
         // toggleFirstColumnItem1
         // 
         this.toggleFirstColumnItem1.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
         this.toggleFirstColumnItem1.Id = 25;
         this.toggleFirstColumnItem1.Name = "toggleFirstColumnItem1";
         // 
         // toggleLastColumnItem1
         // 
         this.toggleLastColumnItem1.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
         this.toggleLastColumnItem1.Id = 26;
         this.toggleLastColumnItem1.Name = "toggleLastColumnItem1";
         // 
         // toggleBandedColumnsItem1
         // 
         this.toggleBandedColumnsItem1.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
         this.toggleBandedColumnsItem1.Id = 27;
         this.toggleBandedColumnsItem1.Name = "toggleBandedColumnsItem1";
         // 
         // galleryChangeTableStyleItem1
         // 
         this.galleryChangeTableStyleItem1.CurrentItem = null;
         this.galleryChangeTableStyleItem1.DeleteItemLink = null;
         // 
         // 
         // 
         this.galleryChangeTableStyleItem1.Gallery.ColumnCount = 3;
         this.galleryChangeTableStyleItem1.Gallery.Groups.AddRange(new DevExpress.XtraBars.Ribbon.GalleryItemGroup[] {
            galleryItemGroup2});
         this.galleryChangeTableStyleItem1.Gallery.ImageSize = new System.Drawing.Size(65, 46);
         this.galleryChangeTableStyleItem1.Id = 28;
         this.galleryChangeTableStyleItem1.ModifyItemLink = null;
         this.galleryChangeTableStyleItem1.Name = "galleryChangeTableStyleItem1";
         this.galleryChangeTableStyleItem1.NewItemLink = null;
         this.galleryChangeTableStyleItem1.PopupGallery = null;
         // 
         // changeTableBorderLineStyleItem1
         // 
         this.changeTableBorderLineStyleItem1.Edit = this.repositoryItemBorderLineStyle1;
         this.changeTableBorderLineStyleItem1.EditWidth = 130;
         this.changeTableBorderLineStyleItem1.Id = 29;
         this.changeTableBorderLineStyleItem1.Name = "changeTableBorderLineStyleItem1";
         // 
         // repositoryItemBorderLineStyle1
         // 
         this.repositoryItemBorderLineStyle1.AutoHeight = false;
         this.repositoryItemBorderLineStyle1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.repositoryItemBorderLineStyle1.Control = this.uxRichEditControl;
         this.repositoryItemBorderLineStyle1.Name = "repositoryItemBorderLineStyle1";
         // 
         // changeTableBorderLineWeightItem1
         // 
         this.changeTableBorderLineWeightItem1.Edit = this.repositoryItemBorderLineWeight1;
         this.changeTableBorderLineWeightItem1.EditValue = 20;
         this.changeTableBorderLineWeightItem1.EditWidth = 130;
         this.changeTableBorderLineWeightItem1.Id = 30;
         this.changeTableBorderLineWeightItem1.Name = "changeTableBorderLineWeightItem1";
         // 
         // repositoryItemBorderLineWeight1
         // 
         this.repositoryItemBorderLineWeight1.AutoHeight = false;
         this.repositoryItemBorderLineWeight1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.repositoryItemBorderLineWeight1.Control = this.uxRichEditControl;
         this.repositoryItemBorderLineWeight1.Name = "repositoryItemBorderLineWeight1";
         // 
         // changeTableBorderColorItem1
         // 
         this.changeTableBorderColorItem1.Id = 31;
         this.changeTableBorderColorItem1.Name = "changeTableBorderColorItem1";
         // 
         // changeTableBordersItem1
         // 
         this.changeTableBordersItem1.Id = 32;
         this.changeTableBordersItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleTableCellsBottomBorderItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleTableCellsTopBorderItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleTableCellsLeftBorderItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleTableCellsRightBorderItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.resetTableCellsAllBordersItem1, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleTableCellsAllBordersItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleTableCellsOutsideBorderItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleTableCellsInsideBorderItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleTableCellsInsideHorizontalBorderItem1, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleTableCellsInsideVerticalBorderItem1),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.KeyTip, this.toggleShowTableGridLinesItem1, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard, "TG", "")});
         this.changeTableBordersItem1.Name = "changeTableBordersItem1";
         // 
         // toggleTableCellsBottomBorderItem1
         // 
         this.toggleTableCellsBottomBorderItem1.Id = 33;
         this.toggleTableCellsBottomBorderItem1.Name = "toggleTableCellsBottomBorderItem1";
         // 
         // toggleTableCellsTopBorderItem1
         // 
         this.toggleTableCellsTopBorderItem1.Id = 34;
         this.toggleTableCellsTopBorderItem1.Name = "toggleTableCellsTopBorderItem1";
         // 
         // toggleTableCellsLeftBorderItem1
         // 
         this.toggleTableCellsLeftBorderItem1.Id = 35;
         this.toggleTableCellsLeftBorderItem1.Name = "toggleTableCellsLeftBorderItem1";
         // 
         // toggleTableCellsRightBorderItem1
         // 
         this.toggleTableCellsRightBorderItem1.Id = 36;
         this.toggleTableCellsRightBorderItem1.Name = "toggleTableCellsRightBorderItem1";
         // 
         // resetTableCellsAllBordersItem1
         // 
         this.resetTableCellsAllBordersItem1.Id = 37;
         this.resetTableCellsAllBordersItem1.Name = "resetTableCellsAllBordersItem1";
         // 
         // toggleTableCellsAllBordersItem1
         // 
         this.toggleTableCellsAllBordersItem1.Id = 38;
         this.toggleTableCellsAllBordersItem1.Name = "toggleTableCellsAllBordersItem1";
         // 
         // toggleTableCellsOutsideBorderItem1
         // 
         this.toggleTableCellsOutsideBorderItem1.Id = 39;
         this.toggleTableCellsOutsideBorderItem1.Name = "toggleTableCellsOutsideBorderItem1";
         // 
         // toggleTableCellsInsideBorderItem1
         // 
         this.toggleTableCellsInsideBorderItem1.Id = 40;
         this.toggleTableCellsInsideBorderItem1.Name = "toggleTableCellsInsideBorderItem1";
         // 
         // toggleTableCellsInsideHorizontalBorderItem1
         // 
         this.toggleTableCellsInsideHorizontalBorderItem1.Id = 41;
         this.toggleTableCellsInsideHorizontalBorderItem1.Name = "toggleTableCellsInsideHorizontalBorderItem1";
         // 
         // toggleTableCellsInsideVerticalBorderItem1
         // 
         this.toggleTableCellsInsideVerticalBorderItem1.Id = 42;
         this.toggleTableCellsInsideVerticalBorderItem1.Name = "toggleTableCellsInsideVerticalBorderItem1";
         // 
         // toggleShowTableGridLinesItem1
         // 
         this.toggleShowTableGridLinesItem1.Id = 43;
         this.toggleShowTableGridLinesItem1.Name = "toggleShowTableGridLinesItem1";
         // 
         // changeTableCellsShadingItem1
         // 
         this.changeTableCellsShadingItem1.Id = 44;
         this.changeTableCellsShadingItem1.Name = "changeTableCellsShadingItem1";
         // 
         // selectTableElementsItem1
         // 
         this.selectTableElementsItem1.Id = 45;
         this.selectTableElementsItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.selectTableCellItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.selectTableColumnItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.selectTableRowItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.selectTableItem1)});
         this.selectTableElementsItem1.Name = "selectTableElementsItem1";
         // 
         // selectTableCellItem1
         // 
         this.selectTableCellItem1.Id = 46;
         this.selectTableCellItem1.Name = "selectTableCellItem1";
         // 
         // selectTableColumnItem1
         // 
         this.selectTableColumnItem1.Id = 47;
         this.selectTableColumnItem1.Name = "selectTableColumnItem1";
         // 
         // selectTableRowItem1
         // 
         this.selectTableRowItem1.Id = 48;
         this.selectTableRowItem1.Name = "selectTableRowItem1";
         // 
         // selectTableItem1
         // 
         this.selectTableItem1.Id = 49;
         this.selectTableItem1.Name = "selectTableItem1";
         // 
         // showTablePropertiesFormItem1
         // 
         this.showTablePropertiesFormItem1.Id = 50;
         this.showTablePropertiesFormItem1.Name = "showTablePropertiesFormItem1";
         // 
         // deleteTableElementsItem1
         // 
         this.deleteTableElementsItem1.Id = 51;
         this.deleteTableElementsItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.showDeleteTableCellsFormItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.deleteTableColumnsItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.deleteTableRowsItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.deleteTableItem1)});
         this.deleteTableElementsItem1.Name = "deleteTableElementsItem1";
         // 
         // showDeleteTableCellsFormItem1
         // 
         this.showDeleteTableCellsFormItem1.Id = 52;
         this.showDeleteTableCellsFormItem1.Name = "showDeleteTableCellsFormItem1";
         // 
         // deleteTableColumnsItem1
         // 
         this.deleteTableColumnsItem1.Id = 53;
         this.deleteTableColumnsItem1.Name = "deleteTableColumnsItem1";
         // 
         // deleteTableRowsItem1
         // 
         this.deleteTableRowsItem1.Id = 54;
         this.deleteTableRowsItem1.Name = "deleteTableRowsItem1";
         // 
         // deleteTableItem1
         // 
         this.deleteTableItem1.Id = 55;
         this.deleteTableItem1.Name = "deleteTableItem1";
         // 
         // insertTableRowAboveItem1
         // 
         this.insertTableRowAboveItem1.Id = 56;
         this.insertTableRowAboveItem1.Name = "insertTableRowAboveItem1";
         // 
         // insertTableRowBelowItem1
         // 
         this.insertTableRowBelowItem1.Id = 57;
         this.insertTableRowBelowItem1.Name = "insertTableRowBelowItem1";
         // 
         // insertTableColumnToLeftItem1
         // 
         this.insertTableColumnToLeftItem1.Id = 58;
         this.insertTableColumnToLeftItem1.Name = "insertTableColumnToLeftItem1";
         // 
         // insertTableColumnToRightItem1
         // 
         this.insertTableColumnToRightItem1.Id = 59;
         this.insertTableColumnToRightItem1.Name = "insertTableColumnToRightItem1";
         // 
         // mergeTableCellsItem1
         // 
         this.mergeTableCellsItem1.Id = 60;
         this.mergeTableCellsItem1.Name = "mergeTableCellsItem1";
         // 
         // showSplitTableCellsForm1
         // 
         this.showSplitTableCellsForm1.Id = 61;
         this.showSplitTableCellsForm1.Name = "showSplitTableCellsForm1";
         // 
         // splitTableItem1
         // 
         this.splitTableItem1.Id = 62;
         this.splitTableItem1.Name = "splitTableItem1";
         // 
         // toggleTableAutoFitItem1
         // 
         this.toggleTableAutoFitItem1.Id = 63;
         this.toggleTableAutoFitItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleTableAutoFitContentsItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleTableAutoFitWindowItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleTableFixedColumnWidthItem1)});
         this.toggleTableAutoFitItem1.Name = "toggleTableAutoFitItem1";
         // 
         // toggleTableAutoFitContentsItem1
         // 
         this.toggleTableAutoFitContentsItem1.Id = 64;
         this.toggleTableAutoFitContentsItem1.Name = "toggleTableAutoFitContentsItem1";
         // 
         // toggleTableAutoFitWindowItem1
         // 
         this.toggleTableAutoFitWindowItem1.Id = 65;
         this.toggleTableAutoFitWindowItem1.Name = "toggleTableAutoFitWindowItem1";
         // 
         // toggleTableFixedColumnWidthItem1
         // 
         this.toggleTableFixedColumnWidthItem1.Id = 66;
         this.toggleTableFixedColumnWidthItem1.Name = "toggleTableFixedColumnWidthItem1";
         // 
         // toggleTableCellsTopLeftAlignmentItem1
         // 
         this.toggleTableCellsTopLeftAlignmentItem1.Id = 67;
         this.toggleTableCellsTopLeftAlignmentItem1.Name = "toggleTableCellsTopLeftAlignmentItem1";
         // 
         // toggleTableCellsMiddleLeftAlignmentItem1
         // 
         this.toggleTableCellsMiddleLeftAlignmentItem1.Id = 68;
         this.toggleTableCellsMiddleLeftAlignmentItem1.Name = "toggleTableCellsMiddleLeftAlignmentItem1";
         // 
         // toggleTableCellsBottomLeftAlignmentItem1
         // 
         this.toggleTableCellsBottomLeftAlignmentItem1.Id = 69;
         this.toggleTableCellsBottomLeftAlignmentItem1.Name = "toggleTableCellsBottomLeftAlignmentItem1";
         // 
         // toggleTableCellsTopCenterAlignmentItem1
         // 
         this.toggleTableCellsTopCenterAlignmentItem1.Id = 70;
         this.toggleTableCellsTopCenterAlignmentItem1.Name = "toggleTableCellsTopCenterAlignmentItem1";
         // 
         // toggleTableCellsMiddleCenterAlignmentItem1
         // 
         this.toggleTableCellsMiddleCenterAlignmentItem1.Id = 71;
         this.toggleTableCellsMiddleCenterAlignmentItem1.Name = "toggleTableCellsMiddleCenterAlignmentItem1";
         // 
         // toggleTableCellsBottomCenterAlignmentItem1
         // 
         this.toggleTableCellsBottomCenterAlignmentItem1.Id = 72;
         this.toggleTableCellsBottomCenterAlignmentItem1.Name = "toggleTableCellsBottomCenterAlignmentItem1";
         // 
         // toggleTableCellsTopRightAlignmentItem1
         // 
         this.toggleTableCellsTopRightAlignmentItem1.Id = 73;
         this.toggleTableCellsTopRightAlignmentItem1.Name = "toggleTableCellsTopRightAlignmentItem1";
         // 
         // toggleTableCellsMiddleRightAlignmentItem1
         // 
         this.toggleTableCellsMiddleRightAlignmentItem1.Id = 74;
         this.toggleTableCellsMiddleRightAlignmentItem1.Name = "toggleTableCellsMiddleRightAlignmentItem1";
         // 
         // toggleTableCellsBottomRightAlignmentItem1
         // 
         this.toggleTableCellsBottomRightAlignmentItem1.Id = 75;
         this.toggleTableCellsBottomRightAlignmentItem1.Name = "toggleTableCellsBottomRightAlignmentItem1";
         // 
         // showTableOptionsFormItem1
         // 
         this.showTableOptionsFormItem1.Id = 76;
         this.showTableOptionsFormItem1.Name = "showTableOptionsFormItem1";
         // 
         // pasteItem1
         // 
         this.pasteItem1.Id = 84;
         this.pasteItem1.Name = "pasteItem1";
         // 
         // cutItem1
         // 
         this.cutItem1.Id = 85;
         this.cutItem1.Name = "cutItem1";
         // 
         // copyItem1
         // 
         this.copyItem1.Id = 86;
         this.copyItem1.Name = "copyItem1";
         // 
         // pasteSpecialItem1
         // 
         this.pasteSpecialItem1.Id = 87;
         this.pasteSpecialItem1.Name = "pasteSpecialItem1";
         // 
         // barButtonGroup1
         // 
         this.barButtonGroup1.Id = 77;
         this.barButtonGroup1.ItemLinks.Add(this.changeFontNameItem1, "FF");
         this.barButtonGroup1.ItemLinks.Add(this.changeFontSizeItem1);
         this.barButtonGroup1.ItemLinks.Add(this.fontSizeIncreaseItem1, "FG");
         this.barButtonGroup1.ItemLinks.Add(this.fontSizeDecreaseItem1, "FK");
         this.barButtonGroup1.Name = "barButtonGroup1";
         this.barButtonGroup1.Tag = "{97BBE334-159B-44d9-A168-0411957565E8}";
         // 
         // changeFontNameItem1
         // 
         this.changeFontNameItem1.Edit = this.repositoryItemFontEdit1;
         this.changeFontNameItem1.Id = 88;
         this.changeFontNameItem1.Name = "changeFontNameItem1";
         // 
         // repositoryItemFontEdit1
         // 
         this.repositoryItemFontEdit1.AutoHeight = false;
         this.repositoryItemFontEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.repositoryItemFontEdit1.Name = "repositoryItemFontEdit1";
         // 
         // changeFontSizeItem1
         // 
         this.changeFontSizeItem1.Edit = this.repositoryItemRichEditFontSizeEdit1;
         this.changeFontSizeItem1.Id = 89;
         this.changeFontSizeItem1.Name = "changeFontSizeItem1";
         // 
         // repositoryItemRichEditFontSizeEdit1
         // 
         this.repositoryItemRichEditFontSizeEdit1.AutoHeight = false;
         this.repositoryItemRichEditFontSizeEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.repositoryItemRichEditFontSizeEdit1.Control = this.uxRichEditControl;
         this.repositoryItemRichEditFontSizeEdit1.Name = "repositoryItemRichEditFontSizeEdit1";
         // 
         // fontSizeIncreaseItem1
         // 
         this.fontSizeIncreaseItem1.Id = 90;
         this.fontSizeIncreaseItem1.Name = "fontSizeIncreaseItem1";
         // 
         // fontSizeDecreaseItem1
         // 
         this.fontSizeDecreaseItem1.Id = 91;
         this.fontSizeDecreaseItem1.Name = "fontSizeDecreaseItem1";
         // 
         // barButtonGroup2
         // 
         this.barButtonGroup2.Id = 78;
         this.barButtonGroup2.ItemLinks.Add(this.toggleFontBoldItem1);
         this.barButtonGroup2.ItemLinks.Add(this.toggleFontItalicItem1);
         this.barButtonGroup2.ItemLinks.Add(this.toggleFontUnderlineItem1);
         this.barButtonGroup2.ItemLinks.Add(this.toggleFontDoubleUnderlineItem1);
         this.barButtonGroup2.ItemLinks.Add(this.toggleFontStrikeoutItem1);
         this.barButtonGroup2.ItemLinks.Add(this.toggleFontDoubleStrikeoutItem1);
         this.barButtonGroup2.ItemLinks.Add(this.toggleFontSuperscriptItem1);
         this.barButtonGroup2.ItemLinks.Add(this.toggleFontSubscriptItem1);
         this.barButtonGroup2.Name = "barButtonGroup2";
         this.barButtonGroup2.Tag = "{433DA7F0-03E2-4650-9DB5-66DD92D16E39}";
         // 
         // toggleFontBoldItem1
         // 
         this.toggleFontBoldItem1.Id = 92;
         this.toggleFontBoldItem1.Name = "toggleFontBoldItem1";
         // 
         // toggleFontItalicItem1
         // 
         this.toggleFontItalicItem1.Id = 93;
         this.toggleFontItalicItem1.Name = "toggleFontItalicItem1";
         // 
         // toggleFontUnderlineItem1
         // 
         this.toggleFontUnderlineItem1.Id = 94;
         this.toggleFontUnderlineItem1.Name = "toggleFontUnderlineItem1";
         // 
         // toggleFontDoubleUnderlineItem1
         // 
         this.toggleFontDoubleUnderlineItem1.Id = 95;
         this.toggleFontDoubleUnderlineItem1.Name = "toggleFontDoubleUnderlineItem1";
         // 
         // toggleFontStrikeoutItem1
         // 
         this.toggleFontStrikeoutItem1.Id = 96;
         this.toggleFontStrikeoutItem1.Name = "toggleFontStrikeoutItem1";
         // 
         // toggleFontDoubleStrikeoutItem1
         // 
         this.toggleFontDoubleStrikeoutItem1.Id = 97;
         this.toggleFontDoubleStrikeoutItem1.Name = "toggleFontDoubleStrikeoutItem1";
         // 
         // toggleFontSuperscriptItem1
         // 
         this.toggleFontSuperscriptItem1.Id = 98;
         this.toggleFontSuperscriptItem1.Name = "toggleFontSuperscriptItem1";
         // 
         // toggleFontSubscriptItem1
         // 
         this.toggleFontSubscriptItem1.Id = 99;
         this.toggleFontSubscriptItem1.Name = "toggleFontSubscriptItem1";
         // 
         // barButtonGroup3
         // 
         this.barButtonGroup3.Id = 79;
         this.barButtonGroup3.ItemLinks.Add(this.changeFontColorItem1, "FC");
         this.barButtonGroup3.ItemLinks.Add(this.changeFontBackColorItem1, "I");
         this.barButtonGroup3.Name = "barButtonGroup3";
         this.barButtonGroup3.Tag = "{DF8C5334-EDE3-47c9-A42C-FE9A9247E180}";
         // 
         // changeFontColorItem1
         // 
         this.changeFontColorItem1.Id = 100;
         this.changeFontColorItem1.Name = "changeFontColorItem1";
         // 
         // changeFontBackColorItem1
         // 
         this.changeFontBackColorItem1.Id = 101;
         this.changeFontBackColorItem1.Name = "changeFontBackColorItem1";
         // 
         // changeTextCaseItem1
         // 
         this.changeTextCaseItem1.Id = 102;
         this.changeTextCaseItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.makeTextUpperCaseItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.makeTextLowerCaseItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.capitalizeEachWordCaseItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleTextCaseItem1)});
         this.changeTextCaseItem1.Name = "changeTextCaseItem1";
         // 
         // makeTextUpperCaseItem1
         // 
         this.makeTextUpperCaseItem1.Id = 103;
         this.makeTextUpperCaseItem1.Name = "makeTextUpperCaseItem1";
         // 
         // makeTextLowerCaseItem1
         // 
         this.makeTextLowerCaseItem1.Id = 104;
         this.makeTextLowerCaseItem1.Name = "makeTextLowerCaseItem1";
         // 
         // capitalizeEachWordCaseItem1
         // 
         this.capitalizeEachWordCaseItem1.Id = 105;
         this.capitalizeEachWordCaseItem1.Name = "capitalizeEachWordCaseItem1";
         // 
         // toggleTextCaseItem1
         // 
         this.toggleTextCaseItem1.Id = 106;
         this.toggleTextCaseItem1.Name = "toggleTextCaseItem1";
         // 
         // clearFormattingItem1
         // 
         this.clearFormattingItem1.Id = 107;
         this.clearFormattingItem1.Name = "clearFormattingItem1";
         // 
         // barButtonGroup4
         // 
         this.barButtonGroup4.Id = 80;
         this.barButtonGroup4.ItemLinks.Add(this.toggleBulletedListItem1, "U");
         this.barButtonGroup4.ItemLinks.Add(this.toggleNumberingListItem1, "N");
         this.barButtonGroup4.ItemLinks.Add(this.toggleMultiLevelListItem1, "M");
         this.barButtonGroup4.Name = "barButtonGroup4";
         this.barButtonGroup4.Tag = "{0B3A7A43-3079-4ce0-83A8-3789F5F6DC9F}";
         // 
         // toggleBulletedListItem1
         // 
         this.toggleBulletedListItem1.Id = 108;
         this.toggleBulletedListItem1.Name = "toggleBulletedListItem1";
         // 
         // toggleNumberingListItem1
         // 
         this.toggleNumberingListItem1.Id = 109;
         this.toggleNumberingListItem1.Name = "toggleNumberingListItem1";
         // 
         // toggleMultiLevelListItem1
         // 
         this.toggleMultiLevelListItem1.Id = 110;
         this.toggleMultiLevelListItem1.Name = "toggleMultiLevelListItem1";
         // 
         // barButtonGroup5
         // 
         this.barButtonGroup5.Id = 81;
         this.barButtonGroup5.ItemLinks.Add(this.decreaseIndentItem1, "AO");
         this.barButtonGroup5.ItemLinks.Add(this.increaseIndentItem1, "AI");
         this.barButtonGroup5.ItemLinks.Add(this.toggleShowWhitespaceItem1);
         this.barButtonGroup5.Name = "barButtonGroup5";
         this.barButtonGroup5.Tag = "{4747D5AB-2BEB-4ea6-9A1D-8E4FB36F1B40}";
         // 
         // decreaseIndentItem1
         // 
         this.decreaseIndentItem1.Id = 111;
         this.decreaseIndentItem1.Name = "decreaseIndentItem1";
         // 
         // increaseIndentItem1
         // 
         this.increaseIndentItem1.Id = 112;
         this.increaseIndentItem1.Name = "increaseIndentItem1";
         // 
         // toggleShowWhitespaceItem1
         // 
         this.toggleShowWhitespaceItem1.Id = 117;
         this.toggleShowWhitespaceItem1.Name = "toggleShowWhitespaceItem1";
         // 
         // barButtonGroup6
         // 
         this.barButtonGroup6.Id = 82;
         this.barButtonGroup6.ItemLinks.Add(this.toggleParagraphAlignmentLeftItem1, "AL");
         this.barButtonGroup6.ItemLinks.Add(this.toggleParagraphAlignmentCenterItem1, "AC");
         this.barButtonGroup6.ItemLinks.Add(this.toggleParagraphAlignmentRightItem1, "AR");
         this.barButtonGroup6.ItemLinks.Add(this.toggleParagraphAlignmentJustifyItem1, "AJ");
         this.barButtonGroup6.Name = "barButtonGroup6";
         this.barButtonGroup6.Tag = "{8E89E775-996E-49a0-AADA-DE338E34732E}";
         // 
         // toggleParagraphAlignmentLeftItem1
         // 
         this.toggleParagraphAlignmentLeftItem1.Id = 113;
         this.toggleParagraphAlignmentLeftItem1.Name = "toggleParagraphAlignmentLeftItem1";
         // 
         // toggleParagraphAlignmentCenterItem1
         // 
         this.toggleParagraphAlignmentCenterItem1.Id = 114;
         this.toggleParagraphAlignmentCenterItem1.Name = "toggleParagraphAlignmentCenterItem1";
         // 
         // toggleParagraphAlignmentRightItem1
         // 
         this.toggleParagraphAlignmentRightItem1.Id = 115;
         this.toggleParagraphAlignmentRightItem1.Name = "toggleParagraphAlignmentRightItem1";
         // 
         // toggleParagraphAlignmentJustifyItem1
         // 
         this.toggleParagraphAlignmentJustifyItem1.Id = 116;
         this.toggleParagraphAlignmentJustifyItem1.Name = "toggleParagraphAlignmentJustifyItem1";
         // 
         // barButtonGroup7
         // 
         this.barButtonGroup7.Id = 83;
         this.barButtonGroup7.ItemLinks.Add(this.changeParagraphLineSpacingItem1, "K");
         this.barButtonGroup7.ItemLinks.Add(this.changeParagraphBackColorItem1, "H");
         this.barButtonGroup7.Name = "barButtonGroup7";
         this.barButtonGroup7.Tag = "{9A8DEAD8-3890-4857-A395-EC625FD02217}";
         // 
         // changeParagraphLineSpacingItem1
         // 
         this.changeParagraphLineSpacingItem1.Id = 118;
         this.changeParagraphLineSpacingItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.setSingleParagraphSpacingItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.setSesquialteralParagraphSpacingItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.setDoubleParagraphSpacingItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.showLineSpacingFormItem1),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.KeyTip, this.addSpacingBeforeParagraphItem1, "", false, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard, "B", ""),
            new DevExpress.XtraBars.LinkPersistInfo(this.removeSpacingBeforeParagraphItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.addSpacingAfterParagraphItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.removeSpacingAfterParagraphItem1)});
         this.changeParagraphLineSpacingItem1.Name = "changeParagraphLineSpacingItem1";
         // 
         // setSingleParagraphSpacingItem1
         // 
         this.setSingleParagraphSpacingItem1.Id = 119;
         this.setSingleParagraphSpacingItem1.Name = "setSingleParagraphSpacingItem1";
         // 
         // setSesquialteralParagraphSpacingItem1
         // 
         this.setSesquialteralParagraphSpacingItem1.Id = 120;
         this.setSesquialteralParagraphSpacingItem1.Name = "setSesquialteralParagraphSpacingItem1";
         // 
         // setDoubleParagraphSpacingItem1
         // 
         this.setDoubleParagraphSpacingItem1.Id = 121;
         this.setDoubleParagraphSpacingItem1.Name = "setDoubleParagraphSpacingItem1";
         // 
         // showLineSpacingFormItem1
         // 
         this.showLineSpacingFormItem1.Id = 122;
         this.showLineSpacingFormItem1.Name = "showLineSpacingFormItem1";
         // 
         // addSpacingBeforeParagraphItem1
         // 
         this.addSpacingBeforeParagraphItem1.Id = 123;
         this.addSpacingBeforeParagraphItem1.Name = "addSpacingBeforeParagraphItem1";
         // 
         // removeSpacingBeforeParagraphItem1
         // 
         this.removeSpacingBeforeParagraphItem1.Id = 124;
         this.removeSpacingBeforeParagraphItem1.Name = "removeSpacingBeforeParagraphItem1";
         // 
         // addSpacingAfterParagraphItem1
         // 
         this.addSpacingAfterParagraphItem1.Id = 125;
         this.addSpacingAfterParagraphItem1.Name = "addSpacingAfterParagraphItem1";
         // 
         // removeSpacingAfterParagraphItem1
         // 
         this.removeSpacingAfterParagraphItem1.Id = 126;
         this.removeSpacingAfterParagraphItem1.Name = "removeSpacingAfterParagraphItem1";
         // 
         // changeParagraphBackColorItem1
         // 
         this.changeParagraphBackColorItem1.Id = 127;
         this.changeParagraphBackColorItem1.Name = "changeParagraphBackColorItem1";
         // 
         // findItem1
         // 
         this.findItem1.Id = 129;
         this.findItem1.Name = "findItem1";
         // 
         // replaceItem1
         // 
         this.replaceItem1.Id = 130;
         this.replaceItem1.Name = "replaceItem1";
         // 
         // tableToolsRibbonPageCategory1
         // 
         this.tableToolsRibbonPageCategory1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(233)))), ((int)(((byte)(20)))));
         this.tableToolsRibbonPageCategory1.Control = this.uxRichEditControl;
         this.tableToolsRibbonPageCategory1.Name = "tableToolsRibbonPageCategory1";
         this.tableToolsRibbonPageCategory1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.tableDesignRibbonPage1,
            this.tableLayoutRibbonPage1});
         // 
         // tableDesignRibbonPage1
         // 
         this.tableDesignRibbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.tableStyleOptionsRibbonPageGroup1,
            this.tableStylesRibbonPageGroup1,
            this.tableDrawBordersRibbonPageGroup1});
         this.tableDesignRibbonPage1.Name = "tableDesignRibbonPage1";
         // 
         // tableStyleOptionsRibbonPageGroup1
         // 
         this.tableStyleOptionsRibbonPageGroup1.ItemLinks.Add(this.toggleFirstRowItem1);
         this.tableStyleOptionsRibbonPageGroup1.ItemLinks.Add(this.toggleLastRowItem1);
         this.tableStyleOptionsRibbonPageGroup1.ItemLinks.Add(this.toggleBandedRowsItem1);
         this.tableStyleOptionsRibbonPageGroup1.ItemLinks.Add(this.toggleFirstColumnItem1);
         this.tableStyleOptionsRibbonPageGroup1.ItemLinks.Add(this.toggleLastColumnItem1);
         this.tableStyleOptionsRibbonPageGroup1.ItemLinks.Add(this.toggleBandedColumnsItem1);
         this.tableStyleOptionsRibbonPageGroup1.Name = "tableStyleOptionsRibbonPageGroup1";
         // 
         // tableStylesRibbonPageGroup1
         // 
         this.tableStylesRibbonPageGroup1.ItemLinks.Add(this.galleryChangeTableStyleItem1);
         this.tableStylesRibbonPageGroup1.Name = "tableStylesRibbonPageGroup1";
         // 
         // tableDrawBordersRibbonPageGroup1
         // 
         this.tableDrawBordersRibbonPageGroup1.ItemLinks.Add(this.changeTableBorderLineStyleItem1);
         this.tableDrawBordersRibbonPageGroup1.ItemLinks.Add(this.changeTableBorderLineWeightItem1);
         this.tableDrawBordersRibbonPageGroup1.ItemLinks.Add(this.changeTableBorderColorItem1, "C");
         this.tableDrawBordersRibbonPageGroup1.ItemLinks.Add(this.changeTableBordersItem1, "B");
         this.tableDrawBordersRibbonPageGroup1.ItemLinks.Add(this.changeTableCellsShadingItem1, "H");
         this.tableDrawBordersRibbonPageGroup1.Name = "tableDrawBordersRibbonPageGroup1";
         // 
         // tableLayoutRibbonPage1
         // 
         this.tableLayoutRibbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.tableTableRibbonPageGroup1,
            this.tableRowsAndColumnsRibbonPageGroup1,
            this.tableMergeRibbonPageGroup1,
            this.tableCellSizeRibbonPageGroup1,
            this.tableAlignmentRibbonPageGroup1});
         this.tableLayoutRibbonPage1.Name = "tableLayoutRibbonPage1";
         // 
         // tableTableRibbonPageGroup1
         // 
         this.tableTableRibbonPageGroup1.ItemLinks.Add(this.selectTableElementsItem1, "K");
         this.tableTableRibbonPageGroup1.ItemLinks.Add(this.toggleShowTableGridLinesItem1, "TG");
         this.tableTableRibbonPageGroup1.ItemLinks.Add(this.showTablePropertiesFormItem1, "O");
         this.tableTableRibbonPageGroup1.Name = "tableTableRibbonPageGroup1";
         // 
         // tableRowsAndColumnsRibbonPageGroup1
         // 
         this.tableRowsAndColumnsRibbonPageGroup1.ItemLinks.Add(this.deleteTableElementsItem1, "D");
         this.tableRowsAndColumnsRibbonPageGroup1.ItemLinks.Add(this.insertTableRowAboveItem1, "A");
         this.tableRowsAndColumnsRibbonPageGroup1.ItemLinks.Add(this.insertTableRowBelowItem1, "E");
         this.tableRowsAndColumnsRibbonPageGroup1.ItemLinks.Add(this.insertTableColumnToLeftItem1, "L");
         this.tableRowsAndColumnsRibbonPageGroup1.ItemLinks.Add(this.insertTableColumnToRightItem1, "R");
         this.tableRowsAndColumnsRibbonPageGroup1.Name = "tableRowsAndColumnsRibbonPageGroup1";
         // 
         // tableMergeRibbonPageGroup1
         // 
         this.tableMergeRibbonPageGroup1.ItemLinks.Add(this.mergeTableCellsItem1, "M");
         this.tableMergeRibbonPageGroup1.ItemLinks.Add(this.showSplitTableCellsForm1, "P");
         this.tableMergeRibbonPageGroup1.ItemLinks.Add(this.splitTableItem1, "Q");
         this.tableMergeRibbonPageGroup1.Name = "tableMergeRibbonPageGroup1";
         // 
         // tableCellSizeRibbonPageGroup1
         // 
         this.tableCellSizeRibbonPageGroup1.AllowTextClipping = false;
         this.tableCellSizeRibbonPageGroup1.ItemLinks.Add(this.toggleTableAutoFitItem1, "F");
         this.tableCellSizeRibbonPageGroup1.Name = "tableCellSizeRibbonPageGroup1";
         // 
         // tableAlignmentRibbonPageGroup1
         // 
         this.tableAlignmentRibbonPageGroup1.Glyph = ((System.Drawing.Image)(resources.GetObject("tableAlignmentRibbonPageGroup1.Glyph")));
         this.tableAlignmentRibbonPageGroup1.ItemLinks.Add(this.toggleTableCellsTopLeftAlignmentItem1, "TL");
         this.tableAlignmentRibbonPageGroup1.ItemLinks.Add(this.toggleTableCellsMiddleLeftAlignmentItem1, "CL");
         this.tableAlignmentRibbonPageGroup1.ItemLinks.Add(this.toggleTableCellsBottomLeftAlignmentItem1, "BL");
         this.tableAlignmentRibbonPageGroup1.ItemLinks.Add(this.toggleTableCellsTopCenterAlignmentItem1, "TC");
         this.tableAlignmentRibbonPageGroup1.ItemLinks.Add(this.toggleTableCellsMiddleCenterAlignmentItem1, "CC");
         this.tableAlignmentRibbonPageGroup1.ItemLinks.Add(this.toggleTableCellsBottomCenterAlignmentItem1, "BC");
         this.tableAlignmentRibbonPageGroup1.ItemLinks.Add(this.toggleTableCellsTopRightAlignmentItem1, "TR");
         this.tableAlignmentRibbonPageGroup1.ItemLinks.Add(this.toggleTableCellsMiddleRightAlignmentItem1, "CR");
         this.tableAlignmentRibbonPageGroup1.ItemLinks.Add(this.toggleTableCellsBottomRightAlignmentItem1, "BR");
         this.tableAlignmentRibbonPageGroup1.ItemLinks.Add(this.showTableOptionsFormItem1, "N");
         this.tableAlignmentRibbonPageGroup1.Name = "tableAlignmentRibbonPageGroup1";
         // 
         // homeRibbonPage1
         // 
         this.homeRibbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.clipboardRibbonPageGroup1,
            this.fontRibbonPageGroup1,
            this.paragraphRibbonPageGroup1,
            this.stylesRibbonPageGroup1,
            this.editingRibbonPageGroup1});
         this.homeRibbonPage1.Name = "homeRibbonPage1";
         reduceOperation1.Behavior = DevExpress.XtraBars.Ribbon.ReduceOperationBehavior.UntilAvailable;
         reduceOperation1.Group = this.stylesRibbonPageGroup1;
         reduceOperation1.ItemLinkIndex = 0;
         reduceOperation1.ItemLinksCount = 0;
         reduceOperation1.Operation = DevExpress.XtraBars.Ribbon.ReduceOperationType.Gallery;
         this.homeRibbonPage1.ReduceOperations.Add(reduceOperation1);
         // 
         // clipboardRibbonPageGroup1
         // 
         this.clipboardRibbonPageGroup1.ItemLinks.Add(this.pasteItem1, "V");
         this.clipboardRibbonPageGroup1.ItemLinks.Add(this.cutItem1, "X");
         this.clipboardRibbonPageGroup1.ItemLinks.Add(this.copyItem1, "C");
         this.clipboardRibbonPageGroup1.ItemLinks.Add(this.pasteSpecialItem1);
         this.clipboardRibbonPageGroup1.Name = "clipboardRibbonPageGroup1";
         // 
         // fontRibbonPageGroup1
         // 
         this.fontRibbonPageGroup1.ItemLinks.Add(this.barButtonGroup1);
         this.fontRibbonPageGroup1.ItemLinks.Add(this.barButtonGroup2);
         this.fontRibbonPageGroup1.ItemLinks.Add(this.barButtonGroup3);
         this.fontRibbonPageGroup1.ItemLinks.Add(this.changeTextCaseItem1);
         this.fontRibbonPageGroup1.ItemLinks.Add(this.clearFormattingItem1, "E");
         this.fontRibbonPageGroup1.Name = "fontRibbonPageGroup1";
         // 
         // paragraphRibbonPageGroup1
         // 
         this.paragraphRibbonPageGroup1.ItemLinks.Add(this.barButtonGroup4);
         this.paragraphRibbonPageGroup1.ItemLinks.Add(this.barButtonGroup5);
         this.paragraphRibbonPageGroup1.ItemLinks.Add(this.barButtonGroup6);
         this.paragraphRibbonPageGroup1.ItemLinks.Add(this.barButtonGroup7);
         this.paragraphRibbonPageGroup1.Name = "paragraphRibbonPageGroup1";
         // 
         // editingRibbonPageGroup1
         // 
         this.editingRibbonPageGroup1.ItemLinks.Add(this.findItem1, "FD");
         this.editingRibbonPageGroup1.ItemLinks.Add(this.replaceItem1, "R");
         this.editingRibbonPageGroup1.Name = "editingRibbonPageGroup1";
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
         this.commonRibbonPageGroup1.ItemLinks.Add(this.fileNewItem1, "N");
         this.commonRibbonPageGroup1.ItemLinks.Add(this.fileOpenItem1, "O");
         this.commonRibbonPageGroup1.ItemLinks.Add(this.fileSaveItem1, "S");
         this.commonRibbonPageGroup1.ItemLinks.Add(this.fileSaveAsItem1, "A");
         this.commonRibbonPageGroup1.ItemLinks.Add(this.quickPrintItem1);
         this.commonRibbonPageGroup1.ItemLinks.Add(this.printItem1, "P");
         this.commonRibbonPageGroup1.ItemLinks.Add(this.printPreviewItem1);
         this.commonRibbonPageGroup1.Name = "commonRibbonPageGroup1";
         // 
         // insertRibbonPage1
         // 
         this.insertRibbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.pagesRibbonPageGroup,
            this.tablesRibbonPageGroup1,
            this.illustrationsRibbonPageGroup1,
            this.linksRibbonPageGroup1,
            this.headerFooterRibbonPageGroup,
            this.textRibbonPageGroup1,
            this.symbolsRibbonPageGroup1});
         this.insertRibbonPage1.Name = "insertRibbonPage1";
         // 
         // pagesRibbonPageGroup
         // 
         this.pagesRibbonPageGroup.AllowTextClipping = false;
         this.pagesRibbonPageGroup.ItemLinks.Add(this.insertPageBreakItem21, "B");
         this.pagesRibbonPageGroup.Name = "pagesRibbonPageGroup";
         // 
         // tablesRibbonPageGroup1
         // 
         this.tablesRibbonPageGroup1.AllowTextClipping = false;
         this.tablesRibbonPageGroup1.ItemLinks.Add(this.insertTableItem1, "T");
         this.tablesRibbonPageGroup1.Name = "tablesRibbonPageGroup1";
         // 
         // illustrationsRibbonPageGroup1
         // 
         this.illustrationsRibbonPageGroup1.ItemLinks.Add(this.insertPictureItem1, "P");
         this.illustrationsRibbonPageGroup1.ItemLinks.Add(this.insertFloatingPictureItem1);
         this.illustrationsRibbonPageGroup1.Name = "illustrationsRibbonPageGroup1";
         // 
         // linksRibbonPageGroup1
         // 
         this.linksRibbonPageGroup1.ItemLinks.Add(this.insertBookmarkItem1, "K");
         this.linksRibbonPageGroup1.ItemLinks.Add(this.insertHyperlinkItem1, "I");
         this.linksRibbonPageGroup1.Name = "linksRibbonPageGroup1";
         // 
         // headerFooterRibbonPageGroup
         // 
         this.headerFooterRibbonPageGroup.ItemLinks.Add(this.editPageHeaderItem1, "H");
         this.headerFooterRibbonPageGroup.ItemLinks.Add(this.editPageFooterItem1, "O");
         this.headerFooterRibbonPageGroup.ItemLinks.Add(this.insertPageNumberItem1, "NU");
         this.headerFooterRibbonPageGroup.ItemLinks.Add(this.insertPageCountItem1);
         this.headerFooterRibbonPageGroup.Name = "headerFooterRibbonPageGroup";
         // 
         // textRibbonPageGroup1
         // 
         this.textRibbonPageGroup1.Glyph = ((System.Drawing.Image)(resources.GetObject("textRibbonPageGroup1.Glyph")));
         this.textRibbonPageGroup1.ItemLinks.Add(this.insertTextBoxItem1, "X");
         this.textRibbonPageGroup1.Name = "textRibbonPageGroup1";
         // 
         // symbolsRibbonPageGroup1
         // 
         this.symbolsRibbonPageGroup1.AllowTextClipping = false;
         this.symbolsRibbonPageGroup1.ItemLinks.Add(this.insertSymbolItem1, "U");
         this.symbolsRibbonPageGroup1.Name = "symbolsRibbonPageGroup1";
         // 
         // cbOrigin
         // 
         this.cbOrigin.Location = new System.Drawing.Point(837, 2);
         this.cbOrigin.MenuManager = this.ribbonControl;
         this.cbOrigin.Name = "cbOrigin";
         this.cbOrigin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbOrigin.Size = new System.Drawing.Size(101, 20);
         this.cbOrigin.StyleController = this.layoutControl1;
         this.cbOrigin.TabIndex = 7;
         // 
         // tagEdit
         // 
         this.tagEdit.Location = new System.Drawing.Point(106, 379);
         this.tagEdit.MenuManager = this.ribbonControl;
         this.tagEdit.Name = "tagEdit";
         this.tagEdit.Properties.EditMode = DevExpress.XtraEditors.TokenEditMode.Manual;
         this.tagEdit.Properties.Separators.AddRange(new string[] {
            ","});
         this.tagEdit.Size = new System.Drawing.Size(362, 20);
         this.tagEdit.StyleController = this.layoutControl1;
         this.tagEdit.TabIndex = 5;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.richEditControlItem,
            this.layoutItemTokenEdit,
            this.layoutItemSource,
            this.layoutItemTitle,
            this.layoutItemButtonPreviousPage,
            this.layoutItemButtonNextPage});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(940, 403);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // richEditControlItem
         // 
         this.richEditControlItem.Control = this.uxRichEditControl;
         this.richEditControlItem.CustomizationFormText = "richEditControlItem";
         this.richEditControlItem.Location = new System.Drawing.Point(0, 24);
         this.richEditControlItem.Name = "richEditControlItem";
         this.richEditControlItem.Size = new System.Drawing.Size(940, 353);
         this.richEditControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.richEditControlItem.TextVisible = false;
         // 
         // layoutItemTokenEdit
         // 
         this.layoutItemTokenEdit.Control = this.tagEdit;
         this.layoutItemTokenEdit.CustomizationFormText = "tokenEditControlItem";
         this.layoutItemTokenEdit.Location = new System.Drawing.Point(0, 377);
         this.layoutItemTokenEdit.Name = "layoutItemTokenEdit";
         this.layoutItemTokenEdit.Size = new System.Drawing.Size(470, 26);
         this.layoutItemTokenEdit.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
         this.layoutItemTokenEdit.TextSize = new System.Drawing.Size(99, 13);
         this.layoutItemTokenEdit.TextToControlDistance = 5;
         // 
         // layoutItemSource
         // 
         this.layoutItemSource.Control = this.cbOrigin;
         this.layoutItemSource.Location = new System.Drawing.Point(745, 0);
         this.layoutItemSource.MaxSize = new System.Drawing.Size(195, 0);
         this.layoutItemSource.MinSize = new System.Drawing.Size(195, 24);
         this.layoutItemSource.Name = "layoutItemSource";
         this.layoutItemSource.Size = new System.Drawing.Size(195, 24);
         this.layoutItemSource.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemSource.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
         this.layoutItemSource.TextSize = new System.Drawing.Size(85, 13);
         this.layoutItemSource.TextToControlDistance = 5;
         // 
         // layoutItemTitle
         // 
         this.layoutItemTitle.Control = this.tbTitle;
         this.layoutItemTitle.Location = new System.Drawing.Point(0, 0);
         this.layoutItemTitle.Name = "layoutItemTitle";
         this.layoutItemTitle.Size = new System.Drawing.Size(745, 24);
         this.layoutItemTitle.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
         this.layoutItemTitle.TextSize = new System.Drawing.Size(72, 13);
         this.layoutItemTitle.TextToControlDistance = 5;
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
         this.richEditBarController1.BarItems.Add(this.insertPageBreakItem21);
         this.richEditBarController1.BarItems.Add(this.insertTableItem1);
         this.richEditBarController1.BarItems.Add(this.insertPictureItem1);
         this.richEditBarController1.BarItems.Add(this.insertFloatingPictureItem1);
         this.richEditBarController1.BarItems.Add(this.insertBookmarkItem1);
         this.richEditBarController1.BarItems.Add(this.insertHyperlinkItem1);
         this.richEditBarController1.BarItems.Add(this.editPageHeaderItem1);
         this.richEditBarController1.BarItems.Add(this.editPageFooterItem1);
         this.richEditBarController1.BarItems.Add(this.insertPageNumberItem1);
         this.richEditBarController1.BarItems.Add(this.insertPageCountItem1);
         this.richEditBarController1.BarItems.Add(this.insertTextBoxItem1);
         this.richEditBarController1.BarItems.Add(this.insertSymbolItem1);
         this.richEditBarController1.BarItems.Add(this.toggleFirstRowItem1);
         this.richEditBarController1.BarItems.Add(this.toggleLastRowItem1);
         this.richEditBarController1.BarItems.Add(this.toggleBandedRowsItem1);
         this.richEditBarController1.BarItems.Add(this.toggleFirstColumnItem1);
         this.richEditBarController1.BarItems.Add(this.toggleLastColumnItem1);
         this.richEditBarController1.BarItems.Add(this.toggleBandedColumnsItem1);
         this.richEditBarController1.BarItems.Add(this.galleryChangeTableStyleItem1);
         this.richEditBarController1.BarItems.Add(this.changeTableBorderLineStyleItem1);
         this.richEditBarController1.BarItems.Add(this.changeTableBorderLineWeightItem1);
         this.richEditBarController1.BarItems.Add(this.changeTableBorderColorItem1);
         this.richEditBarController1.BarItems.Add(this.changeTableBordersItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsBottomBorderItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsTopBorderItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsLeftBorderItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsRightBorderItem1);
         this.richEditBarController1.BarItems.Add(this.resetTableCellsAllBordersItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsAllBordersItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsOutsideBorderItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsInsideBorderItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsInsideHorizontalBorderItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsInsideVerticalBorderItem1);
         this.richEditBarController1.BarItems.Add(this.toggleShowTableGridLinesItem1);
         this.richEditBarController1.BarItems.Add(this.changeTableCellsShadingItem1);
         this.richEditBarController1.BarItems.Add(this.selectTableElementsItem1);
         this.richEditBarController1.BarItems.Add(this.selectTableCellItem1);
         this.richEditBarController1.BarItems.Add(this.selectTableColumnItem1);
         this.richEditBarController1.BarItems.Add(this.selectTableRowItem1);
         this.richEditBarController1.BarItems.Add(this.selectTableItem1);
         this.richEditBarController1.BarItems.Add(this.showTablePropertiesFormItem1);
         this.richEditBarController1.BarItems.Add(this.deleteTableElementsItem1);
         this.richEditBarController1.BarItems.Add(this.showDeleteTableCellsFormItem1);
         this.richEditBarController1.BarItems.Add(this.deleteTableColumnsItem1);
         this.richEditBarController1.BarItems.Add(this.deleteTableRowsItem1);
         this.richEditBarController1.BarItems.Add(this.deleteTableItem1);
         this.richEditBarController1.BarItems.Add(this.insertTableRowAboveItem1);
         this.richEditBarController1.BarItems.Add(this.insertTableRowBelowItem1);
         this.richEditBarController1.BarItems.Add(this.insertTableColumnToLeftItem1);
         this.richEditBarController1.BarItems.Add(this.insertTableColumnToRightItem1);
         this.richEditBarController1.BarItems.Add(this.mergeTableCellsItem1);
         this.richEditBarController1.BarItems.Add(this.showSplitTableCellsForm1);
         this.richEditBarController1.BarItems.Add(this.splitTableItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableAutoFitItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableAutoFitContentsItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableAutoFitWindowItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableFixedColumnWidthItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsTopLeftAlignmentItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsMiddleLeftAlignmentItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsBottomLeftAlignmentItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsTopCenterAlignmentItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsMiddleCenterAlignmentItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsBottomCenterAlignmentItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsTopRightAlignmentItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsMiddleRightAlignmentItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTableCellsBottomRightAlignmentItem1);
         this.richEditBarController1.BarItems.Add(this.showTableOptionsFormItem1);
         this.richEditBarController1.BarItems.Add(this.pasteItem1);
         this.richEditBarController1.BarItems.Add(this.cutItem1);
         this.richEditBarController1.BarItems.Add(this.copyItem1);
         this.richEditBarController1.BarItems.Add(this.pasteSpecialItem1);
         this.richEditBarController1.BarItems.Add(this.changeFontNameItem1);
         this.richEditBarController1.BarItems.Add(this.changeFontSizeItem1);
         this.richEditBarController1.BarItems.Add(this.fontSizeIncreaseItem1);
         this.richEditBarController1.BarItems.Add(this.fontSizeDecreaseItem1);
         this.richEditBarController1.BarItems.Add(this.toggleFontBoldItem1);
         this.richEditBarController1.BarItems.Add(this.toggleFontItalicItem1);
         this.richEditBarController1.BarItems.Add(this.toggleFontUnderlineItem1);
         this.richEditBarController1.BarItems.Add(this.toggleFontDoubleUnderlineItem1);
         this.richEditBarController1.BarItems.Add(this.toggleFontStrikeoutItem1);
         this.richEditBarController1.BarItems.Add(this.toggleFontDoubleStrikeoutItem1);
         this.richEditBarController1.BarItems.Add(this.toggleFontSuperscriptItem1);
         this.richEditBarController1.BarItems.Add(this.toggleFontSubscriptItem1);
         this.richEditBarController1.BarItems.Add(this.changeFontColorItem1);
         this.richEditBarController1.BarItems.Add(this.changeFontBackColorItem1);
         this.richEditBarController1.BarItems.Add(this.changeTextCaseItem1);
         this.richEditBarController1.BarItems.Add(this.makeTextUpperCaseItem1);
         this.richEditBarController1.BarItems.Add(this.makeTextLowerCaseItem1);
         this.richEditBarController1.BarItems.Add(this.capitalizeEachWordCaseItem1);
         this.richEditBarController1.BarItems.Add(this.toggleTextCaseItem1);
         this.richEditBarController1.BarItems.Add(this.clearFormattingItem1);
         this.richEditBarController1.BarItems.Add(this.toggleBulletedListItem1);
         this.richEditBarController1.BarItems.Add(this.toggleNumberingListItem1);
         this.richEditBarController1.BarItems.Add(this.toggleMultiLevelListItem1);
         this.richEditBarController1.BarItems.Add(this.decreaseIndentItem1);
         this.richEditBarController1.BarItems.Add(this.increaseIndentItem1);
         this.richEditBarController1.BarItems.Add(this.toggleParagraphAlignmentLeftItem1);
         this.richEditBarController1.BarItems.Add(this.toggleParagraphAlignmentCenterItem1);
         this.richEditBarController1.BarItems.Add(this.toggleParagraphAlignmentRightItem1);
         this.richEditBarController1.BarItems.Add(this.toggleParagraphAlignmentJustifyItem1);
         this.richEditBarController1.BarItems.Add(this.toggleShowWhitespaceItem1);
         this.richEditBarController1.BarItems.Add(this.changeParagraphLineSpacingItem1);
         this.richEditBarController1.BarItems.Add(this.setSingleParagraphSpacingItem1);
         this.richEditBarController1.BarItems.Add(this.setSesquialteralParagraphSpacingItem1);
         this.richEditBarController1.BarItems.Add(this.setDoubleParagraphSpacingItem1);
         this.richEditBarController1.BarItems.Add(this.showLineSpacingFormItem1);
         this.richEditBarController1.BarItems.Add(this.addSpacingBeforeParagraphItem1);
         this.richEditBarController1.BarItems.Add(this.removeSpacingBeforeParagraphItem1);
         this.richEditBarController1.BarItems.Add(this.addSpacingAfterParagraphItem1);
         this.richEditBarController1.BarItems.Add(this.removeSpacingAfterParagraphItem1);
         this.richEditBarController1.BarItems.Add(this.changeParagraphBackColorItem1);
         this.richEditBarController1.BarItems.Add(this.galleryChangeStyleItem1);
         this.richEditBarController1.BarItems.Add(this.findItem1);
         this.richEditBarController1.BarItems.Add(this.replaceItem1);
         this.richEditBarController1.Control = this.uxRichEditControl;
         // 
         // buttonNextPage
         // 
         this.buttonNextPage.Location = new System.Drawing.Point(707, 379);
         this.buttonNextPage.Name = "buttonNextPage";
         this.buttonNextPage.Size = new System.Drawing.Size(231, 22);
         this.buttonNextPage.StyleController = this.layoutControl1;
         this.buttonNextPage.TabIndex = 9;
         this.buttonNextPage.Text = "buttonNextPage";
         // 
         // layoutItemButtonNextPage
         // 
         this.layoutItemButtonNextPage.Control = this.buttonNextPage;
         this.layoutItemButtonNextPage.Location = new System.Drawing.Point(705, 377);
         this.layoutItemButtonNextPage.Name = "layoutItemButtonNextPage";
         this.layoutItemButtonNextPage.Size = new System.Drawing.Size(235, 26);
         this.layoutItemButtonNextPage.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemButtonNextPage.TextVisible = false;
         // 
         // buttonPreviousPage
         // 
         this.buttonPreviousPage.Location = new System.Drawing.Point(472, 379);
         this.buttonPreviousPage.Name = "buttonPreviousPage";
         this.buttonPreviousPage.Size = new System.Drawing.Size(231, 22);
         this.buttonPreviousPage.StyleController = this.layoutControl1;
         this.buttonPreviousPage.TabIndex = 10;
         this.buttonPreviousPage.Text = "buttonPreviousPage";
         // 
         // layoutItemButtonPreviousPage
         // 
         this.layoutItemButtonPreviousPage.Control = this.buttonPreviousPage;
         this.layoutItemButtonPreviousPage.Location = new System.Drawing.Point(470, 377);
         this.layoutItemButtonPreviousPage.Name = "layoutItemButtonPreviousPage";
         this.layoutItemButtonPreviousPage.Size = new System.Drawing.Size(235, 26);
         this.layoutItemButtonPreviousPage.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemButtonPreviousPage.TextVisible = false;
         // 
         // JournalPageEditorView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Controls.Add(this.ribbonControl);
         this.Name = "JournalPageEditorView";
         this.Size = new System.Drawing.Size(940, 544);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tbTitle.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemBorderLineStyle1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemBorderLineWeight1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemFontEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichEditFontSizeEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbOrigin.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tagEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.richEditControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTokenEdit)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTitle)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.richEditBarController1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonNextPage)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonPreviousPage)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      protected UxRichEditControl uxRichEditControl;
      private DevExpress.XtraLayout.LayoutControlItem richEditControlItem;
      private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl;
      private DevExpress.XtraRichEdit.UI.UndoItem undoItem1;
      private DevExpress.XtraRichEdit.UI.RedoItem redoItem1;
      private DevExpress.XtraRichEdit.UI.FileNewItem fileNewItem1;
      private DevExpress.XtraRichEdit.UI.FileOpenItem fileOpenItem1;
      private DevExpress.XtraRichEdit.UI.FileSaveItem fileSaveItem1;
      private DevExpress.XtraRichEdit.UI.FileSaveAsItem fileSaveAsItem1;
      private DevExpress.XtraRichEdit.UI.QuickPrintItem quickPrintItem1;
      private DevExpress.XtraRichEdit.UI.PrintItem printItem1;
      private DevExpress.XtraRichEdit.UI.PrintPreviewItem printPreviewItem1;
      private DevExpress.XtraRichEdit.UI.InsertPageBreakItem2 insertPageBreakItem21;
      private DevExpress.XtraRichEdit.UI.InsertTableItem insertTableItem1;
      private DevExpress.XtraRichEdit.UI.InsertPictureItem insertPictureItem1;
      private DevExpress.XtraRichEdit.UI.InsertFloatingPictureItem insertFloatingPictureItem1;
      private DevExpress.XtraRichEdit.UI.InsertBookmarkItem insertBookmarkItem1;
      private DevExpress.XtraRichEdit.UI.InsertHyperlinkItem insertHyperlinkItem1;
      private DevExpress.XtraRichEdit.UI.EditPageHeaderItem editPageHeaderItem1;
      private DevExpress.XtraRichEdit.UI.EditPageFooterItem editPageFooterItem1;
      private DevExpress.XtraRichEdit.UI.InsertPageNumberItem insertPageNumberItem1;
      private DevExpress.XtraRichEdit.UI.InsertPageCountItem insertPageCountItem1;
      private DevExpress.XtraRichEdit.UI.InsertTextBoxItem insertTextBoxItem1;
      private DevExpress.XtraRichEdit.UI.InsertSymbolItem insertSymbolItem1;
      private DevExpress.XtraRichEdit.UI.ToggleFirstRowItem toggleFirstRowItem1;
      private DevExpress.XtraRichEdit.UI.ToggleLastRowItem toggleLastRowItem1;
      private DevExpress.XtraRichEdit.UI.ToggleBandedRowsItem toggleBandedRowsItem1;
      private DevExpress.XtraRichEdit.UI.ToggleFirstColumnItem toggleFirstColumnItem1;
      private DevExpress.XtraRichEdit.UI.ToggleLastColumnItem toggleLastColumnItem1;
      private DevExpress.XtraRichEdit.UI.ToggleBandedColumnsItem toggleBandedColumnsItem1;
      private DevExpress.XtraRichEdit.UI.GalleryChangeTableStyleItem galleryChangeTableStyleItem1;
      private DevExpress.XtraRichEdit.UI.ChangeTableBorderLineStyleItem changeTableBorderLineStyleItem1;
      private DevExpress.XtraRichEdit.Forms.Design.RepositoryItemBorderLineStyle repositoryItemBorderLineStyle1;
      private DevExpress.XtraRichEdit.UI.ChangeTableBorderLineWeightItem changeTableBorderLineWeightItem1;
      private DevExpress.XtraRichEdit.Forms.Design.RepositoryItemBorderLineWeight repositoryItemBorderLineWeight1;
      private DevExpress.XtraRichEdit.UI.ChangeTableBorderColorItem changeTableBorderColorItem1;
      private DevExpress.XtraRichEdit.UI.ChangeTableBordersItem changeTableBordersItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsBottomBorderItem toggleTableCellsBottomBorderItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsTopBorderItem toggleTableCellsTopBorderItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsLeftBorderItem toggleTableCellsLeftBorderItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsRightBorderItem toggleTableCellsRightBorderItem1;
      private DevExpress.XtraRichEdit.UI.ResetTableCellsAllBordersItem resetTableCellsAllBordersItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsAllBordersItem toggleTableCellsAllBordersItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsOutsideBorderItem toggleTableCellsOutsideBorderItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsInsideBorderItem toggleTableCellsInsideBorderItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsInsideHorizontalBorderItem toggleTableCellsInsideHorizontalBorderItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsInsideVerticalBorderItem toggleTableCellsInsideVerticalBorderItem1;
      private DevExpress.XtraRichEdit.UI.ToggleShowTableGridLinesItem toggleShowTableGridLinesItem1;
      private DevExpress.XtraRichEdit.UI.ChangeTableCellsShadingItem changeTableCellsShadingItem1;
      private DevExpress.XtraRichEdit.UI.SelectTableElementsItem selectTableElementsItem1;
      private DevExpress.XtraRichEdit.UI.SelectTableCellItem selectTableCellItem1;
      private DevExpress.XtraRichEdit.UI.SelectTableColumnItem selectTableColumnItem1;
      private DevExpress.XtraRichEdit.UI.SelectTableRowItem selectTableRowItem1;
      private DevExpress.XtraRichEdit.UI.SelectTableItem selectTableItem1;
      private DevExpress.XtraRichEdit.UI.ShowTablePropertiesFormItem showTablePropertiesFormItem1;
      private DevExpress.XtraRichEdit.UI.DeleteTableElementsItem deleteTableElementsItem1;
      private DevExpress.XtraRichEdit.UI.ShowDeleteTableCellsFormItem showDeleteTableCellsFormItem1;
      private DevExpress.XtraRichEdit.UI.DeleteTableColumnsItem deleteTableColumnsItem1;
      private DevExpress.XtraRichEdit.UI.DeleteTableRowsItem deleteTableRowsItem1;
      private DevExpress.XtraRichEdit.UI.DeleteTableItem deleteTableItem1;
      private DevExpress.XtraRichEdit.UI.InsertTableRowAboveItem insertTableRowAboveItem1;
      private DevExpress.XtraRichEdit.UI.InsertTableRowBelowItem insertTableRowBelowItem1;
      private DevExpress.XtraRichEdit.UI.InsertTableColumnToLeftItem insertTableColumnToLeftItem1;
      private DevExpress.XtraRichEdit.UI.InsertTableColumnToRightItem insertTableColumnToRightItem1;
      private DevExpress.XtraRichEdit.UI.MergeTableCellsItem mergeTableCellsItem1;
      private DevExpress.XtraRichEdit.UI.ShowSplitTableCellsForm showSplitTableCellsForm1;
      private DevExpress.XtraRichEdit.UI.SplitTableItem splitTableItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableAutoFitItem toggleTableAutoFitItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableAutoFitContentsItem toggleTableAutoFitContentsItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableAutoFitWindowItem toggleTableAutoFitWindowItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableFixedColumnWidthItem toggleTableFixedColumnWidthItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsTopLeftAlignmentItem toggleTableCellsTopLeftAlignmentItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsMiddleLeftAlignmentItem toggleTableCellsMiddleLeftAlignmentItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsBottomLeftAlignmentItem toggleTableCellsBottomLeftAlignmentItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsTopCenterAlignmentItem toggleTableCellsTopCenterAlignmentItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsMiddleCenterAlignmentItem toggleTableCellsMiddleCenterAlignmentItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsBottomCenterAlignmentItem toggleTableCellsBottomCenterAlignmentItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsTopRightAlignmentItem toggleTableCellsTopRightAlignmentItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsMiddleRightAlignmentItem toggleTableCellsMiddleRightAlignmentItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTableCellsBottomRightAlignmentItem toggleTableCellsBottomRightAlignmentItem1;
      private DevExpress.XtraRichEdit.UI.ShowTableOptionsFormItem showTableOptionsFormItem1;
      private DevExpress.XtraRichEdit.UI.TableToolsRibbonPageCategory tableToolsRibbonPageCategory1;
      private DevExpress.XtraRichEdit.UI.TableDesignRibbonPage tableDesignRibbonPage1;
      private DevExpress.XtraRichEdit.UI.TableStyleOptionsRibbonPageGroup tableStyleOptionsRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.TableStylesRibbonPageGroup tableStylesRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.TableDrawBordersRibbonPageGroup tableDrawBordersRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.TableLayoutRibbonPage tableLayoutRibbonPage1;
      private DevExpress.XtraRichEdit.UI.TableTableRibbonPageGroup tableTableRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.TableRowsAndColumnsRibbonPageGroup tableRowsAndColumnsRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.TableMergeRibbonPageGroup tableMergeRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.TableCellSizeRibbonPageGroup tableCellSizeRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.TableAlignmentRibbonPageGroup tableAlignmentRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.FileRibbonPage fileRibbonPage1;
      private DevExpress.XtraRichEdit.UI.CommonRibbonPageGroup commonRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.InsertRibbonPage insertRibbonPage1;
      private DevExpress.XtraRichEdit.UI.PagesRibbonPageGroup pagesRibbonPageGroup;
      private DevExpress.XtraRichEdit.UI.TablesRibbonPageGroup tablesRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.IllustrationsRibbonPageGroup illustrationsRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.LinksRibbonPageGroup linksRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.HeaderFooterRibbonPageGroup headerFooterRibbonPageGroup;
      private DevExpress.XtraRichEdit.UI.TextRibbonPageGroup textRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.SymbolsRibbonPageGroup symbolsRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.RichEditBarController richEditBarController1;
      private DevExpress.XtraRichEdit.UI.PasteItem pasteItem1;
      private DevExpress.XtraRichEdit.UI.CutItem cutItem1;
      private DevExpress.XtraRichEdit.UI.CopyItem copyItem1;
      private DevExpress.XtraRichEdit.UI.PasteSpecialItem pasteSpecialItem1;
      private DevExpress.XtraBars.BarButtonGroup barButtonGroup1;
      private DevExpress.XtraRichEdit.UI.ChangeFontNameItem changeFontNameItem1;
      private DevExpress.XtraEditors.Repository.RepositoryItemFontEdit repositoryItemFontEdit1;
      private DevExpress.XtraRichEdit.UI.ChangeFontSizeItem changeFontSizeItem1;
      private DevExpress.XtraRichEdit.Design.RepositoryItemRichEditFontSizeEdit repositoryItemRichEditFontSizeEdit1;
      private DevExpress.XtraRichEdit.UI.FontSizeIncreaseItem fontSizeIncreaseItem1;
      private DevExpress.XtraRichEdit.UI.FontSizeDecreaseItem fontSizeDecreaseItem1;
      private DevExpress.XtraBars.BarButtonGroup barButtonGroup2;
      private DevExpress.XtraRichEdit.UI.ToggleFontBoldItem toggleFontBoldItem1;
      private DevExpress.XtraRichEdit.UI.ToggleFontItalicItem toggleFontItalicItem1;
      private DevExpress.XtraRichEdit.UI.ToggleFontUnderlineItem toggleFontUnderlineItem1;
      private DevExpress.XtraRichEdit.UI.ToggleFontDoubleUnderlineItem toggleFontDoubleUnderlineItem1;
      private DevExpress.XtraRichEdit.UI.ToggleFontStrikeoutItem toggleFontStrikeoutItem1;
      private DevExpress.XtraRichEdit.UI.ToggleFontDoubleStrikeoutItem toggleFontDoubleStrikeoutItem1;
      private DevExpress.XtraRichEdit.UI.ToggleFontSuperscriptItem toggleFontSuperscriptItem1;
      private DevExpress.XtraRichEdit.UI.ToggleFontSubscriptItem toggleFontSubscriptItem1;
      private DevExpress.XtraBars.BarButtonGroup barButtonGroup3;
      private DevExpress.XtraRichEdit.UI.ChangeFontColorItem changeFontColorItem1;
      private DevExpress.XtraRichEdit.UI.ChangeFontBackColorItem changeFontBackColorItem1;
      private DevExpress.XtraRichEdit.UI.ChangeTextCaseItem changeTextCaseItem1;
      private DevExpress.XtraRichEdit.UI.MakeTextUpperCaseItem makeTextUpperCaseItem1;
      private DevExpress.XtraRichEdit.UI.MakeTextLowerCaseItem makeTextLowerCaseItem1;
      private DevExpress.XtraRichEdit.UI.CapitalizeEachWordCaseItem capitalizeEachWordCaseItem1;
      private DevExpress.XtraRichEdit.UI.ToggleTextCaseItem toggleTextCaseItem1;
      private DevExpress.XtraRichEdit.UI.ClearFormattingItem clearFormattingItem1;
      private DevExpress.XtraBars.BarButtonGroup barButtonGroup4;
      private DevExpress.XtraRichEdit.UI.ToggleBulletedListItem toggleBulletedListItem1;
      private DevExpress.XtraRichEdit.UI.ToggleNumberingListItem toggleNumberingListItem1;
      private DevExpress.XtraRichEdit.UI.ToggleMultiLevelListItem toggleMultiLevelListItem1;
      private DevExpress.XtraBars.BarButtonGroup barButtonGroup5;
      private DevExpress.XtraRichEdit.UI.DecreaseIndentItem decreaseIndentItem1;
      private DevExpress.XtraRichEdit.UI.IncreaseIndentItem increaseIndentItem1;
      private DevExpress.XtraRichEdit.UI.ToggleShowWhitespaceItem toggleShowWhitespaceItem1;
      private DevExpress.XtraBars.BarButtonGroup barButtonGroup6;
      private DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentLeftItem toggleParagraphAlignmentLeftItem1;
      private DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentCenterItem toggleParagraphAlignmentCenterItem1;
      private DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentRightItem toggleParagraphAlignmentRightItem1;
      private DevExpress.XtraRichEdit.UI.ToggleParagraphAlignmentJustifyItem toggleParagraphAlignmentJustifyItem1;
      private DevExpress.XtraBars.BarButtonGroup barButtonGroup7;
      private DevExpress.XtraRichEdit.UI.ChangeParagraphLineSpacingItem changeParagraphLineSpacingItem1;
      private DevExpress.XtraRichEdit.UI.SetSingleParagraphSpacingItem setSingleParagraphSpacingItem1;
      private DevExpress.XtraRichEdit.UI.SetSesquialteralParagraphSpacingItem setSesquialteralParagraphSpacingItem1;
      private DevExpress.XtraRichEdit.UI.SetDoubleParagraphSpacingItem setDoubleParagraphSpacingItem1;
      private DevExpress.XtraRichEdit.UI.ShowLineSpacingFormItem showLineSpacingFormItem1;
      private DevExpress.XtraRichEdit.UI.AddSpacingBeforeParagraphItem addSpacingBeforeParagraphItem1;
      private DevExpress.XtraRichEdit.UI.RemoveSpacingBeforeParagraphItem removeSpacingBeforeParagraphItem1;
      private DevExpress.XtraRichEdit.UI.AddSpacingAfterParagraphItem addSpacingAfterParagraphItem1;
      private DevExpress.XtraRichEdit.UI.RemoveSpacingAfterParagraphItem removeSpacingAfterParagraphItem1;
      private DevExpress.XtraRichEdit.UI.ChangeParagraphBackColorItem changeParagraphBackColorItem1;
      private DevExpress.XtraRichEdit.UI.GalleryChangeStyleItem galleryChangeStyleItem1;
      private DevExpress.XtraRichEdit.UI.FindItem findItem1;
      private DevExpress.XtraRichEdit.UI.ReplaceItem replaceItem1;
      private DevExpress.XtraRichEdit.UI.HomeRibbonPage homeRibbonPage1;
      private DevExpress.XtraRichEdit.UI.ClipboardRibbonPageGroup clipboardRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.FontRibbonPageGroup fontRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.ParagraphRibbonPageGroup paragraphRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.StylesRibbonPageGroup stylesRibbonPageGroup1;
      private DevExpress.XtraRichEdit.UI.EditingRibbonPageGroup editingRibbonPageGroup1;
      private TokenEdit tagEdit;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemTokenEdit;
      private ImageComboBoxEdit cbOrigin;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSource;
      private TextEdit tbTitle;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemTitle;
      private UxLayoutControl layoutControl1;
      private SimpleButton buttonNextPage;
      private SimpleButton buttonPreviousPage;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemButtonPreviousPage;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemButtonNextPage;
   }
}
