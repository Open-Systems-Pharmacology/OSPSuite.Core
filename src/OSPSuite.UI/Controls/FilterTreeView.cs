using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.Nodes.Operations;
using OSPSuite.Assets;

namespace OSPSuite.UI.Controls
{
   public partial class FilterTreeView : XtraUserControl
   {
      public bool ShowDescendantNode { get; set; }

      public bool EnableFilter
      {
         set { layoutItemFilter.Visibility = LayoutVisibilityConvertor.FromBoolean(value); }
      }

      public FilterTreeView()
      {
         InitializeComponent();
         initializeBinding();
         intializeRessources();
         ShowDescendantNode = true;
      }

      private void intializeRessources()
      {
         layoutItemFilter.Text = Captions.Filter;
      }

      private void initializeBinding()
      {
         btnFilter.Properties.Buttons.Clear();
         btnFilter.EditValueChanging += (o, e) => runFilter(filterValueFrom(e.NewValue));
      }

 
      private string filterValueFrom(object editorValue)
      {
         return editorValue != null ? editorValue.ToString() : string.Empty;
      }

      private void runFilter(string filter)
      {
         using (var operation = new FilterNodeOperation(filter, ShowDescendantNode))
         {
            treeView.DoWithinBatchUpdate(() => treeView.NodesIterator.DoOperation(operation));
         }
      }

      public UxImageTreeView TreeView
      {
         get { return treeView; }
      }

      private class FilterNodeOperation : TreeListOperation, IDisposable
      {
         private readonly string _pattern;
         private readonly bool _showDescendantNode;
         private readonly HashSet<TreeListNode> _nodeVisited;
         private readonly bool _shouldExpand;

         public FilterNodeOperation(string pattern, bool showDescendantNode)
         {
            _pattern = pattern.ToLower();
            _showDescendantNode = showDescendantNode;
            _nodeVisited = new HashSet<TreeListNode>();
            _shouldExpand = !string.IsNullOrEmpty(_pattern);
         }

         public override void Execute(TreeListNode node)
         {
            if (_nodeVisited.Contains(node)) return;
            _nodeVisited.Add(node);
            if (nodeContainsPattern(node))
            {
               makeNodeVisible(node);
               makeAllParentsVisible(node);

               if (_showDescendantNode)
                  makeAllDescendantVisible(node);
            }
            else
               node.Visible = false;
         }

         private void makeNodeVisible(TreeListNode node)
         {
            node.Visible = true;
            if (_shouldExpand)
               node.Expanded = true;
            _nodeVisited.Add(node);
         }

         private void makeAllDescendantVisible(TreeListNode node)
         {
            if (node == null) return;
            makeNodeVisible(node);
            node.Nodes.Cast<TreeListNode>().Each(makeAllDescendantVisible);
         }

         private void makeAllParentsVisible(TreeListNode node)
         {
            if (node == null) return;
            makeNodeVisible(node);
            makeAllParentsVisible(node.ParentNode);
         }

         private bool nodeContainsPattern(TreeListNode node)
         {
            return node.TreeList.Columns.Cast<TreeListColumn>()
               .Select(col => node.GetValue(col))
               .Where(value => value != null)
               .Any(value => value.ToString().ToLower().Contains(_pattern));
         }

         protected virtual void Cleanup()
         {
            _nodeVisited.Clear();
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

         ~FilterNodeOperation()
         {
            Cleanup();
         }

         #endregion
      }
   }
}