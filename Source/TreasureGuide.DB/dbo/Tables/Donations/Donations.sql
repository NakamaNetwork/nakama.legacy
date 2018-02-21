CREATE TABLE [dbo].[Donations]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [UserId] NVARCHAR(450) NOT NULL,
    [Amount] MONEY NOT NULL,
    [Message] NVARCHAR(500) NULL,
    [Date] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.Donations_SubmittedDate] DEFAULT SYSDATETIMEOFFSET(),
    [State] TINYINT NOT NULL,
    [TransactionType] TINYINT NOT NULL,
    [TransactionId] NVARCHAR(450) NOT NULL,
    [Public] BIT NULL CONSTRAINT [DF_dbo.Donations_Public] DEFAULT 0,
    CONSTRAINT [PK_dbo.Donations] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK.dbo_Donations_dbo.UserProfiles] FOREIGN KEY([UserId]) REFERENCES [dbo].[UserProfiles]([Id])
)
