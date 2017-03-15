using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Converter.v5_2;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Converter.v5_2
{
   public abstract class concern_for_DataRepositoryConverter : ContextSpecification<IDataRepositoryConverter>
   {
      private IUsingDimensionConverter _usingDimensionConverter;
      protected IDimensionMapper _dimensionMapper;

      protected override void Context()
      {
         _usingDimensionConverter = A.Fake<IUsingDimensionConverter>();
         _dimensionMapper = A.Fake<IDimensionMapper>();
         sut = new DataRepositoryConverter(_usingDimensionConverter, _dimensionMapper);
      }
   }

   public class When_converting_a_data_repository_whose_columns_need_conversion : concern_for_DataRepositoryConverter
   {
      private DataRepository _dataRepository;
      private BaseGrid _baseGrid;
      private DataColumn _column;

      protected override void Context()
      {
         base.Context();
         _baseGrid = new BaseGrid("Time", A.Fake<IDimension>()) {Values = new float[] {1, 2, 3, 4}};
         _column = new DataColumn("Col", A.Fake<IDimension>(), _baseGrid) {Values = new float[] {10, 20, 30, 40}};
         A.CallTo(() => _dimensionMapper.ConversionFactor("MolecularWeight")).Returns(1e-9);
         _column.DataInfo.MolWeight = 250;
         _dataRepository = new DataRepository();
         A.CallTo(() => _dimensionMapper.ConversionFactor(_column)).Returns(10);
         A.CallTo(() => _dimensionMapper.ConversionFactor(_baseGrid)).Returns(1);
         _dataRepository.Add(_baseGrid);
         _dataRepository.Add(_column);
      }

      protected override void Because()
      {
         sut.Convert(_dataRepository);
      }

      [Observation]
      public void should_convert_the_values_in_each_column_that_requires_conversion()
      {
         _column.Values.ShouldOnlyContain(100,200,300,400);
      }

      [Observation]
      public void should_have_converted_the_mol_weight_if_available()
      {
         _column.DataInfo.MolWeight.ShouldBeEqualTo(250*1e-9);
         }
   }

   public class When_converting_a_data_repository_which_has_extended_property_molWeight_columns_need_conversion : concern_for_DataRepositoryConverter
   {
      private DataRepository _dataRepository;
      private BaseGrid _baseGrid;
      private DataColumn _column;
      private ExtendedProperty<double> _molWeight;

      protected override void Context()
      {
         base.Context();
         _baseGrid = new BaseGrid("Time", A.Fake<IDimension>()) { Values = new float[] { 1, 2, 3, 4 } };
         _column = new DataColumn("Col", A.Fake<IDimension>(), _baseGrid) { Values = new float[] { 10, 20, 30, 40 } };
         A.CallTo(() => _dimensionMapper.ConversionFactor("MolecularWeight")).Returns(1e-9);
         _column.DataInfo.MolWeight = 250;
         _dataRepository = new DataRepository();
         A.CallTo(() => _dimensionMapper.ConversionFactor(_column)).Returns(10);
         A.CallTo(() => _dimensionMapper.ConversionFactor(_baseGrid)).Returns(1);
         _dataRepository.Add(_baseGrid);
         _dataRepository.Add(_column);
         _molWeight = new ExtendedProperty<double> { Value = 400, Name = Constants.MOL_WEIGHT_EXTENDED_PROPERTY };
         _dataRepository.ExtendedProperties.Add(_molWeight);
      }

      protected override void Because()
      {
         sut.Convert(_dataRepository);
      }

      [Observation]
      public void should_convert_extended_Property()
      {
         _molWeight.Value.ShouldBeEqualTo(400 *1e-9);
      }
   }
}