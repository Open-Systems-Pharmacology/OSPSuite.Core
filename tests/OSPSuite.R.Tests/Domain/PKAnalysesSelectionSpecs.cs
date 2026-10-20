using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.R.Domain
{
   public abstract class concern_for_PKAnalysesSelection : ContextSpecification<PKAnalysesSelection>
   {
      protected const string PLASMA_PATH = "Organism|Plasma";
      protected const string BLOOD_PATH = "Organism|Blood";
      protected QuantityPKParameter _plasmaCmax;
      protected QuantityPKParameter _plasmaTmax;
      protected QuantityPKParameter _bloodCmax;

      protected override void Context()
      {
         _plasmaCmax = new QuantityPKParameter { QuantityPath = PLASMA_PATH, Name = Constants.PKParameters.C_max };
         _plasmaTmax = new QuantityPKParameter { QuantityPath = PLASMA_PATH, Name = Constants.PKParameters.Tmax };
         _bloodCmax = new QuantityPKParameter { QuantityPath = BLOOD_PATH, Name = Constants.PKParameters.C_max };
         sut = new PKAnalysesSelection();
      }
   }

   public class When_nothing_was_added_to_the_pk_analyses_selection : concern_for_PKAnalysesSelection
   {
      [Observation]
      public void should_keep_all_pk_analyses()
      {
         sut.Keeps(_plasmaCmax).ShouldBeTrue();
         sut.Keeps(_plasmaTmax).ShouldBeTrue();
         sut.Keeps(_bloodCmax).ShouldBeTrue();
      }
   }

   public class When_quantity_paths_were_added_to_the_pk_analyses_selection : concern_for_PKAnalysesSelection
   {
      protected override void Because()
      {
         sut.AddQuantityPaths(PLASMA_PATH);
      }

      [Observation]
      public void should_keep_all_pk_parameters_of_these_outputs_only()
      {
         sut.Keeps(_plasmaCmax).ShouldBeTrue();
         sut.Keeps(_plasmaTmax).ShouldBeTrue();
         sut.Keeps(_bloodCmax).ShouldBeFalse();
      }
   }

   public class When_pk_parameter_names_were_added_to_the_pk_analyses_selection_in_several_calls : concern_for_PKAnalysesSelection
   {
      protected override void Because()
      {
         sut.AddPKParameterNames(Constants.PKParameters.Tmax);
         sut.AddPKParameterNames(Constants.PKParameters.C_max);
      }

      [Observation]
      public void should_keep_the_pk_parameters_added_in_all_calls()
      {
         sut.Keeps(_plasmaCmax).ShouldBeTrue();
         sut.Keeps(_plasmaTmax).ShouldBeTrue();
         sut.Keeps(_bloodCmax).ShouldBeTrue();
      }
   }

   public class When_quantity_paths_and_pk_parameter_names_were_added_to_the_pk_analyses_selection : concern_for_PKAnalysesSelection
   {
      protected override void Because()
      {
         sut.AddQuantityPaths(PLASMA_PATH);
         sut.AddPKParameterNames(Constants.PKParameters.C_max);
      }

      [Observation]
      public void should_only_keep_the_pk_analyses_matching_both()
      {
         sut.Keeps(_plasmaCmax).ShouldBeTrue();
         sut.Keeps(_plasmaTmax).ShouldBeFalse();
         sut.Keeps(_bloodCmax).ShouldBeFalse();
      }
   }

   public class When_an_empty_list_of_pk_parameter_names_was_added_to_the_pk_analyses_selection : concern_for_PKAnalysesSelection
   {
      protected override void Because()
      {
         sut.AddPKParameterNames();
      }

      [Observation]
      public void should_keep_no_pk_analysis()
      {
         sut.Keeps(_plasmaCmax).ShouldBeFalse();
         sut.Keeps(_plasmaTmax).ShouldBeFalse();
         sut.Keeps(_bloodCmax).ShouldBeFalse();
      }
   }
}
