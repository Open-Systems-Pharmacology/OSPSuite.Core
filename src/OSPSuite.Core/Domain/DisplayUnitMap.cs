using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public class DisplayUnitMap : Notifier, IValidatable, IWithDisplayUnit
   {
      public IBusinessRuleSet Rules { get; private set; }

      private IDimension _dimension;

      public virtual IDimension Dimension
      {
         get { return _dimension; }
         set
         {
            _dimension = value;
            updateDefaultUnit();
            OnPropertyChanged(() => Dimension);
         }
      }

      private void updateDefaultUnit()
      {
         if (_dimension == null)
         {
            DisplayUnit = null;
            return;
         }

         if (_dimension.Units.Contains(DisplayUnit))
            return;

         DisplayUnit = _dimension.DefaultUnit;
      }

      private Unit _displayUnit;

      public virtual Unit DisplayUnit
      {
         get { return _displayUnit; }
         set
         {
            _displayUnit = value;
            OnPropertyChanged(() => DisplayUnit);
         }
      }

      public DisplayUnitMap()
      {
         Rules = new BusinessRuleSet(AllRules.All());
      }

      public DisplayUnitMap Clone()
      {
         return new DisplayUnitMap {Dimension = Dimension, DisplayUnit = DisplayUnit};
      }

      private static class AllRules
      {
         public static IEnumerable<IBusinessRule> All()
         {
            yield return GenericRules.NotNull<DisplayUnitMap, IDimension>(x => x.Dimension);
            yield return GenericRules.NotNull<DisplayUnitMap, Unit>(x => x.DisplayUnit);
         }
      }
   }
}