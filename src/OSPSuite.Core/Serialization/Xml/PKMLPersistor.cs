using System;
using System.Xml.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Converters;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Core.Serialization.Xml
{
   public interface IPKMLPersistor
   {
      void SaveToPKML<T>(T entityToSerialize, string fileName);
      string Serialize<T>(T entityToSerialize);

      T Load<T>(string pkmlFileFullPath,
         IDimensionFactory dimensionFactory = null,
         IObjectBaseFactory objectBaseFactory = null,
         IWithIdRepository withIdRepository = null,
         ICloneManagerForModel cloneManagerForModel = null) where T : class;
   }

   public class PKMLPersistor : IPKMLPersistor
   {
      private readonly IOSPSuiteXmlSerializerRepository _serializerRepository;
      private readonly IContainer _container;
      private readonly IReferencesResolver _referencesResolver;
      private readonly IObjectConverterFinder _objectConverterFinder;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly ICloneManagerForModel _cloneManagerForModel;

      public PKMLPersistor(
         IOSPSuiteXmlSerializerRepository serializerRepository,
         IContainer container,
         IReferencesResolver referencesResolver,
         IObjectConverterFinder objectConverterFinder,
         IDimensionFactory dimensionFactory,
         IObjectBaseFactory objectBaseFactory,
         ICloneManagerForModel cloneManagerForModel)
      {
         _serializerRepository = serializerRepository;
         _container = container;
         _referencesResolver = referencesResolver;
         _objectConverterFinder = objectConverterFinder;
         _dimensionFactory = dimensionFactory;
         _objectBaseFactory = objectBaseFactory;
         _cloneManagerForModel = cloneManagerForModel;
      }

      public string Serialize<T>(T entityToSerialize)
      {
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            return xElementFor(entityToSerialize, serializationContext).ToString();
         }
      }

      public void SaveToPKML<T>(T entityToSerialize, string fileName)
      {
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            var xElement = xElementFor(entityToSerialize, serializationContext);
            xElement.PermissiveSave(fileName);
         }
      }

      private XElement xElementFor<T>(T entityToSerialize, SerializationContext serializationContext)
      {
         var xElement = serializeModelPart(entityToSerialize, serializationContext);
         xElement.AddAttribute(Constants.Serialization.Attribute.VERSION, Constants.PKML_VERSION.ToString());
         return xElement;
      }

      public T Load<T>(string pkmlFileFullPath,
         IDimensionFactory dimensionFactory = null,
         IObjectBaseFactory objectBaseFactory = null,
         IWithIdRepository withIdRepository = null,
         ICloneManagerForModel cloneManagerForModel = null) where T : class
      {
         T loadedObject;
         int version;
         using (var serializationContext = SerializationTransaction.Create(
                   _container,
                   dimensionFactory ?? _dimensionFactory,
                   objectBaseFactory ?? _objectBaseFactory,
                   withIdRepository ?? new WithIdRepository(),
                   cloneManagerForModel ?? _cloneManagerForModel))
         {
            var element = XElementSerializer.PermissiveLoad(pkmlFileFullPath);
            version = element.GetPKMLVersion();

            convertXml(element, version);

            var serializer = _serializerRepository.SerializerFor<T>();
            if (!string.Equals(serializer.ElementName, element.Name.LocalName))
               throw new OSPSuiteException(Error.CouldNotLoadObjectFromFile(pkmlFileFullPath, serializer.ElementName));

            loadedObject = serializer.Deserialize<T>(element, serializationContext);
         }

         resolveReferenceIfRequired(loadedObject);
         convert(loadedObject, version, x => x.Convert);

         return loadedObject;
      }

      private void convertXml(XElement sourceElement, int version)
      {
         if (sourceElement == null) return;
         //set version to avoid double conversion in the case of multiple load
         convert(sourceElement, version, x => x.ConvertXml);
         sourceElement.SetAttributeValue(Constants.Serialization.Attribute.VERSION, Constants.PKML_VERSION);
      }

      private void convert<T>(T objectToConvert, int objectVersion, Func<IObjectConverter, Func<T, (int, bool)>> converterAction)
      {
         int version = objectVersion;
         if (version <= PKMLVersion.NON_CONVERTABLE_VERSION)
            throw new OSPSuiteException(Constants.TOO_OLD_PKML);

         while (version != Constants.PKML_VERSION)
         {
            var converter = _objectConverterFinder.FindConverterFor(version);
            var (convertedVersion, _) = converterAction(converter).Invoke(objectToConvert);
            version = convertedVersion;
         }
      }

      private void resolveReferenceIfRequired<T>(T loadedObject)
      {
         switch (loadedObject)
         {
            case Module module:
               resolveReferenceIfRequired(module.SpatialStructure);
               break;
            case SimulationConfiguration simulationConfiguration:
               simulationConfiguration.ModuleConfigurations.Each(x => resolveReferenceIfRequired(x.Module));
               break;
            case IModelCoreSimulation simulation:
               _referencesResolver.ResolveReferencesIn(simulation.Model);
               resolveReferenceIfRequired(simulation.Configuration);
               break;
            case SimulationTransfer simulationTransfer:
               resolveReferenceIfRequired(simulationTransfer.Simulation);
               break;
            case SpatialStructure spatialStructure:
               spatialStructure.ResolveReferencesInNeighborhoods();
               break;
            default:
               return;
         }
      }

      private XElement serializeModelPart<T>(T entityToSerialize, SerializationContext serializationContext)
      {
         var partSerializer = _serializerRepository.SerializerFor(entityToSerialize);
         var xElement = partSerializer.Serialize(entityToSerialize, serializationContext);
         if (needsFormulaCacheSerialization(entityToSerialize))
         {
            var xElementFormulas = serializeFormulas(serializationContext.Formulas, serializationContext);
            xElement.Add(xElementFormulas);
         }

         return xElement;
      }

      private static bool needsFormulaCacheSerialization<T>(T entityToSerialize)
      {
         return !(entityToSerialize.IsAnImplementationOf<IBuildingBlock>() ||
                  entityToSerialize.IsAnImplementationOf<IModelCoreSimulation>() ||
                  entityToSerialize.IsAnImplementationOf<SimulationConfiguration>() ||
                  entityToSerialize.IsAnImplementationOf<IModel>() ||
                  entityToSerialize.IsAnImplementationOf<DataRepository>() ||
                  entityToSerialize.IsAnImplementationOf<ImporterConfiguration>());
      }

      private XElement serializeFormulas(IFormulaCache formulas, SerializationContext serializationContext)
      {
         var serializer = _serializerRepository.SerializerFor(formulas);
         return serializer.Serialize(formulas, serializationContext);
      }
   }
}