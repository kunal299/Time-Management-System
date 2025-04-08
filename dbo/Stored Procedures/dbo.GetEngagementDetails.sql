USE [UCITMSDev]
GO

/****** Object:  StoredProcedure [dbo].[GetEngagementDetails]    Script Date: 21-10-2024 10:08:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[GetEngagementDetails]
    @EngagementID INT = NULL -- If NULL, it will return all engagements
AS
BEGIN
    -- Select engagement details
    SELECT 
        E.EngagementID,
        E.Title,
        E.StartDate,
        E.EndDate,
        E.Description,
        E.CreatedBy,
        --U.DisplayName AS CreatedByName,
        E.ModifiedBy,
        --U2.DisplayName AS ModifiedByName,
        E.ModifiedOn,
        E.CreatedOn,
        E.IsActive,
        E.EngagementCategoryID
    FROM 
        Engagements E
    LEFT JOIN 
        Users U ON E.CreatedBy = U.UserID
    LEFT JOIN 
        Users U2 ON E.ModifiedBy = U2.UserID
    WHERE 
        (@EngagementID IS NULL OR E.EngagementID = @EngagementID);

    

    -- Select tasks related to engagement(s)
    SELECT 
        ETM.EngagementTaskID,
        ETM.EngagementID,
        ET.TaskID,
        ET.TaskName,
        ET.TaskDescription,
        ET.CreatedBy,
        ET.ModifiedBy,
        ET.ModifiedOn,
        ET.CreatedOn
    FROM 
        EngagementTaskMapping ETM
    INNER JOIN 
        EngagementTasks ET ON ETM.TaskID = ET.TaskID
    WHERE 
        (@EngagementID IS NULL OR ETM.EngagementID = @EngagementID);

    

    -- Select team members related to engagement(s)
    SELECT 
        EUM.MappingID,
		 EUM.EngagementID, -- Added EngagementID
        EUM.UserID,
        U.DisplayName AS TeamMemberName,
        EUM.StartDate,
        EUM.EndDate,
        EUM.MaxWeeklyHours,
        EUM.CreatedBy,
        EUM.ModifiedBy,
        EUM.ModifiedOn,
        EUM.CreatedOn
    FROM 
        EngagementUserMapping EUM
    INNER JOIN 
        Users U ON EUM.UserID = U.UserID
    WHERE 
        (@EngagementID IS NULL OR EUM.EngagementID = @EngagementID);

    
END
GO


