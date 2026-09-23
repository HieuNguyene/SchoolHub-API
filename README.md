# 🎓 SchoolHub API - Advanced School & Student Management RESTful API

Một dự án **ASP.NET Core 8 Web API** chuyên sâu dành cho hệ thống quản lý học sinh và trường học. Dự án được thiết kế chuẩn mực theo mô hình **Clean Architecture (4 Tầng)** kết hợp **CQRS (MediatR)**, **AutoMapper**, **FluentValidation**, và hệ thống bảo mật toàn diện bằng **JWT Authentication** cùng cơ chế **Refresh Token Rotation (Xoay vòng token)** và **Phân quyền đa cấp (Role & Policy Authorization)**.

---

## 🚀 Công nghệ & Thư viện sử dụng

- **Framework:** ASP.NET Core 8.0 Web API (.NET 8)
- **Ngôn ngữ:** C# 12
- **Cơ sở dữ liệu:** SQL Server (LocalDB / SQL Server Express)
- **ORM & Data Access:** Entity Framework Core 8.0 & Dapper
- **Kiến trúc:** Clean Architecture (4 Projects) + CQRS Pattern (MediatR)
- **Object Mapping:** AutoMapper
- **Validation:** FluentValidation (Tự động validate Model)
- **Xác thực & Bảo mật:** JWT Bearer, PBKDF2 Password Hashing, Refresh Token
- **Tài liệu API:** Swagger UI (Tích hợp nút ổ khóa Bearer Authorization)

---

## 📂 Cấu trúc dự án (Mô hình Clean Architecture)

```text
📦 SchoolHub.slnx
 ┣ 📂 SchoolHub.API                   <-- TẦNG 1: PRESENTATION LAYER
 ┃ ┣ 📂 Controllers          : Nhận HTTP Requests, phân quyền [Authorize], gọi MediatR.
 ┃ ┣ 📂 Middlewares          : Bắt lỗi toàn cục (ExceptionMiddleware), ghi nhật ký (LoggingMiddleware).
 ┃ ┣ 📂 Extensions           : Đăng ký Dependency Injection (DI) tập trung.
 ┃ ┣ 📜 Program.cs           : Cấu hình Pipeline, JWT Bearer, Policy Authorization & Swagger.
 ┃ ┗ 📜 appsettings.json     : Lưu ConnectionString và cấu hình Secret Key cho JWT.
 ┃
 ┣ 📂 SchoolHub.Application           <-- TẦNG 2: BUSINESS LOGIC & CQRS LAYER
 ┃ ┣ 📂 Common               : Cấu hình hệ thống (JwtSettings).
 ┃ ┣ 📂 DTOs                 : Requests và Responses chuyển tải dữ liệu.
 ┃ ┣ 📂 Features             : Tổ chức nghiệp vụ theo chuẩn CQRS:
 ┃ ┃ ┣ 📂 Auth               : Commands cho Register, Login, RefreshToken, RevokeToken.
 ┃ ┃ ┣ 📂 Students           : Commands (Thêm, Sửa, Xóa) & Queries (Tìm kiếm, Xem chi tiết).
 ┃ ┃ ┣ 📂 Classes            : Commands & Queries quản lý lớp học.
 ┃ ┃ ┣ 📂 Subjects           : Commands & Queries quản lý môn học.
 ┃ ┃ ┗ 📂 Scores             : Commands & Queries quản lý điểm số.
 ┃ ┣ 📂 Interfaces           : Giao diện ITokenService, IPasswordHasher, IUserRepository...
 ┃ ┣ 📂 Mappings             : Profiles AutoMapper (StudentProfile, ClassProfile...).
 ┃ ┗ 📂 Validations          : Các bộ luật kiểm tra dữ liệu bằng FluentValidation.
 ┃
 ┣ 📂 SchoolHub.Infrastructure        <-- TẦNG 3: DATA ACCESS & INFRASTRUCTURE LAYER
 ┃ ┣ 📂 Data                 : ApplicationDbContext (EF Core nối SQL Server).
 ┃ ┣ 📂 Repositories         : Triển khai các Repository truy xuất DB.
 ┃ ┣ 📂 Services             : Triển khai TokenService (JWT), PasswordHasher (PBKDF2).
 ┃ ┗ 📂 Migrations           : Lịch sử tiến hóa Database (EF Core Migrations).
 ┃
 ┣ 📂 SchoolHub.Domain                <-- TẦNG 4: DOMAIN LAYER (CỐT LÕI)
 ┃ ┣ 📂 Entities             : Các thực thể trung tâm (User, RefreshToken, Student, Class, Subject, Score).
 ┃ ┗ 📂 Enums                : GenderType, RoleType...
 ┃
 ┗ 📂 SchoolHub.UnitTests             <-- TẦNG 5: AUTOMATED TESTING (xUnit, Moq, FluentAssertions)
   ┣ 📂 Domain               : Kiểm thử các logic và bất biến của Domain Entities.
   ┣ 📂 Features             : Kiểm thử Command Handlers với Mock Repository.
   ┣ 📂 Services             : Kiểm thử thuật toán mã hóa mật khẩu PBKDF2.
   ┗ 📂 Validations          : Kiểm thử toàn bộ bộ luật FluentValidation.
```

