using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    A Building block, a partial information for model building, that can be
   ///    modified or replaced without harming other model parts
   /// </summary>
   public interface IBuildingBlock : IObjectBase, IWithCreationMetaData
   {
      /// <summary>
      ///    Gets or sets the formula cache for the building block. This contains
      ///    the formulas used in the block and some mainly used their
      /// </summary>
      /// <value>The formula cache.</value>
      IFormulaCache FormulaCache { get; }

      uint Version { set; get; }

      void AddFormula(IFormula formula);
   }

   /// <summary>
   ///    A Building block, a partial information for model building, that can be
   ///    modified or replaced without harming other model parts
   /// </summary>
   public abstract class BuildingBlock : ObjectBase, IBuildingBlock
   {
      public IFormulaCache FormulaCache { get; }
      public uint Version { set; get; }
      public CreationMetaData Creation { get; set; }

      public void AddFormula(IFormula formula)
      {
         FormulaCache.Add(formula);
      }

      protected BuildingBlock()
      {
         FormulaCache = new BuildingBlockFormulaCache();
         Creation = new CreationMetaData();
         Version = 0;
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         FormulaCache.Each(formula => formula.AcceptVisitor(visitor));
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceBuildingBlock = source as IBuildingBlock;
         if (sourceBuildingBlock == null) return;
         Version = sourceBuildingBlock.Version;
         Creation = sourceBuildingBlock.Creation.Clone();
      }
   }

   public interface IBuildingBlock<TBuilder> : IBuildingBlock, IEnumerable<TBuilder> where TBuilder : class, IObjectBase
   {
      /// <summary>
      ///    Adds the specified builder.
      /// </summary>
      /// <param name="builderToAdd">The builder to add.</param>
      void Add(TBuilder builderToAdd);

      /// <summary>
      ///    Removes the specified builder.
      /// </summary>
      /// <param name="builderToRemove">The builder to remove.</param>
      void Remove(TBuilder builderToRemove);
   }

   /// <summary>
   ///    A Building block, a partial information for model building, that can be
   ///    modified or replaced without harming other model parts.
   ///    This type of building block mainly is a collection of TBuilder
   ///    each of them containing the information to build a modelObject
   /// </summary>
   /// <typeparam name="TBuilder">The type of the builder.</typeparam>
   public abstract class BuildingBlock<TBuilder> : BuildingBlock, IBuildingBlock<TBuilder> where TBuilder : class, IObjectBase
   {
      private readonly IList<TBuilder> _allElements;

      protected BuildingBlock() : this(new List<TBuilder>())
      {
      }

      protected BuildingBlock(IEnumerable<TBuilder> builders)
      {
         _allElements = builders.ToList();
      }

      public void Add(TBuilder builderToAdd)
      {
         _allElements.Add(builderToAdd);
      }

      public void Remove(TBuilder builderToRemove)
      {
         _allElements.Remove(builderToRemove);
      }

      /// <summary>
      ///    Returns an enumerator that iterates through the collection.
      /// </summary>
      /// <returns>
      ///    A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
      /// </returns>
      public IEnumerator<TBuilder> GetEnumerator()
      {
         return _allElements.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceBuildingBlock = source as IBuildingBlock<TBuilder>;
         if (sourceBuildingBlock == null) return;
         sourceBuildingBlock.Each(builder => Add(cloneManager.Clone(builder)));
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         _allElements.Each(e => e.AcceptVisitor(visitor));
      }
   }
}