namespace OSPSuite.Core.Serialization.SimModel.DTO
{
   public class ParameterExport : QuantityExport
   {
      public ParameterExport()
      {
      }

      public ParameterExport(int id, string name, int formulaId)
      {
         Id = id;
         Name = name;
         FormulaId = formulaId;
         Unit = string.Empty;
         CanBeVaried = true;
         Path = string.Empty;
      }

      public bool CanBeVaried { get; set; }
   }
}