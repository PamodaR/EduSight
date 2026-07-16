CREATE TABLE [dbo].[TblStudentMarksEntry]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [StudentId] BIGINT NOT NULL,
    [Term] NVARCHAR(50) NOT NULL,
    [Subject] NVARCHAR(50) NOT NULL,
    [Marks] DECIMAL(5,2) NOT NULL,
    [IsActive] BIT NULL,
    [CreatedDate] DATETIME NULL,
    FOREIGN KEY (StudentId) REFERENCES TblUserRegistration(UserId)
)
