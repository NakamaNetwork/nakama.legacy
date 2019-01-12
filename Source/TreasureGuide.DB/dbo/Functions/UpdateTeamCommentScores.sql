CREATE PROCEDURE [dbo].[UpdateTeamCommentScores]
    @teamCommentId int = -1
AS
    UPDATE S
        SET [Value] = ISNULL( (SELECT SUM(V.[Value]) FROM [dbo].[TeamCommentVotes] AS V WHERE V.[TeamCommentId] = S.[TeamCommentId]), 0)
    FROM [dbo].[TeamCommentScores] AS S
        LEFT JOIN [dbo].[TeamCommentVotes] AS V
            ON V.[TeamCommentId] = CASE WHEN @teamCommentId <> -1 THEN @teamCommentId ELSE S.[TeamCommentId] END