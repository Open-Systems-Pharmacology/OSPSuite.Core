using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ColumnInfo
   {
      private string _name;

      public string Name
      {
         get => _name;
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

      /// <summary>
      ///    List of possible supported dimensions.This List may be empty if all dimensions are supported
      /// </summary>
      public IList<IDimension> SupportedDimensions { get; }

      public IDimension DefaultDimension { get; set; }
      public IList<MetaDataCategory> MetaDataCategories { get; }

      public Type DataType { get; set; }

      public ColumnInfo()
      {
         DataType = typeof(double);
         Name = string.Empty;
         DisplayName = string.Empty;
         Description = string.Empty;
         NullValuesHandling = NullValuesHandlingType.DeleteRow;
         IsMandatory = true;
         SupportedDimensions = new List<IDimension>();
         DefaultDimension = null;
         MetaDataCategories = new List<MetaDataCategory>();
      }
   }
}