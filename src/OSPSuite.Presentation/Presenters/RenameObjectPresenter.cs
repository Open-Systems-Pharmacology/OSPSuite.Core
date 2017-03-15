using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IRenameObjectPresenter : IObjectBasePresenter<IWithName>
   {
      string NewNameFrom(IWithName namedObject, IEnumerable<string> forbiddenNames, string entityType = null);
   }

   public class RenameObjectPresenter : ObjectBasePresenter<IWithName>, IRenameObjectPresenter
   {
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IRenameObjectDTOFactory _renameObjectDTOFactory;
      private readonly List<string> _forbiddenNames;
      private string _entityType;

      public RenameObjectPresenter(IObjectBaseView view, IObjectTypeResolver objectTypeResolver, IRenameObjectDTOFactory renameObjectDTOFactory)
         : base(view, descriptionVisible: false)
      {
         _objectTypeResolver = objectTypeResolver;
         _renameObjectDTOFactory = renameObjectDTOFactory;
         _forbiddenNames = new List<string>();
      }

      protected override void InitializeResourcesFor(IWithName namedObject)
      {
         string entityType = _entityType ?? _objectTypeResolver.TypeFor(namedObject);
         _view.Caption = Captions.Rename;
         _view.NameDescription = Captions.RenameEntityCaption(entityType, namedObject.Name);
      }

      protected override ObjectBaseDTO CreateDTOFor(IWithName objectBase)
      {
         var dto = _renameObjectDTOFactory.CreateFor(objectBase);
         dto.AddUsedNames(_forbiddenNames);
         return dto;
      }

      public string NewNameFrom(IWithName namedObject, IEnumerable<string> forbiddenNames = null, string entityType = null)
      {
         _entityType = entityType ?? _objectTypeResolver.TypeFor(namedObject);
         if (forbiddenNames != null)
            _forbiddenNames.AddRange(forbiddenNames);

         return Edit(namedObject) ? Name : string.Empty;
      }
   }
}