using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Diagram.Services;

namespace OSPSuite.Presentation.Diagram
{
   public abstract class concern_for_DiagramModelToXmlMapper_with_containers : concern_for_DiagramModelToXmlMapper
   {
      protected DiagramModel _model;
      protected ContainerNode _organism;
      protected ContainerNode _plasma;
      protected ContainerNode _moleculeProperties;
      protected ContainerNode _interstitial;
      protected NeighborhoodNode _neighborhood;

      protected override void Context()
      {
         base.Context();
         _model = new DiagramModel();
         _organism = _model.CreateNode<ContainerNode>("organism", new PointF(100, 200), _model);
         _organism.Name = "Organism";
         _organism.Description = "Organism\nThe organism";
         _organism.Size = new SizeF(400, 300);
         _organism.IsLogical = true;
         _organism.IsExpandedByDefault = true;
         _organism.LocationFixed = true;

         _plasma = _model.CreateNode<ContainerNode>("plasma", new PointF(120, 230), _organism);
         _plasma.Name = "Plasma";
         _plasma.Size = new SizeF(150, 60);
         _plasma.IsExpanded = false;

         _moleculeProperties = _model.CreateNode<ContainerNode>("mp", new PointF(135, 250), _plasma);
         _moleculeProperties.Name = "MoleculeProperties";
         _moleculeProperties.Size = new SizeF(120, 30);
         _moleculeProperties.IsLogical = true;
         _moleculeProperties.IsVisible = false;

         _interstitial = _model.CreateNode<ContainerNode>("interstitial", new PointF(320, 230), _organism);
         _interstitial.Name = "Interstitial";
         _interstitial.Size = new SizeF(150, 60);

         _neighborhood = _model.CreateNode<NeighborhoodNode>("pls_int", new PointF(295, 260), _organism);
         _neighborhood.Name = "pls_int";
         _neighborhood.Initialize(_plasma, _interstitial);
      }

      protected static string pointToString(PointF point) => $"{floatToString(point.X)} {floatToString(point.Y)}";

      protected static string sizeToString(SizeF size) => $"{floatToString(size.Width)} {floatToString(size.Height)}";

      private static string floatToString(float value) => value.ToString(CultureInfo.InvariantCulture);
   }

   public class When_serializing_a_spatial_structure_diagram_model : concern_for_DiagramModelToXmlMapper_with_containers
   {
      private XmlDocument _xmlDoc;
      private XmlElement _organismElement;

      protected override void Because()
      {
         _xmlDoc = sut.DiagramModelToXmlDocument(_model);
         _organismElement = elementWithId(_xmlDoc, "organism");
      }

      [Observation]
      public void should_write_the_containers_nested_with_the_legacy_element_name_and_attributes()
      {
         _organismElement.Name.ShouldBeEqualTo("SimpleContainerNode");
         _organismElement.GetAttribute("Name").ShouldBeEqualTo("Organism");
         _organismElement.GetAttribute("Location").ShouldBeEqualTo(pointToString(_organism.Location));
         _organismElement.GetAttribute("Size").ShouldBeEqualTo(sizeToString(_organism.Size));
         _organismElement.GetAttribute("LocationFixed").ShouldBeEqualTo("true");
         _organismElement.GetAttribute("UserFlags").ShouldBeEqualTo(NodeLayoutType.CONTAINER_NODE.ToString());
         _organismElement.GetAttribute("Description").ShouldBeEqualTo("Organism\nThe organism");
         _organismElement.GetAttribute("IsLogical").ShouldBeEqualTo("true");
         _organismElement.GetAttribute("IsExpanded").ShouldBeEqualTo("true");
         _organismElement.GetAttribute("IsExpandedByDefault").ShouldBeEqualTo("true");
         _organismElement.HasAttribute("NodeSize").ShouldBeFalse();
         _organismElement.HasAttribute("GoSubGraph.SavedBounds").ShouldBeFalse();

         var children = _organismElement.ChildNodes.OfType<XmlElement>().ToList();
         children.Select(x => x.GetAttribute("Id")).ShouldOnlyContainInOrder("plasma", "interstitial", "pls_int");
      }

