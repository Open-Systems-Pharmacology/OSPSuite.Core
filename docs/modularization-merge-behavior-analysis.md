# Modularization Merge/Combination Behavior Analysis

> Investigated on branch `copilot/research-spatial-structure-merge` (V13 codebase).  
> All line-number citations refer to files under `src/OSPSuite.Core/`.

---

## 1. Core Architecture

### `MergeBehavior` enum

**File:** `Domain/Module.cs`, lines 18–34

```csharp
public enum MergeBehavior
{
    Overwrite,   // default – last definition wins
    Extend       // merge incoming builder on top of accumulated base
}
```

`MergeBehavior` is a **module-level** property (`Module.MergeBehavior`).  
It applies uniformly to every building block inside that module (reactions, molecules,
transports, observers, events, spatial structure).

### Central merge dispatch — `SimulationBuilder.analyzeBuilderMerges`

**File:** `Domain/Builder/SimulationBuilder.cs`, lines 432–484

For every named builder the algorithm works as follows:

1. **Only one definition** → used as-is, no merge required.
2. **Last definition has `Overwrite`** → only the last builder is kept; all earlier definitions are discarded entirely.
3. **Last definition has `Extend`** → walk backwards from position `[n-2]` to find the last `Overwrite` entry (becomes the *base*); every subsequent `Extend` builder is applied on top via `extendStrategyAction`.

`extendStrategyAction` always calls both:
- `tryExtendContainers` – recursively merges child-container structure (fires only when both target and source implement `IContainer`)
- the type-specific merge method (`mergeReactions`, `mergeMolecules`, `mergeTransports`, `mergeObservers`, `mergeEvents`)

---

## 2. Spatial Structure Merge

**Files:** `Domain/Services/SpatialStructureMerger.cs`, `Domain/Services/ContainerMergeTask.cs`

### Entry point

`SpatialStructureMerger.createMergedContainerStructureInRoot` (lines 53–94):

- The **first** spatial structure's top containers are mapped to model containers and added directly to root.
- Every subsequent structure's top containers are processed by `tryMergeTopContainerInStructure`.

### Per-container routing (`replaceOrMergeContainerIntoParent`, lines 141–148)

```csharp
if (mergeBehavior == MergeBehavior.Extend || containerToMerge.IsNamed(Constants.MOLECULE_PROPERTIES))
    _containerMergeTask.AddOrMergeContainer(parentContainer, containerToMerge);
else
    _containerMergeTask.AddOrReplaceInContainer(parentContainer, containerToMerge);
```

### `ContainerMergeTask.MergeContainers` (Extend path, lines 42–68)

1. **Update container properties** (`updateContainerProperties`, lines 65–69):
   - `targetContainer.Mode = containerToMerge.Mode` — Mode/ContainerType **overwritten**
   - `containerToMerge.Tags.Each(targetContainer.AddTag)` — Tags **added** (not cleared first)
2. **Recurse into child containers** — if child with same name exists, `MergeContainers` is called recursively; otherwise the new child container is added.
3. **Non-container children** (parameters, formula objects, etc.) — all processed with `AddOrReplaceInContainer` (replace by name or add new).

### 2a. MoleculeProperties (`GlobalMoleculeDependentProperties`)

`SpatialStructureMerger.cs`, lines 79–93. A hardcoded loop collects ALL spatial structures'
`GlobalMoleculeDependentProperties` and merges them with `MergeContainers` — **the `MergeBehavior`
flag is never consulted**. The comment in the code explicitly states:
> "For molecule properties, we always merged as we used to and never replace"

- ✅ **Doc claim "always extended in both modes" — CONFIRMED**

### 2b. Parameters within containers

- **Extend mode:** `AddOrReplaceInContainer` replaces same-named parameters; new parameters are added; base-only parameters that are absent from the incoming container are **kept**.
- **Overwrite mode:** the container is fully replaced via `AddOrReplaceInContainer` (lines 33–40 of `ContainerMergeTask`), so all contained parameters come exclusively from the incoming container.

- ✅ **Doc claim "always overwritten in both modes" — BROADLY CONFIRMED** (Extend caveat: base-only params survive)

### 2c. Container Mode

`updateContainerProperties` (line 67): `targetContainer.Mode = containerToMerge.Mode`.  
In Overwrite mode the whole container is replaced.

- ✅ **Doc claim "overwritten in both modes" — CONFIRMED**

### 2d. Tags

