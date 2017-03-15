using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationConfigurationPresenter : IParameterIdentificationItemPresenter
   {
      IEnumerable<LLOQMode> AllLLOQModes { get; }
      IEnumerable<RemoveLLOQMode> AllRemoveLLOQModes { get; }
      IParameterIdentificationRunModePresenter RunModePresenter { get; set; }
      IReadOnlyList<IOptimizationAlgorithm> Algorithms { get; }
      void ChangeOptimizationAlgorithm(IOptimizationAlgorithm newValue);
      IEnumerable<IParameterIdentificationRunModePresenter> Options();
      void SimulationAdded(ISimulation simulation);
      void SimulationRemoved(ISimulation simulation);
   }

   public class ParameterIdentificationConfigurationPresenter : AbstractSubPresenter<IParameterIdentificationConfigurationView, IParameterIdentificationConfigurationPresenter>, IParameterIdentificationConfigurationPresenter
   {
      private readonly IExtendedPropertiesPresenter _algorithmOptionsPresenter;
      private readonly IParameterIdentificationConfigurationToParameterIdentificationConfigurationDTOMapper _configurationToConfigurationDTOMapper;
      private readonly IOptimizationAlgorithmToOptimizationAlgorithmPropertiesMapper _optimizationAlgorithmToAlgorithmMapper;
      private ParameterIdentification _parameterIdentification;
      private readonly List<IParameterIdentificationRunModePresenter> _allRunModePresenters;
      private IParameterIdentificationRunModePresenter _runModePresenter;
      private ParameterIdentificationConfiguration _configuration;
      public IReadOnlyList<IOptimizationAlgorithm> Algorithms { get; }

      public ParameterIdentificationConfigurationPresenter(IParameterIdentificationConfigurationView view, IExtendedPropertiesPresenter algorithmOptionsPresenter,
         IOptimizationAlgorithmRepository optimizationAlgorithmRepository,
         IParameterIdentificationConfigurationToParameterIdentificationConfigurationDTOMapper configurationToConfigurationDTOMapper,
         IOptimizationAlgorithmToOptimizationAlgorithmPropertiesMapper optimizationAlgorithmToAlgorithmMapper,
         IStandardParameterIdentificationRunModePresenter standardRunModePresenter,
         IMultipleParameterIdentificationRunModePresenter multipleRunModePresenter,
         ICategorialParameterIdentificationRunModePresenter categorialRunModePresenter) : base(view)
      {
         _algorithmOptionsPresenter = algorithmOptionsPresenter;
         _configurationToConfigurationDTOMapper = configurationToConfigurationDTOMapper;
         _optimizationAlgorithmToAlgorithmMapper = optimizationAlgorithmToAlgorithmMapper;
         AddSubPresenters(_algorithmOptionsPresenter, standardRunModePresenter, multipleRunModePresenter, categorialRunModePresenter);

         _view.AddAlgorithmOptionsView(_algorithmOptionsPresenter.View);
         Algorithms = optimizationAlgorithmRepository.All().ToList();

         _allRunModePresenters = new List<IParameterIdentificationRunModePresenter> { standardRunModePresenter, multipleRunModePresenter };

         if (categorialRunModePresenter.HasCategories)
            _allRunModePresenters.Add(categorialRunModePresenter);
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         _parameterIdentification = parameterIdentification;
         _configuration = _parameterIdentification.Configuration;

         //Sequence is important here :Set default algorithm if required and then create the DTO
         setDefaultAlgorithm();
         var parameterIdentificationConfigurationDTO = _configurationToConfigurationDTOMapper.MapFrom(_configuration, Algorithms);

         editAlgorithmOptions();
         editParameterIdentificationOptions();
         _view.BindTo(parameterIdentificationConfigurationDTO);
      }

      private void setDefaultAlgorithm()
      {
         if (_configuration.AlgorithmIsDefined)
            return;

         updateOptimizationAlgorithm(Algorithms.Find(x => Equals(x.Name, Constants.OptimizationAlgorithm.DEFAULT)));
      }

      private void editParameterIdentificationOptions()
      {
         _runModePresenter = _allRunModePresenters.First(presenter => presenter.CanEdit(_parameterIdentification));

         // Sequence is important here. The presenter must have his edited option changed before you can call ChangeOptimizationOptions
         _runModePresenter.Edit(_parameterIdentification);
         changeRunModePresenter(_runModePresenter);
      }

      private void editAlgorithmOptions()
      {
         _algorithmOptionsPresenter.Edit(_configuration.AlgorithmProperties);
      }

      public IEnumerable<LLOQMode> AllLLOQModes => LLOQModes.AllModes;

      public IEnumerable<RemoveLLOQMode> AllRemoveLLOQModes => RemoveLLOQModes.AllUsages;

      public IParameterIdentificationRunModePresenter RunModePresenter
      {
         get { return _runModePresenter; }
         set
         {
            changeRunModePresenter(value);
            _runModePresenter = value;
         }
      }

      public void ChangeOptimizationAlgorithm(IOptimizationAlgorithm optimizationAlgorithm)
      {
         updateOptimizationAlgorithm(optimizationAlgorithm);
         editAlgorithmOptions();
      }

      private void updateOptimizationAlgorithm(IOptimizationAlgorithm optimizationAlgorithm)
      {
         _configuration.AlgorithmProperties = _optimizationAlgorithmToAlgorithmMapper.MapFrom(optimizationAlgorithm);
      }

      private void changeRunModePresenter(IParameterIdentificationRunModePresenter presenter)
      {
         _parameterIdentification.Configuration.RunMode = presenter.RunMode;
         presenter.Edit(_parameterIdentification);
         _view.UpdateOptimizationsView(presenter.BaseView);
      }

      public IEnumerable<IParameterIdentificationRunModePresenter> Options()
      {
         return _allRunModePresenters;
      }

      public void SimulationAdded(ISimulation simulation)
      {
         RunModePresenter.Refresh();
      }

      public void SimulationRemoved(ISimulation simulation)
      {
         RunModePresenter.Refresh();
      }
   }
}