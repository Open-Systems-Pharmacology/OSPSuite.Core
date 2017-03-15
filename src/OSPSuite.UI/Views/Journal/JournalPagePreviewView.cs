using System.Linq;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Journal;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Journal
{
   public partial class JournalPagePreviewView : BaseContainerUserControl, IJournalPagePreviewView
   {
      private readonly ScreenBinder<JournalPageDTO> _screenBinder;
      private IJournalPagePreviewPresenter _presenter;
      private JournalPageDTO _journalPageDTO;

      public JournalPagePreviewView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<JournalPageDTO>();
         tokenTags.Initialize();
         tokenTags.InitializeSeparator(JournalPageDTO.Separator);
         tokenTags.ValidateToken += (o, e) => validateToken(e);
      }

      private void validateToken(TokenEditValidateTokenEventArgs tokenEditValidateTokenEventArgs)
      {
         tokenEditValidateTokenEventArgs.IsValid = _journalPageDTO.ValidateTag(tokenEditValidateTokenEventArgs.Description);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(x => x.UpdatedAtBy).To(lblUpdatedAt);

         _screenBinder.Bind(x => x.Tags)
            .To(tokenTags)
            .OnSelectedItemsChanged(_presenter.UpdateTags)
            .WithKnownTokens(x => _presenter.AllKnownTags);
      }

      public void BindTo(JournalPageDTO journalPageDTO)
      {
         _journalPageDTO = journalPageDTO;
         _screenBinder.BindToSource(journalPageDTO);
         updateControlVisibility(relatedItemVisible: journalPageDTO.RelatedItems.Any(), tagVisible: true);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemUpdatedAt.Text = Captions.Journal.UpdatedAt.FormatForLabel();
         layoutItemTags.Text = Captions.Journal.Tags;
         layoutItemRelatedItems.Text = Captions.Journal.RelatedItems;
         DeleteBinding();
      }

      public void DeleteBinding()
      {
         lblUpdatedAt.Text = string.Empty;
         updateControlVisibility(relatedItemVisible: false, tagVisible: false);
      }

      public void AddRelatedItemsView(IView view)
      {
         AddViewTo(layoutItemRelatedItems, view);
      }

      private void updateControlVisibility(bool relatedItemVisible, bool tagVisible)
      {
         layoutItemRelatedItems.Visibility = LayoutVisibilityConvertor.FromBoolean(relatedItemVisible);
         layoutItemTags.Visibility = LayoutVisibilityConvertor.FromBoolean(tagVisible);
      }

      public void AttachPresenter(IJournalPagePreviewPresenter presenter)
      {
         _presenter = presenter;
      }

      protected override int TopicId=> HelpId.Tool_Journal;
   }
}