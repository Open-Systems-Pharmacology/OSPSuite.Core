using OSPSuite.Core;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Engine.Domain;
using OSPSuite.Utility.Container;

namespace OSPSuite.Engine
{
   public class EngineRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<EngineRegister>();
            scan.WithConvention(new OSPSuiteRegistrationConvention());
         });

         InitFormulaParser();
      }

      public static void InitFormulaParser()
      {
         //setup real ExplicitFormulaParser
         FormulaWithFormulaString.ExplicitFormulaParserCreator = (v, p) => new ExplicitFormulaParser(v, p);
      }
   }
}