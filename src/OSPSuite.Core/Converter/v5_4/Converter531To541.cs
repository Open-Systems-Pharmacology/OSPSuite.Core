using System.Linq;
using System.Xml.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Exchange;

namespace OSPSuite.Core.Converter.v5_4
{
   public class Converter531To541 : IObjectConverter, 
      IVisitor<SimulationTransfer>, 
      IVisitor<IEventGroupBuildingBlock>
   {
      public bool IsSatisfiedBy(int version)
      {
         return PKMLVersion.V5_3_1.Equals(version);
      }

      public int Convert(object objectToUpdate)
      {
         this.Visit(objectToUpdate);
         return PKMLVersion.V5_4_1;
      }

      public int ConvertXml(XElement element)
      {
         return PKMLVersion.V5_4_1;
      }

      public void Visit(SimulationTransfer simulationTransfer)
      {
         var simulation = simulationTransfer.Simulation;
         convertSimulation(simulation);
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

      public void Visit(IEventGroupBuildingBlock objToVisit)
      {
         convertEventGroupBuildingBlock(objToVisit);
      }
   }
}