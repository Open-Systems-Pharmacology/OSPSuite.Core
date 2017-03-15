using System.Collections.Generic;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    Defines a single event. The event will fire if its IfCondition is fulfilled
   /// </summary>
   public interface IEventBuilder : IUsingFormula, IContainsParameters, IContainer
   {
      /// <summary>
      ///    List of <see cref="IEventAssignmentBuilder" /> triggered by the event.
      /// </summary>
      IEnumerable<IEventAssignmentBuilder> Assignments { get; }

      /// <summary>
      ///    Adds the <see cref="IEventAssignmentBuilder" />
      /// </summary>
      void AddAssignment(IEventAssignmentBuilder assignment);

      /// <summary>
      ///    Removes the <see cref="IEventAssignmentBuilder" />.
      /// </summary>
      void RemoveAssignment(IEventAssignmentBuilder assignment);

      /// <summary>
      ///    If true, event will fire only one time (once the IfCondition-formula is fulfilled)
      ///    <para></para>
      ///    If false (default), event will fire every time its IfCondition-formula is fulfilled
      /// </summary>
      bool OneTime { get; set; }
   }

   public class EventBuilder : Container, IEventBuilder
   {
      public IDimension Dimension { get; set; }
      public bool OneTime { get; set; }

      /// <summary>
      ///    IfCondition-formula of the event
      /// </summary>
      public IFormula Formula { get; set; }

      public EventBuilder()
      {
         OneTime = false;
      }

      public IEnumerable<IEventAssignmentBuilder> Assignments
      {
         get { return GetChildren<IEventAssignmentBuilder>(); }
      }

      public void AddAssignment(IEventAssignmentBuilder assignment)
      {
         Add(assignment);
      }

      public void RemoveAssignment(IEventAssignmentBuilder assignment)
      {
         RemoveChild(assignment);
      }

      /// <summary>
      ///    List of <see cref="IParameter" /> used by this switch only
      /// </summary>
      public IEnumerable<IParameter> Parameters
      {
         get { return GetChildren<IParameter>(); }
      }

      public void AddParameter(IParameter parameter)
      {
         Add(parameter);
      }

      public void RemoveParameter(IParameter parameter)
      {
         RemoveChild(parameter);
      }


      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcEventBuilder = source as IEventBuilder;
         if (srcEventBuilder == null) return;

         Dimension = srcEventBuilder.Dimension;
         OneTime = srcEventBuilder.OneTime;
      }
   }
}