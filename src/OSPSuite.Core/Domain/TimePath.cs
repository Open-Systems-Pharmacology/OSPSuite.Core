using System.Collections;
using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    represents a reference to the Simulation time
   /// </summary>
   public class TimePath : IFormulaUsablePath
   {
      private readonly List<string> _path;
      private readonly TimeParameter _timeParameter;
      public string Alias { set; get; }

      public TimePath()
      {
         _path = new List<string> {Constants.TIME};
         _timeParameter = new TimeParameter();
         Alias = Constants.TIME;
      }

      public string PathAsString
      {
         get { return "TIME"; }
      }

      public void Add(string pathEntry)
      {
         /*nothing to do here*/
      }

      public void Replace(string entry, string replacement)
      {
         /*nothing to do here*/
      }

      public void Replace(string entry, IEnumerable<string> replacements)
      {
         /*nothing to do here*/
      }

      public void Remove(string entry)
      {
         /*nothing to do here*/
      }

      public T Resolve<T>(IEntity dependentObject) where T : class
      {
         _timeParameter.ParentContainer = dependentObject.RootContainer;
         return _timeParameter.DowncastTo<T>();
      }

      public IDimension TimeDimension
      {
         get { return _timeParameter.Dimension; }
         set { _timeParameter.Dimension = value; }
      }

      public T Clone<T>() where T : IObjectPath
      {
         return new TimePath {TimeDimension = TimeDimension, Alias = Alias}.DowncastTo<T>();
      }

      public string this[int index]
      {
         get { return Constants.TIME; }
         set
         {
            /*nothing to do here*/
         }
      }

      public void RemoveAt(int index)
      {
         /*nothing to do here*/
      }

      public void RemoveFirst()
      {
         /*nothing to do here*/
      }

      public void AddAtFront(string pathEntry)
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

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      public IEnumerator<string> GetEnumerator()
      {
         return _path.GetEnumerator();
      }

      public override string ToString()
      {
         return PathAsString;
      }

      public int Count
      {
         get { return _path.Count; }
      }

      public IDimension Dimension
      {
         get { return TimeDimension; }
         set { TimeDimension = value; }
      }
   }
}