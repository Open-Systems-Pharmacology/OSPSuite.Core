//TODO SIMMODEL



//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using OSPSuite.Utility.Extensions;
//
//namespace OSPSuite.Core.Domain
//{
//   internal class PopulationDataSplitter
//   {
//      private readonly DataTable _populationData;
//      private readonly DataTable _agingData;
//      private readonly DataTable _initialValues;
//      private readonly int _numberOfCores;
//      private readonly int _numberOfSimulationsPerCore;
//
//      public PopulationDataSplitter(DataTable populationData, DataTable agingData, DataTable initialValues, int numberOfCores)
//      {
//         _populationData = populationData;
//         _agingData = agingData;
//         _initialValues = initialValues;
//         _numberOfCores = numberOfCores;
//         _numberOfSimulationsPerCore = getNumberOfSimulationsPerCore();
//      }
//
//      public void UpdateParametersAndInitialValuesForIndividual(int individualId, IList<IParameterProperties> variableParameters, IList<ISpeciesProperties> variableProperties)
//      {
//         fillParameterAndInitialValuesFor(individualId, variableParameters, variableProperties);
//      }
//
//      public IEnumerable<int> GetIndividualIdsFor(int coreIndex)
//      {
//         var rowIndices = getRowIndices(coreIndex);
//         return rowIndices.Select(rowIndex => _populationData.Rows[rowIndex])
//            .Select(individualIdFrom);
//      }
//
//      private int getNumberOfSimulationsPerCore()
//      {
//         int numberOfJobs = NumberOfIndividuals;
//
//         //cannot use more cores than jobs
//         int numberOfCoresToUse = Math.Min(numberOfJobs, _numberOfCores);
//
//         int rowsPerCore = numberOfJobs / numberOfCoresToUse;
//
//         if (rowsPerCore * _numberOfCores < numberOfJobs)
//            rowsPerCore = rowsPerCore + 1;
//
//         return rowsPerCore;
//      }
//
//      public int NumberOfIndividuals => _populationData.Rows.Count;
//
//      private IEnumerable<int> getRowIndices(int coreIndex)
//      {
//         int firstRow = _numberOfSimulationsPerCore * coreIndex;
//
//         //Zero based index
//         int lastRow = Math.Min(NumberOfIndividuals - 1, firstRow + _numberOfSimulationsPerCore - 1);
//
//         for (int i = firstRow; i <= lastRow; i++)
//            yield return i;
//      }
//
//      private static int individualIdFrom(DataRow popParametersDataRow)
//      {
//         return Convert.ToInt32(popParametersDataRow[Constants.Population.INDIVIDUAL_ID_COLUMN]);
//      }
//
//      private string parameterPathFrom(DataRow popParametersDataRow)
//      {
//         return popParametersDataRow[Constants.Population.PARAMETER_PATH_COLUMN].ToString();
//      }
//
//      private DataRow parameterDataRowFromIndividualId(int individualId)
//      {
//         return dataRowFromIndividualId(_populationData, individualId);
//      }
//
//      private DataRow initialValuesDataRowFromIndividualId(int individualId)
//      {
//         return dataRowFromIndividualId(_initialValues, individualId);
//      }
//
//      private DataRow dataRowFromIndividualId(DataTable dataTable, int individualId)
//      {
//         return dataTable.Rows.Cast<DataRow>().FirstOrDefault(r => individualIdFrom(r) == individualId);
//      }
//
//      private void fillParameterAndInitialValuesFor(int individualId, IEnumerable<IParameterProperties> parameters, IEnumerable<ISpeciesProperties> initialValueProperties)
//      {
//         var nonTableParameterValues = parameterDataRowFromIndividualId(individualId);
//
//         foreach (var parameterProperties in parameters)
//         {
//            // first, assign all table points for the parameter from the aging data (if any)
//            fillTableParameterValuesFor(parameterProperties, individualId);
//
//            // if no aging data found, parameter value is stored in the population data table
//            // (single value)
//            if (parameterProperties.TablePoints.Count == 0)
//               parameterProperties.Value = nonTableParameterValues[parameterProperties.Path].ConvertedTo<double>();
//         }
//
//         var initialValues = initialValuesDataRowFromIndividualId(individualId);
//         if (initialValues == null)
//            return;
//
//         foreach (var initialValue in initialValueProperties)
//         {
//            initialValue.Value = initialValues[initialValue.Path].ConvertedTo<double>();
//         }
//      }
//
//      private void fillTableParameterValuesFor(IParameterProperties parameterProperties, int individualId)
//      {
//         var parameterPath = parameterProperties.Path;
//
//         var tablePoints = from DataRow dr in _agingData.Rows
//            where individualIdFrom(dr) == individualId
//            where string.Equals(parameterPathFrom(dr), parameterPath)
//            select tablePointFromAgingData(dr);
//
//         parameterProperties.TablePoints = tablePoints.ToList();
//      }
//
//      private IValuePoint tablePointFromAgingData(DataRow dr)
//      {
//         var time = dr[Constants.Population.TIME_COLUMN].ConvertedTo<double>();
//         var value = dr[Constants.Population.VALUE_COLUMN].ConvertedTo<double>();
//
//         // for now always false for aging table
//         return new ValuePoint(time, value, restartSolver: false);
//      }
//
//      /// <summary>
//      ///    Get all parameter paths of parameters to be varied from PopulationData and AgingData.
//      ///    Parameter paths DO NOT contain root element (e.g. "Organism|Age")
//      /// </summary>
//      public IEnumerable<string> ParameterPathsToBeVaried()
//      {
//         var nonTableParametersToBeVaried = from DataColumn dc in _populationData.Columns
//            where !dc.ColumnName.Equals(Constants.Population.INDIVIDUAL_ID_COLUMN)
//            select dc.ColumnName;
//
//         var tableParametersToBeVaried = (from DataRow dr in _agingData.Rows
//            select dr[Constants.Population.PARAMETER_PATH_COLUMN].ToString()).Distinct();
//
//         return nonTableParametersToBeVaried.Union(tableParametersToBeVaried);
//      }
//
//      /// <summary>
//      ///    Get all variable paths of molecules to be varied from InitialValues.
//      /// </summary>
//      public IEnumerable<string> InitialValuesPathsToBeVaried()
//      {
//         return from DataColumn dc in _initialValues.Columns
//            where !dc.ColumnName.Equals(Constants.Population.INDIVIDUAL_ID_COLUMN)
//            select dc.ColumnName;
//      }
//   }
//}