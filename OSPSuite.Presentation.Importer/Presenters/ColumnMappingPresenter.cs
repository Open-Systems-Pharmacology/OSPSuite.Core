using System;
using System.Collections.Generic;
using System.Linq;
using Org.BouncyCastle.Asn1.Cms;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ColumnMappingPresenter : AbstractPresenter<IColumnMappingControl, IColumnMappingPresenter>, IColumnMappingPresenter
   {
      private IDataFormat _format;
      private IEnumerable<ColumnMappingViewModel> _mappings;
      private IReadOnlyList<ColumnInfo> _columnInfos; 
      private IReadOnlyList<MetaDataCategory> _metaDataCategories;
      private DataImporterSettings _dataImporterSettings;
      private readonly IImporterTask _importerTask;


      public ColumnMappingPresenter(IColumnMappingControl view, IImporterTask importerTask) : base(view)
      {
         _importerTask = importerTask;
      }

      public void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      )
      {
         _columnInfos = columnInfos;
         _metaDataCategories = metaDataCategories;
         _dataImporterSettings = dataImporterSettings;
      }

      public void SetDataFormat(IDataFormat format)
      {
         _format = format;
         _mappings = format.Parameters.Select(p =>
         {
            var description = "";
            switch (p)
            {
               case IgnoredDataFormatParameter _:
                  description = Captions.Importer.NoneEditorNullText;
                  break;
               case GroupByDataFormatParameter _:
                  description = Captions.GroupByTitle;
                  break;
               default:
                  description = p.Configuration.ToString();
                  break;
            }
            return new ColumnMappingViewModel(p.ColumnName, description, p);
         });
         View.SetMappingSource(_mappings);
      }

      private ColumnMappingOption generateIgnoredColumnMappingOption()
      {
         return new ColumnMappingOption()
         {
            Description = Captions.Importer.NoneEditorNullText,
            IconIndex = -1
         };
      }

      private ColumnMappingOption generateGroupByColumnMappingOption()
      {
         return new ColumnMappingOption()
         {
            Description = Captions.GroupByTitle,
            IconIndex = _importerTask.GetImageIndex(new GroupByDataFormatParameter(""))
         };
      }

      private ColumnMappingOption generateMappingColumnMappingOption(string description, string columnName, string mappingId)
      {
         var mappedColumnName = EnumHelper.ParseValue<Column.ColumnNames>(mappingId);
         return new ColumnMappingOption()
         {
            Description = description,
            IconIndex = _importerTask.GetImageIndex(new MappingDataFormatParameter(columnName, new Column() { Name = mappedColumnName }), _mappings.Select(m => m.Source))
         };
      }

      private ColumnMappingOption generateMetaDataColumnMappingOption(string description, string columnName, string metadataId)
      {
         return new ColumnMappingOption()
         {
            Description = description,
            IconIndex = _importerTask.GetImageIndex(new MetaDataFormatParameter(columnName, metadataId), _mappings.Select(m => m.Source))
         };
      }

      public IEnumerable<ColumnMappingOption> GetAvailableOptionsFor(int rowHandle)
      {
         var mappingRow = _mappings.ElementAt(rowHandle);
         var options = new List<ColumnMappingOption>();
         switch (mappingRow.Source)
         {
            case IgnoredDataFormatParameter _:
               options.Add(generateIgnoredColumnMappingOption());
               break;
            case GroupByDataFormatParameter _:
               options.Add(generateGroupByColumnMappingOption());
               break;
            case MappingDataFormatParameter tm:
               options.Add(generateMappingColumnMappingOption(mappingRow.Description, tm.ColumnName, tm.MappedColumn.Name.ToString()));
               break;
            case MetaDataFormatParameter tm:
               options.Add(generateMetaDataColumnMappingOption(mappingRow.Description, tm.ColumnName, tm.MetaDataId));
               break;
            default:
               throw new Exception(Error.TypeNotSupported(mappingRow.Source.GetType()));
         }

         //Ignored
         if (!(mappingRow.Source is IgnoredDataFormatParameter))
         {
            options.Add(generateIgnoredColumnMappingOption());
         }

         //GroupBy
         if (!(mappingRow.Source is GroupByDataFormatParameter))
         {
            options.Add(generateGroupByColumnMappingOption());
         }

         //Mappings only for missing columns
         foreach (var info in _columnInfos)
         {
            if (!_mappings.Any(m =>
               m.Source is MappingDataFormatParameter && (m.Source as MappingDataFormatParameter)?.MappedColumn.Name.ToString() == info.DisplayName))
            {
               options.Add(generateMappingColumnMappingOption(Captions.MappingDescription(info.DisplayName, "?"), mappingRow.ColumnName, info.DisplayName));
            }
         }

         //MetaData only for missing data
         foreach (var category in _metaDataCategories)
         {
            if (!_mappings.Any(m => m.Source is MetaDataFormatParameter && (m.Source as MetaDataFormatParameter).MetaDataId == category.DisplayName))
            {
               options.Add(generateMetaDataColumnMappingOption(Captions.MetaDataDescription(category.DisplayName), mappingRow.ColumnName, category.DisplayName));
            }
         }

         return options;
      }
   }
}