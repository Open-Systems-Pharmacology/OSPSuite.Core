using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public class BuildingBlockWithFormulaCacheXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestSimpleBB()
      {
         var x1 = CreateObject<ObserverBuildingBlock>().WithName("Bob");

         x1.Creation.Origin = Origins.MoBi;
         x1.Creation.CreationMode = CreationMode.Configure;
         x1.Creation.ClonedFrom = "Sim";

         var f1 = CreateObject<ConstantFormula>().WithName("F.Constantin").WithDimension(DimensionLength).WithValue(2.1);
         var f2 = CreateObject<ConstantFormula>().WithName("F.Constanze").WithDimension(DimensionLength).WithValue(5.6);
         var f3 = CreateObject<ExplicitFormula>().WithName("F.Erika").WithDimension(DimensionLength).WithFormulaString("A * 2");
         var fup = new FormulaUsablePath(new[] {"aa", "bb"}).WithAlias("b").WithDimension(DimensionLength);
         f3.AddObjectPath(fup);

         x1.AddFormula(f1);
         x1.AddFormula(f2);
         x1.AddFormula(f3);

         var builder = CreateObject<AmountObserverBuilder>();
         builder.MoleculeList.ForAll = false;
         builder.AddMoleculeName("H2O");
         builder.AddMoleculeName("CO2");
         builder.ContainerCriteria = new DescriptorCriteria
         {
            new MatchTagCondition("Organ"),
            new NotMatchTagCondition("Organ")
         };
         builder.Formula = f3;

         var builder2 = CreateObject<ContainerObserverBuilder>();
         builder2.ForAll = true;
         builder2.Formula = f2;

         x1.Add(builder);
         x1.Add(builder2);

         var x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualBuildingBlock(x2, x1);
      }

      [Test]
      public void TestComplexObserverBB()
      {
         ObserverBuildingBlock x1 = _buildConfiguration.Observers as ObserverBuildingBlock;
         Assert.IsNotNull(x1);

         IObserverBuildingBlock x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualBuildingBlock(x2, x1);
      }
   }

   public class MoleculeBuildingBlockXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexMoleculeBB()
      {
         MoleculeBuildingBlock x1 = _buildConfiguration.Molecules as MoleculeBuildingBlock;
         Assert.IsNotNull(x1);


         IMoleculeBuildingBlock x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualBuildingBlock(x2, x1);
      }
   }

   public class PassiveTransportBuildingBlockXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexPassiveTransportBB()
      {
         PassiveTransportBuildingBlock x1 = _buildConfiguration.PassiveTransports as PassiveTransportBuildingBlock;
         Assert.IsNotNull(x1);

         IPassiveTransportBuildingBlock x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualBuildingBlock(x2, x1);
      }
   }

   public class ReactionBuildingBlockXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexReactionBB()
      {
         ReactionBuildingBlock x1 = _buildConfiguration.Reactions as ReactionBuildingBlock;
         Assert.IsNotNull(x1);

         IReactionBuildingBlock x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualBuildingBlock(x2, x1);
      }
   }

   public class EventGroupBuildingBlockXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexEventGroupBB()
      {
         EventGroupBuildingBlock x1 = _buildConfiguration.EventGroups as EventGroupBuildingBlock;
         Assert.IsNotNull(x1);
         ApplicationBuilder a1 = CreateObject<ApplicationBuilder>().WithName("App.Builder");
         x1.Add(a1);
         var applicationMoleculeBuilder = CreateObject<ApplicationMoleculeBuilder>().WithName("AppMolecule");
         IFormula f2 = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(3.4);
         applicationMoleculeBuilder.Formula = f2;
         applicationMoleculeBuilder.RelativeContainerPath = new ObjectPath(new[] {"A", "B"});
         a1.AddMolecule(applicationMoleculeBuilder);
         IEventGroupBuildingBlock x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualBuildingBlock(x2, x1);
      }
   }

   public class SpatialStructureXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexSpatialStructure()
      {
         SpatialStructure x1 = _buildConfiguration.SpatialStructure as SpatialStructure;
         Assert.IsNotNull(x1);

         ISpatialStructure x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualSpatialStructure(x2, x1);
      }
   }

   public class MoleculeStartValuesBuildingBlockXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexMoleculeStartValuesBuildingBlock()
      {
         MoleculeStartValuesBuildingBlock x1 = _buildConfiguration.MoleculeStartValues as MoleculeStartValuesBuildingBlock;
         Assert.IsNotNull(x1);

         IMoleculeStartValuesBuildingBlock x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualMoleculeStartValuesBuildingBlock(x2, x1);
      }
   }

   public class ParameterStartValuesBuildingBlockXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexParameterStartValuesBuildingBlock()
      {
         ParameterStartValuesBuildingBlock x1 = _buildConfiguration.ParameterStartValues as ParameterStartValuesBuildingBlock;
         Assert.IsNotNull(x1);
         IParameterStartValuesBuildingBlock x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualParameterStartValuesBuildingBlock(x2, x1);
      }
   }

   public class BuildConfigurationXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexBuildConfiguration()
      {
         BuildConfiguration x1 = _buildConfiguration as BuildConfiguration;
         Assert.IsNotNull(x1);

         IBuildConfiguration x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualBuildConfiguration(x2, x1);
      }
   }
}