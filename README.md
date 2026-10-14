# 🛡 RansomRescue

> **Công cụ nhận diện và hỗ trợ phục hồi sau tấn công Ransomware dành cho Windows**
>
> Ransomware identification and recovery assistance tool for Windows

![Platform](https://img.shields.io/badge/platform-Windows-blue)
![.NET](https://img.shields.io/badge/.NET%20Framework-4.6.1+-purple)
![License](https://img.shields.io/badge/license-MIT-green)
![Release](https://img.shields.io/github/v/release/ThanhNguyenVN93/RansomRescue)

---

## 📋 Giới thiệu / Overview

**RansomRescue** quét ổ đĩa, thu thập bằng chứng (phần mở rộng file, ransom note, magic bytes, registry) rồi chấm điểm để xác định dòng họ ransomware đang tấn công máy bạn — từ đó chỉ đến công cụ giải mã chính hãng.

RansomRescue scans your drives, collects evidence (file extensions, ransom notes, magic bytes, registry entries) and scores them to identify the ransomware family — then points you to the official decryption tool.

> ⚠ Tool **CHỈ nhận diện** và tìm công cụ giải mã. Không giải mã, không xóa virus.  
> ⚠ Tool **only identifies** and finds decryption tools. Does NOT decrypt or remove malware.

---

## ✨ Tính năng / Features

| | |
|---|---|
| 🔍 **Nhận diện 31+ dòng họ** | STOP/Djvu, LockBit, WannaCry, Phobos, REvil, BlackCat... |
| 📊 **Hệ thống điểm** | Extension +50 / Note +30 / Keyword +20 / Magic bytes +15 |
| 🌐 **Multi-family detection** | Liệt kê top 2-3 ứng viên khi điểm gần nhau |
| 🛡 **Tích hợp KVRT** | Tải Kaspersky Virus Removal Tool ngầm trong lúc quét |
| 💾 **Lưu báo cáo** | Xuất kết quả ra file .txt |
| 🌙 **Dark mode** | Toggle sáng/tối |
| 🌏 **Song ngữ VN/EN** | Chuyển ngôn ngữ real-time |
| 🔒 **SHA-256 self-check** | Xác minh EXE chưa bị sửa khi khởi động |
| 🔄 **Auto DB refresh** | Tự cập nhật database sau mỗi 7 ngày |
| ✅ **Admin required** | Tự yêu cầu quyền Administrator qua UAC |

---

## 📥 Tải về / Download

👉 **[Download tại Releases](https://github.com/ThanhNguyenVN93/RansomRescue/releases/latest)**

**Yêu cầu / Requirements:**
- Windows 7 / 8.1 / 10 / 11
- .NET Framework 4.6.1+ *(Windows 10/11 đã có sẵn / pre-installed on Win10/11)*

---

## 🚀 Hướng dẫn sử dụng / How to Use

### Bước chuẩn bị (QUAN TRỌNG)
1. **Rút LAN / tắt WiFi ngay lập tức** — ngăn lây lan
2. **Không tắt máy** — mất shadow copy, mất key trong RAM
3. Chép tool vào USB, chạy từ USB

### Sử dụng
1. Chạy **`RansomRescue_Start.bat`** (tự kiểm tra .NET) hoặc thẳng **`RansomRescue.exe`**
2. Tab **Cảnh báo** → đọc kỹ → tick xác nhận → Tiếp theo
3. Tab **Quét** → chọn ổ đĩa → Bắt đầu quét
4. Tab **Kết quả** → xem dòng họ, tải công cụ giải mã, chạy KVRT

### Sau khi có kết quả
- Backup toàn bộ file mã hóa trước khi giải mã
- Tải công cụ giải mã từ **[nomoreransom.org](https://www.nomoreransom.org)**
- Chạy KVRT để xóa ransomware

---

## 🏗 Cấu trúc dự án / Project Structure

```
RansomRescue/
├── Core/           Scanning, identification, DB loading
├── Forms/          WinForms UI (frmMain, frmDonate)
├── Models/         Data models (RansomFamily, ScanResult, IdentifyResult)
├── Utils/          PathHelper, Logger, UptimeHelper
├── Resources/      db.json (31 families), shield.ico, momo.jpg
├── Tests/          MSTest unit tests (25 cases)
└── dist/           Release build (single EXE, launcher, docs)
```

---

## 🧪 Unit Tests

```
Tests/IdentifierTests.cs   — 20 test cases (scoring logic)
Tests/PathHelperTests.cs   —  5 test cases (path matching)
Total: 25/25 ✅
```

---

## ☕ Ủng hộ / Support

Nếu tool hữu ích với bạn:

- **MoMo**: Quét mã QR trong app (nút ☕ Donate)
- **Ko-fi**: [ko-fi.com/thanhnguyen150993](https://ko-fi.com/thanhnguyen150993)

---

## 📄 License

MIT License © 2026 ThanhCode93

Xem chi tiết tại [dist/LICENSE.txt](dist/LICENSE.txt)
