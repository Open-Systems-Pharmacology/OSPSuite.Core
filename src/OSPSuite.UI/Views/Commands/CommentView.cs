using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Presenters.Commands;
using OSPSuite.Presentation.Views.Commands;

namespace OSPSuite.UI.Views.Commands
{
   public partial class CommentView : BaseModalView, ICommentView
   {
      private readonly ScreenBinder<IHistoryItemDTO> _screenBinder;
      private ICommentPresenter _presenter;

      public CommentView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<IHistoryItemDTO>();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(historyItem => historyItem.Comment).To(tbComments);
         btnCancel.Click += (o, e) => _screenBinder.Reset();
      }

      public void BindTo(IHistoryItemDTO historyItemDTO)
      {
         _screenBinder.BindToSource(historyItemDTO);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         lblCommandDescription.AutoSizeMode = LabelAutoSizeMode.Vertical;
         layoutItemComments.Text = Captions.Comments;
         Icon = ApplicationIcons.Edit;
      }

      public string CommandDescription
      {
         set { lblCommandDescription.Text = value; }
      }

      public void AttachPresenter(ICommentPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}