---

## ⚙️ Cấu hình Hệ thống (`appsettings.json`)

Mở file `SchoolHub.API/appsettings.json` và cấu hình chuỗi kết nối và thông số JWT:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=StudentManagement;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"
  },
  "Jwt": {
    "Key": "ChuoiKhoaBiMatToiThieu256BitRatDaiDungDeKyChuKyDienTuHMACSHA256",
    "Issuer": "SchoolHub.API",
    "Audience": "SchoolHub.Client",
    "DurationInMinutes": 15
  }
}
```

### Các bước khởi tạo Database:
1. Mở Terminal tại thư mục `SchoolHub.API`:
   ```bash
   dotnet ef database update --project ../SchoolHub.Infrastructure --startup-project .
   ```
2. Khởi chạy ứng dụng:
   ```bash
   dotnet run --project SchoolHub.API
   ```
3. Truy cập Swagger UI: **`https://localhost:62182/swagger`** *(hoặc `http://localhost:62183/swagger`)*.

---

## 🔐 Hệ thống Xác thực & Phân quyền (Auth & Authorization)

### 1. Cơ chế hoạt động
- **Bảo vệ Mật khẩu:** Sử dụng thuật toán **PBKDF2 (SHA-256)** với 10,000 vòng lặp kết hợp 16 bytes muối ngẫu nhiên (`Salt`). So khớp mật khẩu bằng `CryptographicOperations.FixedTimeEquals` chống tấn công đo thời gian (Timing Attack).
- **Access Token:** Định dạng JWT có chữ ký điện tử HMAC-SHA256, thời gian sống ngắn (**15 phút**), mang theo các Claims (`sub`, `unique_name`, `role`, `jti`).
- **Refresh Token Rotation:** Chuỗi ngẫu nhiên 64-bytes lưu trong Database (hạn **7 ngày**). Khi đổi token mới, token cũ bị hủy ngay lập tức (`IsUsed = true`) và cấp phát cặp token mới để chống đánh cắp token (Replay Attack).
- **Thu hồi (Revoke / Logout):** Đổi cờ `IsRevoked = true` để vô hiệu hóa phiên làm việc.

---

### 2. Bảng Ma trận Phân quyền toàn hệ thống

Toàn bộ hệ thống được phân quyền nghiêm ngặt dựa trên **Roles** và **Policies**:

