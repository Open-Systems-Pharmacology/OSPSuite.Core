using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OSPSuite.Serializer;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Core.Serialization.Xml
{
   public class SerializationContext : IDisposable
   {
      public IWithIdRepository IdRepository { get; }
      private readonly List<DataRepository> _dataRepositories;
      private readonly DataReferenceCache _dataReferenceCache;
      private readonly ModelReferenceCache _modelReferenceCache;

      public IDimensionFactory DimensionFactory { get; private set; }
      public IObjectBaseFactory ObjectFactory { get; private set; }

      //cache containing an id as key and a corresponding path as value.
      public ICache<int, string> IdStringMap { get; private set; }

      //cache containing a string as key and a correspond id. This is used to reduce the size of generated xml
      public ICache<string, int> StringMap { get; private set; }

      public IFormulaCache Formulas { get; private set; }

      /// <summary>
      ///    Specifies whether the clean should be skipped. This is required to preventing further exception processing. Default
      ///    is false
      /// </summary>
      public bool SkipResolveStep { get; set; }

      //cache containing as key the origin formula id of a formula and as value the corresponding formula id 
      private readonly ICache<string, string> _formulaIdMap = new Cache<string, string>();

      /// <summary>
      /// Convenience access method to IoC container to avoid using globals in code
      /// </summary>
      public IContainer Container { get; }

      public SerializationContext(IDimensionFactory dimensionFactory, IObjectBaseFactory objectFactory,
         IWithIdRepository withIdRepository, IEnumerable<DataRepository> dataRepositories, ICloneManagerForModel cloneManagerForModel, IContainer container)
      {
         Container = container;
         StringMap = new Cache<string, int>();
         IdStringMap = new Cache<int, string>();
         IdRepository = withIdRepository;
         DimensionFactory = dimensionFactory;
         ObjectFactory = objectFactory;
         Formulas = new FormulaCache();
         _modelReferenceCache = new ModelReferenceCache(withIdRepository, cloneManagerForModel);
         _dataReferenceCache = new DataReferenceCache(withIdRepository);
         _dataRepositories = new List<DataRepository>();
         dataRepositories?.Each(AddRepository);
      }

      public DataColumn DataColumnById(string columnId)
      {
         return (from dataRepository in _dataRepositories
            where dataRepository.Contains(columnId)
            select dataRepository[columnId]).FirstOrDefault();
      }

      public void AddRepository(DataRepository repository)
      {
         if (repository == null)
            return;

         if (_dataRepositories.Contains(repository))
            return;

         _dataRepositories.Add(repository);
      }

      public DataColumnCollectionReference AddDataCollectionReference(ICache<AuxiliaryType, DataColumn> cache)
      {
         return _dataReferenceCache.AddCollectionReference(cache);
      }

      public IDimension DimensionByName(string dimensionName)
      {
         return DimensionFactory.Dimension(dimensionName);
      }

      /// <summary>
      ///    returns the id cached for the given string or add the string to the cache and returns the id
      /// </summary>
      public string IdForString(string stringToCache)
      {
         if (!StringMap.Contains(stringToCache))
         {
            int id = StringMap.Count();
            IdStringMap.Add(id, stringToCache);
            StringMap.Add(stringToCache, id);
         }
         return StringMap[stringToCache].ToString(CultureInfo.InvariantCulture);
      }

      /// <summary>
      ///    returns the string saved for the given id. An exception is thrown if the path for the given id could not be
      ///    retrieved
      /// </summary>
      public string StringForId(int stringId)
      {
         return IdStringMap[stringId];
      }

      public IReadOnlyList<DataRepository> Repositories => _dataRepositories;

      private void clear()
      {
         if (!SkipResolveStep)
         {
            _dataReferenceCache.ResolveReferences();
            _modelReferenceCache.ResolveReferences();
         }

         DimensionFactory = null;
         _dataRepositories.Clear();
         _dataReferenceCache.Clear();
         ClearFomulaCache();
      }

      public void ClearFomulaCache()
      {
         Formulas.Clear();
         clearCache();
      }

      private void clearCache()
      {
         _formulaIdMap.Clear();
         StringMap.Clear();
         IdStringMap.Clear();
      }

      public void Dispose()
      {
         clear();
      }

      /// <summary>
      ///    to initialize the serialization FormulaCache of a building block with its FormulaCache (which may contain unused
      ///    formulas)
      /// </summary>
      public void AddFormulasToCache(IEnumerable<IFormula> formulas)
      {
         formulas?.Each(f => AddFormulaToCache(f));
      }

      /// <summary>
      ///    add a formula to the cache and returns the id used for referencing the formula
      /// </summary>
      /// <param name="formula">formula to add to the cache</param>
      public string AddFormulaToCache(IFormula formula)
      {
         if (Formulas == null || formula == null)
            return string.Empty;

         //this is a formula that was not cloned from any source formula
         var explicitFormula = formula as ExplicitFormula;
         if (explicitFormula == null || string.IsNullOrEmpty(explicitFormula.OriginId))
            return addFormulaToCache(formula);

         //in that case, the formula was cloned from an origin formula. We only want to reference the formula once

         //was already added to the cache, return the formula
         if (_formulaIdMap.Contains(explicitFormula.OriginId))
            return _formulaIdMap[explicitFormula.OriginId];

         //in that case, we need to add the formula to the cache and save the formula mapp
         _formulaIdMap.Add(explicitFormula.OriginId, explicitFormula.Id);
         return addFormulaToCache(formula);
      }

      private string addFormulaToCache(IFormula formula)
      {
         if (!Formulas.Contains(formula))
            Formulas.Add(formula);

         return formula.Id;
      }

      public void AddModelReference(object objectToDeserialize, IPropertyMap propertyMap, string referenceValue)
      {
         _modelReferenceCache.Add(objectToDeserialize, propertyMap, referenceValue);
      }

      public void RemoveFormulaFromCache(IFormula formula)
      {
         if (!Formulas.Contains(formula)) return;
         Formulas.Remove(formula);
      }

      public void Register(IWithId withId)
      {
         IdRepository.Register(withId);
      }

      public T Resolve<T>()
      {
         return Container.Resolve<T>();
      }

   }
}