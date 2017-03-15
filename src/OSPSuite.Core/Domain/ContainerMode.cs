namespace OSPSuite.Core.Domain
{
   public enum ContainerMode
   {
      /// <summary>
      /// Represents any container that represents a container with a a physical meaning, for example an organ 
      /// or a compartment
      /// </summary>
      Physical,

      /// <summary>
      /// Represents any container that is typically used as an envelop to group other containers an entites. 
      /// For example a "Molecule Properties" container or an ApplicationSet container etc..
      /// </summary>
      Logical,
   }
}