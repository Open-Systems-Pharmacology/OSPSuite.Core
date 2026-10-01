using System.Drawing;
using System.Linq;
using System.Xml;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Diagram.Services;

namespace OSPSuite.Presentation.Diagram
{
   public abstract class concern_for_DiagramModelToXmlMapper : ContextSpecification<DiagramModelToXmlMapper>
   {
      protected const string LEGACY_XML = @"<DiagramModel IsLayouted=""True"">
  <ReactionNode Id=""reaction-id-1"" Name=""R1"" Location=""100 50"" Size=""38 36.072914"" Hidden=""false"" IsVisible=""true"" LocationFixed=""true"" UserFlags=""6"" Description=""R1 description"" NodeSize=""Middle"" DisplayEductsRight=""true"" />
  <MoleculeNode Id=""mol-guid-a"" Name=""A"" Location=""20 40"" Size=""32.5 22.5"" Hidden=""false"" IsVisible=""true"" LocationFixed=""false"" UserFlags=""4"" Description=""A"" NodeSize=""Large"" />
  <MoleculeNode Id=""mol-guid-b"" Name=""B"" Location=""192.5 32.5"" Size=""17.5 10.563799"" Hidden=""true"" IsVisible=""true"" LocationFixed=""false"" UserFlags=""4"" Description=""B"" NodeSize=""Small"" />
</DiagramModel>";

      protected override void Context()
      {
         sut = new DiagramModelToXmlMapper();
      }

      protected static XmlDocument load(string xml)
      {
         var xmlDoc = new XmlDocument();
         xmlDoc.LoadXml(xml);
         return xmlDoc;
      }

      protected static XmlElement elementWithId(XmlDocument xmlDoc, string id)
      {
         return xmlDoc.DocumentElement.ChildNodes.OfType<XmlElement>().Single(x => x.GetAttribute("Id") == id);
      }
   }

   public abstract class concern_for_DiagramModelToXmlMapper_with_model : concern_for_DiagramModelToXmlMapper
   {
      protected DiagramModel _model;
      protected MoleculeNode _moleculeA;
      protected MoleculeNode _moleculeB;
      protected ReactionNode _reaction;

      protected override void Context()
      {
         base.Context();
         _model = new DiagramModel {IsLayouted = true};
         _moleculeA = _model.CreateNode<MoleculeNode>("mol-a", new PointF(12.5F, -3.25F), _model);
         _moleculeA.Name = "A";
         _moleculeA.Description = "molecule A";
         _moleculeA.NodeSize = NodeSize.Small;
         _moleculeA.Hidden = true;
         _moleculeA.IsVisible = false;
         _moleculeA.LocationFixed = true;
         _moleculeB = _model.CreateNode<MoleculeNode>("mol-b", new PointF(0, 0), _model);
         _moleculeB.Name = "B";
         _moleculeB.Description = "B";
         _reaction = _model.CreateNode<ReactionNode>("r1", new PointF(100.75F, 50), _model);
         _reaction.Name = "R1";
         _reaction.Description = "reaction R1";
         _reaction.DisplayEductsRight = true;
         new ReactionLink().Initialize(ReactionLinkType.Educt, _reaction, _moleculeA);
         new ReactionLink().Initialize(ReactionLinkType.Product, _reaction, _moleculeB);
      }
   }

   public class When_retrieving_the_element_name : concern_for_DiagramModelToXmlMapper
   {
      [Observation]
      public void should_return_the_legacy_root_element_name()
      {
         sut.ElementName.ShouldBeEqualTo("DiagramModel");
      }
   }

   public class When_serializing_a_reaction_diagram_model : concern_for_DiagramModelToXmlMapper_with_model
   {
      private XmlDocument _xmlDoc;

      protected override void Because()
      {
         _xmlDoc = sut.DiagramModelToXmlDocument(_model);
      }

      [Observation]
      public void should_write_the_legacy_root_element()
      {
         _xmlDoc.DocumentElement.Name.ShouldBeEqualTo("DiagramModel");
         _xmlDoc.DocumentElement.GetAttribute("IsLayouted").ShouldBeEqualTo("True");
         _xmlDoc.DocumentElement.HasAttribute("LocationX").ShouldBeFalse();
      }

