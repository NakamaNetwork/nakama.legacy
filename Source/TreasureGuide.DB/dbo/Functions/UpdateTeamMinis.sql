CREATE PROCEDURE [dbo].[UpdateTeamMinis]
    @teamId int = NULL
AS
-- BASIC INFO
print 'Setting Basic Info'
    UPDATE M SET
        M.[Name] = T.[Name],
        M.[StageId] = T.[StageId],
        M.[StageName] = S.[Name],
        M.[InvasionId] = T.[Invasionid],
        M.[InvasionName] = I.[Name],
        M.[EventShip] = Sh.[EventShip],
        M.[SubmittedById] = T.[SubmittedById],
        M.[SubmittingUserName] = P.[UserName],
        M.[Draft] = T.[Draft],
        M.[Deleted] = T.[Deleted],
        M.[HasReport] = CASE WHEN R.[AcknowledgedDate] IS NOT NULL THEN 1 ELSE 0 END,
        M.[HelperId] = H.[UnitId],
        M.[LeaderId] = L.[UnitId],
        M.[F2P] = 0,
        M.[F2PC] = 0,
        M.[Global] = 0,
        M.[Type] = 0,
        M.[Class] = 0
    FROM [dbo].[TeamMinis] AS M
        JOIN [dbo].[Teams] AS T
            ON M.[TeamId] = T.[Id]
        LEFT JOIN [dbo].[Stages] AS S
            ON T.[StageId] = S.[Id]
        LEFT JOIN [dbo].[Stages] AS I
            ON T.[InvasionId] = I.[Id]
        LEFT JOIN [dbo].[Ships] AS Sh
            ON Sh.[Id] = T.[ShipId]
        LEFT JOIN [dbo].[UserProfiles] AS P
            ON P.[Id] = T.[SubmittedById]
        LEFT JOIN (SELECT 
                        R.[TeamId], MAX(R.[AcknowledgedDate]) AS [AcknowledgedDate]
                    FROM [dbo].[TeamReports] AS R
                        GROUP BY R.[TeamId]) AS R
            ON R.[TeamId] = S.[Id]
        LEFT JOIN [dbo].[TeamUnits] AS H
            ON H.[TeamId] = T.[Id] AND H.[Sub] = 0 AND H.[Position] = 0
        LEFT JOIN [dbo].[TeamUnits] AS L
            ON L.[TeamId] = T.[Id] AND L.[Sub] = 0 AND L.[Position] = 1
        LEFT JOIN [dbo].[Units] AS LU
            ON LU.[Id] = L.[UnitId]
        WHERE
            T.[Id] = ISNULL(@teamId,T.[Id])

print 'Fetching Units'
DECLARE @unitTable TABLE (
    [TeamId] INT,
    [UnitId] INT,
    [Position] INT,
    [Flags] INT,
    [Type] INT,
    [Class] INT
)

INSERT INTO @unitTable SELECT
        M.[TeamId],
        U.[Id],
        TU.[Position],
        U.[Flags],
        U.[Type],
        U.[Class]
    FROM [dbo].[TeamMinis] AS M
        JOIN [dbo].[Teamunits] AS TU
            ON TU.[TeamId] = M.[TeamId] AND TU.[Sub] = 0
        JOIN [dbo].[Units] AS U
            ON TU.[UnitId] = U.[Id]
        WHERE M.[TeamId] = ISNULL(@teamId, M.[TeamId])
        
print 'Fetching Base Stats'
DECLARE @statTable TABLE (
    [TeamId] INT,
    [AllFlags] INT,
    [CrewFlags] INT,
    [Type] INT,
    [Class] INT
) 

INSERT INTO @statTable SELECT
    M.[TeamId],
    A.[Flags],
    C.[Flags],
    A.[Type],
    A.[Class]
FROM [dbo].[TeamMinis] AS M
    CROSS APPLY (
        SELECT
            [TeamId],
            MAX([Flags]) AS [Flags],
            MAX([Class]) AS [Class],
            MAX([Type]) AS [Type]
        FROM @unitTable AS U
            WHERE M.[TeamId] = U.[TeamId] 
                GROUP BY [TeamId]
    ) A
    CROSS APPLY (
        SELECT
            [TeamId],
            MAX([Flags]) AS [Flags],
            MAX([Class]) AS [Class],
            MAX([Type]) AS [Type]
        FROM @unitTable AS U
            WHERE M.[TeamId] = U.[TeamId]  AND U.[Position] > 1
                GROUP BY [TeamId]
    ) C
WHERE
    M.[TeamId] = ISNULL(@teamId, M.[TeamId])
    
print 'Fetching Working Stats'
DECLARE @enumTable TABLE
(
    [TeamId] INT,
    [Type] INT,
    [Value] INT
)    

INSERT INTO @enumTable
SELECT DISTINCT
    U.[TeamId],
    E.[number],
    CASE
        WHEN E.[number] = 0 THEN U.[Flags]
        WHEN E.[number] = 1 THEN U.[Type]
        WHEN E.[number] = 2 THEN U.[Class]
        WHEN U.[Position] > 1 AND E.[number] = 3 THEN U.[Flags]
        ELSE -1
    END
FROM [master]..[spt_values] AS E
    JOIN @unitTable AS U
        ON 1 = 1
    WHERE E.[number] BETWEEN 0 AND 3
    
print 'Crunching Stats'
UPDATE M SET
    M.[Global] = V.[AllFlags] & 1,
    M.[F2P] = CASE WHEN ((V.[AllFlags] & 2) + (V.[AllFlags] & 4) + (V.[AllFlags] & 8)) > 0 THEN 0 ELSE 1 END,
    M.[F2PC] = CASE WHEN ((V.[CrewFlags] & 2) + (V.[CrewFlags] & 4) + (V.[CrewFlags] & 8)) > 0 THEN 0 ELSE 1 END,
    M.[Class] = V.[Class],
    M.[Type] = V.[Type]
FROM [dbo].[TeamMinis] AS M
CROSS APPLY
(
    SELECT
        S.[TeamId],
        [AllFlags]  = MAX(S.[AllFlags]  & CASE WHEN E.[Type] = 0 THEN E.[Value] ELSE S.[AllFlags]  END),
        [CrewFlags] = MAX(S.[CrewFlags] & CASE WHEN E.[Type] = 3 AND E.[Value] <> -1 THEN E.[Value] ELSE S.[CrewFlags] END),
        [Type]      = MIN(S.[Type]      & CASE WHEN E.[Type] = 1 THEN E.[Value] ELSE S.[Type]      END),
        [Class]     = MIN(S.[Class]     & CASE WHEN E.[Type] = 2 THEN E.[Value] ELSE S.[Class]     END)
    FROM @statTable AS S
        JOIN @unitTable AS U
            ON U.[TeamId] = S.[TeamId]
        JOIN @enumTable as E
            ON S.[TeamId] = E.[TeamId]
    WHERE
        S.[TeamId] = ISNULL(@teamId, S.[TeamId])
    GROUP BY S.[TeamId]
) AS V
WHERE
    M.[TeamId] = V.[TeamId]