| Phân hệ | Endpoint | Method | Phân quyền áp dụng | Quyền hạn thực tế |
| :--- | :--- | :---: | :--- | :--- |
| **Auth** | `/api/auth/*` | POST | `[AllowAnonymous]` | 🌐 Mọi người (Kể cả khách chưa đăng nhập) |
| **Sinh viên** | `/api/student` | POST | `[Authorize(Policy = "AdminOnly")]` | 👑 **Chỉ duy nhất Admin** mới được thêm |
| | `/api/student/{id}` | DELETE | `[Authorize(Policy = "AdminOnly")]` | 👑 **Chỉ duy nhất Admin** mới được xóa |
| | `/api/student/{id}` | PUT | `[Authorize(Policy = "CanManageStudents")]` | 👑 **Admin** & 👨‍🏫 **Teacher** được cập nhật |
| | `/api/student/search` | GET | `[Authorize(Policy = "CanManageStudents")]` | 👑 **Admin** & 👨‍🏫 **Teacher** được tìm kiếm |
| | `/api/student/{id}` | GET | `[Authorize(Policy = "CanManageStudents")]` | 👑 **Admin** & 👨‍🏫 **Teacher** được xem chi tiết |
| | `/api/student/class/{id}`| GET | `[Authorize(Policy = "CanManageStudents")]` | 👑 **Admin** & 👨‍🏫 **Teacher** xem theo lớp |
| **Lớp học** | `/api/class` | GET | `[Authorize]` | 👥 Mọi tài khoản đăng nhập đều xem được |
| | `/api/class/{id}` | GET | `[Authorize]` | 👥 Mọi tài khoản đăng nhập đều xem được |
| | `/api/class` | POST/PUT/DELETE | `[Authorize(Policy = "AdminOnly")]` | 👑 **Chỉ Admin** được Thêm/Sửa/Xóa lớp |
| **Môn học** | `/api/subject` | GET | `[Authorize]` | 👥 Mọi tài khoản đăng nhập đều xem được |
| | `/api/subject` | POST/PUT/DELETE | `[Authorize(Policy = "AdminOnly")]` | 👑 **Chỉ Admin** được Thêm/Sửa/Xóa môn |
| **Điểm số** | `/api/score/{id}` | GET | `[Authorize]` | 👥 Mọi tài khoản đăng nhập đều xem được |
| | `/api/score` | POST/PUT | `[Authorize(Policy = "CanManageStudents")]` | 👑 **Admin** & 👨‍🏫 **Teacher** (Nhập/Sửa điểm) |
| | `/api/score/{id}` | DELETE | `[Authorize(Policy = "AdminOnly")]` | 👑 **Chỉ Admin** (Xóa cột điểm) |

---

## 🧪 Chi tiết các API Endpoints

### 🔑 1. Nhóm Xác thực (`/api/auth`)

#### a. Đăng ký tài khoản: `POST /api/auth/register`
* **Request Body:**
  ```json
  {
    "username": "admin_hieu",
    "password": "Password123@",
    "email": "admin@school.edu.vn",
    "role": "Admin"
  }
  ```
  *(Role hỗ trợ: `"Admin"`, `"Teacher"`, `"User"`)*

#### b. Đăng nhập: `POST /api/auth/login`
* **Request Body:**
  ```json
  {
    "username": "admin_hieu",
    "password": "Password123@"
  }
  ```
* **Response (200 OK):**
  ```json
  {
    "success": true,
    "message": "Đăng nhập thành công!",
    "data": {
      "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
      "refreshToken": "k7Jq8mZ1N9...rX4vP0L2w==",
      "username": "admin_hieu",
      "role": "Admin"
    }
  }
  ```

#### c. Làm mới Token: `POST /api/auth/refresh-token`
* **Request Body:**
  ```json
  {
    "accessToken": "chuỗi_access_token_vừa_hết_hạn",
    "refreshToken": "k7Jq8mZ1N9...rX4vP0L2w=="
  }
  ```

#### d. Thu hồi Token (Đăng xuất): `POST /api/auth/revoke-token`
* **Request Body:**
  ```json
  {
    "refreshToken": "k7Jq8mZ1N9...rX4vP0L2w=="
  }
  ```

---

### 👨‍🎓 2. Nhóm Sinh viên (`/api/student`)
- **`GET /api/student/search?keyword=Hieu&pageNumber=1&pageSize=10`**: Tìm kiếm và phân trang sinh viên.
- **`GET /api/student/{id}`**: Xem chi tiết 1 sinh viên.
- **`GET /api/student/class/{classId}`**: Xem danh sách sinh viên theo lớp.
- **`POST /api/student`**: Thêm sinh viên mới *(Chỉ Admin)*.
  ```json
  {
    "name": "Nguyễn Minh Hiếu",
    "dateOfBirth": "2002-05-20T00:00:00Z",
    "gender": 1,
    "classId": "GUID_HOAC_MA_LOP"
  }
  ```
- **`PUT /api/student/{id}`**: Sửa thông tin sinh viên *(Admin hoặc Teacher)*.
- **`DELETE /api/student/{id}`**: Xóa sinh viên *(Chỉ Admin)*.

---

### 🏫 3. Nhóm Lớp học (`/api/class`)
- **`GET /api/class`**: Danh sách tất cả lớp học.
- **`GET /api/class/{id}`**: Chi tiết lớp học.
- **`POST /api/class`**: Tạo lớp mới *(Admin)*.
  ```json
  {
    "name": "12A1",
    "homeRoomTeacher": "Thầy Nguyễn Văn A"
  }
  ```
