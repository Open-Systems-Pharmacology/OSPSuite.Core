using System.Collections.Generic;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder
{
   public class EventBuilder : Container, IUsingFormula, IContainsParameters
   {
      public IDimension Dimension { get; set; }

      /// <summary>
      ///    If true, event will fire only one time (once the IfCondition-formula is fulfilled)
      ///    <para></para>
      ///    If false (default), event will fire every time its IfCondition-formula is fulfilled
      /// </summary>
      public bool OneTime { get; set; }

      /// <summary>
      ///    IfCondition-formula of the event
      /// </summary>
      public IFormula Formula { get; set; }

      public EventBuilder()
      {
         OneTime = false;
      }

      /// <summary>
      ///    List of <see cref="EventAssignmentBuilder" /> triggered by the event.
      /// </summary>
      public IEnumerable<EventAssignmentBuilder> Assignments => GetChildren<EventAssignmentBuilder>();

      /// <summary>
      ///    Adds the <see cref="EventAssignmentBuilder" />
      /// </summary>
      public void AddAssignment(EventAssignmentBuilder assignment) => Add(assignment);

      /// <summary>
      ///    Removes the <see cref="EventAssignmentBuilder" />.
      /// </summary>
      public void RemoveAssignment(EventAssignmentBuilder assignment) => RemoveChild(assignment);

      /// <summary>
      ///    List of <see cref="IParameter" /> used by this switch only
      /// </summary>
      public IEnumerable<IParameter> Parameters => GetChildren<IParameter>();

      public void AddParameter(IParameter parameter) => Add(parameter);

      public void RemoveParameter(IParameter parameter) => RemoveChild(parameter);

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcEventBuilder = source as EventBuilder;
         if (srcEventBuilder == null) return;

         Dimension = srcEventBuilder.Dimension;
         OneTime = srcEventBuilder.OneTime;
      }
   }
}