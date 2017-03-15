using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   ///    Maps collection of ParameterBuilder-Objects into Collection of (Model-)parameters
   /// </summary>
   public interface IParameterBuilderCollectionToParameterCollectionMapper
   {
      /// <summary>
      ///    Only parameters having one of <paramref name="parameterBuildModesToMap" /> will be mapped or all if <paramref name="parameterBuildModesToMap" /> was not specified
      /// </summary>
      IEnumerable<IParameter> MapFrom(IEnumerable<IParameter> parameterBuilders, IBuildConfiguration buildConfiguration, params ParameterBuildMode[] parameterBuildModesToMap);

      /// <summary>
      ///    Only direct children parameters  of <paramref name="container"/> having one of <paramref name="parameterBuildModesToMap" /> will be mapped or all if <paramref name="parameterBuildModesToMap" /> was not specified
      /// </summary>
      IEnumerable<IParameter> MapFrom(IContainer container, IBuildConfiguration buildConfiguration, params ParameterBuildMode[] parameterBuildModesToMap);

      /// <summary>
      ///    Only direct children parameters  of <paramref name="container"/> of type <see cref="ParameterBuildMode.Global"/> or <seealso cref="ParameterBuildMode.Property"/> will be mapped
      /// </summary>
      IEnumerable<IParameter> MapGlobalOrPropertyFrom(IContainer container, IBuildConfiguration buildConfiguration);

      /// <summary>
      ///    Only direct children parameters  of <paramref name="container"/> of type <see cref="ParameterBuildMode.Local"/> will be mapped
      /// </summary>
      IEnumerable<IParameter> MapLocalFrom(IContainer container, IBuildConfiguration buildConfiguration);

      /// <summary>
      ///    All direct children parameters  of <paramref name="container"/>  will be mapped
      /// </summary>
      IEnumerable<IParameter> MapAllFrom(IContainer container, IBuildConfiguration buildConfiguration);
   }

   public class ParameterBuilderCollectionToParameterCollectionMapper : IParameterBuilderCollectionToParameterCollectionMapper
   {
      private readonly IParameterBuilderToParameterMapper _parameterMapper;

      public ParameterBuilderCollectionToParameterCollectionMapper(IParameterBuilderToParameterMapper parameterMapper)
      {
         _parameterMapper = parameterMapper;
      }

      public IEnumerable<IParameter> MapFrom(IEnumerable<IParameter> parameterBuilders, IBuildConfiguration buildConfiguration, params ParameterBuildMode[] parameterBuildModesToMap)
      {
         return parameterBuilders.Where(p => canBeMapped(p, parameterBuildModesToMap))
            .Select(parameterBuilder => _parameterMapper.MapFrom(parameterBuilder, buildConfiguration)).ToList();
      }

      public IEnumerable<IParameter> MapFrom(IContainer container, IBuildConfiguration buildConfiguration, params ParameterBuildMode[] parameterBuildModesToMap)
      {
         return MapFrom(container.GetChildren<IParameter>(), buildConfiguration, parameterBuildModesToMap);
      }

      public IEnumerable<IParameter> MapGlobalOrPropertyFrom(IContainer container, IBuildConfiguration buildConfiguration)
      {
         return MapFrom(container, buildConfiguration, ParameterBuildMode.Global, ParameterBuildMode.Property);
      }

      public IEnumerable<IParameter> MapLocalFrom(IContainer container, IBuildConfiguration buildConfiguration)
      {
         return MapFrom(container, buildConfiguration, ParameterBuildMode.Local);
      }

      public IEnumerable<IParameter> MapAllFrom(IContainer container, IBuildConfiguration buildConfiguration)
      {
         return MapFrom(container, buildConfiguration);
      }

      private bool canBeMapped(IParameter parameter, params ParameterBuildMode[] parameterBuildModesToMap)
      {
         return !parameterBuildModesToMap.Any() || parameterBuildModesToMap.Any(buildModeToMap => parameter.BuildMode == buildModeToMap);
      }
   }
}