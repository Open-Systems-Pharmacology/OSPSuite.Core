using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Utility.Visitor;
using SolverSettings = OSPSuite.Core.Domain.SolverSettings;

namespace OSPSuite.Core.Converters.v13
{
   public class Converter121To130 : IObjectConverter,
      IVisitor<IModelCoreSimulation>,
      IVisitor<SimulationTransfer>,
      IVisitor<SimulationSettings>,
      IVisitor<SolverSettings>
   {
      private bool _converted;

      private readonly ISolverSettingsFactory _solverSettingsFactory;

      public Converter121To130(ISolverSettingsFactory solverSettingsFactory)
      {
         _solverSettingsFactory = solverSettingsFactory;
      }

      // 13.0 pkml is not compatible with 12.1, but you don't need an explicit conversion to move forward.
      // To satisfy the next converter, the object must pass through v13.0 conversion
      public bool IsSatisfiedBy(int version) => version == PKMLVersion.V12_1;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         _converted = false;
         performConversion(objectToUpdate);
         return (PKMLVersion.V13_0, _converted);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         return (PKMLVersion.V13_0, false);
      }

      private void performConversion(object objectToUpdate) => this.Visit(objectToUpdate);

      public void Visit(SimulationTransfer simulationTransfer)
      {
         Visit(simulationTransfer.Simulation);
      }

      public void Visit(SimulationSettings settings)
      {
         Visit(settings.Solver);
      }

      public void Visit(IModelCoreSimulation modelCoreSimulation)
      {
         Visit(modelCoreSimulation.Settings);
      }

      public void Visit(SolverSettings solverSettings)
      {
         _solverSettingsFactory.AddCheckNegativeValuesParameter(solverSettings);

         _converted = true;
      }
   }
}