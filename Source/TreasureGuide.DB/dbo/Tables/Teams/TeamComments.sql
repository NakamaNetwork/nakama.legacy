CREATE TABLE [dbo].[TeamComments]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [TeamId] INT NOT NULL,
    [ParentId] INT NULL,
    [Text] NVARCHAR(4000) NOT NULL,
    [Deleted] BIT NOT NULL CONSTRAINT [DF_dbo.TeamComments_Deleted] DEFAULT 0,
    [Reported] BIT NOT NULL CONSTRAINT [DF_dbo.TeamComments_Reported] DEFAULT 0,
    [SubmittedById] NVARCHAR(450) NOT NULL,
    [SubmittedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.TeamComments_SubmittedDate] DEFAULT SYSDATETIMEOFFSET(),
    [EditedById] NVARCHAR(450) NOT NULL,
    [EditedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.TeamComments_EditedDate] DEFAULT SYSDATETIMEOFFSET(),
    [Version] INT NOT NULL CONSTRAINT [DF_dbo.TeamComments_Version] DEFAULT 0,
    CONSTRAINT [PK_dbo.TeamComments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TeamComments_dbo.Teams] FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams]([Id]),
    CONSTRAINT [FK_dbo.TeamComments_dbo.TeamComments] FOREIGN KEY([ParentId]) REFERENCES [dbo].[TeamComments]([Id]),
    CONSTRAINT [FK.dbo_TeamComments_SubmittedById_dbo.UserProfiles] FOREIGN KEY([SubmittedById]) REFERENCES [dbo].[UserProfiles]([Id]),
    CONSTRAINT [FK.dbo_TeamComments_EditedById_dbo.UserProfiles] FOREIGN KEY([EditedById]) REFERENCES [dbo].[UserProfiles]([Id])
)
GO
CREATE NONCLUSTERED INDEX [IX_dbo.TeamComments]
    ON [dbo].[TeamComments]([TeamId],[Deleted],[Reported])
        INCLUDE ([Id]);
GO