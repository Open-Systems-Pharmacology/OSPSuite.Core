using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Info about a single individual run
   /// </summary>
   public class IndividualRunInfo
   {
      private readonly List<SolverWarning> _solverWarnings;

      /// <summary>
      ///    Was the individual run ok or not
      /// </summary>
      public bool Success { get; set; }

      /// <summary>
      ///    Is not empty only in case of Success=false
      /// </summary>
      public string ErrorMessage { get; set; }

      public IndividualRunInfo()
      {
         Success = false;
         ErrorMessage = string.Empty;
         _solverWarnings = new List<SolverWarning>();
      }

      /// <summary>
      ///    May be nonempty even in case of Success=true
      /// </summary>
      public IReadOnlyList<SolverWarning> SolverWarnings => _solverWarnings;

      public void AddWarnings(IEnumerable<SolverWarning> warnings)
      {
         _solverWarnings.AddRange(warnings);
      }
   }

   /// <summary>
   ///    Results of the population run. Contains individual results for sucessfull individuals.
   ///    Contains individual run info for all individuals
   /// </summary>
   public class PopulationRunResults
   {
      private readonly ICache<int, IndividualRunInfo> _individualRunInfos;

      public PopulationRunResults()
      {
         _individualRunInfos = new Cache<int, IndividualRunInfo>();
         Results = new SimulationResults();
      }

      /// <summary>
      ///    Results for successfull individuals only
      /// </summary>
      public SimulationResults Results { get; internal set; }

      /// <summary>
      ///    Info for all individual runs
      /// </summary>
      public IEnumerable<IndividualRunInfo> IndividualRunInfos => _individualRunInfos;

      /// <summary>
      ///    Info of an individual run
      /// </summary>
      public IndividualRunInfo IndividualRunInfoFor(int individualId)
      {
         return _individualRunInfos[individualId];
      }

      /// <summary>
      ///    Add results for successfull individual
      /// </summary>
      /// <param name="individualResults"></param>
      public void Add(IndividualResults individualResults)
      {
         Results.Add(individualResults);
         var runInfo = new IndividualRunInfo {Success = true};
         _individualRunInfos[individualResults.IndividualId] = runInfo;
      }

      /// <summary>
      ///    This methods ensure that the time arrays in all <see cref="IndividualResults" /> are using the reference defined in
      ///    the parent <see cref="SimulationResults" />.
      ///    Also results will be reorderd by Individual Id
      /// </summary>
      public void SynchronizeResults()
      {
         if (!Results.Any())
            return;

         //Retrieve time array from the first individual that will be used as reference
         var firstTime = Results.ElementAt(0).Time;
         Results.Time = firstTime;

         foreach (var individualResults in Results)
         {
            if (individualResults.Time.Values.IsEqual(firstTime.Values))
               individualResults.Time = firstTime;

            individualResults.UpdateQuantityTimeReference();
         }

         Results.ReorderByIndividualId();
      }

      /// <summary>
      ///    Add error message for failed individual
      /// </summary>
      /// <param name="individualId">Individual id</param>
      /// <param name="message">Error message</param>
      public void AddFailure(int individualId, string message)
      {
         var runInfo = new IndividualRunInfo
         {
            Success = false,
            ErrorMessage = message
         };
         _individualRunInfos[individualId] = runInfo;
      }

      /// <summary>
      ///    Add solver warnings for an individual
      /// </summary>
      /// <param name="individualId">Individual id</param>
      /// <param name="warnings">Solver warnings</param>
      public void AddWarnings(int individualId, IEnumerable<SolverWarning> warnings)
      {
         if (!_individualRunInfos.Contains(individualId))
            return;

         _individualRunInfos[individualId].AddWarnings(warnings);
      }

      public IReadOnlyList<IndividualRunInfo> Warnings
      {
         get { return _individualRunInfos.Where(x => x.SolverWarnings.Any()).ToList(); }
      }

      public IReadOnlyList<IndividualRunInfo> Errors
      {
         get { return _individualRunInfos.Where(x => !x.Success).ToList(); }
      }

      //Debug only
      public IReadOnlyList<int> MissingIds(int numberOfIndividuals)
      {
         var allIds = Results.AllIndividualResults.Select(x => x.IndividualId).OrderBy(x => x).ToList();
         var range = Enumerable.Range(0, numberOfIndividuals);
         return range.Where(individualId => !allIds.Contains(individualId)).ToList();
      }
   }
}