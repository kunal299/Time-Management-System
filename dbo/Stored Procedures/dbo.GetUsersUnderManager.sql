USE [UCITMSDev]
GO

/****** Object:  StoredProcedure [dbo].[GetUsersUnderManager]    Script Date: 21-10-2024 10:22:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUsersUnderManager]
    @ManagerID INT
AS
BEGIN
    -- Select users who are under a specific manager
    SELECT 
        u.UserID,
        u.DisplayName,
        u.Email,
        u.IsActive,
        u.CreatedBy,        -- Add this if you need CreatedBy
        u.ModifiedBy,       -- Add this if you need ModifiedBy
        u.CreatedOn,        -- Add this if you need CreatedOn
        u.ModifiedOn        -- Add this if you need ModifiedOn
    FROM Users u
    INNER JOIN UserManagerMapping umm ON u.UserID = umm.UserID
    INNER JOIN UserRolesMapping urm ON u.UserID = urm.UserID
    INNER JOIN RolesMaster rm ON urm.RoleID = rm.RoleID
    WHERE umm.ManagerID = @ManagerID
    AND u.IsActive = 1  -- Only show active users
    ORDER BY u.DisplayName;
END

GO


