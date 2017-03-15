using System;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Chart.SensitivityAnalyses;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presenters
{
   public abstract class SimulationAnalysisPresenterFactory : ISimulationAnalysisPresenterFactory
   {
      private readonly IContainer _container;

      protected SimulationAnalysisPresenterFactory(IContainer container)
      {
         _container = container;
      }

      public ISimulationAnalysisPresenter PresenterFor(ISimulationAnalysis simulationAnalysis)
      {
         if (simulationAnalysis.IsAnImplementationOf<ParameterIdentificationTimeProfileChart>())
            return _container.Resolve<IParameterIdentificationTimeProfileChartPresenter>();

         if (simulationAnalysis.IsAnImplementationOf<ParameterIdentificationResidualVsTimeChart>())
            return _container.Resolve<IParameterIdentificationResidualVsTimeChartPresenter>();

         if (simulationAnalysis.IsAnImplementationOf<ParameterIdentificationResidualHistogram>())
            return _container.Resolve<IParameterIdentificationResidualHistogramPresenter>();

         if (simulationAnalysis.IsAnImplementationOf<ParameterIdentificationPredictedVsObservedChart>())
            return _container.Resolve<IParameterIdentificationPredictedVsObservedChartPresenter>();

         if (simulationAnalysis.IsAnImplementationOf<ParameterIdentificationCorrelationMatrix>())
            return _container.Resolve<IParameterIdentificationCorrelationAnalysisPresenter>();

         if (simulationAnalysis.IsAnImplementationOf<ParameterIdentificationCovarianceMatrix>())
            return _container.Resolve<IParameterIdentificationCovarianceAnalysisPresenter>();

         if (simulationAnalysis.IsAnImplementationOf<ParameterIdentificationTimeProfileConfidenceIntervalChart>())
            return _container.Resolve<IParameterIdentificationTimeProfileConfidenceIntervalChartPresenter>();

         if (simulationAnalysis.IsAnImplementationOf<ParameterIdentificationTimeProfilePredictionIntervalChart>())
            return _container.Resolve<IParameterIdentificationTimeProfilePredictionIntervalChartPresenter>();

         if (simulationAnalysis.IsAnImplementationOf<ParameterIdentificationTimeProfileVPCIntervalChart>())
            return _container.Resolve<IParameterIdentificationTimeProfileVPCIntervalChartPresenter>();

         if (simulationAnalysis.IsAnImplementationOf<ParameterIdentificationTimeProfileConfidenceIntervalChart>())
            return _container.Resolve<IParameterIdentificationTimeProfileConfidenceIntervalChartPresenter>();

         if (simulationAnalysis.IsAnImplementationOf<SensitivityAnalysisPKParameterAnalysis>())
            return _container.Resolve<ISensitivityAnalysisPKParameterAnalysisPresenter>();


         var specificPresenter = PresenterFor(simulationAnalysis, _container);
         if (specificPresenter != null)
            return specificPresenter;

         throw new ArgumentException(Error.CannotCreateChartPresenterForChart(simulationAnalysis.GetType()));
      }

      protected abstract ISimulationAnalysisPresenter PresenterFor(ISimulationAnalysis simulationAnalysis, IContainer container);
   }
}