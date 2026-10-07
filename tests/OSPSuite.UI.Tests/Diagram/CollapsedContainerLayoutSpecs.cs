using System.Drawing;
using DevExpress.Utils;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Diagram;
using OSPSuite.Presentation.Services;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Diagram
{
   public class TestDiagramSubject : IWithDiagramFor<TestDiagramSubject>
   {
      public IDiagramModel DiagramModel { get; set; }
      public IDiagramManager<TestDiagramSubject> DiagramManager { get; set; }
   }

   public class TestDiagramManager : BaseDiagramManager<ContainerNode, NeighborhoodNode, TestDiagramSubject>
   {
      protected override void UpdateDiagramModel(TestDiagramSubject pkModel, IDiagramModel diagramModel, bool coupleAll)
      {
      }

      protected override void DecoupleModel()
      {
      }

      protected override bool MustHandleNew(IObjectBase obj) => false;

      public override IDiagramManager<TestDiagramSubject> Create() => new TestDiagramManager();
   }

   public class TestBaseDiagramPresenter : BaseDiagramPresenter<TestDevExpressDiagramView, IBaseDiagramPresenter, TestDiagramSubject>
   {
      public TestBaseDiagramPresenter(TestDevExpressDiagramView view)
         : base(view, A.Fake<IContainerBaseLayouter>(), A.Fake<IDialogCreator>(), A.Fake<IDiagramModelFactory>())
      {
      }

      public override void ShowContextMenu(IBaseNode baseNode, Point popupLocation, PointF locationInDiagramView)
      {
      }

      protected override IDiagramOptions GetDiagramOptions() => new DiagramOptions();
   }

   public class When_editing_a_diagram_whose_containers_were_saved_collapsed : ContextSpecification<TestBaseDiagramPresenter>
   {
      private TestDevExpressDiagramView _view;
      private TestDiagramSubject _subject;
      private DiagramModel _diagramModel;
      private ContainerNode _organism;
      private ContainerNode _liver;
      private ContainerNode _kidney;
      private ContainerNode _plasma;
      private ContainerNode _interstitial;
      private NeighborhoodNode _organNeighborhood;
      private NeighborhoodNode _compartmentNeighborhood;

      protected override void Context()
      {
         var imageListRetriever = A.Fake<IImageListRetriever>();
         A.CallTo(() => imageListRetriever.AllImages16x16).Returns(new SvgImageCollection());
         _view = new TestDevExpressDiagramView(imageListRetriever);
         sut = new TestBaseDiagramPresenter(_view);
         _view.AttachPresenter(sut);
         _view.InitializeResources();

         _diagramModel = new DiagramModel();
         _organism = container("Organism", _diagramModel, 0, 0, 900, 500);
         _liver = container("Liver", _organism, 100, 100, 200, 150);
         _kidney = container("Kidney", _organism, 500, 100, 200, 150);
         _plasma = container("Plasma", _liver, 110, 110, 60, 40);
         _interstitial = container("Interstitial", _liver, 190, 110, 60, 40);
         _liver.IsExpanded = false;
         _kidney.IsExpanded = false;
         _plasma.IsExpanded = false;
         _interstitial.IsExpanded = false;

         _organNeighborhood = neighborhood("liver_kidney", _organism, _liver, _kidney);
         _compartmentNeighborhood = neighborhood("plasma_interstitial", _liver, _plasma, _interstitial);

         _subject = new TestDiagramSubject {DiagramModel = _diagramModel, DiagramManager = new TestDiagramManager()};
      }

      private ContainerNode container(string name, IContainerBase parent, float x, float y, float width, float height)
      {
         var node = _diagramModel.CreateNode<ContainerNode>(name, new PointF(x, y), parent);
         node.Name = name;
         node.Size = new SizeF(width, height);
         return node;
      }

      private NeighborhoodNode neighborhood(string id, IContainerBase parent, ContainerNode first, ContainerNode second)
      {
         var node = _diagramModel.CreateNode<NeighborhoodNode>(id, PointF.Empty, parent);
         node.Initialize(first, second);
         return node;
      }

      private static PointF midPointBetween(ContainerNode first, ContainerNode second)
      {
         var firstCenter = first.DrawnBounds.Center();
         var secondCenter = second.DrawnBounds.Center();
         return new PointF((firstCenter.X + secondCenter.X) / 2, (firstCenter.Y + secondCenter.Y) / 2);
      }

      protected override void Because()
      {
         sut.Edit(_subject);
      }

      [Observation]
      public void should_place_the_neighborhood_between_the_extents_the_collapsed_containers_are_drawn_at()
      {
         _organNeighborhood.Location.ShouldBeEqualTo(midPointBetween(_liver, _kidney));
      }

      [Observation]
      public void should_place_the_neighborhood_of_containers_nested_in_a_collapsed_container_between_the_extents_they_are_drawn_at()
      {
         _compartmentNeighborhood.Location.ShouldBeEqualTo(midPointBetween(_plasma, _interstitial));
      }
   }
}
