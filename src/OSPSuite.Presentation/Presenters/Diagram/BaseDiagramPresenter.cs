using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Diagram.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Diagram;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Diagram
{
   public interface IBaseDiagramPresenter : ISubjectPresenter,
      IDisposable
   {
      void Refresh();

      void SelectLinkedNodesForDiagramSelection();
      void SelectVisibleLinkedNodesForDiagramSelection();
      void SetLocationFixedForDiagramSelection(bool selectionFixed);
      void SetNodeSizeForDiagramSelection(NodeSize nodeSize);
      void MoveDiagramSelectionToFront();
      void MoveDiagramSelectionToBack();

      void SelectChildren(IContainerBase containerBase);
      void InvertSelection(IContainerBase containerBase);
      void HideSelection();
      void ShowSelection();
      void CollapseAllExceptSelection();
      void Focus(IContainerNode containerBaseNode);
      void Unfocus(IContainerNode containerBaseNode);
      void Position0Selection();

      void Layout(IContainerBase containerBase, int levelDepth, IList<IHasLayoutInfo> freeNodes);

      bool GridVisible { set; get; }
      void Zoom(float factor);

      void ShowChildren(IContainerBase containerBase);
      void HideChildren(IContainerBase containerBase);
      void HideNotLinkedNodes();
      void ShowLinkedNodes();

      void HideAll();
      void SetDefaultExpansion();
      void ShowDefaultExpansion();
      void ExpandSelection();
      void ExpandSelectionChildren();
      void ExpandSelectionRecursive();
      void CollapseSelection();
      void CollapseSelectionChildren();
      void CollapseSelectionRecursive();
      bool SelectionContains<T>() where T : class, IHasLayoutInfo;
      T GetFirstSelected<T>() where T : class, IHasLayoutInfo;

      void CopyBitmapToClipboard(IContainerBase containerBase);
      void SaveBitmapToFile(IContainerBase containerBase);
      Bitmap GetBitmap(IContainerBase containerBase);
      void PrintDiagram();
      void SelectionMoved(object sender, EventArgs eventArgs);
      EventHandler NodeMoved { get; set; }

      void ShowContextMenu(IBaseNode baseNode, Point popupLocation, PointF locationInDiagramView);
   }

   public interface IBaseDiagramPresenter<TModel> : IEditPresenter<TModel>, IBaseDiagramPresenter where TModel : IWithDiagramFor<TModel>
   {
   }

   public abstract class BaseDiagramPresenter<TView, TPresenter, TModel> : AbstractCommandCollectorPresenter<TView, TPresenter>, IEditPresenter<TModel>, IBaseDiagramPresenter
      where TView : IBaseDiagramView, IView<TPresenter>
      where TPresenter : IBaseDiagramPresenter
      where TModel : class, IWithDiagramFor<TModel>
   {
      protected IDialogCreator _dialogCreator;
      private readonly IDiagramModelFactory _diagramModelFactory;
      protected IContainerBaseLayouter _layouter;
      protected TModel _model;
      public EventHandler NodeMoved { get; set; } = delegate { };

      protected BaseDiagramPresenter(TView view, IContainerBaseLayouter layouter, IDialogCreator dialogCreator, IDiagramModelFactory diagramModelFactory)
         : base(view)
      {
         _dialogCreator = dialogCreator;
         _diagramModelFactory = diagramModelFactory;
         _layouter = layouter;
      }

      public void SelectionMoved(object sender, EventArgs eventArgs)
      {
         NodeMoved(sender, eventArgs);
      }

      public abstract void ShowContextMenu(IBaseNode baseNode, Point popupLocation, PointF locationInDiagramView);

      public void Dispose()
      {
         _view.Dispose();
      }

      protected IDiagramManager<TModel> DiagramManager => _model?.DiagramManager;

      protected IDiagramModel DiagramModel => _model.DiagramModel;

      public bool GridVisible
      {
         set { _view.GridVisible = value; }
         get { return _view.GridVisible; }
      }

      public virtual void Refresh()
      {
         DiagramManager.RefreshDiagramFromModel();
         DiagramManager.RefreshFromDiagramOptions();
         refreshGrid();
         _view.Refresh();
      }

      public void SelectLinkedNodesForDiagramSelection()
      {
         foreach (var node in _view.GetSelectedNodes<IHasLayoutInfo>())
         {
            foreach (var linkedNode in node.GetLinkedNodes<IHasLayoutInfo>())
            {
               _view.Select(linkedNode);
            }
         }
      }

      public void SelectVisibleLinkedNodesForDiagramSelection()
      {
         var selectedLayoutInfoNodes = _view.GetSelectedNodes<IHasLayoutInfo>();
         var selectedContainerNodes = _view.GetSelectedNodes<IContainerNode>();
         foreach (var node in selectedLayoutInfoNodes)
         {
            foreach (var linkedNode in node.GetLinkedNodes<IHasLayoutInfo>())
            {
               if (linkedNode.IsVisible)
               {
                  _view.Select(linkedNode);

                  if (linkedNode.IsAnImplementationOf<IPortNode>())
                     _view.Select(((IPortNode) linkedNode).PortNodeParent);
               }
            }
         }
         foreach (var containerNode in selectedContainerNodes)
         {
            foreach (var linkedNode in containerNode.GetLinkedNodes<IBaseNode>(true))
            {
               if (linkedNode.IsVisible) _view.Select(linkedNode);
            }
         }
      }

      public void SetLocationFixedForDiagramSelection(bool selectionFixed)
      {
         foreach (var node in _view.GetSelectedNodes<IHasLayoutInfo>())
         {
            node.LocationFixed = selectionFixed;
            node.SetColorFrom(DiagramManager.DiagramOptions.DiagramColors);
         }
      }

      public void SetNodeSizeForDiagramSelection(NodeSize nodeSize)
      {
         foreach (var node in _view.GetSelectedNodes<IElementBaseNode>())
         {
            node.NodeSize = nodeSize;
            node.SetColorFrom(DiagramManager.DiagramOptions.DiagramColors);
         }
      }

      public void MoveDiagramSelectionToFront()
      {
         foreach (var node in _view.GetSelectedNodes<IBaseNode>())
         {
            node.ToFront();
         }
      }

      public void MoveDiagramSelectionToBack()
      {
         foreach (var node in _view.GetSelectedNodes<IBaseNode>())
         {
            node.ToBack();
         }
      }

      public void HideAll()
      {
         DiagramModel.SetHiddenRecursive(true);
      }

      public void SetDefaultExpansion()
      {
         DiagramModel.SetDefaultExpansion();
      }

      public void ShowDefaultExpansion()
      {
         DiagramModel.ShowDefaultExpansion();
         _view.Refresh();
      }

      public void Zoom(float factor)
      {
         if (DiagramManager == null) return;
         _view.Zoom(DiagramManager.CurrentInsertLocation, factor);
      }

      public void ShowChildren(IContainerBase containerBase)
      {
         setHidden(containerBase.GetDirectChildren<IBaseNode>(), false);
      }

      public void HideChildren(IContainerBase containerBase)
      {
         setHidden(containerBase.GetDirectChildren<IBaseNode>(), true);
      }

      private void setHidden<T>(IEnumerable<T> nodes, bool hidden) where T : IHasLayoutInfo
      {
         foreach (var node in nodes)
         {
            var containerBaseNode = node as IContainerNode;
            if (containerBaseNode != null)
               containerBaseNode.SetHiddenRecursive(hidden);
            else
               node.Hidden = hidden;
         }
         _view.Refresh();
      }

      public void HideNotLinkedNodes()
      {
         SelectLinkedNodesForDiagramSelection();
         HideAll();
         ShowSelection();
      }

      public void ShowLinkedNodes()
      {
         SelectLinkedNodesForDiagramSelection();
         ShowSelection();
      }

      public void SelectChildren(IContainerBase containerBase)
      {
         _view.ClearSelection();
         foreach (var baseNode in containerBase.GetDirectChildren<IBaseNode>())
         {
            _view.Select(baseNode);
         }
         _view.Refresh();
      }

      public void InvertSelection(IContainerBase containerBase)
      {
         IList<IBaseNode> notSelectedNodes = new List<IBaseNode>();
         foreach (var baseNode in containerBase.GetDirectChildren<IBaseNode>())
         {
            if (!_view.SelectionContains(baseNode))
               notSelectedNodes.Add(baseNode);
         }
         _view.ClearSelection();
         foreach (var baseNode in notSelectedNodes)
         {
            _view.Select(baseNode);
         }
      }

      public void ShowSelection()
      {
         setHidden(_view.GetSelectedNodes<IHasLayoutInfo>(), false);
      }

      public bool SelectionContains<T>() where T : class, IHasLayoutInfo
      {
         return _view.GetSelectedNodes<T>().Any();
      }

      public T GetFirstSelected<T>() where T : class, IHasLayoutInfo
      {
         return _view.GetSelectedNodes<T>().FirstOrDefault();
      }

      public void HideSelection()
      {
         setHidden(_view.GetSelectedNodes<IBaseNode>(), true);
      }

      public void ExpandSelection()
      {
         foreach (var node in _view.GetSelectedNodes<IContainerNode>())
         {
            node.Expand(0);
         }
      }

      public void Position0Selection()
      {
         foreach (var node in _view.GetSelectedNodes<IContainerNode>())
         {
            node.Location = new PointF(0F, 0F);
         }
      }

      public void ExpandSelectionChildren()
      {
         foreach (var node in _view.GetSelectedNodes<IContainerNode>())
         {
            node.Expand(1);
         }
      }

      public void ExpandSelectionRecursive()
      {
         foreach (var node in _view.GetSelectedNodes<IContainerNode>())
         {
            node.Expand(100);
         }
      }

      public void CollapseSelection()
      {
         foreach (var node in _view.GetSelectedNodes<IContainerNode>())
         {
            node.Collapse(0);
         }
      }

      public void CollapseSelectionChildren()
      {
         foreach (var node in _view.GetSelectedNodes<IContainerNode>())
         {
            foreach (var node1 in node.GetDirectChildren<IContainerNode>())
            {
               node1.Collapse(0);
            }
         }
      }

      public void CollapseSelectionRecursive()
      {
         foreach (var node in _view.GetSelectedNodes<IContainerNode>())
         {
            node.Collapse(100);
         }
      }

      public virtual void CollapseAllExceptSelection()
      {
         foreach (var containerBaseNode in DiagramModel.GetAllChildren<IContainerNode>())
         {
            bool containsSelectedNode = false;
            foreach (var node in _view.GetSelectedNodes<IBaseNode>())
            {
               if (containerBaseNode.ContainsChildNode(node, true))
               {
                  containsSelectedNode = true;
                  break;
               }
            }

            if (!containsSelectedNode)
               containerBaseNode.Collapse(0);
         }
      }

      public virtual void Focus(IContainerNode containerBaseNode)
      {
         // Show node and expand it
         containerBaseNode.SetHiddenRecursive(false);
         containerBaseNode.Expand(0);

         // Show neighbors
         foreach (var neighborNode in containerBaseNode.GetLinkedNodes<IBaseNode>(true))
         {
            neighborNode.Hidden = false;
         }

         containerBaseNode.PostLayoutStep();
         _view.Refresh();
      }

      public void Unfocus(IContainerNode containerBaseNode)
      {
         // Show all inside and neighbors of parent
         containerBaseNode.GetParent().SetHiddenRecursive(false);
         var parentContainerBaseNode = containerBaseNode.GetParent() as IContainerNode;
         if (parentContainerBaseNode != null)
            foreach (var neighborNode in parentContainerBaseNode.GetLinkedNodes<IBaseNode>(true))
            {
               neighborNode.Hidden = false;
            }

         //show all children of ancestors with IsExpandedByDefault
         while (parentContainerBaseNode != null && parentContainerBaseNode.IsExpandedByDefault)
         {
            parentContainerBaseNode.SetHiddenRecursive(false);
            parentContainerBaseNode = parentContainerBaseNode.GetParent() as IContainerNode;
         }

         // Collapse node and arrange its neighborhood nodes
         containerBaseNode.Collapse(100);

         containerBaseNode.PostLayoutStep();
         _view.Refresh();
      }

      public void Layout(IContainerBase containerBase, int levelDepth, IList<IHasLayoutInfo> freeNodes)
      {
         if (containerBase == null) containerBase = DiagramModel;

         if (containerBase == DiagramModel)
            FixLocationFirstVisibleTopContainer(); // to avoid moving container

         _layouter.ForceLayoutConfiguration = LayoutConfiguration;
         _layouter.DoForceLayout(containerBase, freeNodes, levelDepth);
         DiagramModel.IsLayouted = true;
         _view.Refresh();
      }

      public IForceLayoutConfiguration LayoutConfiguration { get; set; }

      public void Edit(object objectToEdit)
      {
         Edit(objectToEdit.DowncastTo<TModel>());
      }

      protected void FixLocationFirstVisibleTopContainer()
      {
         var topContainers = DiagramModel.GetDirectChildren<IContainerNode>().Where(x => x.Visible).ToList();
         if (topContainers.Any())
            topContainers.First().LocationFixed = true;
      }

      public void CopyBitmapToClipboard(IContainerBase containerBase)
      {
         using (var ms = new MemoryStream())
         {
            GetBitmap(containerBase).Save(ms, ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);

            Image image = Image.FromStream(ms);
            _view.CopyToClipboard(image);
         }
      }

      public void SaveBitmapToFile(IContainerBase containerBase)
      {
         var imageFilePath = _dialogCreator.AskForFileToSave(Captions.SaveImage, Constants.Filter.DIAGRAM_IMAGE_FILTER, Constants.DirectoryKey.REPORT);
         if (string.IsNullOrEmpty(imageFilePath)) return;
         GetBitmap(containerBase).Save(imageFilePath, ImageFormat.Png);
      }

      public Bitmap GetBitmap(IContainerBase containerBase)
      {
         return _view.GetBitmap(containerBase);
      }

      public void PrintDiagram()
      {
         _view.PrintPreview();
      }

      public virtual void Edit(TModel pkModel)
      {
         _model = pkModel;
         if (_model.DiagramModel == null)
            _model.DiagramModel = CreateDiagramModel();

         _model.InitializeDiagramManager(GetDiagramOptions());
         refreshGrid();
         _view.Model = DiagramModel;
      }

      protected IDiagramModel CreateDiagramModel()
      {
         return _diagramModelFactory.Create();
      }

      private void refreshGrid()
      {
         GridVisible = GetDiagramOptions().SnapGridVisible;
      }

      protected abstract IDiagramOptions GetDiagramOptions();

      public object Subject => _model;
   }
}