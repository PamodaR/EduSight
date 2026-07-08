CREATE TABLE [dbo].[TblStudentMarks]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [StudentId] BIGINT NULL, 
    [Subject] NVARCHAR(50) NULL, 
    [PredictedMark] NVARCHAR(50) NULL, 
    [IsActive] BIT NULL, 
    [CreatedDate] DATETIME NULL,
     FOREIGN KEY (StudentId) REFERENCES TblUserRegistration(UserId)
)
