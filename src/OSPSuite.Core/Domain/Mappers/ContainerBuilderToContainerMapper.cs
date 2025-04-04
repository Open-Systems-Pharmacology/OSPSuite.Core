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
      private readonly IObjectTracker _objectTracker;

      public ContainerBuilderToContainerMapper(ICloneManagerForModel cloneManagerForModel, IObjectTracker objectTracker)
      {
         _cloneManagerForModel = cloneManagerForModel;
         _objectTracker = objectTracker;
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

         simulationBuilder.AddBuilderReference(container, containerBuilder);
         _objectTracker.TrackObject(container, containerBuilder, simulationBuilder);
         foreach (var childBuilder in containerBuilder.Children)
         {
            var child = container.GetSingleChildByName(childBuilder.Name);
            if (child.IsAnImplementationOf<IContainer>())
               addBuilderReference(child.DowncastTo<IContainer>(), childBuilder as IContainer, simulationBuilder);
            else
            {
               simulationBuilder.AddBuilderReference(child, childBuilder);
               _objectTracker.TrackObject(child, childBuilder, simulationBuilder);
            }
         }
      }
   }
}