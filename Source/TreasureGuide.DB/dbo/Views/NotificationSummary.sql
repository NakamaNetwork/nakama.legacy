CREATE VIEW [dbo].[NotificationSummary]
    AS SELECT 
    N.*, 
    ISNULL(
        T.[Name], -- TEAM NAME
        CASE      -- COMMENT SUMMARY
            WHEN LEN(C.[Text]) > 50
            THEN LEFT(C.[Text], 50) + '...'
            ELSE C.[Text]
        END
    ) AS [EventInfo], 
    CONVERT(NVARCHAR, C.[TeamId]) AS [ExtraInfo],
    U.[UserName] AS [TriggerUserName]
FROM [dbo].[Notifications] AS N
    LEFT JOIN [dbo].[Teams] AS T
        ON N.[EventId] = T.[Id] AND N.[EventType] IN (1,3)
    LEFT JOIN [dbo].[TeamComments] AS C
        ON N.[EventId] = C.[Id] AND N.[EventType] = 2
    LEFT JOIN [dbo].[UserProfiles] AS U
        ON N.[TriggerUserId] = U.[Id]