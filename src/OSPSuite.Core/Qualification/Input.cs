using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Qualification
{
   public class Input : IWithName, IReferencingProject, IWithSectionReference
   {
      public PKSimBuildingBlockType Type { get; set; }

      public string Name { get; set; }

      public string Project { get; set; }

      /// <summary>
      ///    Optional. For MoBi qualification plans, the name of the PK-Sim module inside the
      ///    MoBi project that this input should be resolved against.
      /// </summary>
      public string PKSimModule { get; set; }

      public int? SectionId { get; set; }

      public string SectionReference { get; set; }

      /// <summary>
      ///    This is not part of the qualification plan. Rather this is some meta information that will be passed along to the
      ///    Input processor to know at what level the input should be processed
      /// </summary>
      public int? SectionLevel { get; set; }
   }
}