using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Importer.Services
{
   public class concern_for_DataSourceToDimensionSelectionDTOMapper : ContextSpecification<DataSourceToDimensionSelectionDTOMapper>
   {
      protected IDataSource _dataSource;

      protected override void Context()
      {
         _dataSource = A.Fake<IDataSource>();
         sut = new DataSourceToDimensionSelectionDTOMapper();
      }
   }

   public class When_mapping_dimensions_from_columns : concern_for_DataSourceToDimensionSelectionDTOMapper
   {
      private IReadOnlyList<DimensionSelectionDTO> _dtos;
      private Cache<string, IDataSet> _datasets;
      private List<ExtendedColumn> _columns;
      private ParsedDataSet _parsedData;

      protected override void Context()
      {
         base.Context();
         _columns = new List<ExtendedColumn>
         {
            new ExtendedColumn { ColumnInfo = new ColumnInfo {SupportedDimensions = { Constants.Dimension.NO_DIMENSION }}, Column = new Column() },
            new ExtendedColumn { ColumnInfo = new ColumnInfo {SupportedDimensions = { Constants.Dimension.NO_DIMENSION }}, Column = new Column {ErrorStdDev = Constants.STD_DEV_GEOMETRIC} }
         };
         var dataSet = A.Fake<IDataSet>();
         _datasets = new Cache<string, IDataSet>
         {
            { "sheet", dataSet }
         };
         var dictionary = new Dictionary<ExtendedColumn, IList<SimulationPoint>>
         {
            { _columns[0], new List<SimulationPoint>() },
            { _columns[1], new List<SimulationPoint>() }
         };

         _parsedData = new ParsedDataSet(new List<string>(), new DataSheet(), new List<UnformattedRow>(), dictionary);
         A.CallTo(() => _dataSource.DataSets).Returns(_datasets);
         A.CallTo(() => dataSet.Data).Returns(new List<ParsedDataSet> { _parsedData });
      }

      protected override void Because()
      {
         _dtos = sut.MapFrom(_dataSource);
      }

      [Observation]
      public void the_dimension_for_geo_std_should_be_no_dimension()
      {
         _dtos.Where(x => x.Column.ErrorStdDev == Constants.STD_DEV_GEOMETRIC).Each(x => x.SelectedDimension.ShouldBeEqualTo(Constants.Dimension.NO_DIMENSION));
         _dtos.Count(x => x.Column.ErrorStdDev == Constants.STD_DEV_GEOMETRIC).ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_dimension_map_should_contain_the_columns_and_dimensions()
      {
         _dtos[0].SelectedDimension.ShouldBeEqualTo(Constants.Dimension.NO_DIMENSION);
      }

      [Observation]
      public void should_create_dto_for_columns()
      {
         _dtos.Count.ShouldBeEqualTo(_columns.Count);
      }
   }
}
