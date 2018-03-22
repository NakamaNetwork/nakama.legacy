CREATE TABLE [dbo].[DeletedItems]
(
    [Id] INT NOT NULL,
    [Type] INT NOT NULL,
    [EditedDate] DATETIMEOFFSET(7) NOT NULL,
    CONSTRAINT [PK_dbo.DeletedItems] PRIMARY KEY CLUSTERED ([Id] ASC, [Type] ASC, [EditedDate] ASC)
)
