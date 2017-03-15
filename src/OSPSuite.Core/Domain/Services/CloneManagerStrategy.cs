using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;

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

      public virtual T Clone<T>(T objectToClone) where T : class, IUpdatable
      {
         if (objectToClone == null)
            return null;

         var repository = objectToClone as DataRepository;
         if (repository != null)
         {
            return cloneDataRepository(objectToClone.DowncastTo<DataRepository>()) as T;
         }

         var formulaToClone = objectToClone as IFormula;
         if (formulaToClone != null)
            return CreateFormulaCloneFor(formulaToClone).DowncastTo<T>();

         var clone = _objectBaseFactory.CreateObjectBaseFrom(objectToClone);
         copyContainerStructure(objectToClone as IContainer, clone as IContainer);

         updateUsingFormula(objectToClone as IUsingFormula, clone as IUsingFormula);
         updateParameter(objectToClone as IParameter, clone as IParameter);

         //it is necessary to update  the formula before the properties, since UpdatePropertiesFrom might use formula
         clone.UpdatePropertiesFrom(objectToClone, this);

         return clone;
      }

      private DataRepository cloneDataRepository(DataRepository objectToClone)
      {
         return _dataRepositoryTask.Clone(objectToClone);
      }

      private void updateParameter(IParameter sourceParameter, IParameter targetParameter)
      {
         if (sourceParameter == null || targetParameter == null) return;
         if (sourceParameter.RHSFormula == null) return;
         targetParameter.RHSFormula = CreateFormulaCloneFor(sourceParameter.RHSFormula);
      }

      private void updateUsingFormula(IUsingFormula sourceUsingFormula, IUsingFormula targetUsingFormula)
      {
         if (sourceUsingFormula == null || targetUsingFormula == null) return;
         var sourceFormula = sourceUsingFormula.Formula;
         if (sourceFormula == null) return;

         targetUsingFormula.Formula = CreateFormulaCloneFor(sourceFormula);
      }

      private void copyContainerStructure(IContainer sourceContainer, IContainer targetContainer)
      {
         if (sourceContainer == null || targetContainer == null) return;

         foreach (var child in sourceContainer.Children)
         {
            targetContainer.Add(Clone(child));
         }
      }

      private void updateFormulaOrigin(ExplicitFormula sourceFormula, ExplicitFormula targetFormula)
      {
         if (sourceFormula == null || targetFormula == null) return;
         targetFormula.OriginId = sourceFormula.Id;
      }

      protected abstract IFormula CreateFormulaCloneFor(IFormula sourceFormula);

      protected TFormula CloneFormula<TFormula>(TFormula formulaToClone) where TFormula : IFormula
      {
         var clone = _objectBaseFactory.CreateObjectBaseFrom(formulaToClone);

         //it is necessary to update  the formula before the properties, since UpdatePropertiesFrom might use formula
         clone.UpdatePropertiesFrom(formulaToClone, this);

         if (_shouldUpdateOriginId)
            //this has to be done last since update properties from might have updated the value of origin 
            updateFormulaOrigin(formulaToClone as ExplicitFormula, clone as ExplicitFormula);

         return clone;
      }
   }
}