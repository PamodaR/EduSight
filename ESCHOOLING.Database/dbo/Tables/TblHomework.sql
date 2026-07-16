CREATE TABLE [dbo].[TblHomework]
(
	[Id] INT NOT NULL PRIMARY KEY,
    [TeacherId] BIGINT NULL,
    [Grade] INT NULL,
    [Subject] NVARCHAR(50) NULL,
    [Description] NVARCHAR(500) NULL,
    [DueDate] DATETIME NULL,
    [IsActive] BIT NULL,
    [CreatedDate] DATETIME NULL,
     FOREIGN KEY (TeacherId) REFERENCES TblUserRegistration(UserId)
)
