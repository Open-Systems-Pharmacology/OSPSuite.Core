using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Computes a hash of the content of a <see cref="Module" /> or of an <see cref="IBuildingBlock" /> so that two objects
   ///    with identical content can be recognized even when they were created independently (for instance when importing a
   ///    building block that already exists in a project).
   ///    <para>
   ///       The hash is computed from the XML serialization of the object, so that every property that is persisted takes
   ///       part automatically. The serialization is normalized first: the content does not depend on object ids,
   ///       on the name of the hashed object itself, on creation meta data, snapshots, building block version counters,
   ///       on the <see cref="ParameterOrigin" /> of parameter values or on the origin of explicit formulas. Formulas are
   ///       identified by their content, so that it does not matter whether a formula is shared by several objects or
   ///       copied for each of them.
   ///    </para>
   /// </summary>
   public interface IBuildingBlockContentHasher
   {
      /// <summary>
      ///    Returns the hash of the content of the <paramref name="module" />
      /// </summary>
      string ContentHashFor(Module module);

      /// <summary>
      ///    Returns the hash of the content of the <paramref name="buildingBlock" />
      /// </summary>
      string ContentHashFor(IBuildingBlock buildingBlock);

      /// <summary>
      ///    Returns <c>true</c> if <paramref name="module1" /> and <paramref name="module2" /> have the same content, otherwise
      ///    <c>false</c>
      /// </summary>
      bool HaveSameContent(Module module1, Module module2);

      /// <summary>
      ///    Returns <c>true</c> if <paramref name="buildingBlock1" /> and <paramref name="buildingBlock2" /> have the same
      ///    content, otherwise <c>false</c>
      /// </summary>
      bool HaveSameContent(IBuildingBlock buildingBlock1, IBuildingBlock buildingBlock2);
   }

   public class BuildingBlockContentHasher : IBuildingBlockContentHasher
   {
      private readonly IOSPSuiteXmlSerializerRepository _serializerRepository;
      private readonly OSPSuite.Utility.Container.IContainer _container;

      //Elements that are excluded from the content. They carry provenance, not content.
      private static readonly IReadOnlyList<string> _excludedElementNames = new[]
      {
         nameof(IBuildingBlock.Creation),
         nameof(Module.Snapshot),
         nameof(ParameterValue.Origin),
      };

      //Attributes that are excluded from the content wherever they appear: the building block version counter and the
      //origin of an explicit formula (the formula it was cloned from), which is provenance as well
      private static readonly IReadOnlyList<string> _excludedAttributeNames = new[]
      {
         Constants.Serialization.Attribute.BUILDING_BLOCK_VERSION,
         Constants.Serialization.Attribute.ORIGIN,
      };

      //Attributes that are excluded from the content when they appear on the hashed object itself
      private static readonly IReadOnlyList<string> _excludedRootAttributeNames = new[]
      {
         Constants.Serialization.Attribute.NAME,
      };

      public BuildingBlockContentHasher(IOSPSuiteXmlSerializerRepository serializerRepository, OSPSuite.Utility.Container.IContainer container)
      {
         _serializerRepository = serializerRepository;
         _container = container;
      }

      public string ContentHashFor(Module module) => contentHashFor(module);

      public string ContentHashFor(IBuildingBlock buildingBlock) => contentHashFor(buildingBlock);

      public bool HaveSameContent(Module module1, Module module2) => haveSameContent(module1, module2);

      public bool HaveSameContent(IBuildingBlock buildingBlock1, IBuildingBlock buildingBlock2) => haveSameContent(buildingBlock1, buildingBlock2);

      private bool haveSameContent(object object1, object object2)
      {
         if (ReferenceEquals(object1, object2))
            return true;

         if (object1 == null || object2 == null)
            return false;

         return string.Equals(contentHashFor(object1), contentHashFor(object2));
      }

      private string contentHashFor(object objectToHash)
      {
         if (objectToHash == null)
            throw new ArgumentNullException(nameof(objectToHash));

         return hashFor(contentOf(normalizedElementFor(objectToHash)));
      }

      private XElement normalizedElementFor(object objectToHash)
      {
         //A marker that cannot occur in the content, so that an attribute referencing an object can be recognized
         //without any assumption on the format of ids (an id may well be equal to a value such as "0" or "1")
         var referenceMarker = $"{Guid.NewGuid():N}:";
         var element = serialize(objectToHash, referenceMarker);
         removeExcludedContent(element);
         var formulaKeysById = normalizeFormulaCaches(element);
         replaceIds(element, referenceMarker, formulaKeysById);
         return element;
      }

      private XElement serialize(object objectToSerialize, string referenceMarker)
      {
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            serializationContext.ReferenceMarker = referenceMarker;
            //strings are written inline so that the output does not depend on the order in which the strings were encountered
            serializationContext.InlineStrings = true;
            var serializer = _serializerRepository.SerializerFor(objectToSerialize);
            return serializer.Serialize(objectToSerialize, serializationContext);
         }
      }

      private static void removeExcludedContent(XElement rootElement)
      {
         _excludedRootAttributeNames.Each(attributeName => rootElement.Attribute(attributeName)?.Remove());

         rootElement.DescendantsAndSelf()
            .Where(x => _excludedElementNames.Contains(x.Name.LocalName))
            .ToList()
            .Each(x => x.Remove());

         rootElement.DescendantsAndSelf()
            .SelectMany(x => x.Attributes())
            .Where(x => _excludedAttributeNames.Contains(x.Name.LocalName))
            .ToList()
            .Each(x => x.Remove());
      }

      /// <summary>
      ///    A formula is identified by its content and not by its id. A building block loaded from file references a clone
      ///    of a cached formula for every object using it, while the original references the cached formula itself, so that
      ///    neither the identity of a formula nor the number of its copies belong to the content. The id of every formula
      ///    of a formula cache is removed and the formula is identified by its serialization instead. The cache is rewritten
      ///    to hold its distinct formulas sorted by content, so that the order in which the formulas were cached does not
      ///    matter either. Formulas do not reference other objects, so that their serialization is self-contained.
      /// </summary>
      /// <returns>The key identifying a formula by its content, for every formula id</returns>
      private static IReadOnlyDictionary<string, string> normalizeFormulaCaches(XElement rootElement)
      {
         var formulaKeysById = new Dictionary<string, string>();
         var formulasElements = rootElement.Descendants(Constants.Serialization.FORMULAS).ToList();
         foreach (var formulasElement in formulasElements)
         {
            var formulaElements = formulasElement.Elements().ToList();
            foreach (var formulaElement in formulaElements)
            {
               var idAttribute = formulaElement.Attribute(Constants.Serialization.Attribute.ID);
               if (idAttribute == null)
                  continue;

               idAttribute.Remove();
               formulaKeysById[idAttribute.Value] = hashFor(contentOf(formulaElement));
            }

            var distinctFormulas = formulaElements
               .GroupBy(contentOf)
               .OrderBy(x => x.Key, StringComparer.Ordinal)
               .Select(x => x.First())
               .ToList();

            formulasElement.RemoveNodes();
            formulasElement.Add(distinctFormulas);
         }

         return formulaKeysById;
      }

      /// <summary>
      ///    Object ids are generated and do not belong to the content. A reference to a formula is replaced by the key
      ///    identifying the formula by its content. A reference to any other object is replaced, both on the reference and
      ///    on the referenced object, by an ordinal that reflects the order in which the ids are referenced. The structure of
      ///    the references is thereby preserved while the ids themselves are discarded. Ids that are not referenced are
      ///    simply removed, so that it does not matter whether an object has an id or not.
      ///    A reference is recognized by the <paramref name="referenceMarker" /> written in front of the referenced id.
      /// </summary>
      private static void replaceIds(XElement rootElement, string referenceMarker, IReadOnlyDictionary<string, string> formulaKeysById)
      {
         var ordinalsById = new Dictionary<string, string>();

         var referenceAttributes = rootElement.DescendantsAndSelf()
            .SelectMany(x => x.Attributes())
            .Where(x => x.Value.StartsWith(referenceMarker, StringComparison.Ordinal));

         foreach (var referenceAttribute in referenceAttributes)
         {
            var referencedId = referenceAttribute.Value.Substring(referenceMarker.Length);
            if (formulaKeysById.TryGetValue(referencedId, out var formulaKey))
            {
               referenceAttribute.Value = formulaKey;
               continue;
            }

            if (!ordinalsById.TryGetValue(referencedId, out var ordinal))
            {
               ordinal = $"#{ordinalsById.Count}";
               ordinalsById.Add(referencedId, ordinal);
            }

            referenceAttribute.Value = ordinal;
         }

         var idAttributes = rootElement.DescendantsAndSelf()
            .Where(isElementWithObjectId)
            .Select(x => x.Attribute(Constants.Serialization.Attribute.ID))
            .Where(x => x != null)
            .ToList();

         foreach (var idAttribute in idAttributes)
         {
            if (ordinalsById.TryGetValue(idAttribute.Value, out var ordinal))
               idAttribute.Value = ordinal;
            else
               idAttribute.Remove();
         }
      }

      /// <summary>
      ///    An element has an object id if its id attribute carries the id of an <see cref="IWithId" />. The
      ///    <see cref="ValueOrigin" /> uses the id attribute for its database id, which is not an object id.
      /// </summary>
      private static bool isElementWithObjectId(XElement element)
      {
         return !string.Equals(element.Name.LocalName, Constants.Serialization.VALUE_ORIGIN);
      }

      private static string contentOf(XElement element) => element.ToString(SaveOptions.DisableFormatting);

      private static string hashFor(string content)
      {
         using (var sha256 = SHA256.Create())
         {
            return hexStringFor(sha256.ComputeHash(Encoding.UTF8.GetBytes(content)));
         }
      }

      private static string hexStringFor(byte[] bytes)
      {
         var hexString = new StringBuilder(bytes.Length * 2);
         foreach (var b in bytes)
         {
            hexString.Append(b.ToString("x2"));
         }

         return hexString.ToString();
      }
   }
}
