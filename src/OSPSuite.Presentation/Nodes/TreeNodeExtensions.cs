using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Nodes
{
   public static class TreeNodeExtensions
   {
      public static ITreeNode WithIcon(this ITreeNode node, ApplicationIcon icon)
      {
         node.Icon = icon;
         return node;
      }

      public static ITreeNode WithText(this ITreeNode node, string text)
      {
         node.Text = text;
         return node;
      }

      /// <summary>
      /// Adds the node <paramref name="childNode"/> under the <paramref name="parentNode"/>.
      /// It does support <paramref name="parentNode"/> being <c>null</c>. In that case, nothing is happening
      /// </summary>
      public static T Under<T>(this T childNode, ITreeNode parentNode) where T : ITreeNode
      {
         if (parentNode != null)
            parentNode.AddChild(childNode);

         return childNode;
      }

      public static void AddChildren(this ITreeNode parentNode, params ITreeNode[] childrenNodes)
      {
         childrenNodes.Each(parentNode.AddChild);
      }

      public static bool HasParentNode(this ITreeNode node)
      {
         return (node.ParentNode != null);
      }
   }
}