CREATE TABLE [dbo].[Boxes]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [UserId] NVARCHAR(450) NOT NULL,
    [Name] NVARCHAR(250) NOT NULL,
    [FriendId] NUMERIC(9) NULL,
    [Global] BIT NULL,
    [Public] BIT NULL CONSTRAINT [DF_dbo.Boxes_Public] DEFAULT 1,
    CONSTRAINT [PK_dbo.Boxes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK.dbo_Boxes_dbo.UserProfiles] FOREIGN KEY([UserId]) REFERENCES [dbo].[UserProfiles]([Id]) ON DELETE CASCADE
)
