CREATE PROCEDURE [dbo].[UpdateTeamCommentScores]
    @teamCommentId int = NULL
AS
    UPDATE S
        SET [Value] = ISNULL( (SELECT SUM(V.[Value]) FROM [dbo].[TeamCommentVotes] AS V WHERE V.[TeamCommentId] = S.[TeamCommentId]), 0)
    FROM [dbo].[TeamCommentScores] AS S
        LEFT JOIN [dbo].[TeamCommentVotes] AS V
            ON V.[TeamCommentId] = S.[TeamCommentId]
    WHERE
        V.[TeamCommentId] = ISNULL(@teamCommentId, V.[TeamCommentId])