using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   ///    Maps container used in a building block to model container
   ///    <para></para>
   ///    At the moment, there is no special ContainerBuilder class, so the mapper
   ///    <para></para>
   ///    will just create the clone of the input container
   /// </summary>
   public interface IContainerBuilderToContainerMapper : IBuilderMapper<IContainer, IContainer>
   {
   }

   internal class ContainerBuilderToContainerMapper : IContainerBuilderToContainerMapper
   {
      private readonly ICloneManagerForModel _cloneManagerForModel;
      private readonly IEntityTracker _entityTracker;

      public ContainerBuilderToContainerMapper(ICloneManagerForModel cloneManagerForModel, IEntityTracker entityTracker)
      {
         _cloneManagerForModel = cloneManagerForModel;
         _entityTracker = entityTracker;
      }

      public IContainer MapFrom(IContainer containerBuilder, SimulationBuilder simulationBuilder)
      {
         var container = _cloneManagerForModel.Clone(containerBuilder);
         addBuilderReference(container, containerBuilder, simulationBuilder);
         return container;
      }

      private void addBuilderReference(IContainer container, IContainer containerBuilder, SimulationBuilder simulationBuilder)
      {
         if (container == null || containerBuilder == null) return;

         _entityTracker.Track(container, containerBuilder, simulationBuilder);
         foreach (var childBuilder in containerBuilder.Children)
         {
            var child = container.GetSingleChildByName(childBuilder.Name);
            if (child.IsAnImplementationOf<IContainer>())
               addBuilderReference(child.DowncastTo<IContainer>(), childBuilder as IContainer, simulationBuilder);
            else
            {
               _entityTracker.Track(child, childBuilder, simulationBuilder);
            }
         }
      }
   }
}