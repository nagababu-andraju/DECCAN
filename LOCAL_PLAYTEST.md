# Local Playtest Guide

To properly verify the gameplay slice on your local machine, follow these steps explicitly since the environment sandbox cannot runtime-test the binary scene.

## Setup Requirements
1. **Clone the Repository** to your local machine.
2. **Open in Unity Hub**:
   - Ensure you are using **Unity 6.3 LTS (6000.3.0f1)**.
   - Click "Open" and point it to the repository folder.
3. **Allow Packages to Import**:
   - Unity will download `com.unity.inputsystem`, `com.unity.cinemachine`, `com.unity.addressables`, and `com.unity.render-pipelines.universal` as specified in `Packages/manifest.json`.
4. **Bootstrap the Vertical Slice**:
   - In the top menu bar, click `Deccan Heat > Bootstrap Vertical Slice`.
   - This script procedurally generates the 3D primitives, assigns all C# scripts to GameObjects, wires wheel colliders, configures input, links mission JSONs, and generates `Assets/Scenes/Main_VerticalSlice.unity`.
   - **Important**: If you are prompted about the New Input System replacing the old one, click "Yes" and restart the Editor.

## Playing the Game
1. Open `Assets/Scenes/Main_VerticalSlice.unity`.
2. Press the **Play** button.
3. **Desktop Controls**:
   - **WASD**: Movement / Driving.
   - **Mouse**: Camera Look.
   - **E**: Interact / Enter / Exit Vehicle.
   - **Space**: Jump / Brake.
   - **Left Mouse**: Melee/Shoot.
   - **Right Mouse**: Aim.
   - **Escape**: Pause Menu.

## Mobile Testing
1. Go to `File > Build Settings`.
2. Ensure `Assets/Scenes/Main_VerticalSlice.unity` is listed and checked.
3. Switch Platform to Android.
4. Click **Build and Run** (requires local Android SDK/NDK path to be configured in Unity Preferences).