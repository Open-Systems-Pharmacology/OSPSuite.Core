using System;
using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using Northwoods.Go;
using OSPSuite.Core.Diagram;

namespace OSPSuite.UI.Diagram.Elements
{
   public class MultiPortContainerNode : SimpleContainerNode
   {
      private IDictionary<NeighborLink, PortNode> _portNodes;
 
      public MultiPortContainerNode()
      {
         _portNodes = new Dictionary<NeighborLink, PortNode>();
         IsLogical = false;
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         base.SetColorFrom(diagramColors);
         foreach (var portNode in _portNodes.Values) portNode.SetColorFrom(diagramColors);
      }

      public override GoObject CopyObject(GoCopyDictionary env)
      {
         var copy = (MultiPortContainerNode)base.CopyObject(env);
         copy._portNodes = new Dictionary<NeighborLink, PortNode>();
         return copy;
      }

      public override RectangleF ComputeResize(RectangleF origRect, PointF newPoint, int handle, SizeF min, SizeF max, bool reshape)
      {
         var rectangle = base.ComputeResize(origRect, newPoint, handle, min, max, reshape);
         if (rectangle.Width * rectangle.Height < 1000 + 1000 * _portNodes.Count)
            rectangle.Inflate(inflationValue(rectangle.Width), inflationValue(rectangle.Height));
         return rectangle;
      }

      private float inflationValue(float size)
      {
         float minSize = 30 + 5 * _portNodes.Count;
         if (size >= minSize) return 1;
         return (minSize - size) / 2;
      }

      public override IEnumerable<T> GetLinkedNodes<T>()
      {
         IList<T> neighborNodes = new List<T>();

         foreach (var portNode in _portNodes.Values)
            foreach (var node in portNode.Nodes)
            {
               T neighborNode = node as T;
               if (neighborNode != null) neighborNodes.Add(neighborNode);
            }
         return neighborNodes;
      }

      public override void AddLinkTo(IBaseLink link)
      {
         addNeighborLink((NeighborLink)link);
      }

      public override void AddLinkFrom(IBaseLink link)
      {
         throw new OSPSuiteException("No IBaseLinks allowed from MultiPortNode.");
      }

      private void addNeighborLink(NeighborLink neighborLink)
      {
         var portNode = new PortNode(neighborLink);
         _portNodes.Add(neighborLink, portNode);
         InsertBefore(null, portNode);
      }

      public override void AddTransportLink(TransportLink link)
      {
         _portNodes[link.NeighborLink].AddDestinationLink(link);
      }

      protected override bool ComputeInsideMarginsSkip(GoObject child)
      {
         if (child.IsAnImplementationOf<GoLink>()) return true;
         if (child.IsAnImplementationOf<PortNode>()) return true;
         return base.ComputeInsideMarginsSkip(child);
      }

 
      public void RefreshPort(NeighborLink neighborLink)
      {
         if (!_portNodes.ContainsKey(neighborLink)) return;
         var portNode = _portNodes[neighborLink];
         var otherContainerNode = neighborLink.NeighborhoodNode.GetOtherContainerNode(this);

         // set name
         string name = "";
         var otherContainerParentNode = otherContainerNode.GetParent() as IContainerNode;
         if (otherContainerParentNode != null) name += otherContainerParentNode.Name + "/";
         name += otherContainerNode.Name;
         portNode.Name = name;

         // set position
         if (portNode.LocationFixed) return;

         PointF borderPosition;
         RectangleF bounds = ComputeInsideMargins(null);

         // find points in both containers near to each other
         float bx = 10;
         float by = 10;
         float otherX, otherY, otherWidth, otherHeight;
         if (neighborLink.NeighborhoodNode.LocationFixed)
         {
            otherX = neighborLink.NeighborhoodNode.Location.X;
            otherY = neighborLink.NeighborhoodNode.Location.Y;
            otherWidth = bx;
            otherHeight = by;
         }
         else
         {
            otherX = otherContainerNode.Center.X;
            otherY = otherContainerNode.Center.Y;
            otherWidth = otherContainerNode.Size.Width;
            otherHeight = otherContainerNode.Size.Height;
         }
         float xo = nearestValueFromIntervalCS(otherX, otherWidth, bx, Center.X);
         float yo = nearestValueFromIntervalCS(otherY, otherHeight, by, Center.Y);
         float x = nearestValueFromInterval(bounds.Left, bounds.Right, bx, otherX);
         float y = nearestValueFromInterval(bounds.Top, bounds.Bottom, by, otherY);

         bool pointExists = GoObject.GetNearestIntersectionPoint(bounds, new PointF(xo, yo), new PointF(x, y), out borderPosition);

         //Correct by difference between location and center 
         if (pointExists)
         {
            var newLocation = borderPosition + new SizeF(portNode.Location.X - portNode.Center.X, portNode.Location.Y - portNode.Center.Y);
            if (Math.Abs(newLocation.X - portNode.Location.X) > 0.1 || Math.Abs(newLocation.Y - portNode.Location.Y) > 0.1)
            {
               portNode.Location = newLocation;
               foreach (var link in portNode.Links) ((GoLink)link).UpdateRoute();
            }
         }
      }

      private float nearestValueFromIntervalCS(float intervalCenter, float intervalSize, float border, float value)
      {
         return nearestValueFromInterval(intervalCenter - intervalSize / 2, intervalCenter + intervalSize / 2, border, value);
      }

      private float nearestValueFromInterval(float intervalMin, float intervalMax, float border, float value)
      {
         if (value < intervalMin + border) return intervalMin + border;
         if (value > intervalMax - border) return intervalMax - border;
         return value;
      }

   }
}