using System.Collections.Generic;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Defines a single event in model. The event will fire if its IfCondition is fulfilled
   /// </summary>
   public class Event : Container, IUsingFormula
   {
      public IDimension Dimension { get; set; }
      public IFormula Formula { get; set; }

      /// <summary>
      ///    If true, event will fire only one time (once the IfCondition-formula is fulfilled)
      ///    <para></para>
      ///    If false (default), event will fire every time its IfCondition-formula is fulfilled
      /// </summary>
      public bool OneTime { get; set; }

      public IEnumerable<EventAssignment> Assignments => GetChildren<EventAssignment>();

      public void AddAssignment(EventAssignment assignment) => Add(assignment);

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var sourceEvent = source as Event;
         if (sourceEvent == null) return;

         Dimension = sourceEvent.Dimension;
         OneTime = sourceEvent.OneTime;
      }
   }
}