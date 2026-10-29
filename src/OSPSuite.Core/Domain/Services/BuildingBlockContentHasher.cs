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
   ///       on the name of the hashed object itself, on creation meta data, snapshots, building block version counters or
   ///       on the <see cref="ParameterOrigin" /> of parameter values.
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

      //Attributes that are excluded from the content wherever they appear
      private static readonly IReadOnlyList<string> _excludedAttributeNames = new[]
      {
         Constants.Serialization.Attribute.BUILDING_BLOCK_VERSION,
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

         var element = normalizedElementFor(objectToHash);
         var content = Encoding.UTF8.GetBytes(element.ToString(SaveOptions.DisableFormatting));
         using (var sha256 = SHA256.Create())
         {
            return hexStringFor(sha256.ComputeHash(content));
         }
      }

      private XElement normalizedElementFor(object objectToHash)
      {
         var element = serialize(objectToHash);
         removeExcludedContent(element);
         replaceIdsWithOrdinals(element);
         return element;
      }

      private XElement serialize(object objectToSerialize)
      {
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
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
      ///    Object ids are generated and do not belong to the content. An id that is referenced somewhere in the
      ///    serialization (for instance the reference from a parameter to its formula in the formula cache) is replaced,
      ///    both on the reference and on the referenced object, by an ordinal that reflects the order in which the ids are
      ///    referenced. The structure of the references is thereby preserved while the ids themselves are discarded. Ids that
      ///    are not referenced are simply removed, so that it does not matter whether an object has an id or not.
      /// </summary>
      private static void replaceIdsWithOrdinals(XElement rootElement)
      {
         var idAttributes = rootElement.DescendantsAndSelf()
            .Where(isElementWithObjectId)
            .Select(x => x.Attribute(Constants.Serialization.Attribute.ID))
            .Where(x => x != null)
            .ToList();

         var objectIds = new HashSet<string>(idAttributes.Select(x => x.Value));
         var ordinalsById = new Dictionary<string, string>();

         var referenceAttributes = rootElement.DescendantsAndSelf()
            .SelectMany(x => x.Attributes())
            .Where(x => !string.Equals(x.Name.LocalName, Constants.Serialization.Attribute.ID))
            .Where(x => objectIds.Contains(x.Value));

         foreach (var referenceAttribute in referenceAttributes)
         {
            if (!ordinalsById.TryGetValue(referenceAttribute.Value, out var ordinal))
            {
               ordinal = $"#{ordinalsById.Count}";
               ordinalsById.Add(referenceAttribute.Value, ordinal);
            }

            referenceAttribute.Value = ordinal;
         }

         foreach (var idAttribute in idAttributes)
         {
            if (ordinalsById.TryGetValue(idAttribute.Value, out var ordinal))
               idAttribute.Value = ordinal;
            else
               idAttribute.Remove();
         }
      }

      /// <summary>
      ///    An element has an object id if its id attribute carries the id of an <see cref="IWithId" />. The string map of
      ///    the formula cache uses the id attribute for the ordinal of a cached string and the <see cref="ValueOrigin" /> for
      ///    its database id. Neither is an object id.
      /// </summary>
      private static bool isElementWithObjectId(XElement element)
      {
         if (string.Equals(element.Name.LocalName, Constants.Serialization.VALUE_ORIGIN))
            return false;

         var isStringMapEntry = string.Equals(element.Name.LocalName, Constants.Serialization.STRING_MAP) &&
                                string.Equals(element.Parent?.Name.LocalName, Constants.Serialization.STRING_MAP_LIST);

         return !isStringMapEntry;
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
