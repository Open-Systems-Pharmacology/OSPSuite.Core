# MoBi Module Merge Behavior – Research Findings

This document summarises how the `MergeBehavior` property of a MoBi module affects the combination of each building block type during simulation creation.

## Background

When creating a simulation from multiple modules, each module carries a `MergeBehavior` value (defined on the module, not on individual building blocks):

| Value | Description |
|---|---|
| `Overwrite` | **Default.** Entities from this module replace entities with the same name that were contributed by earlier modules. |
| `Extend` | Entities from this module are merged into entities with the same name from earlier modules; new entities are added without removing existing ones. |

The merge logic is applied sequentially in the order the modules are selected for the simulation.

### General merge algorithm for named builders (Reactions, Transports, Observers, Events, Molecules)

The algorithm (`analyzeBuilderMerges` in `SimulationBuilder`) groups builders across all modules by **name**, then for each name group:

1. If only **one** module contributes that name → use it as-is.
2. If the **last** contributing module has `Overwrite` → use only that last builder, discard earlier ones.
3. If the **last** contributing module has `Extend` → find the last `Overwrite` module in the sequence as the *base* (or the first module if no `Overwrite` precedes it), then apply each subsequent `Extend` module's builder on top in order.

---

## Per Building-Block Findings

### Spatial Structure

Implemented in `SpatialStructureMerger` and `ContainerMergeTask`.

#### Overwrite

- Each top-level container from the module **replaces** the container with the same path in the accumulated structure.
- All descendants of the replaced container (sub-containers, parameters, formulas) that are not present in the overwriting module are **removed**.

#### Extend

- Each top-level container from the module is **recursively merged** into the existing container tree:
  - Sub-containers that do **not** exist in the target are **added**.
  - Sub-containers that **do** exist are recursively merged (same rules apply).
  - Non-container child entities (parameters, formulas, distributed parameters, …) are **added or replaced** by name.
- **Container mode** (`Physical` / `Logical`) is **overwritten** by the incoming value.
- **Container tags** are **extended** (tags from both modules are kept).

#### Neighborhoods (part of Spatial Structure)

| Property | Overwrite | Extend |
|---|---|---|
| Existing neighborhood with same name | Replaced entirely | Merged (parameters added/replaced) |
| Neighbor references (FirstNeighbor, SecondNeighbor) | Replaced | Replaced |

---

### Reactions

Implemented in `SimulationBuilder.mergeReactions`.

#### Overwrite

The entire reaction from the overwriting module is used; the reaction as defined in earlier modules is discarded.

#### Extend

Starting from the base reaction, each extending module applies the following changes:

| Property / Element | Extend behavior |
|---|---|
| **Educts** | Upsert by molecule name – replaced if the same molecule name exists, added otherwise. Existing educts for molecules not mentioned in the extending module are kept. |
| **Products** | Same upsert-by-molecule-name logic as Educts. |
| **Modifiers** | Set union – new modifier names are added; existing ones are not duplicated. Modifiers cannot be removed via Extend. |
| **Parameters** (child container) | Recursively merged via `ContainerMergeTask.MergeContainers`: new parameters added, existing replaced by name. |
| **Kinetic formula** | Overwritten by the incoming formula. |
| **ContainerCriteria** | Overwritten **if** the incoming value is non-`null`; otherwise unchanged. |
| **CreateProcessRateParameter** | Overwritten. |
| **ProcessRateParameterPersistable** | Overwritten. |
| **Icon** | Overwritten if the incoming value is non-`null`; otherwise unchanged. |
| **Description** | Overwritten if the incoming string is non-empty; otherwise unchanged. |
| **Dimension** | Overwritten if the incoming value is non-`null`; otherwise unchanged. |

> **Note:** Educts/Products can only be *added or replaced* – they cannot be *removed* via the `Extend` mode. To functionally exclude an educt or product, set its stoichiometry to 0.

---

### Passive Transports

Implemented in `SimulationBuilder.mergeTransports`.

#### Overwrite

The entire transport from the overwriting module is used.

#### Extend

| Property / Element | Extend behavior |
|---|---|
| **MoleculeList.ForAll** | Overwritten by the incoming value. |
| **Included molecule names** | Merged (union); names previously in the exclude list are moved back to the include list. |
| **Excluded molecule names** | Merged (union); names previously in the include list are moved to the exclude list. |
| **SourceCriteria.Operator** | Overwritten. |
| **SourceCriteria conditions** | Accumulated (new conditions appended; existing conditions are NOT removed). |
| **TargetCriteria.Operator** | Overwritten. |
| **TargetCriteria conditions** | Accumulated (same rule as SourceCriteria). |
| **Parameters** (child container) | Recursively merged: new parameters added, existing replaced by name. |
| **Kinetic formula** | Overwritten. |
| **CreateProcessRateParameter** | Overwritten. |
| **ProcessRateParameterPersistable** | Overwritten. |

---

### Observers

Implemented in `SimulationBuilder.mergeObservers`.

#### Overwrite

The entire observer from the overwriting module is used.

#### Extend

| Property / Element | Extend behavior |
|---|---|
| **MoleculeList** (ForAll, included, excluded names) | Same logic as Passive Transports (see above). |
| **Parameters** (child container) | Recursively merged: new parameters added, existing replaced by name. |

