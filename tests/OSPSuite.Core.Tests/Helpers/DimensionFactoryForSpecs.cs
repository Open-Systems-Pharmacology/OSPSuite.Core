using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Helpers
{
   public static class DimensionFactoryForSpecs
   {
      private static readonly IDimensionFactory _factory = generateFactory();

      private static IDimensionFactory generateFactory()
      {
         var factory = new DimensionFactory();

         var amountDimension = new Dimension(new BaseDimensionRepresentation(), Constants.Dimension.AMOUNT, "Molar Volume");
         amountDimension.AddUnit(new Unit("mol", 1, 0));

         var massDimension = new Dimension(new BaseDimensionRepresentation(), DimensionNames.Mass, "g");
         massDimension.AddUnit(new Unit("kg", 1000, 0));
         massDimension.AddUnit(new Unit("mg", 0.001, 0));

         var concentrationDimension = new Dimension(new BaseDimensionRepresentation(), DimensionNames.Concentration, "mol");

         factory.AddDimension(massDimension);
         factory.AddDimension(concentrationDimension);
         factory.AddDimension(amountDimension);
         factory.AddDimension(new Dimension(new BaseDimensionRepresentation(), Constants.Dimension.AMOUNT_PER_TIME, "mol/min"));
         factory.AddDimension(new Dimension(new BaseDimensionRepresentation(), Constants.Dimension.VOLUME, "l"));
         factory.AddDimension(Constants.Dimension.NO_DIMENSION);

         var timeDimension = new Dimension(new BaseDimensionRepresentation(), Constants.Dimension.TIME, Constants.Dimension.Units.Minutes);
         timeDimension.AddUnit(new Unit(Constants.Dimension.Units.Days, 60 * 24, 0));
         timeDimension.AddUnit(new Unit(Constants.Dimension.Units.Hours, 60, 0));
         timeDimension.AddUnit(new Unit(Constants.Dimension.Units.Months, 60 * 24 * 30, 0));
         timeDimension.AddUnit(new Unit(Constants.Dimension.Units.Seconds, 1.0/60, 0));
         timeDimension.AddUnit(new Unit(Constants.Dimension.Units.Weeks, 60 * 24 * 7, 0));
         timeDimension.AddUnit(new Unit(Constants.Dimension.Units.Years, 60 * 24 * 365, 0));
         factory.AddDimension(timeDimension);
         
         return factory;
      }

      public static IDimensionFactory Factory => _factory;

      public static IDimension ConcentrationDimension => Factory.Dimension(DimensionNames.Concentration);

      public static IDimension TimeDimension => Factory.Dimension(Constants.Dimension.TIME);

      public static class DimensionNames
      {
         public static string Mass = "Mass";
         public static string Concentration = "Concentration";
      }
   }
}