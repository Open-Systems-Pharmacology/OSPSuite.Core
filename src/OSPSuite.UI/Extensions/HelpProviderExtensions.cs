using System.Globalization;
using System.Windows.Forms;
using OSPSuite.Core.Domain;

namespace OSPSuite.UI.Extensions
{
   public static class HelpProviderExtensions
   {
      public static void Initialize(this HelpProvider helpProvider, Control control)
      {
         helpProvider.HelpNamespace = Constants.HELP_NAMESPACE;
         helpProvider.SetHelpNavigator(control, HelpNavigator.Topic);
      }

      public static void SetTopicId(this HelpProvider helpProvider, Control control, int topicId)
      {
         helpProvider.SetHelpKeyword(control, topicId.ToString(CultureInfo.InvariantCulture));
      }
   }
}  