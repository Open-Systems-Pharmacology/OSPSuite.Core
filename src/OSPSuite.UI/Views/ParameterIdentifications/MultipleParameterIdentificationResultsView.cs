using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Formatters;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class MultipleParameterIdentificationResultsView : BaseUserControl, IMultipleParameterIdentificationResultsView
   {
      private readonly IImageListRetriever _imageListRetriever;
      private readonly IToolTipCreator _toolTipCreator;
      private IMultipleParameterIdentificationResultsPresenter _presenter;
      private readonly GridViewBinder<ParameterIdentificationRunResultDTO> _gridViewBinder;
      private readonly Cache<DevExpress.XtraGrid.Views.Base.BaseView, OptimizedParametersBinder> _optimizedParametersBinderCache = new Cache<DevExpress.XtraGrid.Views.Base.BaseView, OptimizedParametersBinder>();
      private readonly UxRepositoryItemButtonEdit _repositoryItemTransferToSimulations;
      private readonly RepositoryItemMemoEdit _repositoryItemDescription;
      private IGridViewColumn _colTransfer;
      private readonly TimeSpanFormatter _timeSpanFormatter;

      public MultipleParameterIdentificationResultsView(IImageListRetriever imageListRetriever, IToolTipCreator toolTipCreator)
      {
         _imageListRetriever = imageListRetriever;
         _toolTipCreator = toolTipCreator;
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<ParameterIdentificationRunResultDTO>(mainView);

         _repositoryItemTransferToSimulations = new UxRepositoryItemButtonImage(ApplicationIcons.Commit, Captions.ParameterIdentification.TransferToSimulation);
         _timeSpanFormatter = new TimeSpanFormatter();

         _repositoryItemDescription = new RepositoryItemMemoEdit
         {
            AutoHeight = true
         };

         gridControl.ViewRegistered += (o, e) => OnEvent(viewRegistered, o, e);
         gridControl.ViewRemoved += (o, e) => OnEvent(viewRemoved, o, e);

         var toolTipController = new ToolTipController();
         toolTipController.GetActiveObjectInfo += onToolTipControllerGetActiveObjectInfo;
         toolTipController.Initialize(_imageListRetriever);
         gridControl.ToolTipController = toolTipController;

         initializeMainView();

         initializeOptimizedParametersView();

         //Create a dummy column for the detail view to avoid auto binding to details
         optimizedParametersView.Columns.AddField("Dummy");
      }

      private void initializeOptimizedParametersView()
      {
         optimizedParametersView.AllowsFiltering = false;
         optimizedParametersView.SynchronizeClones = false;
         optimizedParametersView.ShouldUseColorForDisabledCell = false;
         optimizedParametersView.OptionsCustomization.AllowSort = true;
      }

      private void initializeMainView()
      {
         mainView.AllowsFiltering = false;
         mainView.ShouldUseColorForDisabledCell = false;
         mainView.MultiSelect = true;
         mainView.OptionsDetail.AllowExpandEmptyDetails = false;
         mainView.MasterRowGetChildList += mainViewMasterRowGetChildList;
         mainView.MasterRowGetRelationCount += masterRowGetRelationCount;
         mainView.MasterRowGetRelationName += mainViewMasterRowGetRelationName;
         mainView.MasterRowExpanded += masterRowExpanded;
         mainView.OptionsDetail.ShowDetailTabs = false;
         mainView.OptionsDetail.EnableDetailToolTip = false;
         mainView.OptionsView.ShowPreview = true;
         mainView.OptionsView.AutoCalcPreviewLineCount = true;
         mainView.OptionsCustomization.AllowSort = true;
         mainView.OptionsView.RowAutoHeight = true;
         mainView.ShowingEditor += (o, e) => OnEvent(showingEditor, e);
      }

      private void showingEditor(CancelEventArgs e)
      {
         //prevent editor from being shown except for tranfer column
         if (mainView.FocusedColumn == _colTransfer.XtraColumn)
            return;

         e.Cancel = true;
      }

      private void onToolTipControllerGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         var runResultDTO = _gridViewBinder.ElementAt(e);

         if (_optimizedParametersBinderCache.Any(x => x.ShowTooltip(e, runResultDTO?.LegendImage)))
            return;

         if (string.IsNullOrEmpty(runResultDTO?.Message)) return;

         var superToolTip = _toolTipCreator.CreateToolTip(runResultDTO.Message, image: runResultDTO.StatusIcon);
         e.Info = _toolTipCreator.ToolTipControlInfoFor(runResultDTO, superToolTip);
      }

      private void mainViewMasterRowGetRelationName(object sender, MasterRowGetRelationNameEventArgs e)
      {
         e.RelationName = "OptimizedParameters";
      }

      private void mainViewMasterRowGetChildList(object sender, MasterRowGetChildListEventArgs e)
      {
         var schema = _gridViewBinder.ElementAt(e.RowHandle);
         if (schema == null) return;

         e.ChildList = schema.OptimizedParameters;
      }

      private void masterRowGetRelationCount(object sender, MasterRowGetRelationCountEventArgs e)
      {
         e.RelationCount = 1;
      }

      private void viewRemoved(object sender, ViewOperationEventArgs e)
      {
         if (!_optimizedParametersBinderCache.Contains(e.View))
            return;
         var binder = _optimizedParametersBinderCache[e.View];
         binder.Dispose();
         _optimizedParametersBinderCache.Remove(e.View);
      }

      private void viewRegistered(object sender, ViewOperationEventArgs e)
      {
         registerBinderFor<OptimizedParameterDTO>(e.View, initializeOptimizedParameterDTOBinding);
      }

      private OptimizedParametersBinder initializeOptimizedParameterDTOBinding(GridViewBinder<OptimizedParameterDTO> gridViewBinder)
      {
         var optimizedParametersBinder = new OptimizedParametersBinder(_imageListRetriever, _toolTipCreator);
         optimizedParametersBinder.InitializeBinding(gridViewBinder);
         return optimizedParametersBinder;
      }

      private void registerBinderFor<T>(DevExpress.XtraGrid.Views.Base.BaseView view, Func<GridViewBinder<T>, OptimizedParametersBinder> initBinding) where T : class
      {
         var dataSource = view.DataSource as IEnumerable<T>;
         if (!_optimizedParametersBinderCache.Contains(view))
         {
            var gridView = view.DowncastTo<GridView>();
            var binder = new GridViewBinder<T>(gridView) {BindingMode = BindingMode.OneWay};
            _optimizedParametersBinderCache.Add(gridView, initBinding(binder));
         }

         var gridViewBinder = _optimizedParametersBinderCache[view].GridViewBinder.DowncastTo<GridViewBinder<T>>();
         gridViewBinder.BindToSource(dataSource);
      }

      private void masterRowExpanded(object sender, CustomMasterRowEventArgs e)
      {
         var masterView = (GridView) sender;
         var detailView = (GridView) masterView.GetDetailView(e.RowHandle, e.RelationIndex);

         detailView?.BestFitColumns();
      }

      public void AttachPresenter(IMultipleParameterIdentificationResultsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IEnumerable<ParameterIdentificationRunResultDTO> allResultsDTO)
      {
         _optimizedParametersBinderCache.DisposeAll();
         _gridViewBinder.BindToSource(allResultsDTO);
         gridControl.RefreshDataSource();
         mainView.BestFitColumns();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         bind(x => x.Index)
            .WithRepository(indexRepositoryFor)
            .WithCaption(Captions.ParameterIdentification.RunIndex)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_CHECK_BOX_WIDTH)
            .WithShowButton(ShowButtonModeEnum.ShowOnlyInEditor);

         bind(x => x.Description)
            .WithRepository(x => _repositoryItemDescription);

         bind(x => x.TotalError)
            .WithCaption(Captions.ParameterIdentification.TotalError);

         bind(x => x.NumberOfEvaluations)
            .WithCaption(Captions.ParameterIdentification.NumberOfEvaluations);

         bind(x => x.Duration)
            .WithFormat(x => _timeSpanFormatter)
            .WithCaption(Captions.ParameterIdentification.Duration);

         bind(x => x.Status)
            .WithCaption(Captions.ParameterIdentification.Status)
            .WithRepository(statusRepositoryFor)
            .AsReadOnly();

         _colTransfer = _gridViewBinder.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH)
            .WithRepository(x => _repositoryItemTransferToSimulations)
            .WithShowButton(ShowButtonModeEnum.ShowAlways);

         _repositoryItemTransferToSimulations.ButtonClick += (o, e) => OnEvent(transferToSimulation);
      }

      private RepositoryItem indexRepositoryFor(ParameterIdentificationRunResultDTO runResultDTO)
      {
         var indexRepositoryItem = new UxRepositoryItemImageComboBox(mainView, _imageListRetriever);
         return indexRepositoryItem.AddItem(runResultDTO.Index, runResultDTO.BoundaryCheckIcon);
      }

      private RepositoryItem statusRepositoryFor(ParameterIdentificationRunResultDTO runResultDTO)
      {
         var statusRepositoryItem = new UxRepositoryItemImageComboBox(mainView, _imageListRetriever);
         return statusRepositoryItem.AddItem(runResultDTO.Status, runResultDTO.StatusIcon);
      }

      private void transferToSimulation()
      {
         _presenter.TransferToSimulation(_gridViewBinder.FocusedElement);
      }

      private IGridViewAutoBindColumn<ParameterIdentificationRunResultDTO, T> bind<T>(Expression<Func<ParameterIdentificationRunResultDTO, T>> expression)
      {
         return _gridViewBinder.AutoBind(expression);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemResults.TextVisible = false;
      }
   }
}