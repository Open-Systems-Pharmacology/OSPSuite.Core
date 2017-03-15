using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters.Classifications
{
   public interface INameClassificationPresenter : IObjectBasePresenter<IClassification>
   {
      bool NewName(IClassification parentClassification);
   }

   public class NameClassificationPresenter : ObjectBasePresenter<IClassification>, INameClassificationPresenter
   {
      private readonly IProjectRetriever _projectRetriever;
      private IClassification _parentClassification;
      private ClassificationType _classificationType;

      public NameClassificationPresenter(IObjectBaseView view, IProjectRetriever projectRetriever)
         : base(view, descriptionVisible: false)
      {
         _projectRetriever = projectRetriever;
      }

      protected override void InitializeResourcesFor(IClassification objectContext)
      {
         _view.Caption = Captions.EnterNameEntityCaption(Captions.Folder);
         _view.NameDescription = Captions.Name;
      }

      protected override ObjectBaseDTO CreateDTOFor(IClassification classification)
      {
         var dto = new ObjectBaseDTO();
         AddExistingClassificationName(dto);
         return dto;
      }

      protected virtual void AddExistingClassificationName(ObjectBaseDTO dto)
      {
         var project = _projectRetriever.CurrentProject;

         var allExistingNames = project.AllClassificationsByType(_classificationType)
            .Where(x => Equals(x.Parent, _parentClassification))
            .Select(x => x.Name);

         dto.AddUsedNames(allExistingNames);
      }

      public bool NewName(IClassification parentClassification)
      {
         return NewNameFor(new Classification { ClassificationType = parentClassification.ClassificationType }, parentClassification);
      }

      protected bool NewNameFor(IClassification classification, IClassification parentClassification)
      {
         try
         {
            //root classification is not saved as reference in project. hence the null here
            _parentClassification = parentClassification.IsAnImplementationOf<RootNodeType>() ? null : parentClassification;
            _classificationType = classification.ClassificationType;
            return Edit(classification);
         }
         finally
         {
            _parentClassification = null;
         }
      }
   }

}