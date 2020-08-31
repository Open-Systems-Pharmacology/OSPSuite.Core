using System;
using System.Collections.Generic;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface ISourceFilePresenter : IPresenter<ISourceFileControl>
   {
      void OpenFileDialog();

      event SourceFileChangedHandler OnSourceFileChanged;

      void SetFilePath(string filePath);

      void SetColumnInfos(IReadOnlyList<ColumnInfo> columnInfos);
   }

   public delegate void SourceFileChangedHandler(object sender, SourceFileChangedEventArgs e);

   public class SourceFileChangedEventArgs : EventArgs
   {
      /// <summary>
      ///    FileName describing what is missed.
      /// </summary>
      public IDataSourceFile FileName { get; set; }
   }


}