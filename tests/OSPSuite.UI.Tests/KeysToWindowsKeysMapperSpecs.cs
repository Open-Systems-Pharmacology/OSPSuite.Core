using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.UI.Mappers;
using WindowsKey = System.Windows.Forms.Keys;

namespace OSPSuite.UI
{
   public abstract class concern_for_KeysToWindowsKeysMapper : ContextSpecification<IKeysToWindowsKeysMapper>
   {
      protected override void Context()
      {
         sut = new KeysToWindowsKeysMapper();
      }
   }

   public class When_mapping_some_keys_to_windows_keys : concern_for_KeysToWindowsKeysMapper
   {
      [Observation]
      public void should_return_the_expected_keys()
      {
         sut.MapFrom(Keys.A).ShouldBeEqualTo(WindowsKey.A);
         sut.MapFrom(Keys.A | Keys.Alt).ShouldBeEqualTo(WindowsKey.A | WindowsKey.Alt);
         sut.MapFrom(Keys.Shift | Keys.Alt | Keys.D).ShouldBeEqualTo(WindowsKey.D | WindowsKey.Alt | WindowsKey.Shift);
      }
   }
}