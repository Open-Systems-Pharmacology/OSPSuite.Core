using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ValueOriginPresenter : ContextSpecification<IValueOriginPresenter>
   {
      protected IValueOriginView _view;
      protected ValueOrigin _valueOriginToEdit;
      protected ValueOriginSource _originalSource = ValueOriginSources.Internet;
      protected string _originalDescription = "Original description";
      protected ValueOrigin _dto;

      protected override void Context()
      {
         _view = A.Fake<IValueOriginView>();
         sut = new ValueOriginPresenter(_view);

         _valueOriginToEdit = new ValueOrigin
         {
            Source = _originalSource,
            Description = _originalDescription,
         };


         A.CallTo(() => _view.BindTo(A<ValueOrigin>._)).Invokes(x => _dto = x.GetArgument<ValueOrigin>(0));
      }
   }

   public class When_the_value_origin_presenter_is_editing_a_value_origin : concern_for_ValueOriginPresenter
   {
      protected override void Because()
      {
         sut.Edit(_valueOriginToEdit);
      }

      [Observation]
      public void it_should_edit_another_instance_of_value_origin_having_the_same_properties_as_the_edited_value_origin()
      {
         Assert.AreNotSame(_dto,_valueOriginToEdit);
         _dto.Source.ShouldBeEqualTo(_originalSource);
         _dto.Description.ShouldBeEqualTo(_originalDescription);
      }
   }

   public class When_the_value_origin_presenter_is_saving_a_value_origin_for_which_the_type_is_not_set_and_the_description_is_not_empty : concern_for_ValueOriginPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_valueOriginToEdit);
         _dto.Source = ValueOriginSources.Undefined;
         _dto.Description = "New Description";
      }

      protected override void Because()
      {
         sut.Save();
      }

      [Observation]
      public void should_udpate_the_value_type_to_unknown()
      {
         _dto.Source.ShouldBeEqualTo(ValueOriginSources.Unknown);
         _dto.Description.ShouldBeEqualTo(_dto.Description);
      }
   }

   public class When_the_value_origin_presenter_is_saving_a_value_origin_for_which_the_type_is_not_set_and_the_description_is_empty : concern_for_ValueOriginPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_valueOriginToEdit);
         _dto.Source = ValueOriginSources.Undefined;
         _dto.Description = "   ";
      }

      protected override void Because()
      {
         sut.Save();
      }

      [Observation]
      public void should_not_update_the_value_origin_type()
      {
         _dto.Source.ShouldBeEqualTo(ValueOriginSources.Undefined);
      }
   }

   public class When_the_value_origin_presenter_is_return_all_possible_origin_sources : concern_for_ValueOriginPresenter
   {
      [Observation]
      public void should_return_all_defined_value_origin_sources_except_the_undefined_source()
      {
         sut.AllValueOriginSources.ShouldOnlyContainInOrder(
            ValueOriginSources.All.Except(new[] {ValueOriginSources.Undefined})
         );
      }
   }

   public class When_the_value_origin_presenter_is_return_all_possible_origin_determination_methods : concern_for_ValueOriginPresenter
   {
      [Observation]
      public void should_return_all_defined_value_origin_methodss_except_the_undefined_method()
      {
         sut.AllValueOriginDeterminationMethods.ShouldOnlyContainInOrder(
            ValueOriginDeterminationMethods.All.Except(new[] { ValueOriginDeterminationMethods.Undefined }));
      }
   }

   public class When_checking_if_the_edited_value_origin_has_changed : concern_for_ValueOriginPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_valueOriginToEdit);
      }

      [Observation]
      public void should_return_true_if_the_value_origin_source_has_changed()
      {
         _dto.Source = ValueOriginSources.Database;
         sut.ValueOriginChanged.ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_if_the_value_origin_determination_method_has_changed()
      {
         _dto.Method = ValueOriginDeterminationMethods.ParameterIdentification;
         sut.ValueOriginChanged.ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_if_the_description_has_changed()
      {
         _dto.Description = "A brand new description";
         sut.ValueOriginChanged.ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_description_only_contains_empty_charachters()
      {
         _valueOriginToEdit.Description = "";
         _dto.Description = "     ";
         sut.ValueOriginChanged.ShouldBeFalse();
      }

   }

   public class When_the_value_origin_presenter_is_returning_the_updated_value_origin : concern_for_ValueOriginPresenter
   {
      private ValueOrigin _updatedValueOrigin;

      protected override void Context()
      {
         base.Context();
         sut.Edit(_valueOriginToEdit);
      }


      protected override void Because()
      {
         _updatedValueOrigin = sut.UpdatedValueOrigin;
      }

      [Observation]
      public void should_save_the_value_and_return_the_dto()
      {
         A.CallTo(() => _view.Save()).MustHaveHappened();
         _updatedValueOrigin.ShouldBeEqualTo(_dto);
      }
   }
}