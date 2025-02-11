using System;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Defines new formula for a using formula object for any event in model
   /// </summary>
   public class EventAssignment : Entity, IAssignment
   {
      public IDimension Dimension { get; set; }
      public IFormula Formula { get; set; }

      /// <summary>
      ///    Reference to the model entity, whose formula will be changed when event is fired
      /// </summary>
      public IUsingFormula ChangedEntity { get; set; }

      /// <summary>
      ///    Path to IUsingFormulaEntity object, whose formula will be changed
      /// </summary>
      public ObjectPath ObjectPath { get; set; }

      /// <summary>
      ///    Defines whether the formula itself or the VALUE of the formula
      ///    <para></para>
      ///    at the time point when event fires
      /// </summary>
      public bool UseAsValue { get; set; }

      /// <summary>
      ///    Resolves object path in model
      /// </summary>
      public void ResolveChangedEntity()
      {
         if (ObjectPath == null)
            throw new InvalidOperationException("Cannot resolve changed entity. Object path is null");

         ChangedEntity = ObjectPath.Resolve<IUsingFormula>(this);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceEventAssignment = source as EventAssignment;
         if (sourceEventAssignment == null) return;
         UseAsValue = sourceEventAssignment.UseAsValue;
         ObjectPath = sourceEventAssignment.ObjectPath.Clone<ObjectPath>();
         Dimension = sourceEventAssignment.Dimension;
      }
   }
}