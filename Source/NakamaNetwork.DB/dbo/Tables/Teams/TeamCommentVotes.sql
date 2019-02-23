CREATE TABLE [dbo].[TeamCommentVotes]
(
    [TeamCommentId] INT NOT NULL,
    [UserId] NVARCHAR(450) NOT NULL,
    [SubmittedDate] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_dbo.TeamCommentVotes_SubmittedDate] DEFAULT SYSDATETIMEOFFSET(),
    [Value] SMALLINT NOT NULL CONSTRAINT [DF_dbo.TeamCommentVotes_Value] DEFAULT 0, 
    CONSTRAINT [PK_dbo.TeamCommentVotes] PRIMARY KEY CLUSTERED ([TeamCommentId] ASC, [UserId] ASC),
    CONSTRAINT [FK_dbo.TeamCommentVotes_dbo.TeamComments] FOREIGN KEY([TeamCommentId]) REFERENCES [dbo].[TeamComments] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.TeamCommentVotes_dbo.UserProfiles] FOREIGN KEY([UserId]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE CASCADE
)
GO
CREATE NONCLUSTERED INDEX [IX_dbo.TeamCommentVotes]
    ON [dbo].[TeamCommentVotes]([UserId])
        INCLUDE ([TeamCommentId]);
GO