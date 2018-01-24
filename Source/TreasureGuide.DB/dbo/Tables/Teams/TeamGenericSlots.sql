CREATE TABLE [dbo].[TeamGenericSlots]
(
    [TeamId] INT NOT NULL,
    [Position] TINYINT NOT NULL,
    [Type] SMALLINT NULL,
    [Class] SMALLINT NULL,
    [Role] SMALLINT NULL,
    [Combo] TINYINT NULL,
    CONSTRAINT [PK_dbo.TeamGenericSlots] PRIMARY KEY CLUSTERED ([TeamId] ASC, [Position] ASC),
    CONSTRAINT [CK_dbo.TeamGenericSlots_Position] CHECK ([Position] >= 0 AND [Position] < 7),
    CONSTRAINT [FK_dbo.TeamGenericSlots_dbo.Teams] FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE
)
