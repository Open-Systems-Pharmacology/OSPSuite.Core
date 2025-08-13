using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class concern_for_DimensionMappingPresenter : ContextSpecification<DimensionMappingPresenter>
   {
      protected IDimensionMappingView _view;

      protected override void Context()
      {
         _view = A.Fake<IDimensionMappingView>();
         sut = new DimensionMappingPresenter(_view);
      }
   }

   public class When_mapping_ambiguous_dimensions_and_the_view_is_canceled : concern_for_DimensionMappingPresenter
   {
      private List<DimensionSelectionDTO> _dimensionDTOs;
      private IReadOnlyList<IDimension> _possibleDimensions;
      private Column _column;
      private IDimension _pickedDimension;

      protected override void Context()
      {
         base.Context();
         _pickedDimension = new Dimension(new BaseDimensionRepresentation(), "Mass", "mg");
         _possibleDimensions = new List<IDimension>();
         _column = new Column();
         _dimensionDTOs = new List<DimensionSelectionDTO> { new DimensionSelectionDTO("sheet", new[] { string.Empty }, _column, _possibleDimensions) };

         A.CallTo(() => _view.BindTo(_dimensionDTOs)).Invokes(x => _dimensionDTOs.Single().SelectedDimension = _pickedDimension);

         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         sut.EditUnitToDimensionMap(_dimensionDTOs);
      }

      [Observation]
      public void the_column_mapping_is_unset()
      {
         _dimensionDTOs.Single().SelectedDimension.ShouldBeNull();
      }
   }

   public class When_mapping_ambiguous_dimensions_and_the_view_is_not_canceled : concern_for_DimensionMappingPresenter
   {
      private List<DimensionSelectionDTO> _dimensionDTOs;
      private IReadOnlyList<IDimension> _possibleDimensions;
      private Column _column;
      private IDimension _pickedDimension;

      protected override void Context()
      {
         base.Context();
         _pickedDimension = new Dimension(new BaseDimensionRepresentation(), "Mass", "mg");
         _possibleDimensions = new List<IDimension>();
         _column = new Column();
         _dimensionDTOs = new List<DimensionSelectionDTO> { new DimensionSelectionDTO("sheet", new [] {string.Empty}, _column, _possibleDimensions) };

         A.CallTo(() => _view.BindTo(_dimensionDTOs)).Invokes(x => _dimensionDTOs.Single().SelectedDimension = _pickedDimension);
         A.CallTo(() => _view.Canceled).Returns(false);
      }

      protected override void Because()
      {
         sut.EditUnitToDimensionMap(_dimensionDTOs);
      }

      [Observation]
      public void the_column_mapping_is_set()
      {
         _dimensionDTOs.Single().SelectedDimension.ShouldBeEqualTo(_pickedDimension);
      }
   }

   public class When_applying_all_mappings_to_matching_ambiguous_columns : concern_for_DimensionMappingPresenter
   {
      
      private List<IDimension> _possibleDimensions;
      private Column _column;
      private List<DimensionSelectionDTO> _dimensionDTOs;
      private DimensionSelectionDTO _dimensionSelect1;
      private DimensionSelectionDTO _dimensionSelect2;
      private DimensionSelectionDTO _pickedDimensionDTO;
      private IDimension _massDimension;
      private DimensionSelectionDTO _dimensionSelect3;
      private List<IDimension> _otherPossibleDimensions;
      private Dimension _doseDimension;

      protected override void Context()
      {
         base.Context();
         _massDimension = new Dimension(new BaseDimensionRepresentation(), "Mass", "mg");
         _doseDimension = new Dimension(new BaseDimensionRepresentation(), "Dose", "mg");

         _possibleDimensions = new List<IDimension>
         {
            _massDimension,
            _doseDimension
         };

         _otherPossibleDimensions = new List<IDimension> { _doseDimension, _massDimension, new Dimension(new BaseDimensionRepresentation(), "Time", "h") };
         _column = new Column();
         
         _dimensionSelect1 = new DimensionSelectionDTO("sheet", new[] { string.Empty }, _column, _possibleDimensions);
         _dimensionSelect2 = new DimensionSelectionDTO("sheet", new[] { string.Empty }, _column, _possibleDimensions);
         _dimensionSelect3 = new DimensionSelectionDTO("sheet", new[] { string.Empty }, _column, _otherPossibleDimensions);
         _dimensionDTOs = new List<DimensionSelectionDTO> { _dimensionSelect1, _dimensionSelect2, _dimensionSelect3 };
         _pickedDimensionDTO = _dimensionSelect1;

         A.CallTo(() => _view.BindTo(_dimensionDTOs)).Invokes(x => _dimensionSelect1.SelectedDimension = _massDimension);
         A.CallTo(() => _view.Canceled).Returns(false);

         sut.EditUnitToDimensionMap(_dimensionDTOs);
      }

      protected override void Because()
      {
         sut.ApplyToAllMatching(_pickedDimensionDTO);
      }

      [Observation]
      public void all_matching_columns_were_updated()
      {
         _dimensionSelect1.SelectedDimension.ShouldBeEqualTo(_massDimension);
         _dimensionSelect2.SelectedDimension.ShouldBeEqualTo(_massDimension);
      }

      [Observation]
      public void not_matching_columns_were_not_updated()
      {
         _dimensionSelect3.SelectedDimension.ShouldBeEqualTo(null);
      }

      [Observation]
      public void the_view_cannot_close_with_unmapped_dimension()
      {
         sut.CanClose.ShouldBeFalse();
      }
   }
}