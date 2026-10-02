using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Utility.Container;
using DiagramModelToXmlMapper = OSPSuite.Presentation.Diagram.Services.DiagramModelToXmlMapper;

namespace OSPSuite.UI.Diagram
{
   public abstract class concern_for_DiagramModelRegistration : ContextSpecification<IContainer>
   {
      protected override void Context()
      {
         sut = new CastleWindsorContainer();
         sut.RegisterImplementationOf(sut);
         sut.AddRegister(x => x.FromType<PresenterRegister>());
         sut.AddRegister(x => x.FromType<UIRegister>());
      }

      public override void Cleanup()
      {
         sut.Dispose();
      }
   }

   public class When_resolving_the_diagram_model_of_the_application : concern_for_DiagramModelRegistration
   {
      [Observation]
      public void should_create_the_ui_free_diagram_model()
      {
         sut.Resolve<IDiagramModel>().ShouldBeAnInstanceOf<DiagramModel>();
      }

      [Observation]
      public void should_create_the_ui_free_diagram_model_to_xml_mapper()
      {
         sut.Resolve<IDiagramModelToXmlMapper>().ShouldBeAnInstanceOf<DiagramModelToXmlMapper>();
      }
   }
}
