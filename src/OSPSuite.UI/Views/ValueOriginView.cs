using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views
{
   public partial class ValueOriginView : BaseUserControl, IValueOriginView
   {
      private readonly ScreenBinder<ValueOrigin> _screenBinder = new ScreenBinder<ValueOrigin>();
      private readonly IImageListRetriever _imageListRetriever;
      private MRUEditElementBinder<ValueOrigin> _descriptionElement;
      private IValueOriginPresenter _presenter;

      public ValueOriginView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _imageListRetriever = imageListRetriever;

         imageComboBoxValueOriginSource.SetImages(imageListRetriever);
         imageComboBoxValueOriginDeterminationMethod.SetImages(imageListRetriever);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _screenBinder.Bind(x => x.Source)
            .To(imageComboBoxValueOriginSource)
            .WithImages(x => _imageListRetriever.ImageIndex(x.Icon))
            .WithValues( x=> _presenter.AllValueOriginSources)
            .AndDisplays(x => x.Display);

         _screenBinder.Bind(x => x.Method)
            .To(imageComboBoxValueOriginDeterminationMethod)
            .WithImages(x => _imageListRetriever.ImageIndex(x.Icon))
            .WithValues( x=> _presenter.AllValueOriginDeterminationMethods)
            .AndDisplays(x => x.Display);

         _descriptionElement = _screenBinder.Bind(x => x.Description)
            .To(mruDescription);
      }

      public void AttachPresenter(IValueOriginPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ValueOrigin valueOrigin)
      {
         _screenBinder.BindToSource(valueOrigin);
         mruDescription.Focus();
         ActiveControl = mruDescription;
      }

      public void Save()
      {
         //Value may not be saved directly if the view is being closed WHILE the text is being edited.
         _descriptionElement.SetValueToSource(mruDescription.Text);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemValueOriginDescription.Text = Captions.ValueOriginDescription.FormatForLabel();
         layoutItemValueOriginSource.Text = Captions.ValueOriginSource.FormatForLabel();
         layoutItemValueOriginDeterminationMethod.Text = Captions.ValueOriginDeterminationMethod.FormatForLabel();
      }
   }
}