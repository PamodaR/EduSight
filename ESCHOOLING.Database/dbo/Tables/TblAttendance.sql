CREATE TABLE [dbo].[TblAttendance]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
    [StudentId] BIGINT NOT NULL,
    [IsPresent] BIT NULL,
    [CreatedDate] DATETIME NULL,
    [IsActive] BIT NULL,
    [Date] DATE NULL,
    [MonthForSearch] NVARCHAR(50) NULL,
    FOREIGN KEY (StudentId) REFERENCES TblUserRegistration(UserId),
    INDEX IX_TblAttendance_StudentId_Date NONCLUSTERED (StudentId, Date),
    INDEX IX_TblAttendance_StudentId_MonthForSearch NONCLUSTERED (StudentId, MonthForSearch)
)
