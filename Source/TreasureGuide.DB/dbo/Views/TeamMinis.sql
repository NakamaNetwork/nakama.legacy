CREATE VIEW [dbo].[TeamMinis] AS
(
    SELECT
        T.[Id], 
        T.[StageId], 
        U1.[UnitId] AS [Slot1], 
        U2.[UnitId] AS [Slot2],
        U3.[UnitId] AS [Slot3], 
        U4.[UnitId] AS [Slot4], 
        U5.[UnitId] AS [Slot5], 
        U6.[UnitId] AS [Slot6]
    FROM [dbo].[Teams] AS T
        LEFT JOIN [dbo].[TeamUnits] AS U1
            ON U1.[TeamId] = T.[Id] AND U1.[Sub] = 0 AND U1.[Position] = 0
        LEFT JOIN [dbo].[TeamUnits] AS U2
            ON U2.[TeamId] = T.[Id] AND U2.[Sub] = 0 AND U2.[Position] = 1
        LEFT JOIN [dbo].[TeamUnits] AS U3
            ON U3.[TeamId] = T.[Id] AND U3.[Sub] = 0 AND U3.[Position] = 2
        LEFT JOIN [dbo].[TeamUnits] AS U4
            ON U4.[TeamId] = T.[Id] AND U4.[Sub] = 0 AND U4.[Position] = 3
        LEFT JOIN [dbo].[TeamUnits] AS U5
            ON U5.[TeamId] = T.[Id] AND U5.[Sub] = 0 AND U5.[Position] = 4
        LEFT JOIN [dbo].[TeamUnits] AS U6
            ON U6.[TeamId] = T.[Id] AND U6.[Sub] = 0 AND U6.[Position] = 5
)
