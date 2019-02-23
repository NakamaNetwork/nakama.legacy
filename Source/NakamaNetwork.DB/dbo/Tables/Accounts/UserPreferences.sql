CREATE TABLE [dbo].[UserPreferences]
(
    [UserId] NVARCHAR(450) NOT NULL,
    [Key] INT NOT NULL,
    [Value] NVARCHAR(250) NULL,
    CONSTRAINT [PK_dbo.UserPreferences] PRIMARY KEY CLUSTERED ([UserId] ASC, [Key] ASC),
    CONSTRAINT [FK.dbo_UserPreferences_dbo.UserProfiles] FOREIGN KEY([UserId]) REFERENCES [dbo].[UserProfiles]([Id]) ON DELETE CASCADE
)
