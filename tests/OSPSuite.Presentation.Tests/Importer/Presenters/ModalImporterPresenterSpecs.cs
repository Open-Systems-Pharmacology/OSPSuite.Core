﻿using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public abstract class ConcernForModalImporterPresenter : ContextSpecification<ModalImporterPresenter>
   {
      protected ImportTriggeredEventArgs eventArgs;
      protected List<MetaDataCategory> metaDataCategories;
      protected DataImporterSettings dataImporterSettings;

      protected override void Context()
      {
         dataImporterSettings = new DataImporterSettings();
         base.Context();
         var mapper = A.Fake<IDataSetToDataRepositoryMapper>();
         sut = new ModalImporterPresenter(A.Fake<IModalImporterView>(), mapper);
         eventArgs = new ImportTriggeredEventArgs();
         var cache = new Cache<string, IDataSet>();
         var dataSet = new DataSet();
         dataSet.AddData(new List<ParsedDataSet>() 
         {
            new ParsedDataSet(new List<(string ColumnName, IList<string> ExistingValues)>(), A.Fake<IUnformattedData>(), new List<UnformattedRow>(), new Dictionary<ExtendedColumn, IList<SimulationPoint>>())
         });
         var dataSource = A.Fake<IDataSource>();
         eventArgs.DataSource = dataSource;
         A.CallTo(() => dataSource.DataSets).Returns(cache);
         cache.Add("sheet1", dataSet);
         var dataRepository = new DataRepository { Name = "name" };
         dataRepository.ExtendedProperties.Add(new ExtendedProperty<string>() { Name = "Molecule", Value = "Molecule1" });
         dataRepository.ExtendedProperties.Add(new ExtendedProperty<string>() { Name = "Mol weight", Value = 22.0.ToString() });

         var dataColumn = new BaseGrid("Time", A.Fake<IDimension>());
         var dataInfo = new DataInfo(ColumnOrigins.Undefined);
         dataColumn.DataInfo = dataInfo;
         dataRepository.Add(dataColumn);

         var moleculeDataColumn = new DataColumn("Measurement", A.Fake<IDimension>(), dataColumn);
         dataColumn.DataInfo = dataInfo;
         dataRepository.Add(moleculeDataColumn);
         A.CallTo(() => mapper.ConvertImportDataSet(A<ImportedDataSet>.Ignored)).Returns(dataRepository);

         var moleculeMetaDataCategory = createMetaDataCategory<string>("Molecule", isMandatory: true);
         moleculeMetaDataCategory.IsListOfValuesFixed = true;
         moleculeMetaDataCategory.DefaultValue = "Molecule1";
         moleculeMetaDataCategory.ListOfValues.Add("Molecule1", 6.0.ToString());
         moleculeMetaDataCategory.ShouldListOfValuesBeIncluded = true;
         moleculeMetaDataCategory.SelectDefaultValue = true;

         metaDataCategories = new List<MetaDataCategory>()
         {
            moleculeMetaDataCategory,
            createMetaDataCategory<string>("Mol weight", isMandatory: false)
         };
      }

      protected static MetaDataCategory createMetaDataCategory<T>(string descriptiveName, bool isMandatory = false, bool isListOfValuesFixed = false, Action<MetaDataCategory> fixedValuesRetriever = null)
      {
         var category = new MetaDataCategory
         {
            Name = descriptiveName,
            DisplayName = descriptiveName,
            Description = descriptiveName,
            MetaDataType = typeof(T),
            IsMandatory = isMandatory,
            IsListOfValuesFixed = isListOfValuesFixed
         };

         fixedValuesRetriever?.Invoke(category);

         return category;
      }
   }

   public class When_importing_data : ConcernForModalImporterPresenter
   {
      [Observation]
      public void sets_molWeight_from_molecule()
      {
         dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation = "Molecule";
         var importerPresenter = A.Fake<IImporterPresenter>();
         var result = sut.ImportDataSets(
            importerPresenter,
            metaDataCategories,
            new List<ColumnInfo>(),
            dataImporterSettings
         );
         importerPresenter.OnTriggerImport += Raise.With(eventArgs);
         var molWeight = 6.0;
         Assert.IsTrue(result.DataRepositories.All(dr => dr.AllButBaseGrid().All(x => x.DataInfo.MolWeight == molWeight)));
      }

      [Observation]
      public void should_throw_when_inconsistent_mol_weight()
      {
         dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation = "Molecule";
         dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation = "Mol weight";
         var importerPresenter = A.Fake<IImporterPresenter>();
         var result = sut.ImportDataSets(
            importerPresenter,
            metaDataCategories,
            new List<ColumnInfo>(),
            dataImporterSettings
         );
         The.Action(() => importerPresenter.OnTriggerImport += Raise.With(eventArgs)).ShouldThrowAn<InconsistenMoleculeAndMoleWeightException>();
      }

      [Observation]
      public void sets_molWeight_from_molWeight()
      {
         dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation = "Mol weight";
         var importerPresenter = A.Fake<IImporterPresenter>();
         var result = sut.ImportDataSets(
            importerPresenter,
            metaDataCategories,
            new List<ColumnInfo>(),
            dataImporterSettings
         );
         importerPresenter.OnTriggerImport += Raise.With(eventArgs);
         var molWeight = 22.0;
         Assert.IsTrue(result.DataRepositories.All(dr => dr.AllButBaseGrid().All(x => x.DataInfo.MolWeight == molWeight)));
      }
   }

}