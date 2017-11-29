using System.Linq;
using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Converter.v5_4
{
   public class Converter531To541 : IObjectConverter,
      IVisitor<SimulationTransfer>,
      IVisitor<IEventGroupBuildingBlock>
   {
      private bool _converted;

      public bool IsSatisfiedBy(int version) => version == PKMLVersion.V5_3_1;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         _converted = false;
         this.Visit(objectToUpdate);
         return (PKMLVersion.V5_4_1, _converted);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         return (PKMLVersion.V5_4_1, false);
      }

      public void Visit(SimulationTransfer simulationTransfer)
      {
         var simulation = simulationTransfer.Simulation;
         convertSimulation(simulation);
         _converted = true;
      }

      public void Visit(IEventGroupBuildingBlock eventGroupBuildingBlock)
      {
         convertEventGroupBuildingBlock(eventGroupBuildingBlock);
         _converted = true;
      }

      private void convertSimulation(IModelCoreSimulation simulation)
      {
         var eventGroupBuildingBlock = simulation.BuildConfiguration.EventGroups;
         convertEventGroupBuildingBlock(eventGroupBuildingBlock);
         var allApplicationNames = eventGroupBuildingBlock.SelectMany(eg => eg.GetAllContainersAndSelf<IApplicationBuilder>()).Select(ap => ap.Name);
         var allEventGroups = simulation.Model.Root.GetAllChildren<IEventGroup>();
         var allApplications = allEventGroups.Where(eg => allApplicationNames.Contains(eg.Name));
         allApplications.Each(app => app.ContainerType = ContainerType.Application);
      }

      private void convertEventGroupBuildingBlock(IEventGroupBuildingBlock eventGroups)
      {
         var allApplications = eventGroups.SelectMany(eg => eg.GetAllContainersAndSelf<IApplicationBuilder>());
         allApplications.Each(app => app.ContainerType = ContainerType.Application);
      }
   }
}