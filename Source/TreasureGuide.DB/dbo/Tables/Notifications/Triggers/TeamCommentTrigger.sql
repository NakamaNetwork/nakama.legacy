CREATE TRIGGER [TeamCommentTrigger]
    ON [dbo].[TeamComments]
    FOR INSERT
    AS
    BEGIN
        SET NOCOUNT ON
        INSERT INTO [dbo].[Notifications]([UserId], [EventType], [EventId])
            SELECT 
                T.[SubmittedById], 1, T.[Id]
            FROM INSERTED AS I
            JOIN [dbo].[Teams] AS T
                ON I.[TeamId] = T.[Id]
            WHERE T.[SubmittedById] != I.[SubmittedById]
    END
GO