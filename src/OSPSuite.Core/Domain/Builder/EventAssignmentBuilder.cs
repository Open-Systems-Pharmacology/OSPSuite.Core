using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IAssignment : IUsingFormula
   {
      /// <summary>
      ///    Path to IUsingFormulaEntity object, whose formula will be changed
      /// </summary>
      IObjectPath ObjectPath { get; set; }

      /// <summary>
      ///    Defines whether the formula itself or the VALUE of the formula
      ///    <para></para>
      ///    at the timepoint when event fires
      /// </summary>
      bool UseAsValue { get; set; }
   }

   /// <summary>
   ///    Defines new formula for a using formula object
   /// </summary>
   public interface IEventAssignmentBuilder : IAssignment
   {
   }

   public class EventAssignmentBuilder : Entity, IEventAssignmentBuilder
   {
      public IObjectPath ObjectPath { get; set; }

      /// <summary>
      ///    New formula to be set when event fires
      /// </summary>
      public IFormula Formula { get; set; }

      public bool UseAsValue { get; set; }
      public IDimension Dimension { get; set; }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcEventAssignmentBuilder = source as IEventAssignmentBuilder;
         if (srcEventAssignmentBuilder == null) return;

         UseAsValue = srcEventAssignmentBuilder.UseAsValue;
         Dimension = srcEventAssignmentBuilder.Dimension;

         ObjectPath = srcEventAssignmentBuilder.ObjectPath.Clone<IObjectPath>();
      }
   }
}