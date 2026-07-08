CREATE TABLE [dbo].[TblEvents]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [EventName] NVARCHAR(50) NULL, 
    [Date] DATETIME NULL, 
    [Place] NVARCHAR(50) NULL, 
    [Time] NVARCHAR(50) NULL, 
    [IsActive] BIT NULL
)
