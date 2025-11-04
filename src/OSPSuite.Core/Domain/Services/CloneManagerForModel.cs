using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Clone manager, which creates a NEW copy of any formula
   ///    <para></para>
   ///    EVERY TIME the formula is referenced
   ///    <para></para>
   ///    by the object to be cloned or any of its subobjects.
   ///    <para></para>
   ///    Can be used for:
   ///    <para></para>
   ///    - Creating model objects from Builder objects
   ///    - Cloning models
   /// </summary>
   public interface ICloneManagerForModel : ICloneManager
   {
      /// <summary>
      ///    Clones the <paramref name="model " /> and ensure that all references are finalized
      /// </summary>
      IModel CloneModel(IModel model);

      /// <summary>
      ///    Special clone method that will keep the id of the object being cloned as well as all children implementing IWithId
      /// </summary>
      /// <returns>A clone of the object with all id kept.</returns>
      /// <remarks>This method should be used with care as it can create issues if the object is added to a repository</remarks>
      T CloneAndKeepId<T>(T objectToClone) where T : class, IUpdatable;
   }

   public class CloneManagerForModel : CloneManagerStrategy, ICloneManagerForModel
   {
      private readonly IModelFinalizer _modelFinalizer;

      public CloneManagerForModel(IObjectBaseFactory objectBaseFactory, IDataRepositoryTask dataRepositoryTask, IModelFinalizer modelFinalizer) : base(objectBaseFactory, dataRepositoryTask, shouldUpdateOriginId: true)
      {
         _modelFinalizer = modelFinalizer;
      }

      protected override IFormula CreateFormulaCloneFor(IFormula sourceFormula, bool keepId) => CloneFormula(sourceFormula, keepId);

      public IModel CloneModel(IModel model)
      {
         var cloneModel = Clone(model);
         _modelFinalizer.FinalizeClone(cloneModel, model);
         return cloneModel;
      }

      public T CloneAndKeepId<T>(T objectToClone) where T : class, IUpdatable
      {
         //special case for transient object where we want to keep id to ensure we can match even if references are not the same
         return CloneObject(objectToClone, keepId: true);
      }
   }
}