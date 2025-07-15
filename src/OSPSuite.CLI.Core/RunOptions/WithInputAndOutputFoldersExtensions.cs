using System.Text;

namespace OSPSuite.CLI.Core.RunOptions
{
   public static class WithInputAndOutputFoldersExtensions
   {
      public static void LogInputFolder(this IWithInputFolder option, StringBuilder sb)
      {
         sb.AppendLine($"Input Folder: {option.InputFolder}");
      }

      public static void LogOutputFolder(this IWithOutputFolder option, StringBuilder sb)
      {
         sb.AppendLine($"Output Folder: {option.OutputFolder}");
      }

      public static void LogFolders(this IWithInputAndOutputFolders option, StringBuilder sb)
      {
         LogInputFolder((IWithInputFolder)option, sb);
         LogOutputFolder((IWithOutputFolder)option, sb);
      }
   }
}