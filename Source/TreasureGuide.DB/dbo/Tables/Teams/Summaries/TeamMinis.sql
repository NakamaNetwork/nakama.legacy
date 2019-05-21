CREATE TABLE [dbo].[TeamMinis]
(
    [TeamId] INT NOT NULL,
    [Name] NVARCHAR(250) NOT NULL,
    [StageId] INT NULL,
    [StageName] NVARCHAR(128) NULL,
    [InvasionId] INT NULL,
    [InvasionName] NVARCHAR(128) NULL,
    [EventShip] BIT NOT NULL,
    [SubmittedById] NVARCHAR(450) NOT NULL,
    [SubmittingUserName] NVARCHAR(256) NOT NULL,
    [Draft] BIT NOT NULL,
    [Deleted] BIT NOT NULL,
    [HasReport] BIT NOT NULL,
    [Global] BIT NOT NULL,
    [HasSupports] BIT NOT NULL,
    [F2P] BIT NOT NULL,
    [F2PC] BIT NOT NULL,
    [Type] SMALLINT NOT NULL,
    [Class] SMALLINT NOT NULL,
    [HelperId] INT NULL,
    [LeaderId] INT NULL,
    CONSTRAINT [PK_dbo.TeamMinis] PRIMARY KEY CLUSTERED ([TeamId] ASC),
    CONSTRAINT [FK.dbo.TeamMinis_dbo.Teams] FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK.dbo.TeamMinis_dbo.Stages] FOREIGN KEY([StageId]) REFERENCES [dbo].[Stages]([Id]),
    CONSTRAINT [FK.dbo.TeamMinis_dbo.Stages_Invasions] FOREIGN KEY([InvasionId]) REFERENCES [dbo].[Stages]([Id]),
    CONSTRAINT [FK.dbo.TeamMinis_SubmittedById_dbo.UserProfiles] FOREIGN KEY([SubmittedById]) REFERENCES [dbo].[UserProfiles]([Id]),
    CONSTRAINT [FK_dbo.TeamMinis_dbo.Units_0] FOREIGN KEY([HelperId]) REFERENCES [dbo].[Units] ([Id]),
    CONSTRAINT [FK_dbo.TeamMinis_dbo.Units_1] FOREIGN KEY([LeaderId]) REFERENCES [dbo].[Units] ([Id])
)
GO
CREATE NONCLUSTERED INDEX [IX_dbo.TeamMinis]
    ON [dbo].[TeamMinis]([Draft],[Deleted],[StageId],[InvasionId],[HelperId],[LeaderId],[SubmittedById])
        INCLUDE ([TeamId]);
GO