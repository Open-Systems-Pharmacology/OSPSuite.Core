using System.Linq;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   internal class BuildingBlockWithFormulaCacheXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
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
         var x1 = _module.Observers;
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualBuildingBlock(x2, x1);
      }
   }

   internal class MoleculeBuildingBlockXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexMoleculeBB()
      {
         var x1 = _module.Molecules;
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualBuildingBlock(x2, x1);
      }
   }

   internal class PassiveTransportBuildingBlockXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexPassiveTransportBB()
      {
         var x1 = _module.PassiveTransports;
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualBuildingBlock(x2, x1);
      }
   }

   internal class ReactionBuildingBlockXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexReactionBB()
      {
         var x1 = _module.Reactions;
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualBuildingBlock(x2, x1);
      }
   }

   internal class EventGroupBuildingBlockXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexEventGroupBB()
      {
         var x1 = _module.EventGroups;
         var a1 = CreateObject<ApplicationBuilder>().WithName("App.Builder");
         x1.Add(a1);
         var applicationMoleculeBuilder = CreateObject<ApplicationMoleculeBuilder>().WithName("AppMolecule");
         var f2 = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(3.4);
         applicationMoleculeBuilder.Formula = f2;
         applicationMoleculeBuilder.RelativeContainerPath = new ObjectPath(new[] {"A", "B"});
         a1.AddMolecule(applicationMoleculeBuilder);
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualBuildingBlock(x2, x1);
      }
   }

   internal class SpatialStructureXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexSpatialStructure()
      {
         var x1 = _simulationBuilder.SpatialStructureAndMergeBehaviors[0].spatialStructure;
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualSpatialStructure(x2, x1);
      }
   }

   internal class InitialConditionsBuildingBlockXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexInitialConditionsBuildingBlock()
      {
         var x1 = _simulationConfiguration.ModuleConfigurations[0].SelectedInitialConditions;
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualInitialConditionsBuildingBlock(x2, x1);
      }
   }

   internal class ParameterValuesBuildingBlockXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexParameterValuesBuildingBlock()
      {
         var x1 = _simulationConfiguration.ModuleConfigurations[0].SelectedParameterValues;
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualParameterValuesBuildingBlock(x2, x1);
      }
   }

   internal class SimulationConfigurationXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexBuildConfiguration()
      {
         var x1 = _simulationConfiguration;
         Assert.IsNotNull(x1);
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualSimulationConfiguration(x2, x1);
      }
   }
}