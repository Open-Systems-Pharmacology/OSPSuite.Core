using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public abstract class ParameterIdentificationMatrixAnalysisPresenter<TMatrix> : AbstractPresenter<IParameterIdentificationSingleRunAnalysisView, IParameterIdentificationSingleRunAnalysisPresenter>, IPresenterWithSettings where TMatrix : class, ISimulationAnalysis
   {
      protected readonly IPresentationSettingsTask _presentationSettingsTask;
      protected readonly IMatrixCalculator _matrixCalculator;
      protected DefaultPresentationSettings _settings;
      protected ParameterIdentification _parameterIdentification;
      private ParameterIdentificationRunResult _selectedRunResults;
      public string PresentationKey => PresenterConstants.PresenterKeys.ParameterIdentificationCorrelationCovarianceMatrixPresenter;
      private TMatrix _matrix;
      private readonly IParameterIdentificationMatrixPresenter _matrixPresenter;

      public IReadOnlyList<ParameterIdentificationRunResult> AllRunResults { get; private set; }

      protected ParameterIdentificationMatrixAnalysisPresenter(IParameterIdentificationSingleRunAnalysisView view, IParameterIdentificationMatrixPresenter matrixPresenter,
         IPresentationSettingsTask presentationSettingsTask, IMatrixCalculator matrixCalculator, ApplicationIcon icon, string defaultNotificationMessage) : base(view)
      {
         _presentationSettingsTask = presentationSettingsTask;
         _matrixCalculator = matrixCalculator;
         _matrixPresenter = matrixPresenter;
         AddSubPresenters(_matrixPresenter);
         View.ApplicationIcon = icon;
         matrixPresenter.DefaultNotificationMessage = defaultNotificationMessage;
      }

      public ISimulationAnalysis Analysis => _matrix;

      public void InitializeAnalysis(ISimulationAnalysis simulationAnalysis, IAnalysable analysable)
      {
         UpdateAnalysisBasedOn(analysable);

         _matrix = simulationAnalysis.DowncastTo<TMatrix>();
         _view.Caption = _matrix.Name;

         UpdateAnalysis();
      }

      public void LoadSettingsForSubject(IWithId subject)
      {
         _settings = _presentationSettingsTask.PresentationSettingsFor<DefaultPresentationSettings>(this, subject);
      }

      public void Clear()
      {
         _matrix = null;
      }

      protected virtual void UpdateAnalysis()
      {
         try
         {
            var matrix = CalculateMatrix();
            _matrixPresenter.Edit(matrix);
         }
         catch (MatrixCalculationException e)
         {
            _matrixPresenter.ShowCalculationError(e.FullMessage());
         }
      }

      protected abstract Matrix CalculateMatrix();

      public void UpdateAnalysisBasedOn(IAnalysable analysable)
      {
         _parameterIdentification = analysable.DowncastTo<ParameterIdentification>();

         AllRunResults = _parameterIdentification.Results.OrderBy(x => x.TotalError).ToList();
         _view.CanSelectRunResults = AllRunResults.Count > 1;

         if (!AllRunResults.Any()) return;

         SelectedRunResults = AllRunResults.First();

         _view.BindToSelectedRunResult();
      }

      public ParameterIdentificationRunResult SelectedRunResults
      {
         get { return _selectedRunResults; }
         set
         {
            _selectedRunResults = value;
            UpdateAnalysis();
         }
      }
   }
}