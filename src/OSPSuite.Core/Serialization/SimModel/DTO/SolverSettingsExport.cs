using System.Collections.Generic;

namespace OSPSuite.Core.Serialization.SimModel.DTO
{
   public class SolverSettingsExport
   {
      public SolverSettingsExport()
      {
         SolverOptions = new List<SolverOptionExport>();
      }

      public string Name { get; set; }
      public int AbsTol { get; set; }
      public int RelTol { get; set; }
      public int H0 { get; set; }
      public int HMin { get; set; }
      public int HMax { get; set; }
      public int    MxStep { get; set; }
      public int UseJacobian { get; set; }
      //For Solver Specific Options
      public IEnumerable<SolverOptionExport> SolverOptions { get; set; }
   }

   public class SolverOptionExport
   {
      public SolverOptionExport(string name, int parameterId)
      {
         Name = name;
         ParameterId = parameterId;
      }

      public int ParameterId { get; set; }
      public string Name { get; private set; }
   }

   
}