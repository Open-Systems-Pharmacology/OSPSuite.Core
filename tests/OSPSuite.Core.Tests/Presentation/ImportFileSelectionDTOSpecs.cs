using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ImportFileSelectionDTO : ContextSpecification<ImportFileSelectionDTO>
   {
      protected override void Context()
      {
         sut = new ImportFileSelectionDTO();
      }
   }

   public class When_checking_if_a_file_was_selected : concern_for_ImportFileSelectionDTO
   {
      [Observation]
      public void should_return_true_if_the_file_was_defined()
      {
         sut.FilePath = "AA";
         sut.FileDefined.ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_file_was_not_defined()
      {
         sut.FilePath = "";
         sut.FileDefined.ShouldBeFalse();

         sut.FilePath = null;
         sut.FileDefined.ShouldBeFalse();
      }
   }
}	