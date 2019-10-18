using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationSimulationSelectionView : BaseUserControl, IParameterIdentificationSimulationSelectionView, IViewWithPopup
   {
      private IParameterIdentificationSimulationSelectionPresenter _presenter;
      public BarManager PopupBarManager { get; private set; }

      public bool Updating { get; private set; }

      public ParameterIdentificationSimulationSelectionView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         PopupBarManager = new BarManager {Form = this, Images = imageListRetriever.AllImagesForContextMenu};
         treeView.AllowDrop = true;
         treeView.StateImageList = imageListRetriever.AllImagesForTreeView;
         treeView.DataColumn.SortMode = ColumnSortMode.DisplayText;
         treeView.DataColumn.SortOrder = SortOrder.Ascending;
         treeView.NodeClick += (e, node) => OnEvent(() => nodeClick(e, node));
      }

      private void nodeClick(MouseEventArgs e, ITreeNode selectedNode)
      {
         if (e.Button != MouseButtons.Right)
            return;

         var currentlySelectedNode = selectedNodes;
         if (currentlySelectedNode.Count > 1)
            _presenter.CreatePopupMenuFor(currentlySelectedNode).At(e.Location);
         else
            _presenter.CreatePopupMenuFor(selectedNode).At(e.Location);
      }

      public void AttachPresenter(IParameterIdentificationSimulationSelectionPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         treeView.DragOver += (o, e) => OnEvent(treeViewDragOver, e);
         treeView.DragDrop += (o, e) => OnEvent(treeViewDragDrop, e);
         btnAddSimulation.Click += (o, e) => OnEvent(_presenter.SelectSimulationsToAdd);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemTreeView.TextVisible = false;
         btnAddSimulation.InitWithImage(ApplicationIcons.Add, Captions.ParameterIdentification.AddSimulation);
         layoutItemAddSimulation.AdjustLargeButtonSize();
      }

      public ITreeNode AddNode(ITreeNode nodeToAdd)
      {
         treeView.AddNode(nodeToAdd);
         return nodeToAdd;
      }

      private void treeViewDragOver(DragEventArgs e)
      {
         e.Effect = canAcceptData(simulationsFrom(e)) ? DragDropEffects.Move : DragDropEffects.None;
      }

      private void treeViewDragDrop(DragEventArgs e)
      {
         var simulations = simulationsFrom(e);
         if (!canAcceptData(simulations))
            return;

         _presenter.AddSimulations(simulations);
      }

      private List<ISimulation> simulationsFrom(DragEventArgs e)
      {
         var node = e.Data<List<ITreeNode>>();

         return node?.Select(x => x.TagAsObject)
            .OfType<IClassifiableWrapper>()
            .Select(x => x.WrappedObject)
            .OfType<ISimulation>().ToList();
      }

      public IUxTreeView TreeView => treeView;

      private IReadOnlyList<ITreeNode> selectedNodes => treeView.Selection.ToList().Select(treeView.NodeFrom).ToList();

      private bool canAcceptData(IReadOnlyList<ISimulation> simulations) =>
         simulations != null &&
         simulations.Any() &&
         _presenter.CanUseAllSimulations(simulations);

      public void BeginUpdate()
      {
         treeView.BeginUpdate();
         Updating = true;
      }

      public void EndUpdate()
      {
         treeView.EndUpdate();
         Updating = false;
      }
   }
}