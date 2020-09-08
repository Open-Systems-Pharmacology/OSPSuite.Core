using System;
using System.Collections.Generic;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataFormat;
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

   /// <summary>
   ///    Handler for OnMissingMapping event.
   /// </summary>
   public delegate void MissingMappingHandler(object sender, MissingMappingEventArgs e);

   public delegate void MappingCompletedHandler(object sender);

   public class DataFormatParametersChangedArgs : EventArgs
   {
      public IReadOnlyList<DataFormatParameter> Parameters { get; }

      public DataFormatParametersChangedArgs(params DataFormatParameter[] parameters)
      {
         Parameters = new List<DataFormatParameter>(parameters);
      }
   }

   public delegate void DataFormatParametersChangedHandler(object sender, DataFormatParametersChangedArgs e);

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

   public delegate void ParameterChangedHandler(string columnName, DataFormatParameter parameter);

   public delegate void FormatPropertiesChangedHandler(IEnumerable<DataFormatParameter> parameters);

   public interface IColumnMappingPresenter : IPresenter<IColumnMappingControl>
   {
      void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos
      );

      void SetDataFormat(IDataFormat format);

      IEnumerable<ColumnMappingOption> GetAvailableOptionsFor(ColumnMappingViewModel model);

      ToolTipDescription ToolTipDescriptionFor(int index);

      void SetDescriptionForRow(ColumnMappingViewModel model);

      void ClearRow(ColumnMappingViewModel model);

      void ResetMapping();

      void ClearMapping();

      void ChangeUnitsOnRow(ColumnMappingViewModel model);

      void ValidateMapping();

      event MappingCompletedHandler OnMappingCompleted; //status: you can import

      event MissingMappingHandler OnMissingMapping;
      
      event ParameterChangedHandler OnParameterChanged;

      event FormatPropertiesChangedHandler OnFormatPropertiesChanged;
   }
}