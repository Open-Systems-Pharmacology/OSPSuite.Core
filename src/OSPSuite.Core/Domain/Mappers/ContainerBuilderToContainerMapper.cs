using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   /// Maps container used in a building block to model container <para></para>
   /// At the moment, there is no special ContainerBuilder class, so the mapper <para></para>
   /// will just create the clone of the input container
   /// </summary>
   public interface IContainerBuilderToContainerMapper : IBuilderMapper<IContainer, IContainer>
   {
   }

   public class ContainerBuilderToContainerMapper : IContainerBuilderToContainerMapper
   {
      private readonly ICloneManagerForModel _cloneManagerForModel;

      public ContainerBuilderToContainerMapper(ICloneManagerForModel cloneManagerForModel)
      {
         _cloneManagerForModel = cloneManagerForModel;
      }

      public IContainer MapFrom(IContainer containerBuilder, IBuildConfiguration buildConfiguration)
      {
         var container= _cloneManagerForModel.Clone(containerBuilder);
         addBuilderReference(container, containerBuilder, buildConfiguration);
         return container;
      }

      private void addBuilderReference(IContainer container, IContainer containerBuilder, IBuildConfiguration buildConfiguration)
      {
         if (container == null || containerBuilder == null) return;

         buildConfiguration.AddBuilderReference(container, containerBuilder);

         foreach (var childBuilder in containerBuilder.Children)
         {
            var child = container.GetSingleChildByName(childBuilder.Name);
            if(child.IsAnImplementationOf<IContainer>())
               addBuilderReference(child.DowncastTo<IContainer>(), childBuilder as IContainer, buildConfiguration);
            else
               buildConfiguration.AddBuilderReference(child, childBuilder);
         }
      }
   }
}