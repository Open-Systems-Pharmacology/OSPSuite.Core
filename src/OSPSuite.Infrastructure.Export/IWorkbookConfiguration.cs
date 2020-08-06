using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace OSPSuite.Infrastructure.Export
{
   public interface IWorkbookConfiguration
   {
       XSSFWorkbook WorkBook { get; set; }
       ICellStyle HeadersStyle { get; set; }
       ICellStyle BodyStyle { get; set; }


      void SetHeadersBold();
   }

   public class WorkbookConfiguration : IWorkbookConfiguration
   {
      public XSSFWorkbook WorkBook { get; set; }
      public ICellStyle HeadersStyle {get; set;}
      public ICellStyle BodyStyle {get; set;}

      private IFont getDefaultFont( )
      {
         var font = WorkBook.CreateFont();
         font.FontHeightInPoints = 11;
         font.FontName = "Times New Roman";
         font.IsBold = false;

         return font;
      }
      public WorkbookConfiguration()
      {
         WorkBook = new XSSFWorkbook();

         var font = getDefaultFont();

         HeadersStyle = WorkBook.CreateCellStyle();
         HeadersStyle.SetFont(font);

         BodyStyle = WorkBook.CreateCellStyle();
         BodyStyle.SetFont(font);
      }

      public void SetHeadersBold()
      {
         var font = WorkBook.CreateFont();
         font.FontHeightInPoints = 11;
         font.FontName = "Arial";
         font.IsBold = true;

         HeadersStyle = WorkBook.CreateCellStyle();
         HeadersStyle.SetFont(font);
      }
   }
}
