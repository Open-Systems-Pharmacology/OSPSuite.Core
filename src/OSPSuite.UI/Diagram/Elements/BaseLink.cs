using Northwoods.Go;
using OSPSuite.Core.Diagram;

namespace OSPSuite.UI.Diagram.Elements
{
   public abstract class BaseLink : GoLink, IBaseLink
   {
      public bool IsVisible { get; set; }

      protected BaseLink()
      {
         Style = GoStrokeStyle.Line;
         IsVisible = true;
         Resizable = false;
         Reshapable = false;
      }

      protected BaseLink(IBaseNode fromNode, IBaseNode toNode) : this()
      {
         Initialize(fromNode, toNode);
      }

      public virtual void Initialize(IBaseNode fromNode, IBaseNode toNode)
      {
         fromNode.AddLinkFrom(this);
         toNode.AddLinkTo(this);
         GoSubGraphBase.ReparentToCommonSubGraph(this, (GoObject) fromNode, (GoObject) toNode, true,
                                                 ((GoObject) fromNode).Document.DefaultLayer);
      }

      public IContainerBase GetParent()
      {
         return Parent as IContainerBase;
      }

      public override bool Visible
      {
         get
         {
            var fromNode = base.FromNode as IHasLayoutInfo;
            var toNode = base.ToNode as IHasLayoutInfo;
            
            if (fromNode != null && toNode != null) return base.Visible && IsVisible && fromNode.Visible && toNode.Visible;
            return true;
         }
         set { base.Visible = value; }
      }

      public abstract void SetColorFrom(IDiagramColors diagramColors);

      public IBaseNode GetOtherNode(IBaseNode node)
      {
         var goNode = node as IGoNode;
         if (goNode == null) return null;
         return GetOtherNode(goNode) as IBaseNode;
      }

      public IBaseNode GetFromNode()
      {
         return base.FromNode as IBaseNode;
      }

      public IBaseNode GetToNode()
      {
         return base.ToNode as IBaseNode;
      }
   }
}