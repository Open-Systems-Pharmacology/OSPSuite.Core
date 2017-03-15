using System.Xml.Linq;
using OSPSuite.Serializer;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization.Xml.Extensions;

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
      where TBuilder : class, IObjectBase
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
   public abstract class SpatialStructureXmlSerializerBase<T> : BuildingBlockXmlSerializer<T> where T : class, ISpatialStructure
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

   public abstract class StartValuesBuildingBlockXmlSerializer<TBuildingBlock, TStartValue> : BuildingBlockXmlSerializer<TBuildingBlock, TStartValue>
      where TBuildingBlock : class, IStartValuesBuildingBlock<TStartValue>
      where TStartValue : class, IStartValue
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.SpatialStructureId);
         Map(x => x.MoleculeBuildingBlockId);
      }
   }

   public class MoleculeStartValuesBuildingBlockXmlSerializer : StartValuesBuildingBlockXmlSerializer<MoleculeStartValuesBuildingBlock, IMoleculeStartValue>
   {
   }

   public class ParameterStartValuesBuildingBlockXmlSerializer : StartValuesBuildingBlockXmlSerializer<ParameterStartValuesBuildingBlock, IParameterStartValue>
   {
   }

   public class MoleculeBuildingBlockXmlSerializer : BuildingBlockXmlSerializer<MoleculeBuildingBlock, IMoleculeBuilder>
   {
   }

   public class ObserverBuildingBlockXmlSerializer : BuildingBlockXmlSerializer<ObserverBuildingBlock, IObserverBuilder>
   {
   }

   public class PassiveTransportBuildingBlockXmlSerializer : BuildingBlockXmlSerializer<PassiveTransportBuildingBlock, ITransportBuilder>
   {
   }

   public abstract class ReactionBuildingBlockXmlSerializerBase<T> : BuildingBlockXmlSerializer<T, IReactionBuilder> where T : class, IReactionBuildingBlock
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

   public class EventGroupBuildingBlockXmlSerializer : BuildingBlockXmlSerializer<EventGroupBuildingBlock, IEventGroupBuilder>
   {
   }
}