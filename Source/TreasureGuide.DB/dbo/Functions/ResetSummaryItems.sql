CREATE PROCEDURE [dbo].[ResetSummaryItems]
AS
    INSERT INTO [dbo].[TeamMinis]([TeamId],  [Name],[EventShip],  [SubmittedById],[SubmittingUserName],[Draft],[Deleted],[HasReport],[Global],[F2P],[F2PC],[Type],[Class])
                             SELECT T.[Id],T.[Name],          0,T.[SubmittedById],  N'Monkey D. Luffy',      1,        1,          0,       0,    0,     0,     0,      0
    FROM [dbo].[Teams] AS T
    LEFT JOIN [dbo].[TeamMinis] AS M
        ON T.[Id] = M.[TeamId]
            WHERE M.[TeamId] IS NULL

            
    INSERT INTO [dbo].[TeamScores]([TeamId],  [Value])
                             SELECT  T.[Id],        0
    FROM [dbo].[Teams] AS T
    LEFT JOIN [dbo].[TeamScores] AS S
        ON T.[Id] = S.[TeamId]
            WHERE S.[TeamId] IS NULL
            
    INSERT INTO [dbo].[TeamCommentScores]([TeamCommentId],  [Value])
                                           SELECT  T.[Id],        0
    FROM [dbo].[TeamComments] AS T
    LEFT JOIN [dbo].[TeamCommentScores] AS S
        ON T.[Id] = S.[TeamCommentId]
            WHERE S.[TeamCommentId] IS NULL
            
    EXEC [dbo].[UpdateTeamMinis]
    EXEC [dbo].[UpdateTeamScores]
    EXEC [dbo].[UpdateTeamCommentScores]
RETURN 0
