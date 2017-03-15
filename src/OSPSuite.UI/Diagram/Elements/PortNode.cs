using System.Collections.Generic;
using System.Drawing;
using Northwoods.Go;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Diagram.Elements
{
   public class PortNode : GoBasicNode, IPortNode
   {
      public bool LocationFixed { get; set; }
      public NeighborLink NeighborLink { get; private set; }

      public PortNode(NeighborLink neighborLink)
      {
         Port = new PortNodePort();
         NeighborLink = neighborLink;
         Port.AddSourceLink(neighborLink);

         IsVisible = true;
         LocationFixed = false;
         UserFlags = NodeLayoutType.NEIGHBORHOOD_PORT;

         AutoResizes = false;

         Text = string.Empty;
         Label.FontSize = 8;
         Label.TextColor = SuiteColors.Gray;
         LabelSpot = MiddleTop;

         Shape = new GoEllipse();
         InsertBefore(Shape, Port);
         Shape.Size = new SizeF(15, 10);
         Port.Size = new SizeF(1, 1);
      }

      public MultiPortContainerNode GetParent()
      {
         return Parent as MultiPortContainerNode;
      }

      public string Name
      {
         get { return Text; }
         set { Text = value; }
      }

      public string Description
      {
         get { return ToolTipText; }
         set { ToolTipText = value; }
      }

      public Color Color
      {
         get { return Shape.BrushColor; }
         set { Shape.BrushColor = value; }
      }

      public bool Hidden
      {
         get { return GetParent().Hidden; }
         set { GetParent().Hidden = value; }
      }

      public bool IsVisible
      {
         get { return GetParent().IsVisible; }
         set { }
      }

      public override bool Visible
      {
         get { return GetParent().IsExpanded && !Hidden && IsVisible; }
         set { base.Visible = value; }
      }

      public void SetColorFrom(IDiagramColors diagramColors)
      {
         Shape.BrushColor = diagramColors.NeighborhoodPort;
         if (LocationFixed)
         {
            Shape.PenWidth = 2.0F;
            Shape.PenColor = diagramColors.BorderFixed;
         }
         else
         {
            Shape.PenWidth = 1.0F;
            Shape.PenColor = diagramColors.BorderUnfixed;
         }
      }

      public void ToFront()
      {
         BaseDiagramExtensions.ToFront(this);
      }

      public void ToBack()
      {
         BaseDiagramExtensions.ToBack(this);
      }

      public void AddDestinationLink<T>(T link) where T : class
      {
         Port.AddDestinationLink(link as GoLink);
      }

      public void AddSourceLink<T>(T link) where T : class
      {
         Port.AddSourceLink(link as GoLink);
      }

      public void RemoveLink<T>(T link) where T : class
      {
         Port.RemoveLink(link as GoLink);
      }

      public virtual IEnumerable<T> GetLinkedNodes<T>() where T : class, IHasLayoutInfo
      {
         IList<T> linkedNodes = new List<T>();
         foreach (var node in Nodes)
         {
            T linkedNode = node as T;
            if (linkedNode != null) linkedNodes.Add(linkedNode);
         }

         return linkedNodes;
      }

      public object PortNodeParent
      {
         get { return Parent; }
      }
   }
}