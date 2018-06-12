using System.Linq;
using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Converter.v7_3
{
   public class Converter710To730 : IObjectConverter,
      IVisitor<IParameterStartValuesBuildingBlock>,
      IVisitor<IPassiveTransportBuildingBlock>,
      IVisitor<ISimulationSettings>,
      IVisitor<IEventGroupBuildingBlock>,
      IVisitor<IMoleculeBuildingBlock>,
      IVisitor<IReactionBuildingBlock>,
      IVisitor<ISpatialStructure>,
      IVisitor<ISimulation>
   {
      private bool _converted = false;

      public bool IsSatisfiedBy(int version) => version == PKMLVersion.V7_1_0;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         _converted = false;
         this.Visit(objectToUpdate);
         return (PKMLVersion.V7_3_0, _converted);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         var converted = false;
         //retrieve all elements with an attribute dimension
         var allValueDescriptionAttributes = from child in element.DescendantsAndSelf()
            where child.HasAttributes
            let attr = child.Attribute(Constants.Serialization.Attribute.VALUE_DESCRIPTION)
            where attr != null
            select attr;


         foreach (var valueDescriptionAttribute in allValueDescriptionAttributes)
         {
            var (description, parentElement) = (valueDescriptionAttribute.Value, valueDescriptionAttribute.Parent);
            valueDescriptionAttribute.Remove();
            parentElement.Add(valueOriginFor(description));
            converted = true;
         }

         return (PKMLVersion.V7_3_0, converted);
      }

      private XElement valueOriginFor(string valueDescriptioon)
      {
         var element = new XElement(Constants.Serialization.VALUE_ORIGIN);
         element.SetAttributeValue(Constants.Serialization.Attribute.DESCRIPTION, valueDescriptioon);
         return element;
      }

      public void Visit(ISimulation simulation)
      {
         ConvertAllParametersIn(simulation.Model.Root);
      }

      public void Visit(IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock)
      {
         parameterStartValuesBuildingBlock.Each(ConvertWithDefaultStateObjectToDefault);
      }

      public void Visit(IEventGroupBuildingBlock eventGroupBuildingBlock)
      {
         ConvertAllParametersIn(eventGroupBuildingBlock);
      }

      public void Visit(IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         ConvertAllParametersIn(moleculeBuildingBlock);
      }

      public void Visit(IReactionBuildingBlock reactionBuildingBlock)
      {
         ConvertAllParametersIn(reactionBuildingBlock);
      }

      public void Visit(ISpatialStructure spatialStructure)
      {
         ConvertAllParametersIn(spatialStructure);
      }

      public void ConvertAllParametersIn<T>(IBuildingBlock<T> buildingBlock) where T : class, IContainer
      {
         buildingBlock.Each(ConvertAllParametersIn);
      }

      public void ConvertAllParametersIn(IContainer container)
      {
         container?.GetAllChildren<IParameter>().Each(ConvertWithDefaultStateObjectToDefault);
      }

      public void ConvertWithDefaultStateObjectToDefault(IWithDefaultState withDefaultState)
      {
         withDefaultState.IsDefault = true;
         _converted = true;
      }

      public void Visit(IPassiveTransportBuildingBlock passiveTransportBuildingBlock)
      {
         ConvertAllParametersIn(passiveTransportBuildingBlock);
      }

      public void Visit(ISimulationSettings simulationSettings)
      {
         ConvertAllParametersIn(simulationSettings.Solver);
         ConvertAllParametersIn(simulationSettings.OutputSchema);
      }
   }
}