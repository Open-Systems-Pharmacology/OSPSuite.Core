using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ValueOriginPresenter : ContextSpecification<IValueOriginPresenter>
   {
      private IValueOriginView _view;
      protected ValueOrigin _valueOriginToEdit;
      protected ValueOriginType _originalType = ValueOriginTypes.Internet;
      protected string _originalDescription = "Original description";
      protected ValueOrigin _dto;

      protected override void Context()
      {
         _view = A.Fake<IValueOriginView>();
         sut = new ValueOriginPresenter(_view);

         _valueOriginToEdit = new ValueOrigin
         {
            Type = _originalType,
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
         _dto.ShouldNotBeEqualTo(_valueOriginToEdit);
         _dto.Type.ShouldBeEqualTo(_originalType);
         _dto.Description.ShouldBeEqualTo(_originalDescription);
      }
   }

   public class When_the_value_origin_presenter_is_saving_the_edited_value_origin : concern_for_ValueOriginPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_valueOriginToEdit);
         _dto.Type = ValueOriginTypes.ParameterIdentification;
         _dto.Description = "Hello";

         _valueOriginToEdit.Type.ShouldBeEqualTo(_originalType);
         _valueOriginToEdit.Description.ShouldBeEqualTo(_originalDescription);
      }

      protected override void Because()
      {
         sut.Save();
      }

      [Observation]
      public void should_update_the_value_from_the_value_origin_bound_to_the_view()
      {
         _valueOriginToEdit.Type.ShouldBeEqualTo(_dto.Type);
         _valueOriginToEdit.Description.ShouldBeEqualTo(_dto.Description);
      }
   }

   public class When_the_value_origin_presenter_is_saving_a_value_origin_for_which_the_type_is_not_set_and_the_description_is_not_empty : concern_for_ValueOriginPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_valueOriginToEdit);
         _dto.Type = ValueOriginTypes.Undefined;
         _dto.Description = "New Description";
      }

      protected override void Because()
      {
         sut.Save();
      }

      [Observation]
      public void should_udpate_the_value_type_to_unknown()
      {
         _valueOriginToEdit.Type.ShouldBeEqualTo(ValueOriginTypes.Unknown);
         _valueOriginToEdit.Description.ShouldBeEqualTo(_dto.Description);
      }
   }

   public class When_the_value_origin_presenter_is_saving_a_value_origin_for_which_the_type_is_not_set_and_the_description_is_empty : concern_for_ValueOriginPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_valueOriginToEdit);
         _dto.Type = ValueOriginTypes.Undefined;
         _dto.Description = "   ";
      }

      protected override void Because()
      {
         sut.Save();
      }

      [Observation]
      public void should_not_update_the_value_origin_type()
      {
         _valueOriginToEdit.Type.ShouldBeEqualTo(_dto.Type);
      }
   }

   public class When_the_value_origin_presenter_is_return_all_possible_value_origin_types : concern_for_ValueOriginPresenter
   {
      [Observation]
      public void should_return_all_defined_value_origin_types_except_the_undefined_type()
      {
         sut.AllValueOrigins.ShouldOnlyContain(
            ValueOriginTypes.AllValueOrigins.Except(new[] {ValueOriginTypes.Undefined})
         );
      }
   }
}