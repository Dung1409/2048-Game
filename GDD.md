# GAME DESIGN DOCUMENT

## Project Information

| Item            | Description                        |
| --------------- | ---------------------------------- |
| Project Name    | 2048                               |
| Genre           | Puzzle / Casual                    |
| Platform        | PC (Unity)                         |
| Target Audience | Casual players, puzzle enthusiasts |
| Session Length  | 2–10 phút                          |

---

# 1. Game Overview

## Game Concept

2048 là một trò chơi giải đố sử dụng các ô số trên bàn cờ. Người chơi di chuyển toàn bộ các ô theo bốn hướng để gộp những ô có cùng giá trị, từ đó tạo ra các ô có giá trị lớn hơn.

Mục tiêu chính là tạo được ô số **2048**, đồng thời đạt điểm số cao nhất có thể trước khi không còn nước đi.

## Core Experience

Mang đến trải nghiệm đơn giản, dễ học nhưng có chiều sâu chiến thuật. Người chơi liên tục đưa ra quyết định về vị trí và hướng di chuyển nhằm tối ưu không gian trên bàn cờ và đạt được các giá trị số lớn hơn.

## Key Features

* Gameplay điều khiển bằng bốn hướng di chuyển.
* Cơ chế gộp số trực quan và dễ hiểu.
* Hệ thống điểm số và lưu điểm cao nhất.
* Chế độ Undo cho phép hoàn tác một lượt di chuyển.
* Hỗ trợ nhiều kích thước bàn chơi.

---

# 2. Core Gameplay

## Gameplay Loop

1. Người chơi chọn hướng di chuyển.
2. Tất cả tile trên bàn cờ trượt theo hướng đã chọn.
3. Các tile cùng giá trị sẽ được gộp lại.
4. Một tile mới xuất hiện tại vị trí ngẫu nhiên.
5. Người chơi tiếp tục thực hiện các lượt đi tiếp theo.

## Session Flow

### Main Menu

* Bắt đầu trò chơi.
* Chọn kích thước bàn chơi.
* Xem điểm cao nhất.

### Gameplay

* Di chuyển tile.
* Gộp số.
* Tích lũy điểm số.
* Sử dụng Undo khi cần.

### End Game

* Hiển thị kết quả.
* Cập nhật điểm cao nhất.
* Chơi lại hoặc quay về menu.

## Win Condition

Người chơi tạo thành công tile có giá trị **2048**.

## Lose Condition

Không còn ô trống và không còn cặp tile nào có thể gộp được.

---

# 3. Core Systems

## Grid System

Bàn chơi được xây dựng dưới dạng lưới vuông.

Các kích thước hiện có:

* 4×4 (mặc định)
* 5×5 (nâng cao)

Mỗi ô trên bàn chơi có thể chứa tối đa một tile.

## Movement System

Người chơi sử dụng các phím điều hướng để di chuyển toàn bộ tile theo một hướng.

Các tile sẽ:

* Trượt đến vị trí xa nhất có thể.
* Dừng lại khi gặp tile khác hoặc mép bàn cờ.

## Merge System

Khi hai tile có cùng giá trị va chạm:

* Chúng sẽ hợp nhất thành một tile mới.
* Giá trị tile mới bằng tổng giá trị của hai tile cũ.
* Mỗi tile chỉ được gộp một lần trong một lượt di chuyển.

Ví dụ:

| Tile A | Tile B | Result |
| ------ | ------ | ------ |
| 2      | 2      | 4      |
| 4      | 4      | 8      |
| 8      | 8      | 16     |

## Tile Spawn System

Sau mỗi lượt di chuyển hợp lệ:

* Một tile mới xuất hiện tại vị trí trống ngẫu nhiên.
* Giá trị tile mới thường là 2 hoặc 4.

---

# 4. Scoring System

Người chơi nhận điểm khi thực hiện gộp tile.

Điểm nhận được bằng giá trị của tile mới tạo ra.

Ví dụ:

| Merge | Score Earned |
| ----- | ------------ |
| 2 + 2 | +4           |
| 4 + 4 | +8           |
| 8 + 8 | +16          |

## High Score

Điểm cao nhất được lưu lại để người chơi cạnh tranh với thành tích của chính mình trong các phiên chơi tiếp theo.

---

# 5. Progression

## Difficulty Options

### 4×4 Grid

* Chế độ mặc định.
* Dễ tiếp cận.
* Phù hợp với người chơi mới.

### 5×5 Grid

* Nhiều không gian hơn.
* Ván chơi kéo dài hơn.
* Yêu cầu quản lý bàn cờ tốt hơn.

## Player Motivation

* Đạt tile 2048.
* Chinh phục các tile cao hơn như 4096 hoặc 8192.
* Cải thiện điểm số cá nhân.
* Hoàn thành bàn chơi với số lượt ít nhất có thể.

---

# 6. Save System

Dữ liệu được lưu cục bộ để hỗ trợ tiếp tục ván chơi.

Thông tin được lưu:

* Trạng thái bàn chơi.
* Điểm hiện tại.
* Điểm cao nhất.
* Kích thước bàn chơi đang sử dụng.

Người chơi có thể tiếp tục phiên chơi trước đó sau khi mở lại trò chơi.

---

# 7. Undo System

Người chơi có thể hoàn tác một lượt di chuyển gần nhất.

## Mục đích

* Giảm cảm giác bị phạt vì thao tác sai.
* Hỗ trợ người chơi mới học game.
* Tạo thêm cơ hội tối ưu chiến thuật.

Hiện tại hệ thống chỉ hỗ trợ hoàn tác một bước.

---

# 8. Visual Direction

## Art Style

* Thiết kế tối giản.
* Màu sắc rõ ràng và dễ phân biệt.
* Giao diện tập trung vào khả năng đọc thông tin.

## Tile Design

Mỗi giá trị tile sẽ có màu sắc riêng giúp người chơi dễ dàng nhận biết mức độ tiến triển.

Ví dụ:

* 2
* 4
* 8
* 16
* 32
* 64
* 128
* 256
* 512
* 1024
* 2048

## Feedback

* Animation khi tile di chuyển.
* Animation khi merge.
* Hiển thị điểm số được cộng thêm.
* Hiệu ứng chiến thắng khi đạt 2048.

---

# 9. MVP Scope

## Must Have

* Grid 4×4
* Tile Movement
* Tile Merge
* Random Tile Spawn
* Score System
* Win/Lose Condition
* Restart Game

## Additional Features

* Undo System
* High Score Save
* Multiple Grid Sizes
* Auto Save

## Nice To Have

* Sound Effects
* Background Music
* Settings Menu
* Touch/Swipe Controls

---

# 10. Future Improvements

## Mobile Support

Bổ sung điều khiển bằng thao tác vuốt để hỗ trợ Android và iOS.

## Achievement System

Ví dụ:

* Đạt tile 2048.
* Đạt tile 4096.
* Chơi 100 trận.
* Đạt 10.000 điểm.

## Daily Challenge

Cung cấp thử thách mới mỗi ngày với luật chơi hoặc kích thước bàn cờ khác nhau.

## Leaderboard

Cho phép người chơi cạnh tranh điểm số với cộng đồng thông qua bảng xếp hạng trực tuyến.

## Themes & Customization

Bổ sung các giao diện khác nhau cho:

* Tile
* Background
* Board
* UI

## Endless Mode

Sau khi tạo được tile 2048, người chơi có thể tiếp tục chơi để đạt các mốc cao hơn như 4096, 8192 hoặc xa hơn nữa.
