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
      /// Clones the <paramref name="model "/> and ensure that all references are finalized
      /// </summary>
      IModel CloneModel(IModel model);
   }

   public class CloneManagerForModel : CloneManagerStrategy, ICloneManagerForModel
   {
      private readonly IModelFinalizer _modelFinalizer;

      public CloneManagerForModel(IObjectBaseFactory objectBaseFactory, IDataRepositoryTask dataRepositoryTask, IModelFinalizer modelFinalizer) : base(objectBaseFactory, dataRepositoryTask, shouldUpdateOriginId: true)
      {
         _modelFinalizer = modelFinalizer;
      }

      /// <summary>
      ///    Creates clone of the given formula.
      /// </summary>
      /// <param name="sourceFormula">Formula to be cloned</param>
      /// <returns>Clone (deep copy) of the formula</returns>
      protected override IFormula CreateFormulaCloneFor(IFormula sourceFormula)
      {
         return CloneFormula(sourceFormula);
      }

      public IModel CloneModel(IModel model)
      {
         var cloneModel = Clone(model);
         _modelFinalizer.FinalizeClone(cloneModel, model);
         return cloneModel;
      }
   }
}