      [Observation]
      public void should_write_one_element_per_node_and_no_links()
      {
         var elements = _xmlDoc.DocumentElement.ChildNodes.OfType<XmlElement>().ToList();
         elements.Count.ShouldBeEqualTo(3);
         elements.Select(x => x.Name).ShouldOnlyContain("MoleculeNode", "MoleculeNode", "ReactionNode");
      }

      [Observation]
      public void should_write_the_molecule_node_attributes_in_the_legacy_format()
      {
         var element = elementWithId(_xmlDoc, "mol-a");
         element.Name.ShouldBeEqualTo("MoleculeNode");
         element.GetAttribute("Name").ShouldBeEqualTo("A");
         element.GetAttribute("Location").ShouldBeEqualTo("12.5 -3.25");
         element.GetAttribute("Size").ShouldBeEqualTo("7.5 7.5");
         element.GetAttribute("Hidden").ShouldBeEqualTo("true");
         element.GetAttribute("IsVisible").ShouldBeEqualTo("false");
         element.GetAttribute("LocationFixed").ShouldBeEqualTo("true");
         element.GetAttribute("UserFlags").ShouldBeEqualTo("4");
         element.GetAttribute("Description").ShouldBeEqualTo("molecule A");
         element.GetAttribute("NodeSize").ShouldBeEqualTo("Small");
         element.HasAttribute("DisplayEductsRight").ShouldBeFalse();
      }

      [Observation]
      public void should_write_the_reaction_node_attributes_in_the_legacy_format()
      {
         var element = elementWithId(_xmlDoc, "r1");
         element.Name.ShouldBeEqualTo("ReactionNode");
         element.GetAttribute("Name").ShouldBeEqualTo("R1");
         element.GetAttribute("Location").ShouldBeEqualTo("100.75 50");
         element.GetAttribute("Size").ShouldBeEqualTo("30 20");
         element.GetAttribute("Hidden").ShouldBeEqualTo("false");
         element.GetAttribute("IsVisible").ShouldBeEqualTo("true");
         element.GetAttribute("LocationFixed").ShouldBeEqualTo("false");
         element.GetAttribute("UserFlags").ShouldBeEqualTo("6");
         element.GetAttribute("Description").ShouldBeEqualTo("reaction R1");
         element.GetAttribute("NodeSize").ShouldBeEqualTo("Middle");
         element.GetAttribute("DisplayEductsRight").ShouldBeEqualTo("true");
      }
   }

   public class When_round_tripping_a_reaction_diagram_model : concern_for_DiagramModelToXmlMapper_with_model
   {
      private IDiagramModel _copy;

      protected override void Because()
      {
         _copy = sut.XmlDocumentToDiagramModel(sut.DiagramModelToXmlDocument(_model));
      }

      [Observation]
      public void should_create_a_ui_free_diagram_model()
      {
         _copy.ShouldBeAnInstanceOf<DiagramModel>();
         _copy.IsLayouted.ShouldBeTrue();
         _copy.GetAllChildren<IBaseNode>().Count().ShouldBeEqualTo(3);
      }

      [Observation]
      public void should_restore_the_molecule_nodes()
      {
         var moleculeA = _copy.GetNode<MoleculeNode>("mol-a");
         moleculeA.Name.ShouldBeEqualTo("A");
         moleculeA.Description.ShouldBeEqualTo("molecule A");
         moleculeA.Location.ShouldBeEqualTo(new PointF(12.5F, -3.25F));
         moleculeA.NodeSize.ShouldBeEqualTo(NodeSize.Small);
         moleculeA.Hidden.ShouldBeTrue();
         moleculeA.IsVisible.ShouldBeFalse();
         moleculeA.LocationFixed.ShouldBeTrue();
         moleculeA.UserFlags.ShouldBeEqualTo(4);
         moleculeA.GetParent().ShouldBeEqualTo(_copy);

         var moleculeB = _copy.GetNode<MoleculeNode>("mol-b");
         moleculeB.Name.ShouldBeEqualTo("B");
         moleculeB.NodeSize.ShouldBeEqualTo(NodeSize.Large);
         moleculeB.Hidden.ShouldBeFalse();
         moleculeB.IsVisible.ShouldBeTrue();
         moleculeB.LocationFixed.ShouldBeFalse();
      }

