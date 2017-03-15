using System;
using System.Collections.Generic;
using OSPSuite.Utility;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Services
{
   public class KeyPathMap
   {
      public string Path { get; private set; }
      public string Molecule { get; private set; }

      public KeyPathMap(string path = null, string molecule = null)
      {
         Path = path ?? string.Empty;
         Molecule = molecule ?? string.Empty;
      }
   }

   public interface IKeyPathMapper :
      IMapper<DataColumn, string>,
      IMapper<IQuantity, string>,
      IMapper<QuantitySelection, string>,
      IMapper<string, string>
   {
      /// <summary>
      ///    Maps the <paramref name="quantityPath" /> to a key using the provided <paramref name="quantityType" /> to indentify
      ///    the strategy to use.
      ///    If <paramref name="removeFirstEntry" /> is <c>true</c>, the first path entry will be removed (assumed to be a full
      ///    path in a simulation).
      ///    Otherwise it will be left as is
      /// </summary>
      /// <param name="quantityPath">Full path based on which the key will be created and returned</param>
      /// <param name="quantityType">Quantity type associated to the quantity path</param>
      /// <param name="removeFirstEntry">Specifies if the first entry of the path should be removed or not</param>
      /// <returns></returns>
      KeyPathMap MapFrom(string quantityPath, QuantityType quantityType, bool removeFirstEntry);

      string MoleculeNameFrom(DataColumn dataColumn);
   }

   public class KeyPathMapper : IKeyPathMapper
   {
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly IObjectPathFactory _objectPathFactory;

      public KeyPathMapper(IEntityPathResolver entityPathResolver, IObjectPathFactory objectPathFactory)
      {
         _entityPathResolver = entityPathResolver;
         _objectPathFactory = objectPathFactory;
      }

      public string MoleculeNameFrom(DataColumn dataColumn)
      {
         return calculationKeyFor(dataColumn).Molecule;
      }

      public string MapFrom(DataColumn dataColumn)
      {
         return calculationKeyFor(dataColumn).Path;
      }

      private KeyPathMap calculationKeyFor(DataColumn dataColumn)
      {
         if (dataColumn == null)
            return new KeyPathMap();

         var quantityInfo = dataColumn.QuantityInfo;
         var dataInfo = dataColumn.DataInfo;

         if (quantityInfo == null || dataInfo == null)
            return new KeyPathMap(path: dataColumn.Name);

         switch (dataInfo.Origin)
         {
            case ColumnOrigins.Undefined:
            case ColumnOrigins.Observation:
            case ColumnOrigins.ObservationAuxiliary:
               return new KeyPathMap(path: _objectPathFactory.CreateFormulaUsablePathFrom(dataInfo.Source, dataColumn.Name).ToString());
            case ColumnOrigins.BaseGrid:
               return new KeyPathMap(path: quantityInfo.PathAsString);
            case ColumnOrigins.Calculation:
            case ColumnOrigins.CalculationAuxiliary:
               return calculationKeyFor(quantityInfo);
            default:
               throw new ArgumentOutOfRangeException();
         }
      }

      private KeyPathMap calculationKeyFor(IEnumerable<string> quantityPath, QuantityType quantityType, bool removeFirstEntry)
      {
         var path = new List<string>(quantityPath);
         var moleculeName = string.Empty;

         if (removeFirstEntry && path.Count > 1)
            path.RemoveAt(0);

         //path represents a molecule amount. Name of molecule is last entry
         if (path.Count >= 1 && quantityTypeIsMoleculeAmount(quantityType))
         {
            moleculeName = path[path.Count - 1];
            path.RemoveAt(path.Count - 1);
         }

         //if the path contains at least 2 elements and represents a molecule remove 
         //the one before last entry corresponding to the molecule name
         else if (path.Count >= 2 && quantityTypeIsMoleculeObserver(quantityType))
         {
            moleculeName = path[path.Count - 2];
            path.RemoveAt(path.Count - 2);
         }

         return new KeyPathMap(path.ToPathString(), moleculeName);
      }

      private bool quantityTypeIsMoleculeAmount(QuantityType quantityType)
      {
         return quantityType.Is(QuantityType.Molecule) && !quantityType.Is(QuantityType.Observer) && !quantityType.Is(QuantityType.Parameter);
      }

      private bool quantityTypeIsMoleculeObserver(QuantityType quantityType)
      {
         return quantityType.Is(QuantityType.Molecule);
      }

      private KeyPathMap calculationKeyFor(QuantityInfo quantityInfo)
      {
         return calculationKeyFor(quantityInfo.Path, quantityInfo.Type, removeFirstEntry: true);
      }

      public string MapFrom(IQuantity quantity)
      {
         return MapFrom(_entityPathResolver.PathFor(quantity), quantity.QuantityType, removeFirstEntry: true).Path;
      }

      public KeyPathMap MapFrom(string quantityPath, QuantityType quantityType, bool removeFirstEntry)
      {
         return calculationKeyFor(quantityPath.ToPathArray(), quantityType, removeFirstEntry);
      }

      public string MapFrom(QuantitySelection quantitySelection)
      {
         return MapFrom(quantitySelection.Path, quantitySelection.QuantityType, removeFirstEntry: true).Path;
      }

      public string MapFrom(string path)
      {
         return MapFrom(path, QuantityType.Drug, removeFirstEntry: true).Path;
      }
   }
}