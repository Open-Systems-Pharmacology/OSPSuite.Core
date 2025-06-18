﻿using System.Threading.Tasks;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public class ParameterIdentificationAlgorithmMapper : SnapshotMapperBase<OptimizationAlgorithmProperties, OptimizationAlgorithm, SnapshotContext>
   {
      private readonly ExtendedPropertyMapper _extendedPropertyMapper;

      public ParameterIdentificationAlgorithmMapper(ExtendedPropertyMapper extendedPropertyMapper)
      {
         _extendedPropertyMapper = extendedPropertyMapper;
      }

      public override async Task<OptimizationAlgorithm> MapToSnapshot(OptimizationAlgorithmProperties algorithmProperties)
      {
         var snapshot = await SnapshotFrom(algorithmProperties, x => { x.Name = algorithmProperties.Name; });
         snapshot.Properties = await _extendedPropertyMapper.MapToSnapshots(algorithmProperties);
         snapshot.Properties?.Each(_extendedPropertyMapper.ClearDatabaseProperties);
         return snapshot;
      }

      public override async Task<OptimizationAlgorithmProperties> MapToModel(OptimizationAlgorithm snapshot, SnapshotContext snapshotContext)
      {
         var algorithmProperties = new OptimizationAlgorithmProperties(snapshot.Name);
         var properties = await _extendedPropertyMapper.MapToModels(snapshot.Properties, snapshotContext);
         properties?.Each(algorithmProperties.Add);
         return algorithmProperties;
      }
   }
}