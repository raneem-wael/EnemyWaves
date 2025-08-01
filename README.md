EnemyWaves

# Enemy Wave System
A dynamic enemy wave spawning system built in Unity, featuring object pooling, state-based AI, and optimized performance for WebGL builds.
Players face progressively harder waves of enemies in a 3D arena and can control wave flow via UI buttons.

## Features
# Core Gameplay
Progressive waves:

- Wave 1: 30 enemies

- Wave 2: 50 enemies

- Wave 3: 70 enemies

- Wave 4+: +10 enemies per wave

Three enemy types (Ghost, Large, Alien) chosen randomly per spawn.

Enemy AI uses Finite State Machine (FSM):

- Idle – waits for player

- Chase – approaches player if in range

- Attack – attacks when close

- Die – plays death animation and returns to pool

# UI
Displays wave number, active enemy count, and FPS.

Buttons for:

Pause/Resume waves

Next Wave (skip waiting time)

Kill All Enemies

# Optimization
Object pooling to avoid runtime instantiation overhead.

Baked lighting with URP for visual quality + performance balance.

Efficient use of Observer pattern for enemy death events.

Static batching and lightweight shaders for WebGL.

# Controls
- WASD –> Move player

- Mouse –> Rotate camera 

- Left Click –> Damage enemy (50 damage per click)

Technical Details
Engine: Unity 6000.0.32f1 (URP)

Platform: WebGL

Design Patterns: Singleton (UI Manager), Object Pool, Observer, FSM for AI

SOLID Principles: Clear separation of concerns (WaveManager, EnemySpawner, UIManager, Pooling system)

