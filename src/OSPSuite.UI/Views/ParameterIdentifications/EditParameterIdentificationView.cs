using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class EditParameterIdentificationView : EditAnalyzableView, IEditParameterIdentificationView
   {
      public EditParameterIdentificationView(IShell shell, IImageListRetriever imageListRetriever) : base(shell, imageListRetriever)
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditParameterIdentificationPresenter presenter)
      {
         _presenter = presenter;
      }

      public override ApplicationIcon ApplicationIcon => ApplicationIcons.ParameterIdentification;
   }
}
