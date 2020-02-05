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
         sut.MapFrom(Keys.B).ShouldBeEqualTo(WindowsKey.B);
         sut.MapFrom(Keys.C).ShouldBeEqualTo(WindowsKey.C);
         sut.MapFrom(Keys.D).ShouldBeEqualTo(WindowsKey.D);
         sut.MapFrom(Keys.E).ShouldBeEqualTo(WindowsKey.E);
         sut.MapFrom(Keys.F).ShouldBeEqualTo(WindowsKey.F);
         sut.MapFrom(Keys.G).ShouldBeEqualTo(WindowsKey.G);
         sut.MapFrom(Keys.H).ShouldBeEqualTo(WindowsKey.H);
         sut.MapFrom(Keys.I).ShouldBeEqualTo(WindowsKey.I);
         sut.MapFrom(Keys.J).ShouldBeEqualTo(WindowsKey.J);
         sut.MapFrom(Keys.K).ShouldBeEqualTo(WindowsKey.K);
         sut.MapFrom(Keys.L).ShouldBeEqualTo(WindowsKey.L);
         sut.MapFrom(Keys.M).ShouldBeEqualTo(WindowsKey.M);
         sut.MapFrom(Keys.N).ShouldBeEqualTo(WindowsKey.N);
         sut.MapFrom(Keys.O).ShouldBeEqualTo(WindowsKey.O);
         sut.MapFrom(Keys.P).ShouldBeEqualTo(WindowsKey.P);
         sut.MapFrom(Keys.Q).ShouldBeEqualTo(WindowsKey.Q);
         sut.MapFrom(Keys.R).ShouldBeEqualTo(WindowsKey.R);
         sut.MapFrom(Keys.S).ShouldBeEqualTo(WindowsKey.S);
         sut.MapFrom(Keys.T).ShouldBeEqualTo(WindowsKey.T);
         sut.MapFrom(Keys.U).ShouldBeEqualTo(WindowsKey.U);
         sut.MapFrom(Keys.V).ShouldBeEqualTo(WindowsKey.V);
         sut.MapFrom(Keys.W).ShouldBeEqualTo(WindowsKey.W);
         sut.MapFrom(Keys.X).ShouldBeEqualTo(WindowsKey.X);
         sut.MapFrom(Keys.Y).ShouldBeEqualTo(WindowsKey.Y);
         sut.MapFrom(Keys.Z).ShouldBeEqualTo(WindowsKey.Z);
         sut.MapFrom(Keys.Control).ShouldBeEqualTo(WindowsKey.Control);
         sut.MapFrom(Keys.Alt).ShouldBeEqualTo(WindowsKey.Alt);
         sut.MapFrom(Keys.Shift).ShouldBeEqualTo(WindowsKey.Shift);
         sut.MapFrom(Keys.F1).ShouldBeEqualTo(WindowsKey.F1);
         sut.MapFrom(Keys.F2).ShouldBeEqualTo(WindowsKey.F2);
         sut.MapFrom(Keys.F3).ShouldBeEqualTo(WindowsKey.F3);
         sut.MapFrom(Keys.F4).ShouldBeEqualTo(WindowsKey.F4);
         sut.MapFrom(Keys.F5).ShouldBeEqualTo(WindowsKey.F5);
         sut.MapFrom(Keys.F6).ShouldBeEqualTo(WindowsKey.F6);
         sut.MapFrom(Keys.F7).ShouldBeEqualTo(WindowsKey.F7);
         sut.MapFrom(Keys.F8).ShouldBeEqualTo(WindowsKey.F8);
         sut.MapFrom(Keys.F9).ShouldBeEqualTo(WindowsKey.F9);
         sut.MapFrom(Keys.F10).ShouldBeEqualTo(WindowsKey.F10);
         sut.MapFrom(Keys.F11).ShouldBeEqualTo(WindowsKey.F11);
         sut.MapFrom(Keys.F12).ShouldBeEqualTo(WindowsKey.F12);
         sut.MapFrom(Keys.A | Keys.Alt).ShouldBeEqualTo(WindowsKey.A | WindowsKey.Alt);
         sut.MapFrom(Keys.Shift | Keys.Alt | Keys.D).ShouldBeEqualTo(WindowsKey.D | WindowsKey.Alt | WindowsKey.Shift);
      }
   }
}