namespace OSPSuite.Core.Domain
{

   public interface IObjectReference
   {
      IFormulaUsable Object { set; get; }
      string Alias { set; get; }
   }

   public class ObjectReference : IObjectReference
   {
      public ObjectReference(IFormulaUsable usedObject, string alias)
      {
         Object = usedObject;
         Alias = alias;
      }

      public IFormulaUsable Object { get; set; }
      public string Alias { get; set; }
   }
}