`updateContainerProperties` (line 68): `containerToMerge.Tags.Each(targetContainer.AddTag)`.  
Tags are **accumulated** in Extend mode. In Overwrite mode the container is replaced wholesale,
so only the new container's tags survive.

- ✅ **Doc claim "extended in Extend, overwritten in Overwrite" — CONFIRMED**

### 2e. Neighborhoods

`NeighborhoodCollectionToContainerMapper.mergeNeighborhoodsInStructure` (lines 80–91):

```csharp
if (mergeBehavior == MergeBehavior.Extend)
{
    var mergeNeighbor = _containerMergeTask.AddOrMergeContainer(neighborhoods, neighborhoodToMerge) as Neighborhood;
    updateNeighbors(mergeNeighbor, neighborhoodToMerge);   // overwrites FirstNeighbor / SecondNeighbor
}
else
    _containerMergeTask.AddOrReplaceInContainer(neighborhoods, neighborhoodToMerge);
```

- ✅ **Doc claim "extended in Extend, overwritten by name in Overwrite" — CONFIRMED**  
  Note: in Extend mode the neighbor references (`FirstNeighbor`, `SecondNeighbor`) are always overwritten.

### 2f. Formulas

No dedicated formula-protection code exists in either `SpatialStructureMerger` or `ContainerMergeTask`.  
Formula objects embedded as child entities follow the same `AddOrReplaceInContainer` path as any
other non-container entity. In Overwrite mode the container (and everything inside it) is fully
replaced.

- ❌ **Doc claim "never overwritten in Overwrite mode" — NOT SUPPORTED BY CODE**

---

## 3. Molecules Merge

**File:** `Domain/Builder/SimulationBuilder.cs`, `mergeMolecules`, lines 266–279

`MoleculeBuilder` extends `Container`, so `tryExtendContainers` fires and recursively merges
child containers (parameters, etc.) before the type-specific logic runs.

Explicit property assignments in `mergeMolecules` (executed in Extend scenario):

```csharp
target.DefaultStartFormula = incoming.DefaultStartFormula;  // overwritten
target.Dimension            = incoming.Dimension;           // overwritten
target.DisplayUnit          = incoming.DisplayUnit;         // overwritten
target.IsFloating           = incoming.IsFloating;          // overwritten (Stationary flag)
target.IsXenobiotic         = incoming.IsXenobiotic;        // overwritten
target.QuantityType         = incoming.QuantityType;        // overwritten (Molecule type!)
target.ClearUsedCalculationMethods();                       // cleared
incoming.UsedCalculationMethods.Each(x => target.AddUsedCalculationMethod(x.Clone())); // replaced
```

### 3a. Molecule type (`QuantityType`)

Line 274 explicitly sets `target.QuantityType = incoming.QuantityType` in Extend mode.

- ❌ **Doc claim "NOT changed in Extend, overwritten in Overwrite" — DISCREPANCY**  
  The code **does** overwrite `QuantityType` in Extend mode.

### 3b. Stationary (`IsFloating`)

Line 272: `target.IsFloating = incoming.IsFloating` — overwritten in Extend.

- ✅ **Doc claim "always overwritten in both" — CONFIRMED**

### 3c. Calculation methods

Lines 277–278: cleared then replaced in Extend; last builder's methods in Overwrite.

- ✅ **Doc claim "always overwritten in both" — CONFIRMED**

### 3d. Parameters

`tryExtendContainers` → `MergeContainers`: same-named parameters replaced, new ones added,
base-only parameters kept. In Overwrite: last builder's parameters only.

- ✅ **Doc claim "Extend: overwritten; Overwrite: only params in last module" — BROADLY CONFIRMED**  
  (Extend caveat: base-only parameters are not removed)

### 3e. Parameter type (`ParameterBuildMode`)

`AddOrReplaceInContainer` replaces the whole `IParameter` object, so `BuildMode` travels with it.

- ✅ **Doc claim "always overwritten" — CONFIRMED**

---

## 4. Reactions

**File:** `Domain/Builder/SimulationBuilder.cs`, `mergeReactions`, lines 172–196

`ReactionBuilder` → `ProcessBuilder` → `Container`, so `tryExtendContainers` fires.

In `mergeReactions` (Extend scenario):

