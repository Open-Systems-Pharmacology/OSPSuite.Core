//TODO SIMMODEL
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using OSPSuite.BDDHelper;
//using OSPSuite.BDDHelper.Extensions;
//using FakeItEasy;
//using SimModelNET;
//using OSPSuite.Core.Domain;
//using OSPSuite.Core.Domain.Services;
//using OSPSuite.Core.Extensions;
//using OSPSuite.Engine.Domain;
//using OSPSuite.Helpers;
//
//namespace OSPSuite.Core
//{
//   internal abstract class concern_for_PopulationDataSplitterSpecs : ContextSpecification<PopulationDataSplitter>
//   {
//      protected DataTable _agingData;
//      protected DataTable _populationData;
//      protected DataTable _initialValuesData;
//      protected const int _numberOfCores = 2;
//      protected const string _agingPath1 = "XXX";
//      protected const string _agingPath2 = "YYY";
//      protected const string _nonAgingPath2 = "ZZZ";
//      protected const string _agingPath3 = "aaa";
//      protected const string _initialValuePath1 = "molecule1";
//      protected const string _initialValuePath2 = "molecule2";
//      protected string _nonAgingPath = new []{ ConstantsForSpecs.Organism,ConstantsForSpecs.BW}.ToPathString();
//
//      protected override void Context()
//      {
//         _populationData = createPopTableParameters();
//         _agingData = createAgingData();
//         _initialValuesData = createInitialValues();
//         sut = new PopulationDataSplitter(_populationData, _agingData,_initialValuesData,  _numberOfCores);
//      }
//
//      private DataTable createInitialValues()
//      {
//         var dt = new DataTable("InitialValues");
//
//         dt.Columns.Add(Constants.Population.INDIVIDUAL_ID_COLUMN);
//         dt.Columns.Add(_initialValuePath1);
//         dt.Columns.Add(_initialValuePath2);
//
//         dt.Rows.Add(0, 2, 22);
//         dt.Rows.Add(1, 3, 33);
//         dt.Rows.Add(2, 4, 44);
//         dt.Rows.Add(3, 5, 55);
//         dt.Rows.Add(4, 6, 66);
//         dt.Rows.Add(5, 7, 77);
//         dt.Rows.Add(6, 8, 88);
//         return dt;
//      }
//
//      private DataTable createAgingData()
//      {
//         var dt = new DataTable("AgingParams");
//
//         dt.Columns.Add(Constants.Population.INDIVIDUAL_ID_COLUMN);
//         dt.Columns.Add(Constants.Population.PARAMETER_PATH_COLUMN);
//         dt.Columns.Add(Constants.Population.TIME_COLUMN);
//         dt.Columns.Add(Constants.Population.VALUE_COLUMN);
//
//         int individualId = 0;
//         dt.Rows.Add(individualId, _agingPath1, 1, "1.0");
//         dt.Rows.Add(individualId, _agingPath1, "2.0", 2);
//         dt.Rows.Add(individualId, _agingPath2, "11.00", "11.00");
//         dt.Rows.Add(individualId, _agingPath2, "22.00", "22.00");
//         dt.Rows.Add(individualId, _agingPath2, 33, 33);
//         dt.Rows.Add(individualId, _agingPath3, 6.66, 6.66);
//
//         individualId = 1;
//         dt.Rows.Add(individualId, _agingPath1, 1, 1);
//         dt.Rows.Add(individualId, _agingPath1, 2, 2);
//         dt.Rows.Add(individualId, _agingPath2, 1, 1);
//         dt.Rows.Add(individualId, _agingPath2, 2, 2);
//         dt.Rows.Add(individualId, _agingPath3, 3.33, 3.33);
//
//         return dt;
//      }
//
//      private DataTable createPopTableParameters()
//      {
//         var dt = new DataTable("PopParams");
//
//         dt.Columns.Add(Constants.Population.INDIVIDUAL_ID_COLUMN);
//         dt.Columns.Add(_nonAgingPath);
//         dt.Columns.Add(_nonAgingPath2);
//
//         dt.Rows.Add(0, "24.0", 666);
//         dt.Rows.Add(1, 25.0, 666);
//         dt.Rows.Add(2, 25.0, 666);
//         dt.Rows.Add(3, 25.0, 666);
//         dt.Rows.Add(4, 25.0, 666);
//         dt.Rows.Add(5, 25.0, 666);
//         dt.Rows.Add(6, 25.0, 666);
//         return dt;
//      }
//   }
//
//   internal class When_filling_parameters_and_initial_values_for_the_first_individual : concern_for_PopulationDataSplitterSpecs
//   {
//      private IList<IParameterProperties> _variableParameters;
//      private IList<ISpeciesProperties> _variableInitialValues;
//
//      protected override void Context()
//      {
//         base.Context();
//
//         _variableParameters = new List<IParameterProperties>();
//
//         var parameterInfo1 = A.Fake<IParameterProperties>();
//         A.CallTo(()=>parameterInfo1.IsTable).Returns(false);
//         A.CallTo(() => parameterInfo1.Path).Returns(_nonAgingPath);
//
//         var parameterInfo2 = A.Fake<IParameterProperties>();
//         A.CallTo(() => parameterInfo2.IsTable).Returns(true);
//         A.CallTo(() => parameterInfo2.Path).Returns(_agingPath1);
//
//         var parameterInfo3 = A.Fake<IParameterProperties>();
//         A.CallTo(() => parameterInfo3.IsTable).Returns(true);
//         A.CallTo(() => parameterInfo3.Path).Returns(_agingPath2);
//
//         //setting non aging parameter (=no table points), while
//         //the corresponding parameter is marked as a table
//         var parameterInfo4 = A.Fake<IParameterProperties>();
//         A.CallTo(() => parameterInfo4.IsTable).Returns(true);
//         A.CallTo(() => parameterInfo4.Path).Returns(_nonAgingPath2);
//
//         //setting aging parameter (table points available),
//         //while the corresponding parameter is NOT marked as a table
//         var parameterInfo5 = A.Fake<IParameterProperties>();
//         A.CallTo(() => parameterInfo5.IsTable).Returns(false);
//         A.CallTo(() => parameterInfo5.Path).Returns(_agingPath3);
//
//         _variableParameters.Add(parameterInfo1);
//         _variableParameters.Add(parameterInfo2);
//         _variableParameters.Add(parameterInfo3);
//         _variableParameters.Add(parameterInfo4);
//         _variableParameters.Add(parameterInfo5);
//
//         _variableInitialValues = new List<ISpeciesProperties>();
//
//         var initialValueInfo1 = A.Fake<ISpeciesProperties>();
//         A.CallTo(() => initialValueInfo1.Path).Returns(_initialValuePath1);
//
//         var initialValueInfo2 = A.Fake<ISpeciesProperties>();
//         A.CallTo(() => initialValueInfo2.Path).Returns(_initialValuePath2);
//
//         _variableInitialValues.Add(initialValueInfo1);
//         _variableInitialValues.Add(initialValueInfo2);
//      }
//
//      protected override void Because()
//      {
//         sut.UpdateParametersAndInitialValuesForIndividual(0, _variableParameters,_variableInitialValues);
//      }
//
//      [Observation]
//      public void should_fill_correct_value_for_variable_initial_values()
//      {
//         _variableInitialValues.Count.ShouldBeEqualTo(2);
//
//         _variableInitialValues[0].Value.ShouldBeEqualTo(2);
//         _variableInitialValues[1].Value.ShouldBeEqualTo(22);
//      }
//
//      [Observation]
//      public void should_fill_correct_value_for_nonTableParameters()
//      {
//         var nonTableParameters = _variableParameters.Where(x => !x.TablePoints.Any()).ToList();
//         nonTableParameters.Count.ShouldBeEqualTo(2);
//
//         nonTableParameters[0].Value.ShouldBeEqualTo(24);
//         nonTableParameters[1].Value.ShouldBeEqualTo(666);
//      }      
//
//      [Observation]
//      public void should_fill_correct_value_for_nonTableParameters_in_case_of_corresponding_parameter_in_basis_individual_is_table()
//      {
//         var nonTableParameters = _variableParameters.Where(x => x.Path.Equals(_nonAgingPath2)).ToList();
//         nonTableParameters.Count.ShouldBeEqualTo(1);
//
//         nonTableParameters[0].TablePoints.Count.ShouldBeEqualTo(0);
//         nonTableParameters[0].Value.ShouldBeEqualTo(666);         
//      }
//
//      [Observation]
//      public void should_fill_correct_values_for_table_parameters()
//      {
//         var tableParameter1 = _variableParameters.First(x => x.IsTable && x.Path.Equals(_agingPath1));
//         var tablePoints1 = tableParameter1.TablePoints;
//
//         tablePoints1.Count.ShouldBeEqualTo(2);
//         tablePoints1[0].X.ShouldBeEqualTo(1);
//         tablePoints1[0].Y.ShouldBeEqualTo(1);
//         tablePoints1[1].X.ShouldBeEqualTo(2);
//         tablePoints1[1].Y.ShouldBeEqualTo(2);
//
//         var tableParameter2 = _variableParameters.First(x => x.IsTable && x.Path.Equals(_agingPath2));
//         var tablePoints2 = tableParameter2.TablePoints;
//
//         tablePoints2.Count.ShouldBeEqualTo(3);
//         tablePoints2[0].X.ShouldBeEqualTo(11);
//         tablePoints2[0].Y.ShouldBeEqualTo(11);
//         tablePoints2[1].X.ShouldBeEqualTo(22);
//         tablePoints2[1].Y.ShouldBeEqualTo(22);
//         tablePoints2[2].X.ShouldBeEqualTo(33);
//         tablePoints2[2].Y.ShouldBeEqualTo(33);
//
//         var tableParameter3 = _variableParameters.First(x=>x.Path.Equals(_agingPath3));
//         var tablePoints3 = tableParameter3.TablePoints;
//
//         tablePoints3.Count.ShouldBeEqualTo(1);
//         tablePoints3[0].X.ShouldBeEqualTo(6.66);
//      }
//   }
//
//   internal class When_filling_initial_values_from_empty_table : concern_for_PopulationDataSplitterSpecs
//   {
//      private IList<ISpeciesProperties> _variableInitialValues;
//
//      protected override void Context()
//      {
//         base.Context();
//
//         _initialValuesData = createEmptyInitialValuesData();
//         sut = new PopulationDataSplitter(_populationData, _agingData, _initialValuesData, _numberOfCores);
//      }
//
//      protected override void Because()
//      {
//         _variableInitialValues = new List<ISpeciesProperties>();
//         var initialValueInfo1 = A.Fake<ISpeciesProperties>();
//         A.CallTo(() => initialValueInfo1.Path).Returns("Trululu");
//         _variableInitialValues.Add(initialValueInfo1);
//
//         sut.UpdateParametersAndInitialValuesForIndividual(0, new List<IParameterProperties>(), _variableInitialValues);
//      }
//
//      private DataTable createEmptyInitialValuesData()
//      {
//         var dt = new DataTable("InitialValues");
//         dt.Columns.Add(Constants.Population.INDIVIDUAL_ID_COLUMN);
//         dt.Columns.Add("Trululu");
//         return dt;
//      }
//
//      [Observation]
//      public void should_not_crash_with_empty_table()
//      {
//
//      }
//   }
//
//   internal class When_geting_Individual_id_s_for_first_core : concern_for_PopulationDataSplitterSpecs
//   {
//      private IEnumerable<int> _result;
//
//      protected override void Because()
//      {
//         _result = sut.GetIndividualIdsFor(0);
//      }
//
//      [Observation]
//      public void should_return_the_First_IDs()
//      {
//         _result.ShouldOnlyContain(0, 1, 2, 3);
//      }
//
//   }
//
//   internal class When_geting_Individual_id_s_for_last_core : concern_for_PopulationDataSplitterSpecs
//   {
//      private IEnumerable<int> _result;
//
//      protected override void Because()
//      {
//         _result = sut.GetIndividualIdsFor(1);
//      }
//
//      [Observation]
//      public void should_return_the_Last_IDs()
//      {
//         _result.ShouldOnlyContain(4, 5, 6);
//      }
//   }
//}