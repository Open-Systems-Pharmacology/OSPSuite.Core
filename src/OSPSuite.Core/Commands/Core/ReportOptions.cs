using OSPSuite.Utility;

namespace OSPSuite.Core.Commands.Core
{
   public class ReportOptions
   {
      /// <summary>
      /// Name of sheet where the report will be created
      /// </summary>
      public string SheetName;

      /// <summary>
      /// Full report path as selected by the user
      /// </summary>
      public string ReportFullPath { get; set; }

      /// <summary>
      /// Open report after generation?
      /// </summary>
      public bool OpenReport { get; set; }

      /// <summary>
      /// Report output folder, where the report files will be generated
      /// </summary>
      public string OutputFolder
      {
         get { return FileHelper.FolderFromFileFullPath(ReportFullPath); }
      }

      /// <summary>
      /// Name of report, determined from report full path
      /// </summary>
      public string ReportName
      {
         get { return FileHelper.FileNameFromFileFullPath(ReportFullPath); }
      }

   }
}