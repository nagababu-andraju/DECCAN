# Technical Architecture

## OVERVIEW
DECCAN HEAT relies on Unity 6 LTS and the Universal Render Pipeline (URP). Our primary objective is to maintain scalable mobile performance while presenting an expansive open-world urban environment.

## CORE SYSTEMS

1. **Rendering**:
   - Universal Render Pipeline (URP).
   - Strict draw call budgets and texture streaming limits.

2. **World Streaming**:
   - Built on Unity Addressables.
   - Sectors load/unload dynamically to restrict the memory footprint.

3. **Input Handling**:
   - Unity Input System (Mobile touch controls + optional Gamepad).

4. **Data-Driven Gameplay**:
   - Missions, NPC stats, and vehicle definitions are defined via JSON/ScriptableObjects, avoiding hard-coded logic.

5. **Entity Pooling**:
   - Object pooling for pedestrians, vehicles, particles, and projectiles is MANDATORY. Runtime allocations are strictly limited.
