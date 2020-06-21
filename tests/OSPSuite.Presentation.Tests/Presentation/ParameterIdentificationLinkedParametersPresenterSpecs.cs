using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterIdentificationLinkedParametersPresenter : ContextSpecification<IParameterIdentificationLinkedParametersPresenter>
   {
      protected ISimulationQuantitySelectionToLinkedParameterDTOMapper _linkedParameterDTOMapper;
      protected IParameterIdentificationLinkedParametersView _view;
      protected ParameterIdentification _parameterIdentification;
      protected List<LinkedParameterDTO> _allLinkedParameterDTO;
      protected IdentificationParameter _identificationParameter;
      protected ParameterSelection _linkedParameter;
      protected LinkedParameterDTO _linkedParameterDTO;

      protected override void Context()
      {
         _linkedParameterDTOMapper = A.Fake<ISimulationQuantitySelectionToLinkedParameterDTOMapper>();
         _view = A.Fake<IParameterIdentificationLinkedParametersView>();
         sut = new ParameterIdentificationLinkedParametersPresenter(_view, _linkedParameterDTOMapper);

         _parameterIdentification = new ParameterIdentification();
         sut.EditParameterIdentification(_parameterIdentification);

         A.CallTo(() => _view.BindTo(A<IEnumerable<LinkedParameterDTO>>._))
            .Invokes(x => _allLinkedParameterDTO = x.GetArgument<IEnumerable<LinkedParameterDTO>>(0).ToList());

         _identificationParameter = new IdentificationParameter();
         _linkedParameter = A.Fake<ParameterSelection>();
         _identificationParameter.AddLinkedParameter(_linkedParameter);

         _linkedParameterDTO = A.Fake<LinkedParameterDTO>();
         A.CallTo(() => _linkedParameterDTO.Quantity).Returns(_linkedParameter.Quantity);
         A.CallTo(() => _linkedParameterDTOMapper.MapFrom(_linkedParameter)).Returns(_linkedParameterDTO);

         sut.Edit(_identificationParameter);
      }
   }

   public class When_the_linked_parameters_presenter_is_editing_an_identification_parameter : concern_for_ParameterIdentificationLinkedParametersPresenter
   {
      [Observation]
      public void should_display_all_linked_parameters_to_the_user()
      {
         _allLinkedParameterDTO.ShouldContain(_linkedParameterDTO);
      }
   }

   public class When_the_linked_parameters_presennter_is_asked_if_a_linked_parameter_can_be_removed : concern_for_ParameterIdentificationLinkedParametersPresenter
   {
      [Observation]
      public void should_return_true_if_at_least_two_linked_parameteres_are_defined_in_the_identification_parameter()
      {
         var newParameterSelection = A.Fake<ParameterSelection>();   
         A.CallTo(() => newParameterSelection.Dimension).Returns(_linkedParameter.Dimension);

         _identificationParameter.AddLinkedParameter(newParameterSelection);
         sut.CanRemove(_linkedParameterDTO).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         sut.CanRemove(_linkedParameterDTO).ShouldBeFalse();
      }
   }

   public class When_the_linked_parameters_presennter_is_asked_if_a_linked_parameter_can_be_unlinked : concern_for_ParameterIdentificationLinkedParametersPresenter
   {
      [Observation]
      public void should_return_true_if_at_least_two_linked_parameteres_are_defined_in_the_identification_parameter()
      {
         var newParameterSelection = A.Fake<ParameterSelection>();
         A.CallTo(() => newParameterSelection.Dimension).Returns(_linkedParameter.Dimension);

         _identificationParameter.AddLinkedParameter(newParameterSelection);
         sut.CanUnlink(_linkedParameterDTO).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         sut.CanUnlink(_linkedParameterDTO).ShouldBeFalse();
      }
   }

   public class When_the_linked_parameters_presennter_is_told_to_unlink_a_linked_parameter : concern_for_ParameterIdentificationLinkedParametersPresenter
   {
      private SimulationQuantitySelection _unlinkedParameter;

      protected override void Context()
      {
         base.Context();
         sut.ParameterUnlinkedFromIdentificationParameter += (o, e) => { _unlinkedParameter = e.LinkedParameter; };
      }

      protected override void Because()
      {
         sut.UnlinkParameter(_linkedParameterDTO);
      }

      [Observation]
      public void should_remove_the_linked_parameter_from_the_identification_parameter()
      {
         _identificationParameter.AllLinkedParameters.ShouldNotContain(_linkedParameter);
      }

      [Observation]
      public void should_notify_that_a_parameter_was_unlinked()
      {
         _unlinkedParameter.ShouldBeEqualTo(_linkedParameter);
      }
   }

   public class When_the_linked_parameters_presennter_is_told_to_parameter : concern_for_ParameterIdentificationLinkedParametersPresenter
   {
      private ParameterSelection _newLinkedParameter;

      protected override void Context()
      {
         base.Context();
         sut.ParameterLinkedToIdentificationParameter += (o, e) => { _newLinkedParameter = e.LinkedParameter; };
      }

      protected override void Because()
      {
         sut.AddLinkedParameters(new [] {_linkedParameter});
      }

      [Observation]
      public void should_notify_that_a_parameter_was_linked()
      {
         _newLinkedParameter.ShouldBeEqualTo(_linkedParameter);
      }
   }

   public class When_the_linked_parameters_presennter_is_told_to_remove_a_linked_parameter : concern_for_ParameterIdentificationLinkedParametersPresenter
   {
      private SimulationQuantitySelection _unlinkedParameter;

      protected override void Context()
      {
         base.Context();
         sut.ParameterRemovedFromIdentificationParameter += (o, e) => { _unlinkedParameter = e.LinkedParameter; };
      }

      protected override void Because()
      {
         sut.RemoveParameter(_linkedParameterDTO);
      }

      [Observation]
      public void should_remove_the_linked_parameter_from_the_identification_parameter()
      {
         _identificationParameter.AllLinkedParameters.ShouldNotContain(_linkedParameter);
      }

      [Observation]
      public void should_notify_that_a_parameter_was_removed()
      {
         _unlinkedParameter.ShouldBeEqualTo(_linkedParameter);
      }
   }

   public class When_the_linked_parameter_presenter_is_told_to_clear_the_current_selection : concern_for_ParameterIdentificationLinkedParametersPresenter
   {
      protected override void Because()
      {
         sut.ClearSelection();
      }

      [Observation]
      public void should_also_clear_the_view()
      {
         _allLinkedParameterDTO.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_the_linked_parameter_presenter_is_told_to_add_some_parameters_as_linked_parameter_ : concern_for_ParameterIdentificationLinkedParametersPresenter
   {
      private List<ParameterSelection> _parameterSelections;
      private ParameterSelection _newParameterSelection;
      private LinkedParameterDTO _newLinkedParameterDTO;

      protected override void Context()
      {
         base.Context();
         _newParameterSelection = A.Fake<ParameterSelection>();
         A.CallTo(() => _newParameterSelection.Dimension).Returns(_linkedParameter.Dimension);
         _parameterSelections = new List<ParameterSelection> {_newParameterSelection};
         _newLinkedParameterDTO = A.Fake<LinkedParameterDTO>();
         A.CallTo(() => _linkedParameterDTOMapper.MapFrom(_newParameterSelection)).Returns(_newLinkedParameterDTO);
      }

      protected override void Because()
      {
         sut.AddLinkedParameters(_parameterSelections);
      }

      [Observation]
      public void should_add_those_parameters_to_the_edited_identification_parameter()
      {
         _identificationParameter.AllLinkedParameters.Select(x=>x.Parameter).ShouldContain(_newParameterSelection.Parameter);
      }

      [Observation]
      public void should_refresh_the_view()
      {
         _allLinkedParameterDTO.ShouldContain(_newLinkedParameterDTO);
      }
   }
}