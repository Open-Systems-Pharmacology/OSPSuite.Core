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
   public interface IEventAssignment : IAssignment
   {
      /// <summary>
      ///    Reference to the model entity, whose formula will be changed when event is fired
      /// </summary>
      IUsingFormula ChangedEntity { get; }

      /// <summary>
      ///    Resolves object path in model
      /// </summary>
      void ResolveChangedEntity();
   }

   public class EventAssignment : Entity, IEventAssignment
   {
      public IDimension Dimension { get; set; }
      public IFormula Formula { get; set; }
      public IUsingFormula ChangedEntity { get; set; }

      /// <summary>
      ///    Path to IUsingFormulaEntity object, whose formula will be changed
      /// </summary>
      public IObjectPath ObjectPath { get; set; }

      /// <summary>
      ///    Defines whether the formula itself or the VALUE of the formula
      ///    <para></para>
      ///    at the timepoint when event fires
      /// </summary>
      public bool UseAsValue { get; set; }

      public void ResolveChangedEntity()
      {
         if (ChangedEntity != null) return;

         if (ObjectPath == null)
            throw new InvalidOperationException("Cannot resolve changed entity. Object path is null");

         ChangedEntity = ObjectPath.Resolve<IUsingFormula>(this);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceEventAssignment = source as IEventAssignment;
         if (sourceEventAssignment == null) return;
         UseAsValue = sourceEventAssignment.UseAsValue;
         ObjectPath = sourceEventAssignment.ObjectPath.Clone<IObjectPath>();
         Dimension = sourceEventAssignment.Dimension;
      }
   }
}