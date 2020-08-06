using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;

namespace OSPSuite.Infrastructure.Export
{
   public interface IWorkbookConfiguration
   {
      void SetHeadersBold();
   }

   public class WorkbookConfiguration : IWorkbookConfiguration
   {
      private XSSFWorkbook _workBook;
      public WorkbookConfiguration()
      {
         _workBook = new XSSFWorkbook();
      }

      public void SetHeadersBold()
      {
         throw new NotImplementedException();
      }
   }
}
