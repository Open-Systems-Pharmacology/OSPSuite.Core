using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.Serializer.Xml;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Serialization.Xml.Extensions
{
   public static class XElementExtensions
   {
      public static XElement AddDisplayUnitFor(this XElement element, IWithDisplayUnit withDisplayUnit)
      {
         return AddDisplayUnit(element, withDisplayUnit.DisplayUnit);
      }

      public static XElement AddDisplayUnit(this XElement element, Unit unit)
      {
         if (unit == null || string.IsNullOrEmpty(unit.Name))
            return element;

         element.AddAttribute(Constants.Serialization.Attribute.DISPLAY_UNIT, unit.Name);
         return element;
      }

      public static void UpdateDisplayUnit(this XElement element, IWithDisplayUnit withDisplayUnit)
      {
         withDisplayUnit.DisplayUnit = GetDisplayUnit(element, withDisplayUnit);
      }

      public static Unit GetDisplayUnit(this XElement element, IWithDimension withDimension)
      {
         return GetDisplayUnit(element, withDimension.Dimension);
      }

      public static Unit GetDisplayUnit(this XElement element, IDimension dimension)
      {
         if (dimension == null)
            return null;

         var displayUnit = element.GetAttribute(Constants.Serialization.Attribute.DISPLAY_UNIT);
         return dimension.UnitOrDefault(displayUnit);
      }

      public static IEnumerable<XElement> DescendantsAndSelfNamed(this XElement element, params string[] names)
      {
         return element.DescendantsAndSelf().Where(x => names.Contains(x.Name.ToString()));
      }

       public static XElement CreateSimulationReferenceListElement(this IXmlSerializerRepository<SerializationContext> serializerRepository, ParameterIdentification parameterIdentification)
      {
         return serializerRepository.CreateObjectReferenceListElement(parameterIdentification, x => x.AllSimulations, Constants.Serialization.SIMULATION_LIST, Constants.Serialization.SIMULATION);
      }

      public static XElement CreateObjectReferenceListElement<T>(this IXmlSerializerRepository<SerializationContext> serializerRepository, T objectWithReference, Func<T, IEnumerable<IWithId>> referenceRetriever,
        string referenceElementListName, string referenceElementName)
      {
         var referenceElementList = serializerRepository.CreateElement(referenceElementListName);
         foreach (var reference in referenceRetriever(objectWithReference))
         {
            var referenceElement = serializerRepository.CreateElement(referenceElementName);
            referenceElement.AddAttribute(Constants.Serialization.Attribute.ID, reference.Id);
            referenceElementList.Add(referenceElement);
         }
         return referenceElementList;
      }

      public static void AddReferencedSimulations(this XElement parameterIdentificationElement, ParameterIdentification parameterIdentification, IWithIdRepository withIdRepository, ILazyLoadTask lazyLoadTask) 
      {
         parameterIdentificationElement.AddReferencedObject<ParameterIdentification, ISimulation>(
            parameterIdentification, x => x.AddSimulation, withIdRepository, lazyLoadTask, Constants.Serialization.SIMULATION);
      }


      public static void AddReferencedObject<TObectWithReference, TReference>(this XElement referenceElementList, TObectWithReference objectWithReference, Func<TObectWithReference, Action<TReference>> addAction,
         IWithIdRepository withIdRepository, ILazyLoadTask lazyLoadTask, string referenceElementName) where TReference : class, IWithId
      {
         foreach (var referenceElement in referenceElementList.Descendants(referenceElementName))
         {
            var id = referenceElement.GetAttribute(Constants.Serialization.Attribute.ID);

            //reference might have been deleted...in that case, simply ignore the simulation
            if (!withIdRepository.ContainsObjectWithId(id))
               continue;

            var reference = withIdRepository.Get<TReference>(id);
            var lazyLoadable = reference as ILazyLoadable;
            if (lazyLoadable != null)
               lazyLoadTask?.Load(lazyLoadable);

            addAction(objectWithReference)(reference);
         }
      }
   }
}