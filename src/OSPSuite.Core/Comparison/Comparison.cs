namespace OSPSuite.Core.Comparison
{
   public interface IComparison<out TObject> where TObject : class
   {
      /// <summary>
      ///    The first object of type <typeparamref name="TObject" /> to compare
      /// </summary>
      TObject Object1 { get; }

      /// <summary>
      ///    The second object of type <typeparamref name="TObject" /> to compare
      /// </summary>
      TObject Object2 { get; }

      /// <summary>
      ///    Settings to use for the comparison
      /// </summary>
      ComparerSettings Settings { get; }

      /// <summary>
      ///    Current reference to report being generated for comparison
      /// </summary>
      DiffReport Report { get; }

      /// <summary>
      ///    Parent of object 1. This is used to find the path where the objects differs.
      /// </summary>
      object CommonAncestor { get; }

      /// <summary>
      ///    Adds a <see cref="DiffItem" /> to the report
      /// </summary>
      /// <param name="diffItem"></param>
      void Add(DiffItem diffItem);

      /// <summary>
      ///    Returns <c>true</c> if <see cref="Object1" /> and <see cref="Object2" /> are defined (not null) otherwise
      ///    <c>false</c>
      /// </summary>
      bool ComparedObjectsDefined { get; }
   }

   public class Comparison<TObject> : IComparison<TObject> where TObject : class
   {
      public TObject Object1 { get; }
      public TObject Object2 { get; }
      public ComparerSettings Settings { get; }
      public DiffReport Report { get; }
      public object CommonAncestor { get; }

      public Comparison(TObject object1, TObject object2, ComparerSettings settings, DiffReport report, object commonAncestor)
      {
         Object1 = object1;
         Object2 = object2;
         Settings = settings;
         Report = report;
         CommonAncestor = commonAncestor;
      }

      public void Add(DiffItem diffItem)
      {
         Report.Add(diffItem);
      }

      public bool ComparedObjectsDefined => Object1 != null && Object2 != null;
   }
}