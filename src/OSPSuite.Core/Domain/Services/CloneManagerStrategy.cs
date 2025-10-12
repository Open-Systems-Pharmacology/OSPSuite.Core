using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public abstract class CloneManagerStrategy : ICloneManager
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly bool _shouldUpdateOriginId;
      private readonly IDataRepositoryTask _dataRepositoryTask;

      protected CloneManagerStrategy(IObjectBaseFactory objectBaseFactory, IDataRepositoryTask dataRepositoryTask, bool shouldUpdateOriginId)
      {
         _objectBaseFactory = objectBaseFactory;
         _shouldUpdateOriginId = shouldUpdateOriginId;
         _dataRepositoryTask = dataRepositoryTask;
      }

      protected T CloneObject<T>(T objectToClone, bool keepId) where T : class, IUpdatable
      {
         if (objectToClone == null)
            return null;

         if (objectToClone is DataRepository repository)
            return cloneDataRepository(repository, keepId) as T;

         if (objectToClone is IFormula formulaToClone)
            return CreateFormulaCloneFor(formulaToClone, keepId).DowncastTo<T>();

         var clone = _objectBaseFactory.CreateObjectBaseFrom(objectToClone);
         copyContainerStructure(objectToClone as IContainer, clone as IContainer, keepId);
         updateUsingFormula(objectToClone as IUsingFormula, clone as IUsingFormula, keepId);
         updateParameter(objectToClone as IParameter, clone as IParameter, keepId);

         //it is necessary to update  the formula before the properties, since UpdatePropertiesFrom might use formula
         clone.UpdatePropertiesFrom(objectToClone, this);

         if (clone is IWithHasChanged withHasChanged)
            withHasChanged.HasChanged = true;

         return withUpdatedId(objectToClone, clone, keepId);
      }

      public virtual T Clone<T>(T objectToClone) where T : class, IUpdatable
      {
         //Standard clone strategy: We overwrite
         return CloneObject(objectToClone, keepId: false);
      }

      private DataRepository cloneDataRepository(DataRepository objectToClone, bool keepId)
      {
         var clone = _dataRepositoryTask.Clone(objectToClone);
         return withUpdatedId(objectToClone, clone, keepId);
      }

      private T withUpdatedId<T>(T objectToClone, T clone, bool keepId)
      {
         if (objectToClone == null || clone == null)
            return clone;

         if (objectToClone is IWithId objectWithId && clone is IWithId cloneWithId && keepId)
            cloneWithId.Id = objectWithId.Id;

         return clone;
      }

      private void updateParameter(IParameter sourceParameter, IParameter targetParameter, bool keepId)
      {
         if (sourceParameter == null || targetParameter == null) return;
         if (sourceParameter.RHSFormula == null) return;
         targetParameter.RHSFormula = CreateFormulaCloneFor(sourceParameter.RHSFormula, keepId);
      }

      private void updateUsingFormula(IUsingFormula sourceUsingFormula, IUsingFormula targetUsingFormula, bool keepId)
      {
         if (sourceUsingFormula == null || targetUsingFormula == null) return;
         var sourceFormula = sourceUsingFormula.Formula;
         if (sourceFormula == null) return;

         targetUsingFormula.Formula = CreateFormulaCloneFor(sourceFormula, keepId);
      }

      private void copyContainerStructure(IContainer sourceContainer, IContainer targetContainer, bool keepId)
      {
         if (sourceContainer == null || targetContainer == null) return;

         foreach (var child in sourceContainer.Children)
         {
            targetContainer.Add(CloneObject(child, keepId));
         }
      }

      private void updateFormulaOrigin(ExplicitFormula sourceFormula, ExplicitFormula targetFormula)
      {
         if (sourceFormula == null || targetFormula == null) return;
         targetFormula.OriginId = sourceFormula.Id;
      }

      protected abstract IFormula CreateFormulaCloneFor(IFormula sourceFormula, bool keepId);

      protected TFormula CloneFormula<TFormula>(TFormula formulaToClone, bool keepId) where TFormula : IFormula
      {
         var clone = _objectBaseFactory.CreateObjectBaseFrom(formulaToClone);

         //it is necessary to update  the formula before the properties, since UpdatePropertiesFrom might use formula
         clone.UpdatePropertiesFrom(formulaToClone, this);

         if (_shouldUpdateOriginId)
            //this has to be done last since update properties from might have updated the value of origin 
            updateFormulaOrigin(formulaToClone as ExplicitFormula, clone as ExplicitFormula);


         return withUpdatedId(formulaToClone, clone, keepId);
      }
   }
}