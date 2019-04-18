using OSPSuite.Assets;
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

      protected AbstractClonePresenter(IObjectBaseView view, IObjectTypeResolver objectTypeResolver, IRenameObjectDTOFactory renameObjectDTOFactory) : base(view)
      {
         _objectTypeResolver = objectTypeResolver;
         _renameObjectBaseDTOFactory = renameObjectDTOFactory;
      }

      protected abstract TObjectBase Clone(TObjectBase objectToClone);

      public TObjectBase CreateCloneFor(TObjectBase objectToClone)
      {
         var entityType = _objectTypeResolver.TypeFor(objectToClone);
         _view.Caption = Captions.ParameterIdentification.Clone;
         _view.NameDescription = Captions.CloneObjectBase(entityType, objectToClone.Name);
         _view.Icon = ApplicationIcons.Clone;
         _renameObjectBaseDTO = _renameObjectBaseDTOFactory.CreateFor(objectToClone);
         _renameObjectBaseDTO.AllowSameNameAsOriginalInDifferentCase = false;
         _renameObjectBaseDTO.Description = objectToClone.Description;
         _view.BindTo(_renameObjectBaseDTO);
         _view.Display();

         if (_view.Canceled)
            return null;

         var clonedObject = Clone(objectToClone);
         clonedObject.Name = _renameObjectBaseDTO.Name;
         clonedObject.Description = _renameObjectBaseDTO.Description;

         return clonedObject;
      }
   }
}