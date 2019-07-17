using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ReactionBuilder : ContextSpecification<IReactionBuilder>
   {
      protected override void Context()
      {
         sut = new ReactionBuilder();
      }
   }

   class When_Adding_a_modifier : concern_for_ReactionBuilder
   {
      private string _modifier;

      protected override void Context()
      {
         base.Context();
         _modifier = "Modifier";
      }
      protected override void Because()
      {
         sut.AddModifier(_modifier);
      }
      [Observation]
      public void modifierNames_should_contain_modifier()
      {
         sut.ModifierNames.ShouldContain(_modifier);
      }
   }

   class When_Removing_a_modifier : concern_for_ReactionBuilder
   {
      private string _modifier;
      private string _otherModifier;

      protected override void Context()
      {
         base.Context();
         _modifier = "Modifier";
         _otherModifier = "other";
         sut.AddModifier(_modifier);
         sut.AddModifier(_otherModifier);
      }
      protected override void Because()
      {
         sut.RemoveModifier(_modifier);
      }
      [Observation]
      public void modifierNames_should_only_contain_other_modifier()
      {
         sut.ModifierNames.ShouldOnlyContain(_otherModifier);
      }
   }

   public class When_setting_the_create_parameter_rate_flag_to_false : concern_for_ReactionBuilder
   {
      protected override void Context()
      {
         base.Context();
         sut.CreateProcessRateParameter = true;
         sut.ProcessRateParameterPersistable = true;
      }

      protected override void Because()
      {
         sut.CreateProcessRateParameter = false;
      }

      [Observation]
      public void should_also_set_the_persistable_flag_to_false()
      {
         sut.ProcessRateParameterPersistable.ShouldBeFalse();
      }
   }

   public class When_setting_the_plot_parameter_rate_flag_to_true_when_the_create_parameter_flag_is_false : concern_for_ReactionBuilder
   {
      protected override void Context()
      {
         base.Context();
         sut.CreateProcessRateParameter = false;
      }

      protected override void Because()
      {
         sut.ProcessRateParameterPersistable = true;
      }

      [Observation]
      public void should_also_set_the_persistable_flag_to_false()
      {
         sut.ProcessRateParameterPersistable.ShouldBeFalse();
      }
   }
}	

