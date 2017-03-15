using OSPSuite.DataBinding.DevExpress.XtraGrid;
using DevExpress.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class SingleParameterIdentificationResultsView : BaseUserControl, ISingleParameterIdentificationResultsView
   {
      private ISingleParameterIdentificationResultsPresenter _presenter;
      private readonly GridViewBinder<OptimizedParameterDTO> _gridViewBinder;
      private readonly OptimizedParametersBinder _optimizedParametersBinder;


      public SingleParameterIdentificationResultsView(IImageListRetriever imageListRetriever, IToolTipCreator toolTipCreator)
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<OptimizedParameterDTO>(gridView);
         _optimizedParametersBinder = new OptimizedParametersBinder(imageListRetriever, toolTipCreator);
         gridView.AllowsFiltering = false;
         gridView.ShouldUseColorForDisabledCell = false;
      }

      public void AttachPresenter(ISingleParameterIdentificationResultsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ParameterIdentificationRunResultDTO resultDTO)
      {
         _gridViewBinder.BindToSource(resultDTO.OptimizedParameters);
         gridParameters.ToolTipController = new ToolTipController();
         gridParameters.ToolTipController.GetActiveObjectInfo += (o, e) => OnEvent(() => _optimizedParametersBinder.ShowTooltip(e, resultDTO.LegendImage));
      }

      public void AddResultPropertiesView(IResizableView view)
      {
         panelProperties.FillWith(view);
         view.HeightChanged += (o, e) => OnEvent(() => adjustHeight(view, e.Height));
      }

      private void adjustHeight(IResizableView view, int height)
      {
         layoutItemProperties.AdjustControlHeight(height);
         view.Repaint();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _optimizedParametersBinder.InitializeBinding(_gridViewBinder);
         btnTransferToSimulations.Click += (o, e) => OnEvent(_presenter.TransferToSimulation);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemOptimizedParameters.TextVisible = false;
         layoutGroupRunResultProperties.Text = Captions.ParameterIdentification.RunResultsProperties;
         layoutItemButtonTransfer.AdjustLargeButtonSize();
         btnTransferToSimulations.InitWithImage(ApplicationIcons.Commit, Captions.ParameterIdentification.TransferToSimulation);
      }
   }
}