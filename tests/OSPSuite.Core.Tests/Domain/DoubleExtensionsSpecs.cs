using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using System.Globalization;
using System.Linq;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
    public abstract class concern_for_DoubleExtensions : StaticContextSpecification
    {
        protected double _tolerance;

        protected override void Context()
        {
            _tolerance = 1e-10;
        }
    }

    public class When_comparing_two_indentical_values : concern_for_DoubleExtensions
    {
        private double _value;

        protected override void Context()
        {
            base.Context();
            _value = 1.154684798654;
        }

        [Observation]
        public void should_return_that_the_values_are_equal()
        {
            _value.EqualsByTolerance(_value, _tolerance).ShouldBeTrue();
        }
    }

    public class When_comparing_two_different_values : concern_for_DoubleExtensions
    {
        private double _value1;
        private double _value2;
        private double _value3;

        protected override void Context()
        {
            base.Context();
            _value1 = 1.154684798654;
            _value2 = 1.15445798654;
            _value3 = 5;
        }

        [Observation]
        public void should_return_that_the_values_are_not_equal()
        {
            _value1.EqualsByTolerance(_value2, _tolerance).ShouldBeFalse();
            _value1.EqualsByTolerance(_value3, _tolerance).ShouldBeFalse();
        }
    }

    public class When_comparing_two_values_equals_up_to_the_tolerance : concern_for_DoubleExtensions
    {
        private double _value1;
        private double _value2;

        protected override void Context()
        {
            base.Context();
            _value1 = 1.15468479865412;
            _value2 = 1.15468479865434;
        }

        [Observation]
        public void should_return_that_the_values_are_not_equal()
        {
            _value1.EqualsByTolerance(_value2, _tolerance).ShouldBeTrue();
        }
    }

    public class When_comparing_two_arrays_of_same_length_with_equal_values : concern_for_DoubleExtensions
    {
        private double[] _values1;
        private double[] _values2;

        protected override void Context()
        {
            base.Context();
            _values1 = new double[] {1, 2, 3};
            _values2 = new double[] {1, 2, 3};
        }

        [Observation]
        public void should_return_that_the_arrays_equal()
        {
            _values1.EqualsByTolerance(_values2, _tolerance).ShouldBeTrue();
        }
    }

    
    public class When_comparing_two_arrays_of_same_length_with_different_values : concern_for_DoubleExtensions
    {
        private double[] _values1;
        private double[] _values2;

        protected override void Context()
        {
            base.Context();
            _values1 = new double[] { 1, 2, 3 };
            _values2 = new double[] { 1, 3, 3 };
        }

        [Observation]
        public void should_return_that_the_arrays_are_different()
        {
            _values1.EqualsByTolerance(_values2, _tolerance).ShouldBeFalse();
        }
    }

    
    public class When_comparing_two_arrays_of_different_length : concern_for_DoubleExtensions
    {
        private double[] _values1;
        private double[] _values2;

        protected override void Context()
        {
            base.Context();
            _values1 = new double[] { 1, 2, 3 };
            _values2 = new double[] { 1, 3, 3,4 };
        }

        [Observation]
        public void should_return_that_the_arrays_are_different()
        {
            _values1.EqualsByTolerance(_values2, _tolerance).ShouldBeFalse();
        }
    }

    
    public class When_comparing_an_array_with_a_null_array : concern_for_DoubleExtensions
    {
        private double[] _values1;
        private double[] _values2;

        protected override void Context()
        {
            base.Context();
            _values1 = new double[] { 1, 2, 3 };
            _values2 = null;
        }

        [Observation]
        public void should_return_that_the_arrays_are_different()
        {
            _values1.EqualsByTolerance(_values2, _tolerance).ShouldBeFalse();
            _values2.EqualsByTolerance(_values1, _tolerance).ShouldBeFalse();
        }
    }

    
    public class When_comparing_two_null_arrays : concern_for_DoubleExtensions
    {
        private double[] _values1;
        private double[] _values2;

        protected override void Context()
        {
            base.Context();
            _values1 = null;
            _values2 = null;
        }

        [Observation]
        public void should_return_that_the_arrays_are_equal()
        {
            _values1.EqualsByTolerance(_values2, _tolerance).ShouldBeTrue();
            _values2.EqualsByTolerance(_values1, _tolerance).ShouldBeTrue();
        }
    }

    public class When_converting_a_float_to_double_using_a_culture_with_a_comma_as_decimal_separator : concern_for_DoubleExtensions
    {
        private CultureInfo _originalCulture;

        protected override void Context()
        {
            base.Context();
            _originalCulture = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = new CultureInfo("de-DE");
        }

        public override void Cleanup()
        {
            base.Cleanup();
            CultureInfo.CurrentCulture = _originalCulture;
        }

        //https://github.com/Open-Systems-Pharmacology/PK-Sim/issues/3392
        [Observation]
        public void should_not_interpret_the_decimal_separator_as_a_group_separator()
        {
            0.1f.ToDouble().ShouldBeEqualTo(0.1, 1e-10);
            0.18f.ToDouble().ShouldBeEqualTo(0.18, 1e-10);
            1.5f.ToDouble().ShouldBeEqualTo(1.5, 1e-10);
        }

        [Observation]
        public void should_keep_distinct_values_distinct()
        {
            var values = new[] {0.1f, 0.2f, 0.3f, 1f}.Select(x => x.ToDouble()).ToList();
            values.Distinct().Count().ShouldBeEqualTo(values.Count);
        }
    }
}
