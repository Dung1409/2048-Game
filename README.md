# 2048 Game – Unity Implementation

## 📌 Overview
A Unity implementation of the classic 2048 puzzle game.
The project focuses on clean architecture, movement algorithm clarity, undo system, and auto save/load functionality.

---

## 🎮 Gameplay

- 4x4 grid containing tiles with values that are powers of 2.
- Use arrow keys to move all tiles in one direction.
- When two tiles with the same value collide, they merge into one tile with double value.
- After each valid move, a new tile (2 or 4) spawns at a random empty cell.
- Win condition: reach tile value 2048.
- Lose condition: no empty cells and no valid moves left.
- Supports Undo (1 step) and auto save/load game state.

---

## ⚙ Core Systems

### Movement & Merge Logic
- Direction-based traversal order (left/right/up/down).
- Each tile can merge only once per move.
- Merge increases score based on new tile value.
- New tile spawns only if a real movement occurred.

### Game Over Detection
- Step 1: Check for empty cells.
- Step 2: Check adjacent tiles (right & down) for possible merges.
- Game ends only if both conditions fail.

### Undo System
- Save previous board state before each valid move.
- Restore board and score when Undo is triggered.

---

## 🏗 Design Patterns

### Singleton
Used to manage core game systems and ensure only one instance controls board, input, score, and UI.

### Observer (Event-driven)
Used to notify UI and other systems when:
- Score changes
- Game state changes (Win/Game Over)
- Tile updates

### Object Pooling
Used to reuse Tile GameObjects instead of instantiating/destroying repeatedly, improving performance.

### State Pattern
Used to manage tile visual states (color and style) based on tile value using data-driven configuration.

### Command-like Undo
Used to store previous board state and rollback one move, keeping game state consistent.

---

## 💾 Save / Load

- Board state serialized into JSON.
- Score, HighScore, and TurnScore stored using PlayerPrefs.
- Game auto-loads previous state if available.

---

## 🚀 Requirements
- Unity 2021.3+
