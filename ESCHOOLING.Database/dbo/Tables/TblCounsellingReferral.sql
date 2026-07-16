CREATE TABLE [dbo].[TblCounsellingReferral]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [StudentId] BIGINT NOT NULL,
    [CounselorId] BIGINT NOT NULL,
    [TeacherId] BIGINT NOT NULL,
    [Reason] NVARCHAR(500) NOT NULL,
    [IsActive] BIT NULL,
    [CreatedDate] DATETIME NULL,
    FOREIGN KEY (StudentId) REFERENCES TblUserRegistration(UserId),
    FOREIGN KEY (CounselorId) REFERENCES TblCounselor(CounselorId),
    FOREIGN KEY (TeacherId) REFERENCES TblUserRegistration(UserId)
)
