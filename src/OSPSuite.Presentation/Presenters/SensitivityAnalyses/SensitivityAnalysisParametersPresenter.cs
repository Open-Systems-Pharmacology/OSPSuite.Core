using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.SensitivityAnalyses;
using OSPSuite.Presentation.Mappers.SensitivityAnalyses;
using OSPSuite.Presentation.Views.SensitivityAnalyses;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.SensitivityAnalyses
{
   public interface ISensitivityAnalysisParametersPresenter : IPresenter<ISensitivityAnalysisParametersView>, IAbstractParameterSelectionPresenter, ISensitivityAnalysisPresenter
   {
      void ChangeName(SensitivityParameterDTO sensitivityParameterDTO, string newName);
      void RemoveSensitivityParameter(SensitivityParameterDTO sensitivityParameterDTO);
      void RemoveSelectedSensitivityParameters();
      void Refresh();
      void RemoveAllParameters();
   }

   public class SensitivityAnalysisParametersPresenter : AbstractParameterSelectionPresenter<ISensitivityAnalysisParametersView, ISensitivityAnalysisParametersPresenter>, ISensitivityAnalysisParametersPresenter
   {
      private readonly ISensitivityParameterFactory _sensitivityParameterFactory;
      private readonly ISensitivityParameterToSensitivityParameterDTOMapper _sensitivityParameterDTOMapper;
      private SensitivityAnalysis _sensitivityAnalysis;
      private readonly List<SensitivityParameterDTO> _allSensitivityParameterDTOs = new List<SensitivityParameterDTO>();
      private SensitivityParameter _sensitivityParameterForUpdates;
      private readonly ISensitivityAnalysisSetValuePresenter _sensitivityAnalysisSetRangePresenter;
      private readonly ISensitivityAnalysisTask _sensitivityAnalysisTask;
      private readonly ISensitivityAnalysisSetValuePresenter _sensitivityAnalysisSetNMaxPresenter;

      public SensitivityAnalysisParametersPresenter(
         ISensitivityAnalysisParametersView view,
         ISensitivityParameterFactory sensitivityParameterFactory,
         ISensitivityParameterToSensitivityParameterDTOMapper sensitivityParameterDTOMapper,
         ISensitivityAnalysisSetValuePresenter sensitivityAnalysisSetNMaxPresenter,
         ISensitivityAnalysisSetValuePresenter sensitivityAnalysisSetRangePresenter,
         ISensitivityAnalysisTask sensitivityAnalysisTask
      ) : base(view)
      {
         _sensitivityParameterFactory = sensitivityParameterFactory;
         _sensitivityParameterDTOMapper = sensitivityParameterDTOMapper;

         _sensitivityAnalysisSetRangePresenter = sensitivityAnalysisSetRangePresenter;
         _sensitivityAnalysisTask = sensitivityAnalysisTask;
         _sensitivityAnalysisSetNMaxPresenter = sensitivityAnalysisSetNMaxPresenter;

         _subPresenterManager.Add(sensitivityAnalysisSetNMaxPresenter);
         _subPresenterManager.Add(sensitivityAnalysisSetRangePresenter);

         _view.SetNMaxView(sensitivityAnalysisSetNMaxPresenter.BaseView);
         _view.SetRangeView(sensitivityAnalysisSetRangePresenter.BaseView);

         configureSetValuePresenters(sensitivityAnalysisSetNMaxPresenter, sensitivityAnalysisSetRangePresenter);
      }

      private void configureSetValuePresenters(ISensitivityAnalysisSetValuePresenter sensitivityAnalysisSetNMaxPresenter, ISensitivityAnalysisSetValuePresenter sensitivityAnalysisSetRangePresenter)
      {
         sensitivityAnalysisSetNMaxPresenter.ApplyAll = applyValueToAllNMax;
         sensitivityAnalysisSetNMaxPresenter.ApplySelection = applyValueToSelectedNMax;
         sensitivityAnalysisSetNMaxPresenter.ConfigureForUInt();

         sensitivityAnalysisSetRangePresenter.ApplyAll = applyValueToAllRange;
         sensitivityAnalysisSetRangePresenter.ApplySelection = applyValueToSelectedRange;
      }

      private void applyValueToSelectedRange(IParameterDTO parameterDTO)
      {
         selectedSensitivityParameters.Each(x => setRange(parameterDTO, x));
      }

      private static void setRange(IParameterDTO parameterDTO, SensitivityParameter sensitivityParameter)
      {
         setParameterValues(parameterDTO, sensitivityParameter.VariationRangeParameter);
      }

      private static void setParameterValues(IParameterDTO parameterDTO, IParameter parameter)
      {
         parameter.Value = parameter.ConvertToBaseUnit(parameterDTO.Value);
      }

      private void applyValueToAllRange(IParameterDTO parameterDTO)
      {
         allSensitivityParameters.Each(x => setRange(parameterDTO, x));
      }

      private IEnumerable<SensitivityParameter> allSensitivityParameters
      {
         get { return _allSensitivityParameterDTOs.Select(x => x.SensitivityParameter); }
      }

      private void applyValueToSelectedNMax(IParameterDTO parameterDTO)
      {
         selectedSensitivityParameters.Each(x => setNumberOfSteps(parameterDTO, x));
      }

      private static void setNumberOfSteps(IParameterDTO parameterDTO, SensitivityParameter sensitivityParameter)
      {
         setParameterValues(parameterDTO, sensitivityParameter.NumberOfStepsParameter);
      }

      private void applyValueToAllNMax(IParameterDTO parameterDTO)
      {
         allSensitivityParameters.Each(x => setNumberOfSteps(parameterDTO, x));
      }

      public void EditSensitivityAnalysis(SensitivityAnalysis sensitivityAnalysis)
      {
         _sensitivityAnalysis = sensitivityAnalysis;
         _sensitivityParameterForUpdates = _sensitivityParameterFactory.EmptySensitivityParameter();
         _sensitivityAnalysisSetNMaxPresenter.Edit(_sensitivityParameterForUpdates.NumberOfStepsParameter);
         _sensitivityAnalysisSetRangePresenter.Edit(_sensitivityParameterForUpdates.VariationRangeParameter);

         updateView();
      }

      public void ChangeName(SensitivityParameterDTO sensitivityParameterDTO, string newName)
      {
         _sensitivityAnalysisTask.UpdateSensitivityParameterName(_sensitivityAnalysis, sensitivityParameterDTO.SensitivityParameter, newName);
      }

      public void RemoveSensitivityParameter(SensitivityParameterDTO sensitivityParameterDTO)
      {
         removeSensitivityParameters(new[] {sensitivityParameterDTO.SensitivityParameter});
      }

      public void RemoveSelectedSensitivityParameters()
      {
         removeSensitivityParameters(selectedSensitivityParameters.ToList());
      }

      private IEnumerable<SensitivityParameter> selectedSensitivityParameters
      {
         get { return _view.SelectedParameters().Select(x => x.SensitivityParameter); }
      }

      public void Refresh()
      {
         updateView();
      }

      public void RemoveAllParameters()
      {
         removeSensitivityParameters(_sensitivityAnalysis.AllSensitivityParameters.ToList());
      }

      private void removeSensitivityParameters(IReadOnlyList<SensitivityParameter> parametersToRemove)
      {
         parametersToRemove.Each(x => _sensitivityAnalysis.RemoveSensitivityParameter(x));
         updateView();
         ViewChanged();
      }

      private void updateView()
      {
         _allSensitivityParameterDTOs.Clear();
         _allSensitivityParameterDTOs.AddRange(_sensitivityAnalysis.AllSensitivityParameters.MapAllUsing(_sensitivityParameterDTOMapper));
         _view.BindTo(_allSensitivityParameterDTOs);
      }

      public override void AddParameters(IReadOnlyList<ParameterSelection> parameters)
      {
         parameters.Select(parameterSelection => _sensitivityParameterFactory.CreateFor(parameterSelection, _sensitivityAnalysis))
            .Where(sensitivityParameter => sensitivityParameter != null)
            .Each(sensitivityParameter => _sensitivityAnalysis.AddSensitivityParameter(sensitivityParameter));

         updateView();
         ViewChanged();
      }
   }
}