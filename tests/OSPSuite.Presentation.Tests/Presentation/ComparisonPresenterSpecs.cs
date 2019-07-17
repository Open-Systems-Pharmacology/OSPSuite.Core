using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.Comparisons;
using OSPSuite.Presentation.Views.Comparisons;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ComparisonPresenter : ContextSpecification<IComparisonPresenter>
   {
      protected IDiffItemToDiffItemDTOMapper _diffItemDTOMapper;
      protected IComparisonView _view;
      protected IObjectComparer _objectComparer;
      protected IObjectBase _object1;
      protected IObjectBase _object2;
      protected string _caption1 = "AA";
      protected string _caption2 = "BB";
      protected ComparerSettings _settings;
      protected DiffReport _report;
      private IDiffItemDTOsToDataTableMapper _dataTableMapper;

      protected override void Context()
      {
         _view = A.Fake<IComparisonView>();
         _diffItemDTOMapper = A.Fake<IDiffItemToDiffItemDTOMapper>();
         _objectComparer = A.Fake<IObjectComparer>();
         _dataTableMapper = A.Fake<IDiffItemDTOsToDataTableMapper>();
         sut = new ComparisonPresenter(_view, _objectComparer, _diffItemDTOMapper, _dataTableMapper);

         _settings = new ComparerSettings();
         _object1 = A.Fake<IObjectBase>();
         _object2 = A.Fake<IObjectBase>();
         _report = new DiffReport();

         A.CallTo(() => _objectComparer.Compare(_object1, _object2, _settings)).Returns(_report);
      }
   }

   public class When_starting_the_comparison_between_two_objects : concern_for_ComparisonPresenter
   {
      private DiffItem _diffItem1;
      private DiffItem _diffItem2;
      private DiffItemDTO _dto1;
      private DiffItemDTO _dto2;
      private List<DiffItemDTO> _allDiffItemDTO;

      protected override void Context()
      {
         base.Context();
         _diffItem1 = new PropertyValueDiffItem();
         _diffItem2 = new PropertyValueDiffItem();
         _dto1 = new DiffItemDTO();
         _dto2 = new DiffItemDTO();

         _dto1.PathElements[PathElement.Name] = new PathElementDTO {DisplayName = "A"};
         _dto2.PathElements[PathElement.Name] = new PathElementDTO { DisplayName = "B" };
         
         _dto1.PathElements[PathElement.Molecule] = new PathElementDTO {DisplayName = "Mol"};
         _dto2.PathElements[PathElement.Molecule] = new PathElementDTO {DisplayName = "Mol2"};

         _dto1.PathElements[PathElement.TopContainer] = new PathElementDTO {DisplayName = "A"};
         _dto2.PathElements[PathElement.TopContainer] = new PathElementDTO {DisplayName = "A"};

         _dto1.PathElements[PathElement.BottomCompartment] = new PathElementDTO { DisplayName = "" };
         _dto2.PathElements[PathElement.BottomCompartment] = new PathElementDTO { DisplayName = "" };


         _report.Add(_diffItem1);
         _report.Add(_diffItem2);
         A.CallTo(() => _diffItemDTOMapper.MapFrom(_diffItem1)).Returns(_dto1);
         A.CallTo(() => _diffItemDTOMapper.MapFrom(_diffItem2)).Returns(_dto2);

         A.CallTo(() => _view.BindTo(A<IEnumerable<DiffItemDTO>>._))
            .Invokes(x => _allDiffItemDTO = x.GetArgument<IEnumerable<DiffItemDTO>>(0).ToList());
      }

      protected override void Because()
      {
         sut.StartComparison(_object1, _object2, _caption1, _caption2, _settings);
      }

      [Observation]
      public void should_leverage_the_object_comparer_to_start_the_comparison()
      {
         A.CallTo(() => _objectComparer.Compare(_object1, _object2, _settings)).MustHaveHappened();
      }

      [Observation]
      public void should_display_the_comparison_results_in_the_view()
      {
         _allDiffItemDTO.ShouldOnlyContain(_dto1, _dto2);
      }

      [Observation]
      public void should_set_the_left_and_right_caption_int_the_view()
      {
         _view.LeftCaption.ShouldBeEqualTo(_caption1);
         _view.RightCaption.ShouldBeEqualTo(_caption2);
      }

      [Observation]
      public void should_hide_columns_in_the_view_with_only_empty_value()
      {
         A.CallTo(() => _view.SetVisibility(PathElement.BottomCompartment, false)).MustHaveHappened();
         A.CallTo(() => _view.SetVisibility(PathElement.Simulation, false)).MustHaveHappened();
      }

      [Observation]
      public void should_show_columns_in_the_view_with_different_values()
      {
         A.CallTo(() => _view.SetVisibility(PathElement.TopContainer, true)).MustHaveHappened();
         A.CallTo(() => _view.SetVisibility(PathElement.Molecule, true)).MustHaveHappened();
      }

      [Observation]
      public void should_always_hide_the_column_name_as_it_is_redundant_with_the_column_object_name()
      {
         A.CallTo(() => _view.SetVisibility(PathElement.Name, false)).MustHaveHappened();
      }
   }
}