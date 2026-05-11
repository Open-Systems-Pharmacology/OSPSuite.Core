using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.UI.Extensions
{
   public abstract class concern_for_ApplicationIconExtensions : ContextSpecification<ApplicationIcon>
   {
   }

   public class When_rendering_an_application_icon_initialized_without_bytes : concern_for_ApplicationIconExtensions
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         sut = new ApplicationIcon(bytes: null);
      }

      [Observation]
      public void to_svg_image_should_return_null()
      {
         sut.ToSvgImage().ShouldBeNull();
      }

      [Observation]
      public void to_image_should_fall_back_to_a_blank_bitmap_at_the_default_size()
      {
         var image = sut.ToImage();
         image.ShouldNotBeNull();
         image.Width.ShouldBeEqualTo(IconSizes.Size16x16.Width);
         image.Height.ShouldBeEqualTo(IconSizes.Size16x16.Height);
      }

      [Observation]
      public void to_image_should_fall_back_to_a_blank_bitmap_at_the_requested_size()
      {
         var image = sut.ToImage(IconSizes.Size32x32);
         image.ShouldNotBeNull();
         image.Width.ShouldBeEqualTo(IconSizes.Size32x32.Width);
         image.Height.ShouldBeEqualTo(IconSizes.Size32x32.Height);
      }
   }

   public class When_rendering_a_named_application_icon_twice : concern_for_ApplicationIconExtensions
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         sut = ApplicationIcons.Application;
      }

      [Observation]
      public void should_return_the_cached_svg_image_instance()
      {
         var first = sut.ToSvgImage();
         var second = sut.ToSvgImage();
         first.ShouldNotBeNull();
         ReferenceEquals(first, second).ShouldBeTrue();
      }
   }
}