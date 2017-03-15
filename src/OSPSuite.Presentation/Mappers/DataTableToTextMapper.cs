using System;
using System.Text;

namespace OSPSuite.Presentation.Mappers
{
   public interface IDataTableToTextMapper : IDataTableToStringMapper
   {
   }

   public class DataTableToTextMapper : BaseDataTableMapper, IDataTableToTextMapper
   {
      protected override void CloseDataCell(StringBuilder outputBuilder)
      {
         outputBuilder.Append("\t");
      }

      protected override void OpenDataCell(StringBuilder outputBuilder)
      {

      }

      protected override void CloseTable(StringBuilder outputBuilder)
      {

      }

      protected override void CloseRow(StringBuilder outputBuilder)
      {
         outputBuilder.Append(Environment.NewLine);
      }

      protected override void CloseHeaderCell(StringBuilder outputBuilder)
      {
         outputBuilder.Append("\t");
      }

      protected override void OpenHeaderCell(StringBuilder outputBuilder)
      {

      }

      protected override void OpenRow(StringBuilder outputBuilder)
      {

      }

      protected override void OpenTable(StringBuilder outputBuilder)
      {

      }
   }
}