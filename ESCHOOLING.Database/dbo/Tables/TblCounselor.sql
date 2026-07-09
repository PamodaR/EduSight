CREATE TABLE [dbo].[TblCounselor]
(
	[CounselorId] BIGINT NOT NULL PRIMARY KEY Identity,
    [Name] VARCHAR(50) NULL,
    [Email] VARCHAR(50) NULL,
    [MobileNo] VARCHAR(15) NULL,
    [Address] VARCHAR(50) NULL,
    [Specialization] VARCHAR(100) NULL,
    [IsActive] BIT NULL,
    [CreatedDate] DATETIME NULL,
    INDEX IX_TblCounselor_Email NONCLUSTERED (Email)
)
