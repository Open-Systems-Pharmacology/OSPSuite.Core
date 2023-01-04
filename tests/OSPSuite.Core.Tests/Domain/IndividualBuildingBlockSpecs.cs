using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using System.Linq;

namespace OSPSuite.Core.Domain
{
   public class concern_for_IndividualBuildingBlock : ContextSpecification<IndividualBuildingBlock>
   {
      protected override void Context()
      {
         sut = new IndividualBuildingBlock();
      }
   }

   public class when_updating_properties_of_individual : concern_for_IndividualBuildingBlock
   {
      private IndividualBuildingBlock _sourceIndividualBuildingBlock;
      private ICloneManager _cloneManager;
      private IObjectBaseFactory _objectBaseFactory;
      private IDataRepositoryTask _repositoryTask;
      private IDimensionFactory _dimensionFactory;
      private OriginDataItem _originDataItem;

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
         _sourceIndividualBuildingBlock.Add(new IndividualParameter().WithName("name1"));
         _originDataItem = new OriginDataItem
         {
            Description = "Description",
            DisplayName = "DisplayName",
            Icon = "Icon",
            Name = "Name",
            Value = "Value"
         };
         _sourceIndividualBuildingBlock.OriginData.AddOriginDataItem(_originDataItem);
         _sourceIndividualBuildingBlock.OriginData.ValueOrigin = new ValueOrigin { Description = "ValueOrigin" };

      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_sourceIndividualBuildingBlock, _cloneManager);
      }

      [Observation]
      public void the_updated_expression_profile_should_have_properties_set()
      {
         sut.Name.ShouldBeEqualTo("An Individual");
         sut.PKSimVersion.ShouldBeEqualTo("11.1");
         sut.Count().ShouldBeEqualTo(1);
         sut.OriginData.AllDataItems.Count.ShouldBeEqualTo(1);
         var clonedOriginDataItem = sut.OriginData.AllDataItems.First();

         clonedOriginDataItem.DisplayName.ShouldBeEqualTo(_originDataItem.DisplayName);
         clonedOriginDataItem.Description.ShouldBeEqualTo(_originDataItem.Description);
         clonedOriginDataItem.Icon.ShouldBeEqualTo(_originDataItem.Icon);
         clonedOriginDataItem.Name.ShouldBeEqualTo(_originDataItem.Name);
         clonedOriginDataItem.Value.ShouldBeEqualTo(_originDataItem.Value);
         sut.OriginData.ValueOrigin.Description.ShouldBeEqualTo(_sourceIndividualBuildingBlock.OriginData.ValueOrigin.Description);
      }
   }
}
