using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_DataRepositoryXmlSerializer : ModellingXmlSerializerBaseSpecs
   {
      protected BaseGrid _baseGrid;
      protected DataColumn _col1;
      protected DataColumn _relCol;
      protected DataColumn _col2;
      protected DataColumn _col3;

      protected override void Context()
      {
         base.Context();
         var path = new List<string>(new[] {"aa", "bb"});
         _baseGrid = new BaseGrid("Bastian", DimensionTime) {Values = new[] {0F, 3600F, 7200F}};

         _col1 = new DataColumn("Columbus", DimensionLength, _baseGrid)
         {
            QuantityInfo = new QuantityInfo("Quain", path, QuantityType.Parameter),
            DataInfo = new DataInfo(ColumnOrigins.Observation, AuxiliaryType.Undefined, "cm", new DateTime(2010, 10, 22), "Study1", "Dog", 2.4),
            Values = new[] {1.0F, 2.1F, -3.4F}
         };

         _relCol = new DataColumn("Renate", DimensionLength, _baseGrid);
         _relCol.DataInfo = new DataInfo(ColumnOrigins.ObservationAuxiliary, AuxiliaryType.ArithmeticStdDev, "cm", new DateTime(2010, 10, 22), "Study1", "Dog", 2.4);

         _col2 = new DataColumn("Columbine", DimensionLength, _baseGrid);
         _col2.AddRelatedColumn(_relCol);

         _col3 = new DataColumn("Colette", DimensionLength, _baseGrid);
         _col3.AddRelatedColumn(_relCol);
      }
   }

   public class When_serializing_without_base_grid : concern_for_DataRepositoryXmlSerializer
   {
      private DataRepository _originalRepository;
      private DataRepository _deserializedRepository;

      protected override void Context()
      {
         base.Context();
         _originalRepository = new DataRepository
         {
            Name = "Rene",
            Description = "The best Repository"
         };
         _originalRepository.Add(_col1);
         _originalRepository.Add(_col2);
         _originalRepository.Add(_relCol);
         _originalRepository.Add(_col3);
         _originalRepository.ExtendedProperties.Add(new ExtendedProperty<int> { Name = "Age", Value = 34 });
      }

      protected override void Because()
      {
         _deserializedRepository = SerializeAndDeserialize(_originalRepository);
      }

      [Observation]
      public void deserialized_repository_should_be_equal_to_original()
      {
         AssertForSpecs.AreEqualMcDataRepository(_originalRepository, _deserializedRepository);
      }
   }

   public class When_serializing_with_base_grid : concern_for_DataRepositoryXmlSerializer
   {
      private DataRepository _originalDataRepository;
      private DataRepository _deserializedDataRepository;

      protected override void Context()
      {
         base.Context();
         _originalDataRepository = new DataRepository
         {
            Name = "Rene",
            Description = "The best Repository"
         };
         _originalDataRepository.Add(_col1);
         _originalDataRepository.Add(_baseGrid);
         _originalDataRepository.Add(_col2);
         _originalDataRepository.Add(_relCol);
         _originalDataRepository.Add(_col3);
      }

      protected override void Because()
      {
         _deserializedDataRepository = SerializeAndDeserialize(_originalDataRepository);
      }

      [Observation]
      public void deserialized_repository_should_be_equal_to_the_original()
      {
         AssertForSpecs.AreEqualMcDataRepository(_originalDataRepository, _deserializedDataRepository);
      }
   }
}