      [Observation]
      public void should_write_the_children_of_a_collapsed_container_at_the_container_location_with_their_saved_bounds()
      {
         var plasma = _organismElement.ChildNodes.OfType<XmlElement>().Single(x => x.GetAttribute("Id") == "plasma");
         plasma.GetAttribute("IsExpanded").ShouldBeEqualTo("false");
         plasma.GetAttribute("Location").ShouldBeEqualTo("120 230");
         plasma.HasAttribute("GoSubGraph.SavedBounds").ShouldBeFalse();

         var moleculeProperties = plasma.ChildNodes.OfType<XmlElement>().Single();
         moleculeProperties.GetAttribute("Id").ShouldBeEqualTo("mp");
         moleculeProperties.GetAttribute("GoSubGraph.SavedBounds").ShouldBeEqualTo("15 20 120 30");
         moleculeProperties.GetAttribute("Location").ShouldBeEqualTo("120 230");
         moleculeProperties.GetAttribute("Size").ShouldBeEqualTo("120 30");
         moleculeProperties.GetAttribute("IsVisible").ShouldBeEqualTo("false");
         moleculeProperties.GetAttribute("IsLogical").ShouldBeEqualTo("true");
      }

      [Observation]
      public void should_write_the_neighborhood_node_with_null_neighbor_references()
      {
         var neighborhood = _organismElement.ChildNodes.OfType<XmlElement>().Single(x => x.GetAttribute("Id") == "pls_int");
         neighborhood.Name.ShouldBeEqualTo("SimpleNeighborhoodNode");
         neighborhood.GetAttribute("Location").ShouldBeEqualTo("295 260");
         neighborhood.GetAttribute("Size").ShouldBeEqualTo("15 15");
         neighborhood.GetAttribute("NodeSize").ShouldBeEqualTo("Middle");
         neighborhood.GetAttribute("UserFlags").ShouldBeEqualTo(NodeLayoutType.NEIGHBORHOOD_NODE.ToString());
         neighborhood.GetAttribute("FirstNeighbor").ShouldBeEqualTo("null");
         neighborhood.GetAttribute("SecondNeighbor").ShouldBeEqualTo("null");
      }
   }

   public class When_round_tripping_a_spatial_structure_diagram_model : concern_for_DiagramModelToXmlMapper_with_containers
   {
      private DiagramModel _copy;

      protected override void Because()
      {
         _copy = sut.XmlDocumentToDiagramModel(sut.DiagramModelToXmlDocument(_model)) as DiagramModel;
      }

