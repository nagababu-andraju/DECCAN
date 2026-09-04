# Mission System

## Overview
The mission system is entirely data-driven. Missions are defined as serialized files (JSON or ScriptableObjects) and parsed by the runtime.

## Core Primitives
Missions are built by chaining atomic objective primitives:
- `GoTo`
- `TalkTo`
- `EnterVehicle`
- `DriveTo`
- `Follow`
- `Chase`
- `Escape`
- `LoseHeat`
- `Defend`
- `Attack`
- `Stealth`
- `Collect`
- `Deliver`
- `Photograph`
- `Interact`
- `Timer`
- `Race`
- `Escort`
- `Survive`
- `Purchase`
- `ReturnToNPC`
- `Branch`
- `Reward`

## Mission Definitions Support
- Prerequisites (e.g., must have completed Mission A).
- Sequential and parallel objectives.
- Optional objectives.
- Fail conditions and Retries.
- Checkpoints.
- Dialogue integration.

**Tooling**: A mission validator tool must check the structural integrity of all mission data before committing.
