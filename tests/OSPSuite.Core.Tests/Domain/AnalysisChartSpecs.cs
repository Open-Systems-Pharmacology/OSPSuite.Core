using System.Drawing;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain;

public abstract class concern_for_AnalysisChart : ContextSpecification<AnalysisChart>
{
   protected ParameterIdentification _parameterIdentification;
   protected ParameterIdentificationTimeProfileChart _peerTimeProfile;
   protected DataColumn _outputColumn;
   protected const string PATH = "Sim|Liver|Cell|Concentration";

   protected override void Context()
   {
      _parameterIdentification = new ParameterIdentification();

      //a DataColumn whose QuantityInfo path matches what the peer-lookup keys on
      var baseGrid = new BaseGrid("BaseGrid", DomainHelperForSpecs.TimeDimensionForSpecs());
      _outputColumn = new DataColumn("OutputCol", DomainHelperForSpecs.NoDimension(), baseGrid)
      {
         QuantityInfo = new QuantityInfo(new[] { "Sim", "Liver", "Cell", "Concentration" }, QuantityType.Drug)
      };

      //SUT is a non-Time-Profile chart so it has a reason to consult peers
      sut = new ParameterIdentificationPredictedVsObservedChart();
      _parameterIdentification.AddAnalysis(sut);
   }
}

public class When_a_peer_time_profile_chart_carries_a_curve_for_the_path : concern_for_AnalysisChart
{
   private static readonly Color CANONICAL_COLOR = Color.Magenta;
   private Color? _result;

   protected override void Context()
   {
      base.Context();

      _peerTimeProfile = new ParameterIdentificationTimeProfileChart();
      var peerCurve = new Curve { Name = "Peer" };
      var dimensionFactory = A.Fake<IDimensionFactory>();
      peerCurve.SetxData(_outputColumn.BaseGrid, dimensionFactory);
      peerCurve.SetyData(_outputColumn, dimensionFactory);
      peerCurve.Color = CANONICAL_COLOR;
      _peerTimeProfile.AddCurve(peerCurve, useAxisDefault: false);

      _parameterIdentification.AddAnalysis(_peerTimeProfile);
   }

   protected override void Because()
   {
      _result = sut.PeerColorForPath(PATH);
   }

   [Observation]
   public void should_return_the_color_from_the_time_profile_peer()
   {
      _result.ShouldBeEqualTo(CANONICAL_COLOR);
   }
}

public class When_only_non_time_profile_peers_carry_a_curve_for_the_path : concern_for_AnalysisChart
{
   private static readonly Color FALLBACK_COLOR = Color.Olive;
   private ParameterIdentificationResidualVsTimeChart _peerResidual;
   private Color? _result;

   protected override void Context()
   {
      base.Context();

      _peerResidual = new ParameterIdentificationResidualVsTimeChart();
      var peerCurve = new Curve { Name = "Peer" };
      var dimensionFactory = A.Fake<IDimensionFactory>();
      peerCurve.SetxData(_outputColumn.BaseGrid, dimensionFactory);
      peerCurve.SetyData(_outputColumn, dimensionFactory);
      peerCurve.Color = FALLBACK_COLOR;
      _peerResidual.AddCurve(peerCurve, useAxisDefault: false);

      _parameterIdentification.AddAnalysis(_peerResidual);
   }

   protected override void Because()
   {
      _result = sut.PeerColorForPath(PATH);
   }

   [Observation]
   public void should_fall_back_to_any_peer_chart_that_has_a_curve_for_the_path()
   {
      _result.ShouldBeEqualTo(FALLBACK_COLOR);
   }
}

public class When_no_peer_carries_a_curve_for_the_path : concern_for_AnalysisChart
{
   private Color? _result;

   protected override void Because()
   {
      _result = sut.PeerColorForPath(PATH);
   }

   [Observation]
   public void should_return_null_so_the_caller_can_pick_a_fresh_color()
   {
      _result.HasValue.ShouldBeFalse();
   }
}

public class When_the_chart_has_no_analysable : concern_for_AnalysisChart
{
   private Color? _result;

   protected override void Context()
   {
      base.Context();
      //detach the chart from the analysable so we exercise the early-return path
      sut.Analysable = null;
   }

   protected override void Because()
   {
      _result = sut.PeerColorForPath(PATH);
   }

   [Observation]
   public void should_return_null_without_throwing()
   {
      _result.HasValue.ShouldBeFalse();
   }
}

public class When_the_only_peer_with_a_matching_path_is_the_chart_itself : concern_for_AnalysisChart
{
   private static readonly Color SELF_COLOR = Color.SteelBlue;
   private Color? _result;

   protected override void Context()
   {
      base.Context();
      //add a curve on the SUT so we can prove the self-filter excludes our own curves
      var selfCurve = new Curve { Name = "Self" };
      var dimensionFactory = A.Fake<IDimensionFactory>();
      selfCurve.SetxData(_outputColumn.BaseGrid, dimensionFactory);
      selfCurve.SetyData(_outputColumn, dimensionFactory);
      selfCurve.Color = SELF_COLOR;
      sut.AddCurve(selfCurve, useAxisDefault: false);
   }

   protected override void Because()
   {
      _result = sut.PeerColorForPath(PATH);
   }

   [Observation]
   public void should_skip_the_chart_itself_and_return_null()
   {
      _result.HasValue.ShouldBeFalse();
   }
}