| Property | Behavior |
|---|---|
| `Formula` | Overwritten (line 176) |
| `CreateProcessRateParameter` | Overwritten |
| Educts / Products | Upserted by molecule name (`upsertPartners`, lines 198–222) |
| Modifiers | **Extended** — new names added, existing kept (lines 183–188) |
| `ContainerCriteria` | Overwritten if non-null (lines 194–195) |
| Icon / Description / Dimension | Overwritten if incoming non-null/non-empty |
| Parameters | Recursively merged via `tryExtendContainers` |

- ❌ **Doc claim "always overwritten by name" — OVERSIMPLIFICATION**  
  In Overwrite mode the last definition wins (correct). In Extend mode, reactions are **merged**:
  educts/products are upserted, modifiers are extended, parameters are recursively merged.

---

## 5. Passive Transports

**File:** `Domain/Builder/SimulationBuilder.cs`, `mergeTransports`, lines 224–234

`TransportBuilder` → `ProcessBuilder` → `Container`, so `tryExtendContainers` fires.

In `mergeTransports` (Extend scenario):

```csharp
mergeMoleculeLists(target, source);
mergeDescriptorCriteria(target.SourceCriteria, source.SourceCriteria);  // Operator overwritten, conditions ADDED
mergeDescriptorCriteria(target.TargetCriteria, source.TargetCriteria);  // same
target.CreateProcessRateParameter = source.CreateProcessRateParameter;
target.Formula = source.Formula;                             // kinetic equation overwritten
```

`mergeMoleculeLists` (lines 242–258):
- `ForAll` (All checkbox): overwritten — `targetMoleculeList.ForAll = sourceMoleculeList.ForAll`
- Include names: added to target's include list (removed from exclude first)
- Exclude names: added to target's exclude list (removed from include first)

- ❌ **Doc claim "Extend is identical to Overwrite" — INCORRECT**  
  In Overwrite the last builder is used wholesale; in Extend, `SourceCriteria`/`TargetCriteria`
  conditions are **accumulated** and molecule lists are **extended**.
- ✅ **Doc claim "Kinetic equation overwritten" — CONFIRMED** (effectively same in both modes)
- ❌ **Doc claim "Parameters list overwritten (removing missing params)" — INCORRECT for Extend**  
  Base-only parameters survive in Extend mode.
- ❌ **Doc claim "Source/Target lists overwritten" — INCORRECT for Extend**  
  `mergeDescriptorCriteria` *adds* conditions (operator is overwritten); lists are extended not replaced.
- ✅ **Doc claim "Include/Exclude molecule lists always extended, All checkbox overwritten" — CONFIRMED**

---

## 6. Observers

**File:** `Domain/Builder/SimulationBuilder.cs`, `mergeObservers`, lines 261–264

```csharp
private void mergeObservers(ObserverBuilder target, BuilderSource<ObserverBuilder> source)
{
    mergeMoleculeLists(target, source.Builder);
}
```

**Critical structural fact:** `ObserverBuilder` extends `Entity`, **not** `Container`
(`Domain/Builder/ObserverBuilder.cs`, line 12). Therefore `tryExtendContainers` does **nothing**
for observers — the entire container-merge path is skipped.

`ObserverBuilder.Formula` (monitoring equation) and `ObserverBuilder.ContainerCriteria`
(InContainer criteria) are both **properties**, not child entities, and `mergeObservers` does not
touch them. They are therefore **unchanged** when a later Extend module provides a different observer.