      [Observation]
      public void should_restore_the_reaction_node()
      {
         var reaction = _copy.GetNode<ReactionNode>("r1");
         reaction.Name.ShouldBeEqualTo("R1");
         reaction.Description.ShouldBeEqualTo("reaction R1");
         reaction.Location.ShouldBeEqualTo(new PointF(100.75F, 50));
         reaction.NodeSize.ShouldBeEqualTo(NodeSize.Middle);
         reaction.DisplayEductsRight.ShouldBeTrue();
         reaction.UserFlags.ShouldBeEqualTo(6);
      }

      [Observation]
      public void should_not_restore_links()
      {
         _copy.GetAllChildren<ReactionLink>().ShouldBeEmpty();
      }
   }

   public class When_deserializing_the_legacy_godiagram_xml : concern_for_DiagramModelToXmlMapper
   {
      private DiagramModel _model;
      private int _changedCount;

      protected override void Context()
      {
         base.Context();
         _model = new DiagramModel();
         _changedCount = 0;
         _model.Changed += () => _changedCount++;
      }

      protected override void Because()
      {
         sut.Deserialize(_model, load(LEGACY_XML));
      }

      [Observation]
      public void should_read_the_layout_state()
      {
         _model.IsLayouted.ShouldBeTrue();
         _model.GetAllChildren<IBaseNode>().Count().ShouldBeEqualTo(3);
      }

      [Observation]
      public void should_read_the_reaction_node()
      {
         var reaction = _model.GetNode<ReactionNode>("reaction-id-1");
         reaction.Name.ShouldBeEqualTo("R1");
         reaction.Location.ShouldBeEqualTo(new PointF(100, 50));
         reaction.Hidden.ShouldBeFalse();
         reaction.IsVisible.ShouldBeTrue();
         reaction.LocationFixed.ShouldBeTrue();
         reaction.UserFlags.ShouldBeEqualTo(6);
         reaction.Description.ShouldBeEqualTo("R1 description");
         reaction.NodeSize.ShouldBeEqualTo(NodeSize.Middle);
         reaction.DisplayEductsRight.ShouldBeTrue();
         reaction.Size.ShouldBeEqualTo(new SizeF(30, 20));
      }

      [Observation]
      public void should_read_the_molecule_nodes()
      {
         var moleculeA = _model.GetNode<MoleculeNode>("mol-guid-a");
         moleculeA.Name.ShouldBeEqualTo("A");
         moleculeA.Location.ShouldBeEqualTo(new PointF(20, 40));
         moleculeA.Hidden.ShouldBeFalse();
         moleculeA.IsVisible.ShouldBeTrue();
         moleculeA.LocationFixed.ShouldBeFalse();
         moleculeA.UserFlags.ShouldBeEqualTo(4);
         moleculeA.Description.ShouldBeEqualTo("A");
         moleculeA.NodeSize.ShouldBeEqualTo(NodeSize.Large);

         var moleculeB = _model.GetNode<MoleculeNode>("mol-guid-b");
         moleculeB.Name.ShouldBeEqualTo("B");
         moleculeB.Location.ShouldBeEqualTo(new PointF(192.5F, 32.5F));
         moleculeB.Hidden.ShouldBeTrue();
         moleculeB.NodeSize.ShouldBeEqualTo(NodeSize.Small);
      }

      [Observation]
      public void should_ignore_the_serialized_size()
      {
         _model.GetNode<MoleculeNode>("mol-guid-a").Size.ShouldBeEqualTo(new SizeF(22.5F, 22.5F));
         _model.GetNode<MoleculeNode>("mol-guid-b").Size.ShouldBeEqualTo(new SizeF(7.5F, 7.5F));
      }

      [Observation]
      public void should_raise_the_changed_event_once()
      {
         _changedCount.ShouldBeEqualTo(1);
      }
   }

   public class When_deserializing_xml_with_unknown_elements : concern_for_DiagramModelToXmlMapper
   {
      private IDiagramModel _model;

