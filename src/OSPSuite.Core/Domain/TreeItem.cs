using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class TreeItem<T>
   {
      public IList<TreeItem<T>> Children { get; } = new List<TreeItem<T>>();
      public TreeItem<T> Parent { get; set; }
      public T Item { get; set; }

      public void AddToAllLeaves(IEnumerable<T> itemsToAdd)
      {
         allLeaves(this).Each(leaf => leaf.addChildren(itemsToAdd));
      }

      private void addChildren(IEnumerable<T> itemsToAdd)
      {
         itemsToAdd.Each(item => Children.Add(new TreeItem<T> { Item = item, Parent = this }));
      }

      public IEnumerable<IEnumerable<T>> UniquePaths()
      {
         var list = new List<IEnumerable<T>>();
         allLeaves(this).Each(leaf => list.Add(pathToRoot(leaf)));
         return list;
      }

      private IReadOnlyList<T> pathToRoot(TreeItem<T> item)
      {
         var list = new List<T>();

         if (item.Item != null)
            list.Add(item.Item);

         if (item.Parent == null)
            return list;

         list.AddRange(pathToRoot(item.Parent));
         return list;
      }

      private IEnumerable<TreeItem<T>> allLeaves(TreeItem<T> leaf)
      {
         var leaves = new List<TreeItem<T>>();
         if (!leaf.Children.Any())
         {
            leaves.Add(leaf);
         }
         else
         {
            leaf.Children.Each(child => leaves.AddRange(allLeaves(child)));
         }
         return leaves;
      }
   }
}