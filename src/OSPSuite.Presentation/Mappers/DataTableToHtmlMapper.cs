using System.Text;

namespace OSPSuite.Presentation.Mappers
{
   public interface IDataTableToHtmlMapper : IDataTableToStringMapper
   {
   }

   public class DataTableToHtmlMapper : BaseDataTableMapper, IDataTableToHtmlMapper
   {
      protected override void CloseDataCell(StringBuilder outputBuilder)
      {
         outputBuilder.AppendLine("</td>");
      }

      protected override void OpenDataCell(StringBuilder outputBuilder)
      {
         outputBuilder.Append("<td>");
      }

      protected override void CloseTable(StringBuilder outputBuilder)
      {
         outputBuilder.AppendLine("</table>");
      }

      protected override void CloseRow(StringBuilder outputBuilder)
      {
         outputBuilder.AppendLine("</tr>");
      }

      protected override void CloseHeaderCell(StringBuilder outputBuilder)
      {
         outputBuilder.AppendLine("</th>");
      }

      protected override void OpenHeaderCell(StringBuilder outputBuilder)
      {
         outputBuilder.Append("<th>");
      }

      protected override void OpenRow(StringBuilder outputBuilder)
      {
         outputBuilder.AppendLine("<tr>");
      }

      protected override void OpenTable(StringBuilder outputBuilder)
      {
         outputBuilder.AppendLine("<table>");
      }
   }
}