      [Observation]
      public void should_restore_the_container_hierarchy()
      {
         var organism = _copy.GetNode<ContainerNode>("organism");
         organism.GetParent().ShouldBeEqualTo(_copy);
         _copy.GetNode<ContainerNode>("plasma").GetParent().ShouldBeEqualTo(organism);
         _copy.GetNode<ContainerNode>("interstitial").GetParent().ShouldBeEqualTo(organism);
         _copy.GetNode<NeighborhoodNode>("pls_int").GetParent().ShouldBeEqualTo(organism);
         _copy.GetNode<ContainerNode>("mp").GetParent().ShouldBeEqualTo(_copy.GetNode<ContainerNode>("plasma"));
         _copy.GetAllChildren<IBaseNode>().Count().ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_restore_the_container_layout_and_flags()
      {
         var organism = _copy.GetNode<ContainerNode>("organism");
         organism.Location.ShouldBeEqualTo(_organism.Location);
         organism.Size.ShouldBeEqualTo(_organism.Size);
         organism.IsLogical.ShouldBeTrue();
         organism.IsExpanded.ShouldBeTrue();
         organism.IsExpandedByDefault.ShouldBeTrue();
         organism.LocationFixed.ShouldBeTrue();
         organism.Description.ShouldBeEqualTo("Organism\nThe organism");

         var plasma = _copy.GetNode<ContainerNode>("plasma");
         plasma.Location.ShouldBeEqualTo(new PointF(120, 230));
         plasma.Size.ShouldBeEqualTo(new SizeF(150, 60));
         plasma.IsExpanded.ShouldBeFalse();
         plasma.IsLogical.ShouldBeFalse();
      }

      [Observation]
      public void should_restore_the_expanded_layout_of_the_children_of_a_collapsed_container()
      {
         var moleculeProperties = _copy.GetNode<ContainerNode>("mp");
         moleculeProperties.Location.ShouldBeEqualTo(new PointF(135, 250));
         moleculeProperties.Size.ShouldBeEqualTo(new SizeF(120, 30));
         moleculeProperties.IsVisible.ShouldBeFalse();
         moleculeProperties.Hidden.ShouldBeFalse();
      }

      [Observation]
      public void should_restore_the_neighborhood_node_without_its_links()
      {
         var neighborhood = _copy.GetNode<NeighborhoodNode>("pls_int");
         neighborhood.Location.ShouldBeEqualTo(new PointF(295, 260));
         neighborhood.NodeSize.ShouldBeEqualTo(NodeSize.Middle);
         neighborhood.FirstNeighbor.ShouldBeNull();
         neighborhood.SecondNeighbor.ShouldBeNull();
         _copy.GetAllChildren<IBaseLink>().ShouldBeEmpty();
      }
   }

   public class When_deserializing_the_legacy_godiagram_container_xml : concern_for_DiagramModelToXmlMapper
   {
      private const string LEGACY_CONTAINER_XML = @"<DiagramModel IsLayouted=""False"">
  <SimpleContainerNode Id=""breasts"" Name=""Breasts"" Location=""285.176758 1428.38989"" Size=""173.958313 33.43799"" Hidden=""false"" IsVisible=""true"" LocationFixed=""true"" UserFlags=""1"" Description=""Breasts&#xA;Heart"" IsLogical=""true"" IsExpanded=""false"" IsExpandedByDefault=""false"">
    <SimpleContainerNode GoSubGraph.SavedBounds=""20 18.0911865 144.049438 28.0913086"" Id=""plasma"" Name=""Plasma"" Location=""285.176758 1428.38989"" Size=""173.958313 33.43799"" Hidden=""false"" IsVisible=""true"" LocationFixed=""false"" UserFlags=""1"" Description=""Plasma&#xA;Plasma"" IsLogical=""false"" IsExpanded=""false"" IsExpandedByDefault=""false"">
      <SimpleContainerNode GoSubGraph.SavedBounds=""15 18.0911865 137.0495 21.0911865"" Id=""mp"" Name=""MoleculeProperties"" Location=""280.176758 1425.38989"" Size=""173.958313 28.8638916"" Hidden=""true"" IsVisible=""false"" LocationFixed=""false"" UserFlags=""1"" Description=""MoleculeProperties&#xA;Molecule dependent properties"" IsLogical=""true"" IsExpanded=""false"" IsExpandedByDefault=""false"" />
    </SimpleContainerNode>
    <SimpleNeighborhoodNode GoSubGraph.SavedBounds=""84.52472 54.63684 15 15"" Id=""pls_int"" Name=""Breasts_pls_Breasts_int"" Location=""352.260071 1438.93555"" Size=""15 15"" Hidden=""false"" IsVisible=""true"" LocationFixed=""false"" UserFlags=""3"" Description=""Breasts_pls_Breasts_int&#xD;&#xA;Neighborhood between Heart/Plasma and Heart/Interstitial"" NodeSize=""Middle"" FirstNeighbor=""null"" SecondNeighbor=""null"" />
  </SimpleContainerNode>
  <MultiPortContainerNode Id=""ignored"" Name=""Ignored"" Location=""0 0"" Size=""10 10"">
    <MoleculeNode Id=""nested"" Name=""N"" Location=""1 1"" NodeSize=""Middle"" />
  </MultiPortContainerNode>
</DiagramModel>";

