namespace OSPSuite.Presentation.Presenters.Commands
{
   public class ColumnProperties
   {
      public int Index { get; private set; }
      public string Name { get; private set; }
      public string Caption { get; set; }
      public bool IsVisible { get;  set; }
      public bool IsReadOnly { get; private set; }
      public int Position { get; set; }

      public ColumnProperties(int index, string name, bool isReadOnly)
      {
         Index = index;
         Name = name;
         IsReadOnly = isReadOnly;
         Position = index;
      }
   }
}