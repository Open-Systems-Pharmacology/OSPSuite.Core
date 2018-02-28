using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Presentation.DTO
{
   public class CategoryDTO : Notifier, IWithName
   {
      private bool _selected;
      public string Name { set; get; }
      public string DisplayName { get; set; }

      public IReadOnlyList<CalculationMethod> Methods { get; set; }

      public bool Selected
      {
         get => _selected;
         set => SetProperty(ref _selected, value);
      }
   }
}