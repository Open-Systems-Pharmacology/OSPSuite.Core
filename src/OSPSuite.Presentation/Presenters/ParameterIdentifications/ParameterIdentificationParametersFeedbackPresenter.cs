using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services.ParameterIdentifications;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationParametersFeedbackPresenter : IPresenter<IParameterIdentificationParametersFeedbackView>, IParameterIdentificationRunFeedbackPresenter
   {
      void ExportParametersHistory();
   }

   public class ParameterIdentificationParametersFeedbackPresenter : AbstractPresenter<IParameterIdentificationParametersFeedbackView, IParameterIdentificationParametersFeedbackPresenter>, IParameterIdentificationParametersFeedbackPresenter
   {
      private readonly IParameterIdentificationExportTask _exportTask;
      private readonly Cache<string, ParameterFeedbackDTO> _parametersDTO = new Cache<string, ParameterFeedbackDTO>(x => x.Name);
      private readonly Cache<string, IRunPropertyDTO> _allProperties = new Cache<string, IRunPropertyDTO>(x => x.Name);
      private RunPropertyDTO<RunStatus> _runStatusProperty;
      private IReadOnlyCollection<IdentificationParameterHistory> _parametersHistory;
      private IReadOnlyList<float> _totalErrorHistory;

      public ParameterIdentificationParametersFeedbackPresenter(IParameterIdentificationParametersFeedbackView view, IParameterIdentificationExportTask exportTask) : base(view)
      {
         _exportTask = exportTask;
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         var errorFeedbackParameterDTO = createFeedbackDTOFor(Captions.ParameterIdentification.TotalError, Constants.Dimension.NO_DIMENSION);
         errorFeedbackParameterDTO.NeedsBoundaryCheck = false;
         _parametersDTO.Add(errorFeedbackParameterDTO);

         parameterIdentification.AllIdentificationParameters.Each(x => _parametersDTO.Add(createFeedbackDTOFor(x)));

         _runStatusProperty = new RunPropertyDTO<RunStatus>(Captions.ParameterIdentification.Status, RunStatus.Created, icon: RunStatus.Created.Icon);

         _allProperties.Add(_runStatusProperty);

         _view.BindTo(_parametersDTO, _allProperties);
      }

      public void ClearReferences()
      {
      }

      public void ResetFeedback()
      {
         _parametersDTO.Clear();
         _allProperties.Clear();
         _parametersHistory = null;
         updateExportParametersHistoryEnableState();
      }

      private void updateExportParametersHistoryEnableState()
      {
         _view.CanExportParametersHistory = (_parametersHistory != null);
      }

      public void UpdateFeedback(ParameterIdentificationRunState runState)
      {
         _parametersHistory = runState.ParametersHistory;
         _totalErrorHistory  = runState.ErrorHistory;
         _runStatusProperty.Value = runState.Status;
         _runStatusProperty.Icon = runState.Status.Icon;

         updateParameter(Captions.ParameterIdentification.TotalError, runState.BestResult.TotalError, runState.CurrentResult.TotalError);

         runState.BestResult.Values.Each((bestValue, i) =>
         {
            var currentValue = runState.CurrentResult.Values[i];
            updateParameter(bestValue.Name, bestValue.Value, currentValue.Value);
         });

         updateExportParametersHistoryEnableState();
         _view.RefreshData();
      }

      public async void ExportParametersHistory()
      {
         if (_parametersHistory == null)
            return;

         await _exportTask.SecureAwait(x => x.ExportParametersHistoryToExcel(_parametersHistory, _totalErrorHistory));
      }

      private void updateParameter(string name, double best, double current)
      {
         var dto = _parametersDTO[name];
         dto.Best.Value = best;
         dto.Current.Value = current;
      }

      private ParameterFeedbackDTO createFeedbackDTOFor(IdentificationParameter identificationParameter)
      {
         return createFeedbackDTOFor(identificationParameter.Name, identificationParameter.Dimension,
            identificationParameter.StartValueParameter.DisplayUnit, identificationParameter.StartValue, identificationParameter.MinValue, identificationParameter.MaxValue);
      }

      private ParameterFeedbackDTO createFeedbackDTOFor(string name, IDimension dimension, Unit displayUnit = null, double value = double.NaN, double min = double.NegativeInfinity, double max = double.PositiveInfinity)
      {
         return new ParameterFeedbackDTO
         {
            Name = name,
            Best = new ValueDTO {Dimension = dimension, DisplayUnit = displayUnit ?? dimension.DefaultUnit, Value = value},
            Current = new ValueDTO {Dimension = dimension, DisplayUnit = displayUnit ?? dimension.DefaultUnit, Value = value},
            MinValue = new ValueDTO {Dimension = dimension, DisplayUnit = displayUnit ?? dimension.DefaultUnit, Value = min},
            MaxValue = new ValueDTO {Dimension = dimension, DisplayUnit = displayUnit ?? dimension.DefaultUnit, Value = max},
         };
      }
   }
}