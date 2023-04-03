using System.Linq;
using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.Xml.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Converters.v12
{
   public class Converter110To120 : IObjectConverter,
      IVisitor<ISpatialStructure>,
      IVisitor<SimulationTransfer>,
      IVisitor<IModelCoreSimulation>,
      IVisitor<SimulationConfiguration>,
      IVisitor<ModuleConfiguration>
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private bool _converted;

      public Converter110To120(IObjectPathFactory objectPathFactory)
      {
         _objectPathFactory = objectPathFactory;
      }

      public bool IsSatisfiedBy(int version) => version == PKMLVersion.V11_0;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         _converted = false;
         performConversion(objectToUpdate);
         return (PKMLVersion.V12_0, _converted);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         _converted = false;
         element.DescendantsAndSelfNamed("Simulation").Each(ConvertSimulation);
         return (PKMLVersion.V12_0, _converted);
      }

      public void ConvertSimulation(XElement simulationElement)
      {
         //use to list because the node will be deleted
         simulationElement.Descendants("BuildConfiguration").ToList().Each(convertBuildConfigurationToSimulationConfiguration);
      }

      private void convertBuildConfigurationToSimulationConfiguration(XElement buildConfigurationElement)
      {
         var simulationNode = buildConfigurationElement.Parent;
         if (simulationNode == null)
            return;

         var moleculeStartValueCollectionElement = new XElement("MoleculeStartValuesCollection");
         var parameterStartValueCollectionElement = new XElement("ParameterStartValuesCollection");
         var moduleElement = new XElement("Module");
         var simulationConfigurationElement = new XElement(Constants.Serialization.SIMULATION_CONFIGURATION);
         simulationConfigurationElement.Add(moduleElement);

         moduleElement.Add(getAndRename(buildConfigurationElement, "Molecules", "Molecule"));
         moduleElement.Add(getAndRename(buildConfigurationElement, "Reactions", "Reaction"));
         moduleElement.Add(getAndRename(buildConfigurationElement, "PassiveTransports", "PassiveTransport"));
         //already singular
         moduleElement.Add(buildConfigurationElement.Element("SpatialStructure"));
         moduleElement.Add(getAndRename(buildConfigurationElement, "Observers", "Observer"));
         moduleElement.Add(getAndRename(buildConfigurationElement, "EventGroups", "EventGroup"));

         var parameterStartValuesElement = buildConfigurationElement.Element("ParameterStartValues");
         parameterStartValuesElement.Name = "ParameterStartValuesBuildingBlock";
         parameterStartValueCollectionElement.Add(parameterStartValuesElement);

         var moleculeStartValueElement = buildConfigurationElement.Element("MoleculeStartValues");
         moleculeStartValueElement.Name = "MoleculeStartValuesBuildingBlock";
         moleculeStartValueCollectionElement.Add(moleculeStartValueElement);

         moduleElement.Add(parameterStartValueCollectionElement);
         moduleElement.Add(moleculeStartValueCollectionElement);

         var simulationSettings = buildConfigurationElement.Element("SimulationSettings");
         var allCalculationMethods = buildConfigurationElement.Element("AllCalculationMethods");
         buildConfigurationElement.Remove();
         simulationConfigurationElement.Add(moduleElement);
         simulationConfigurationElement.Add(simulationSettings);
         simulationConfigurationElement.Add(allCalculationMethods);

         simulationNode.Add(simulationConfigurationElement);
         _converted = true;
      }

      private XElement getAndRename(XElement element, string oldName, string newName)
      {
         var childElement = element.Element(oldName);
         childElement.Name = newName;
         return childElement;
      }

      private void performConversion(object objectToUpdate) => this.Visit(objectToUpdate);

      public void Visit(ISpatialStructure spatialStructure)
      {
         spatialStructure.Neighborhoods.Each(updateNeighborsPathIn);
      }

      private void updateNeighborsPathIn(NeighborhoodBuilder neighborhoodBuilder)
      {
         neighborhoodBuilder.FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(neighborhoodBuilder.FirstNeighbor);
         neighborhoodBuilder.SecondNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(neighborhoodBuilder.SecondNeighbor);
         _converted = true;
      }

      public void Visit(SimulationTransfer simulationTransfer)
      {
         Visit(simulationTransfer.Simulation);
      }

      public void Visit(IModelCoreSimulation modelCoreSimulation)
      {
         Visit(modelCoreSimulation.Configuration);
      }

      public void Visit(SimulationConfiguration simulationConfiguration)
      {
         simulationConfiguration.ModuleConfigurations.Each(Visit);
      }

      public void Visit(ModuleConfiguration moduleConfiguration)
      {
         moduleConfiguration.AcceptVisitor(this);
      }
   }
}