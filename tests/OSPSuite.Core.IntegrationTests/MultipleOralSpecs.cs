using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_MultipleOral : ContextForModelConstructorIntegration
   {
      protected IModel _model;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _model = CreateFrom("multi_admin_oral").Model;
      }
   }

   public class When_creating_a_simulation_based_on_a_multi_oral_pksim_configuration : concern_for_MultipleOral
   {

      [Observation]
      public void should_create_the_expected_application_in_the_model()
      {
         _model.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_created_one_EHC_event_group()
      {
         var events = _model.Root.GetSingleChildByName<IContainer>("Events");
         var ehc = events.GetSingleChildByName<IEventGroup>("EHC");
         ehc.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_created_one_event_group_F1_for_the_weibull_application_with_three_applications()
      {
         var events = _model.Root.GetSingleChildByName<IContainer>("Applications");
         var f1 = events.GetSingleChildByName<IEventGroup>("F1");
         f1.ShouldNotBeNull();
         f1.GetAllChildren<IEventGroup>().Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_have_created_one_event_group_F1_for_the_lint80_application_with_three_applications()
      {
         var events = _model.Root.GetSingleChildByName<IContainer>("Applications");
         var f2 = events.GetSingleChildByName<IEventGroup>("F2");
         f2.ShouldNotBeNull();
         f2.GetAllChildren<IEventGroup>().Count.ShouldBeEqualTo(1);
      }

   }
}	