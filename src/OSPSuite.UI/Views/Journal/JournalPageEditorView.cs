using System.Drawing;
using System.Windows.Forms;
using DevExpress.Office.Utils;
using DevExpress.Utils;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Commands;
using DevExpress.XtraRichEdit.Forms;
using DevExpress.XtraRichEdit.Services;
using OSPSuite.Assets;
using OSPSuite.Core.Journal;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Settings;
using OSPSuite.Presentation.Views.Journal;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.Journal
{
   public partial class JournalPageEditorView : BaseUserControl, IJournalPageEditorView
   {
      // A decent default paragraph width for word wrap
      private const int PARAGRAPH_WIDTH = 8;

      // This appears to be the largest width that DevExpress can handle reliably. You can use larger values and the page width will increase
      // but some things will not behave correctly. In particular, the dialog boxes allowing adjustment of paragraph width and page width
      // begin to show negative numbers.
      private const int PAGE_WIDTH = 22;
      private readonly IImageListRetriever _imageListRetriever;
      private IJournalPageEditorPresenter _presenter;
      private readonly IJournalTask _journalTask;
      private readonly ScreenBinder<JournalPageDTO> _screenBinder;
      private JournalPageDTO _journalPageDTO;
      private TokenEditBinder<JournalPageDTO, string> _tokenBinder;
      private JournalSearch _journalSearch;
      private readonly IClipboardTask _clipboardTask;

      public JournalPageEditorView(IImageListRetriever imageListRetriever, IJournalTask journalTask, IClipboardTask clipboardTask)
      {
         InitializeComponent();
         _imageListRetriever = imageListRetriever;
         _journalTask = journalTask;
         _clipboardTask = clipboardTask;
         _screenBinder = new ScreenBinder<JournalPageDTO>();
      }

      protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
      {
         if (keyData.IsPressed(Keys.Control | Keys.S))
         {
            OnEvent(() => uxRichEditControl.CreateCommand(RichEditCommandId.FileSave).Execute());
            return true;
         }

         if (keyData.IsPressed(Keys.Control | Keys.P))
         {
            OnEvent(() => uxRichEditControl.CreateCommand(RichEditCommandId.Print).Execute());
            return true;
         }

         return base.ProcessCmdKey(ref msg, keyData);
      }

      private void tagEditOnValidateToken(object sender, TokenEditValidateTokenEventArgs tokenEditValidateTokenEventArgs)
      {
         tokenEditValidateTokenEventArgs.IsValid = _journalPageDTO.ValidateTag(tokenEditValidateTokenEventArgs.Description);
      }

      public void BindTo(JournalPageDTO journalPageDTO)
      {
         _journalPageDTO = journalPageDTO;
         if (journalPageDTO.Data != null)
            uxRichEditControl.OpenXmlBytes = journalPageDTO.Data;
         else
            createDocumentForUninitializedJournalPage(journalPageDTO);

         // colors from Word Header style
         // Paragraph styles are added after binding, because they belong to the document (and are not saved, when not used) 
         addParagraphStyle("Header 1", "Cambria", 14, true, Color.FromArgb(255, 54, 95, 145), 16);
         addParagraphStyle("Header 2", "Cambria", 13, true, Color.FromArgb(255, 79, 129, 189), 12);
         addParagraphStyle("Header 3", "Cambria", 11, true, Color.FromArgb(255, 79, 129, 189), 6);
         addCharacterStyle("Emphasized", "Normal", true, true, Color.FromArgb(255, 54, 95, 145));

         _screenBinder.BindToSource(journalPageDTO);

         //Ensure that the control is not marked as modified after binding. 
         uxRichEditControl.Modified = false;

         ActiveControl = uxRichEditControl;
      }

      public void ApplyUserSettingsToRichEdit(JournalPageEditorSettings settings)
      {
         uxRichEditControl.Options.TableOptions.GridLines = settings.ShowTableGridLines ? RichEditTableGridLinesVisibility.Visible : RichEditTableGridLinesVisibility.Hidden;
      }

      public bool GetGridLinesPreference()
      {
         return uxRichEditControl.Options.TableOptions.GridLines == RichEditTableGridLinesVisibility.Visible;
      }

      public bool HasChanges()
      {
         return workingJournalDTOModified() || richEditModified();
      }

      private bool richEditModified()
      {
         return uxRichEditControl.Modified;
      }

      private bool workingJournalDTOModified()
      {
         return _journalPageDTO != null && _journalPageDTO.Edited;
      }

      public void SaveDocument()
      {
         uxRichEditControl.CreateCommand(RichEditCommandId.FileSave).Execute();
      }

      private void createDocumentForUninitializedJournalPage(JournalPageDTO journalPage)
      {
         uxRichEditControl.CreateNewDocument(raiseDocumentClosing: true);

         setPageWidth();
         setDefaultParagraphWidth();

         _presenter.InitializeJournalPageContent(journalPage.JournalPage, uxRichEditControl.OpenXmlBytes);
      }

      private void setPageWidth()
      {
         uxRichEditControl.Document.Sections.Each(section => { section.Page.Width = Units.InchesToDocumentsF(PAGE_WIDTH); });
      }

      private void setDefaultParagraphWidth()
      {
         var paragraphs = uxRichEditControl.Document.BeginUpdateParagraphs(uxRichEditControl.Document.Range);
         paragraphs.RightIndent = Units.InchesToDocumentsF(PAGE_WIDTH - PARAGRAPH_WIDTH);
         uxRichEditControl.Document.EndUpdateParagraphs(paragraphs);
      }

      public void ShowSearch(JournalSearch journalSearch)
      {
         _journalSearch = journalSearch;
         uxRichEditControl.ShowSearchForm();
      }

      private void onSearchFormShowing(SearchFormShowingEventArgs e)
      {
         SearchTextForm searchTextForm;
         if (_journalSearch != null)
         {
            searchTextForm = new JournalSearchTextForm(e.ControllerParameters, _journalSearch);
            _journalSearch = null;
         }
         else
            searchTextForm = new SearchTextForm(e.ControllerParameters);

         e.DialogResult = searchTextForm.ShowDialog();
         e.Handled = true;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         uxRichEditControl.ReplaceService<IRichEditCommandFactoryService>(createCustomCommandFactoryService());
         uxRichEditControl.SearchFormShowing += (o, e) => OnEvent(onSearchFormShowing, e);

         _screenBinder.Bind(x => x.Title)
            .To(tbTitle)
            .Changing += () => OnEvent(changeTitle);

         _tokenBinder = _screenBinder
            .Bind(item => item.Tags)
            .To(tagEdit)
            .OnSelectedItemsChanged(tags => _presenter.TagsChanged(tags))
            .WithKnownTokens(workingJournalItem => _presenter.AllKnownTags);

         _screenBinder.Bind(x => x.Origin)
            .To(cbOrigin)
            .WithImages(x => _imageListRetriever.ImageIndex(x.Icon))
            .WithValues(dto => _presenter.AllOrigins)
            .AndDisplays(x => x.DisplayName)
            .Changed += () => OnEvent(originChanged);

         buttonPreviousPage.Click += (o, e) => OnEvent(_presenter.NavigateToPreviousPage);
         buttonNextPage.Click += (o, e) => OnEvent(_presenter.NavigateToNextPage);
      }

      private CustomRichEditCommandFactoryService createCustomCommandFactoryService()
      {
         return new CustomRichEditCommandFactoryService(uxRichEditControl, uxRichEditControl.GetService<IRichEditCommandFactoryService>(),
            _journalTask, _clipboardTask, () => _journalPageDTO);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();

         ribbonControl.Images = _imageListRetriever.AllImages32x32;

         uxRichEditControl.Options.Behavior.Open = DocumentCapability.Hidden;
         uxRichEditControl.Options.Behavior.CreateNew = DocumentCapability.Hidden;
         uxRichEditControl.Options.Behavior.SaveAs = DocumentCapability.Hidden;
         uxRichEditControl.Options.DocumentCapabilities.HeadersFooters = DocumentCapability.Hidden;
         uxRichEditControl.ActiveViewType = RichEditViewType.Draft;

         pagesRibbonPageGroup.Visible = false;
         headerFooterRibbonPageGroup.Visible = false;

         layoutItemTokenEdit.Text = Captions.TaggedWith.FormatForLabel();
         tagEdit.Initialize();
         tagEdit.InitializeSeparator(JournalPageDTO.Separator);
         tagEdit.ValidateToken += tagEditOnValidateToken;

         layoutItemTitle.Text = Captions.Journal.Title.FormatForLabel();
         layoutItemSource.Text = Captions.Journal.Source.FormatForLabel();

         cbOrigin.SetImages(_imageListRetriever);

         ribbonControl.ShowToolbarCustomizeItem = false;
         ribbonControl.ToolbarLocation = RibbonQuickAccessToolbarLocation.Hidden;
         ribbonControl.ShowApplicationButton = DefaultBoolean.False;

         buttonPreviousPage.InitWithImage(ApplicationIcons.Previous, imageLocation: ImageLocation.MiddleCenter, toolTip: ToolTips.Journal.NavigateToPreviousPage);
         buttonNextPage.InitWithImage(ApplicationIcons.Next, imageLocation: ImageLocation.MiddleCenter, toolTip: ToolTips.Journal.NavigateToNextPage);
         layoutItemButtonNextPage.AdjustButtonSizeWithImageOnly();
         layoutItemButtonPreviousPage.AdjustButtonSizeWithImageOnly();
      }

      private void addParagraphStyle(string name, string fontName, int fontSize, bool isBold, Color color, int spacingBefore)
      {
         var styles = uxRichEditControl.Document.ParagraphStyles;
         if (styles[name] == null)
         {
            var style = styles.CreateNew();
            style.Name = name;
            style.ForeColor = color;
            style.FontName = fontName;
            style.FontSize = fontSize;
            style.Bold = isBold;
            style.SpacingBefore = spacingBefore;
            style.NextStyle = styles["Normal"];
            styles.Add(style);
         }
      }

      private void addCharacterStyle(string name, string baseStyleName, bool isBold, bool isItalic, Color color)
      {
         var styles = uxRichEditControl.Document.ParagraphStyles;
         if (styles[name] == null)
         {
            var style = styles.CreateNew();
            style.Name = name;
            style.Parent = styles[baseStyleName];
            style.ForeColor = color;
            style.Bold = isBold;
            style.Italic = isItalic;
            styles.Add(style);
         }
      }

      private void changeTitle() => _presenter.TitleChanged(tbTitle.Text);

      private void originChanged() => _presenter.SourceChanged();

      public void EnableSaveButton() => uxRichEditControl.Modified = true;

      public void RefreshTags() => _tokenBinder.Update();

      public void DeleteBinding()
      {
         _screenBinder.DeleteBinding();
         uxRichEditControl.Modified = false;
         _journalPageDTO = null;
      }

      public void AttachPresenter(IJournalPageEditorPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}