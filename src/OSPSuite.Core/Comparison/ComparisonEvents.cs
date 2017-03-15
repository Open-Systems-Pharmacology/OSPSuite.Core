using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class StartComparisonEvent
   {
      public IObjectBase RightObject { get; private set; }
      public bool RunComparison { get; private set; }
      public string LeftCaption { get; private set; }
      public string RightCaption { get; private set; }
      public IObjectBase LeftObject { get; private set; }

      public StartComparisonEvent(IObjectBase leftObject, IObjectBase rightObject, bool runComparison = true, string leftCaption = null, string rightCaption = null)
      {
         RightObject = rightObject;
         RunComparison = runComparison;
         LeftCaption = leftCaption;
         RightCaption = rightCaption;
         LeftObject = leftObject;
      }
   }
}