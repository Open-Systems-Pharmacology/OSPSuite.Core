using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IMultipleParameterIdentificationResultsPresenter : IParameterIdentificationPresenter, ICommandCollectorPresenter
   {
      void TransferToSimulation(ParameterIdentificationRunResultDTO runResultDTO);
   }

   public class MultipleParameterIdentificationResultsPresenter : AbstractCommandCollectorPresenter<IMultipleParameterIdentificationResultsView, IMultipleParameterIdentificationResultsPresenter>, IMultipleParameterIdentificationResultsPresenter
   {
      private readonly IParameterIdentificationRunResultToRunResultDTOMapper _runResultDTOMapper;
      private readonly ITransferOptimizedParametersToSimulationsTask _transferOptimizedParametersToSimulationsTask;
      private readonly List<ParameterIdentificationRunResultDTO> _allResultsDTO = new List<ParameterIdentificationRunResultDTO>();
      private ParameterIdentification _parameterIdentification;

      public MultipleParameterIdentificationResultsPresenter(IMultipleParameterIdentificationResultsView view,
         IParameterIdentificationRunResultToRunResultDTOMapper runResultDTOMapper, ITransferOptimizedParametersToSimulationsTask transferOptimizedParametersToSimulationsTask) : base(view)
      {
         _runResultDTOMapper = runResultDTOMapper;
         _transferOptimizedParametersToSimulationsTask = transferOptimizedParametersToSimulationsTask;
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         _parameterIdentification = parameterIdentification;
         _allResultsDTO.Clear();
         parameterIdentification.Results.Each(x => { _allResultsDTO.Add(_runResultDTOMapper.MapFrom(parameterIdentification, x)); });

         _view.BindTo(_allResultsDTO);
      }

      public void TransferToSimulation(ParameterIdentificationRunResultDTO runResultDTO)
      {
         AddCommand(_transferOptimizedParametersToSimulationsTask.TransferParametersFrom(_parameterIdentification, runResultDTO.RunResult));
      }
   }
}