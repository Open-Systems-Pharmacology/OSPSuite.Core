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
      IVisitor<ModuleConfiguration>,
      IVisitor<ParameterValuesBuildingBlock>
   {
      private const string _initialConditionsBuildingBlockName = "InitialConditionsBuildingBlock";
      private const string _parameterValuesBuildingBlockName = "ParameterValuesBuildingBlock";
      private const string _buildMode = "buildMode";
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
         element.DescendantsAndSelfNamed("EventGroupBuildingBlock").Each(convertEventGroupBuildingBlock);
         element.DescendantsAndSelf().Where(x => x.GetAttribute(_buildMode) != null).Each(convertBuildMode);
         return (PKMLVersion.V12_0, _converted);
      }

      private void convertEventGroupBuildingBlock(XElement eventGroupNode)
      {
         eventGroupNode.SetAttributeValue("icon", "Event");
         _converted = true;
      }

      private void convertBuildMode(XElement parameterNode)
      {
         var buildMode = parameterNode.GetAttribute(_buildMode);
         
         if (!string.Equals(buildMode, "Property"))
            return;

         parameterNode.SetAttributeValue(_buildMode, "Global");
         _converted = true;
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
         updateToParameterValuesElement(element);
         _converted = true;
      }

      private void convertMoleculeStartValuesBuildingBlock(XElement element)
      {
         updateToInitialConditionsElement(element);
         _converted = true;
      }

      private static void updateToInitialConditionsElement(XElement element)
      {
         element.Name = _initialConditionsBuildingBlockName;
         element.SetAttributeValue("icon", "InitialConditions");
      }

      private static void updateToParameterValuesElement(XElement element)
      {
         element.Name = _parameterValuesBuildingBlockName;
         element.SetAttributeValue("icon", "ParameterValues");
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
         updateToParameterValuesElement(parameterStartValuesElement);
         buildingBlockList.Add(parameterStartValuesElement);

         var moleculeStartValueElement = buildConfigurationElement.Element("MoleculeStartValues");
         var selectedInitialConditionsId = moleculeStartValueElement.Attribute("id").Value;
         updateToInitialConditionsElement(moleculeStartValueElement);
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

      public void Visit(ParameterValuesBuildingBlock buildingBlock)
      {
         // In building blocks prior to v12 (parameter start values building blocks) it was possible to have parameter values
         // with both formula and value. In those cases, the formula would always be used and the value ignored
         // In v12 the logic is modified, if there is a value, it overrides the formula, so for building blocks older than v12
         // the value, which would have been ignored anyway, has to be removed when there is a formula
         buildingBlock.Each(x =>
         {
            if (x.Formula != null)
               x.Value = null;
         });
      }

      public void Visit(ModuleConfiguration moduleConfiguration)
      {
         //I am using the actual visit here so that it will itself manage to find the most appropriate visitor implementation 
         //for children of the configuration
         moduleConfiguration.AcceptVisitor(this);
      }
   }
}