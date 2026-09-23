-- ==========================================
-- SCRIPT TẠO DỮ LIỆU MẪU (SEED DATA)
-- DỰ ÁN: STUDENT MANAGEMENT
-- ==========================================

USE StudentManagement;
GO

-- 1. Xóa dữ liệu cũ (Xóa theo thứ tự con trước, cha sau để không vi phạm Khóa Ngoại)
DELETE FROM Scores;
DELETE FROM Students;
DELETE FROM Subjects;
DELETE FROM Classes;
GO

-- 2. Thêm dữ liệu Lớp học (Classes)
INSERT INTO Classes (ClassId, ClassName) VALUES
('C10A', N'Lớp 10A - Chuyên Toán'),
('C10B', N'Lớp 10B - Chuyên Lý'),
('C10C', N'Lớp 10C - Chuyên Anh');
GO

-- 3. Thêm dữ liệu Môn học (Subjects)
INSERT INTO Subjects (SubjectId, SubjectName) VALUES
('MATH', N'Toán Học Cao Cấp'),
('PHYS', N'Vật Lý Cơ Bản'),
('ENG',  N'Tiếng Anh Giao Tiếp');
GO

-- 4. Thêm dữ liệu Sinh viên (Students)
-- Định nghĩa các GUID cố định để dễ quản lý Khóa Ngoại ở bảng Điểm
DECLARE @Stu1 UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';
DECLARE @Stu2 UNIQUEIDENTIFIER = '22222222-2222-2222-2222-222222222222';
DECLARE @Stu3 UNIQUEIDENTIFIER = '33333333-3333-3333-3333-333333333333';
DECLARE @Stu4 UNIQUEIDENTIFIER = '44444444-4444-4444-4444-444444444444';
DECLARE @Stu5 UNIQUEIDENTIFIER = '55555555-5555-5555-5555-555555555555';

INSERT INTO Students (Id, Name, DateOfBirth, Gender, ClassId) VALUES
(@Stu1, N'Nguyễn Trường Giang', '2005-05-15', 2, 'C10A'),
(@Stu2, N'Trần Thị Thu Hà', '2005-08-20', 2, 'C10A'),
(@Stu3, N'Lê Hoàng Phúc', '2005-12-01', 1, 'C10B'),
(@Stu4, N'Phạm Bích Ngọc', '2005-03-08', 2, 'C10C'),
(@Stu5, N'Vũ Đức Đam', '2005-09-02', 1, 'C10C');
GO

-- 5. Thêm dữ liệu Điểm số (Scores)
DECLARE @Stu1 UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';
DECLARE @Stu2 UNIQUEIDENTIFIER = '22222222-2222-2222-2222-222222222222';
DECLARE @Stu3 UNIQUEIDENTIFIER = '33333333-3333-3333-3333-333333333333';
DECLARE @Stu4 UNIQUEIDENTIFIER = '44444444-4444-4444-4444-444444444444';
DECLARE @Stu5 UNIQUEIDENTIFIER = '55555555-5555-5555-5555-555555555555';

INSERT INTO Scores (Id, Value, StudentId, SubjectId) VALUES
(NEWID(), 9.5, @Stu1, 'MATH'),
(NEWID(), 8.0, @Stu1, 'PHYS'),
(NEWID(), 7.5, @Stu2, 'MATH'),
(NEWID(), 9.0, @Stu2, 'ENG'),
(NEWID(), 6.5, @Stu3, 'PHYS'),
(NEWID(), 8.5, @Stu4, 'ENG'),
(NEWID(), 10.0, @Stu5, 'MATH'),
(NEWID(), 9.5, @Stu5, 'ENG');
GO

-- 6. Thủ tục lưu trữ (Stored Procedure) cho Dapper Query
CREATE OR ALTER PROCEDURE sp_GetStudentsByClass
    @ClassId NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Students WHERE ClassId = @ClassId;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetClassById
    @ClassId NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Classes WHERE ClassId = @ClassId;
END;
GO

CREATE OR ALTER PROCEDURE sp_CountStudentsInClass
    @ClassId NVARCHAR(50),
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @TotalCount = COUNT(1) FROM Students WHERE ClassId = @ClassId;
END;
GO

PRINT N'✅ Đã nạp dữ liệu mẫu và tạo Stored Procedures thành công!';
