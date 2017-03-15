using System.Collections.Generic;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Defines a single event in model. The event will fire if its IfCondition is fulfilled
   /// </summary>
   public interface IEvent : IUsingFormula, IContainer
   {
      IEnumerable<IEventAssignment> Assignments { get; }

      void AddAssignment(IEventAssignment assignment);

      /// <summary>
      ///    If true, event will fire only one time (once the IfCondition-formula is fulfilled)
      ///    <para></para>
      ///    If false (default), event will fire every time its IfCondition-formula is fulfilled
      /// </summary>
      bool OneTime { get; set; }
   }

   public class Event : Container, IEvent
   {
      public IDimension Dimension { get; set; }
      public IFormula Formula { get; set; }
      public bool OneTime { get; set; }

      public IEnumerable<IEventAssignment> Assignments
      {
         get { return GetChildren<IEventAssignment>(); }
      }

      public void AddAssignment(IEventAssignment assignment)
      {
         Add(assignment);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcEvent = source as IEvent;
         if (srcEvent == null) return;

         Dimension = srcEvent.Dimension;
         OneTime = srcEvent.OneTime;
      }
   }
}