using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors.Repository;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Formatters;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Format;

namespace OSPSuite.UI.Views
{
   public partial class SimulationParametersView : BaseUserControl, ISimulationParametersView
   {
      private ISimulationParametersPresenter _presenter;
      private readonly PathElementsBinder<SimulationParameterSelectionDTO> _pathElementsBinder;
      private readonly GridViewBinder<SimulationParameterSelectionDTO> _gridViewBinder;
      private readonly ScreenBinder<ISimulationParametersPresenter> _screenBinder;
      private readonly IFormatter<bool> _favoriteFormatter;
      private readonly RepositoryItem _textRepository;

      public SimulationParametersView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<SimulationParameterSelectionDTO>(gridView);
         _pathElementsBinder = new PathElementsBinder<SimulationParameterSelectionDTO>(imageListRetriever);
         gridView.AllowsFiltering = true;
         gridView.MultiSelect = true;
         gridView.ShouldUseColorForDisabledCell = false;
         gridView.OptionsFind.ShowCloseButton = false;
         gridView.OptionsFind.AlwaysVisible = true;
         gridView.GroupFormat = "[#image]{1}";
         _textRepository = new RepositoryItemTextEdit {Enabled = false};
         _favoriteFormatter = new BooleanAsStringFormatter();
         _screenBinder = new ScreenBinder<ISimulationParametersPresenter>();
      }

      public void AttachPresenter(ISimulationParametersPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IEnumerable<SimulationParameterSelectionDTO> allParameterDTOs)
      {
         _gridViewBinder.BindToSource(allParameterDTOs);
      }

      public void Rebind()
      {
         _gridViewBinder.Rebind();
      }

      public void BindToModeSelection()
      {
         _screenBinder.BindToSource(_presenter);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _pathElementsBinder.InitializeBinding(_gridViewBinder);

         _gridViewBinder.AutoBind(x => x.Value)
            .WithCaption(Captions.Value)
            .WithFormat(x => x.ValueParameter.ParameterFormatter())
            .AsReadOnly();

         _gridViewBinder.AutoBind(x => x.IsFavorite)
            .WithRepository(x => _textRepository)
            .WithCaption(Captions.Favorite)
            .WithFormat(_favoriteFormatter)
            .AsReadOnly();

         _screenBinder.Bind(x => x.ParameterGroupingMode)
            .To(cbModeSelection)
            .WithValues(x => _presenter.AllGroupingModes);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemParameters.TextVisible = false;
         layoutItemModeSelection.TextVisible = false;
      }

      public void GroupBy(PathElement pathElement, int groupIndex = 0)
      {
         groupByColumn(_pathElementsBinder.ColumnAt(pathElement), groupIndex);
      }

      private void groupByColumn(IGridViewColumn gridViewColumn, int groupIndex = 0)
      {
         if (!gridViewColumn.Visible) return;

         gridViewColumn.XtraColumn.GroupIndex = groupIndex;

         //set the first row visible
         int rowHandle = gridView.GetVisibleRowHandle(0);
         gridView.FocusedRowHandle = rowHandle;
      }

      public IEnumerable<SimulationParameterSelectionDTO> SelectedParameters
      {
         get
         {
            return gridView.GetSelectedRows()
               .Select(rowHandle => _gridViewBinder.ElementAt(rowHandle));
         }
      }
   }
}