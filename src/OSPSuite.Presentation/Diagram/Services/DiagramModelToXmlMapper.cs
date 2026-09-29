using System;
using System.Collections.Generic;
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
      private const string REACTION_NODE = "ReactionNode";
      private const string MOLECULE_NODE = "MoleculeNode";

      public string ElementName => Constants.Serialization.DIAGRAM_MODEL;

      public XmlDocument DiagramModelToXmlDocument(IDiagramModel diagramModel)
      {
         var xmlDoc = documentFor(diagramModel.GetDirectChildren<IBaseNode>());
         xmlDoc.DocumentElement.SetAttribute(IS_LAYOUTED, diagramModel.IsLayouted.ToString());
         return xmlDoc;
      }

      public XmlDocument ContainerToXmlDocument(IContainerBase containerBase)
      {
         var xmlDoc = containerBase is IDiagramModel diagramModel ? DiagramModelToXmlDocument(diagramModel) : documentFor(containerBase.GetDirectChildren<IBaseNode>());
         xmlDoc.DocumentElement.SetAttribute(LOCATION_X, floatToString(containerBase.Location.X));
         xmlDoc.DocumentElement.SetAttribute(LOCATION_Y, floatToString(containerBase.Location.Y));
         return xmlDoc;
      }

      private XmlDocument documentFor(IEnumerable<IBaseNode> nodes)
      {
         var xmlDoc = new XmlDocument();
         var root = xmlDoc.CreateElement(ElementName);
         xmlDoc.AppendChild(root);

         nodes.Each(node =>
         {
            var element = elementFor(xmlDoc, node);
            if (element != null)
               root.AppendChild(element);
         });

         return xmlDoc;
      }

      private XmlElement elementFor(XmlDocument xmlDoc, IBaseNode node)
      {
         switch (node)
         {
            case ReactionNode reactionNode:
               var reactionElement = elementFor(xmlDoc, reactionNode, REACTION_NODE);
               reactionElement.SetAttribute(DISPLAY_EDUCTS_RIGHT, XmlConvert.ToString(reactionNode.DisplayEductsRight));
               return reactionElement;
            case MoleculeNode moleculeNode:
               return elementFor(xmlDoc, moleculeNode, MOLECULE_NODE);
            default:
               return null;
         }
      }

      private XmlElement elementFor(XmlDocument xmlDoc, ElementBaseNode node, string elementName)
      {
         var element = xmlDoc.CreateElement(elementName);
         element.SetAttribute(ID, node.Id);
         element.SetAttribute(NAME, node.Name ?? string.Empty);
         element.SetAttribute(LOCATION, pointToString(node.Location));
         element.SetAttribute(SIZE, sizeToString(node.Size));
         element.SetAttribute(HIDDEN, XmlConvert.ToString(node.Hidden));
         element.SetAttribute(IS_VISIBLE, XmlConvert.ToString(node.IsVisible));
         element.SetAttribute(LOCATION_FIXED, XmlConvert.ToString(node.LocationFixed));
         element.SetAttribute(USER_FLAGS, XmlConvert.ToString(node.UserFlags));
         element.SetAttribute(DESCRIPTION, node.Description ?? string.Empty);
         element.SetAttribute(NODE_SIZE, node.NodeSize.ToString());
         return element;
      }

      public void Deserialize(IDiagramModel diagramModel, XmlDocument xmlDoc)
      {
         var root = xmlDoc.DocumentElement;
         if (root == null)
            return;

         try
         {
            diagramModel.BeginUpdate();
            foreach (var element in root.ChildNodes.OfType<XmlElement>())
            {
               switch (element.Name)
               {
                  case REACTION_NODE:
                     var reactionNode = readNode<ReactionNode>(diagramModel, element);
                     reactionNode.DisplayEductsRight = boolAttribute(element, DISPLAY_EDUCTS_RIGHT, reactionNode.DisplayEductsRight);
                     break;
                  case MOLECULE_NODE:
                     readNode<MoleculeNode>(diagramModel, element);
                     break;
               }
            }

            if (root.HasAttribute(LOCATION_X) && root.HasAttribute(LOCATION_Y))
               diagramModel.Location = new PointF(parseFloat(root.GetAttribute(LOCATION_X)), parseFloat(root.GetAttribute(LOCATION_Y)));

            if (root.HasAttribute(IS_LAYOUTED))
               diagramModel.IsLayouted = bool.Parse(root.GetAttribute(IS_LAYOUTED));
         }
         finally
         {
            diagramModel.EndUpdate();
         }
      }

      private T readNode<T>(IDiagramModel diagramModel, XmlElement element) where T : ElementBaseNode, new()
      {
         var id = element.HasAttribute(ID) ? element.GetAttribute(ID) : Guid.NewGuid().ToString();
         var node = diagramModel.CreateNode<T>(id, parsePoint(element.GetAttribute(LOCATION)), diagramModel);
         node.Name = element.GetAttribute(NAME);
         node.Description = element.GetAttribute(DESCRIPTION);
         node.Hidden = boolAttribute(element, HIDDEN, node.Hidden);
         node.IsVisible = boolAttribute(element, IS_VISIBLE, node.IsVisible);
         node.LocationFixed = boolAttribute(element, LOCATION_FIXED, node.LocationFixed);
         if (element.HasAttribute(USER_FLAGS) && int.TryParse(element.GetAttribute(USER_FLAGS), NumberStyles.Integer, CultureInfo.InvariantCulture, out var userFlags))
            node.UserFlags = userFlags;
         if (element.HasAttribute(NODE_SIZE) && Enum.TryParse(element.GetAttribute(NODE_SIZE), out NodeSize nodeSize))
            node.NodeSize = nodeSize;
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

      private static PointF parsePoint(string value)
      {
         var parts = (value ?? string.Empty).Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
         if (parts.Length != 2)
            return PointF.Empty;
         return new PointF(parseFloat(parts[0]), parseFloat(parts[1]));
      }
   }
}
