using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Serialization.SimModel;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Core.Serialization.SimModel.Serializer;

namespace OSPSuite.Core.Domain
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
      private const int ABS_TOL_ID = 1;
      private const int REL_TOL_ID = 2;
      private const int H0_ID = 3;
      private const int H_MAX_ID = 4;
      private const int H_MIN_ID = 5;
      private const int MX_STEP_ID = 6;
      private const int USE_JACOBIAN_ID = 7;
      private const int CHECK_FOR_NEGATIVE_VALUES_ID = 8;
      private const int SOLVER_OPTION_ID = 22;

      private SolverSettingsExport _solverSettingsExport;
      private XElement _xmlResultNode;

      protected override void Context()
      {
         base.Context();
         _solverSettingsExport = new SolverSettingsExport();
         _solverSettingsExport.AbsTol = ABS_TOL_ID;
         _solverSettingsExport.RelTol = REL_TOL_ID;
         _solverSettingsExport.H0 = H0_ID;
         _solverSettingsExport.HMax = H_MAX_ID;
         _solverSettingsExport.HMin = H_MIN_ID;
         _solverSettingsExport.MxStep = MX_STEP_ID;
         _solverSettingsExport.Name = "CVODE";
         _solverSettingsExport.UseJacobian = USE_JACOBIAN_ID;
         _solverSettingsExport.CheckForNegativeValues = CHECK_FOR_NEGATIVE_VALUES_ID;
         _solverSettingsExport.SolverOptions = new Collection<SolverOptionExport> {new SolverOptionExport("Option", SOLVER_OPTION_ID)};

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
      public void should_add_the_check_for_negative_values_node_referencing_the_solver_parameter()
      {
         _xmlResultNode.Element(XName.Get("CheckForNegativeValues", SimModelSchemaConstants.Namespace)).Attribute(SimModelSchemaConstants.Id).Value.ShouldBeEqualTo(_solverSettingsExport.CheckForNegativeValues.ToString());
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