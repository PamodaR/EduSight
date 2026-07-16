CREATE TABLE [dbo].[TblParentNote]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ParentId] BIGINT NOT NULL,
    [StudentId] BIGINT NOT NULL,
    [NoteText] NVARCHAR(1000) NOT NULL,
    [IsActive] BIT NULL,
    [CreatedDate] DATETIME NULL,
    FOREIGN KEY (ParentId) REFERENCES TblUserRegistration(UserId),
    FOREIGN KEY (StudentId) REFERENCES TblUserRegistration(UserId)
)
