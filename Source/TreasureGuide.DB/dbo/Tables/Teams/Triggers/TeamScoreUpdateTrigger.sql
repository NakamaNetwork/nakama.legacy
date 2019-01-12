CREATE TRIGGER [TeamScoreUpdateTrigger]
    ON [dbo].[TeamVotes]
    FOR INSERT, UPDATE, DELETE
    AS
    BEGIN
        SET NOCOUNT ON
        DECLARE @PractitionerId int

        DECLARE MY_CURSOR CURSOR 
          LOCAL STATIC READ_ONLY FORWARD_ONLY
        FOR 
        SELECT DISTINCT [TeamId] FROM INSERTED
        UNION 
        SELECT DISTINCT [TeamId] FROM DELETED

        OPEN MY_CURSOR
        FETCH NEXT FROM MY_CURSOR INTO @PractitionerId
        WHILE @@FETCH_STATUS = 0
        BEGIN 
            EXEC [dbo].[UpdateTeamScores] @PractitionerId
            FETCH NEXT FROM MY_CURSOR INTO @PractitionerId
        END
        CLOSE MY_CURSOR
        DEALLOCATE MY_CURSOR
    END
GO