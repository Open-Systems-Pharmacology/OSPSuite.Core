using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;

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

      public bool IsMandatory { get; set; }

      public string BaseGridName { get; set; }

      /// <summary>
      ///    Column name of related column. The related column must have an auxiliary type as meta data category.
      /// </summary>
      public string RelatedColumnOf { get; set; }

      /// <summary>
      ///    List of possible supported dimensions.This List may be empty if all dimensions are supported
      /// </summary>
      public List<IDimension> SupportedDimensions { get; }

      public IDimension DefaultDimension { get; set; }
      public IList<MetaDataCategory> MetaDataCategories { get; }

      public ColumnInfo()
      {
         Name = string.Empty;
         DisplayName = string.Empty;
         IsMandatory = true;
         SupportedDimensions = new List<IDimension>();
         DefaultDimension = null;
         MetaDataCategories = new List<MetaDataCategory>();
      }

      public bool IsBase
      {
         get => string.IsNullOrEmpty(BaseGridName) || BaseGridName == Name;
      }

      public bool IsAuxiliary
      {
         get => !IsMandatory;
      }

      public bool IsMeasurement
      {
         get => !IsBase && !IsAuxiliary;
      }

      public IDimension MappedDimension { get; set; }

      public IDimension DimensionForUnit(string unitName)
      {
         return MappedDimension ?? SupportedDimensions.DimensionForUnit(unitName);
      }
   }

   public class ColumnInfoCache : Cache<string, ColumnInfo>
   {
      public ColumnInfoCache() : base(x => x.DisplayName)
      {
      }

      public ColumnInfoCache(IReadOnlyList<ColumnInfo> columnInfos) : this()
      {
         AddRange(columnInfos);
      }

      public IEnumerable<ColumnInfo> RelatedColumnsFrom(string anotherColumn)
      {
         return this.Where(c => c.IsAuxiliary && c.RelatedColumnOf == anotherColumn);
      }
   }
}