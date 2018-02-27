CREATE TABLE [dbo].[StageAliases]
(
    [StageId] INT NOT NULL,
    [Name] NVARCHAR(128) NOT NULL,
    [EditedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.StageAliases_EditedDate] DEFAULT SYSDATETIMEOFFSET(),
    CONSTRAINT [PK_dbo.StageAliases] PRIMARY KEY CLUSTERED ([StageId] ASC, [Name] ASC),
    CONSTRAINT [FK_dbo.StageAliasess_dbo.Stages] FOREIGN KEY([StageId]) REFERENCES [dbo].[Stages] ([Id]) ON DELETE CASCADE
)
