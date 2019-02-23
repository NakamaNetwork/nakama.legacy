CREATE TABLE [dbo].[TeamCredits]
(
    [TeamId] INT NOT NULL,
    [Credit] NVARCHAR(450) NOT NULL,
    [Type] SMALLINT NOT NULL CONSTRAINT [DF_dbo.TeamCredits_Type] DEFAULT 0, 
    CONSTRAINT [PK_dbo.TeamCredits] PRIMARY KEY CLUSTERED ([TeamId] ASC),
    CONSTRAINT [FK_dbo.TeamCredits_dbo.Teams] FOREIGN KEY([TeamId]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE
)
