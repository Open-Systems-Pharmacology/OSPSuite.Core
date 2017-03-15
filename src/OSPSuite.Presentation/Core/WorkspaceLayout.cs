using System.Collections.Generic;

namespace OSPSuite.Presentation.Core
{
   public interface IWorkspaceLayout
   {
      IEnumerable<WorkspaceLayoutItem> LayoutItems { get; }
      void AddLayoutItem(WorkspaceLayoutItem layoutItem);
      void RemoveLayoutItem(WorkspaceLayoutItem layoutItem);
   }

   public class WorkspaceLayout : IWorkspaceLayout
   {
      private readonly IList<WorkspaceLayoutItem> _layoutItems;

      public WorkspaceLayout()
      {
         _layoutItems = new List<WorkspaceLayoutItem>();
      }

      public IEnumerable<WorkspaceLayoutItem> LayoutItems
      {
         get { return _layoutItems; }
      }

      public void RemoveLayoutItem(WorkspaceLayoutItem layoutItem)
      {
         _layoutItems.Remove(layoutItem);
      }

      public void AddLayoutItem(WorkspaceLayoutItem layoutItem)
      {
         _layoutItems.Add(layoutItem);
      }
   }
}
