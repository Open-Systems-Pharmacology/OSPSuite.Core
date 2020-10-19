using System;
using System.Collections.Generic;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataFormat;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   /// <summary>
   ///    Event arguments for OnMissingMapping event.
   /// </summary>
   public class MissingMappingEventArgs : EventArgs
   {
      /// <summary>
      ///    FileName describing what is missed.
      /// </summary>
      public string Message { get; set; }
   }

   public delegate void MappingCompletedHandler(object sender);

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
         Mapping,
         AddGroupBy
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
      void SetSettings( IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos);
      IDataFormat GetDataFormat();
      void SetDataFormat(IDataFormat format);
      void SetRawData(UnformattedData rawData);
      IEnumerable<ColumnMappingOption> GetAvailableOptionsFor(ColumnMappingDTO model);
      ToolTipDescription ToolTipDescriptionFor(int index);
      void SetDescriptionForRow(ColumnMappingDTO model);
      void ClearRow(ColumnMappingDTO model);
      void AddGroupBy(AddGroupByFormatParameter source);
      void ResetMapping();
      void ClearMapping();
      void ChangeUnitsOnRow(ColumnMappingDTO model);
      void ChangeLloqOnRow(ColumnMappingDTO model);
      void ValidateMapping();

      event EventHandler OnMappingCompleted; //status: you can import

      event EventHandler<MissingMappingEventArgs> OnMissingMapping;
   }
}