using System.Linq;
using System.Text;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;

namespace OSPSuite.UI.Diagram.Services
{
   public interface IDiagramToolTipCreator
   {
      string GetToolTipFor(RelatedItem relatedItem);
      string GetToolTipFor(JournalPage page);
   }

   public class DiagramToolTipCreator : IDiagramToolTipCreator
   {
      private readonly DateTimeFormatter _dateTimeFormatter;

      public DiagramToolTipCreator()
      {
         _dateTimeFormatter = new DateTimeFormatter(displayTime: true);
      }

      public string GetToolTipFor(RelatedItem relatedItem)
      {
         var sb = new StringBuilder();

         sb.AppendLine(string.Format("{0}: {1}", relatedItem.ItemType, relatedItem.Name));
         sb.AppendLine(string.Format("{0} {1}", Captions.Journal.CreatedAt, _dateTimeFormatter.Format(relatedItem.CreatedAt)));
         sb.AppendLine(string.Format("{0}", Captions.Journal.UsingOrigin(relatedItem.Origin.DisplayName)));

         if (!string.IsNullOrEmpty(relatedItem.FullPath))
            sb.AppendLine(string.Format("{0}", relatedItem.FullPath));

         if (string.IsNullOrEmpty(relatedItem.Description)) 
            return sb.ToString();

         sb.AppendLine();
         sb.AppendLine(relatedItem.Description);

         return sb.ToString();
      }

      public string GetToolTipFor(JournalPage page)
      {
         var sb = new StringBuilder();
         createMandatoryPortion(page, sb);

         createOptionalPortion(page, sb);

         return sb.ToString().TrimEnd();
      }

      private static void createOptionalPortion(JournalPage page, StringBuilder stringBuilder)
      {
         stringBuilder.AppendLine();
         if (!string.IsNullOrEmpty(page.Description))
            stringBuilder.AppendLine(page.Description);
         if (page.Tags.Any())
            stringBuilder.AppendLine(page.Tags.ToString(","));
      }

      private void createMandatoryPortion(JournalPage page, StringBuilder stringBuilder)
      {
         stringBuilder.AppendLine(Captions.Journal.UpdatedAtBy(_dateTimeFormatter.Format(page.UpdatedAt), page.UpdatedBy));
         stringBuilder.AppendLine(Captions.Journal.UsingOrigin(page.Origin.DisplayName));
         stringBuilder.AppendLine(Captions.Journal.CreatedAtBy(_dateTimeFormatter.Format(page.CreatedAt), page.CreatedBy));
      }

   }
}
