using System.Collections.Generic;
using OSPSuite.Utility.Reflection;
using OSPSuite.Core.Domain;

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
         get { return _selected; }
         set
         {
            if(_selected==value)
               return;
            
            _selected = value;
            OnPropertyChanged(() => Selected);
         }
      }
   }
}