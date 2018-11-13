using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.SensitivityAnalyses;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.SensitivityAnalyses
{
   public partial class EditSensitivityAnalysisView : EditAnalyzableView, IEditSensitivityAnalysisView
   {
      public EditSensitivityAnalysisView(IShell shell, IImageListRetriever imageListRetriever) : base(shell, imageListRetriever)
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditSensitivityAnalysisPresenter presenter)
      {
         _presenter = presenter;
      }

      public override ApplicationIcon ApplicationIcon => ApplicationIcons.SensitivityAnalysis;
   }
}
