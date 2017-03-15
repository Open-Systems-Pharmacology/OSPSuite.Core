using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public abstract class concern_for_Comparison : ContextSpecification<IComparison<IParameter>>
   {
      private IEntity _commonAncestor;
      protected IParameter _parameter1;
      protected IParameter _parameter2;
      private ComparerSettings _settings;
      protected DiffReport _report;

      protected override void Context()
      {
         _report = new DiffReport();
         _commonAncestor = new Container();
         _settings = new ComparerSettings();
         CreateSut();
      }

      protected void CreateSut()
      {
         sut = new Comparison<IParameter>(_parameter1, _parameter2, _settings, _report, _commonAncestor);
      }
   }

   public class When_checking_if_the_objects_are_well_defined_in_a_comparison : concern_for_Comparison
   {
      [Observation]
      public void should_return_true_if_both_objects_are_not_null()
      {
         _parameter1 = new Parameter();
         _parameter2 = new Parameter();
         CreateSut();
         sut.ComparedObjectsDefined.ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_first_object_is_null()
      {
         _parameter1 = null;
         _parameter2 = new Parameter();
         CreateSut();
         sut.ComparedObjectsDefined.ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_if_the_second_object_is_null()
      {
         _parameter1 = new Parameter();
         _parameter2 = null;
         CreateSut();
         sut.ComparedObjectsDefined.ShouldBeFalse();
      }
   }

   public class When_adding_an_item_to_the_comparison_report : concern_for_Comparison
   {
      private DiffItem _diffItem;

      protected override void Context()
      {
         base.Context();
         _diffItem = new PropertyValueDiffItem();
      }

      protected override void Because()
      {
         sut.Add(_diffItem);
      }

      [Observation]
      public void should_add_the_item_to_the_underlying_report()
      {
         _report.ShouldContain(_diffItem);
      }
   }
}