USE [UCITMSDev]
GO

/****** Object:  StoredProcedure [dbo].[GetEngagementsforEmployee]    Script Date: 21-10-2024 10:12:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetEngagementsforEmployee]
    @UserId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    -- Select engagements for the team member and include all members of the same engagement
    SELECT 
        E.EngagementID,
        E.Title,
        E.CreatedBy,
		U.DisplayName AS CreatedByName,
        E.StartDate,
        E.EndDate,
        STRING_AGG(OTM.DisplayName, ', ') AS TeamMembers  -- Concatenate team members' names
    FROM Engagements E
    INNER JOIN EngagementUserMapping EUM ON E.EngagementID = EUM.EngagementID
    INNER JOIN Users TM ON EUM.UserID = TM.UserID
    INNER JOIN EngagementUserMapping OtherEUM ON E.EngagementID = OtherEUM.EngagementID
    INNER JOIN Users OTM ON OtherEUM.UserID = OTM.UserID
	INNER JOIN Users U ON E.CreatedBy = U.UserID
    WHERE TM.UserID = @UserId
    GROUP BY E.EngagementID, E.Title, E.StartDate, E.EndDate, E.CreatedBy, U.DisplayName;
END
GO


