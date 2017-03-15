using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IEditAnalyzablePresenter :
      IListener<SimulationAnalysisCreatedEvent>,
      IListener<SimulationResultsUpdatedEvent>,
      IListener<SimulationStatusChangedEvent>,
      IPresenterWithContextMenu<ISimulationAnalysisPresenter>, IPresenterWithSettings

   {
      void RemoveAnalysis(ISimulationAnalysisPresenter simulationAnalysisPresenter);
      void RemoveAllAnalyses();
      void SetSelectedTabIndex(int tabIndex);
      void CloneAnalysis(ISimulationAnalysis analysis);
   }

   public interface IEditAnalyzablePresenter<TAnalyzable> : ISingleStartPresenter<TAnalyzable>, IEditAnalyzablePresenter where TAnalyzable : IAnalysable
   {
   }

   public abstract class EditAnalyzablePresenter<TView, TPresenter, TAnalyzable, TSubPresenter> : SingleStartContainerPresenter<TView, TPresenter, TAnalyzable, TSubPresenter>, IEditAnalyzablePresenter<TAnalyzable>
      where TAnalyzable : IAnalysable, IWithId
      where TView : IView<TPresenter>, IEditAnalyzableView
      where TPresenter : IPresenter
      where TSubPresenter : ISubPresenter

   {
      private readonly ISimulationAnalysisPresenterFactory _simulationAnalysisPresenterFactory;
      private readonly ISimulationAnalysisPresenterContextMenuFactory _contextMenuFactory;
      private readonly IPresentationSettingsTask _presentationSettingsTask;
      private readonly ISimulationAnalysisCreator _simulationAnalysisCreator;
      protected TAnalyzable Analyzable { get; set; }
      private readonly IList<ISimulationAnalysisPresenter> _allAnalysisPresenters = new List<ISimulationAnalysisPresenter>();
      private TabbedPresenterSettings _settings;

      protected EditAnalyzablePresenter(TView view, ISubPresenterItemManager<TSubPresenter> subPresenterItemManager, IReadOnlyList<ISubPresenterItem> subPresenterItems,
         ISimulationAnalysisPresenterFactory simulationAnalysisPresenterFactory,
         ISimulationAnalysisPresenterContextMenuFactory contextMenuFactory,
         IPresentationSettingsTask presentationSettingsTask, ISimulationAnalysisCreator simulationAnalysisCreator)
         : base(view, subPresenterItemManager, subPresenterItems)
      {
         _simulationAnalysisPresenterFactory = simulationAnalysisPresenterFactory;
         _contextMenuFactory = contextMenuFactory;
         _presentationSettingsTask = presentationSettingsTask;
         _simulationAnalysisCreator = simulationAnalysisCreator;
         _settings = new TabbedPresenterSettings();
      }

      public override object Subject => Analyzable;

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _allAnalysisPresenters.Each(x => x.Clear());
         _allAnalysisPresenters.Each(x => x.ReleaseFrom(eventPublisher));
         _allAnalysisPresenters.Clear();
      }

      public override void Edit(TAnalyzable analyzable)
      {
         if (!Equals(analyzable, Analyzable))
         {
            Analyzable = analyzable;
            InitializeSubPresentersWith(analyzable);
            //plot are just loaded=> do not load from template
            Analyzable.Analyses.Each(createAnalysis);
         }
         LoadSettingsForSubject(analyzable);
         UpdateCaption();
         _view.Display();
      }

      protected virtual void InitializeSubPresentersWith(TAnalyzable analyzable)
      {
         //nothing to do here
      }

      public void RemoveAnalysis(ISimulationAnalysisPresenter simulationAnalysisPresenter)
      {
         simulationAnalysisPresenter.ReleaseFrom(_subPresenterItemManager.EventPublisher);
         _allAnalysisPresenters.Remove(simulationAnalysisPresenter);
         Analyzable.RemoveAnalysis(simulationAnalysisPresenter.Analysis);
         View.RemoveAnalysis(simulationAnalysisPresenter);
         simulationAnalysisPresenter.Clear();
      }

      public void RemoveAllAnalyses()
      {
         _allAnalysisPresenters.ToList().Each(RemoveAnalysis);
      }

      public void SetSelectedTabIndex(int tabIndex)
      {
         _settings.SelectedTabIndex = tabIndex;
      }

      public void CloneAnalysis(ISimulationAnalysis analysis)
      {
         var newAnalysis = _simulationAnalysisCreator.CreateAnalysisBasedOn(analysis);
         _simulationAnalysisCreator.AddSimulationAnalysisTo(Analyzable, newAnalysis);
      }

      public void Handle(SimulationAnalysisCreatedEvent eventToHandle)
      {
         if (!Equals(eventToHandle.Analysable, Analyzable))
            return;

         //new plot created=>load from template
         createAnalysis(eventToHandle.SimulationAnalysis);
      }

      private void createAnalysis(ISimulationAnalysis simulationAnalysis)
      {
         var analysisPresenter = _simulationAnalysisPresenterFactory.PresenterFor(simulationAnalysis);

         _allAnalysisPresenters.Add(analysisPresenter);

         analysisPresenter.InitializeAnalysis(simulationAnalysis, Analyzable);

         analysisPresenter.LoadSettingsForSubject(simulationAnalysis);

         View.AddAnalysis(analysisPresenter);

         UpdateTrafficLightFor(analysisPresenter);
      }

      public void Handle(SimulationResultsUpdatedEvent eventToHandle)
      {
         if (!CanHandle(eventToHandle))
            return;

         UpdateResultsInCharts();
         updateAllTrafficLights();
      }

      protected void UpdateResultsInCharts()
      {
         _allAnalysisPresenters.Each(presenter => presenter.UpdateAnalysisBasedOn(Analyzable));
         updateAllTrafficLights();
      }

      protected abstract bool CanHandle(AnalysableEvent analysableEvent);

      private ApplicationIcon analysisIconFor(ISimulationAnalysisPresenter simulationAnalysisPresenter)
      {
         return simulationAnalysisPresenter.BaseView.ApplicationIcon;
      }

      public void Handle(SimulationStatusChangedEvent eventToHandle)
      {
         if (!CanHandle(eventToHandle))
            return;

         updateAllTrafficLights();
      }

      private void updateAllTrafficLights()
      {
         _allAnalysisPresenters.Each(UpdateTrafficLightFor);
      }

      protected virtual void UpdateTrafficLightFor(ISimulationAnalysisPresenter simulationAnalysisPresenter)
      {
         var icon = analysisIconFor(simulationAnalysisPresenter);
         View.UpdateTrafficLightFor(simulationAnalysisPresenter, Analyzable.HasUpToDateResults ? ApplicationIcons.GreenOverlayFor(icon) : ApplicationIcons.RedOverlayFor(icon));
      }

      public void ShowContextMenu(ISimulationAnalysisPresenter analysisPresenter, Point popupLocation)
      {
         if (analysisPresenter == null) return;
         var contextMenu = _contextMenuFactory.CreateFor(analysisPresenter, this);
         contextMenu.Show(_view, popupLocation);
      }

      public void LoadSettingsForSubject(IWithId subject)
      {
         _settings = _presentationSettingsTask.PresentationSettingsFor<TabbedPresenterSettings>(this, subject);
         setSelectedTab(_settings.SelectedTabIndex);
      }

      private void setSelectedTab(int selectedTabIndex)
      {
         _view.SelectTabByIndex(selectedTabIndex);
      }

      public abstract string PresentationKey { get; }
   }
}