using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public abstract class AbstractClonePresenter<TObjectBase> : AbstractDisposablePresenter<IObjectBaseView, IObjectBasePresenter> where TObjectBase : class, IObjectBase
   {
      private readonly IRenameObjectDTOFactory _renameObjectBaseDTOFactory;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private RenameObjectDTO _renameObjectBaseDTO;
      private readonly IOSPSuiteExecutionContext _executionContext;

      protected AbstractClonePresenter(
         IObjectBaseView view, 
         IObjectTypeResolver objectTypeResolver, 
         IRenameObjectDTOFactory renameObjectDTOFactory, 
         IOSPSuiteExecutionContext executionContext) : base(view)
      {
         _objectTypeResolver = objectTypeResolver;
         _renameObjectBaseDTOFactory = renameObjectDTOFactory;
         _executionContext = executionContext;
      }

      protected abstract TObjectBase Clone(TObjectBase objectToClone);

      public TObjectBase CreateCloneFor(TObjectBase objectToClone)
      {
         var entityType = _objectTypeResolver.TypeFor(objectToClone);
         _view.Caption = Captions.ParameterIdentification.Clone;
         _view.NameDescription = Captions.CloneObjectBase(entityType, objectToClone.Name);
         _view.Image = ApplicationIcons.Clone;
         _renameObjectBaseDTO = _renameObjectBaseDTOFactory.CreateFor(objectToClone);
         _renameObjectBaseDTO.AllowSameNameAsOriginalInDifferentCase = false;
         _renameObjectBaseDTO.Description = objectToClone.Description;
         _view.BindTo(_renameObjectBaseDTO);
         _view.Display();

         if (_view.Canceled)
            return null;

         _executionContext.Load(objectToClone);

         var clonedObject = Clone(objectToClone);
         clonedObject.Name = _renameObjectBaseDTO.Name;
         clonedObject.Description = _renameObjectBaseDTO.Description;

         return clonedObject;
      }
   }
}