- **`PUT /api/class/{id}`**: Sửa tên/giáo viên chủ nhiệm *(Admin)*.
- **`DELETE /api/class/{id}`**: Xóa lớp học *(Admin)*.

---

### 📚 4. Nhóm Môn học (`/api/subject`)
- **`GET /api/subject`**: Danh sách môn học.
- **`POST /api/subject`**: Tạo môn học mới *(Admin)*.
  ```json
  {
    "name": "Lập trình C# Nâng cao",
    "credits": 3
  }
  ```
- **`PUT /api/subject/{id}`**: Sửa thông tin môn *(Admin)*.
- **`DELETE /api/subject/{id}`**: Xóa môn *(Admin)*.

---

### 📝 5. Nhóm Điểm số (`/api/score`)
- **`GET /api/score/{id}`**: Xem điểm số theo ID.
- **`POST /api/score`**: Chấm điểm cho sinh viên *(Admin hoặc Teacher)*.
  ```json
  {
    "studentId": "GUID_SINH_VIEN",
    "subjectId": "GUID_MON_HOC",
    "scoreValue": 9.5
  }
  ```
- **`PUT /api/score/{id}`**: Sửa điểm đã chấm *(Admin hoặc Teacher)*.
- **`DELETE /api/score/{id}`**: Xóa điểm *(Chỉ Admin)*.

---

## 🎯 Hướng dẫn Kiểm thử trên Swagger UI (Nút Ổ Khóa 🔓)

Dự án đã tích hợp sẵn cơ chế **OpenAPI Bearer Security** ngay trên giao diện Swagger:

1. Chạy server bằng `dotnet run --project SchoolHub.API`.
2. Mở trình duyệt vào link: `https://localhost:62182/swagger`.
3. Gọi API `POST /api/auth/login` với tài khoản của bạn để lấy chuỗi `token`.
4. Cuộn lên đầu trang, bấm vào nút màu xanh **`Authorize` 🔓** (góc trên bên phải).
5. Dán token vào ô **Value** $\rightarrow$ Bấm nút **Authorize** $\rightarrow$ Bấm **Close**.
6. Biểu tượng ổ khóa chuyển sang trạng thái đã khóa 🔒. Bây giờ bạn có thể thử nghiệm mọi API trực tiếp trên trình duyệt!

---

## 🧪 Kịch bản Test Phân quyền Thực tế

Tạo 3 tài khoản qua `POST /api/auth/register` để kiểm tra phân quyền:
- **Admin:** `admin_test` / `Admin@123` (Role: `Admin`)
- **Giáo viên:** `teacher_test` / `Teacher@123` (Role: `Teacher`)
- **Học sinh/Khách:** `user_test` / `User@123` (Role: `User`)

| Tình huống Test | Token sử dụng | Kết quả quan sát được |
| :--- | :--- | :--- |
| **Không đăng nhập** | Không gửi Token | Bị chặn ngay từ cửa với mã **`401 Unauthorized`**. |
| **User thường xóa sinh viên** | Token của `user_test` | Server biết danh tính nhưng từ chối với mã **`403 Forbidden`**. |
| **Giáo viên sửa điểm** | Token của `teacher_test` | Thành công **`200 OK`**. |
| **Giáo viên xóa sinh viên** | Token của `teacher_test` | Bị chặn với mã **`403 Forbidden`** (chỉ Admin mới được xóa). |
| **Admin thực hiện mọi thao tác** | Token của `admin_test` | Toàn quyền Thêm, Sửa, Xóa thành công **`200 OK`**. |

---

## ⚠️ Bảng giải mã Status Codes
- **`200 OK` / `201 Created`**: Thao tác thành công.
- **`400 Bad Request`**: Dữ liệu gửi lên không đúng luật (do FluentValidation chặn lại).
- **`401 Unauthorized`**: Chưa đăng nhập, token sai hoặc token hết hạn.
- **`403 Forbidden`**: Đã đăng nhập nhưng không đủ quyền thực hiện hành động.
- **`404 Not Found`**: Bản ghi cần tìm không tồn tại.
- **`500 Internal Server Error`**: Lỗi hệ thống bất ngờ (bắt qua ExceptionMiddleware).
