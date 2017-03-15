using System;

namespace OSPSuite.Core.Domain.Services
{
   public interface IEntitiesInSimulationRetriever
   {
      /// <summary>
      ///    Returns all <see cref="IParameter" /> defined in the <paramref name="simulation" /> fulfilling the given
      ///    <paramref name="predicate" />
      ///    (search performed using complete hierarchy)
      /// </summary>
      PathCache<IParameter> ParametersFrom(ISimulation simulation, Func<IParameter, bool> predicate);


      /// <summary>
      ///    Returns all <see cref="IQuantity" /> defined under the <paramref name="simulation" />  (search performed using
      ///    complete hierarchy)
      /// </summary>
      PathCache<IQuantity> QuantitiesFrom(ISimulation simulation);


      /// <summary>
      ///    Returns all <typeparamref name="TEntity" /> defined in the <paramref name="simulation" />  (search performed using
      ///    complete hierarchy)
      /// </summary>
      PathCache<TEntity> EntitiesFrom<TEntity>(ISimulation simulation) where TEntity : class, IEntity;

      /// <summary>
      ///    Returns all  <typeparamref name="TEntity" /> defined in the <paramref name="simulation" /> fulfilling the given
      ///    <paramref name="predicate" />
      ///    (search performed using complete hierarchy)
      /// </summary>
      PathCache<TEntity> EntitiesFrom<TEntity>(ISimulation simulation, Func<TEntity, bool> predicate) where TEntity : class, IEntity;
 
      /// <summary>
      ///    Returns all <see cref="IParameter" /> defined in the <paramref name="simulation" /> (search performed using
      ///    complete hierarchy)
      /// </summary>
      PathCache<IParameter> ParametersFrom(ISimulation simulation);

      /// <summary>
      ///    Returns all <see cref="IQuantity" /> that were selected when calculating the <paramref name="simulation" />  (e.g.
      ///    Persistable=true)
      /// </summary>
      PathCache<IQuantity> OutputsFrom(ISimulation simulation);

      /// <summary>
      ///    Returns all <see cref="IQuantity" /> defined under the <paramref name="simulation" />  (search performed using
      ///    complete hierarchy)
      /// </summary>
      PathCache<IQuantity> QuantitiesFrom(IModelCoreSimulation simulation);
   }

   public class EntitiesInSimulationRetriever : IEntitiesInSimulationRetriever
   {
      protected readonly IEntityPathResolver _entityPathResolver;
      protected readonly IContainerTask _containerTask;

      public EntitiesInSimulationRetriever(IEntityPathResolver entityPathResolver, IContainerTask containerTask)
      {
         _entityPathResolver = entityPathResolver;
         _containerTask = containerTask;
      }

      public PathCache<IQuantity> OutputsFrom(ISimulation simulation)
      {
         var allQuantities = QuantitiesFrom(simulation);
         var outputs = new PathCache<IQuantity>(_entityPathResolver);

         foreach (var selectedQuantity in simulation.OutputSelections)
         {
            var quantity = allQuantities[selectedQuantity.Path];
            if (quantity != null)
               outputs.Add(quantity);
         }

         return outputs;
      }

      public PathCache<IQuantity> QuantitiesFrom(IModelCoreSimulation simulation)
      {
         return EntitiesFrom<IQuantity>(simulation.Model.Root);
      }

      public PathCache<IQuantity> QuantitiesFrom(ISimulation simulation)
      {
         return EntitiesFrom<IQuantity>(simulation);
      }

      public PathCache<IParameter> ParametersFrom(ISimulation simulation)
      {
         return EntitiesFrom<IParameter>(simulation);
      }
      
      public PathCache<IParameter> ParametersFrom(ISimulation simulation, Func<IParameter, bool> predicate)
      {
         return EntitiesFrom(simulation, predicate);
      }

      public PathCache<TEntity> EntitiesFrom<TEntity>(ISimulation simulation) where TEntity : class, IEntity
      {
         return EntitiesFrom<TEntity>(simulation, x => true);
      }

      public PathCache<TEntity> EntitiesFrom<TEntity>(ISimulation simulation, Func<TEntity, bool> predicate) where TEntity : class, IEntity
      {
         return EntitiesFrom(simulation.Model.Root, predicate);
      }

      public PathCache<TEntity> EntitiesFrom<TEntity>(IContainer container) where TEntity : class, IEntity
      {
         return EntitiesFrom<TEntity>(container, x => true);
      }

      public PathCache<TEntity> EntitiesFrom<TEntity>(IContainer container, Func<TEntity, bool> predicate) where TEntity : class, IEntity
      {
         return _containerTask.CacheAllChildrenSatisfying(container, predicate);
      }

   }
}