<div align="center">

# 🎭 MASKBALL

### A Pinball Roguelite Boss Rush

*Face your emotions. Break the masks.*

[![Global Game Jam](https://img.shields.io/badge/Global%20Game%20Jam-Singapore%202026-FF2E63?style=for-the-badge&logo=gamejolt&logoColor=white)](https://globalgamejam.org/)
[![Unity](https://img.shields.io/badge/Unity-6-000000?style=for-the-badge&logo=unity&logoColor=white)](https://unity.com/)
[![Platform](https://img.shields.io/badge/Platform-PC%20|%20WebGL-blue?style=for-the-badge)](#)

<br/>

[**Play Now**](#play) · [**Documentation**](#documentation)

<br/>

<img src="docs/images/gameplay-preview.gif" alt="Gameplay Preview" width="600"/>

*Screenshot/GIF placeholder — replace with actual gameplay*

</div>

---

## 🎮 About

**Maskball** is a pinball roguelite boss rush where you battle giant emotional masks that transform the table itself. Each mask — Happy, Sad, and Angry — changes the physics, layout, and feel of the game, challenging you to adapt your playstyle on the fly.

Built for **Global Game Jam Singapore 2026**.

### The Loop

```
🎯 Complete Quests → 💥 Damage the Mask → 🎭 Break It → 🔄 Table Transforms → Repeat
```

### Features

- 🎭 **3 Emotional Boss Masks** — Each with unique physics and table configurations
- 🎯 **Dynamic Quest System** — Randomized objectives weighted by current mask
- ⚡ **Combo System** — Rack up rapid hits for damage multipliers
- 🛒 **Roguelite Upgrades** — Build your power through Slay the Spire-style card picks
- 🎱 **Unlockable Balls** — Meta-progression with different ball types and abilities
- 🔄 **Transforming Table** — Elements shift, sink, and change between phases

---

## 🛠️ Tech Stack

<div align="center">

| Category | Technology |
|----------|------------|
| **Engine** | ![Unity](https://img.shields.io/badge/Unity_6-000000?style=flat-square&logo=unity&logoColor=white) |
| **Language** | ![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=c-sharp&logoColor=white) |
| **Rendering** | ![URP](https://img.shields.io/badge/URP-Universal_Render_Pipeline-purple?style=flat-square) |
| **Platforms** | ![Windows](https://img.shields.io/badge/Windows-0078D6?style=flat-square&logo=windows&logoColor=white) ![WebGL](https://img.shields.io/badge/WebGL-990000?style=flat-square&logo=webgl&logoColor=white) |
| **Version Control** | ![Git](https://img.shields.io/badge/Git-F05032?style=flat-square&logo=git&logoColor=white) ![GitHub](https://img.shields.io/badge/GitHub-181717?style=flat-square&logo=github&logoColor=white) |

</div>

---

## 🎭 The Masks

<table>
<tr>
<td align="center" width="33%">
<h3>😊 Happy</h3>
<em>Bouncy & Playful</em>
<br/><br/>
High bounce coefficients<br/>
Chaotic ball movement<br/>
Upper bumpers active
</td>
<td align="center" width="33%">
<h3>😢 Sad</h3>
<em>Heavy & Creepy</em>
<br/><br/>
Weighted, sluggish physics<br/>
Sparse, quiet atmosphere<br/>
Precision-focused quests
</td>
<td align="center" width="33%">
<h3>😠 Angry</h3>
<em>Aggressive & Hostile</em>
<br/><br/>
Fast, punishing ball speed<br/>
Angles bias toward drain<br/>
Combo-heavy challenges
</td>
</tr>
</table>

---

## 🚀 Getting Started

### Prerequisites

- [Unity 6](https://unity.com/releases/unity-6) (6000.x.x or later)
- Git LFS (for large assets)

### Installation

```bash
# Clone the repository
git clone https://github.com/YOUR_USERNAME/maskball.git

# Navigate to project folder
cd maskball

# Pull LFS assets
git lfs pull

# Open in Unity Hub and select Unity 6
```

### Build

**PC Build:**
```
File → Build Settings → PC, Mac & Linux Standalone → Build
```

**WebGL Build:**
```
File → Build Settings → WebGL → Build
```

> ⚠️ **WebGL Note:** Test flipper responsiveness early. Use Brotli compression and code stripping for smaller builds.

---

## 📖 Documentation

| Document | Description |
|----------|-------------|
| [Game Design Document](docs/GDD.md) | Full game design specification |
| [Interactive GDD](docs/pinball-gdd.html) | Visual, interactive version |
| [Technical Notes](docs/TECHNICAL.md) | Physics, WebGL considerations |

---

## 🔀 Git Workflow

### Branch Naming

```
feature/[issue-number]-short-description
bugfix/[issue-number]-short-description
art/[issue-number]-short-description
audio/[issue-number]-short-description

# Examples:
feature/3-bumper-system
bugfix/15-flipper-input-lag
art/27-mask-textures
```

### Commit Messages

Reference issues with `#` and use keywords to auto-close:

```bash
# Reference an issue
git commit -m "Add bumper hit detection #3"

# Close an issue
git commit -m "Implement flipper controls, closes #1"

# Multiple issues
git commit -m "Quest UI and tracking, closes #9, closes #25"
```

**Commit Format:**
```
[Category] Short description

- Detail 1
- Detail 2

Closes #X
```

**Categories:** `[Core]`, `[Boss]`, `[Quest]`, `[Damage]`, `[Shop]`, `[UI]`, `[Art]`, `[Audio]`, `[Meta]`, `[Fix]`

### Code Style

- Use **PascalCase** for public members, **camelCase** for private
- Prefix private fields with underscore: `_privateField`
- One class per file, filename matches class name
- Use `[SerializeField]` instead of public fields for Inspector exposure

---

## 🎯 Roadmap

### Jam Scope (Must Have)
- [x] Project setup
- [ ] Core pinball mechanics
- [ ] 3 masks with unique physics
- [ ] Quest system (4 types)
- [ ] Damage windows & combo system
- [ ] Shop with upgrades
- [ ] Win/lose flow
- [ ] Basic UI

### Stretch Goals
- [ ] Currency shop during play
- [ ] Endless mode
- [ ] Orbit/loop shots
- [ ] Full ball unlock roster
- [ ] Additional masks
- [ ] Leaderboards

---

## 👥 Team

<table>
<tr>
<td align="center">
<img src="https://via.placeholder.com/100" width="100px;" alt=""/>
<br/>
<sub><b>Name</b></sub>
<br/>
<sub>Role</sub>
</td>
<td align="center">
<img src="https://via.placeholder.com/100" width="100px;" alt=""/>
<br/>
<sub><b>Name</b></sub>
<br/>
<sub>Role</sub>
</td>
<td align="center">
<img src="https://via.placeholder.com/100" width="100px;" alt=""/>
<br/>
<sub><b>Name</b></sub>
<br/>
<sub>Role</sub>
</td>
<td align="center">
<img src="https://via.placeholder.com/100" width="100px;" alt=""/>
<br/>
<sub><b>Name</b></sub>
<br/>
<sub>Role</sub>
</td>
</tr>
</table>

---

## 🏆 Global Game Jam Singapore 2026

<div align="center">

[![GGJ](https://img.shields.io/badge/🌍_Global_Game_Jam-Singapore_2026-FF2E63?style=for-the-badge)](https://globalgamejam.org/)

**Theme:** Masks

**Jam Site:** Singapore

**Date:** January 2026

[View on GGJ Site](#)

</div>

---

<div align="center">

**Made with ❤️ and ☕**

[⬆ Back to Top](#-maskball)

</div>
