using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization.Xml.Extensions;
using OSPSuite.Serializer;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class BuildingBlockXmlSerializer<TBuildingBlock> : ObjectBaseXmlSerializer<TBuildingBlock> where TBuildingBlock : class, IBuildingBlock
   {
      protected BuildingBlockXmlSerializer(string name)
         : base(name)
      {
      }

      protected BuildingBlockXmlSerializer()
      {
      }

      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Version).WithMappingName(Constants.Serialization.Attribute.BUILDING_BLOCK_VERSION);
         Map(x => x.Creation);
      }

      protected override XElement TypedSerialize(TBuildingBlock bb, SerializationContext serializationContext)
      {
         serializationContext.AddFormulasToCache(bb.FormulaCache);
         var element = base.TypedSerialize(bb, serializationContext);
         SerializerRepository.AddFormulaCacheElement(element, serializationContext);
         return element;
      }

      protected override void TypedDeserialize(TBuildingBlock bb, XElement element, SerializationContext serializationContext)
      {
         var formulaCache = SerializerRepository.DeserializeFormulaCacheIn(element, serializationContext);
         base.TypedDeserialize(bb, element, serializationContext);
         formulaCache.Each(bb.AddFormula);
      }
   }

   public abstract class BuildingBlockXmlSerializer<TBuildingBlock, TBuilder> : BuildingBlockXmlSerializer<TBuildingBlock>
      where TBuildingBlock : class, IBuildingBlock, IBuildingBlock<TBuilder>
      where TBuilder : class, IBuilder
   {
      protected BuildingBlockXmlSerializer(string name)
         : base(name)
      {
      }

      protected BuildingBlockXmlSerializer()
      {
      }

      public override void PerformMapping()
      {
         base.PerformMapping();
         MapEnumerable(x => x, x => x.Add).WithMappingName(Constants.Serialization.BUILDERS);
      }
   }

   //do not user the generic builder since enumeration returns all containers
   public abstract class SpatialStructureXmlSerializerBase<T> : BuildingBlockXmlSerializer<T> where T : SpatialStructure
   {
      protected SpatialStructureXmlSerializerBase(string name)
         : base(name)
      {
      }

      protected SpatialStructureXmlSerializerBase()
      {
      }

      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.GlobalMoleculeDependentProperties);
         MapEnumerable(x => x.TopContainers, x => x.AddTopContainer).WithMappingName(Constants.Serialization.TOP_CONTAINERS);
         Map(x => x.NeighborhoodsContainer);
      }
   }

   public class SpatialStructureXmlSerializer : SpatialStructureXmlSerializerBase<SpatialStructure>
   {
   }

   public abstract class PathAndValueEntityBuildingBlockXmlSerializer<TBuildingBlock, TStartValue> : BuildingBlockXmlSerializer<TBuildingBlock, TStartValue>
      where TBuildingBlock : PathAndValueEntityBuildingBlock<TStartValue>
      where TStartValue : PathAndValueEntity
   {
   }

   public class PathAndValueEntityBuildingBlockFromPKSimXmlSerializer<TBuildingBlock, TBuilder> : PathAndValueEntityBuildingBlockXmlSerializer<TBuildingBlock, TBuilder>
      where TBuilder : PathAndValueEntity
      where TBuildingBlock : PathAndValueEntityBuildingBlockFromPKSim<TBuilder>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.PKSimVersion);
      }
   }

   public class IndividualBuildingBlockXmlSerializer : PathAndValueEntityBuildingBlockFromPKSimXmlSerializer<IndividualBuildingBlock, IndividualParameter>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         MapEnumerable(x => x.OriginData, x => x.OriginData.Add);
      }
   }

   public class ExpressionProfileBuildingBlockXmlSerializer : PathAndValueEntityBuildingBlockFromPKSimXmlSerializer<ExpressionProfileBuildingBlock, ExpressionParameter>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Type);
         MapEnumerable(x => x.InitialConditions, x => x.AddInitialCondition);
      }
   }

   public class InitialConditionsBuildingBlockXmlSerializer : PathAndValueEntityBuildingBlockXmlSerializer<InitialConditionsBuildingBlock, InitialCondition>
   {
   }

   public class ParameterValuesBuildingBlockXmlSerializer : PathAndValueEntityBuildingBlockXmlSerializer<ParameterValuesBuildingBlock, ParameterValue>
   {
   }

   public class MoleculeBuildingBlockXmlSerializer : BuildingBlockXmlSerializer<MoleculeBuildingBlock, MoleculeBuilder>
   {
   }

   public class ObserverBuildingBlockXmlSerializer : BuildingBlockXmlSerializer<ObserverBuildingBlock, ObserverBuilder>
   {
   }

   public class PassiveTransportBuildingBlockXmlSerializer : BuildingBlockXmlSerializer<PassiveTransportBuildingBlock, TransportBuilder>
   {
   }

   public abstract class ReactionBuildingBlockXmlSerializerBase<T> : BuildingBlockXmlSerializer<T, ReactionBuilder> where T : ReactionBuildingBlock
   {
      protected ReactionBuildingBlockXmlSerializerBase()
      {
      }

      protected ReactionBuildingBlockXmlSerializerBase(string name)
         : base(name)
      {
      }
   }

   public class ReactionBuildingBlockXmlSerializer : ReactionBuildingBlockXmlSerializerBase<ReactionBuildingBlock>
   {
   }

   public class EventGroupBuildingBlockXmlSerializer : BuildingBlockXmlSerializer<EventGroupBuildingBlock, EventGroupBuilder>
   {
   }
}