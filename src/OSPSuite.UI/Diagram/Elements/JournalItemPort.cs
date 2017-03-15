using System.Drawing;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Diagram;

namespace OSPSuite.UI.Diagram.Elements
{
   public abstract class JournalItemPort : BasePort
   {
      protected virtual bool IsValidLink(IBaseNode node1, IBaseNode node2, object arg3, object arg4)
      {
         return NodesAreBothJournalPageNodes(node1, node2) && ParentToChildOnly(arg3 as JournalItemPort, arg4 as JournalItemPort);
      }

      protected JournalItemPort()
      {
         Size = new SizeF(4, 4);
         SetLinkValidator(IsValidLink);
      }

      protected static bool NodesAreBothJournalPageNodes(IBaseNode node1, IBaseNode node2)
      {
         return node1.IsAnImplementationOf<JournalPageNode>() && node2.IsAnImplementationOf<JournalPageNode>();
      }

      protected bool ParentToChildOnly(JournalItemPort journalItemPort, JournalItemPort journalItemPort1)
      {
         return
            (journalItemPort.IsAnImplementationOf<JournalParentPort>() && journalItemPort1.IsAnImplementationOf<JournalChildPort>()) ||
            (journalItemPort1.IsAnImplementationOf<JournalParentPort>() && journalItemPort.IsAnImplementationOf<JournalChildPort>());
      }
   }
}
