namespace OSPSuite.Presentation.Core
{
   public class MenuBarItemId
   {
      /// <summary>
      /// Name of the element that should be unique over all menus
      /// </summary>
      public string Name { get; private set; }
      public int Id { get; private set; }

      public MenuBarItemId(string name, int id)
      {
         Name = name;
         Id = id;
      }

      public override string ToString()
      {
         return Name;
      }
   }
}