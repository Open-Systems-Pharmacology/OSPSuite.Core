using System.Collections.Generic;
using OSPSuite.Serializer;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Serialization
{
   public class ModelReferenceCache
   {
      private readonly List<Reference> _references;
      private readonly IWithIdRepository _withIdRepository;
      private readonly ICloneManagerForModel _cloneManagerForModel;

      public ModelReferenceCache(IWithIdRepository withIdRepository, ICloneManagerForModel cloneManagerForModel)
      {
         _withIdRepository = withIdRepository;
         _cloneManagerForModel = cloneManagerForModel;
         _references = new List<Reference>();
      }

      public void Add(object obj, IPropertyMap propertyMap, string id)
      {
         _references.Add(new Reference(obj, propertyMap, id));
      }

      private void setReference(Reference reference)
      {
         if (reference.AlreadyResolved) return;
         var referenceValue = _withIdRepository.Get<IWithId>(reference.Id);
         if (referenceNeedsToBeCloned(referenceValue))
         {
            var formula = referenceValue.DowncastTo<ExplicitFormula>();
            referenceValue = _cloneManagerForModel.Clone(formula);
            _withIdRepository.Register(referenceValue);
         }

         reference.SetReference(referenceValue);
      }

      private bool referenceNeedsToBeCloned(IWithId referenceValue)
      {
         var formula = referenceValue as ExplicitFormula;
         return formula != null && !string.IsNullOrEmpty(formula.OriginId);
      }

      public void ResolveReferences()
      {
         _references.Each(setReference);
      }

      public void Clear()
      {
         _references.Clear();
      }

      private class Reference
      {
         private readonly object _objectWithReference;
         private readonly IPropertyMap _property;
         public string Id { get; private set; }

         public Reference(object objectWithReference, IPropertyMap propertyMap, string id)
         {
            _objectWithReference = objectWithReference;
            _property = propertyMap;
            Id = id;
         }

         public bool AlreadyResolved
         {
            get { return _property.ResolveValue(_objectWithReference) != null; }
         }

         public void SetReference(object referenceValue)
         {
            _property.SetValue(_objectWithReference, referenceValue);
         }
      }
   }
}