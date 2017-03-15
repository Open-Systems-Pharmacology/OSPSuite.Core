using System.Collections.Generic;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Views.Diagram;

namespace OSPSuite.Presentation.Views.Journal
{
   public interface IJournalDiagramView : IView<IJournalDiagramPresenter>, IBaseDiagramView
   {
      void RefreshDiagram();

      /// <summary>
      /// Removes the selection boxes that are painted around selected nodes
      /// </summary>
      void RemoveSelectionHandles();

      /// <summary>
      /// Gets the current selection of nodes in the diagram
      /// </summary>
      List<IBaseObject> GetSelection();
   }
}
