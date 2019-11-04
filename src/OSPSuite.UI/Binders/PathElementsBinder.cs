using System;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.XtraEditors.Repository;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Binders
{
   public class PathElementsBinder<T> : IDisposable where T : IPathRepresentableDTO
   {
      private readonly IImageListRetriever _imageListRetriever;
      private GridViewBinder<T> _gridViewBinder;
      private readonly Cache<PathElementId, IGridViewColumn> _columnPathElementCache = new Cache<PathElementId, IGridViewColumn>();

      public PathElementsBinder(IImageListRetriever imageListRetriever)
      {
         _imageListRetriever = imageListRetriever;
      }

      public void InitializeBinding(GridViewBinder<T> gridViewBinder)
      {
         _gridViewBinder = gridViewBinder;
         configureImagePathElement(p => p.SimulationPathElement, PathElementId.Simulation, Captions.SimulationPath);
         configureImagePathElement(p => p.TopContainerPathElement, PathElementId.TopContainer, Captions.TopContainerPath);
         configureImagePathElement(p => p.ContainerPathElement, PathElementId.Container, Captions.ContainerPath);
         configureImagePathElement(p => p.BottomCompartmentPathElement, PathElementId.BottomCompartment, Captions.BottomCompartmentPath);
         configureImagePathElement(p => p.MoleculePathElement, PathElementId.Molecule, Captions.Molecule);
         configurePathElement(p => p.NamePathElement, PathElementId.Name, Captions.Name);
      }

      private void configureImagePathElement(Expression<Func<T, PathElement>> parameterPathExpression, PathElementId pathElementId, string caption)
      {
         configurePathElement(parameterPathExpression, pathElementId, caption)
            .WithRepository(dto => configureContainerRepository(dto, parameterPathExpression.Compile()));
      }

      public ICache<PathElementId, IGridViewColumn> ColumnCache => _columnPathElementCache;

      private IGridViewAutoBindColumn<T, PathElement> configurePathElement(Expression<Func<T, PathElement>> parameterPathExpression, PathElementId pathElementId, string caption)
      {
         var column = _gridViewBinder.AutoBind(parameterPathExpression)
            .WithCaption(caption)
            .AsReadOnly();

         _columnPathElementCache.Add(pathElementId, column);
         return column;
      }

      private RepositoryItem configureContainerRepository(T dto, Func<T, PathElement> pathElementExpression)
      {
         var parameterPathDTO = pathElementExpression(dto);
         var pathElementRepository = new UxRepositoryItemImageComboBox(_gridViewBinder.GridView, _imageListRetriever);
         return pathElementRepository.AddItem(parameterPathDTO, parameterPathDTO.IconName);
      }

      public void SetCaption(PathElementId pathElementId, string caption)
      {
         ColumnAt(pathElementId).Caption = caption;
      }

      public void SetVisibility(PathElementId pathElementId, bool visible)
      {
         var colIndex = _columnPathElementCache.Keys.ToList().IndexOf(pathElementId);
         if (colIndex < 0) return;
         ColumnAt(pathElementId).UpdateVisibility(visible);
      }

      public IGridViewColumn ColumnAt(PathElementId pathElementId)
      {
         return _columnPathElementCache[pathElementId];
      }

      public bool PathVisible
      {
         set { _columnPathElementCache.Keys.Each(pathColumn => SetVisibility(pathColumn, value)); }
      }

      protected virtual void Cleanup()
      {
         _columnPathElementCache.Clear();
      }

      #region Disposable properties

      private bool _disposed;

      public void Dispose()
      {
         if (_disposed) return;

         Cleanup();
         GC.SuppressFinalize(this);
         _disposed = true;
      }

      ~PathElementsBinder()
      {
         Cleanup();
      }

      #endregion
   }
}