CREATE TABLE [dbo].[TeamGenericSlots]
(
    [TeamId] INT NOT NULL,
    [Position] TINYINT NOT NULL,
    [Type] SMALLINT NOT NULL CONSTRAINT [DF_dbo.TeamGenericSlots_Type] DEFAULT 0,
    [Class] SMALLINT NOT NULL CONSTRAINT [DF_dbo.TeamGenericSlots_Class] DEFAULT 0,
    [Role] INT NOT NULL CONSTRAINT [DF_dbo.TeamGenericSlots_Role] DEFAULT 0,
    [Sub] BIT NOT NULL CONSTRAINT [DF_dbo.TeamGenericSlots_Sub] DEFAULT 0,
    CONSTRAINT [PK_dbo.TeamGenericSlots] PRIMARY KEY CLUSTERED ([TeamId] ASC, [Position] ASC, [Type] ASC, [Class] ASC, [Role] ASC),
    CONSTRAINT [CK_dbo.TeamGenericSlots_Position] CHECK ([Position] >= 0 AND [Position] < 6),
    CONSTRAINT [FK_dbo.TeamGenericSlots_dbo.Teams] FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE
)
