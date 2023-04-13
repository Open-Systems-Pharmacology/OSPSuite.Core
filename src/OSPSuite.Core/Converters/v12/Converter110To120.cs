using System.Linq;
using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.Xml.Extensions;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Converters.v12
{
   public class Converter110To120 : IObjectConverter,
      IVisitor<SpatialStructure>,
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
         var simulationConfigurationElement = new XElement(Constants.Serialization.SIMULATION_CONFIGURATION);
         var moduleConfigurationList = new XElement("ModuleConfigurations");
         var moduleConfiguration = new XElement("ModuleConfiguration");
         var moduleElement = new XElement("Module");
         moduleConfigurationList.Add(moduleConfiguration);
         moduleConfiguration.Add(moduleElement);
         simulationConfigurationElement.Add(moduleConfigurationList);

         moduleElement.Add(buildConfigurationElement.Element("Molecules"));
         moduleElement.Add(buildConfigurationElement.Element("Reactions"));
         moduleElement.Add(buildConfigurationElement.Element("PassiveTransports"));
         //already singular
         moduleElement.Add(buildConfigurationElement.Element("SpatialStructure"));
         moduleElement.Add(buildConfigurationElement.Element("Observers"));
         moduleElement.Add(buildConfigurationElement.Element("EventGroups"));

         var parameterStartValuesElement = buildConfigurationElement.Element("ParameterStartValues");
         var parameterStartValuesId = parameterStartValuesElement.Attribute("id").Value;
         parameterStartValuesElement.Name = "ParameterStartValuesBuildingBlock";
         parameterStartValueCollectionElement.Add(parameterStartValuesElement);

         var moleculeStartValueElement = buildConfigurationElement.Element("MoleculeStartValues");
         var moleculeStartValuesId = moleculeStartValueElement.Attribute("id").Value;
         moleculeStartValueElement.Name = "MoleculeStartValuesBuildingBlock";
         moleculeStartValueCollectionElement.Add(moleculeStartValueElement);

         moduleConfiguration.AddAttribute("selectedMoleculeStartValues", moleculeStartValuesId);
         moduleConfiguration.AddAttribute("selectedParameterStartValues", parameterStartValuesId);

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

      private void performConversion(object objectToUpdate) => this.Visit(objectToUpdate);

      public void Visit(SpatialStructure spatialStructure)
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
         //I am using the actual visit here so that it will itself manage to find the most appropriate visitor implementation 
         //for children of the configuration
         moduleConfiguration.AcceptVisitor(this);
      }
   }
}