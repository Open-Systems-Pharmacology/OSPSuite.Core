using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraBars;
using Northwoods.Go;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.Diagram;
using OSPSuite.Presentation.Views.Diagram;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Diagram.Elements;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.Diagram
{
   public partial class BaseDiagramView : BaseUserControl, IBaseDiagramView, IViewWithPopup
   {
      protected IBaseDiagramPresenter _presenter;

      private GoOverview _overView;

      public override void InitializeResources()
      {
         base.InitializeResources();

         _goView.SelectionMoved += (o, e) => OnEvent(() => onSelectionMoved(o, e));
         _goView.ObjectContextClicked += (o, e) => OnEvent(OnContextClicked,e);
         _goView.ObjectSingleClicked += (o, e) => OnEvent(onSingleClicked, e);
      }

      protected virtual void OnContextClicked(GoObjectEventArgs e)
      {
         if (e.GoObject == null) return;
         var baseNode = e.GoObject as IBaseNode;
         if (baseNode == null && e.GoObject.Parent != null)
            baseNode = e.GoObject.Parent as IBaseNode;
         if (baseNode == null) return;

         _presenter.ShowContextMenu(baseNode, e.ViewPoint, e.DocPoint);
      }

      private void onSelectionMoved(object sender, EventArgs e)
      {
         _presenter.SelectionMoved(sender, e);
      }

      public Control Overview
      {
         set
         {
            _overView = value as GoOverview;
            if (_overView != null)
            {
               _overView.Observed = _goView;
               _overView.Show();
            }
         }
      }

      public BaseDiagramView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _goView.NoFocusSelectionColor = _goView.SecondarySelectionColor;
         PopupBarManager.Images = imageListRetriever.AllImages16x16;
      }

      public IDiagramModel Model
      {
         set
         {
            var baseDiagramModel = value as DiagramModel;
            if (baseDiagramModel == null) throw new InvalidTypeException(baseDiagramModel, typeof(DiagramModel));
            _goView.Document = baseDiagramModel;
         }
      }

      public virtual void AttachPresenter(IBaseDiagramPresenter presenter)
      {
         _presenter = presenter;
      }

      private void onSingleClicked(GoObjectEventArgs e)
      {
         var containerBaseHandle = e.GoObject as GoSubGraphHandle;
         var containerBaseNode = containerBaseHandle?.Parent as IContainerNode;
         if (containerBaseNode == null)
            return;

         // keep original Expansion state
         bool containerBaseNodeIsExpanded = containerBaseNode.IsExpanded;
         try
         {
            if (e.Control && containerBaseNode.IsExpanded)
               containerBaseNode.Collapse(100); // collapse recursively

            if (!e.Control && !e.Shift) // collapse and show other/ expand and hide other
            {
               if (containerBaseNode.IsExpanded)
                  Presenter.Unfocus(containerBaseNode);
               else
               {
                  if (!containerBaseNode.IsExpandedByDefault) Presenter.HideAll();
                  Presenter.Focus(containerBaseNode);
               }
            }
         }
         finally
         {
            containerBaseNode.IsExpanded = containerBaseNodeIsExpanded; // because Default Action for GoSubGraphHandle is performed afterwards
         }
      }

      public bool SelectionContains<T>(T obj)
      {
         return _goView.Selection.Contains(obj as GoObject);
      }

      public void CenterAt<T>(T node)
      {
         var goObject = node as GoObject;
         if (goObject != null)
            _goView.RescaleWithCenter(_goView.DocScale, goObject.Location);
      }

      public bool GridVisible
      {
         set => _goView.GridStyle = value ? GoViewGridStyle.Dot : GoViewGridStyle.None;
         get => _goView.GridStyle != GoViewGridStyle.None;
      }

      public void Zoom(PointF currentLocation, float factor)
      {
         if (factor <= 0.0F) _goView.RescaleToFit();
         else _goView.RescaleWithCenter(_goView.DocScale * factor, currentLocation);
      }

      public void SetBackColor(Color color)
      {
         _goView.BackColor = color;
      }

      public void Select<T>(T node)
      {
         GoObject goObject = node as GoObject;
         _goView.Selection.Add(goObject);
      }

      public IEnumerable<T> GetSelectedNodes<T>() where T : class
      {
         IList<T> nodes = new List<T>();
         foreach (GoObject goObject in _goView.Selection)
         {
            T node = goObject as T;
            if (node != null) nodes.Add(node);
         }

         return nodes;
      }

      public void BeginUpdate()
      {
         _goView.BeginUpdate();
      }

      public void EndUpdate()
      {
         _goView.EndUpdate();
      }

      public void ClearSelection()
      {
         _goView.Selection.Clear();
      }

      public IBaseDiagramPresenter Presenter => _presenter;

      public BarManager PopupBarManager { get; private set; }

      public Bitmap GetBitmap(IContainerBase containerBase)
      {
         var goCollection = containerBase as IGoCollection;
         if (goCollection != null)
            return _goView.GetBitmapFromCollection(goCollection);

         return _goView.GetBitmap();
      }

      public void PrintPreview()
      {
         _goView.PrintScale = _goView.DocScale;
         _goView.PrintPreview();
      }
   }

   public class NewLink : GoLink
   {
      public NewLink()
      {
         PenColor = Color.Gray;
      }
   }
}