      private DiagramModel _model;

      protected override void Because()
      {
         _model = sut.XmlDocumentToDiagramModel(load(LEGACY_CONTAINER_XML)) as DiagramModel;
      }

      [Observation]
      public void should_read_the_collapsed_top_container()
      {
         var breasts = _model.GetNode<ContainerNode>("breasts");
         breasts.GetParent().ShouldBeEqualTo(_model);
         breasts.Location.ShouldBeEqualTo(new PointF(285.176758F, 1428.38989F));
         breasts.IsExpanded.ShouldBeFalse();
         breasts.IsLogical.ShouldBeTrue();
         breasts.LocationFixed.ShouldBeTrue();
         breasts.Description.ShouldBeEqualTo("Breasts\nHeart");
         breasts.UserFlags.ShouldBeEqualTo(NodeLayoutType.CONTAINER_NODE);
      }

      [Observation]
      public void should_restore_the_expanded_bounds_of_the_children_from_the_saved_bounds()
      {
         var plasma = _model.GetNode<ContainerNode>("plasma");
         plasma.Location.ShouldBeEqualTo(new PointF(285.176758F + 20, 1428.38989F + 18.0911865F));
         plasma.Size.ShouldBeEqualTo(new SizeF(144.049438F, 28.0913086F));

         var moleculeProperties = _model.GetNode<ContainerNode>("mp");
         moleculeProperties.GetParent().ShouldBeEqualTo(plasma);
         moleculeProperties.Location.ShouldBeEqualTo(new PointF(plasma.Location.X + 15, plasma.Location.Y + 18.0911865F));
         moleculeProperties.Size.ShouldBeEqualTo(new SizeF(137.0495F, 21.0911865F));
         moleculeProperties.IsVisible.ShouldBeFalse();
         moleculeProperties.Hidden.ShouldBeTrue();
         plasma.Hidden.ShouldBeFalse();
      }

      [Observation]
      public void should_enlarge_the_collapsed_containers_to_their_restored_children()
      {
         var breasts = _model.GetNode<ContainerNode>("breasts");
         var plasma = _model.GetNode<ContainerNode>("plasma");
         breasts.Bounds.Contains(plasma.Bounds).ShouldBeTrue();
         breasts.Bounds.Contains(_model.GetNode<NeighborhoodNode>("pls_int").Bounds).ShouldBeTrue();
      }

      [Observation]
      public void should_restore_the_neighborhood_node_center_from_the_saved_bounds()
      {
         var neighborhood = _model.GetNode<NeighborhoodNode>("pls_int");
         neighborhood.GetParent().ShouldBeEqualTo(_model.GetNode<ContainerNode>("breasts"));
         neighborhood.Location.ShouldBeEqualTo(new PointF(285.176758F + 84.52472F + 7.5F, 1428.38989F + 54.63684F + 7.5F));
         neighborhood.Size.ShouldBeEqualTo(new SizeF(15, 15));
         neighborhood.UserFlags.ShouldBeEqualTo(NodeLayoutType.NEIGHBORHOOD_NODE);
      }

      [Observation]
      public void should_ignore_unknown_elements_and_their_children()
      {
         _model.GetNode("ignored").ShouldBeNull();
         _model.GetNode("nested").ShouldBeNull();
         _model.GetAllChildren<IBaseNode>().Count().ShouldBeEqualTo(4);
      }
   }

   public class When_serializing_an_expanded_container_inside_a_collapsed_container : concern_for_DiagramModelToXmlMapper_with_containers
   {
      private XmlDocument _xmlDoc;
      private DiagramModel _copy;

      protected override void Context()
      {
         base.Context();
         _organism.IsExpanded = false;
         _plasma.IsExpanded = true;
      }

