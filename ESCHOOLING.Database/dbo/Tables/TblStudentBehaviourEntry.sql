CREATE TABLE [dbo].[TblStudentBehaviourEntry]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [StudentId] BIGINT NOT NULL,
    [BehaviourType] NVARCHAR(20) NOT NULL,
    [Description] NVARCHAR(500) NOT NULL,
    [MonthForSearch] NVARCHAR(50) NULL,
    [IsActive] BIT NULL,
    [CreatedDate] DATETIME NULL,
    FOREIGN KEY (StudentId) REFERENCES TblUserRegistration(UserId)
)
