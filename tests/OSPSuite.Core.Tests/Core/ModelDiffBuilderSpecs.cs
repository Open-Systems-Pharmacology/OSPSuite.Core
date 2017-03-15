using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Core.Domain;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public class When_comparing_models_with_different_parameters_values : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var simulaitonHelper = IoC.Resolve<SimulationHelperForSpecs>();
         var model1 = simulaitonHelper.CreateSimulation().Model;
         var model2 = simulaitonHelper.CreateSimulation().Model;

         var param1 = model1.Root.EntityAt<IParameter>(Constants.ORGANISM, "BW");
         param1.Value = 10;

         var param2 = model1.Root.EntityAt<IParameter>(Constants.ORGANISM, "BW");
         param2.Value = 30;

         _object1 = model1;
         _object2 = model2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }
}