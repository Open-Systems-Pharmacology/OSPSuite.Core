﻿using System;
using System.Collections.Generic;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   /// <summary>
   ///    Event arguments for OnMissingMapping event.
   /// </summary>
   public class MissingMappingEventArgs : EventArgs
   {
      /// <summary>
      ///    Message describing what is missed.
      /// </summary>
      public string Message { get; set; }

      public string RowName { get; set; }
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

   public interface IColumnMappingPresenter : IPresenter<IColumnMappingView>
   {
      void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, ColumnInfoCache columnInfos);
      IDataFormat GetDataFormat();
      void SetDataFormat(IDataFormat format);
      void SetRawData(DataSheet rawDataSheet);
      IEnumerable<ColumnMappingOption> GetAvailableOptionsFor(ColumnMappingDTO model);
      IEnumerable<RowOptionDTO> GetAvailableRowsFor(ColumnMappingDTO model);
      ToolTipDescription ToolTipDescriptionFor(int index);
      void ClearRow(ColumnMappingDTO model);
      void AddGroupBy(AddGroupByFormatParameter source);
      void ClearMapping();
      void ValidateMapping();
      void SetSubEditorSettingsForMapping(ColumnMappingDTO model);
      bool ShouldManualInputOnMetaDataBeEnabled(ColumnMappingDTO model);
      bool ShouldManualInputOnMetaDataBeEnabled(DataFormatParameter parameter);
      void UpdateDescriptionForModel(MappingDataFormatParameter mappingSource);
      void UpdateMetaDataForModel(MetaDataFormatParameter mappingSource);
      void SetDescriptionForRow(ColumnMappingDTO model);

      event EventHandler OnMappingCompleted; //status: you can import

      event EventHandler<MissingMappingEventArgs> OnMissingMapping;
      IEnumerable<string> GetAllAvailableExcelColumns();
   }
}