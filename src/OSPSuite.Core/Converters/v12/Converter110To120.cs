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
      private const string _initialConditionsBuildingBlockName = "InitialConditionsBuildingBlock";
      private const string _parameterValuesBuildingBlockName = "ParameterValuesBuildingBlock";
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
         element.DescendantsAndSelfNamed("MoleculeStartValue").Each(convertMoleculeStartValue);
         element.DescendantsAndSelfNamed("ParameterStartValue").Each(convertParameterStartValue);
         element.DescendantsAndSelfNamed("MoleculeStartValuesBuildingBlock").Each(convertMoleculeStartValuesBuildingBlock);
         element.DescendantsAndSelfNamed("ParameterStartValuesBuildingBlock").Each(convertParameterStartValuesBuildingBlock);
         return (PKMLVersion.V12_0, _converted);
      }

      private void convertParameterStartValue(XElement element)
      {
         element.Name = "ParameterValue";
         replaceStartValueAttribute(element);
         _converted = true;
      }

      private void replaceStartValueAttribute(XElement element)
      {
         var attribute = element.Attribute("startValue");
         if (attribute != null)
         {
            attribute.Remove();
            element.AddAttribute("value", attribute.Value);
            _converted = true;
         }
      }

      private void convertParameterStartValuesBuildingBlock(XElement element)
      {
         element.Name = _parameterValuesBuildingBlockName;
         _converted = true;
      }

      private void convertMoleculeStartValuesBuildingBlock(XElement element)
      {
         element.Name = _initialConditionsBuildingBlockName;
         _converted = true;
      }

      private void convertMoleculeStartValue(XElement element)
      {
         element.Name = "InitialCondition";
         replaceStartValueAttribute(element);
         _converted = true;
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

         var simulationConfigurationElement = new XElement(Constants.Serialization.SIMULATION_CONFIGURATION);
         var moduleConfigurationList = new XElement("ModuleConfigurations");
         var moduleConfiguration = new XElement("ModuleConfiguration");
         var moduleElement = new XElement("Module");
         var buildingBlockList = new XElement("BuildingBlocks");
         moduleElement.Add(buildingBlockList);
         moduleElement.SetAttributeValue("name", simulationNode.GetAttribute("name"));
         
         moduleConfigurationList.Add(moduleConfiguration);
         moduleConfiguration.Add(moduleElement);
         simulationConfigurationElement.Add(moduleConfigurationList);

         var buildingBlockElement = buildConfigurationElement.Element("Molecules");
         buildingBlockElement.Name = "MoleculeBuildingBlock";
         buildingBlockList.Add(buildingBlockElement);

         buildingBlockElement = buildConfigurationElement.Element("Reactions");
         buildingBlockElement.Name = "ReactionBuildingBlock";
         buildingBlockList.Add(buildingBlockElement);
         
         buildingBlockElement = buildConfigurationElement.Element("PassiveTransports");
         buildingBlockElement.Name = "PassiveTransportBuildingBlock";
         buildingBlockList.Add(buildingBlockElement);

         //already singular
         buildingBlockList.Add(buildConfigurationElement.Element("SpatialStructure"));

         buildingBlockElement = buildConfigurationElement.Element("Observers");
         buildingBlockElement.Name = "ObserverBuildingBlock";
         buildingBlockList.Add(buildingBlockElement);

         buildingBlockElement = buildConfigurationElement.Element("EventGroups");
         buildingBlockElement.Name = "EventGroupBuildingBlock";
         buildingBlockList.Add(buildingBlockElement);

         var parameterStartValuesElement = buildConfigurationElement.Element("ParameterStartValues");
         var selectedParameterValuesId = parameterStartValuesElement.Attribute("id").Value;
         parameterStartValuesElement.Name = _parameterValuesBuildingBlockName;
         buildingBlockList.Add(parameterStartValuesElement);

         var moleculeStartValueElement = buildConfigurationElement.Element("MoleculeStartValues");
         var selectedInitialConditionsId = moleculeStartValueElement.Attribute("id").Value;
         moleculeStartValueElement.Name = _initialConditionsBuildingBlockName;
         buildingBlockList.Add(moleculeStartValueElement);

         moduleConfiguration.AddAttribute("selectedInitialConditions", selectedInitialConditionsId);
         moduleConfiguration.AddAttribute("selectedParameterValues", selectedParameterValuesId);


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