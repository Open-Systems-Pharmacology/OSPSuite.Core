using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Presentation.DTO
{
   public class QuantitySelectionDTO : PathRepresentableDTO, IWithDimension
   {
      private bool _selected;

      /// <summary>
      ///    String used when ToString is called. If not, set, DisplayPathAsString will be used instead
      /// </summary>
      public string DisplayString { get; set; }

      /// <summary>
      ///    Name of the underlying quantity
      /// </summary>
      public string QuantityName { get; set; }

      /// <summary>
      ///    Path of the quantity in the model (without the name of the simulation)
      /// </summary>
      public string QuantityPath { get; set; }

      /// <summary>
      ///    The underlying quantity
      /// </summary>
      public IQuantity Quantity { get; set; }

      public QuantityType QuantityType { get; set; }

      public IDimension Dimension { get; set; }

      public string DimensionDisplayName => Dimension?.ToString() ?? string.Empty;

      public int Sequence { get; set; }

      public QuantitySelection ToQuantitySelection() => new QuantitySelection(QuantityPath, QuantityType);

      public bool Selected
      {
         get { return _selected; }
         set
         {
            //this check is extremly important to avoid raising Selected event too many time 
            //which may result in total rebind of the UI
            if (_selected == value)
               return;
            _selected = value;
            OnPropertyChanged(() => Selected);
         }
      }

      public override string ToString()
      {
         return DisplayString ?? DisplayPathAsString;
      }
   }
}