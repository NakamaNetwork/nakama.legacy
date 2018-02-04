CREATE TABLE [dbo].[Teams]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [Name] NVARCHAR(250) NOT NULL,
    [Guide] NVARCHAR(MAX) NULL,
    [Credits] NVARCHAR(2000) NULL,
    [StageId] INT NULL,
    [ShipId] INT NOT NULL CONSTRAINT [DF_dbo.Teams_ShipId] DEFAULT 0, 
    [SubmittedById] NVARCHAR(450) NOT NULL,
    [SubmittedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.Teams_SubmittedDate] DEFAULT SYSDATETIMEOFFSET(),
    [EditedById] NVARCHAR(450) NOT NULL,
    [EditedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.Teams_EditedDate] DEFAULT SYSDATETIMEOFFSET(),
    [Version] INT NOT NULL CONSTRAINT [DF_dbo.Teams_Version] DEFAULT 0,
    [Draft] BIT NOT NULL CONSTRAINT [DF_dbo.Teams_Draft] DEFAULT 0,
    [Deleted] BIT NOT NULL CONSTRAINT [DF_dbo.Teams_Deleted] DEFAULT 0,
    CONSTRAINT [PK_dbo.Teams] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK.dbo_Teams_dbo.Stages] FOREIGN KEY([StageId]) REFERENCES [dbo].[Stages]([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK.dbo_Teams_dbo.Ships] FOREIGN KEY([ShipId]) REFERENCES [dbo].[Ships]([Id]) ON DELETE SET DEFAULT,
    CONSTRAINT [FK.dbo_Teams_SubmittedById_dbo.UserProfiles] FOREIGN KEY([SubmittedById]) REFERENCES [dbo].[UserProfiles]([Id]),
    CONSTRAINT [FK.dbo_Teams_EditedById_dbo.UserProfiles] FOREIGN KEY([EditedById]) REFERENCES [dbo].[UserProfiles]([Id])
)
GO
CREATE NONCLUSTERED INDEX [IX_dbo.Teams] ON [dbo].[Teams]([StageId] ASC) INCLUDE ([Id]);