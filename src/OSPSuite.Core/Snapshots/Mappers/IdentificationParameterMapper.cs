﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using static OSPSuite.Core.Domain.Constants;
using ModelIdentificationParameter = OSPSuite.Core.Domain.ParameterIdentifications.IdentificationParameter;
using SnapshotIdentificationParameter = OSPSuite.Core.Snapshots.IdentificationParameter;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public abstract class IdentificationParameterMapper : ParameterContainerSnapshotMapperBase<ModelIdentificationParameter, SnapshotIdentificationParameter, ParameterIdentificationContext>
   {
      private readonly IIdentificationParameterFactory _identificationParameterFactory;
      private readonly IIdentificationParameterTask _identificationParameterTask;
      private readonly IOSPSuiteLogger _logger;

      public IdentificationParameterMapper(ParameterMapper parameterMapper,
         IIdentificationParameterFactory identificationParameterFactory,
         IIdentificationParameterTask identificationParameterTask,
         IOSPSuiteLogger logger) : base(parameterMapper)
      {
         _identificationParameterFactory = identificationParameterFactory;
         _identificationParameterTask = identificationParameterTask;
         _logger = logger;
      }

      public override Task<SnapshotIdentificationParameter> MapToSnapshot(ModelIdentificationParameter identificationParameter)
      {
         return SnapshotFrom(identificationParameter, x =>
         {
            x.Scaling = identificationParameter.Scaling;
            x.UseAsFactor = SnapshotValueFor(identificationParameter.UseAsFactor);
            x.IsFixed = SnapshotValueFor(identificationParameter.IsFixed);
            x.LinkedParameters = linkedParametersFrom(identificationParameter.AllLinkedParameters);
         });
      }

      private string[] linkedParametersFrom(IReadOnlyList<ParameterSelection> linkedParameters)
      {
         return !linkedParameters.Any() ? null : linkedParameters.Select(x => x.FullQuantityPath).ToArray();
      }

      public override async Task<ModelIdentificationParameter> MapToModel(SnapshotIdentificationParameter snapshot, ParameterIdentificationContext snapshotContext)
      {
         if (snapshot.LinkedParameters == null || !snapshot.LinkedParameters.Any())
            return null;

         var parameterSelections = snapshot.LinkedParameters.Select(x => parameterSelectionFrom(x, snapshotContext));

         var identificationParameter = _identificationParameterFactory.CreateFor(parameterSelections, snapshotContext.ParameterIdentification);
         if (identificationParameter == null)
         {
            _logger.AddWarning(Error.CannotCreateIdentificationParameter(snapshot.LinkedParameters[0], snapshotContext.ParameterIdentification.Name));
            return null;
         }

         MapSnapshotPropertiesToModel(snapshot, identificationParameter);
         identificationParameter.IsFixed = ModelValueFor(snapshot.IsFixed);
         identificationParameter.UseAsFactor = ModelValueFor(snapshot.UseAsFactor);
         identificationParameter.Scaling = snapshot.Scaling;

         if (identificationParameter.UseAsFactor)
            _identificationParameterTask.UpdateParameterRange(identificationParameter);

         await UpdateParametersFromSnapshot(snapshot, identificationParameter, snapshotContext);

         return identificationParameter;
      }

      private ParameterSelection parameterSelectionFrom(string parameterFullPath, ParameterIdentificationContext parameterIdentificationContext)
      {
         var parameterPath = new ObjectPath(parameterFullPath.ToPathArray());
         if (parameterPath.Count == 0)
            return null;

         var simulationName = parameterPath[0];
         var simulation = SimulationByName(parameterIdentificationContext, simulationName);
         if (simulation == null)
         {
            _logger.AddWarning(Error.CouldNotFindSimulation(simulationName));
            return null;
         }

         parameterPath.RemoveAt(0);

         if (parameterIdentificationContext.IsV11FormatOrEarlier)
            updatePathsForV12(parameterPath);

         return new ParameterSelection(simulation, parameterPath);
      }

      protected abstract IModelCoreSimulation SimulationByName(ParameterIdentificationContext parameterIdentificationContext, string simulationName);

      private static void updatePathsForV12(ObjectPath parameterPath)
      {
         parameterPath.Replace(ContainerName.Applications, EVENTS);

         replaceRenalClearanceName(parameterPath, Captions.GlomerularFiltration);
         replaceRenalClearanceName(parameterPath, Captions.RenalClearance);
      }

      // renal clearances container names have been modified to append the name of the compound
      private static void replaceRenalClearanceName(ObjectPath parameterPath, string processName)
      {
         var processContainerName = parameterPath.FirstOrDefault(x => x.StartsWith(processName));
         if (string.IsNullOrEmpty(processContainerName))
            return;

         var compoundName = parameterPath.ElementAt(parameterPath.IndexOf(processContainerName) - 1);
         parameterPath.Replace(processContainerName, CompositeNameFor(processContainerName, compoundName));
      }
   }
}