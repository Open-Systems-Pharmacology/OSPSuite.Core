using System.Collections.Generic;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationFeedbackView : IView<IParameterIdentificationFeedbackPresenter>, IToggleableView
   {
      void NoFeedbackAvailable();
      void ShowFeedbackView(IView view);

      void BindToProperties();
      bool Visible { get; }
   }

   public interface IMultipleParameterIdentificationFeedbackView : IView<IMultipleParameterIdentificationFeedbackPresenter>
   {
      void RefreshData();
      void BindTo(IEnumerable<MultiOptimizationRunResultDTO> allRunResultDTO);
      void DeleteBinding();
   }
}