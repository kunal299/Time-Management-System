USE [UCITMSDev]
GO

/****** Object:  StoredProcedure [dbo].[GetUserMenuById]    Script Date: 21-10-2024 10:22:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUserMenuById]
    @UserId INT
AS
BEGIN
    -- Select statement to get menu information based on user roles
    SELECT 
        m.ID, 
        m.MenuName, 
        m.ImagePath, 
        m.NavigationPath, 
        m.NavigationType, 
        m.SortOrder, 
        m.IsActive, 
        m.CreatedBy, 
        m.ModifiedBy
    FROM Menu m
    INNER JOIN MenuRoleMapping mr ON m.ID = mr.MenuId
    INNER JOIN UserRolesMapping ur ON mr.RoleId = ur.RoleId
    WHERE ur.UserId = @UserId 
    AND m.IsActive = 1
    ORDER BY m.SortOrder;
END

GO


