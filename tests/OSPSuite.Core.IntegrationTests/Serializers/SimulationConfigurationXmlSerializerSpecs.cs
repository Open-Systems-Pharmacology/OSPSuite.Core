using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core.Serializers
{
   public class SimulationConfigurationXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var schema = new OutputSchema();
         var interval = new OutputInterval();
         var quantitySelection=new OutputSelections();
         var solverSettingsFactory = IoC.Resolve<ISolverSettingsFactory>();
         var x1 = new SimulationSettings { Solver = solverSettingsFactory.CreateCVODE(), OutputSchema = schema, OutputSelections = quantitySelection };
         interval.Add(CreateObject<Parameter>().WithName(Constants.Parameters.START_TIME).WithFormula(new ConstantFormula(0)));
         interval.Add(CreateObject<Parameter>().WithName(Constants.Parameters.END_TIME).WithFormula(new ConstantFormula(1440)));
         interval.Add(CreateObject<Parameter>().WithName(Constants.Parameters.RESOLUTION).WithFormula(new ConstantFormula(240)));

         quantitySelection.AddOutput(new QuantitySelection("A|B|C",QuantityType.Protein));         
         quantitySelection.AddOutput(new QuantitySelection("A|B",QuantityType.Enzyme));         

         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualSimulationSettings(x2, x1);
      }
   }
}