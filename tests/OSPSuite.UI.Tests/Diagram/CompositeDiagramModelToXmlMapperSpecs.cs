using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.UI.Diagram.Services;
using GoDiagramModel = OSPSuite.UI.Diagram.Elements.DiagramModel;
using GoMoleculeNode = OSPSuite.UI.Diagram.Elements.MoleculeNode;
using GoReactionNode = OSPSuite.UI.Diagram.Elements.ReactionNode;
using GoContainerNode = OSPSuite.UI.Diagram.Elements.SimpleContainerNode;

namespace OSPSuite.UI.Diagram
{
   //transitional: removed together with GoDiagram once every diagram uses the UI-free model
   public abstract class concern_for_CompositeDiagramModelToXmlMapper : ContextSpecification<CompositeDiagramModelToXmlMapper>
   {
      protected const string REACTION_DIAGRAM_XML = @"<DiagramModel IsLayouted=""True"">
  <ReactionNode Id=""r1"" Name=""R1"" Location=""100 50"" Size=""38 36"" Hidden=""false"" IsVisible=""true"" LocationFixed=""true"" UserFlags=""6"" Description=""R1"" NodeSize=""Middle"" DisplayEductsRight=""true"" />
  <MoleculeNode Id=""m1"" Name=""A"" Location=""20 40"" Size=""32.5 22.5"" Hidden=""false"" IsVisible=""true"" LocationFixed=""false"" UserFlags=""4"" Description=""A"" NodeSize=""Large"" />
</DiagramModel>";

      protected const string EMPTY_XML = @"<DiagramModel IsLayouted=""False"" />";

