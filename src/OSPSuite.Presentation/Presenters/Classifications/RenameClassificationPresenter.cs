using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters.Classifications
{
   public interface IRenameClassificationPresenter : IObjectBasePresenter<IClassification>
   {
      bool Rename(IClassification classification);

   }

   public class RenameClassificationPresenter : NameClassificationPresenter, IRenameClassificationPresenter
   {
      private readonly IRenameObjectDTOFactory _renameObjectBaseDTOFactory;

      public RenameClassificationPresenter(IObjectBaseView view, IProjectRetriever projectRetriever, IRenameObjectDTOFactory renameObjectBaseDTOFactory)
         : base(view, projectRetriever)
      {
         _renameObjectBaseDTOFactory = renameObjectBaseDTOFactory;
      }

      protected override void InitializeResourcesFor(IClassification classification)
      {
         _view.Caption = Captions.Rename;
         _view.NameDescription = Captions.RenameEntityCaption(Captions.Folder, classification.Name);
      }

      protected override ObjectBaseDTO CreateDTOFor(IClassification classification)
      {
         var dto = _renameObjectBaseDTOFactory.CreateFor(classification);
         AddExistingClassificationName(dto);
         return dto;
      }

      public bool Rename(IClassification classification)
      {
         return NewNameFor(classification, classification.Parent);
      }
   }

}