CREATE VIEW [dbo].[GlobalClearRates]
    AS
SELECT
    T.[Id],
    L.[UnitId] AS [LeaderId],
    T.[StageId],
    CAST(CASE WHEN (LU.[Flags] & 1 = 1) THEN 1 ELSE 0 END AS BIT) AS [GlobalLead],
    CAST(CASE WHEN SUM(CASE WHEN U.[Position] = 0 OR (SU.[Flags] & 2 = 0 AND SU.[Flags] & 4 = 0 AND SU.[Flags] & 8 = 0) THEN 1 ELSE 0 END) = COUNT(U.[UnitId]) THEN 1 ELSE 0 END AS BIT) AS [F2P],
    CAST(CASE WHEN SUM(CASE WHEN SU.[Flags] & 1 = 1 THEN 1 ELSE 0 END) = COUNT(U.[UnitId]) THEN 1 ELSE 0 END AS BIT) AS [Global],
    CAST(CASE WHEN COUNT(V.[Id]) > 0 THEN 1 ELSE 0 END AS BIT) AS [Video]
FROM [dbo].[Teams] AS T
    JOIN [dbo].[TeamUnits] AS L
        ON T.[Id] = L.[TeamId] AND L.[Position] = 1 AND L.[Sub] = 0 AND L.[Flags] IS NOT NULL
    LEFT JOIN [dbo].[Units] AS LU
        ON L.[UnitId] = LU.[Id]
    LEFT JOIN [dbo].[TeamUnits] AS U
        ON T.[Id] = U.[TeamId] AND U.[Position] <> 1 AND U.[Sub] = 0
    LEFT JOIN [dbo].[Units] AS SU
        ON U.[UnitId] = SU.[Id]
    LEFT JOIN [dbo].[TeamVideos] AS V
        ON T.[Id] = V.[TeamId]
    WHERE T.[StageId] IS NOT NULL AND T.[Draft] = 0 AND T.[Deleted] = 0 AND ISNULL(V.[Deleted], 0) = 0
    GROUP BY T.[Id], T.[StageId], L.[UnitId], LU.[Flags]