In Extend mode:
- Molecule lists: extended (ForAll overwritten, include/exclude names accumulated)
- Formula: **unchanged** (stays as BASE builder's formula)
- ContainerCriteria: **unchanged** (stays as BASE builder's criteria)

In Overwrite mode: last builder used as-is (formula, criteria, molecule lists all from last builder).

- ❌ **Doc claim "Extend is identical to Overwrite" — INCORRECT**  
  In Extend mode, formula and criteria come from the BASE builder; in Overwrite they come from the LAST builder.
- ❌ **Doc claim "Monitoring equation overwritten" — INCORRECT for Extend mode**
- ❌ **Doc claim "In container list overwritten" — INCORRECT for Extend mode**
- ✅ **Doc claim "Include/Exclude extended, All checkbox overwritten" — CONFIRMED**

---

## 7. Events

**File:** `Domain/Builder/SimulationBuilder.cs`, `mergeEvents`, lines 164–170

`EventGroupBuilder` and `EventBuilder` both extend `Container`, so `tryExtendContainers` fires
and recursively merges the full child-container tree.

`EventAssignmentBuilder` extends `Entity` (non-container child of `EventBuilder`) — it is subject
to `AddOrReplaceInContainer` (replaced by name).

`EventBuilder.Formula` (the IfCondition / start-condition formula) is a **property**, not a child
entity, so `MergeContainers` cannot update it. It always stays as the BASE builder's formula
in Extend mode.

In `mergeEvents` (Extend scenario):

| Element | Behavior |
|---|---|
| `EventGroupType` | Overwritten (line 167) |
| `SourceCriteria` | Operator overwritten, conditions **added** (line 169) |
| Child EventGroupBuilders / EventBuilders | Tree recursively extended |
| Parameters (child entities of EventBuilder) | Replaced by name or added (base-only kept) |
| `EventAssignmentBuilder`s | Replaced by name |
| `EventBuilder.Formula` (start condition) | **Not updated** — stays as base builder's formula |

- ✅ **Doc claim "Overwrite: complete tree overwritten, params overwritten, assignments overwritten" — CONFIRMED**
- ✅ **Doc claim "Extend: tree extended" — CONFIRMED**
- ✅ **Doc claim "Extend: parameters extended" — CONFIRMED** (base-only params kept)
- ✅ **Doc claim "Events start condition equation: Changes are not applied!" — CONFIRMED**  
  `EventBuilder.Formula` is a property; `MergeContainers` cannot touch it.
- ✅ **Doc claim "assignments overwritten" — CONFIRMED**

---

## 8. Parameter Values Application Order

**File:** `Domain/Services/QuantityValuesUpdater.cs`, `UpdateQuantitiesValues`, lines 57–71

```csharp
updateMoleculeAmountFromInitialConditions(modelConfiguration);

// Expression profiles BEFORE individual (comment: individual may overwrite for aging)
updateParameterFromExpressionProfiles(valueUpdater);
updateParameterFromIndividualValues(valueUpdater);

// PV BBs applied last = highest priority
updateParameterValueFromParameterValues(valueUpdater);
```

Effective override chain (later = higher priority):

```
BB default values → Expression Profiles → Individual → Parameter Value BBs
```

- ❌ **Doc claim "BB values → Individual → Expression Profile → PV BBs" — INCORRECT**  
  The code applies Expression Profiles **before** Individual. Correct order is:  
  **BB values → Expression Profile → Individual → PV BBs**

---

## 9. Initial Conditions Application Order

**File:** `Domain/Builder/SimulationBuilder.cs`, `mergeInitialConditions`, lines 310–321

```csharp
mergeParameterValueBuilders(x => x.SelectedInitialConditions, initialConditionsFromConfigurationsCache);
var expressionProfileInitialConditions = allInitialConditionsFromExpressionProfileSources();
expressionProfileInitialConditionsCache.AddRange(...);

// Concat order: expression profiles first → IC BBs second → IC BBs win on duplicates
_initialConditions.AddRange(expressionProfileInitialConditionsCache.Concat(initialConditionsFromConfigurationsCache));
```

`PathAndValueEntityCache<T>` is a `MergeCache` that overwrites by path key on `Add`.
Concatenating expression-profile ICs first and IC BBs second means IC BBs overwrite expression
profiles on any duplicate path. Within the IC BBs, later modules overwrite earlier ones (same
`MergeCache` semantics).

Effective priority:
```
Molecule builder defaults → Expression Profile ICs → IC BBs (later modules win)
```

- ✅ **Doc claim "Molecules BB → Expression Profiles → IC BBs" — CONFIRMED**

---

## 10. Summary: Documentation Accuracy Table

| # | Area | Documentation Claim | Code Location | Status | Notes |
|---|------|-------------------|---------------|--------|-------|
| 1 | Spatial Structure – MoleculeProperties | Always extended in both modes | `SpatialStructureMerger.cs` L79–93 | ✅ Confirmed | Hardcoded; MergeBehavior never consulted |
| 2 | Spatial Structure – Parameters | Always overwritten in both modes | `ContainerMergeTask.cs` L33–40, L62 | ✅ Broadly confirmed | Extend: base-only params survive |
| 3 | Spatial Structure – Container Mode | Overwritten in both modes | `ContainerMergeTask.cs` L67 | ✅ Confirmed | |
| 4 | Spatial Structure – Tags | Extended in Extend, overwritten in Overwrite | `ContainerMergeTask.cs` L68 | ✅ Confirmed | |
| 5 | Spatial Structure – Neighborhoods | Extended in Extend, overwritten by name in Overwrite | `NeighborhoodCollectionToContainerMapper.cs` L80–91 | ✅ Confirmed | Neighbor refs also overwritten in Extend |
| 6 | Spatial Structure – Formulas | Never overwritten in Overwrite mode | `ContainerMergeTask.cs` L33–40 | ❌ Not supported | No formula protection; Overwrite replaces entire container |
| 7 | Molecules – Molecule type | NOT changed in Extend, overwritten in Overwrite | `SimulationBuilder.cs` L274 | ❌ Discrepancy | `QuantityType` IS overwritten in Extend mode |
| 8 | Molecules – Stationary | Always overwritten in both modes | `SimulationBuilder.cs` L272 | ✅ Confirmed | |
| 9 | Molecules – Calculation methods | Always overwritten in both modes | `SimulationBuilder.cs` L277–278 | ✅ Confirmed | |
| 10 | Molecules – Parameters | Extend: overwritten; Overwrite: only last-module params | `SimulationBuilder.cs` L143–161 | ✅ Broadly confirmed | Extend: base-only params survive |
| 11 | Molecules – Parameter type | Always overwritten | `ContainerMergeTask.cs` L33–40 | ✅ Confirmed | |
| 12 | Reactions | Always overwritten by name | `SimulationBuilder.cs` L172–196 | ❌ Oversimplified | Extend: educts/products upserted, modifiers extended, params merged |
| 13 | Passive Transports – Extend vs Overwrite | Extend identical to Overwrite | `SimulationBuilder.cs` L224–258 | ❌ Incorrect | Extend: SourceCriteria/TargetCriteria extended, molecule lists accumulated |
| 14 | Passive Transports – Kinetic equation | Overwritten | `SimulationBuilder.cs` L233 | ✅ Confirmed | Same in both modes |
| 15 | Passive Transports – Parameters | Overwritten (removing missing) | `SimulationBuilder.cs` L143–161 | ❌ Incorrect for Extend | Base-only params survive in Extend |
| 16 | Passive Transports – Source/Target lists | Overwritten | `SimulationBuilder.cs` L236–239 | ❌ Incorrect for Extend | Conditions are added; operator overwritten |
| 17 | Passive Transports – Include/Exclude + All | Extended / overwritten (resp.) | `SimulationBuilder.cs` L242–258 | ✅ Confirmed | |
| 18 | Observers – Extend vs Overwrite | Extend identical to Overwrite | `SimulationBuilder.cs` L261–264 | ❌ Incorrect | In Extend, formula and ContainerCriteria stay as base |
| 19 | Observers – Monitoring equation | Overwritten | `SimulationBuilder.cs` L261–264 | ❌ Incorrect for Extend | `ObserverBuilder.Formula` is a property; not touched in Extend |
| 20 | Observers – InContainer list | Overwritten | `SimulationBuilder.cs` L261–264 | ❌ Incorrect for Extend | `ContainerCriteria` is a property; not touched in Extend |
| 21 | Observers – Include/Exclude + All | Extended / overwritten (resp.) | `SimulationBuilder.cs` L242–258 | ✅ Confirmed | |
| 22 | Events – Overwrite mode | Complete tree overwritten, params overwritten, assignments overwritten | `SimulationBuilder.cs` L432–484 | ✅ Confirmed | |
| 23 | Events – Extend: tree | Extended | `SimulationBuilder.cs` L143–161 | ✅ Confirmed | |
| 24 | Events – Extend: parameters | Extended | `ContainerMergeTask.cs` L62 | ✅ Confirmed | Base-only params kept |
| 25 | Events – Start condition equation | Not applied in Extend | `EventBuilder.cs` (property) | ✅ Confirmed | `EventBuilder.Formula` is a property; MergeContainers cannot touch it |
| 26 | Events – Assignments | Overwritten | `ContainerMergeTask.cs` L62 | ✅ Confirmed | `EventAssignmentBuilder` is a non-container Entity child |
| 27 | Parameter Values order | BB → Individual → Expression Profile → PV BBs | `QuantityValuesUpdater.cs` L57–71 | ❌ Incorrect | Actual order: BB → Expression Profile → Individual → PV BBs |
| 28 | Initial Conditions order | Molecules BB → Expression Profiles → IC BBs | `SimulationBuilder.cs` L310–321 | ✅ Confirmed | |
