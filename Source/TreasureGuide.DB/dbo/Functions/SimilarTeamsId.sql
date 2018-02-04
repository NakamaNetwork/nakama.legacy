CREATE FUNCTION [dbo].[SimilarTeamsId]
(
    @teamId INT
)
RETURNS @returntable TABLE
(
    [TeamId] INT,
    [StageId] INT,
    [Matches] INT
)
AS
BEGIN
    INSERT @returntable
        SELECT S.* FROM [dbo].[TeamMinis] AS T
            CROSS APPLY
        [dbo].[SimilarTeams](T.[Id], T.[StageId], T.[Slot1], T.[Slot2], T.[Slot3], T.[Slot4], T.[Slot5], T.[Slot6]) AS S
            WHERE T.[Id] = @teamId
    RETURN
END
