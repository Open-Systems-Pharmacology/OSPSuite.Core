using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Diagram;

namespace OSPSuite.UI.Diagram.Elements
{
   public class JournalParentPort : JournalItemPort
   {
      public JournalParentPort()
      {
         UserObject = this;
      }

      protected override bool IsValidLink(IBaseNode node1, IBaseNode node2, object arg3, object arg4)
      {
         return base.IsValidLink(node1, node2, arg3, arg4) && notAlreadyLinked(arg3 as JournalItemPort, arg4 as JournalItemPort);
      }

      private bool notAlreadyLinked(JournalItemPort journalItemPort, JournalItemPort journalItemPort1)
      {
         return !isParentPortWithLinks(journalItemPort) && !isParentPortWithLinks(journalItemPort1);
      }

      private static bool isParentPortWithLinks(JournalItemPort journalItemPort)
      {
         return journalItemPort is JournalParentPort && journalItemPort.DowncastTo<JournalParentPort>().Links.Any();
      }
   }
}