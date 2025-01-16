using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_DimensionSelectionDTO : ContextSpecification<DimensionSelectionDTO>
   {
      protected override void Context()
      {
         sut = new DimensionSelectionDTO(SheetName, Description, ColumnInfo, SupportingDimensions);
      }

      protected abstract IReadOnlyList<IDimension> SupportingDimensions { get; }

      protected abstract ColumnInfo ColumnInfo { get; } 

      protected abstract IReadOnlyList<string> Description { get; }

      protected abstract string SheetName { get; }
   }

   public class When_selecting_dimension_and_sheet_name_is_empty : concern_for_DimensionSelectionDTO
   {
      protected override IReadOnlyList<IDimension> SupportingDimensions { get; } = new List<IDimension>();
      protected override ColumnInfo ColumnInfo { get; } = new ColumnInfo() {Name = "column"};
      protected override IReadOnlyList<string> Description { get; } = new[] { "group 1" };
      protected override string SheetName { get; } = string.Empty;

      [Observation]
      public void the_display_name_only_had_column_and_description()
      {
         sut.DisplayName.ShouldBeEqualTo("column - group 1");
      }
   }

   public class When_selecting_dimension_and_sheet_name_is_not_empty : concern_for_DimensionSelectionDTO
   {
      protected override IReadOnlyList<IDimension> SupportingDimensions { get; } = new List<IDimension>();
      protected override ColumnInfo ColumnInfo { get; } = new ColumnInfo() { Name = "column" };
      protected override IReadOnlyList<string> Description { get; } = new[] { "group 1" };
      protected override string SheetName { get; } = "sheet";

      [Observation]
      public void the_display_name_only_had_column_and_description()
      {
         sut.DisplayName.ShouldBeEqualTo("sheet - column - group 1");
      }
   }

   public class When_selecting_dimension_and_group_is_empty : concern_for_DimensionSelectionDTO
   {
      protected override IReadOnlyList<IDimension> SupportingDimensions { get; } = new List<IDimension>();
      protected override ColumnInfo ColumnInfo { get; } = new ColumnInfo() { Name = "column" };
      protected override IReadOnlyList<string> Description { get; } = new List<string>();
      protected override string SheetName { get; } = "sheet";

      [Observation]
      public void the_display_name_only_had_column_and_description()
      {
         sut.DisplayName.ShouldBeEqualTo("sheet - column");
      }
   }
}
