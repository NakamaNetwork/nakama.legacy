﻿CREATE TRIGGER [TeamVideoTrigger]
    ON [dbo].[TeamVideos]
    FOR INSERT
    AS
    BEGIN
        SET NOCOUNT ON
        INSERT INTO [dbo].[Notifications]([UserId], [EventType], [EventId], [TriggerUserId])
            SELECT 
                T.[SubmittedById], 3, T.[Id], I.[UserId]
            FROM INSERTED AS I
            JOIN [dbo].[Teams] AS T
                ON I.[TeamId] = T.[Id]
            WHERE T.[SubmittedById] != I.[UserId]
    END
GO