-- CREATE STORED PROCEDURES FOR UserRol TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. Error en la bd o no paso una validación, 2. No existe el recurso)

--INSERT UserRole
IF OBJECT_ID('Security.uspInsertUserRole', 'P') IS NOT NULL
	DROP PROCEDURE [Security].uspInsertUserRole
GO
CREATE PROC [Security].uspInsertUserRole (
	@UserId INT,
	@RoleId INT
)
AS
BEGIN
	--
	IF (@UserId IS NULL OR @UserId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del usuario es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Security].[User] WHERE UserId = @UserId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un usuario con el Id ingresado' AS [Message]
		RETURN
	END
	--
	IF (@RoleId IS NULL OR @RoleId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del rol es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Security].[Role] WHERE RoleId = @RoleId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un rol con el Id ingresado' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Security].UserRole WHERE UserId = @UserId AND RoleId = @RoleId)
	BEGIN
		SELECT 1 AS IsSuccess, 'El usuario ya tiene el rol asignado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Security].[UserRole] (UserId, RoleId) VALUES (@UserId, @RoleId)
		--
		SELECT 0 AS IsSuccess, 'Rol del usuario ingresado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--DELETE UserRole
IF OBJECT_ID('Security.uspDeleteUserRole', 'P') IS NOT NULL
	DROP PROCEDURE [Security].uspDeleteUserRole
GO
CREATE PROC [Security].uspDeleteUserRole (
	@UserId INT,
	@RoleId INT
)
AS
BEGIN
	--
	IF (@UserId IS NULL OR @UserId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del usuario es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@RoleId IS NULL OR @RoleId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del rol es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Security].UserRole WHERE UserId = @UserId AND RoleId = @RoleId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un usuario con el rol ingresado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [Security].UserRole WHERE UserId = @UserId AND RoleId = @RoleId
		--
		SELECT 0 AS IsSuccess, 'Rol del usuario eliminado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--DELETE ALL UserRole
IF OBJECT_ID('Security.uspDeleteAllUserRole', 'P') IS NOT NULL
	DROP PROCEDURE [Security].uspDeleteAllUserRole
GO
CREATE PROC [Security].uspDeleteAllUserRole (
	@UserId INT
)
AS
BEGIN
	--
	IF (@UserId IS NULL OR @UserId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del usuario es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Security].[User] WHERE UserId = @UserId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un usuario con el Id ingresado' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Security].UserRole WHERE UserId = @UserId)
	BEGIN
		SELECT 2 AS IsSuccess, 'El usuario no tiene ningun rol asignado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [Security].UserRole WHERE UserId = @UserId
		--
		SELECT 0 AS IsSuccess, 'Roles del usuario eliminados exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET UserRole
IF OBJECT_ID('Security.uspGetUserRole', 'P') IS NOT NULL
	DROP PROCEDURE [Security].uspGetUserRole
GO
CREATE PROC [Security].uspGetUserRole (
	@UserId INT = NULL,
	@RoleId INT = NULL
)
AS
BEGIN
	--
	IF (@UserId IS NULL OR @UserId = '' OR @RoleId IS NULL OR @RoleId = '')
	BEGIN
		SELECT UserId, RoleId FROM [Security].UserRole
	END
	ELSE
	BEGIN
		SELECT UserId, RoleId FROM [Security].UserRole WHERE UserId = @UserId AND RoleId = @RoleId
	END
END
GO