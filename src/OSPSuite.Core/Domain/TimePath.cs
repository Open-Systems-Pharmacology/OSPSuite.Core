using System.Collections;
using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    represents a reference to the Simulation time
   /// </summary>
   public class TimePath : FormulaUsablePath
   {
      private readonly TimeParameter _timeParameter;

      public TimePath()
      {
         _timeParameter = new TimeParameter();
         Alias = Constants.TIME;
         _pathEntries.Add(Constants.TIME);
      }


      public override void Add(string pathEntry)
      {
         /*nothing to do here*/
      }

      public override void Replace(string entry, string replacement)
      {
         /*nothing to do here*/
      }

      public override void Replace(string entry, IEnumerable<string> replacements)
      {
         /*nothing to do here*/
      }

      public override void Remove(string entry)
      {
         /*nothing to do here*/
      }

      public override T Resolve<T>(IEntity dependentObject)
      {
         _timeParameter.ParentContainer = dependentObject.RootContainer;
         return _timeParameter.DowncastTo<T>();
      }

      public IDimension TimeDimension
      {
         get => _timeParameter.Dimension;
         set => _timeParameter.Dimension = value;
      }

      public override T Clone<T>()
      {
         return new TimePath {TimeDimension = TimeDimension, Alias = Alias}.DowncastTo<T>();
      }

      public override string this[int index]
      {
         get => Constants.TIME;
         set
         {
            /*nothing to do here*/
         }
      }

      public override void RemoveAt(int index)
      {
         /*nothing to do here*/
      }

      public override void RemoveFirst()
      {
         /*nothing to do here*/
      }

      public override void ReplaceWith(IEnumerable<string> pathEntries)
      {
         /*nothing to do here*/
      }

      public override void AddAtFront(string pathEntry)
      {
         /*nothing to do here*/
      }

      public override bool Equals(object obj)
      {
         var timePath = obj as TimePath;
         return timePath != null;
      }

      public override int GetHashCode()
      {
         return PathAsString.GetHashCode();
      }

      public override string ToString()
      {
         return PathAsString;
      }


      public override IDimension Dimension
      {
         get => TimeDimension;
         set => TimeDimension = value;
      }
   }
}