using System.Collections.Generic;

namespace OSPSuite.Core.Serialization.SimModel.DTO
{
   public class EventExport
   {
      public EventExport()
      {
         AssignmentList = new List<AssigmentExport>();
      }

      public bool OneTime { get; set; }
      public int Id { set; get; }
      public string EntityId { set; get; }
      public int ConditionFormulaId { get; set; }
      public IList<AssigmentExport> AssignmentList { get; set; }
   }
}