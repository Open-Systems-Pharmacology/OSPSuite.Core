using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Importer
{
   public class ColumnInfo
   {
      private string _name;

      public string Name
      {
         get { return _name; }
         set
         {
            _name = value;
            if (string.IsNullOrEmpty(DisplayName))
               DisplayName = value;
         }
      }

      public string DisplayName { get; set; }
      public string Description { get; set; }
      public NullValuesHandlingType NullValuesHandling { get; set; }
      public bool IsMandatory { get; set; }
      public string BaseGridName { get; set; }

      /// <summary>
      ///    Column name of related column. The related column must have an auxiliary type as meta data category.
      /// </summary>
      public string RelatedColumnOf { get; set; }
      public IList<DimensionInfo> DimensionInfos { get; private set; }
      public IDimension DefaultDimension { get; set; }
      public IList<MetaDataCategory> MetaDataCategories { get; private set; }

      public Type DataType { get; set; }
      public ColumnInfo()
      {
         DataType = typeof(double);
         Name = string.Empty;
         DisplayName = string.Empty;
         Description = string.Empty;
         NullValuesHandling = NullValuesHandlingType.DeleteRow;
         IsMandatory = true;
         DimensionInfos = new List<DimensionInfo>();
         DefaultDimension = null;
         MetaDataCategories = new List<MetaDataCategory>();
      }
   }
}