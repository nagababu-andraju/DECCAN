# DECCAN HEAT - Jules Agent Instructions

## CONTEXT
You are working on **DECCAN HEAT**, a commercial mobile third-person open-world game (Android/iOS) set in a fictionalized Hyderabad.

## CRITICAL ARCHITECTURAL CONSTRAINTS

1. **Technology Stack**:
   - Unity 6 LTS
   - Universal Render Pipeline (URP)
   - C#
   - Unity Input System
   - Cinemachine (where appropriate)
   - Addressables for streamed content
   - Target: Mobile-first rendering and physics, stable 30 FPS on mid-range Android hardware.
   - Do NOT introduce paid assets or external dependencies without documenting licenses.

2. **No Plagiarism**:
   - MUST NOT copy Grand Theft Auto or any other existing game's protected assets, UI, or distinctive presentation.
   - Fictionalized settings only. Do NOT make real religious/ethnic communities, politicians, or identifiable real organizations the villains.

3. **Data-Driven Architecture**:
   - Do NOT hard-code mission content into gameplay systems.
   - Missions must be expressible through ScriptableObjects, JSON, or robust data representation.
   - Multiplayer is explicitly OUT OF SCOPE.

4. **Autonomy & Maintenance**:
   - Make reasonable autonomous engineering decisions.
   - Record important decisions as ADRs in `/docs/adr/`.
   - Never fabricate credentials. Never commit secrets.
   - Ensure you follow `PERFORMANCE_BUDGET.md` and `QUALITY_GATES.md`.

5. **Jules Orchestrator**:
   - Check `/tools/jules_orchestrator/pipeline.yaml` for tasks.
   - Ensure the required CI/CD workflows run successfully.
