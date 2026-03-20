# 🏍️ VR Moto Drive

**A Proof-of-Concept VR motorcycle simulator focused on motion-sickness prevention, ghost racing, and exploring new paradigms for bike control in virtual reality.**

> ⚠️ **This is a PoC (Proof of Concept).** The primary goal was rapid experimentation — not production-grade code quality. The codebase prioritises exploring ideas over polish.

---

## 🎯 Project Goals

| Goal | Status |
|------|--------|
| Eliminate (or drastically reduce) VR motion sickness through viewport smoothing and stabilisation | ✅ Core focus |
| Ghost rider feature — chase a recorded phantom on the track | ✅ Implemented |
| Novel VR input methods for motorcycle control (head tracking, controller mapping) | ✅ Implemented |
| Multiplayer networking for shared VR sessions | ✅ Prototype |

---

## 🤢 → 😊 Motion-Sickness Prevention

The single biggest challenge in VR locomotion is **visually induced motion sickness (VIMS)**. Traditional approaches either teleport the player or limit movement speed — both unacceptable for a motorcycle game.

### What We Tried

| Technique | How It Works | File |
|-----------|-------------|------|
| **Viewport Smoothing (DOTween)** | Instead of snapping the camera to the motorcycle's rotation every frame, rotations are interpolated over a **0.4 s** window using DOTween tweens. This removes the micro-jitter that triggers nausea while keeping the rider's sense of direction intact. | `MotoViewportStabilather.cs` |
| **XR Camera Re-centering** | On every relevant event the XR camera rig is re-aligned with the motorcycle body so the horizon stays level, reducing conflict between visual and vestibular signals. | `MotoViewPortController.cs` |
| **Speed-Dependent Steering Curves** | At higher speeds the steering response is automatically dampened via animation curves, which prevents the wild oscillations that are a major sickness trigger. | `MotoSteeringSystem.cs` |

The combination of these three layers turned what was originally a "two-minute-max" experience into something that could be used comfortably for extended sessions.

---

## 👻 Ghost Rider Feature

One of the most engaging mechanics we prototyped is **ghost racing**:

* A previous run (your own or another player's) is recorded and replayed on the track as a semi-transparent phantom motorcycle.
* The ghost gives you a concrete target to chase without requiring a second live player.
* Because the ghost's trajectory is deterministic, it doubles as a benchmark for testing motion-sickness countermeasures — you can replay the same aggressive cornering sequence repeatedly and compare comfort across different stabilisation settings.
* The multiplayer layer (Mirror Networking) also enables live "ghost" opponents in shared sessions, where remote players appear as networked phantoms rendered with a distinct visual style.

---

## 🎮 VR-First Control Methods

A key area of experimentation was finding control schemes that feel **natural on a motorcycle in VR**. The project supports several input modes that can be swapped at runtime:

### 1. VR Controller Input
| Action | Mapping |
|--------|---------|
| Throttle | Right-hand stick Y-axis |
| Steering | Right-hand stick X-axis |
| Brake | Right-hand trigger |

### 2. Head-Tracking Steering
Instead of using your hands, you steer the bike by **leaning your head**:

* **Roll (tilt left/right)** → steering angle
* **Pitch (lean forward)** → throttle

This is the most immersive mode — it mimics the way a real rider shifts body weight to corner. A configurable sensitivity curve prevents over-steering.

### 3. Desktop Fallback
Standard WASD + mouse for quick iteration without a headset.

The input system auto-detects whether an XR headset is connected and switches modes accordingly.

---

## 🌐 Multiplayer & Networking

Built on the **Mirror** networking framework:

* **Host / Client / Dedicated Server** topologies supported.
* Connection configuration loaded from a JSON file (`IP`, `Port`, `Password`, `PlayerName`).
* Platform auto-detection: headless Linux builds start as a dedicated server; everything else starts as a client.
* Local-player awareness: each client only renders full detail for its own motorcycle; remote riders appear as lightweight ghost representations.

---

## 🏗️ Technical Stack

| Layer | Technology |
|-------|-----------|
| Engine | Unity 2021.3 LTS |
| Rendering | Universal Render Pipeline (URP) |
| VR Runtime | OpenXR 1.8 + Oculus SDK 3.2 |
| VR Interaction | XR Interaction Toolkit 2.5 |
| Networking | Mirror |
| Tweening / Smoothing | DOTween |
| Serialisation | Newtonsoft JSON |
| Editor Tools | Odin Inspector |

---

## 📂 Project Structure

```
Assets/
├── VRMoto/                     # Active VR-first codebase
│   ├── Scripts/
│   │   ├── MotoViewportStabilather.cs   ← motion-sickness smoothing
│   │   ├── MotoViewPortController.cs    ← XR camera alignment
│   │   ├── Network/                     ← Mirror connection logic
│   │   └── UI/                          ← VR / flat-screen UI switching
│   ├── Scenes/
│   │   ├── VR/                          ← VR test tracks
│   │   ├── NoVR/                        ← desktop test track
│   │   └── Multiplayer/                 ← lobby + multiplayer track
│   └── Prefabs/                         ← player, wheels, logic prefabs
│
├── OldProject/                 # Legacy physics prototype (still referenced)
│   └── Motorcycle/SimulationCore/
│       └── Scripts/
│           ├── Interfaces/              ← IVehicleController, IVehicleEngine, …
│           ├── MainLogic/
│           │   ├── Base/                ← abstract vehicle classes
│           │   └── MotoTest/
│           │       ├── Controllers/     ← engine, steering, brake, gearbox
│           │       ├── InputHandler/    ← VR + desktop + head-tracking input
│           │       └── NetworkController/
│           └── SettingLoader/           ← JSON-driven physics config
```

---

## 🚀 Getting Started

1. **Clone** the repository.
2. Open with **Unity 2021.3 LTS** (or compatible).
3. Import any missing packages via the Package Manager (most are referenced in `Packages/manifest.json`).
4. Open one of the VR scenes under `Assets/VRMoto/Scenes/VR/` and press **Play** with your headset connected.

> For desktop testing, open `Assets/VRMoto/Scenes/NoVR/MotorCycleNonVr_Polygon.unity`.

---

## 🔮 Interactivity & Future Directions

Because this is a PoC, many ideas were explored at a prototype level:

* **Haptic Feedback** — mapping engine RPM and road surface to controller vibration for deeper immersion.
* **Full-Body Leaning** — using additional trackers (e.g. Vive Trackers on the torso) to detect full-body lean for even more realistic steering.
* **Dynamic Ghost Difficulty** — adjusting the ghost's speed in real time to keep the chase competitive.
* **Environmental Interaction** — VR hand-grab mechanics for adjusting mirrors, toggling switches on the dashboard, and other cockpit interactions that increase presence.
* **Adaptive Comfort Settings** — automatically dialling motion-sickness countermeasures up or down based on real-time physiological signals (heart rate, galvanic skin response) from wearable sensors.

---

## 📄 License

[MIT](LICENSE) © 2024 alkihuri
