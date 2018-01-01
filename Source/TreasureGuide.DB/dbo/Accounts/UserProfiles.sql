CREATE TABLE [dbo].[UserProfiles]
(
    [Id] NVARCHAR(450) NOT NULL,
    [UserName] NVARCHAR(256) NOT NULL,
    [AccountNumber] NUMERIC(9) NULL,
    [Website] NVARCHAR(200) NULL,
    [UnitId] INT NULL,
    CONSTRAINT [PK_dbo.UserProfile] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK.dbo_UserProfile_dbo.AspNetUsers] FOREIGN KEY([Id]) REFERENCES [dbo].[AspNetUsers]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK.dbo_UserProfile_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units]([Id])
)