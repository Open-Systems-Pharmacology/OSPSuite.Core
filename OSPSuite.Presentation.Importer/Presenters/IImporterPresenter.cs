using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Importer.Core;
using System.Collections.Generic;
using OSPSuite.Core.Importer;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface IImporterPresenter : IPresenter<IImporterView>, ICommandCollectorPresenter
   {
      void SetDataFormat(IDataFormat format, IEnumerable<IDataFormat> availableFormats);

      //comes from ColumnMappingPresenter. Not sure if it really needs to be here
      void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      );

      event FormatChangedHandler OnFormatChanged;
      void SetDataSource(string path);
      void SelectTab(string tabName);
   }
}
