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
      void AddObjectBase(IObjectBase objectBase);
      void UpdateInsertLocation();
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
