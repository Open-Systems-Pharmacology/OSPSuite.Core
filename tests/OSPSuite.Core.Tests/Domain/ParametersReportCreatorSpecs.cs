using System.Collections.Generic;
using System.Data;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParametersReportCreator : ContextSpecification<IParametersReportCreator>
   {
      protected List<IParameter> _allParameters;

      protected override void Context()
      {
         _allParameters = new List<IParameter>();
         sut = new ParametersReportCreator(new ObjectPathFactory(new AliasCreator()));
      }
   }

   public class When_exporting_some_parameters_to_a_csv_file_using_the_paramteres_report : concern_for_ParametersReportCreator
   {
      private string _csvFile;

      protected override void Context()
      {
         base.Context();
         _csvFile = FileHelper.GenerateTemporaryFileName();
         _allParameters.Add(new Parameter {Name = "P1", Formula = new ConstantFormula(1)});
         _allParameters.Add(new Parameter {Name = "P2", Formula = new ExplicitFormula("4+5")});
      }

      protected override void Because()
      {
         sut.ExportParametersTo(_allParameters, _csvFile);
      }

      [Observation]
      public void should_have_create_the_file()
      {
         FileHelper.FileExists(_csvFile).ShouldBeTrue();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_csvFile);
      }
   }

   public class When_exporting_some_parameters_to_a_datatable_using_the_paramteres_report : concern_for_ParametersReportCreator
   {
      private DataTable _table;

      protected override void Context()
      {
         base.Context();
         _allParameters.Add(new Parameter {Name = "P1", Formula = new ConstantFormula(1)});
         _allParameters.Add(new Parameter {Name = "P2", Formula = new ExplicitFormula("4+5")});
         _allParameters.Add(new Parameter {Name = "P3", Formula = new ExplicitFormula("8+9"), RHSFormula = new ExplicitFormula("A;B")});
      }

      protected override void Because()
      {
         _table = sut.ExportParametersToTable(_allParameters);
      }

      [Observation]
      public void should_have_create_a_table_with_the_required_columns()
      {
         _table.Columns.Count.ShouldBeEqualTo(4);
      }

      [Observation]
      public void should_have_written_the_path_of_the_parameters()
      {
         _table.Rows[0][Constants.ParameterExport.PARAMETER_PATH].ShouldBeEqualTo("P1");
         _table.Rows[1][Constants.ParameterExport.PARAMETER_PATH].ShouldBeEqualTo("P2");
         _table.Rows[2][Constants.ParameterExport.PARAMETER_PATH].ShouldBeEqualTo("P3");
      }

      [Observation]
      public void should_have_written_the_value_of_the_parameters()
      {
         _table.Rows[0][Constants.ParameterExport.VALUE].ShouldBeEqualTo(1);
         _table.Rows[1][Constants.ParameterExport.VALUE].ShouldBeEqualTo(9);
         _table.Rows[2][Constants.ParameterExport.VALUE].ShouldBeEqualTo(17);
      }

      [Observation]
      public void should_have_written_the_formula_string_of_the_parameters_with_explicit_formula()
      {
         _table.Rows[0][Constants.ParameterExport.FORMULA].ShouldBeEqualTo(string.Empty);
         _table.Rows[1][Constants.ParameterExport.FORMULA].ShouldBeEqualTo("4+5");
         _table.Rows[2][Constants.ParameterExport.FORMULA].ShouldBeEqualTo("8+9");
      }

      [Observation]
      public void should_have_written_the_RHS_formula_string_of_the_parameters_with_explicit_RHS_formula()
      {
         _table.Rows[0][Constants.ParameterExport.RHS_FORMULA].ShouldBeEqualTo(string.Empty);
         _table.Rows[1][Constants.ParameterExport.RHS_FORMULA].ShouldBeEqualTo(string.Empty);
         _table.Rows[2][Constants.ParameterExport.RHS_FORMULA].ShouldBeEqualTo("A;B");
      }
   }
}