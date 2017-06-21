using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Commands;
using OSPSuite.Presentation.Views.Commands;

namespace OSPSuite.UI.Views.Commands
{
   public partial class LabelView : BaseModalView, ILabelView
   {
      private readonly ScreenBinder<LabelDTO> _screenBinder;
      private ILabelPresenter _presenter;

      public LabelView()
      {
         InitializeComponent();
         layoutControl.AllowCustomization = false;
         _screenBinder = new ScreenBinder<LabelDTO>();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(labelDTO => labelDTO.Label).To(tbLabel);
         _screenBinder.Bind(labelDTO => labelDTO.Comment).To(tbComments);
         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Text = Captions.Commands.LabelViewCaption;
         Icon = ApplicationIcons.LabelAdd;
         layoutItemLabel.Text = Captions.Label.FormatForLabel();
         layoutItemComments.Text = Captions.Comments;
      }

      public void BindTo(LabelDTO labelDTO)
      {
         _screenBinder.BindToSource(labelDTO);
         SetButtonOKEnable();
      }

      public override bool HasError => _screenBinder.HasError;

      protected virtual void SetButtonOKEnable()
      {
         btnOk.Enabled = !_screenBinder.HasError;
      }

      public void AttachPresenter(ILabelPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}
