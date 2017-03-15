using OSPSuite.Assets;

namespace OSPSuite.Presentation.Core
{
   public class ClassificationTemplate
   {
      public string ClassificationName { get; private set; }
      public ApplicationIcon Icon { get; private set; }

      public ClassificationTemplate(string classificationName)
         : this(classificationName, ApplicationIcons.DefaultIcon)
      {
      }

      public ClassificationTemplate(string classificationName, ApplicationIcon icon)
      {
         ClassificationName = classificationName;
         Icon = icon;
      }
   }
}