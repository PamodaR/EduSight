CREATE TABLE [dbo].[TblUserRegistration]
(
	[UserId] BIGINT NOT NULL PRIMARY KEY Identity,
    [Username] VARCHAR(50) NULL,
    [Email] VARCHAR(50) NULL,
    [Password] VARCHAR(50) NULL,
    [Address] VARCHAR(50) NULL,
    [MobileNo] VARCHAR(15) NULL,
    [IsActive] BIT NULL,
    [UserType] INT NULL,
    [CreatedDate] DATETIME NULL,
    [Grade] INT NULL,
    [IsPresent] BIT NULL,
    [ChildStudentId] BIGINT NULL,
    [ProfilePicturePath] VARCHAR(260) NULL,
    INDEX IX_TblUserRegistration_Email NONCLUSTERED (Email),
    INDEX IX_TblUserRegistration_UserType_Grade NONCLUSTERED (UserType, Grade),
    FOREIGN KEY (ChildStudentId) REFERENCES TblUserRegistration(UserId)
)
