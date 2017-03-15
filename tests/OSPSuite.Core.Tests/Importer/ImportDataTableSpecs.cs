using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Importer;

namespace OSPSuite.Importer
{
   public abstract class concern_for_ImportDataTable : ContextSpecification<ImportDataTable>
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         sut = new ImportDataTable();
      }
   }

   
   public class BasicTests : concern_for_ImportDataTable
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         sut.Columns.Add(new ImportDataColumn
                            {
                               DataType = typeof (DateTime),
                               ColumnName = "DateColumn",
                               DisplayName = "myDate",
                               Required = true,
                               SkipNullValueRows = false
                            });
         sut.Columns.Add(new ImportDataColumn
                            {
                               DataType = typeof (string),
                               ColumnName = "StringColumn",
                               DisplayName = "myString",
                               Required = false,
                               SkipNullValueRows = true
                            });

         sut.Columns.Add(new ImportDataColumn
                            {
                               DataType = typeof (double),
                               ColumnName = "NumberColumn",
                               DisplayName = "myNumber",
                               Required = true,
                               SkipNullValueRows = true
                            });
         sut.Columns.Add(new ImportDataColumn
                            {
                               DataType = typeof (bool),
                               ColumnName = "BoolColumn",
                               DisplayName = "myBool",
                               Required = false,
                               SkipNullValueRows = false 
                            });

         // dimension
         var dim = new Dimension
                      {
                         Name = "MolWeight",
                         DisplayName = "MolWeight",
                         IsDefault = true,
                         InputParameters =
                            new List<InputParameter>
                               {
                                  new InputParameter
                                     {
                                        Name = "Density",
                                        DisplayName = "Density",
                                        Unit = new Unit {Name = "g/l", DisplayName = "Gram per liter", IsDefault = true}
                                     }
                               },
                         Units =
                            new List<Unit>
                               {
                                  new Unit {Name = "mg/mol", DisplayName = "Milligram per mol", IsDefault = false},
                                  new Unit {Name = "g/mol", DisplayName = "Gram per mol", IsDefault = true}
                               }
                      };
         sut.Columns.ItemByIndex(1).Dimensions = new List<Dimension> {dim};


         // metadata
         var metaData = new MetaDataTable();
         metaData.Columns.Add(new MetaDataColumn
                                 {
                                    DataType = typeof (string),
                                    ColumnName = "varGender",
                                    DisplayName = "Gender",
                                    Description = "The gender can be Male of Female.",
                                    ListOfValues =
                                       new Dictionary<string, string> {{"Male", "Male"}, {"Female", "Female"}},
                                    IsListOfValuesFixed = true,
                                    Required = false
                                 });

         metaData.Columns.Add(new MetaDataColumn
                                 {
                                    DataType = typeof (string),
                                    ColumnName = "varSpecies",
                                    DisplayName = "Species",
                                    Description = "The name of kind of species.",
                                    ListOfValues = new Dictionary<string, string>
                                                      {
                                                         {"Human", "Human"},
                                                         {"Beagle", "Beagle"},
                                                         {"Mouse", "Mouse"},
                                                         {"Rate", "Rate"}
                                                      },
                                    IsListOfValuesFixed = true,
                                    Required = true
                                 });

         sut.MetaData = metaData;
         sut.Columns.ItemByIndex(1).MetaData = metaData.Clone();
      }

      [Observation]
      public void Required_should_toggle_AllowDBNull()
      {
         sut.Columns.ContainsName("DateColumn").ShouldBeTrue();
         var col = sut.Columns.ItemByName("DateColumn");
         sut.Columns.ContainsColumn(col).ShouldBeTrue();
         col.Required = false;
         col.Required.ShouldBeFalse();
         col.AllowDBNull.ShouldBeTrue();
         col.AllowDBNull = false;
         col.Required.ShouldBeTrue();
      }

      [Observation]
      public void should_retrieve_ImportDataTable_as_Table()
      {
         var col = sut.Columns.ItemByName("NumberColumn");
         col.Table.ShouldBeAnInstanceOf<ImportDataTable>();
      }

      [Observation]
      public void Clone_should_create_a_copy()
      {
         sut.Sheet = "mySource";
         var myCopy = sut.Clone();
         ReferenceEquals(myCopy, sut).ShouldBeFalse();
         myCopy.Source.ShouldBeEqualTo(sut.Source);
         myCopy.Columns.Count.ShouldBeEqualTo(4);
         myCopy.Columns.ContainsName("DateColumn").ShouldBeTrue();
         myCopy.Columns.ContainsName("StringColumn").ShouldBeTrue();
         myCopy.Columns.ContainsName("NumberColumn").ShouldBeTrue();
         myCopy.Columns.ContainsName("BoolColumn").ShouldBeTrue();

         // test meta data on table
         myCopy.MetaData.ShouldNotBeNull();
         myCopy.MetaData.Columns.Count.ShouldBeEqualTo(2);
         myCopy.MetaData.Columns.ContainsName("varGender").ShouldBeTrue();
         myCopy.MetaData.Columns.ContainsName("varSpecies").ShouldBeTrue();
         myCopy.MetaData.Columns.ItemByIndex(0).Table.ShouldBeAnInstanceOf<MetaDataTable>();

         var metaCol = myCopy.MetaData.Columns.ItemByName("varGender");
         metaCol.DisplayName.ShouldBeEqualTo("Gender");
         metaCol.Description.ShouldBeEqualTo("The gender can be Male of Female.");
         metaCol.ListOfValues.Keys.ShouldOnlyContain("Male", "Female");
         metaCol.Required.ShouldBeFalse();

         metaCol = myCopy.MetaData.Columns.ItemByName("varSpecies");
         metaCol.DisplayName.ShouldBeEqualTo("Species");
         metaCol.Description.ShouldBeEqualTo("The name of kind of species.");
         metaCol.ListOfValues.Keys.ShouldOnlyContain("Human", "Beagle", "Mouse", "Rate");
         metaCol.Required.ShouldBeTrue();

         // test meta data on column
         var col = myCopy.Columns.ItemByIndex(1);
         col.MetaData.ShouldNotBeNull();
         col.MetaData.Columns.Count.ShouldBeEqualTo(2);
         col.MetaData.Columns.ContainsName("varGender").ShouldBeTrue();
         col.MetaData.Columns.ContainsName("varSpecies").ShouldBeTrue();

         metaCol = col.MetaData.Columns.ItemByName("varGender");
         metaCol.DisplayName.ShouldBeEqualTo("Gender");
         metaCol.ListOfValues.Keys.ShouldOnlyContain("Male", "Female");
         metaCol.Required.ShouldBeFalse();

         metaCol = col.MetaData.Columns.ItemByName("varSpecies");
         metaCol.DisplayName.ShouldBeEqualTo("Species");
         metaCol.ListOfValues.Keys.ShouldOnlyContain("Human", "Beagle", "Mouse", "Rate");
         metaCol.Required.ShouldBeTrue();

         for (var i = 0; i<col.Dimensions.Count; i++)
         {
            col.Dimensions[i].DisplayName.ShouldBeEqualTo(sut.Columns.ItemByIndex(1).Dimensions[i].DisplayName);
            col.Dimensions[i].Name.ShouldBeEqualTo(sut.Columns.ItemByIndex(1).Dimensions[i].Name);
            col.Dimensions[i].IsDefault.ShouldBeEqualTo(sut.Columns.ItemByIndex(1).Dimensions[i].IsDefault);
            for (var j = 0; j < col.Dimensions[i].Units.Count; j++)
               col.Dimensions[i].Units[j].ShouldBeEqualTo(sut.Columns.ItemByIndex(1).Dimensions[i].Units[j]);
            for (var j = 0; j < col.Dimensions[i].InputParameters.Count; j++)
               col.Dimensions[i].InputParameters[j].ShouldBeEqualTo(
                  sut.Columns.ItemByIndex(1).Dimensions[i].InputParameters[j]);
         }
      }

      [Observation]
      public void should_throw_exception_MissingDefaultDimension()
      {
         IList<Dimension> dimensions = new List<Dimension>(1)
                                          {
                                             new Dimension
                                                {
                                                   DisplayName = "DimA",
                                                   IsDefault = false,
                                                   Name = "A",
                                                   Units =
                                                      new List<Unit>
                                                         {
                                                            new Unit {DisplayName = "UnitA", IsDefault = true, Name = "A"}
                                                         }
                                                }
                                          };
         var importDataColumn = new ImportDataColumn();
         The.Action(() => importDataColumn.Dimensions = dimensions).ShouldThrowAn<MissingDefaultDimension>();
      }

      [Observation]
      public void should_throw_exception_MissingDimensionName()
      {
         IList<Dimension> dimensions = new List<Dimension>(1)
                                          {
                                             new Dimension
                                                {
                                                   DisplayName = "DimA",
                                                   IsDefault = true,
                                                   Units =
                                                      new List<Unit>
                                                         {
                                                            new Unit {DisplayName = "UnitA", IsDefault = true, Name = "A"}
                                                         }
                                                }
                                          };
         var importDataColumn = new ImportDataColumn();
         The.Action(() => importDataColumn.Dimensions = dimensions).ShouldThrowAn<MissingDimensionName>();
      }

      [Observation]
      public void should_throw_exception_MissingUnitName()
      {
         IList<Dimension> dimensions = new List<Dimension>(1)
                                          {
                                             new Dimension
                                                {
                                                   DisplayName = "DimA",
                                                   IsDefault = true,
                                                   Name = "A",
                                                   Units =
                                                      new List<Unit>
                                                         {
                                                            new Unit {DisplayName = "UnitA", IsDefault = true}
                                                         }
                                                }
                                          };
         var importDataColumn = new ImportDataColumn();
         The.Action(() => importDataColumn.Dimensions = dimensions).ShouldThrowAn<MissingUnitName>();
      }

      [Observation]
      public void should_throw_exception_MultipleDefaultDimensionsFound()
      {
         IList<Dimension> dimensions = new List<Dimension>(2)
                                          {
                                             new Dimension
                                                {
                                                   DisplayName = "DimA",
                                                   IsDefault = true,
                                                   Name = "A",
                                                   Units =
                                                      new List<Unit>
                                                         {
                                                            new Unit {DisplayName = "UnitA", IsDefault = true, Name = "A"}
                                                         }
                                                },
                                             new Dimension
                                                {
                                                   DisplayName = "DimB",
                                                   IsDefault = true,
                                                   Name = "B",
                                                   Units =
                                                      new List<Unit>
                                                         {
                                                            new Unit {DisplayName = "UnitA", IsDefault = true, Name = "A"}
                                                         }
                                                }
                                          };
         var importDataColumn = new ImportDataColumn();
         The.Action(() => importDataColumn.Dimensions = dimensions).ShouldThrowAn<MultipleDefaultDimensionsFound>();
      }

      [Observation]
      public void should_throw_exception_MissingUnitsForDimension()
      {
         IList<Dimension> dimensions = new List<Dimension>(1)
                                          {
                                             new Dimension {DisplayName = "DimA", IsDefault = true, Name = "A"}
                                          };
         var importDataColumn = new ImportDataColumn();
         The.Action(() => importDataColumn.Dimensions = dimensions).ShouldThrowAn<MissingUnitsForDimension>();
      }

      [Observation]
      public void should_throw_exception_MultipleDefaultUnitsFoundForDimension()
      {
         var units = new List<Unit>(2)
                        {
                           new Unit {DisplayName = "UnitA", IsDefault = true, Name = "A"},
                           new Unit {DisplayName = "UnitB", IsDefault = true, Name = "B"}
                        };
         var dim = new Dimension { Name = "DimA", IsDefault = true, Units = units};

         IList<Dimension> dimensions = new List<Dimension> {dim};
         var importDataColumn = new ImportDataColumn();
         The.Action(() => importDataColumn.Dimensions = dimensions).ShouldThrowAn
            <MultipleDefaultUnitsFoundForDimension>();

      }

      [Observation]
      public void should_throw_exception_MissingDefaultUnitFoundForDimension()
      {
         var units = new List<Unit>(2)
                        {
                           new Unit {DisplayName = "UnitA", IsDefault = false, Name = "A"},
                           new Unit {DisplayName = "UnitB", IsDefault = false, Name = "B"}
                        };
         var dim = new Dimension { Name = "DimA", IsDefault = true, Units = units };

         IList<Dimension> dimensions = new List<Dimension> { dim };
         var importDataColumn = new ImportDataColumn();
         The.Action(() => importDataColumn.Dimensions = dimensions).ShouldThrowAn<MissingDefaultUnitForDimension>();
      }

      [Observation]
      public void should_throw_exception_DublicatedUnitFoundForDimension_For_Name()
      {
         var units = new List<Unit>(2)
                        {
                           new Unit {DisplayName = "UnitA", IsDefault = false, Name = "A"},
                           new Unit {DisplayName = "UnitB", IsDefault = true, Name = "A"}
                        };
         var dim = new Dimension { Name = "DimA", IsDefault = true, Units = units };

         IList<Dimension> dimensions = new List<Dimension> { dim };
         var importDataColumn = new ImportDataColumn();
         The.Action(() => importDataColumn.Dimensions = dimensions).ShouldThrowAn<DublicatedUnitForDimension>();
      }

      [Observation]
      public void should_throw_exception_DublicatedUnitFoundForDimension_For_DisplayName()
      {
         var units = new List<Unit>(2)
                        {
                           new Unit {DisplayName = "UnitA", IsDefault = false, Name = "A"},
                           new Unit {DisplayName = "UnitA", IsDefault = true, Name = "B"}
                        };
         var dim = new Dimension { Name = "DimA", IsDefault = true, Units = units };

         IList<Dimension> dimensions = new List<Dimension> { dim };
         var importDataColumn = new ImportDataColumn();
         The.Action(() => importDataColumn.Dimensions = dimensions).ShouldThrowAn<DublicatedUnitForDimension>();
      }

      [Observation]
      public void should_throw_exception_DublicatedInputParameterFoundForDimension_For_Name()
      {
         IList<Dimension> dimensions = new List<Dimension>
                                          {
                                             new Dimension
                                                {
                                                   Name = "DimA",
                                                   IsDefault = true,
                                                   Units = new List<Unit>(2)
                                                              {
                                                                 new Unit
                                                                    {
                                                                       DisplayName = "UnitA",
                                                                       IsDefault = false,
                                                                       Name = "A"
                                                                    },
                                                                 new Unit
                                                                    {
                                                                       DisplayName = "UnitB",
                                                                       IsDefault = true,
                                                                       Name = "B"
                                                                    }
                                                              },
                                                   InputParameters = new List<InputParameter>(2)
                                                                        {
                                                                           new InputParameter
                                                                              {
                                                                                 DisplayName = "Molecular Weight",
                                                                                 Name = "Velocity",
                                                                                 Unit = new Unit
                                                                                           {
                                                                                              DisplayName =
                                                                                                 "Gram per Mol",
                                                                                              Name = "g/mol"
                                                                                           }
                                                                              },
                                                                           new InputParameter
                                                                              {
                                                                                 DisplayName = "Velocity",
                                                                                 Name = "Velocity",
                                                                                 Unit =
                                                                                    new Unit
                                                                                       {
                                                                                          DisplayName = "Gram per Litre",
                                                                                          Name = "g/l"
                                                                                       }
                                                                              }
                                                                        }

                                                }
                                          };
         var importDataColumn = new ImportDataColumn();
         The.Action(() => importDataColumn.Dimensions = dimensions).ShouldThrowAn
            <DublicatedInputParameterForDimension>();
      }

      [Observation]
      public void should_throw_exception_DublicatedInputParameterFoundForDimension_For_DisplayName()
      {
         IList < Dimension > dimensions = new List<Dimension>
                                          {
                                             new Dimension
                                                {
                                                   Name = "DimA",
                                                   IsDefault = true,
                                                   Units = new List<Unit>(2)
                                                              {
                                                                 new Unit
                                                                    {
                                                                       DisplayName = "UnitA",
                                                                       IsDefault = false,
                                                                       Name = "A"
                                                                    },
                                                                 new Unit
                                                                    {
                                                                       DisplayName = "UnitB",
                                                                       IsDefault = true,
                                                                       Name = "B"
                                                                    }
                                                              },
                                                   InputParameters = new List<InputParameter>(2)
                                                                        {
                                                                           new InputParameter
                                                                              {
                                                                                 DisplayName = "Molecular Weight",
                                                                                 Name = "MolWeight",
                                                                                 Unit = new Unit
                                                                                           {
                                                                                              DisplayName =
                                                                                                 "Gram per Mol",
                                                                                              Name = "g/mol"
                                                                                           }
                                                                              },
                                                                           new InputParameter
                                                                              {
                                                                                 DisplayName = "Molecular Weight",
                                                                                 Name = "Velocity",
                                                                                 Unit =
                                                                                    new Unit
                                                                                       {
                                                                                          DisplayName = "Gram per Litre",
                                                                                          Name = "g/l"
                                                                                       }
                                                                              }
                                                                        }

                                                }
                                          };
         var importDataColumn = new ImportDataColumn();
         The.Action(() => importDataColumn.Dimensions = dimensions).ShouldThrowAn
            <DublicatedInputParameterForDimension>();
      }


      [Observation]
      public void check_ImportDataColumnCollection()
      {
         var myCopy = sut.Clone();
         var cols = myCopy.Columns;
         var col = cols.ItemByIndex(0);
         var count = cols.Count;

         cols.ContainsColumn(col).ShouldBeTrue();
         cols.ContainsName(col.ColumnName).ShouldBeTrue();
         cols.Remove(col);
         cols.ContainsColumn(col).ShouldBeFalse();
         cols.Add(col);
         cols.ContainsColumn(col).ShouldBeTrue();
         cols.RemoveAt(0);
         cols.ContainsColumn(col).ShouldBeTrue();
         cols.Count.ShouldBeEqualTo(count-1);
         cols.Clear();
         cols.Count.ShouldBeEqualTo(0);
         cols.Add(col);
      }

      [Observation]
      public void check_MetaDataColumnCollection()
      {
         var myCopy = sut.Clone();
         var cols = myCopy.MetaData.Columns;
         var col = cols.ItemByIndex(0);
         var count = cols.Count;

         cols.ContainsColumn(col).ShouldBeTrue();
         cols.ContainsName(col.ColumnName).ShouldBeTrue();
         cols.Remove(col);
         cols.ContainsColumn(col).ShouldBeFalse();
         cols.Add(col);
         cols.ContainsColumn(col).ShouldBeTrue();
         cols.RemoveAt(0);
         cols.ContainsColumn(col).ShouldBeTrue();
         cols.Count.ShouldBeEqualTo(count - 1);
         cols.Clear();
         cols.Count.ShouldBeEqualTo(0);
         cols.Add(col);
      }
   }

}	