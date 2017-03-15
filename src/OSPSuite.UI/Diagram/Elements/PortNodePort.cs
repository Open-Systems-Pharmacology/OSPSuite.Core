using Northwoods.Go;

namespace OSPSuite.UI.Diagram.Elements
{
   internal class PortNodePort : GoPort
   {
      public PortNodePort()
      {
         FromSpot = NoSpot;
         ToSpot = NoSpot;
         Brush = null;

         IsValidSelfNode = false;
         IsValidDuplicateLinks = false;
         IsValidFrom = false;
         IsValidTo = false;
      }

   }
}