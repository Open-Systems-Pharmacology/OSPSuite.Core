using System.Windows.Forms;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI
{
   public abstract class concern_for_DragEventArgsExtensions : StaticContextSpecification
   {
      protected DragEventArgs EventFor(object data)
      {
         return new DragEventArgs(new DataObject(data), 0, 0, 0, DragDropEffects.All, DragDropEffects.All);
      }
   }

   public class When_checking_if_a_type_is_being_dragged : concern_for_DragEventArgsExtensions
   {
      [Observation]
      public void should_return_true_if_the_type_is_in_the_actual_data()
      {
         EventFor(new Parameter()).TypeBeingDraggedIs(typeof(Parameter)).ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_if_the_type_is_wrapped_into_drag_drop_info()
      {
         EventFor(new DragDropInfo(new Parameter())).TypeBeingDraggedIs(typeof(Parameter)).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         EventFor(new Parameter()).TypeBeingDraggedIs(typeof(MoleculeAmount)).ShouldBeFalse();
         EventFor(new DragDropInfo(new Parameter())).TypeBeingDraggedIs(typeof(MoleculeAmount)).ShouldBeFalse();
      }
   }

   public class When_checking_if_a_derived_type_is_being_dragged : concern_for_DragEventArgsExtensions
   {
      [Observation]
      public void should_return_true_if_the_data_is_wrapped_in_drag_drop_info_and_its_type_derived_from_the_given_type()
      {
         EventFor(new DragDropInfo(new Parameter())).TypeBeingDraggedIs(typeof(IEntity)).ShouldBeTrue();
         EventFor(new DragDropInfo(new Parameter())).TypeBeingDraggedIs<IEntity>().ShouldBeTrue();
      }
   }

   public class When_retrieving_the_data_in_an_event_drag_args : concern_for_DragEventArgsExtensions
   {
      [Observation]
      public void should_return_the_data_if_the_data_is_wrapped_in_a_data_info_and_matches_the_given_type()
      {
         EventFor(new DragDropInfo(new Parameter())).Data<Parameter>().ShouldNotBeNull();
         EventFor(new DragDropInfo(new Parameter())).Data<IEntity>().ShouldNotBeNull();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         EventFor(new Parameter()).Data<MoleculeAmount>().ShouldBeNull();
         EventFor(new DragDropInfo(new Parameter())).Data<MoleculeAmount>().ShouldBeNull();
      }
   }
}