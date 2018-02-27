CREATE TABLE [dbo].[UnitAliases]
(
    [UnitId] INT NOT NULL,
    [Name] NVARCHAR(128) NOT NULL,
    [EditedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.UnitAliases_EditedDate] DEFAULT SYSDATETIMEOFFSET(),
    CONSTRAINT [PK_dbo.UnitAliases] PRIMARY KEY CLUSTERED ([UnitId] ASC, [Name] ASC),
    CONSTRAINT [FK_dbo.UnitAliasess_dbo.Units] FOREIGN KEY([UnitId]) REFERENCES [dbo].[Units] ([Id]) ON DELETE CASCADE
)
