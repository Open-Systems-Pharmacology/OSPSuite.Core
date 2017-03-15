using System;
using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Utility.Extensions;
using Northwoods.Go;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Diagram.Elements
{
   public class ElementBaseNode : GoBasicNode, IElementBaseNode
   {
      protected NodeSize _nodeSize;
      public string Id { get; set; }
      public SizeF NodeBaseSize { get; set; }
      public bool IsVisible { get; set; }
      public bool Hidden { get; set; }
      public bool LocationFixed { get; set; }

      public ElementBaseNode()
      {
         IsVisible = true;
         LocationFixed = false;

         AutoResizes = false;
         Shape = new GoEllipse();

         Port = new BasePort(this, true);

         Text = "";
         LabelSpot = MiddleTop;
         NodeBaseSize = new SizeF(20, 20);
         NodeSize = NodeSize.Middle;
      }

      public T FindChild<T>(string childName)
      {
         return FindChild(childName).DowncastTo<T>();
      }

      public virtual bool CanLink
      {
         get { return Port.IsValidFrom; }
         set
         {
            Port.IsValidFrom = value;
            Port.IsValidTo = value;
         }
      }

      public virtual void CopyLayoutInfoFrom(IBaseNode baseNode, PointF parentLocation)
      {
         var node = baseNode as IElementBaseNode;
         if (node == null) return;

         // copy location relative to parent
         PointF location = node.Location;

         if (GetParent() != null && node.GetParent() != null)
            location = location.Plus(parentLocation.Minus(node.GetParent().Location));

         Bounds = node.Bounds;
         Location = location;
         LocationFixed = node.LocationFixed;
         Hidden = node.Hidden;
         IsVisible = node.IsVisible;
         NodeSize = node.NodeSize;
      }

      public new IBaseNode Copy()
      {
         return base.Copy() as IBaseNode;
      }

      public IContainerBase GetParent()
      {
         if (Parent == null) return Document as IContainerBase;

         var parentContainerBaseNode = Parent as IContainerNode;
         if (parentContainerBaseNode != null) return parentContainerBaseNode;
         return Parent.Parent as IContainerNode;
      }

      protected DiagramModel DiagramModel
      {
         get { return Document as DiagramModel; }
      }

      public virtual string Name
      {
         get { return Text; }
         set { Text = value; }
      }

      public string Description
      {
         get { return ToolTipText; }
         set { ToolTipText = value; }
      }

      public virtual NodeSize NodeSize
      {
         get { return _nodeSize; }
         set
         {
            _nodeSize = value;
            RefreshSize();
            RefreshLabel();
         }
      }

      protected virtual void RefreshSize()
      {
         SetPortSize(0.5F);
         SetShapeSize();
      }

      protected void SetShapeSize()
      {
         Shape.Size = new SizeF(_nodeSize.DowncastTo<int>() / 100F * NodeBaseSize.Width, _nodeSize.DowncastTo<int>() / 100F * NodeBaseSize.Height);
      }

      protected void SetPortSize(float portSizeRatio)
      {
         if (Port != null)
            Port.Size = new SizeF(_nodeSize.DowncastTo<int>() / 100F * NodeBaseSize.Width * portSizeRatio, _nodeSize.DowncastTo<int>() / 100F * NodeBaseSize.Height * portSizeRatio);
      }

      protected virtual void RefreshLabel()
      {
         if (Label == null) return;

         switch (_nodeSize)
         {
            case NodeSize.Small:
               Label.Visible = false;
               break;
            case NodeSize.Middle:
               Label.Visible = true;
               Label.FontSize = 8;
               Label.TextColor = SuiteColors.Gray;
               break;
            case NodeSize.Large:
               Label.Visible = true;
               Label.FontSize = 10;
               Label.TextColor = Color.Black;
               break;
         }
      }

      public virtual void SetColorFrom(IDiagramColors diagramColors)
      {
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

      protected void SetBrushColor(IDiagramColors diagramColors, Color color)
      {
         var nodeAlpha = Alpha(diagramColors.NodeSizeOpacity);
         int opacity = Convert.ToInt16(diagramColors.PortOpacity * nodeAlpha);
         Shape.BrushColor = Color.FromArgb(opacity, color);
         if (Port != null) Port.BrushColor = Color.FromArgb(nodeAlpha, color);
      }

      public int Alpha(float nodeSizeOpacity)
      {
         int alpha;
         switch (_nodeSize)
         {
            case NodeSize.Small:
               alpha = Convert.ToInt16(nodeSizeOpacity*nodeSizeOpacity*255);
               break;
            case NodeSize.Middle:
               alpha = Convert.ToInt16(nodeSizeOpacity*255);
               break;
            case NodeSize.Large:
               alpha = 255;
               break;
            default:
               alpha = 128;
               break;
         }
         return alpha;
      }

      public virtual void ShowParents()
      {
         foreach (var parent in this.GetParentNodes()) parent.Hidden = false;
      }

      public override bool Visible
      {
         get { return base.Visible && IsVisible && !Hidden; }
         set { base.Visible = value; }
      }

      public virtual void AddLinkFrom(IBaseLink link)
      {
         Port.AddDestinationLink((GoLink) link);
      }

      public virtual void AddLinkTo(IBaseLink link)
      {
         Port.AddSourceLink((GoLink) link);
      }

      public IEnumerable<T> GetLinks<T>() where T : class
      {
         IList<T> baseLinks = new List<T>();
         foreach (var link in Links)
         {
            var baseLink = link as T;
            if (baseLink != null) baseLinks.Add(baseLink);
         }
         return baseLinks;
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


      public void ToFront()
      {
         BaseDiagramExtensions.ToFront(this);
      }

      public void ToBack()
      {
         BaseDiagramExtensions.ToBack(this);
      }
   }
}