      protected override void Because()
      {
         _model = sut.XmlDocumentToDiagramModel(load(@"<DiagramModel IsLayouted=""False"">
  <SimpleContainerNode Id=""c"" Name=""C"" Location=""0 0"" Size=""10 10"" IsExpanded=""true"">
    <MoleculeNode Id=""nested"" Name=""N"" Location=""1 1"" NodeSize=""Middle"" />
  </SimpleContainerNode>
  <MoleculeNode Id=""mol-guid-a"" Name=""A"" Location=""20 40"" NodeSize=""Large"" />
</DiagramModel>"));
      }

      [Observation]
      public void should_only_read_the_known_reaction_diagram_nodes()
      {
         _model.GetAllChildren<IBaseNode>().Count().ShouldBeEqualTo(1);
         _model.GetNode("mol-guid-a").ShouldNotBeNull();
         _model.GetNode("c").ShouldBeNull();
         _model.GetNode("nested").ShouldBeNull();
         _model.IsLayouted.ShouldBeFalse();
      }
   }

   public class When_deserializing_nodes_without_node_size_into_a_model_with_diagram_options : concern_for_DiagramModelToXmlMapper
   {
      private DiagramModel _model;

      protected override void Context()
      {
         base.Context();
         _model = new DiagramModel {DiagramOptions = new DiagramOptions {DefaultNodeSizeMolecule = NodeSize.Small, DefaultNodeSizeReaction = NodeSize.Large}};
      }

      protected override void Because()
      {
         sut.Deserialize(_model, load(@"<DiagramModel>
  <MoleculeNode Id=""a"" Name=""A"" Location=""20 40"" />
  <MoleculeNode Id=""b"" Name=""B"" Location=""20 40"" NodeSize=""Middle"" />
  <ReactionNode Id=""r"" Name=""R"" Location=""20 40"" />
</DiagramModel>"));
      }

      [Observation]
      public void should_apply_the_default_node_sizes_unless_the_node_size_is_serialized()
      {
         _model.GetNode<MoleculeNode>("a").NodeSize.ShouldBeEqualTo(NodeSize.Small);
         _model.GetNode<MoleculeNode>("b").NodeSize.ShouldBeEqualTo(NodeSize.Middle);
         _model.GetNode<ReactionNode>("r").NodeSize.ShouldBeEqualTo(NodeSize.Large);
      }

      [Observation]
      public void should_keep_the_default_layout_flags()
      {
         var node = _model.GetNode<MoleculeNode>("a");
         node.Hidden.ShouldBeFalse();
         node.IsVisible.ShouldBeTrue();
         node.LocationFixed.ShouldBeFalse();
         node.UserFlags.ShouldBeEqualTo(NodeLayoutType.MOLECULE_NODE);
         node.Description.ShouldBeEqualTo(string.Empty);
      }
   }

   public class When_serializing_a_model_as_container : concern_for_DiagramModelToXmlMapper_with_model
   {
      private XmlDocument _xmlDoc;

      protected override void Because()
      {
         _xmlDoc = sut.ContainerToXmlDocument(_model);
      }

      [Observation]
      public void should_add_the_location_of_the_container_to_the_root_element()
      {
         _xmlDoc.DocumentElement.Name.ShouldBeEqualTo("DiagramModel");
         _xmlDoc.DocumentElement.GetAttribute("LocationX").ShouldBeEqualTo("-11.25");
         _xmlDoc.DocumentElement.GetAttribute("LocationY").ShouldBeEqualTo("-11.25");
         _xmlDoc.DocumentElement.GetAttribute("IsLayouted").ShouldBeEqualTo("True");
      }

      [Observation]
      public void should_write_the_nodes()
      {
         _xmlDoc.DocumentElement.ChildNodes.OfType<XmlElement>().Count().ShouldBeEqualTo(3);
      }
   }

   public class When_deserializing_xml_with_a_container_location : concern_for_DiagramModelToXmlMapper
   {
      private DiagramModel _model;

      protected override void Because()
      {
         _model = sut.XmlDocumentToDiagramModel(load(@"<DiagramModel IsLayouted=""False"" LocationX=""10"" LocationY=""20"">
  <MoleculeNode Id=""a"" Name=""A"" Location=""20 40"" NodeSize=""Large"" />
</DiagramModel>")) as DiagramModel;
      }

      [Observation]
      public void should_translate_the_model_to_the_serialized_location()
      {
         _model.Location.ShouldBeEqualTo(new PointF(10, 20));
         _model.GetNode<MoleculeNode>("a").Location.ShouldBeEqualTo(new PointF(21.25F, 31.25F));
      }
   }
}
