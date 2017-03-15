using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Converter.v5_4;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core
{
   public abstract class concern_for_DataRepositoryConverter : ContextSpecification<IDataRepositoryConverter>
   {
      protected override void Context()
      {
         sut = new DataRepositoryConverter();
      }
   }

   public class When_converting_a_data_repository_to_version_5_3 : concern_for_DataRepositoryConverter
   {
      private DataRepository _dataRepository;
      private DataColumn _col1;
      private DataColumn _col2;

      protected override void Context()
      {
         base.Context();
         var baseGrid = new BaseGrid("baseGrid", "baseGrid", Constants.Dimension.NO_DIMENSION);
         _col1 = new DataColumn("col1", "col1", Constants.Dimension.NO_DIMENSION, baseGrid);
         _col2 = new DataColumn("col2", "col2", Constants.Dimension.NO_DIMENSION, baseGrid);
         _col1.DataInfo.ExtendedProperties.Add(new ExtendedProperty<string> {Name = "Prop1", Value = "toto1"});
         _col1.DataInfo.ExtendedProperties.Add(new ExtendedProperty<string> {Name = "Prop2", Value = "toto2"});

         _col2.DataInfo.ExtendedProperties.Add(new ExtendedProperty<string> {Name = "Prop3", Value = "toto3"});
         _col2.DataInfo.ExtendedProperties.Add(new ExtendedProperty<string> {Name = "Prop2", Value = "toto4"});
         _dataRepository = new DataRepository {_col1, _col2};
      }

      protected override void Because()
      {
         sut.Convert(_dataRepository);
      }

      [Observation]
      public void should_have_moved_the_extended_properties_from_each_column_to_the_data_repository_itself()
      {
         _dataRepository.ExtendedPropertyValueFor("Prop1").ShouldBeEqualTo("toto1");
         _dataRepository.ExtendedPropertyValueFor("Prop2").ShouldBeEqualTo("toto2");
         _dataRepository.ExtendedPropertyValueFor("Prop3").ShouldBeEqualTo("toto3");
      }

      [Observation]
      public void should_have_left_the_duplicate_extended_properties_as_is()
      {
         _col2.DataInfo.ExtendedProperties.Contains("Prop2").ShouldBeTrue();
      }
   }
}