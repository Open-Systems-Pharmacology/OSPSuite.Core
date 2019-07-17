using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.DiffBuilders
{
   public class When_comparing_two_molecule_start_values_with_same_properties : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();

         var msv1 = new MoleculeStartValue().WithName("Tada").WithFormula(new ConstantFormula(2));
         msv1.Path = new ObjectPath("Root", "Liver", "Plasma");
         msv1.IsPresent = true;
         msv1.ScaleDivisor = 10;
         var msv2 = new MoleculeStartValue().WithName("Tada").WithFormula(new ConstantFormula(2));
         msv2.Path = new ObjectPath("Root", "Liver", "Plasma");
         msv2.IsPresent = true;
         msv2.ScaleDivisor = 10;

         _object1 = msv1;
         _object2 = msv2;
      }

      [Observation]
      public void should_not_report_any_differences()
      {
         _report.ShouldBeEmpty();
      }
   }

   public class When_comparing_two_molecule_start_values_with_different_is_present_setting : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();

         var msv1 = new MoleculeStartValue().WithName("Tada").WithFormula(new ConstantFormula(2));
         msv1.Path = new ObjectPath("Root", "Liver", "Plasma");
         msv1.IsPresent = true;
         var msv2 = new MoleculeStartValue().WithName("Tada").WithFormula(new ConstantFormula(2));
         msv2.Path = new ObjectPath("Root", "Liver", "Plasma");
         msv2.IsPresent = false;

         _object1 = msv1;
         _object2 = msv2;
      }

      [Observation]
      public void should_report_the_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_molecule_start_values_with_different_scale_divisor : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();

         var msv1 = new MoleculeStartValue().WithName("Tada").WithFormula(new ConstantFormula(2));
         msv1.Path = new ObjectPath("Root", "Liver", "Plasma");
         msv1.IsPresent = true;
         msv1.ScaleDivisor = 1;
         var msv2 = new MoleculeStartValue().WithName("Tada").WithFormula(new ConstantFormula(2));
         msv2.Path = new ObjectPath("Root", "Liver", "Plasma");
         msv2.IsPresent = true;
         msv2.ScaleDivisor = 10;

         _object1 = msv1;
         _object2 = msv2;
      }

      [Observation]
      public void should_report_the_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_molecule_start_values_with_different_scale_divisor_and_the_value_origin_should_be_visible : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();

         var msv1 = new MoleculeStartValue().WithName("Tada").WithFormula(new ConstantFormula(2));
         msv1.Path = new ObjectPath("Root", "Liver", "Plasma");
         msv1.IsPresent = true;
         msv1.ScaleDivisor = 1;
         msv1.ValueOrigin.Description = "DESC1";
         var msv2 = new MoleculeStartValue().WithName("Tada").WithFormula(new ConstantFormula(2));
         msv2.Path = new ObjectPath("Root", "Liver", "Plasma");
         msv2.IsPresent = true;
         msv2.ScaleDivisor = 10;
         msv2.ValueOrigin.Description = "DESC2";

         _object1 = msv1;
         _object2 = msv2;

         _comparerSettings.ShowValueOrigin = true;
      }

      [Observation]
      public void should_report_the_differences_of_values_and_value_origin()
      {
         _report.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_two_molecule_start_values_with_different_scale_divisor_and_the_value_origin_should_not_be_visible : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();

         var msv1 = new MoleculeStartValue().WithName("Tada").WithFormula(new ConstantFormula(2));
         msv1.Path = new ObjectPath("Root", "Liver", "Plasma");
         msv1.IsPresent = true;
         msv1.ScaleDivisor = 1;
         msv1.ValueOrigin.Description = "DESC1";
         var msv2 = new MoleculeStartValue().WithName("Tada").WithFormula(new ConstantFormula(2));
         msv2.Path = new ObjectPath("Root", "Liver", "Plasma");
         msv2.IsPresent = true;
         msv2.ScaleDivisor = 10;
         msv2.ValueOrigin.Description = "DESC2";

         _object1 = msv1;
         _object2 = msv2;

         _comparerSettings.ShowValueOrigin = false;
      }

      [Observation]
      public void should_report_the_differences_of_values_only()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_parmeter_start_values_with_different_formulas : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();

         var msv1 = new ParameterStartValue().WithName("Tada").WithFormula(new ConstantFormula(2));
         msv1.Path = new ObjectPath("Root", "Liver", "Plasma");

         var msv2 = new ParameterStartValue().WithName("Tada").WithFormula(new ExplicitFormula("2"));
         msv2.Path = new ObjectPath("Root", "Liver", "Plasma");


         _object1 = msv1;
         _object2 = msv2;
      }

      [Observation]
      public void should_report_the_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_parmeter_start_values_with_different_values : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();

         var msv1 = new ParameterStartValue().WithName("Tada");
         msv1.Path = new ObjectPath("Root", "Liver", "Plasma");
         msv1.StartValue = 2;

         var msv2 = new ParameterStartValue().WithName("Tada");
         msv2.Path = new ObjectPath("Root", "Liver", "Plasma");
         msv1.StartValue = 3;

         _object1 = msv1;
         _object2 = msv2;
      }

      [Observation]
      public void should_report_the_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_parameter_start_values_with_two_null_values : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();

         var msv1 = new ParameterStartValue().WithName("Tada");
         msv1.Path = new ObjectPath("Root", "Liver", "Plasma");
         msv1.StartValue = null;

         var msv2 = new ParameterStartValue().WithName("Tada");
         msv2.Path = new ObjectPath("Root", "Liver", "Plasma");
         msv2.StartValue = null;

         _object1 = msv1;
         _object2 = msv2;
      }

      [Observation]
      public void should_report_no_difference()
      {
         _report.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_comparing_two_parameter_start_values_with_one_null_values : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();

         var msv1 = new ParameterStartValue().WithName("Tada");
         msv1.Path = new ObjectPath("Root", "Liver", "Plasma");
         msv1.StartValue = 5;

         var msv2 = new ParameterStartValue().WithName("Tada");
         msv2.Path = new ObjectPath("Root", "Liver", "Plasma");
         msv2.StartValue = null;

         _object1 = msv1;
         _object2 = msv2;
      }


      [Observation]
      public void should_report_the_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_parmeter_start_values_with_different_formulas_and_only_value_compare :concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();

         _comparerSettings.FormulaComparison = FormulaComparison.Value;

         var msv1 = new ParameterStartValue().WithName("Tada").WithFormula(new ExplicitFormula("1+1"));
         msv1.Path = new ObjectPath("Root", "Liver", "Plasma");

         var msv2 = new ParameterStartValue().WithName("Tada").WithFormula(new ExplicitFormula("2"));
         msv2.Path = new ObjectPath("Root", "Liver", "Plasma");
         _object1 = msv1;
         _object2 = msv2;
      }

      [Observation]
      public void should_also_report_the_formula_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_parmeter_start_values_with_different_dimensions : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();

         _comparerSettings.FormulaComparison = FormulaComparison.Value;

         var dim = A.Fake<IDimension>();
         var msv1 = new ParameterStartValue().WithName("Tada").WithFormula(new ConstantFormula(2)).WithDimension(dim);
         msv1.Path = new ObjectPath("Root", "Liver", "Plasma");
         var dim2 = A.Fake<IDimension>();
         var msv2 = new ParameterStartValue().WithName("Tada").WithFormula(new ConstantFormula(2)).WithDimension(dim2);
         msv2.Path = new ObjectPath("Root", "Liver", "Plasma");
         _object1 = msv1;
         _object2 = msv2;
      }

      [Observation]
      public void should_report_the_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }
}