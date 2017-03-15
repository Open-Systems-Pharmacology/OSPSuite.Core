using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Serialization.SimModel;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Core.Serialization.SimModel.Serializer;

namespace OSPSuite.Core
{
   public abstract class Concern_for_SolverSettingsExportSerializer : ContextSpecification<SolverSettingsExportSerializer>
   {
      private SimModelSerializerRepository _repository;

      protected override void Context()
      {
         _repository = new SimModelSerializerRepository();
         sut = (SolverSettingsExportSerializer) _repository.SerializerFor<SolverSettingsExport>();
      }
   }

   public class When_serializing_the_solver_settings : Concern_for_SolverSettingsExportSerializer
   {
      private SolverSettingsExport _solverSettingsExport;
      private XElement _xmlResultNode;

      protected override void Context()
      {
         base.Context();
         _solverSettingsExport = new SolverSettingsExport();
         _solverSettingsExport.AbsTol = 1;
         _solverSettingsExport.RelTol = 2;
         _solverSettingsExport.H0 = 3;
         _solverSettingsExport.HMax = 4;
         _solverSettingsExport.HMin = 5;
         _solverSettingsExport.MxStep = 6;
         _solverSettingsExport.Name = "CVODE";
         _solverSettingsExport.UseJacobian = 7;
         _solverSettingsExport.SolverOptions = new Collection<SolverOptionExport> {new SolverOptionExport("Option",22)};

      }
      protected override void Because()
      {
         _xmlResultNode = sut.Serialize(_solverSettingsExport, new SimModelSerializationContext());
      }
      [Observation]
      public void should_create_the_solver_setting_node()
      {
         _xmlResultNode.Name.LocalName.ShouldBeEqualTo(SimModelSchemaConstants.Solver);
         _xmlResultNode.Attribute("name").Value.ShouldBeEqualTo(_solverSettingsExport.Name);
         _xmlResultNode.Element(XName.Get("AbsTol",SimModelSchemaConstants.Namespace)).Attribute(SimModelSchemaConstants.Id).Value.ShouldBeEqualTo(_solverSettingsExport.AbsTol.ToString());
         _xmlResultNode.Element(XName.Get("RelTol", SimModelSchemaConstants.Namespace)).Attribute(SimModelSchemaConstants.Id).Value.ShouldBeEqualTo(_solverSettingsExport.RelTol.ToString());
         _xmlResultNode.Element(XName.Get("H0", SimModelSchemaConstants.Namespace)).Attribute(SimModelSchemaConstants.Id).Value.ShouldBeEqualTo(_solverSettingsExport.H0.ToString());
         _xmlResultNode.Element(XName.Get("HMax", SimModelSchemaConstants.Namespace)).Attribute(SimModelSchemaConstants.Id).Value.ShouldBeEqualTo(_solverSettingsExport.HMax.ToString());
         _xmlResultNode.Element(XName.Get("HMin", SimModelSchemaConstants.Namespace)).Attribute(SimModelSchemaConstants.Id).Value.ShouldBeEqualTo(_solverSettingsExport.HMin.ToString());
         _xmlResultNode.Element(XName.Get("MxStep", SimModelSchemaConstants.Namespace)).Attribute(SimModelSchemaConstants.Id).Value.ShouldBeEqualTo(_solverSettingsExport.MxStep.ToString());
         _xmlResultNode.Element(XName.Get("UseJacobian", SimModelSchemaConstants.Namespace)).Attribute(SimModelSchemaConstants.Id).Value.ShouldBeEqualTo(_solverSettingsExport.UseJacobian.ToString());
         
      }
      [Observation]
      public void should_add_the_solver_options_node()
      {
         var xmlSolverOptions = _xmlResultNode.Element(XName.Get(SimModelSchemaConstants.SolverOptionList, SimModelSchemaConstants.Namespace));
         xmlSolverOptions.ShouldNotBeNull();
         xmlSolverOptions.Descendants().Count().ShouldBeEqualTo(_solverSettingsExport.SolverOptions.Count());
      }
   }
}	