> **Note:** The observer formula and `InContainer` criteria are **not** currently modified by the extend strategy action (`mergeObservers` only merges the molecule list). Container children (parameters) are merged by the generic `tryExtendContainers` step that runs before every specific strategy.

---

### Events (Event Groups)

Implemented in `SimulationBuilder.mergeEvents`.

#### Overwrite

The entire event group from the overwriting module is used.

#### Extend

| Property / Element | Extend behavior |
|---|---|
| **EventGroupType** | Overwritten. |
| **SourceCriteria.Operator** | Overwritten. |
| **SourceCriteria conditions** | Accumulated (new conditions appended). |
| **Child containers / sub-events / parameters** | Recursively merged via `ContainerMergeTask`. |

---

### Molecules

Implemented in `SimulationBuilder.mergeMolecules`.

#### Overwrite

The entire molecule definition from the overwriting module is used.

#### Extend

| Property / Element | Extend behavior |
|---|---|
| **DefaultStartFormula** | Overwritten. |
| **Dimension** | Overwritten. |
| **DisplayUnit** | Overwritten. |
| **IsFloating** | Overwritten. |
| **IsXenobiotic** | Overwritten. |
| **QuantityType** | Overwritten. |
| **UsedCalculationMethods** | Replaced entirely (cleared, then incoming methods added). |
| **Parameters** (child container) | Recursively merged: new parameters added, existing replaced by name. |

---

### Parameter Values (PV)

Implemented in `SimulationBuilder.mergeParameterValueBuilders`.

- The `MergeBehavior` property of a module has **no effect** on Parameter Values building blocks.
- All PV entries from all selected modules are collected in module order. For the same parameter path, the **last** module's value wins (later module always overwrites earlier).
- Individual and Expression Profile parameter values are applied before module PV BBs (see priority order in the concept documentation).

---

### Initial Conditions (IC)

Implemented in `SimulationBuilder.mergeInitialConditions`.

- The `MergeBehavior` property of a module has **no effect** on Initial Conditions building blocks.
- Expression Profile IC entries are collected first; module IC entries are collected next. For the same molecule/container path, the **last** entry encountered wins (module IC overwrites Expression Profile IC if both define the same entry).

---

## Summary Table

| Building Block | Overwrite behavior | Extend: new entity | Extend: existing entity – what is overwritten | Extend: existing entity – what is accumulated / kept |
|---|---|---|---|---|
| **Spatial Structure – containers** | Whole sub-tree replaced | Added | Container mode, non-container children (params/formulas) | Tags, sub-containers (merged recursively) |
| **Spatial Structure – neighborhoods** | Neighborhood replaced by name | Added | Neighbor references, parameters replaced by name | — |
| **Reactions** | Whole reaction replaced | Added | Formula, CreateProcessRateParameter, ProcessRateParameterPersistable, ContainerCriteria (if non-null), stoichiometry of existing educts/products, existing parameters | Educts/products for other molecules kept; modifier set is extended |
| **Passive Transports** | Whole transport replaced | Added | ForAll, formula, CreateProcessRateParameter, criteria operators | Molecule include/exclude lists merged; criteria conditions accumulated; existing parameters merged |
| **Observers** | Whole observer replaced | Added | ForAll | Molecule include/exclude lists merged; existing parameters merged |
| **Events** | Whole event group replaced | Added | EventGroupType, SourceCriteria operator | SourceCriteria conditions accumulated; child parameters merged |
| **Molecules** | Whole molecule replaced | Added | All scalar properties, UsedCalculationMethods (full replacement) | Existing parameters merged |
| **Parameter Values** | N/A (last-value-wins regardless) | Added | Previous value for same path | — |
| **Initial Conditions** | N/A (last-value-wins regardless) | Added | Previous value for same path | — |

---

## Known Gaps / Issues

- **Reactions – Extend** was not implemented until recently (see [OSPSuite.Core#2640](https://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/2640)). The design specification in the OSMOSES documentation still states "Reactions are always overwritten by name", which no longer reflects the implemented behaviour.
- **Parameter Values and Initial Conditions** do not respect the `MergeBehavior` setting; the last-value-wins rule applies unconditionally.
- **Observers** – the `mergeObservers` strategy currently only merges the molecule list. The observer formula, `InContainer` criteria, and other observer properties are not touched by the extend action; they remain as defined in the base builder.
- The OSMOSES concept document (section "Reactions") should be updated to reflect the new `Extend` behavior.

---

## Source References

| File | Purpose |
|---|---|
| `src/OSPSuite.Core/Domain/Module.cs` | `MergeBehavior` enum and module definition |
| `src/OSPSuite.Core/Domain/ModuleConfiguration.cs` | Exposes `MergeBehavior` per module configuration |
| `src/OSPSuite.Core/Domain/Builder/SimulationBuilder.cs` | Core merge orchestration for all builder types |
| `src/OSPSuite.Core/Domain/Services/SpatialStructureMerger.cs` | Spatial structure and neighborhood merging |
| `src/OSPSuite.Core/Domain/Services/ContainerMergeTask.cs` | Low-level container merge logic |
| `src/OSPSuite.Core/Domain/Mappers/NeighborhoodCollectionToContainerMapper.cs` | Neighborhood-specific merge |
