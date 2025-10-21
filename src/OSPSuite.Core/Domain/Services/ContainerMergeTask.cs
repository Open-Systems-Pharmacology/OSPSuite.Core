using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface IContainerMergeTask
   {
      void MergeContainers(IContainer targetContainer, IContainer containerToMerge);
      IContainer AddOrMergeContainer(IContainer parentContainer, IContainer containerToMerge);

      /// <summary>
      ///    If the entity exists, it will be removed and replace by the new one, otherwise simply added
      /// </summary>
      void AddOrReplaceInContainer(IContainer container, IEntity entityToAddOrReplace);
   }

   public class ContainerMergeTask : IContainerMergeTask
   {
      public IContainer AddOrMergeContainer(IContainer parentContainer, IContainer containerToMerge)
      {
         var existingContainer = parentContainer.Container(containerToMerge.Name);
         if (existingContainer == null)
            parentContainer.Add(containerToMerge);
         else
            MergeContainers(existingContainer, containerToMerge);

         return existingContainer;
      }

      /// <summary>
      ///    If the entity exists, it will be removed and replace by the new one, otherwise simply added
      /// </summary>
      public void AddOrReplaceInContainer(IContainer container, IEntity entityToAddOrReplace)
      {
         var existingChild = container.GetSingleChildByName(entityToAddOrReplace.Name);
         if (existingChild != null)
            container.RemoveChild(existingChild);

         container.Add(entityToAddOrReplace);
      }

      public void MergeContainers(IContainer targetContainer, IContainer containerToMerge)
      {
         updateContainerProperties(targetContainer, containerToMerge);

         var allChildrenContainerToMerge = containerToMerge.GetChildren<IContainer>(x => !x.IsAnImplementationOf<IDistributedParameter>()).ToList();
         var allChildrenEntitiesToMerge = containerToMerge.GetChildren<IEntity>().Except(allChildrenContainerToMerge).ToList();

         //First we do all containers (except distributed parameters which are to be seen as entities) recursively
         allChildrenContainerToMerge.Each(x =>
         {
            var targetChildrenContainer = targetContainer.Container(x.Name);
            //does not exist, we add it
            if (targetChildrenContainer == null)
               AddOrReplaceInContainer(targetContainer, x);
            //it exists, we need to merge
            else
               MergeContainers(targetChildrenContainer, x);
         });

         //then we add or replace all non containers entities
         allChildrenEntitiesToMerge.Each(x => AddOrReplaceInContainer(targetContainer, x));
      }

      private void updateContainerProperties(IContainer targetContainer, IContainer containerToMerge)
      {
         targetContainer.Mode = containerToMerge.Mode;
         containerToMerge.Tags.Each(targetContainer.AddTag);
      }
   }
}