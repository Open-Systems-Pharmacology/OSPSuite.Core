using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public class EventAssignmentBuilderXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         EventAssignmentBuilder x1 = CreateObject<EventAssignmentBuilder>().WithName("Eva.Builder").WithDimension(DimensionLength);
         x1.UseAsValue = true;
         x1.ObjectPath = new ObjectPath(new string[] {"aa", "bb"});
         x1.Dimension = DimensionLength;
         x1.Formula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(23.4);

         IEventAssignmentBuilder x2 = SerializeAndDeserialize(x1);
         
         AssertForSpecs.AreEqualEventAssignmentBuilder(x1, x2);
      }
   }

   public class EventBuilderXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         EventBuilder x1 = CreateObject<EventBuilder>().WithName("Eve.Builder").WithDimension(DimensionLength);
         x1.Formula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(23.4);
         x1.OneTime = true;

         IFormula f1 = CreateObject<ExplicitFormula>().WithDimension(DimensionLength).WithFormulaString("3*Patty");
         IFormulaUsablePath fup = new FormulaUsablePath(new string[] {"Patricia"}).WithAlias("Patty").WithDimension(DimensionLength);
         f1.AddObjectPath(fup);
         IFormula f2 = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(23.4);

         IParameter p1 = WithExtensions.WithValue<Parameter>(CreateObject<Parameter>().WithName("Patricia").WithFormula(f1), 3.1);
         IParameter p2 = WithExtensions.WithValue<Parameter>(CreateObject<Parameter>().WithName("Pascal").WithFormula(f1), 3.2);

         x1.AddParameter(p1);
         x1.AddParameter(p2);

         IEventAssignmentBuilder eab1 = CreateObject<EventAssignmentBuilder>().WithDimension(DimensionLength).WithFormula(f1).WithName("eab1");
         IEventAssignmentBuilder eab2 = CreateObject<EventAssignmentBuilder>().WithFormula(f2).WithName("eab2");

         x1.AddAssignment(eab1);
         x1.AddAssignment(eab2);

         var x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualEventBuilder(x1, x2);
      }
   }

   public class EventGroupBuilderXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         EventGroupBuilder x1 = CreateObject<EventGroupBuilder>().WithName("Evan.Builder");
         x1.EventGroupType = "IV Bolus";
         IFormula f1 = CreateObject<ConstantFormula>().WithValue(2.3);
         IFormula f2 = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(3.4);

         IParameter p1 = CreateObject<Parameter>().WithName("Patricia").WithFormula(f1).WithValue(3.1);
         IParameter p2 = CreateObject<Parameter>().WithName("Pascal").WithFormula(f1).WithValue(3.2);

         x1.Add(p1);
         x1.Add(p2);

         EventBuilder eb1 = CreateObject<EventBuilder>().WithName("Eve.Builder").WithDimension(DimensionLength).WithFormula(f1);
         EventBuilder eb2 = CreateObject<EventBuilder>().WithName("Eva.Builder").WithDimension(DimensionLength).WithFormula(f2);
         eb2.OneTime = true;

         x1.Add(eb1);
         x1.Add(eb2);

         ApplicationBuilder a1 = CreateObject<ApplicationBuilder>().WithName("App.Builder");
         x1.Add(a1);
         var applicationMoleculeBuilder = CreateObject<ApplicationMoleculeBuilder>().WithName("AppMolecule");
         applicationMoleculeBuilder.Formula = f2;
         applicationMoleculeBuilder.RelativeContainerPath = new ObjectPath(new[] {"A", "B"});
         a1.AddMolecule(applicationMoleculeBuilder);

         a1.AddTransport(CreateObject<TransportBuilder>().WithName("PassiveTranport").WithFormula(f1));


         IEventGroupBuilder x2 = SerializeAndDeserialize(x1);
         
         AssertForSpecs.AreEqualEventGroupBuilder(x1, x2);
      }
   }

   public class ApplicationBuilderXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         ApplicationBuilder x1 = CreateObject<ApplicationBuilder>().WithName("App.Builder");

         IFormula f1 = CreateObject<ConstantFormula>().WithValue(2.3);
         IFormula f2 = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(3.4);

         IParameter p1 = CreateObject<Parameter>().WithName("Patricia").WithFormula(f1).WithValue(3.1);
         IParameter p2 = CreateObject<Parameter>().WithName("Pascal").WithFormula(f1).WithValue(3.2);

         x1.Add(p1);
         x1.Add(p2);

         EventBuilder eb1 = CreateObject<EventBuilder>().WithName("Eve.Builder").WithDimension(DimensionLength).WithFormula(f1);
         EventBuilder eb2 = CreateObject<EventBuilder>().WithName("Eva.Builder").WithDimension(DimensionLength).WithFormula(f2);
         eb2.OneTime = true;

         x1.Add(eb1);
         x1.Add(eb2);

         var applicationMoleculeBuilder = CreateObject<ApplicationMoleculeBuilder>().WithName("AppMolecule");
         applicationMoleculeBuilder.Formula = f2;
         applicationMoleculeBuilder.RelativeContainerPath = new ObjectPath(new[] {"A", "B"});
         x1.AddMolecule(applicationMoleculeBuilder);

         x1.AddTransport(CreateObject<TransportBuilder>().WithName("PassiveTranport").WithFormula(f1));


         ApplicationBuilder x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualApplicationBuilder(x1, x2);
      }
   }

   public class ApplicationMoleculeBuilderXMLSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         ApplicationMoleculeBuilder x1 = CreateObject<ApplicationMoleculeBuilder>().WithName("AppMoleculeBuilder");
         IFormula f2 = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(3.4);
         x1.Formula = f2;
         x1.RelativeContainerPath = new ObjectPath(new[] {"A", "B"});

         IApplicationMoleculeBuilder x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualApplicationMoleculeBuilder(x1, x2);
      }
   }
}