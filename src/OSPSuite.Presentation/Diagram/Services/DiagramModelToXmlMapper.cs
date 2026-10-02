using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Diagram.Services
{
   public class DiagramModelToXmlMapper : IDiagramModelToXmlMapper, IContainerBaseXmlSerializer
   {
      private const string IS_LAYOUTED = "IsLayouted";
      private const string LOCATION_X = "LocationX";
      private const string LOCATION_Y = "LocationY";
      private const string ID = "Id";
      private const string NAME = "Name";
      private const string LOCATION = "Location";
      private const string SIZE = "Size";
      private const string HIDDEN = "Hidden";
      private const string IS_VISIBLE = "IsVisible";
      private const string LOCATION_FIXED = "LocationFixed";
      private const string USER_FLAGS = "UserFlags";
      private const string DESCRIPTION = "Description";
      private const string NODE_SIZE = "NodeSize";
      private const string DISPLAY_EDUCTS_RIGHT = "DisplayEductsRight";
      private const string IS_LOGICAL = "IsLogical";
      private const string IS_EXPANDED = "IsExpanded";
      private const string IS_EXPANDED_BY_DEFAULT = "IsExpandedByDefault";
      private const string SAVED_BOUNDS = "GoSubGraph.SavedBounds";
      private const string FIRST_NEIGHBOR = "FirstNeighbor";
      private const string SECOND_NEIGHBOR = "SecondNeighbor";
      private const string NULL_REFERENCE = "null";
      private const string REACTION_NODE = "ReactionNode";
      private const string MOLECULE_NODE = "MoleculeNode";
      private const string CONTAINER_NODE = "SimpleContainerNode";
      private const string NEIGHBORHOOD_NODE = "SimpleNeighborhoodNode";
      private const string JOURNAL_PAGE_NODE = "JournalPageNode";
      private const string RELATED_ITEM_NODE = "RelatedItemNode";

      public string ElementName => Constants.Serialization.DIAGRAM_MODEL;

      public XmlDocument DiagramModelToXmlDocument(IDiagramModel diagramModel)
      {
         var xmlDoc = documentFor(diagramModel, null);
         xmlDoc.DocumentElement.SetAttribute(IS_LAYOUTED, diagramModel.IsLayouted.ToString());
         return xmlDoc;
      }

      public XmlDocument ContainerToXmlDocument(IContainerBase containerBase)
      {
         var xmlDoc = containerBase is IDiagramModel diagramModel ? DiagramModelToXmlDocument(diagramModel) : documentFor(containerBase, null);
         xmlDoc.DocumentElement.SetAttribute(LOCATION_X, floatToString(containerBase.Location.X));
         xmlDoc.DocumentElement.SetAttribute(LOCATION_Y, floatToString(containerBase.Location.Y));
         return xmlDoc;
      }

      private XmlDocument documentFor(IContainerBase containerBase, PointF? collapsedOrigin)
      {
         var xmlDoc = new XmlDocument();
         var root = xmlDoc.CreateElement(ElementName);
         xmlDoc.AppendChild(root);
         appendChildElements(xmlDoc, root, containerBase, collapsedOrigin);
         return xmlDoc;
      }

      private void appendChildElements(XmlDocument xmlDoc, XmlElement parentElement, IContainerBase containerBase, PointF? collapsedOrigin)
      {
         foreach (var node in containerBase.GetDirectChildren<IBaseNode>())
         {
            var element = elementFor(xmlDoc, node, collapsedOrigin);
            if (element == null) continue;

            parentElement.AppendChild(element);
            if (node is ContainerNode containerNode)
               appendChildElements(xmlDoc, element, containerNode, collapsedOriginFor(containerNode, collapsedOrigin));
         }
      }

      private static PointF? collapsedOriginFor(IContainerBase containerBase, PointF? collapsedOrigin)
      {
         if (collapsedOrigin.HasValue)
            return collapsedOrigin;
         return containerBase is ContainerNode containerNode && !containerNode.IsExpanded ? containerNode.Location : (PointF?) null;
      }

      private XmlElement elementFor(XmlDocument xmlDoc, IBaseNode node, PointF? collapsedOrigin)
      {
         switch (node)
         {
            case ReactionNode reactionNode:
               var reactionElement = elementFor(xmlDoc, reactionNode, REACTION_NODE, collapsedOrigin);
               reactionElement.SetAttribute(DISPLAY_EDUCTS_RIGHT, XmlConvert.ToString(reactionNode.DisplayEductsRight));
               return reactionElement;
            case MoleculeNode moleculeNode:
               return elementFor(xmlDoc, moleculeNode, MOLECULE_NODE, collapsedOrigin);
            case JournalPageNode journalPageNode:
               var journalPageElement = elementFor(xmlDoc, journalPageNode, JOURNAL_PAGE_NODE, collapsedOrigin);
               journalPageElement.SetAttribute(IS_EXPANDED, XmlConvert.ToString(journalPageNode.IsExpanded));
               return journalPageElement;
            case RelatedItemNode relatedItemNode:
               return elementFor(xmlDoc, relatedItemNode, RELATED_ITEM_NODE, collapsedOrigin);
            case NeighborhoodNode neighborhoodNode:
               var neighborhoodElement = elementFor(xmlDoc, neighborhoodNode, NEIGHBORHOOD_NODE, collapsedOrigin);
               neighborhoodElement.SetAttribute(FIRST_NEIGHBOR, NULL_REFERENCE);
               neighborhoodElement.SetAttribute(SECOND_NEIGHBOR, NULL_REFERENCE);
               return neighborhoodElement;
            case ContainerNode containerNode:
               var containerElement = xmlDoc.CreateElement(CONTAINER_NODE);
               setBaseAttributes(containerElement, containerNode, containerNode.Location, containerNode.Bounds, collapsedOrigin);
               containerElement.SetAttribute(IS_LOGICAL, XmlConvert.ToString(containerNode.IsLogical));
               containerElement.SetAttribute(IS_EXPANDED, XmlConvert.ToString(containerNode.IsExpanded && !collapsedOrigin.HasValue));
               containerElement.SetAttribute(IS_EXPANDED_BY_DEFAULT, XmlConvert.ToString(containerNode.IsExpandedByDefault));
               return containerElement;
            default:
               return null;
         }
      }

      private XmlElement elementFor(XmlDocument xmlDoc, ElementBaseNode node, string elementName, PointF? collapsedOrigin)
      {
         var element = xmlDoc.CreateElement(elementName);
         setBaseAttributes(element, node, node.Location, node.Bounds, collapsedOrigin);
         element.SetAttribute(NODE_SIZE, node.NodeSize.ToString());
         return element;
      }

      private static void setBaseAttributes(XmlElement element, DiagramNode node, PointF location, RectangleF bounds, PointF? collapsedOrigin)
      {
         if (collapsedOrigin.HasValue)
         {
            var parentLocation = node.GetParent()?.Location ?? PointF.Empty;
            element.SetAttribute(SAVED_BOUNDS, rectangleToString(new RectangleF(bounds.X - parentLocation.X, bounds.Y - parentLocation.Y, bounds.Width, bounds.Height)));
            location = collapsedOrigin.Value;
         }

         element.SetAttribute(ID, node.Id);
         element.SetAttribute(NAME, node.Name ?? string.Empty);
         element.SetAttribute(LOCATION, pointToString(location));
         element.SetAttribute(SIZE, sizeToString(bounds.Size));
         element.SetAttribute(HIDDEN, XmlConvert.ToString(node.Hidden));
         element.SetAttribute(IS_VISIBLE, XmlConvert.ToString(node.IsVisible));
         element.SetAttribute(LOCATION_FIXED, XmlConvert.ToString(node.LocationFixed));
         element.SetAttribute(USER_FLAGS, XmlConvert.ToString(node.UserFlags));
         element.SetAttribute(DESCRIPTION, node.Description ?? string.Empty);
      }

      public void Deserialize(IDiagramModel diagramModel, XmlDocument xmlDoc)
      {
         var root = xmlDoc.DocumentElement;
         if (root == null)
            return;

         try
         {
            diagramModel.BeginUpdate();
            readChildren(diagramModel, diagramModel, root);

            if (root.HasAttribute(LOCATION_X) && root.HasAttribute(LOCATION_Y))
               diagramModel.Location = new PointF(parseFloat(root.GetAttribute(LOCATION_X)), parseFloat(root.GetAttribute(LOCATION_Y)));

            diagramModel.IsLayouted = boolAttribute(root, IS_LAYOUTED, diagramModel.IsLayouted);
         }
         finally
         {
            diagramModel.EndUpdate();
         }
      }

      private void readChildren(IDiagramModel diagramModel, IContainerBase parent, XmlElement parentElement)
      {
         foreach (var element in parentElement.ChildNodes.OfType<XmlElement>())
         {
            switch (element.Name)
            {
               case REACTION_NODE:
                  var reactionNode = readElementNode<ReactionNode>(diagramModel, parent, element);
                  reactionNode.DisplayEductsRight = boolAttribute(element, DISPLAY_EDUCTS_RIGHT, reactionNode.DisplayEductsRight);
                  break;
               case MOLECULE_NODE:
                  readElementNode<MoleculeNode>(diagramModel, parent, element);
                  break;
               case NEIGHBORHOOD_NODE:
                  readElementNode<NeighborhoodNode>(diagramModel, parent, element);
                  break;
               case JOURNAL_PAGE_NODE:
                  var journalPageNode = readElementNode<JournalPageNode>(diagramModel, parent, element);
                  journalPageNode.IsExpanded = boolAttribute(element, IS_EXPANDED, journalPageNode.IsExpanded);
                  break;
               case RELATED_ITEM_NODE:
                  readElementNode<RelatedItemNode>(diagramModel, parent, element);
                  break;
               case CONTAINER_NODE:
                  readContainerNode(diagramModel, parent, element);
                  break;
            }
         }
      }

      private T readElementNode<T>(IDiagramModel diagramModel, IContainerBase parent, XmlElement element) where T : ElementBaseNode, new()
      {
         var location = parsePoint(element.GetAttribute(LOCATION));
         if (element.HasAttribute(SAVED_BOUNDS))
         {
            var savedBounds = savedBoundsOf(element, parent);
            location = new PointF(savedBounds.X + savedBounds.Width / 2, savedBounds.Y + savedBounds.Height / 2);
         }

         var node = createNode<T>(diagramModel, parent, element, location);
         if (element.HasAttribute(NODE_SIZE) && Enum.TryParse(element.GetAttribute(NODE_SIZE), out NodeSize nodeSize))
            node.NodeSize = nodeSize;
         return node;
      }

      private void readContainerNode(IDiagramModel diagramModel, IContainerBase parent, XmlElement element)
      {
         var bounds = element.HasAttribute(SAVED_BOUNDS) ? savedBoundsOf(element, parent) : new RectangleF(parsePoint(element.GetAttribute(LOCATION)), parseSize(element.GetAttribute(SIZE)));
         var node = createNode<ContainerNode>(diagramModel, parent, element, bounds.Location);
         node.Size = bounds.Size;
         node.IsLogical = boolAttribute(element, IS_LOGICAL, node.IsLogical);
         node.IsExpanded = boolAttribute(element, IS_EXPANDED, node.IsExpanded);
         node.IsExpandedByDefault = boolAttribute(element, IS_EXPANDED_BY_DEFAULT, node.IsExpandedByDefault);
         readChildren(diagramModel, node, element);
         if (!node.IsExpanded)
            fitCollapsedContainer(node);
      }

      private static void fitCollapsedContainer(ContainerNode node)
      {
         var frame = node.CalculateFrame();
         node.Size = new SizeF(Math.Max(node.Size.Width, frame.Right - node.Location.X), Math.Max(node.Size.Height, frame.Bottom - node.Location.Y));
      }

      private static RectangleF savedBoundsOf(XmlElement element, IContainerBase parent)
      {
         var savedBounds = parseRectangle(element.GetAttribute(SAVED_BOUNDS));
         return new RectangleF(parent.Location.X + savedBounds.X, parent.Location.Y + savedBounds.Y, savedBounds.Width, savedBounds.Height);
      }

      private static T createNode<T>(IDiagramModel diagramModel, IContainerBase parent, XmlElement element, PointF location) where T : DiagramNode, new()
      {
         var id = element.HasAttribute(ID) ? element.GetAttribute(ID) : Guid.NewGuid().ToString();
         var node = diagramModel.CreateNode<T>(id, location, parent);
         node.Name = element.GetAttribute(NAME);
         node.Description = element.GetAttribute(DESCRIPTION);
         if (boolAttribute(element, HIDDEN, false))
            node.Hidden = true;
         node.IsVisible = boolAttribute(element, IS_VISIBLE, node.IsVisible);
         node.LocationFixed = boolAttribute(element, LOCATION_FIXED, node.LocationFixed);
         if (element.HasAttribute(USER_FLAGS) && int.TryParse(element.GetAttribute(USER_FLAGS), NumberStyles.Integer, CultureInfo.InvariantCulture, out var userFlags))
            node.UserFlags = userFlags;
         return node;
      }

      public IDiagramModel XmlDocumentToDiagramModel(XmlDocument xmlDoc)
      {
         var diagramModel = new DiagramModel();
         Deserialize(diagramModel, xmlDoc);
         return diagramModel;
      }

      public void AddElementBaseNodeBindingFor<T>(T node)
      {
      }

      private static bool boolAttribute(XmlElement element, string name, bool defaultValue)
      {
         return element.HasAttribute(name) && bool.TryParse(element.GetAttribute(name), out var value) ? value : defaultValue;
      }

      private static string floatToString(float value) => value.ToString(CultureInfo.InvariantCulture);

      private static float parseFloat(string value) => float.Parse(value, CultureInfo.InvariantCulture);

      private static string pointToString(PointF point) => $"{floatToString(point.X)} {floatToString(point.Y)}";

      private static string sizeToString(SizeF size) => $"{floatToString(size.Width)} {floatToString(size.Height)}";

      private static string rectangleToString(RectangleF rectangle) => $"{pointToString(rectangle.Location)} {sizeToString(rectangle.Size)}";

      private static float[] parseFloats(string value, int count)
      {
         var parts = (value ?? string.Empty).Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
         return parts.Length == count ? parts.Select(parseFloat).ToArray() : new float[count];
      }

      private static PointF parsePoint(string value)
      {
         var values = parseFloats(value, 2);
         return new PointF(values[0], values[1]);
      }

      private static SizeF parseSize(string value)
      {
         var values = parseFloats(value, 2);
         return new SizeF(values[0], values[1]);
      }

      private static RectangleF parseRectangle(string value)
      {
         var values = parseFloats(value, 4);
         return new RectangleF(values[0], values[1], values[2], values[3]);
      }
   }
}
