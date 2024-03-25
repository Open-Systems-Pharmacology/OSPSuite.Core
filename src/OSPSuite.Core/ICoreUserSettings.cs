namespace OSPSuite.Core
{
   public interface ICoreUserSettings
   {
      /// <summary>
      ///    Specifies how many cores can be used by the application when performing parallel computations
      /// </summary>
      int MaximumNumberOfCoresToUse { get; set; }

      /// <summary>
      ///    Number of bins per plot
      /// </summary>
      int NumberOfBins { get; set; }

      /// <summary>
      /// Number of individuals per bin
      /// </summary>
      int NumberOfIndividualsPerBin { get; set; }

      /// <summary>
      /// Suppresses the warning when the user removes an ObservedData-entry from a simulation
      /// </summary>
      /// <remarks>This property is purposely not included in the serializer so the suppression works for the duration of an application-run.</remarks>
      bool SuppressWarningOnRemovingObservedDataEntryFromSimulation { get; set; }
   }
}