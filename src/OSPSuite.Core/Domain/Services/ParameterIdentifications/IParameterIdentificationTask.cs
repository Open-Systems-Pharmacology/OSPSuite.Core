using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IParameterIdentificationTask
   {
      /// <summary>
      ///    Creates a new <see cref="ParameterIdentification" /> that will optimize the <paramref name="parameters" /> using all
      ///    default settings
      /// </summary>
      ParameterIdentification CreateParameterIdentificationBasedOn(IEnumerable<IParameter> parameters);

      /// <summary>
      ///    Creates a new <see cref="ParameterIdentification" /> that will use the <paramref name="simulations" /> and all
      ///    default settings
      /// </summary>
      ParameterIdentification CreateParameterIdentificationBasedOn(IEnumerable<ISimulation> simulations);

      /// <summary>
      ///    Creates an empty <see cref="ParameterIdentification" />
      /// </summary>
      ParameterIdentification CreateParameterIdentification();

      /// <summary>
      ///    Adds the <paramref name="parameter" /> as <see cref="IdentificationParameter" /> to the
      ///    <paramref name="parameterIdentification" />
      /// </summary>
      void AddParameterTo(ParameterIdentification parameterIdentification, IParameter parameter);

      /// <summary>
      ///    Adds the <paramref name="parameters" /> as <see cref="IdentificationParameter" /> to the
      ///    <paramref name="parameterIdentification" />
      /// </summary>
      void AddParametersTo(ParameterIdentification parameterIdentification, IEnumerable<IParameter> parameters);

      /// <summary>
      ///    Adds the <paramref name="simulation" /> to the <paramref name="parameterIdentification" /> and creates the
      ///    <see cref="OutputMapping" /> that can be generated automatically based on available outputs and observed data
      /// </summary>
      void AddSimulationTo(ParameterIdentification parameterIdentification, ISimulation simulation);

      /// <summary>
      ///    Adds the <paramref name="simulations" /> to the <paramref name="parameterIdentification" /> and creates the
      ///    <see cref="OutputMapping" /> that can be generated automatically based on available outputs and observed data
      /// </summary>
      void AddSimulationsTo(ParameterIdentification parameterIdentification, IEnumerable<ISimulation> simulations);

      /// <summary>
      ///    Returns the default scaling for the <paramref name="output" />
      /// </summary>
      Scalings DefaultScalingFor(IQuantity output);

      void AddToProject(ParameterIdentification parameterIdentification);

      /// <summary>
      /// Removes <paramref name="oldSimulation"/> and adds <paramref name="newSimulation"/> in its place while trying to preserve
      /// the configuration
      /// </summary>
      void SwapSimulations(ParameterIdentification parameterIdentification, ISimulation oldSimulation, ISimulation newSimulation);

      /// <summary>
      /// Returns the parameter identifications which have output mappings that include the <paramref name="observedData"/>
      /// </summary>
      IEnumerable<ParameterIdentification> ParameterIdentificationsUsingObservedData(DataRepository observedData);

      /// <summary>
      /// Asks the user if he really wants to delete the <paramref name="parameterIdentifications"/>. Returns <c>true</c> if the <paramref name="parameterIdentifications"/>
      /// are deleted from the current project otherwise <c>false</c>
      /// </summary>
      bool Delete(IReadOnlyList<ParameterIdentification> parameterIdentifications);

      /// <summary>
      /// returns true if <paramref name="simulation"/> can be used for parameter identification
      /// </summary>
      bool SimulationCanBeUsedForIdentification(ISimulation simulation);

      /// <summary>
      /// Returns a clone of the <paramref name="parameterIdentification"/> or null if the user cancels the clone action
      /// </summary>
      ParameterIdentification Clone(ParameterIdentification parameterIdentification);
   }
}