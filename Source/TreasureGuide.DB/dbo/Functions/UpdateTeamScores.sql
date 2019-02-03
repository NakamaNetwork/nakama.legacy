CREATE PROCEDURE [dbo].[UpdateTeamScores]
    @teamId int = NULL
AS
    UPDATE S
        SET [Value] = ISNULL( (SELECT SUM(V.[Value]) FROM [dbo].[TeamVotes] AS V WHERE V.[TeamId] = S.[TeamId]), 0)
    FROM [dbo].[TeamScores] AS S
        LEFT JOIN [dbo].[TeamVotes] AS V
            ON V.[TeamId] = S.[TeamId]
       WHERE
        V.[TeamId] = ISNULL(@teamId, V.[TeamId])