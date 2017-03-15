using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Views.Comparisons;

namespace OSPSuite.Presentation.Presenters.Comparisons
{
   public interface IMainComparisonPresenter : IMainViewItemPresenter,
      IListener<ProjectClosedEvent>,
      IListener<StartComparisonEvent>
   {
      /// <summary>
      ///    Starts the comparison workflow between <paramref name="leftObject" />  and <paramref name="rightObject" />.
      /// </summary>
      /// <param name="leftObject">Left object of comparison</param>
      /// <param name="rightObject">Right object of comparison</param>
      /// <param name="runComparison">
      ///    If set to <c>true</c> the comparison will start immediatly before showing the view to the
      ///    user. Default is <c>true</c>
      /// </param>
      /// <param name="leftCaption">Optional caption for the left object</param>
      /// <param name="rightCaption">Optional caption for the right object</param>
      /// <param name="viewCaption">View caption</param>
      void CompareObjects(IObjectBase leftObject, IObjectBase rightObject, bool runComparison = true, string leftCaption = null, string rightCaption = null, string viewCaption = null);

      /// <summary>
      ///    Runs the comparison between the left and right object defined when starting the comparison workflow
      /// </summary>
      void RunComparison();

      /// <summary>
      ///    Exports the current comparison to excel
      /// </summary>
      void ExportToExcel();

      /// <summary>
      /// Returns <c>true</c> if the comparison can be started otherwise <c>false</c>
      /// </summary>
      bool CanCompare { get; }

      /// <summary>
      /// Clears the current comparison if one of the compared object is <paramref name="objectBase"/>
      /// </summary>
      void ClearComparisonIfComparing(IObjectBase objectBase);
   }

   public abstract class MainComparisonPresenter : AbstractCommandCollectorPresenter<IMainComparisonView, IMainComparisonPresenter>, IMainComparisonPresenter
   {
      private readonly IRegionResolver _regionResolver;
      private IRegion _region;
      private readonly IComparisonPresenter _comparisonPresenter;
      private readonly IDialogCreator _dialogCreator;
      private readonly IExportDataTableToExcelTask _exportToExcelTask;
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly RegionName _regionName;
      private IObjectBase _leftObject;
      private IObjectBase _rightObject;
      private string _leftCaption;
      private string _rightCaption;
      private bool _initialized;
      private readonly ComparerSettings _comparerSettings;
      private readonly IComparerSettingsPresenter _comparerSettingsPresenter;

      protected MainComparisonPresenter(IMainComparisonView view, IRegionResolver regionResolver, IComparisonPresenter comparisonPresenter, IComparerSettingsPresenter comparerSettingsPresenter,
         IPresentationUserSettings presentationUserSettings, IDialogCreator dialogCreator, IExportDataTableToExcelTask exportToExcelTask, IOSPSuiteExecutionContext executionContext,
         RegionName regionName)
         : base(view)
      {
         _regionResolver = regionResolver;
         _comparisonPresenter = comparisonPresenter;
         _comparerSettingsPresenter = comparerSettingsPresenter;
         _dialogCreator = dialogCreator;
         _exportToExcelTask = exportToExcelTask;
         _executionContext = executionContext;
         _regionName = regionName;
         AddSubPresenters(_comparisonPresenter, comparerSettingsPresenter);
         view.AddSettingsView(comparerSettingsPresenter.View);
         view.AddComparisonView(_comparisonPresenter.View);
         view.SettingsVisible = false;
         _comparerSettings = presentationUserSettings.ComparerSettings;
         comparerSettingsPresenter.Edit(_comparerSettings);
         updateButtons();
      }

      public void ToggleVisibility()
      {
         _region.ToggleVisibility();
      }

      public override void Initialize()
      {
         if (_initialized) return;
         _initialized = true;
         _region = _regionResolver.RegionWithName(_regionName);
         _region.Add(_view);
      }

      public void Handle(ProjectClosedEvent eventToHandle)
      {
         ClearComparison();
      }

      private void updateButtons()
      {
         _view.UpdateButtonsEnableState(CanCompare);
      }

      public void CompareObjects(IObjectBase leftObject, IObjectBase rightObject, bool runComparison = true, string leftCaption = null, string rightCaption = null, string viewCaption = null)
      {
         _leftObject = leftObject;
         _rightObject = rightObject;
         _leftCaption = leftCaption ?? Captions.Comparisons.Left;
         _rightCaption = rightCaption ?? Captions.Comparisons.Right;
         _view.Caption = viewCaption ?? Captions.Comparisons.ComparisonTitle(_executionContext.TypeFor(leftObject));
         updateButtons();

         _executionContext.Load(leftObject);
         _executionContext.Load(rightObject);

         if (runComparison)
            RunComparison();
      }

      public void RunComparison()
      {
         _comparerSettingsPresenter.SaveChanges();
         _comparisonPresenter.StartComparison(_leftObject, _rightObject, _leftCaption, _rightCaption, _comparerSettings);
      }

      public void ExportToExcel()
      {
         var filePath = _dialogCreator.AskForFileToSave(Captions.ExportChartToExcel, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.REPORT);
         if (string.IsNullOrEmpty(filePath))
            return;

         var dataTable = _comparisonPresenter.ComparisonAsTable();
         _exportToExcelTask.ExportDataTableToExcel(dataTable, filePath, openExcel: true);
      }

      public bool CanCompare => _leftObject != null && _rightObject != null;

      public void Handle(StartComparisonEvent eventToHandle)
      {
         CompareObjects(eventToHandle.LeftObject, eventToHandle.RightObject, eventToHandle.RunComparison, eventToHandle.LeftCaption, eventToHandle.RightCaption);
         _region.Show();
      }

      protected virtual void ClearComparison()
      {
         _leftObject = null;
         _rightCaption = null;
         updateButtons();
         _comparisonPresenter.Clear();
      }

      public virtual void ClearComparisonIfComparing(IObjectBase objectBase)
      {
         if (!CanCompare)
            return;

         if (Equals(_leftObject, objectBase) || Equals(_rightObject, objectBase)) 
            ClearComparison();
      }
   }
}