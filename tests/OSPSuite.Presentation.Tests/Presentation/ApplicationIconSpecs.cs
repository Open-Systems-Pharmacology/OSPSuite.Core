using System.Drawing;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ApplicationIcon : ContextSpecification<ApplicationIcon>
   {
    
   }

   public class When_retrieving_the_image_define_for_an_application_icon_initialized_without_an_icon : concern_for_ApplicationIcon
   {
      [Observation]
      public void should_not_be_null()
      {
         sut = new ApplicationIcon((Icon)null);
         sut.ToImage().ShouldNotBeNull();
      }
   }
}	