using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IEditDescriptionPresenter : IObjectBasePresenter<IObjectBase>
   {
   }

   public class EditDescriptionPresenter : ObjectBasePresenter<IObjectBase>, IEditDescriptionPresenter
   {
      private readonly IObjectTypeResolver _objectTypeResolver;

      public EditDescriptionPresenter(IObjectBaseView view, IObjectTypeResolver objectTypeResolver)
         : base(view, descriptionVisible: true, nameVisible: false)
      {
         _objectTypeResolver = objectTypeResolver;
      }

      protected override void InitializeResourcesFor(IObjectBase entity)
      {
         string entityType = _objectTypeResolver.TypeFor(entity);
         _view.Caption = Captions.EditDescription;
         _view.NameDescription = Captions.RenameEntityCaption(entityType, entity.Name);
         _view.Icon = ApplicationIcons.Description;
      }

      protected override ObjectBaseDTO CreateDTOFor(IObjectBase entity)
      {
         return new ObjectBaseDTO { DescriptionRequired = true, Name = entity.Name, Description = entity.Description };
      }
   }
}