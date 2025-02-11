using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public class concern_for_ExpressionProfileBuildingBlock : ContextSpecification<ExpressionProfileBuildingBlock>
   {
      protected override void Context()
      {
         sut = new ExpressionProfileBuildingBlock();
      }
   }

   public class when_updating_properties_of_building_block : concern_for_ExpressionProfileBuildingBlock
   {
      private ExpressionProfileBuildingBlock _expressionProfileBuildingBlock;
      private ICloneManager _cloneManager;
      private IObjectBaseFactory _objectBaseFactory;
      private IDataRepositoryTask _repositoryTask;
      private IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         base.Context();
         _dimensionFactory = new DimensionFactoryForIntegrationTests();
         _dimensionFactory.AddDimension(DomainHelperForSpecs.NoDimension());
         _objectBaseFactory = new ObjectBaseFactoryForSpecs(_dimensionFactory);
         _repositoryTask = new DataRepositoryTask();
         _expressionProfileBuildingBlock = new ExpressionProfileBuildingBlock();
         _cloneManager = new CloneManagerForBuildingBlock(_objectBaseFactory, _repositoryTask);
         _expressionProfileBuildingBlock.Name = "Molecule|Species|Name";
         _expressionProfileBuildingBlock.Type = ExpressionTypes.MetabolizingEnzyme;
         _expressionProfileBuildingBlock.Type = ExpressionTypes.MetabolizingEnzyme;
         _expressionProfileBuildingBlock.PKSimVersion = "11.1";
         _expressionProfileBuildingBlock.Add(new ExpressionParameter().WithName("name1"));
         var initialCondition = new InitialCondition().WithName("ic1");
         initialCondition.Path = new ObjectPath("path1");
         _expressionProfileBuildingBlock.Add(initialCondition);

         var clonedInitialCondition = new InitialCondition().WithName(initialCondition.Name);
         clonedInitialCondition.Path = initialCondition.Path;
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_expressionProfileBuildingBlock, _cloneManager);
      }

      [Observation]
      public void the_updated_expression_profile_should_have_properties_set()
      {
         sut.Name.ShouldBeEqualTo("Molecule|Species|Name");
         sut.Type.ShouldBeEqualTo(ExpressionTypes.MetabolizingEnzyme);
         sut.PKSimVersion.ShouldBeEqualTo("11.1");
         sut.Count<ExpressionParameter>().ShouldBeEqualTo(1);
         sut.Count<InitialCondition>().ShouldBeEqualTo(1);
      }
   }


   public class when_reading_the_icon_name_for_the_building_block : concern_for_ExpressionProfileBuildingBlock
   {
      [Observation]
      public void icon_name_translated_for_each_expression_type()
      {
         sut.Type = ExpressionTypes.MetabolizingEnzyme;
         sut.Icon.ShouldBeEqualTo("Enzyme");

         sut.Type = ExpressionTypes.TransportProtein;
         sut.Icon.ShouldBeEqualTo("Transporter");

         sut.Type = ExpressionTypes.ProteinBindingPartner;
         sut.Icon.ShouldBeEqualTo("Protein");
      }
   }

   public class when_setting_the_name_of_the_building_block : concern_for_ExpressionProfileBuildingBlock
   {
      protected override void Because()
      {
         sut.Name = "Molecule|Species|Phenotype";
      }

      [Observation]
      public void the_name_should_set_the_category_of_the_building_block()
      {
         sut.Category.ShouldBeEqualTo("Phenotype");
         sut.MoleculeName.ShouldBeEqualTo("Molecule");
         sut.Species.ShouldBeEqualTo("Species");
      }
   }
}