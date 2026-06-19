# Game Design Document: 2048

## 1. Game Overview

| Field | Detail |
|-------|--------|
| **Title** | 2048 |
| **Genre** | Puzzle / Number Puzzle / Casual |
| **Platform** | PC (Unity Standalone) |
| **Target Audience** | Casual gamers (mọi lứa tuổi), người yêu thích puzzle |
| **Elevator Pitch** | Gộp các ô số để tạo ra ô 2048 — gameplay đơn giản, gây nghiện, mỗi lượt chỉ mất 30 giây nhưng đủ sức hút để chơi hàng giờ. |

## 2. Core Gameplay

### Core Loop
1. Người chơi nhấn phím mũi tên → tất cả tile trượt theo hướng đó
2. Tile cùng giá trị chạm nhau → gộp thành tile gấp đôi
3. Tile mới (2 hoặc 4) xuất hiện ngẫu nhiên trên ô trống
4. Tiếp tục cho đến khi đạt 2048 (thắng) hoặc hết nước đi (thua)

### Session Flow
Menu → chọn kích thước grid (4x4 / 5x5) → gameplay loop → win/game over → restart/home

### Win/Lose Conditions
- **Win**: tạo được tile 2048
- **Lose**: không còn ô trống và không còn cặp tile kề nào cùng giá trị

## 3. Progression

- **Horizontal**: mỗi game là một session độc lập, tiến trình là score và high score
- **Vertical**: kích thước grid tăng dần (4x4 → 5x5) khi người chơi muốn thử thách hơn
- **Unlock System**: không có — mở khóa ngay từ đầu
- **Difficulty Curve**: 4x4 dễ tiếp cận; 5x5 kéo dài thời gian game + khó kiểm soát hơn

## 4. Systems Design

### Grid System (`Grid.cs`)
- Grid là Singleton, chứa `List<Row>`, mỗi Row chứa `List<Cell>`
- Mỗi Cell giữ reference đến Tile hoặc null
- Object Pooling qua `poolTile` để tái sử dụng tile, tránh Instantiate/Destroy
- Move algorithm: duyệt từng hàng/cột theo hướng, dùng Queue để xử lý compact tiles và merge

### Merge System
- Mỗi tile chỉ merge 1 lần/lượt (`canMerge` flag)
- Merge xong set `canMerge = false`, tránh merge dây chuyền trong cùng lượt
- Tile bị merge set active = false (trả về pool)

### Score System (`ScoreManager.cs`)
- `score`: điểm hiện tại
- `highScore`: điểm cao nhất mọi lúc (lưu PlayerPrefs)
- `turnScore`: điểm kiếm trong lượt hiện tại (dùng cho Undo)
- Merge bao nhiêu điểm thì cộng bấy nhiêu (giá trị tile mới)

### Save/Load System
- Lưu board state qua `BoardData` (int array) → JSON → PlayerPrefs
- Old board state cho Undo (1 bước)
- Auto-save khi về Home, khi thoát PlayMode Editor
- Load board khi Start nếu có save data

### Undo System
- Lưu `oldValue` trong mỗi Cell trước khi move
- Undo: restore từng Cell về oldValue, trừ turnScore
- Chỉ undo được 1 bước

### Input System (`InputManager.cs`)
- Input: 4 phím mũi tên → direction vector
- Không xử lý khi có popup hoặc animation đang chạy

## 5. Content Design

### Tile States (ScriptableObject)
- 14 TileState assets cho giá trị 2 → 16384
- Mỗi state có `backgroundColor` + `textColor` — dễ dàng thêm/sửa màu

### Grid Configurations
- 4x4 (mặc định) — chuẩn classic
- 5x5 — harder variant, kéo dài game

## 6. Monetization

**Hiện tại**: chưa có monetization — game free, không quảng cáo, không IAP.

**Đề xuất**:
- Quảng cáo banner khi ở menu / interstitial giữa các game
- Mua skin màu sắc / theme cho tile
- Undo bằng cách xem quảng cáo (hiện tại undo free)

## 7. Retention

**Hiện tại**: chưa có hệ thống retention.

**Đề xuất**:
- Daily challenge: grid khác nhau mỗi ngày (3x3, 6x6, obstacle cells)
- Weekly leaderboard: điểm cao nhất trong tuần
- Achievement system: "merge 10 tiles trong 1 lượt", "đạt 4096", "chơi 100 game"
- Endless mode: tiếp tục chơi sau 2048, đếm score tối đa

## 8. Technical Notes

| Aspect | Detail |
|--------|--------|
| **Engine** | Unity 2021.3+, .NET Standard 2.1 |
| **UI** | TextMeshPro, Canvas-based |
| **Save** | PlayerPrefs (JSON serialization) |
| **Animation** | Lerp movement coroutine (0.1s), Animator cho merge scale effect |
| **Design Patterns** | Singleton, Observer, Object Pool, State (ScriptableObject), MVC |
| **Grid Data** | int array, 1D mapped to 2D |
| **Known Issues** | Undo 1 bước, không interrupt animation, không touch input |

## 9. MVP Scope

| Priority | Feature |
|----------|---------|
| **Must Have** | Grid 4x4, move/merge, score, win/lose, restart |
| **Has (current)** | Undo, 5x5, auto-save, high score, tile animations |
| **Nice To Have** | Touch/swipe input, sound effects, settings menu |
| **Future** | Daily challenge, leaderboard, achievements, themes |

---

## Review & Đánh giá

| Tiêu chí | Điểm |
|----------|------|
| **Market Fit** | 8/10 — 2048 đã proven, bản Unity này ổn nhưng thiếu touch input cho mobile |
| **Innovation** | 4/10 — faithful clone, chưa có twist riêng |
| **Production Risk** | 2/10 — code đã hoàn chỉnh, rủi ro thấp |
| **Monetization Potential** | 3/10 — cần thêm feature để monetize |

## Next Development Steps

1. **Touch/swipe input** — priority #1 để port lên mobile
2. **Sound effects + music** — tăng immersion
3. **Tile merge animation** — hiện tại chỉ scale, có thể thêm particle
4. **Achievement system** — tăng retention
5. **Mobile build** — Android/iOS với responsive UI
6. **Twist feature** — như gravity mode, timer mode, obstacle cells