      protected override void Because()
      {
         _xmlDoc = sut.DiagramModelToXmlDocument(_model);
         _copy = sut.XmlDocumentToDiagramModel(_xmlDoc) as DiagramModel;
      }

      [Observation]
      public void should_write_the_nested_container_as_collapsed_like_godiagram_did()
      {
         var plasma = _xmlDoc.SelectSingleNode("//SimpleContainerNode[@Id='plasma']") as XmlElement;
         plasma.GetAttribute("IsExpanded").ShouldBeEqualTo("false");
         plasma.GetAttribute("GoSubGraph.SavedBounds").ShouldBeEqualTo("15 20 150 60");
         plasma.GetAttribute("Location").ShouldBeEqualTo("105 210");

         var moleculeProperties = plasma.ChildNodes.OfType<XmlElement>().Single();
         moleculeProperties.GetAttribute("GoSubGraph.SavedBounds").ShouldBeEqualTo("15 20 120 30");
         moleculeProperties.GetAttribute("Location").ShouldBeEqualTo("105 210");
      }

      [Observation]
      public void should_restore_the_expanded_layout_of_all_descendants()
      {
         _copy.GetNode<ContainerNode>("plasma").Location.ShouldBeEqualTo(new PointF(120, 230));
         _copy.GetNode<ContainerNode>("mp").Location.ShouldBeEqualTo(new PointF(135, 250));
         _copy.GetNode<NeighborhoodNode>("pls_int").Location.ShouldBeEqualTo(new PointF(295, 260));
      }
   }

   public class When_serializing_a_collapsed_container_as_template : concern_for_DiagramModelToXmlMapper_with_containers
   {
      private XmlDocument _xmlDoc;
      private DiagramModel _template;

      protected override void Context()
      {
         base.Context();
         _organism.IsExpanded = false;
      }

      protected override void Because()
      {
         _xmlDoc = sut.ContainerToXmlDocument(_organism);
         _template = sut.XmlDocumentToDiagramModel(_xmlDoc) as DiagramModel;
      }

      [Observation]
      public void should_write_the_direct_children_with_their_expanded_bounds_and_the_container_location_on_the_root()
      {
         _xmlDoc.DocumentElement.GetAttribute("LocationX").ShouldBeEqualTo("105");
         _xmlDoc.DocumentElement.GetAttribute("LocationY").ShouldBeEqualTo("210");
         var plasma = elementWithId(_xmlDoc, "plasma");
         plasma.HasAttribute("GoSubGraph.SavedBounds").ShouldBeFalse();
         plasma.GetAttribute("Location").ShouldBeEqualTo("120 230");
      }

      [Observation]
      public void should_read_the_template_back_with_the_container_location_and_unchanged_child_positions()
      {
         _template.Location.ShouldBeEqualTo(new PointF(105, 210));
         _template.GetNode<ContainerNode>("plasma").Location.ShouldBeEqualTo(new PointF(120, 230));
         _template.GetNode<ContainerNode>("interstitial").Location.ShouldBeEqualTo(new PointF(320, 230));
         _template.GetNode<NeighborhoodNode>("pls_int").Location.ShouldBeEqualTo(new PointF(295, 260));
      }
   }

   public class When_round_tripping_a_collapsed_container_that_has_been_drawn : concern_for_DiagramModelToXmlMapper_with_containers
   {
      private DiagramModel _copy;

      protected override void Context()
      {
         base.Context();
         _plasma.CollapsedSize = new SizeF(46, 18);
      }

      protected override void Because()
      {
         _copy = sut.XmlDocumentToDiagramModel(sut.DiagramModelToXmlDocument(_model)) as DiagramModel;
      }

      [Observation]
      public void should_write_the_size_the_container_expands_to_rather_than_the_size_it_is_drawn_at()
      {
         _copy.GetNode<ContainerNode>("plasma").Size.ShouldBeEqualTo(new SizeF(150, 60));
      }
   }
}
