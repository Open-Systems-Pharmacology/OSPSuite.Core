using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface ICloneObjectBasePresenter<TObjectBase> : IObjectBasePresenter
   {
      TObjectBase CreateCloneFor(TObjectBase objectBaseToClone);
   }

   public class CloneObjectBasePresenter<TObjectBase> : AbstractClonePresenter<TObjectBase>, ICloneObjectBasePresenter<TObjectBase> where TObjectBase : class, IObjectBase
   {
      private readonly ICloneManagerForModel _cloneManager;
      public CloneObjectBasePresenter(IObjectBaseView view, IObjectTypeResolver objectTypeResolver, IRenameObjectDTOFactory renameObjectDTOFactory, ICloneManagerForModel cloneManager) :
         base(view, objectTypeResolver, renameObjectDTOFactory)
      {
         _cloneManager = cloneManager;
      }

      protected override TObjectBase Clone(TObjectBase objectBase)
      {
         return _cloneManager.Clone(objectBase);
      }
   }
}