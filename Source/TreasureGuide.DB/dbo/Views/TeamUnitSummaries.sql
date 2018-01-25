CREATE VIEW [dbo].[TeamUnitSummaries] AS 
(
    SELECT 
        T.[Id] AS [TeamId],
        U.[Id] AS [UnitId],
        TU.[Position] AS [Position],
        TU.[Sub] AS [Sub],
        U.[Flags] AS [Flags],
        U.[Class] AS [Class],
        U.[Type] AS [Type]
    FROM [dbo].[Teams] AS T
        JOIN [dbo].[TeamUnits] AS TU
            ON TU.[TeamId] = T.[Id]
        JOIN [dbo].[Units] AS U
            ON TU.[Sub] = 0 AND TU.[UnitId] = U.[Id]
)
UNION
(
    SELECT
        T.[Id] AS [TeamId],
        NULL AS [UnitId],
        TS.[Position] AS [Position],
        TS.[Sub] AS [Sub],
        0 AS [Flags],
        TS.[Class] AS [Class],
        TS.[Type] AS [Type]
    FROM [dbo].[Teams] AS T
        JOIN [dbo].[TeamGenericSlots] AS TS
            ON TS.[TeamId] = T.[Id]
)