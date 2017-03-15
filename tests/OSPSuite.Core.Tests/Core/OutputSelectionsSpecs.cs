using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_OutputSelections : ContextSpecification<OutputSelections>
   {
      protected override void Context()
      {
         sut = new OutputSelections();
      }
   }

   public class When_updating_an_output_selection_with_another_output_selection : concern_for_OutputSelections
   {
      private ICloneManager _cloner;
      private OutputSelections _newOutputSelection;
      private QuantitySelection _output1;
      private QuantitySelection _output2;

      protected override void Context()
      {
         base.Context();
         _cloner = A.Fake<ICloneManager>();
         _output1 = new QuantitySelection("A", QuantityType.Drug);
         _output2 = new QuantitySelection("B", QuantityType.Complex);

         sut.AddOutput(_output1);
         _newOutputSelection = new OutputSelections();
         _newOutputSelection.AddOutput(_output2);
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_newOutputSelection, _cloner);
      }

      [Observation]
      public void should_remove_the_output_already_selected_and_add_the_new_outputs()
      {
         sut.AllOutputs.ShouldOnlyContain(_output2);
      }
   }
}