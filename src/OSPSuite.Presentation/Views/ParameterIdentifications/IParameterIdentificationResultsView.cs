using System.Collections.Generic;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationResultsView : IView<IParameterIdentificationResultsPresenter>
   {
      void ShowResultsView(IView view);
      void HideResultsView();
   }

   public interface ISingleParameterIdentificationResultsView : IView<ISingleParameterIdentificationResultsPresenter>
   {
      void BindTo(ParameterIdentificationRunResultDTO resultDTO);
      void AddResultPropertiesView(IResizableView view);
   }

   public interface IMultipleParameterIdentificationResultsView : IView<IMultipleParameterIdentificationResultsPresenter>
   {
      void BindTo(IEnumerable<ParameterIdentificationRunResultDTO> allResultsDTO);
   }
}