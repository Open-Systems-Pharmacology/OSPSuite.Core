using System.Drawing;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Diagram
{
   public interface IDiagramManager
   {
      void RefreshFromDiagramOptions();
      bool MustHandleExisting(string id);
      bool IsInitialized { get; }
      IDiagramOptions DiagramOptions { get; }
      PointF CurrentInsertLocation { get; set; }
      void RefreshDiagramFromModel();
      bool InsertLocationHasChanged();
      void Cleanup();
      void RemoveObjectBase(IObjectBase objectBase);
      void RefreshObjectBase(IObjectBase objectBase);
      void AddObjectBase(IObjectBase objectBase);
      void UpdateInsertLocation();

      /// <summary>
      /// Normally a diagram node corresponds to a domain entity. For some containers, they can be represented
      /// in the diagram without an entity. In that case, path can still be determined
      /// </summary>
      /// <param name="containerNode">A container node that has already been determined to not correspond to an entity</param>
      /// <returns>The intended entity path</returns>
      ObjectPath PathForNodeWithoutEntity(IContainerNode containerNode);
   }

   public interface IDiagramManager<TModel> : IDiagramManager where TModel : IWithDiagramFor<TModel>
   {
      TModel PkModel { get; }
      void InitializeWith(TModel pkModel, IDiagramOptions diagramOptions);
      /// <summary>
      /// Returns a new instance of the underlying implementation of a <see cref="IDiagramManager"/>
      /// </summary>
      IDiagramManager<TModel> Create();
   }
}
