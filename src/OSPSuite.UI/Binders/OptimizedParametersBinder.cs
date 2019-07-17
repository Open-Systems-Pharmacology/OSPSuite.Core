using System;
using System.Drawing;
using System.Linq.Expressions;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Assets;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Services;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;
using OSPSuite.Presentation.Formatters;

namespace OSPSuite.UI.Binders
{
   public class OptimizedParametersBinder : IDisposable
   {
      private readonly IImageListRetriever _imageListRetriever;
      private readonly IToolTipCreator _toolTipCreator;
      private GridViewBinder<OptimizedParameterDTO> _gridViewBinder;
      private readonly ValueDTOFormatter _valueFormatter = new ValueDTOFormatter();
      private readonly RepositoryItemPictureEdit _rangeRepository = new RepositoryItemPictureEdit();
      private IGridViewBoundColumn<OptimizedParameterDTO, Image> _imageColumn;

      public OptimizedParametersBinder(IImageListRetriever imageListRetriever, IToolTipCreator toolTipCreator)
      {
         _imageListRetriever = imageListRetriever;
         _toolTipCreator = toolTipCreator;
         _rangeRepository.AllowFocused = false;
      }

      public IGridViewBinder GridViewBinder => _gridViewBinder;

      public void InitializeBinding(GridViewBinder<OptimizedParameterDTO> gridViewBinder)
      {
         _gridViewBinder = gridViewBinder;

         var col = _gridViewBinder.AutoBind(x => x.Name)
            .WithCaption(Captions.Name)
            .WithRepository(nameRepositoryFor)
            .AsReadOnly();

         //Allow focus for name so that copy paste can be performed
         col.XtraColumn.OptionsColumn.AllowFocus = true;

         bindValue(x => x.OptimalValue, Captions.ParameterIdentification.OptimalValue);
         bindValue(x => x.StartValue, Captions.ParameterIdentification.StartValue);
         bindValue(x => x.MinValue, Captions.ParameterIdentification.MinValue);
         bindValue(x => x.MaxValue, Captions.ParameterIdentification.MaxValue);

         _imageColumn = gridViewBinder.Bind(x => x.RangeImage)
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithFixedWidth(UIConstants.Size.OPTIMIZED_RANGE_WIDTH)
            .WithRepository(x => _rangeRepository)
            .AsReadOnly();

         updateGridViewSelectionMode(_gridViewBinder.GridView);
      }

      private static void updateGridViewSelectionMode(GridView gridView)
      {
         gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
         gridView.OptionsSelection.EnableAppearanceFocusedRow = true;
         gridView.OptionsSelection.EnableAppearanceFocusedCell = true;
         gridView.OptionsSelection.MultiSelect = true;
      }

      private RepositoryItem nameRepositoryFor(OptimizedParameterDTO optimizedParameterDTO)
      {
         var nameRepository = new UxRepositoryItemImageComboBox(_gridViewBinder.GridView, _imageListRetriever);
         return nameRepository.AddItem(optimizedParameterDTO.Name, optimizedParameterDTO.BoundaryCheckIcon);
      }

      private IGridViewAutoBindColumn<OptimizedParameterDTO, T> bind<T>(Expression<Func<OptimizedParameterDTO, T>> expression, string caption)
      {
         return _gridViewBinder.AutoBind(expression)
            .WithCaption(caption)
            .AsReadOnly();
      }

      private IGridViewAutoBindColumn<OptimizedParameterDTO, ValueDTO> bindValue(Expression<Func<OptimizedParameterDTO, ValueDTO>> expression, string caption)
      {
         var col =  bind(expression, caption)
            .WithFormat(x => _valueFormatter);

         col.XtraColumn.OptionsColumn.AllowFocus = true;
         return col;
      }

      public bool ShowTooltip(ToolTipControllerGetActiveObjectInfoEventArgs e, Image image)
      {
         if (!isInImageColumn(e))
            return false;

         var superToolTip = _toolTipCreator.ToolTipFor(image);
         e.Info = _toolTipCreator.ToolTipControlInfoFor(_gridViewBinder.ElementAt(e), superToolTip);
         return true;
      }

      private bool isInImageColumn(ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         var hi = _gridViewBinder.GridView.CalcHitInfo(e.ControlMousePosition);
         return _imageColumn.XtraColumn == hi.Column;
      }

      public void Dispose()
      {
         _gridViewBinder.Dispose();
      }
   }
}