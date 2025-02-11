using System.Linq;
using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Converters.v7_3
{
   public class Converter710To730 : IObjectConverter,
      IVisitor<ParameterValuesBuildingBlock>,
      IVisitor<PassiveTransportBuildingBlock>,
      IVisitor<SimulationSettings>,
      IVisitor<EventGroupBuildingBlock>,
      IVisitor<MoleculeBuildingBlock>,
      IVisitor<ReactionBuildingBlock>,
      IVisitor<SpatialStructure>,
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

      private XElement valueOriginFor(string valueDescription)
      {
         var element = new XElement(Constants.Serialization.VALUE_ORIGIN);
         element.SetAttributeValue(Constants.Serialization.Attribute.DESCRIPTION, valueDescription);
         return element;
      }

      public void Visit(ISimulation simulation)
      {
         ConvertAllParametersIn(simulation.Model.Root);
      }

      public void Visit(ParameterValuesBuildingBlock parameterValuesBuildingBlock)
      {
         parameterValuesBuildingBlock.Each(ConvertWithDefaultStateObjectToDefault);
      }

      public void Visit(EventGroupBuildingBlock eventGroupBuildingBlock)
      {
         ConvertAllParametersIn(eventGroupBuildingBlock);
      }

      public void Visit(MoleculeBuildingBlock moleculeBuildingBlock)
      {
         ConvertAllParametersIn(moleculeBuildingBlock);
      }

      public void Visit(ReactionBuildingBlock reactionBuildingBlock)
      {
         ConvertAllParametersIn(reactionBuildingBlock);
      }

      public void Visit(SpatialStructure spatialStructure)
      {
         spatialStructure.Each(ConvertAllParametersIn);
      }

      public void ConvertAllParametersIn<T>(IBuildingBlock<T> buildingBlock) where T : class, IContainer, IBuilder
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

      public void Visit(PassiveTransportBuildingBlock passiveTransportBuildingBlock)
      {
         ConvertAllParametersIn(passiveTransportBuildingBlock);
      }

      public void Visit(SimulationSettings simulationSettings)
      {
         ConvertAllParametersIn(simulationSettings.Solver);
         ConvertAllParametersIn(simulationSettings.OutputSchema);
      }
   }
}