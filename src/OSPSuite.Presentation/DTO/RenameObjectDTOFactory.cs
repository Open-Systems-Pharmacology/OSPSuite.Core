using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.DTO
{
   public interface IRenameObjectDTOFactory
   {
      RenameObjectDTO CreateFor(IWithName objectBase);
   }

   public class RenameObjectDTOFactory : IRenameObjectDTOFactory
   {
      private readonly IProjectRetriever _projectRetriever;
      private readonly IObjectTypeResolver _objectTypeResolver;

      public RenameObjectDTOFactory(IProjectRetriever projectRetriever, IObjectTypeResolver objectTypeResolver)
      {
         _projectRetriever = projectRetriever;
         _objectTypeResolver = objectTypeResolver;
      }

      public virtual RenameObjectDTO CreateFor(IWithName objectBase)
      {
         if (objectBase is IParameterAnalysable parameterAnalyzable)
            return CreateFor(parameterAnalyzable);

         var entity = objectBase as IEntity;

         if (entity?.ParentContainer == null)
            return new RenameObjectDTO(objectBase.Name);

         var containerType = _objectTypeResolver.TypeFor(entity.ParentContainer);

         return CreateRenameInContainerDTO(entity, entity.ParentContainer.Children, containerType);
      }

      protected RenameObjectDTO CreateFor(IParameterAnalysable parameterAnalyzable)
      {
         return CreateRenameInProjectDTO(parameterAnalyzable, _projectRetriever.CurrentProject.AllParameterAnalysables.Where(x => x.IsAnImplementationOf(parameterAnalyzable.GetType())));
      }

      protected RenameObjectDTO CreateRenameInProjectDTO(IWithName withName, IEnumerable<IWithName> existingObjects) => CreateRenameInContainerDTO(withName, existingObjects, ObjectTypes.Project);

      protected RenameObjectDTO CreateRenameInContainerDTO(IWithName withName, IEnumerable<IWithName> existingObjects, string containerType)
      {
         var dto = new RenameObjectDTO(withName.Name) {ContainerType = containerType};
         dto.AddUsedNames(existingObjects.AllNames());
         return dto;
      }
   }
}