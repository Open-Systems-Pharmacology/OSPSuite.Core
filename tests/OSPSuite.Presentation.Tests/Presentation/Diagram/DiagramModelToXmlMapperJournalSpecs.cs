using System;
using System.Drawing;
using System.Linq;
using System.Xml;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Diagram.Services;

namespace OSPSuite.Presentation.Diagram
{
   public abstract class concern_for_DiagramModelToXmlMapper_with_journal_model : ContextSpecification<DiagramModelToXmlMapper>
   {
      protected const string LEGACY_JOURNAL_XML = @"<DiagramModel IsLayouted=""True"">
  <JournalPageNode Id=""page-a"" Name=""Page A"" Location=""120 65"" Size=""120 65"" Hidden=""false"" IsVisible=""true"" LocationFixed=""true"" UserFlags=""0"" Description=""tooltip A"" NodeSize=""Middle"" IsExpanded=""false"" />
  <JournalPageNode Id=""page-b"" Name=""Page B"" Location=""353 62"" Size=""120 65"" Hidden=""false"" IsVisible=""true"" LocationFixed=""false"" UserFlags=""0"" Description=""tooltip B"" NodeSize=""Middle"" IsExpanded=""true"" />
  <RelatedItemNode Id=""item-a"" Name=""  Sim 1 (Simulation)"" Location=""80 127.5"" Size=""20 20"" Hidden=""false"" IsVisible=""true"" LocationFixed=""false"" UserFlags=""0"" Description=""tooltip item"" NodeSize=""Middle"" />
  <JournalPageLink Id=""link-1"" FromPort=""page-a:childPort"" ToPort=""page-b:parentPort"" Style=""Bezier"" />
  <RelatedItemLink Id=""link-2"" FromPort=""page-a:relatedItemPort"" ToPort=""item-a:port"" Style=""Line"" />
</DiagramModel>";

      protected DiagramModel _model;
      protected JournalPageNode _page1;
      protected JournalPageNode _page2;
      protected RelatedItemNode _item;

      protected override void Context()
      {
         sut = new DiagramModelToXmlMapper();
         _model = new DiagramModel {IsLayouted = true};

         _page1 = _model.CreateNode<JournalPageNode>("page-1", new PointF(120, 65), _model);
         _page1.UpdateAttributesFrom(pageWith("page-1", 3, "First page"));
         _page1.Description = "first tooltip";
         _page1.IsExpanded = false;
         _page1.LocationFixed = true;
         _page1.UserFlags = 5;

         _page2 = _model.CreateNode<JournalPageNode>("page-2", new PointF(353, 62), _model);
         _page2.UpdateAttributesFrom(pageWith("page-2", 4, "Second page"));
         _page2.Description = "second tooltip";

         _item = _model.CreateNode<RelatedItemNode>("item-1", new PointF(80, 127.5F), _model);
         _item.UpdateAttributesFromItem(new RelatedItem {Id = "item-1", Name = "Sim 1", ItemType = "Simulation"});
         _item.Description = "item tooltip";
         _item.Hidden = true;

         new JournalPageLink().Initialize(_page1, _page2);
         new RelatedItemLink().Initialize(_page1, _item);
      }

