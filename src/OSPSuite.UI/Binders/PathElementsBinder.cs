using System;
using System.Linq;
using System.Linq.Expressions;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Binders
{
   public class PathElementsBinder<T> : IDisposable where T : IPathRepresentableDTO
   {
      private readonly IImageListRetriever _imageListRetriever;
      private readonly ICache<PathElement, IGridViewColumn> _columnPathElementCache;
      private GridViewBinder<T> _gridViewBinder;
      private RepositoryItemImageComboBox _pathElementRepository;

      public PathElementsBinder(IImageListRetriever imageListRetriever)
      {
         _imageListRetriever = imageListRetriever;
         _columnPathElementCache = new Cache<PathElement, IGridViewColumn>();
      }

      public void InitializeBinding(GridViewBinder<T> gridViewBinder)
      {
         _gridViewBinder = gridViewBinder;
         _pathElementRepository = new UxRepositoryItemImageComboBox(gridViewBinder.GridView, _imageListRetriever);
         configureImagePathElement(p => p.SimulationPathElement, PathElement.Simulation, Captions.SimulationPath);
         configureImagePathElement(p => p.TopContainerPathElement, PathElement.TopContainer, Captions.TopContainerPath);
         configureImagePathElement(p => p.ContainerPathElement, PathElement.Container, Captions.ContainerPath);
         configureImagePathElement(p => p.BottomCompartmentPathElement, PathElement.BottomCompartment, Captions.BottomCompartmentPath);
         configureImagePathElement(p => p.MoleculePathElement, PathElement.Molecule, Captions.Molecule);
         configurePathElement(p => p.NamePathElement, PathElement.Name, Captions.Name);
      }

      private void configureImagePathElement(Expression<Func<T, PathElementDTO>> parameterPathExpression, PathElement pathElement, string caption)
      {
         configurePathElement(parameterPathExpression, pathElement, caption)
            .WithRepository(dto => configureContainerRepository(dto, parameterPathExpression.Compile()));
      }

      private IGridViewAutoBindColumn<T, PathElementDTO> configurePathElement(Expression<Func<T, PathElementDTO>> parameterPathExpression, PathElement pathElement, string caption)
      {
         var column = _gridViewBinder.AutoBind(parameterPathExpression)
            .WithCaption(caption)
            .AsReadOnly();

         _columnPathElementCache.Add(pathElement, column);
         return column;
      }

      private RepositoryItem configureContainerRepository(T dto, Func<T, PathElementDTO> pathElementExpression)
      {
         _pathElementRepository.Items.Clear();
         var parameterPathDTO = pathElementExpression(dto);
         _pathElementRepository.Items.Add(new ImageComboBoxItem(parameterPathDTO, _imageListRetriever.ImageIndex(parameterPathDTO.IconName)));
         return _pathElementRepository;
      }

      public void SetCaption(PathElement pathElement, string caption)
      {
         ColumnAt(pathElement).Caption = caption;
      }

      public void SetVisibility(PathElement pathElement, bool visible)
      {
         var colIndex = _columnPathElementCache.Keys.ToList().IndexOf(pathElement);
         if (colIndex < 0) return;
         ColumnAt(pathElement).UpdateVisibleIndex(colIndex + 1, visible);
      }

      public IGridViewColumn ColumnAt(PathElement pathElement)
      {
         return _columnPathElementCache[pathElement];
      }

      public bool PathVisible
      {
         set { _columnPathElementCache.Keys.Each(pathColumn => SetVisibility(pathColumn, value)); }
      }

      public ICache<PathElement, IGridViewColumn> ColumnCache => _columnPathElementCache;

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