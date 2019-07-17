using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_DimensionFactory : ContextForIntegration<DimensionFactory>
   {
      protected IEnumerable<string> _timeUnits;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _timeUnits = IoC.Resolve<IDimensionFactory>().Dimension(Constants.Dimension.TIME).Units.Select(x => x.Name);

      }
   }

   public class When_looking_for_units_from_the_time_dimension : concern_for_DimensionFactory
   {
      [Observation]
      public void they_must_have_the_correct_names()
      {
         _timeUnits.ShouldContain(Constants.Dimension.Units.Days);
         _timeUnits.ShouldContain(Constants.Dimension.Units.Hours);
         _timeUnits.ShouldContain(Constants.Dimension.Units.Minutes);
         _timeUnits.ShouldContain(Constants.Dimension.Units.Months);
         _timeUnits.ShouldContain(Constants.Dimension.Units.Seconds);
         _timeUnits.ShouldContain(Constants.Dimension.Units.Weeks);
         _timeUnits.ShouldContain(Constants.Dimension.Units.Years);
      }
   }

}
