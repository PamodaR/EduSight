CREATE TABLE [dbo].[TblBehaviour]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [StudentId] BIGINT NULL, 
    [behaviour1] NVARCHAR(50) NULL, 
    [behaviour2] NVARCHAR(50) NULL, 
    [behaviour3] NVARCHAR(50) NULL, 
    [behaviour4] NVARCHAR(50) NULL, 
    [behaviour5] NVARCHAR(50) NULL, 
    [Description] NVARCHAR(100) NULL, 
    [IsActive] BIT NULL, 
    [CreatedDate] DATETIME NULL,
    FOREIGN KEY (StudentId) REFERENCES TblUserRegistration(UserId)
)
