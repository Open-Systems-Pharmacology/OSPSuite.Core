using System;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Clone manager, which checks for any formula of the object to be cloned
   ///    <para></para>
   ///    or any of its subobjects, if the formula is already present in the passed
   ///    <para></para>
   ///    formula cache and creates a new copy only if not.
   ///    <para></para>
   ///    Can be used for:
   ///    <para></para>
   ///    - Cloning of building blocks
   ///    <para></para>
   ///    - Cloning of parts of building blocks into another or same building block
   /// </summary>
   public interface ICloneManagerForBuildingBlock : ICloneManager
   {
      /// <summary>
      ///    Creates a clone of the passed object and inserts only NEW formulas into the given fomula cache.
      /// </summary>
      /// <typeparam name="T">Any class inherited from <see cref="ObjectBase" /></typeparam>
      /// <param name="objectToClone">Source object to be cloned</param>
      /// <param name="formulaCache">Formula cache of the target building block (where the cloned object should be inserted) </param>
      /// <returns>Cloned object</returns>
      T Clone<T>(T objectToClone, IFormulaCache formulaCache) where T : class, IObjectBase;

      /// <summary>
      ///    The formula cache should be set when many objects should be cloned in the context of a single cache.
      ///    For instance in PKSim, FormulaCache will be set once when creating the event building block. Then only the clone
      ///    function
      ///    needs to be used.
      /// </summary>
      IFormulaCache FormulaCache { get; set; }

      /// <summary>
      /// Clones the given building block adding all cloned formulas into the <see cref="FormulaCache"/> of the clone.
      /// </summary>
      /// <typeparam name="T">Type of building block to clone</typeparam>
      /// <param name="buildingBlock">The building block to clone</param>
      /// <returns>The cloned building block</returns>
      T CloneBuildingBlock<T>(T buildingBlock) where T : class, IBuildingBlock;
   }

   public class CloneManagerForBuildingBlock : CloneManagerStrategy, ICloneManagerForBuildingBlock
   {
      private IFormulaCache _formulaCache;

      private readonly ICache<string, IFormula> _clonedFormulasByOriginalFormulaId;

      public CloneManagerForBuildingBlock(IObjectBaseFactory objectBaseFactory, IDataRepositoryTask dataRepositoryTask)
         : base(objectBaseFactory, dataRepositoryTask, shouldUpdateOriginId: false)
      {
         _clonedFormulasByOriginalFormulaId = new Cache<string, IFormula>();
      }

      public T Clone<T>(T objectToClone, IFormulaCache formulaCache) where T : class, IObjectBase
      {
         if (formulaCache == null)
            throw new ArgumentNullException(Error.NullFormulaCachePassedToClone);

         FormulaCache = formulaCache;

         return Clone(objectToClone);
      }

      public IFormulaCache FormulaCache
      {
         get { return _formulaCache; }
         set
         {
            if (Equals(value, _formulaCache))
               return;

            _formulaCache = value;
            _clonedFormulasByOriginalFormulaId.Clear();
         }
      }

      public T CloneBuildingBlock<T>(T buildingBlock) where T : class, IBuildingBlock
      {
         var formulaCache = new FormulaCache();
         var clone = Clone(buildingBlock, formulaCache);
         formulaCache.Each(clone.AddFormula);
         return clone;
      }

      protected override IFormula CreateFormulaCloneFor(IFormula srcFormula)
      {
         // constant formulas are always cloned
         if (srcFormula.IsConstant())
            return CloneFormula(srcFormula);

         // Check if target formula cache already contains the formula
         // to be cloned. This can happen if we are cloning
         // any substructure within the same building block
         // In this case, all formulas are already present in the
         // formula cache of this building block
         if (_formulaCache.Contains(srcFormula.Id))
            return srcFormula;

         // Check if the formula was already cloned. Return the cloned
         // formula if so.
         if (_clonedFormulasByOriginalFormulaId.Contains(srcFormula.Id))
            return _clonedFormulasByOriginalFormulaId[srcFormula.Id];

         // Formula is neither present in the passed formula cahce nor
         // was already cloned. So create a new clone and insert it 
         // in both target formula cache and the cache of all cloned formulas
         var clonedFormula = CloneFormula(srcFormula);
         _formulaCache.Add(clonedFormula);

         _clonedFormulasByOriginalFormulaId.Add(srcFormula.Id, clonedFormula);

         return clonedFormula;
      }
   }
}