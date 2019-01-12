CREATE TABLE [dbo].[TeamCommentScores]
(
    [TeamCommentId] INT NOT NULL,
    [Value] INT NOT NULL CONSTRAINT [DF_dbo.TeamCommentScores_Value] DEFAULT 0, 
    CONSTRAINT [PK_dbo.TeamCommentScores] PRIMARY KEY CLUSTERED ([TeamCommentId] ASC),
    CONSTRAINT [FK_dbo.TeamCommentScores_dbo.TeamComments] FOREIGN KEY([TeamCommentId]) REFERENCES [dbo].[TeamComments] ([Id]) ON DELETE CASCADE
)
