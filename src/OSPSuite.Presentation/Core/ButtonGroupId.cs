namespace OSPSuite.Presentation.Core
{
   public class ButtonGroupId
   {
      public string Id { get; private set; }

      public ButtonGroupId(string id)
      {
         Id = id;
      }

      public override string ToString()
      {
         return Id;
      }
   }
}