using System;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.XtraEditors.Controls;
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
      public ICache<PathElement, IGridViewColumn> ColumnCache { get; }

      public PathElementsBinder(IImageListRetriever imageListRetriever)
      {
         _imageListRetriever = imageListRetriever;
         ColumnCache = new Cache<PathElement, IGridViewColumn>();
      }

      public void InitializeBinding(GridViewBinder<T> gridViewBinder)
      {
         _gridViewBinder = gridViewBinder;
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

         ColumnCache.Add(pathElement, column);
         return column;
      }

      private RepositoryItem configureContainerRepository(T dto, Func<T, PathElementDTO> pathElementExpression)
      {
         var parameterPathDTO = pathElementExpression(dto);
         var pathElementRepository = new UxRepositoryItemImageComboBox(_gridViewBinder.GridView, _imageListRetriever);
         return pathElementRepository.AddItem(parameterPathDTO, parameterPathDTO.IconName);
      }

      public void SetCaption(PathElement pathElement, string caption)
      {
         ColumnAt(pathElement).Caption = caption;
      }

      public void SetVisibility(PathElement pathElement, bool visible)
      {
         var colIndex = ColumnCache.Keys.ToList().IndexOf(pathElement);
         if (colIndex < 0) return;
         ColumnAt(pathElement).UpdateVisibleIndex(colIndex + 1, visible);
      }

      public IGridViewColumn ColumnAt(PathElement pathElement)
      {
         return ColumnCache[pathElement];
      }

      public bool PathVisible
      {
         set { ColumnCache.Keys.Each(pathColumn => SetVisibility(pathColumn, value)); }
      }

      protected virtual void Cleanup()
      {
         ColumnCache.Clear();
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