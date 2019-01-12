﻿CREATE PROCEDURE [dbo].[UpdateTeamScores]
    @teamId int = -1
AS
    UPDATE S
        SET [Value] = (SELECT SUM(V.[Value]) FROM [dbo].[TeamVotes] AS V WHERE V.[TeamId] = S.[TeamId])
    FROM [dbo].[TeamScores] AS S
        LEFT JOIN [dbo].[TeamVotes] AS V
            ON V.[TeamId] = CASE WHEN @teamId <> -1 THEN @teamId ELSE S.[TeamId] END