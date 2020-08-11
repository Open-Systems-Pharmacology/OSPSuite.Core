using System.Collections.Generic;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Importer.Views
{
   public class ColumnMappingViewModel
   {
      public string ColumnName { get; private set; }
      public string Description { get; set; }
      public DataFormatParameter Source { get; private set; }
      public ColumnMappingViewModel(string columnName, string description, DataFormatParameter source)
      {
         ColumnName = columnName;
         Description = description;
         Source = source;
      }

      public override bool Equals(object obj)
      {
         var other = obj as ColumnMappingViewModel;
         return ColumnName == other.ColumnName && Description == other.Description && Source.Equals(other.Source);
      }

      public override int GetHashCode()
      {
         return base.GetHashCode();
      }
   }

   public interface IColumnMappingControl : IView<IColumnMappingPresenter>
   {
      void SetMappingSource(IEnumerable<ColumnMappingViewModel> mappings);
   }
}