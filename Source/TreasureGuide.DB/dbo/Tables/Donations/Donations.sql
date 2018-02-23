CREATE TABLE [dbo].[Donations]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [UserId] NVARCHAR(450) NULL,
    [Amount] MONEY NOT NULL,
    [Message] NVARCHAR(500) NULL,
    [Date] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.Donations_SubmittedDate] DEFAULT SYSDATETIMEOFFSET(),
    [State] TINYINT NOT NULL,
    [PaymentType] TINYINT NOT NULL,
    [PaymentId] NVARCHAR(450) NULL,
    [PayerId] NVARCHAR(450) NULL,
    [TokenId] NVARCHAR(450) NULL,
    [Public] BIT NULL CONSTRAINT [DF_dbo.Donations_Public] DEFAULT 0,
    CONSTRAINT [PK_dbo.Donations] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK.dbo_Donations_dbo.UserProfiles] FOREIGN KEY([UserId]) REFERENCES [dbo].[UserProfiles]([Id])
)
