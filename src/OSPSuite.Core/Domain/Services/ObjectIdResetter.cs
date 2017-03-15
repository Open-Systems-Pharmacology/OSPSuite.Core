using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Domain.Services
{
   public interface IObjectIdResetter
   {
      /// <summary>
      ///    Reset the ids of the object with id given as parameter as well as all its descendant
      /// </summary>
      /// <param name="objectWithId"></param>
      void ResetIdFor(object objectWithId);
   }

   public class ObjectIdResetter : IObjectIdResetter,
                                   IVisitor<IWithId>,
                                   IVisitor<IUsingFormula>,
                                   IVisitor<IMoleculeBuilder>,
                                   IVisitor<DataRepository>
   {
      private readonly IIdGenerator _idGenerator;

      public ObjectIdResetter(IIdGenerator idGenerator)
      {
         _idGenerator = idGenerator;
      }

      public virtual void ResetIdFor(object objectWithId)
      {
         var visitable = objectWithId as IVisitable<IVisitor>;
         if (visitable != null)
            visitable.AcceptVisitor(this);
         else
            Visit(objectWithId as IWithId);

         resetFormulaCache(objectWithId);
      }

      private void resetFormulaCache(object objectWithId)
      {
         var buildingBlock = objectWithId as IBuildingBlock;
         if (buildingBlock != null)
            resetFormulaCacheIn(buildingBlock);

         var buildConfiguration = objectWithId as IBuildConfiguration;
         buildConfiguration?.AllBuildingBlocks.Each(resetFormulaCacheIn);
      }

      private void resetFormulaCacheIn(IBuildingBlock buildingBlock)
      {
         buildingBlock.FormulaCache.Refresh();
      }

      public void Visit(IUsingFormula usingFormula)
      {
         if (usingFormula == null) return;
         Visit(usingFormula as IWithId);
         Visit(usingFormula.Formula);
      }

      public void Visit(IMoleculeBuilder moleculeBuilder)
      {
         if (moleculeBuilder == null) return;
         Visit(moleculeBuilder as IWithId);
         Visit(moleculeBuilder.DefaultStartFormula);
      }

      protected void SetIdTo(IWithId objectWithId)
      {
         objectWithId?.WithId(_idGenerator.NewId());
      }

      public void Visit(IWithId withId)
      {
         SetIdTo(withId);
      }

      public void Visit(DataRepository dataRepository)
      {
         Visit(dataRepository as IWithId);
         var allColumns = dataRepository.Columns.ToList();
         allColumns.Each(dataRepository.Remove);
         allColumns.Each(Visit);
         allColumns.Each(dataRepository.Add);
      }
   }
}