      protected const string CONTAINER_DIAGRAM_XML = @"<DiagramModel IsLayouted=""True"">
  <SimpleContainerNode Id=""c1"" Name=""Organism"" Location=""0 0"" Size=""100 60"" Hidden=""false"" IsVisible=""true"" LocationFixed=""false"" UserFlags=""1"" Description="""" IsLogical=""false"" IsExpanded=""true"" IsExpandedByDefault=""true"">
    <MoleculeNode Id=""m1"" Name=""A"" Location=""20 40"" Size=""32.5 22.5"" Hidden=""false"" IsVisible=""true"" LocationFixed=""false"" UserFlags=""4"" Description=""A"" NodeSize=""Large"" />
  </SimpleContainerNode>
</DiagramModel>";

      protected override void Context()
      {
         sut = new CompositeDiagramModelToXmlMapper();
      }

      protected static XmlDocument load(string xml)
      {
         var xmlDoc = new XmlDocument();
         xmlDoc.LoadXml(xml);
         return xmlDoc;
      }

      protected static string[] childElementNames(XmlDocument xmlDoc)
      {
         return xmlDoc.DocumentElement.ChildNodes.OfType<XmlElement>().Select(x => x.Name).ToArray();
      }
   }

   public class When_retrieving_the_element_name_of_the_composite_mapper : concern_for_CompositeDiagramModelToXmlMapper
   {
      [Observation]
      public void should_return_the_legacy_root_element_name()
      {
         sut.ElementName.ShouldBeEqualTo("DiagramModel");
      }
   }

   public class When_creating_a_diagram_model_from_xml_containing_only_reaction_and_molecule_nodes : concern_for_CompositeDiagramModelToXmlMapper
   {
      private IDiagramModel _result;

      protected override void Because()
      {
         _result = sut.XmlDocumentToDiagramModel(load(REACTION_DIAGRAM_XML));
      }

      [Observation]
      public void should_create_the_go_diagram_model()
      {
         _result.ShouldBeAnInstanceOf<GoDiagramModel>();
      }
   }

   public class When_creating_a_diagram_model_from_xml_with_an_empty_root : concern_for_CompositeDiagramModelToXmlMapper
   {
      private IDiagramModel _result;

      protected override void Because()
      {
         _result = sut.XmlDocumentToDiagramModel(load(EMPTY_XML));
      }

      [Observation]
      public void should_create_the_go_diagram_model()
      {
         _result.ShouldBeAnInstanceOf<GoDiagramModel>();
      }
   }

   public class When_creating_a_diagram_model_from_xml_containing_a_container_node : concern_for_CompositeDiagramModelToXmlMapper
   {
      private IDiagramModel _result;

      protected override void Because()
      {
         _result = sut.XmlDocumentToDiagramModel(load(CONTAINER_DIAGRAM_XML));
      }

      [Observation]
      public void should_create_the_go_diagram_model()
      {
         _result.ShouldBeAnInstanceOf<GoDiagramModel>();
      }

      [Observation]
      public void should_read_the_container_and_its_children()
      {
         _result.GetNode<IContainerNode>("c1").ShouldNotBeNull();
         _result.GetNode<IMoleculeNode>("m1").ShouldNotBeNull();
      }
   }

   public class When_serializing_a_ui_free_reaction_diagram_model : concern_for_CompositeDiagramModelToXmlMapper
   {
      private DiagramModel _model;
      private XmlDocument _xmlDoc;

      protected override void Context()
      {
         base.Context();
         _model = new DiagramModel {IsLayouted = true};
         var molecule = _model.CreateNode<MoleculeNode>("m1", new PointF(20, 40), _model);
         var reaction = _model.CreateNode<ReactionNode>("r1", new PointF(100, 50), _model);
         new ReactionLink().Initialize(ReactionLinkType.Educt, reaction, molecule);
      }

      protected override void Because()
      {
         _xmlDoc = sut.DiagramModelToXmlDocument(_model);
      }

      [Observation]
      public void should_use_the_ui_free_mapper()
      {
         _xmlDoc.DocumentElement.Name.ShouldBeEqualTo("DiagramModel");
         _xmlDoc.DocumentElement.GetAttribute("IsLayouted").ShouldBeEqualTo("True");
         childElementNames(_xmlDoc).ShouldOnlyContain("MoleculeNode", "ReactionNode");
      }

      [Observation]
      public void should_be_readable_into_a_ui_free_model()
      {
         var model = new DiagramModel();
         sut.Deserialize(model, _xmlDoc);
         model.GetNode<MoleculeNode>("m1").Location.ShouldBeEqualTo(new PointF(20, 40));
         model.GetNode<ReactionNode>("r1").Location.ShouldBeEqualTo(new PointF(100, 50));
      }
   }

   public class When_serializing_a_go_diagram_model : concern_for_CompositeDiagramModelToXmlMapper
   {
      private GoDiagramModel _model;
      private XmlDocument _xmlDoc;

      protected override void Context()
      {
         base.Context();
         _model = new GoDiagramModel {IsLayouted = true};
         _model.CreateNode<GoMoleculeNode>("m1", new PointF(20, 40), _model);
         _model.CreateNode<GoReactionNode>("r1", new PointF(100, 50), _model);
      }

      protected override void Because()
      {
         _xmlDoc = sut.DiagramModelToXmlDocument(_model);
      }

      [Observation]
      public void should_write_the_legacy_document()
      {
         _xmlDoc.DocumentElement.Name.ShouldBeEqualTo("DiagramModel");
         _xmlDoc.DocumentElement.GetAttribute("IsLayouted").ShouldBeEqualTo("True");
         childElementNames(_xmlDoc).ShouldOnlyContain("MoleculeNode", "ReactionNode");
      }
   }

   public class When_deserializing_xml_into_a_ui_free_diagram_model : concern_for_CompositeDiagramModelToXmlMapper
   {
      private DiagramModel _model;

      protected override void Context()
      {
         base.Context();
         _model = new DiagramModel();
      }

      protected override void Because()
      {
         sut.Deserialize(_model, load(REACTION_DIAGRAM_XML));
      }

      [Observation]
      public void should_create_ui_free_nodes()
      {
         _model.GetNode("r1").ShouldBeAnInstanceOf<ReactionNode>();
         _model.GetNode("m1").ShouldBeAnInstanceOf<MoleculeNode>();
         _model.IsLayouted.ShouldBeTrue();
      }
   }

   public class When_deserializing_xml_into_a_go_diagram_model : concern_for_CompositeDiagramModelToXmlMapper
   {
      private GoDiagramModel _model;

      protected override void Context()
      {
         base.Context();
         _model = new GoDiagramModel();
      }

      protected override void Because()
      {
         sut.Deserialize(_model, load(REACTION_DIAGRAM_XML));
      }

      [Observation]
      public void should_create_go_nodes()
      {
         _model.GetNode("r1").ShouldBeAnInstanceOf<GoReactionNode>();
         _model.GetNode("m1").ShouldBeAnInstanceOf<GoMoleculeNode>();
         _model.IsLayouted.ShouldBeTrue();
      }
   }

   public class When_serializing_ui_free_containers : concern_for_CompositeDiagramModelToXmlMapper
   {
      private DiagramModel _model;
      private ContainerNode _container;

      protected override void Context()
      {
         base.Context();
         _model = new DiagramModel();
         _model.CreateNode<MoleculeNode>("m1", new PointF(20, 40), _model);
         _container = _model.CreateNode<ContainerNode>("c1", new PointF(5, 7), _model);
         _model.CreateNode<ReactionNode>("r1", new PointF(100, 50), _container);
      }

      [Observation]
      public void should_write_the_location_of_the_model()
      {
         var xmlDoc = sut.ContainerToXmlDocument(_model);
         xmlDoc.DocumentElement.Name.ShouldBeEqualTo("DiagramModel");
         xmlDoc.DocumentElement.HasAttribute("LocationX").ShouldBeTrue();
         xmlDoc.DocumentElement.HasAttribute("LocationY").ShouldBeTrue();
      }

      [Observation]
      public void should_write_the_children_of_a_container_node_with_its_location()
      {
         var xmlDoc = sut.ContainerToXmlDocument(_container);
         xmlDoc.DocumentElement.Name.ShouldBeEqualTo("DiagramModel");
         xmlDoc.DocumentElement.GetAttribute("LocationX").ShouldBeEqualTo(_container.Location.X.ToString(CultureInfo.InvariantCulture));
         xmlDoc.DocumentElement.GetAttribute("LocationY").ShouldBeEqualTo(_container.Location.Y.ToString(CultureInfo.InvariantCulture));
         childElementNames(xmlDoc).ShouldOnlyContain("ReactionNode");
      }
   }

   public class When_serializing_a_go_container : concern_for_CompositeDiagramModelToXmlMapper
   {
      private GoDiagramModel _model;
      private GoContainerNode _container;
      private XmlDocument _xmlDoc;

      protected override void Context()
      {
         base.Context();
         _model = new GoDiagramModel();
         _container = _model.CreateNode<GoContainerNode>("c1", new PointF(0, 0), _model);
         _model.CreateNode<GoMoleculeNode>("m1", new PointF(20, 40), _container);
      }

      protected override void Because()
      {
         _xmlDoc = sut.ContainerToXmlDocument(_container);
      }

      [Observation]
      public void should_use_the_go_serializer()
      {
         _xmlDoc.DocumentElement.Name.ShouldBeEqualTo("DiagramModel");
         _xmlDoc.DocumentElement.HasAttribute("LocationX").ShouldBeTrue();
         childElementNames(_xmlDoc).ShouldOnlyContain("MoleculeNode");
      }
   }
}
