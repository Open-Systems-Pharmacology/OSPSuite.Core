using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_DiffItemToDiffItemDTOMapper : ContextSpecification<IDiffItemToDiffItemDTOMapper>
   {
      private IPathToPathElementsMapper _pathToPathElementMapper;
      protected DiffItem _diffItem;
      protected DiffItemDTO _dto;
      protected IEntity _container;
      protected PathElements _pathElements;
      protected IDisplayNameProvider _displayNameProvider;

      protected override void Context()
      {
         _pathToPathElementMapper = A.Fake<IPathToPathElementsMapper>();
         _displayNameProvider = A.Fake<IDisplayNameProvider>();
         _container = new Container().WithName("ROOT");
         _pathElements = new PathElements();
         sut = new DiffItemToDiffItemDTOMapper(_pathToPathElementMapper, _displayNameProvider);

         A.CallTo(() => _pathToPathElementMapper.MapFrom(_container)).Returns(_pathElements);
         A.CallTo(() => _displayNameProvider.DisplayNameFor(A<object>._)).ReturnsLazily(x =>
         {
            var obj = x.GetArgument<object>(0);
            return obj != null ? obj.ToString() : string.Empty;
         });
      }

      protected override void Because()
      {
         _dto = sut.MapFrom(_diffItem);
      }
   }

   public class When_mapping_a_property_diff_item_for_an_entity_to_a_diff_item_dto : concern_for_DiffItemToDiffItemDTOMapper
   {
      protected override void Context()
      {
         base.Context();
         _diffItem = new PropertyValueDiffItem
         {
            Object1 = new Parameter().WithName("P1"),
            Object2 = new Parameter().WithName("P1"),
            FormattedValue1 = "xx",
            FormattedValue2 = "yy",
            CommonAncestor = _container,
            PropertyName = "ABC"
         };
      }

      [Observation]
      public void should_return_a_dto_having_the_expected_left_and_right_object()
      {
         _dto.PathElements.ShouldBeEqualTo(_pathElements);
         _dto.Property.ShouldBeEqualTo("ABC");
         _dto.ObjectName.ShouldBeEqualTo("P1");
         _dto.Value1.ShouldBeEqualTo("xx");
         _dto.Value2.ShouldBeEqualTo("yy");
      }

      [Observation]
      public void should_return_a_dto_having_the_missing_item_flag_to_false()
      {
         _dto.ItemIsMissing.ShouldBeFalse();
      }

      [Observation]
      public void should_have_set_the_path_element_of_the_common_ancestor()
      {
         _dto.PathElements.ShouldBeEqualTo(_pathElements);
      }
   }

   public class When_mapping_a_property_diff_item_for_a_formula_to_a_diff_item_dto : concern_for_DiffItemToDiffItemDTOMapper
   {
      protected override void Context()
      {
         base.Context();
         _diffItem = new PropertyValueDiffItem
         {
            Object1 = new ConstantFormula().WithName("F"),
            Object2 = new ConstantFormula().WithName("F"),
            CommonAncestor = _container,
         };
      }

      [Observation]
      public void should_use_the_name_of_the_parent_container_instead_of_the_formula_name()
      {
         _dto.ObjectName.ShouldBeEqualTo(_container.Name);
      }
   }

   public class When_mapping_a_property_diff_item_for_a_reaction_partner_to_a_diff_item_dto : concern_for_DiffItemToDiffItemDTOMapper
   {
      protected override void Context()
      {
         base.Context();
         _diffItem = new PropertyValueDiffItem
         {
            Object1 = new ReactionPartner(2, new MoleculeAmount().WithName("A")),
            Object2 = new ReactionPartner(1, new MoleculeAmount().WithName("A")),
            CommonAncestor = _container,
         };
      }

      [Observation]
      public void should_use_the_name_of_the_partner_molecule()
      {
         _dto.ObjectName.ShouldBeEqualTo("A");
      }
   }

   public class When_mapping_a_property_diff_item_for_a_reaction_partner_builder_to_a_diff_item_dto : concern_for_DiffItemToDiffItemDTOMapper
   {
      protected override void Context()
      {
         base.Context();
         _diffItem = new PropertyValueDiffItem
         {
            Object1 = new ReactionPartnerBuilder("A", 2),
            Object2 = new ReactionPartnerBuilder("A", 1),
            CommonAncestor = _container,
         };
      }

      [Observation]
      public void should_use_the_name_of_the_partner()
      {
         _dto.ObjectName.ShouldBeEqualTo("A");
      }
   }

   public class When_mapping_a_property_diff_item_for_a_calculation_method_to_a_diff_item_dto : concern_for_DiffItemToDiffItemDTOMapper
   {
      protected override void Context()
      {
         base.Context();
         _diffItem = new PropertyValueDiffItem
         {
            Object1 = new CalculationMethod {Category = "Cat", Name = "PKSim"},
            Object2 = new CalculationMethod {Category = "Cat", Name = "RR"},
            CommonAncestor = _container,
         };
      }

      [Observation]
      public void should_use_the_name_of_the_category()
      {
         _dto.ObjectName.ShouldBeEqualTo("Cat");
      }
   }

   public class When_mapping_a_property_diff_item_for_an_object_path_to_a_diff_item_dto : concern_for_DiffItemToDiffItemDTOMapper
   {
      protected override void Context()
      {
         base.Context();
         _diffItem = new PropertyValueDiffItem
         {
            Object1 = new ObjectPath(),
            Object2 = new ObjectPath(),
            CommonAncestor = _container,
         };
      }

      [Observation]
      public void should_use_the_name_of_the_partner()
      {
         _dto.ObjectName.ShouldBeEqualTo(_container.Name);
      }
   }

   public class When_mapping_a_missing_left_item_to_a_diff_item_dto : concern_for_DiffItemToDiffItemDTOMapper
   {
      protected override void Context()
      {
         base.Context();
         _diffItem = new MissingDiffItem
         {
            MissingObject1 = new Parameter().WithName("P1"),
            MissingObject2 = null,
            MissingObjectName = "A"
         };
      }

      [Observation]
      public void should_return_a_dto_having_the_expected_left_and_right_object()
      {
         _dto.PathElements.ToList().ShouldBeEqualTo(_pathElements.ToList());
         _dto.Value1.ShouldBeEqualTo(Captions.Comparisons.Present);
         _dto.Value2.ShouldBeEqualTo(Captions.Comparisons.Absent);
         _dto.ObjectName.ShouldBeEqualTo("A");
      }

      [Observation]
      public void should_return_a_dto_having_the_missing_item_flag_to_true()
      {
         _dto.ItemIsMissing.ShouldBeTrue();
      }
   }

   public class When_mapping_a_missing_left_item_to_a_diff_item_dto_with_present_object_details : concern_for_DiffItemToDiffItemDTOMapper
   {
      protected override void Context()
      {
         base.Context();
         _diffItem = new MissingDiffItem
         {
            MissingObject1 = new Parameter().WithName("P1"),
            MissingObject2 = null,
            MissingObjectName = "A",
            PresentObjectDetails = "5L"
         };
      }

      [Observation]
      public void should_return_a_dto_having_the_expected_left_and_right_object_including_present_object_details()
      {
         _dto.PathElements.ToList().ShouldBeEqualTo(_pathElements.ToList());
         _dto.Value1.Contains(Captions.Comparisons.Present).ShouldBeTrue();
         _dto.Value1.Contains("5L").ShouldBeTrue();
         _dto.Value2.ShouldBeEqualTo(Captions.Comparisons.Absent);
         _dto.ObjectName.ShouldBeEqualTo("A");
      }

      [Observation]
      public void should_return_a_dto_having_the_missing_item_flag_to_true()
      {
         _dto.ItemIsMissing.ShouldBeTrue();
      }
   }

   public class When_mapping_a_missing_right_item_to_a_diff_item_dto : concern_for_DiffItemToDiffItemDTOMapper
   {
      protected override void Context()
      {
         base.Context();
         _diffItem = new MissingDiffItem
         {
            MissingObject1 = null,
            MissingObject2 = new Parameter().WithName("P1"),
            MissingObjectName = "A"
         };
      }

      [Observation]
      public void should_return_a_dto_having_the_expected_left_and_right_object()
      {
         _dto.PathElements.ToList().ShouldBeEqualTo(_pathElements.ToList());
         _dto.Value1.ShouldBeEqualTo(Captions.Comparisons.Absent);
         _dto.Value2.ShouldBeEqualTo(Captions.Comparisons.Present);
         _dto.ObjectName.ShouldBeEqualTo("A");
      }

      [Observation]
      public void should_return_a_dto_having_the_missing_item_flag_to_true()
      {
         _dto.ItemIsMissing.ShouldBeTrue();
      }
   }

   public class When_mapping_an_type_mismatch_diff_item : concern_for_DiffItemToDiffItemDTOMapper
   {
      protected override void Context()
      {
         base.Context();
         _diffItem = new MismatchDiffItem()
         {
            Object1 = new Container().WithName("P1"),
            Object2 = new Parameter().WithName("P1"),
            Description = "ABC",
            CommonAncestor = new Container().WithName("X")
         };
      }

      [Observation]
      public void should_return_a_dto_having_the_description_set_as_expected()
      {
         _dto.Description.ShouldBeEqualTo("ABC");
         _dto.ObjectName.ShouldBeEqualTo("X");
      }
   }

   public class When_mapping_a_diff_item_with_non_entity_ancestor : concern_for_DiffItemToDiffItemDTOMapper
   {
      protected override void Context()
      {
         base.Context();
         _diffItem = new MismatchDiffItem()
         {
            Object1 = new Container().WithName("P1"),
            Object2 = new Parameter().WithName("P1"),
            Description = "ABC",
            CommonAncestor = new MoleculeBuildingBlock().WithName("X")
         };
      }

      [Observation]
      public void should_return_a_dto_with_empty_path()
      {
         _dto.ShouldNotBeNull();
         _dto.PathElements.ShouldBeEmpty();
      }
   }

   public class When_creating_the_diff_item_dto_corresponding_to_the_comparison_of_two_object_with_different_types : concern_for_DiffItemToDiffItemDTOMapper
   {
      protected override void Context()
      {
         base.Context();
         _diffItem = new MismatchDiffItem()
         {
            Object1 = new Container().WithName("P1"),
            Object2 = new Parameter().WithName("P1"),
            Description = "ABC",
            CommonAncestor = null
         };
      }

      [Observation]
      public void should_return_a_dto_with_empty_path()
      {
         _dto.ShouldNotBeNull();
         _dto.PathElements.ShouldBeEmpty();
      }
   }
}