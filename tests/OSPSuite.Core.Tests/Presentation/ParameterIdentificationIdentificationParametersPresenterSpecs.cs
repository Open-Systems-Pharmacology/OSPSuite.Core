using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ParameterIdentificationIdentificationParametersPresenter : ContextSpecification<IParameterIdentificationIdentificationParametersPresenter>
   {
      protected IIdentificationParameterToIdentificationParameterDTOMapper _identificationParameterDTOMapper;
      protected IParameterIdentificationIdentificationParametersView _view;
      protected IIdentificationParameterFactory _identificationParameterFactory;
      protected ParameterIdentification _parameterIdentification;
      protected IdentificationParameter _identificationParameter;
      protected IdentificationParameterDTO _identificationParameterDTO;
      protected List<IdentificationParameterDTO> _allIdentificationParameterDTO;
      private IIdentificationParameterTask _identificationParameterTask;

      protected override void Context()
      {
         _identificationParameterDTOMapper = A.Fake<IIdentificationParameterToIdentificationParameterDTOMapper>();
         _view = A.Fake<IParameterIdentificationIdentificationParametersView>();
         _identificationParameterFactory = A.Fake<IIdentificationParameterFactory>();
         _identificationParameterTask= A.Fake<IIdentificationParameterTask>();

         sut = new ParameterIdentificationIdentificationParametersPresenter(_view, _identificationParameterFactory, _identificationParameterDTOMapper,_identificationParameterTask);

         _parameterIdentification = new ParameterIdentification();
         _identificationParameter = new IdentificationParameter();
         _parameterIdentification.AddIdentificationParameter(_identificationParameter);
         _identificationParameterDTO = new IdentificationParameterDTO(_identificationParameter);
         A.CallTo(() => _identificationParameterDTOMapper.MapFrom(_identificationParameter)).Returns(_identificationParameterDTO);

         A.CallTo(() => _view.BindTo(A<IEnumerable<IdentificationParameterDTO>>._))
            .Invokes(x => _allIdentificationParameterDTO = x.GetArgument<IEnumerable<IdentificationParameterDTO>>(0).ToList());
      }
   }

   public class When_the_identification_parameter_presenter_is_editiing_a_parameter_identification : concern_for_ParameterIdentificationIdentificationParametersPresenter
   {
      protected override void Because()
      {
         sut.EditParameterIdentification(_parameterIdentification);
      }

      [Observation]
      public void should_display_all_available_identification_parameters_to_the_user()
      {
         _allIdentificationParameterDTO.ShouldOnlyContain(_identificationParameterDTO);
      }
   }

   public class When_the_identification_parameter_presenter_is_adding_some_parameters_to_the_parameter_identification : concern_for_ParameterIdentificationIdentificationParametersPresenter
   {
      private IReadOnlyList<ParameterSelection> _parameters;
      private IdentificationParameter _newIdentificationParameter;
      private IdentificationParameterDTO _newIdentificationParameterDTO;

      protected override void Context()
      {
         base.Context();
         _parameters = new List<ParameterSelection>();
         _newIdentificationParameter = new IdentificationParameter();
         _newIdentificationParameterDTO = new IdentificationParameterDTO(_newIdentificationParameter);
         A.CallTo(() => _identificationParameterFactory.CreateFor(_parameters, _parameterIdentification)).Returns(_newIdentificationParameter);
         A.CallTo(() => _identificationParameterDTOMapper.MapFrom(_newIdentificationParameter)).Returns(_newIdentificationParameterDTO);
         sut.EditParameterIdentification(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.AddParameters(_parameters);
      }

      [Observation]
      public void should_create_a_new_identification_parameter_for_these_parameters_and_add_it_to_the_edited_parameter_identification()
      {
         _parameterIdentification.AllIdentificationParameters.ShouldContain(_newIdentificationParameter);
      }

      [Observation]
      public void should_update_the_view_with_the_newly_added_identification_parameter()
      {
         _allIdentificationParameterDTO.ShouldOnlyContain(_identificationParameterDTO, _newIdentificationParameterDTO);
      }

      [Observation]
      public void should_select_the_new_identification_parameter()
      {
         _view.SelectedIdentificationParameter.ShouldBeEqualTo(_newIdentificationParameterDTO);
      }
   }

   public class When_the_user_is_deleting_an_identification_parameter : concern_for_ParameterIdentificationIdentificationParametersPresenter
   {
      private IdentificationParameter _identificationParameter2;
      private IdentificationParameterDTO _identificationParameterDTO2;

      protected override void Context()
      {
         base.Context();
         _identificationParameter2 = new IdentificationParameter();
         _identificationParameterDTO2 = new IdentificationParameterDTO(_identificationParameter2);
         _parameterIdentification.AddIdentificationParameter(_identificationParameter2);
         A.CallTo(() => _identificationParameterDTOMapper.MapFrom(_identificationParameter2)).Returns(_identificationParameterDTO2);
         sut.EditParameterIdentification(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.RemoveIdentificationParameter(_identificationParameterDTO);
      }

      [Observation]
      public void should_remove_the_identification_parameter_from_the_parameter_identification()
      {
         _parameterIdentification.AllIdentificationParameters.ShouldNotContain(_identificationParameter);
      }

      [Observation]
      public void should_remove_the_identification_parameter_from_the_view()
      {
         _allIdentificationParameterDTO.ShouldOnlyContain(_identificationParameterDTO2);
      }

      [Observation]
      public void should_select_the_first_remaining_identification_parameter()
      {
         _view.SelectedIdentificationParameter.ShouldBeEqualTo(_identificationParameterDTO2);
      }
   }


   public class When_the_user_is_deleting_the_last_identification_parameter : concern_for_ParameterIdentificationIdentificationParametersPresenter
   {
      private bool _eventRaised;

      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
         sut.NoIdentificationParameterSelected += (o, e) => { _eventRaised = true; };
      }

      protected override void Because()
      {
         sut.RemoveIdentificationParameter(_identificationParameterDTO);
      }

      [Observation]
      public void should_notify_that_no_identification_parameter_is_selected()
      {
         _eventRaised.ShouldBeTrue();
      }
   }

   public class When_the_user_is_selecting_an_identification_parameter : concern_for_ParameterIdentificationIdentificationParametersPresenter
   {
      private IdentificationParameter _selectedIdentificationParameter;

      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
         sut.IdentificationParameterSelected += (o, e) => { _selectedIdentificationParameter = e.IdentificationParameter; };
      }

      protected override void Because()
      {
         sut.SelectIdentificationParameter(_identificationParameterDTO);
      }

      [Observation]
      public void should_notify_the_selection()
      {
         _selectedIdentificationParameter.ShouldBeEqualTo(_identificationParameter);
      }
   }

   public class When_the_value_of_a_parameter_is_set_by_the_user_in_the_identification_parameter : concern_for_ParameterIdentificationIdentificationParametersPresenter
   {
      private IParameterDTO _minValueDTO;

      protected override void Context()
      {
         base.Context();
         _minValueDTO= A.Fake<IParameterDTO>();
      }

      protected override void Because()
      {
         sut.SetParameterValue(_minValueDTO,10);
      }
      [Observation]
      public void should_set_the_value_in_display_unit_of_the_underlying_parameter()
      {
         _minValueDTO.Parameter.ValueInDisplayUnit.ShouldBeEqualTo(10);
      }
   }

   public class When_the_unit_of_a_parameter_is_set_by_the_user_in_the_identification_parameter : concern_for_ParameterIdentificationIdentificationParametersPresenter
   {
      private IParameterDTO _minValueDTO;
      private Unit _newDisplayUnit;

      protected override void Context()
      {
         base.Context();
         _newDisplayUnit= A.Fake<Unit>();;
         _minValueDTO = A.Fake<IParameterDTO>();
         _minValueDTO.Value = 5;
      }

      protected override void Because()
      {
         sut.SetParameterUnit(_minValueDTO, _newDisplayUnit);
      }

      [Observation]
      public void should_set_the_value_in_display_unit_of_the_underlying_parameter()
      {
         _minValueDTO.Parameter.DisplayUnit.ShouldBeEqualTo(_newDisplayUnit);
         _minValueDTO.Parameter.ValueInDisplayUnit.ShouldBeEqualTo(5);
      }
   }
}