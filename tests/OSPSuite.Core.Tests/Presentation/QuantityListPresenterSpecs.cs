using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_QuantityListPresenter : ContextSpecification<IQuantityListPresenter>
   {
      protected IQuantityToQuantitySelectionDTOMapper _mapper;
      protected IQuantityListView _view;

      protected override void Context()
      {
         _mapper = A.Fake<IQuantityToQuantitySelectionDTOMapper>();
         _view = A.Fake<IQuantityListView>();
         sut = new QuantityListPresenter(_view, _mapper);
      }
   }

   public class When_the_quantity_list_presenter_is_editing_a_list_of_quanties : concern_for_QuantityListPresenter
   {
      private List<IQuantity> _quantities;
      private IQuantity _q1;
      private IQuantity _q2;
      private readonly QuantitySelectionDTO _dto1 = new QuantitySelectionDTO {QuantityPath = "Path1"};
      private readonly QuantitySelectionDTO _dto2 = new QuantitySelectionDTO {QuantityPath = "Path2"};
      private List<QuantitySelectionDTO> _dtos;

      protected override void Context()
      {
         base.Context();
         _q1 = A.Fake<IQuantity>();
         _q2 = A.Fake<IQuantity>();
         _quantities = new List<IQuantity> {_q1, _q2};
         A.CallTo(() => _mapper.MapFrom(_q1, A<int>._)).Returns(_dto1);
         A.CallTo(() => _mapper.MapFrom(_q2, A<int>._)).Returns(_dto2);
         A.CallTo(() => _view.BindTo(A<IEnumerable<QuantitySelectionDTO>>._))
            .Invokes(x => _dtos = x.GetArgument<IEnumerable<QuantitySelectionDTO>>(0).ToList());
      }

      protected override void Because()
      {
         sut.Edit(_quantities);
      }

      [Observation]
      public void should_display_the_quantities_into_the_view()
      {
         _dtos.ShouldOnlyContain(_dto1, _dto2);
      }
   }

   public class When_checking_if_a_path_column_is_empty: concern_for_QuantityListPresenter
   {

      protected override void Context()
      {
         base.Context();
         var  q1 = A.Fake<IQuantity>();
         var q2 = A.Fake<IQuantity>();
         var dto1  = new QuantitySelectionDTO { QuantityPath = "A|B|C" };
         dto1.PathElements[PathElement.Simulation] = new PathElementDTO();
         dto1.PathElements[PathElement.TopContainer] = new PathElementDTO();
         dto1.PathElements[PathElement.Container] = new PathElementDTO();
         dto1.SimulationPathElement.DisplayName = "A";
         dto1.TopContainerPathElement.DisplayName = "B";
         dto1.ContainerPathElement.DisplayName = "C";
         var dto2  = new QuantitySelectionDTO { QuantityPath = "A|D" };
         dto2.PathElements[PathElement.Simulation] = new PathElementDTO();
         dto2.PathElements[PathElement.TopContainer] = new PathElementDTO();
         dto2.SimulationPathElement.DisplayName = "A";
         dto2.TopContainerPathElement.DisplayName = "D";
         A.CallTo(() => _mapper.MapFrom(q1, A<int>._)).Returns(dto1);
         A.CallTo(() => _mapper.MapFrom(q2, A<int>._)).Returns(dto2);

         sut.Edit(new List<IQuantity> { q1, q2 });
      }

      [Observation]
      public void should_return_true_if_the_column_only_contains_empty_string()
      {
         sut.PathElementIsEmpty(PathElement.Molecule).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_column_only_contains_one_non_empty_string()
      {
         sut.PathElementIsEmpty(PathElement.Container).ShouldBeFalse();

      }

      [Observation]
      public void should_return_false_if_the_column_contains_more_than_one_value()
      {
         sut.PathElementIsEmpty(PathElement.TopContainer).ShouldBeFalse();
      }
   }

   public class When_notify_that_a_quantity_was_selected : concern_for_QuantityListPresenter
   {
      private QuantitySelectionDTO _dto;
      private QuantitySelectionChangedEventArgs _e;

      protected override void Context()
      {
         base.Context();
         _dto = new QuantitySelectionDTO {Selected = true};
         sut.SelectionChanged += (o, e) => { _e = e; };
      }

      protected override void Because()
      {
         sut.UpdateSelection(_dto);
      }

      [Observation]
      public void should_raise_the_selection_changed_event()
      {
         _e.Selected.ShouldBeTrue();
         _e.QuantitySelections.ShouldContain(_dto);
      }
   }

   public class When_asked_for_a_quantity_selection_by_path_that_does_not_exist : concern_for_QuantityListPresenter
   {
      [Observation]
      public void should_return_null()
      {
         sut.QuantityDTOByPath("TOTO").ShouldBeNull();
      }
   }
}