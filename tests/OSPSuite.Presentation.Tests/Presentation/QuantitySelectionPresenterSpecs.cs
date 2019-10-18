using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_QuantitySelectionPresenter : ContextSpecification<IQuantitySelectionPresenter>
   {
      protected IQuantitySelectionView _view;
      protected IContainer _simulation;
      protected List<IQuantity> _quantities;
      protected List<QuantitySelection> _selectedQuantities;
      protected IQuantityListPresenter _selectedQuantityPresenter;
      protected IQuantityListPresenter _allQuantityPresenter;

      protected override void Context()
      {
         _view = A.Fake<IQuantitySelectionView>();
         _selectedQuantityPresenter = A.Fake<IQuantityListPresenter>();
         _allQuantityPresenter = A.Fake<IQuantityListPresenter>();
         sut = new QuantitySelectionPresenter(_view, _allQuantityPresenter, _selectedQuantityPresenter);

         _quantities = new List<IQuantity>();
         _simulation = A.Fake<IContainer>();
         A.CallTo(() => _simulation.GetAllChildren<IQuantity>()).Returns(_quantities);
         _selectedQuantities = new List<QuantitySelection>();
      }
   }

   public class When_the_simulation_quantity_selection_presenter_is_editing_the_settings_based_on_the_available_molecules_in_a_simulation : concern_for_QuantitySelectionPresenter
   {
      private List<IQuantity> _allQuantities;
      private IObserver _observer1;
      private IObserver _observer2;
      private IObserver _observer3;
      private QuantitySelectionDTO _dto;
      private List<QuantitySelectionDTO> _selectedQuantitiesDTO;

      protected override void Context()
      {
         base.Context();
         _observer1 = A.Fake<IObserver>();
         _observer1.Persistable = true;
         _observer2 = A.Fake<IObserver>();
         _observer2.Persistable = true;
         _observer3 = A.Fake<IObserver>();
         _observer3.Persistable = false;
         _dto = new QuantitySelectionDTO();
         _quantities.AddRange(new[] {_observer1, _observer2, _observer3});
         _selectedQuantities.Add(new QuantitySelection("observer2Path", QuantityType.Drug));
         _selectedQuantities.Add(new QuantitySelection("toto", QuantityType.Drug));

         A.CallTo(() => _allQuantityPresenter.Edit(A<IEnumerable<IQuantity>>._))
            .Invokes(x => _allQuantities = x.GetArgument<IEnumerable<IQuantity>>(0).ToList());

         A.CallTo(() => _selectedQuantityPresenter.Edit(A<IEnumerable<QuantitySelectionDTO>>._))
            .Invokes(x => _selectedQuantitiesDTO = x.GetArgument<IEnumerable<QuantitySelectionDTO>>(0).ToList());

         A.CallTo(() => _allQuantityPresenter.QuantityDTOByPath("observer2Path")).Returns(_dto);
         A.CallTo(() => _allQuantityPresenter.QuantityDTOByPath("toto")).Returns(null);
      }

      protected override void Because()
      {
         sut.Edit(_simulation, _selectedQuantities);
      }

      [Observation]
      public void should_add_all_entities_to_the_list_of_possible_quantity_to_select()
      {
         _allQuantities.ShouldOnlyContain(_observer1, _observer2);
      }

      [Observation]
      public void should_have_added_the_selected_item_to_the_selected_presenter()
      {
         _dto.Selected.ShouldBeTrue();
         _selectedQuantitiesDTO.ShouldContain(_dto);
      }
   }

   public class When_the_simulation_quantity_selection_presenter_is_being_notifed_that_the_selection_has_changed : concern_for_QuantitySelectionPresenter
   {
      private QuantitySelectionDTO _quantityDTO1;
      private QuantitySelectionDTO _quantityDTO2;
      private readonly List<QuantitySelectionDTO> _selectionDTO = new List<QuantitySelectionDTO>();

      protected override void Context()
      {
         base.Context();

         //select 2 items
         _quantityDTO1 = new QuantitySelectionDTO {QuantityPath = "toto", Selected = true};
         _quantityDTO2 = new QuantitySelectionDTO {QuantityPath = "tata", Selected = false};

         _selectionDTO.AddRange(new[] {_quantityDTO1, _quantityDTO2});

         sut.Edit(_simulation, _selectedQuantities);
      }

      protected override void Because()
      {
         _allQuantityPresenter.SelectionChanged += Raise.With(new QuantitySelectionChangedEventArgs(_selectionDTO, true));
      }

      [Observation]
      public void it_should_add_the_quantities_to_the_selection()
      {
         A.CallTo(() => _selectedQuantityPresenter.Add(_quantityDTO1)).MustHaveHappened();
         A.CallTo(() => _selectedQuantityPresenter.Add(_quantityDTO2)).MustHaveHappened();
      }
   }

   public class When_the_user_decides_to_deselect_all_selected_observed_data : concern_for_QuantitySelectionPresenter
   {
      private QuantitySelectionDTO _quantityDTO1;
      private readonly List<QuantitySelectionDTO> _selectedDTOs = new List<QuantitySelectionDTO>();
      private IReadOnlyList<QuantitySelectionDTO> _updatedDto;

      protected override void Context()
      {
         base.Context();
         _quantityDTO1 = new QuantitySelectionDTO {QuantityPath = "toto", Selected = true};
         _selectedDTOs.Add(_quantityDTO1);
         A.CallTo(() => _selectedQuantityPresenter.AllQuantityDTOs).Returns(_selectedDTOs);
         sut.Edit(_simulation, _selectedQuantities);

         A.CallTo(() => _selectedQuantityPresenter.UpdateSelection(A<IReadOnlyList<QuantitySelectionDTO>>._, false)).Invokes(x => _updatedDto = x.GetArgument<IReadOnlyList<QuantitySelectionDTO>>(0));
      }

      protected override void Because()
      {
         sut.DeselectAll();
      }

      [Observation]
      public void should_deselect_all_previously_selected_items()
      {
         _updatedDto.ShouldOnlyContain(_quantityDTO1);
      }
   }

   public class When_told_that_all_groups_should_be_expanded : concern_for_QuantitySelectionPresenter
   {
      protected override void Because()
      {
         sut.ExpandAllGroups = true;
      }

      [Observation]
      public void should_expand_the_groups_in_all_sub_presenters()
      {
         _allQuantityPresenter.ExpandAllGroups.ShouldBeTrue();
      }
   }
}