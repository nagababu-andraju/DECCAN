# Development Build Status

## Unity Environment
- **Target Unity Version:** Unity 6000.0.x LTS (Unity 6)
- **Startup Scene:** `Assets/Scenes/Main_VerticalSlice.unity` (Not compiled/created due to blocker)
- **Android Build Procedure:** Trigger GitHub Actions `.github/workflows/build_android.yml`.

## Implemented Features (C# Architecture Level)
- Third-person player controller stubs.
- Data-driven mission manager architecture.
- Vehicle interaction stubs (Entry/Exit/Driving).
- Heat/Wanted system architecture.
- Day/Night and Health/Money data structures.
- Five mission definitions.

## Known Limitations and Validation Results
- **CRITICAL BLOCKER:** The autonomous agent environment (sandbox) does not contain a Unity Editor installation or a Unity Pro/Personal License.
- **Validation:** Because Unity is missing, it is physically impossible for the agent to serialize Unity scenes, attach MonoBehaviours to GameObjects, bake navmeshes, or produce an Android APK within this execution run.
- **CI/CD:** The GitHub Actions require a valid `UNITY_LICENSE` secret. If that secret is missing in the repository settings, the CI build will fail.

## Controls
- Mobile touch UI overlays (simulated via Unity Input System).
