using System.Collections.Generic;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ColumnMappingOption
   {
      public string Label { get; set; }
      public int IconIndex { get; set; }
      public string Description { get; set; }

      public enum DescriptionType
      {
         Ignored,
         GroupBy,
         MetaData,
         Mapping
      }
   }

   public class ToolTipDescription
   {
      public string Title { get; set; }
      public string Description { get; set; }
   }

   public class ButtonsConfiguration
   {
      public bool ShowButtons { get; set; }
      public bool UnitActive { get; set; }
   }

   public interface IColumnMappingPresenter : IPresenter<IColumnMappingControl>
   {
      void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      );

      void SetDataFormat(IDataFormat format);

      IEnumerable<ColumnMappingOption> GetAvailableOptionsFor(int rowHandle);

      ButtonsConfiguration ButtonsConfigurationForActiveRow();

      ToolTipDescription ToolTipDescriptionFor(int index);

      void SetDescriptionForActiveRow(string description);

      void ClearActiveRow();

      void ResetMapping();

      void ClearMapping();
   }
}