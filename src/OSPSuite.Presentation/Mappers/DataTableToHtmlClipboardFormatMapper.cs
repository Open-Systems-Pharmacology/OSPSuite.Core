using System.Data;
using System.Text;

namespace OSPSuite.Presentation.Mappers
{
   public interface IDataTableToHtmlClipboardFormatMapper
   {
      string MapFrom(DataTable dataTable, bool includeHeaders);
   }

   public class DataTableToHtmlClipboardFormatMapper : IDataTableToHtmlClipboardFormatMapper
   {
      private readonly IDataTableToHtmlMapper _dataTableToHtmlMapper;
      //CF_HTML format
      //https://msdn.microsoft.com/en-us/library/windows/desktop/ms649015%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
      private const string _header = @"Version:0.9
StartHTML:" + START_HTML_TOKEN + @"
EndHTML:" + END_HTML_TOKEN + @"
StartFragment:" + START_FRAGMENT_TOKEN + @"
EndFragment:" + END_FRAGMENT_TOKEN + @"
StartSelection:" + START_FRAGMENT_TOKEN + @"
EndSelection:" + END_FRAGMENT_TOKEN;

      private const string _startFragment = "<!--StartFragment-->";
      private const string _endFragment = "<!--EndFragment-->";
      private const string START_HTML_TOKEN = "<<<<<<<<1";
      private const string END_HTML_TOKEN = "<<<<<<<<2";
      private const string START_FRAGMENT_TOKEN = "<<<<<<<<3";
      private const string END_FRAGMENT_TOKEN = "<<<<<<<<4";

      private static readonly char[] _byteCount = new char[1];
      public DataTableToHtmlClipboardFormatMapper(IDataTableToHtmlMapper dataTableToHtmlMapper)
      {
         _dataTableToHtmlMapper = dataTableToHtmlMapper;
      }

      public string MapFrom(DataTable dataTable, bool includeHeaders)
      {
         return getHtmlDataString(_dataTableToHtmlMapper.MapFrom(dataTable, includeHeaders));
      }

      private static string getHtmlDataString(string html)
      {
         var sb = new StringBuilder();
         sb.AppendLine(_header);
         var fragmentStart = startHtmlFragment(sb);
         sb.Append(html);
         var fragmentEnd = endHtmlFragment(sb);

         patchHtmlFragment(sb, fragmentStart, fragmentEnd);

         return sb.ToString();
      }

      private static void patchHtmlFragment(StringBuilder sb, int fragmentStart, int fragmentEnd)
      {
         sb.Replace(START_HTML_TOKEN, _header.Length.ToString("D9"), 0, _header.Length);
         sb.Replace(END_HTML_TOKEN, getByteCount(sb, 0, sb.Length).ToString("D9"), 0, _header.Length);
         sb.Replace(START_FRAGMENT_TOKEN, fragmentStart.ToString("D9"), 0, _header.Length);
         sb.Replace(END_FRAGMENT_TOKEN, fragmentEnd.ToString("D9"), 0, _header.Length);
      }

      private static int endHtmlFragment(StringBuilder sb)
      {
         var fragmentEnd = getByteCount(sb, 0, sb.Length);
         sb.Append(_endFragment);
         sb.Append("</body></html>");
         return fragmentEnd;
      }

      private static int startHtmlFragment(StringBuilder sb)
      {
         sb.AppendLine(@"<!DOCTYPE HTML  PUBLIC ""-//W3C//DTD HTML 4.0  Transitional//EN"">");

         sb.Append("<html><body>");
         sb.Append(_startFragment);
         var fragmentStart = getByteCount(sb, 0, sb.Length);
         return fragmentStart;
      }

      private static int getByteCount(StringBuilder sb, int start, int end)
      {
         var count = 0;
         for (var i = start; i < end; i++)
         {
            _byteCount[0] = sb[i];
            count += Encoding.UTF8.GetByteCount(_byteCount);
         }
         return count;
      }
   }
}
