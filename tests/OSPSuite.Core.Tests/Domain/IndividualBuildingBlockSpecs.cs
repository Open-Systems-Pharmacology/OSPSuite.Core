using System;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_IndividualBuildingBlock : ContextSpecification<IndividualBuildingBlock>
   {
      protected override void Context()
      {
         sut = new IndividualBuildingBlock();
      }
   }

   public class When_updating_properties_of_individual : concern_for_IndividualBuildingBlock
   {
      private IndividualBuildingBlock _sourceIndividualBuildingBlock;
      private ICloneManager _cloneManager;
      private IObjectBaseFactory _objectBaseFactory;
      private IDataRepositoryTask _repositoryTask;
      private IDimensionFactory _dimensionFactory;
      private IExtendedProperty _originDataItem;

      protected override void Context()
      {
         base.Context();
         _dimensionFactory = new DimensionFactoryForIntegrationTests();
         _dimensionFactory.AddDimension(DomainHelperForSpecs.NoDimension());
         _objectBaseFactory = new ObjectBaseFactoryForSpecs(_dimensionFactory);
         _repositoryTask = new DataRepositoryTask();
         _sourceIndividualBuildingBlock = new IndividualBuildingBlock();
         _cloneManager = new CloneManagerForBuildingBlock(_objectBaseFactory, _repositoryTask);
         _sourceIndividualBuildingBlock.Name = "An Individual";
         _sourceIndividualBuildingBlock.PKSimVersion = "11.1";
         var individualParameter = new IndividualParameter().WithName("name1");
         individualParameter.DistributionType = DistributionType.Discrete;
         individualParameter.Origin = new ParameterOrigin
         {
            SimulationId = "SimId",
            BuilingBlockId = "BbId",
            ParameterId = "ParamId"
         };
         individualParameter.Info = new ParameterInfo
         {
            ReadOnly = true
         };

         _sourceIndividualBuildingBlock.Add(individualParameter);
         _originDataItem = new ExtendedProperty<string>()
         {
            Description = "Description",
            FullName = "DisplayName",
            Name = "Name",
            Value = "Value"
         };
         _sourceIndividualBuildingBlock.OriginData.Add(_originDataItem);
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_sourceIndividualBuildingBlock, _cloneManager);
      }

      [Observation]
      public void the_updated_individual_should_have_properties_set()
      {
         sut.Name.ShouldBeEqualTo("An Individual");
         sut.PKSimVersion.ShouldBeEqualTo("11.1");
         sut.Count().ShouldBeEqualTo(1);

         var clonedIndividualParameter = sut.First();
         clonedIndividualParameter.DistributionType.ShouldBeEqualTo(DistributionType.Discrete);
         clonedIndividualParameter.Origin.SimulationId.ShouldBeEqualTo("SimId");
         clonedIndividualParameter.Origin.BuilingBlockId.ShouldBeEqualTo("BbId");
         clonedIndividualParameter.Origin.ParameterId.ShouldBeEqualTo("ParamId");
         clonedIndividualParameter.Info.ReadOnly.ShouldBeTrue();

         sut.OriginData.All.Length.ShouldBeEqualTo(1);
         var clonedOriginDataItem = sut.OriginData.All.First();

         clonedOriginDataItem.DisplayName.ShouldBeEqualTo(_originDataItem.DisplayName);
         clonedOriginDataItem.Description.ShouldBeEqualTo(_originDataItem.Description);
         clonedOriginDataItem.Name.ShouldBeEqualTo(_originDataItem.Name);
         clonedOriginDataItem.ValueAsObject.ShouldBeEqualTo(_originDataItem.ValueAsObject);
      }
   }

   public class When_retrieving_a_individual_parameter_by_path : concern_for_IndividualBuildingBlock
   {
      private IndividualParameter _individualParameter1;
      private IndividualParameter _individualParameter2;

      protected override void Context()
      {
         base.Context();
         _individualParameter1 = new IndividualParameter
         {
            Path = new ObjectPath("A", "B", "C")
         };
         _individualParameter2 = new IndividualParameter
         {
            Path = new ObjectPath("A", "B", "C", "D")
         };

         sut = new IndividualBuildingBlock
         {
            _individualParameter1,
            _individualParameter2
         };
      }

      [Observation]
      public void should_return_the_expected_parameter()
      {
         sut.FindByPath("A|B|C").ShouldBeEqualTo(_individualParameter1);
         sut.FindByPath("A|B|C|D").ShouldBeEqualTo(_individualParameter2);
      }

      [Observation]
      public void should_return_null_if_the_parameter_by_path_is_not_found()
      {
         sut.FindByPath("NOPE").ShouldBeNull();
      }
   }
}