      private static JournalPage pageWith(string id, int uniqueIndex, string title)
      {
         return new JournalPage {Id = id, UniqueIndex = uniqueIndex, Title = title, CreatedAt = new DateTime(2024, 3, 5, 10, 30, 0, DateTimeKind.Utc)};
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

   public class When_serializing_a_journal_diagram_model : concern_for_DiagramModelToXmlMapper_with_journal_model
   {
      private XmlDocument _xmlDoc;

      protected override void Because()
      {
         _xmlDoc = sut.DiagramModelToXmlDocument(_model);
      }

      [Observation]
      public void should_write_one_element_per_node_and_no_link_elements()
      {
         var elements = _xmlDoc.DocumentElement.ChildNodes.OfType<XmlElement>().ToList();
         elements.Count.ShouldBeEqualTo(3);
         elements.Select(x => x.Name).ShouldOnlyContain("JournalPageNode", "JournalPageNode", "RelatedItemNode");
      }

      [Observation]
      public void should_write_the_journal_page_node_attributes_in_the_legacy_format()
      {
         var element = elementWithId(_xmlDoc, "page-1");
         element.Name.ShouldBeEqualTo("JournalPageNode");
         element.GetAttribute("Name").ShouldBeEqualTo("First page");
         element.GetAttribute("Location").ShouldBeEqualTo("120 65");
         element.GetAttribute("Size").ShouldBeEqualTo("120 65");
         element.GetAttribute("Hidden").ShouldBeEqualTo("false");
         element.GetAttribute("IsVisible").ShouldBeEqualTo("true");
         element.GetAttribute("LocationFixed").ShouldBeEqualTo("true");
         element.GetAttribute("UserFlags").ShouldBeEqualTo("5");
         element.GetAttribute("Description").ShouldBeEqualTo("first tooltip");
         element.GetAttribute("NodeSize").ShouldBeEqualTo("Middle");
         element.GetAttribute("IsExpanded").ShouldBeEqualTo("false");
      }

      [Observation]
      public void should_write_the_expansion_state_of_an_expanded_page()
      {
         elementWithId(_xmlDoc, "page-2").GetAttribute("IsExpanded").ShouldBeEqualTo("true");
      }

      [Observation]
      public void should_not_write_the_unique_index_or_the_text()
      {
         var element = elementWithId(_xmlDoc, "page-1");
         element.HasAttribute("UniqueIndex").ShouldBeFalse();
         element.HasAttribute("Text").ShouldBeFalse();
      }

      [Observation]
      public void should_write_the_related_item_node_attributes_in_the_legacy_format()
      {
         var element = elementWithId(_xmlDoc, "item-1");
         element.Name.ShouldBeEqualTo("RelatedItemNode");
         element.GetAttribute("Name").ShouldBeEqualTo("  Sim 1 (Simulation)");
         element.GetAttribute("Location").ShouldBeEqualTo("80 127.5");
         element.GetAttribute("Size").ShouldBeEqualTo("20 20");
         element.GetAttribute("Hidden").ShouldBeEqualTo("true");
         element.GetAttribute("Description").ShouldBeEqualTo("item tooltip");
         element.GetAttribute("NodeSize").ShouldBeEqualTo("Middle");
         element.HasAttribute("IsExpanded").ShouldBeFalse();
      }
   }

   public class When_round_tripping_a_journal_diagram_model : concern_for_DiagramModelToXmlMapper_with_journal_model
   {
      private IDiagramModel _copy;

      protected override void Because()
      {
         _copy = sut.XmlDocumentToDiagramModel(sut.DiagramModelToXmlDocument(_model));
      }

      [Observation]
      public void should_create_a_ui_free_diagram_model_with_the_journal_nodes()
      {
         _copy.ShouldBeAnInstanceOf<DiagramModel>();
         _copy.IsLayouted.ShouldBeTrue();
         _copy.GetAllChildren<IBaseNode>().Count().ShouldBeEqualTo(3);
      }

      [Observation]
      public void should_restore_the_collapsed_journal_page_node()
      {
         var page = _copy.GetNode<JournalPageNode>("page-1");
         page.Name.ShouldBeEqualTo("First page");
         page.Description.ShouldBeEqualTo("first tooltip");
         page.Location.ShouldBeEqualTo(new PointF(120, 65));
         page.Size.ShouldBeEqualTo(new SizeF(120, 65));
         page.NodeSize.ShouldBeEqualTo(NodeSize.Middle);
         page.IsExpanded.ShouldBeFalse();
         page.LocationFixed.ShouldBeTrue();
         page.UserFlags.ShouldBeEqualTo(5);
         page.GetParent().ShouldBeEqualTo(_copy);
      }

      [Observation]
      public void should_restore_the_expanded_journal_page_node()
      {
         var page = _copy.GetNode<JournalPageNode>("page-2");
         page.Name.ShouldBeEqualTo("Second page");
         page.Location.ShouldBeEqualTo(new PointF(353, 62));
         page.IsExpanded.ShouldBeTrue();
         page.LocationFixed.ShouldBeFalse();
      }

      [Observation]
      public void should_not_restore_the_unique_index_or_the_text()
      {
         var page = _copy.GetNode<JournalPageNode>("page-1");
         page.UniqueIndex.ShouldBeEqualTo(0);
         page.Text.ShouldBeEqualTo(string.Empty);
      }

      [Observation]
      public void should_restore_the_related_item_node()
      {
         var item = _copy.GetNode<RelatedItemNode>("item-1");
         item.Name.ShouldBeEqualTo("  Sim 1 (Simulation)");
         item.Description.ShouldBeEqualTo("item tooltip");
         item.Location.ShouldBeEqualTo(new PointF(80, 127.5F));
         item.Size.ShouldBeEqualTo(new SizeF(20, 20));
         item.Hidden.ShouldBeTrue();
         item.CanLink.ShouldBeFalse();
      }

      [Observation]
      public void should_not_restore_links()
      {
         _copy.GetAllChildren<IBaseLink>().ShouldBeEmpty();
         _copy.GetNode<JournalPageNode>("page-2").ParentPageNode.ShouldBeNull();
         _copy.GetNode<JournalPageNode>("page-1").RelatedItemNodes.ShouldBeEmpty();
         _copy.GetNode<RelatedItemNode>("item-1").PageNode.ShouldBeNull();
      }
   }

   public class When_deserializing_the_legacy_godiagram_journal_xml : concern_for_DiagramModelToXmlMapper_with_journal_model
   {
      private IDiagramModel _legacyModel;

      protected override void Because()
      {
         _legacyModel = sut.XmlDocumentToDiagramModel(load(LEGACY_JOURNAL_XML));
      }

      [Observation]
      public void should_read_the_journal_page_nodes_with_their_expansion_state()
      {
         var pageA = _legacyModel.GetNode<JournalPageNode>("page-a");
         pageA.Name.ShouldBeEqualTo("Page A");
         pageA.Description.ShouldBeEqualTo("tooltip A");
         pageA.Location.ShouldBeEqualTo(new PointF(120, 65));
         pageA.LocationFixed.ShouldBeTrue();
         pageA.IsExpanded.ShouldBeFalse();
         pageA.NodeSize.ShouldBeEqualTo(NodeSize.Middle);

         var pageB = _legacyModel.GetNode<JournalPageNode>("page-b");
         pageB.Name.ShouldBeEqualTo("Page B");
         pageB.Location.ShouldBeEqualTo(new PointF(353, 62));
         pageB.IsExpanded.ShouldBeTrue();
      }

      [Observation]
      public void should_read_the_related_item_node()
      {
         var item = _legacyModel.GetNode<RelatedItemNode>("item-a");
         item.Name.ShouldBeEqualTo("  Sim 1 (Simulation)");
         item.Description.ShouldBeEqualTo("tooltip item");
         item.Location.ShouldBeEqualTo(new PointF(80, 127.5F));
         item.Size.ShouldBeEqualTo(new SizeF(20, 20));
      }

      [Observation]
      public void should_ignore_the_legacy_link_elements()
      {
         _legacyModel.IsLayouted.ShouldBeTrue();
         _legacyModel.GetAllChildren<IBaseNode>().Count().ShouldBeEqualTo(3);
         _legacyModel.GetAllChildren<IBaseLink>().ShouldBeEmpty();
         _legacyModel.GetNode("link-1").ShouldBeNull();
         _legacyModel.GetNode("link-2").ShouldBeNull();
      }
   }

   public class When_deserializing_a_journal_page_node_without_expansion_state : concern_for_DiagramModelToXmlMapper_with_journal_model
   {
      private IDiagramModel _legacyModel;

      protected override void Because()
      {
         _legacyModel = sut.XmlDocumentToDiagramModel(load(@"<DiagramModel>
  <JournalPageNode Id=""page-a"" Name=""Page A"" Location=""120 65"" />
</DiagramModel>"));
      }

      [Observation]
      public void should_create_an_expanded_page_node_with_the_journal_page_size()
      {
         var page = _legacyModel.GetNode<JournalPageNode>("page-a");
         page.IsExpanded.ShouldBeTrue();
         page.Size.ShouldBeEqualTo(new SizeF(120, 65));
         page.Description.ShouldBeEqualTo(string.Empty);
      }
   }
}
