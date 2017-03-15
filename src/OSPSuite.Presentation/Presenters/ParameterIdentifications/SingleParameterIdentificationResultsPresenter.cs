using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface ISingleParameterIdentificationResultsPresenter : IParameterIdentificationPresenter
   {
      void TransferToSimulation();
   }

   public class SingleParameterIdentificationResultsPresenter : AbstractCommandCollectorPresenter<ISingleParameterIdentificationResultsView, ISingleParameterIdentificationResultsPresenter>, ISingleParameterIdentificationResultsPresenter
   {
      private readonly IParameterIdentificationRunPropertiesPresenter _runPropertiesPresenter;
      private readonly IParameterIdentificationRunResultToRunResultDTOMapper _runResultDTOMapper;
      private readonly ITransferOptimizedParametersToSimulationsTask _transferParametersToSimulationsTask;
      private ParameterIdentificationRunResultDTO _resultDTO;
      private ParameterIdentification _parameterIdentification;
      private ParameterIdentificationRunResult _result;

      public SingleParameterIdentificationResultsPresenter(ISingleParameterIdentificationResultsView view, IParameterIdentificationRunPropertiesPresenter runPropertiesPresenter,
         IParameterIdentificationRunResultToRunResultDTOMapper runResultDTOMapper, ITransferOptimizedParametersToSimulationsTask transferParametersToSimulationsTask) : base(view)
      {
         _runPropertiesPresenter = runPropertiesPresenter;
         _runResultDTOMapper = runResultDTOMapper;
         _transferParametersToSimulationsTask = transferParametersToSimulationsTask;
         AddSubPresenters(_runPropertiesPresenter);
         _view.AddResultPropertiesView(_runPropertiesPresenter.View);
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         _parameterIdentification = parameterIdentification;
         //We expect one result when landing here.

         _result = parameterIdentification.Results[0];
         _resultDTO = _runResultDTOMapper.MapFrom(parameterIdentification, _result);
         _view.BindTo(_resultDTO);
         _runPropertiesPresenter.Edit(_result);
      }

      public void TransferToSimulation()
      {
         AddCommand(_transferParametersToSimulationsTask.TransferParametersFrom(_parameterIdentification, _result));
      }
   }
}