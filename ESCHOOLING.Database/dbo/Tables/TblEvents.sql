CREATE TABLE [dbo].[TblEvents]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [EventName] NVARCHAR(50) NULL,
    [Description] NVARCHAR(500) NULL,
    [Date] DATETIME NULL,
    [Place] NVARCHAR(50) NULL,
    [Time] NVARCHAR(50) NULL,
    [Grade] INT NULL,
    [IsActive] BIT NULL
)
