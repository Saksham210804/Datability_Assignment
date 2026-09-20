# Fall Race — Unity 3D Android Game

A **Fall Guys / Fall Race–style survival game** built in **Unity 6.3 LTS (6000.3.6f1)** for **Android**.
Move across a field of hexagonal platforms that collapse after you step on them, outlast 4 AI opponents, and be the last one standing.

> Reference: [fall.race.io](https://play.google.com/store/apps/details?id=fall.race.io&hl=en_IN)

---

## 🎮 How to Play

1. Launch the game — the level is generated and all characters wait on the top platform.
2. Press **START**, then wait for the **5 → 1 countdown**.
3. Use the **on-screen joystick** (bottom-left) to move in any direction.
4. There is **no jump button** — your character **jumps automatically** when there is no platform ahead, so keep moving to cross gaps.
5. Every platform you step on **collapses shortly after** and becomes unavailable — never stop moving.
6. Fall below the platforms and you're **eliminated**.
7. **Be the last player remaining to win!**

---

## ✨ Features

- **Procedurally generated levels** — random number, size, height, and layout of platforms each run (object-pooled tiles for performance).
- **On-screen joystick** touch controls (uGUI).
- **Automatic jumping** to cross gaps — shared by the player and the AI.
- **4 AI bots** that read the level, pick safe platforms, jump across gaps, and get eliminated if they miss.
- **Collapsing platforms** with press-down + fade feedback.
- **Cinemachine follow camera** with a clean top-down-behind view.
- **Character animations** (Idle / Run / Jump / Fall) driven by state.
- **Full game flow** — Start menu, countdown, live "Players Remaining" counter, **Game Over**, **Victory**, and **Retry**.
- **Mobile-tuned rendering** — URP, MSAA, ASTC textures, ARM64 / IL2CPP build.

---

## ✅ Assignment Requirements — All Completed

| Requirement | Status |
|---|---|
| Unity 3D Android game | ✅ |
| Large clusters of individual platforms | ✅ |
| Player movement using touch input | ✅ (on-screen joystick) |
| Platforms fall / disappear after being stepped on | ✅ |
| 3–5 AI / bot players | ✅ (4 bots) |
| AI follows the same platform rules as the player | ✅ |
| Automatic jump when no platform ahead | ✅ |
| Characters can fall and be eliminated | ✅ |
| Last remaining player wins | ✅ |
| Display number of players remaining | ✅ |
| Game Over when the player is eliminated | ✅ |
| Victory when the player is the last remaining | ✅ |
| Retry option | ✅ |
| Polish (collapse, jump, landing, elimination, victory / game over) | ✅ |
| Smooth gameplay & game feel | ✅ |

---

## 🏗️ Architecture (brief)

Clean, decoupled MonoBehaviour composition:

- **Controllers decide, `CharacterMovement` performs** — `PlayerController` and `AIController` share the same physics movement.
- **`Tile`** only knows a character stepped on it (shared layer) — no player / AI-specific logic.
- **`GameManager`** owns match state (Waiting → Playing → GameOver / Victory) and player count; **`GameUIManager`** only displays it.
- **`TileGenerator` + `TilePoolManager` + `TileGenerationConfig`** handle procedural, pooled level generation.
- **`AIDecisionMaker`** scores nearby tiles for lightweight, reactive bot behavior (no NavMesh / pathfinding needed).

---

## 🚀 Build & Run

- **Engine:** Unity 6.3 LTS (6000.3.6f1)
- **Platform:** Android (IL2CPP, ARM64, URP)
- Open the project → open `Assets/Scenes/SampleScene.unity` → **File ▸ Build** for Android to produce the APK.

---

## 🙏 Thank You

Thank you so much for the opportunity to work on this assignment. I genuinely enjoyed building it — from the procedural platforms and AI to the game-feel polish and mobile setup. It was a great chance to apply clean architecture to a fun, complete game loop, and I learned a lot along the way. I truly appreciate your time in reviewing it. 🎉
