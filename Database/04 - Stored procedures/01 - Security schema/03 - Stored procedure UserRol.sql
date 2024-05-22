-- CREATE STORED PROCEDURES FOR UserRol TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. Error en la bd o no paso una validación, 2. No existe el recurso)

--INSERT UserRol
IF OBJECT_ID('Security.uspInsertUserRol', 'P') IS NOT NULL
	DROP PROCEDURE [Security].uspInsertUserRol
GO
CREATE PROC [Security].uspInsertUserRol (
	@UserId INT,
	@RolId INT
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
	IF (@RolId IS NULL OR @RolId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del rol es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Security].[Rol] WHERE RolId = @RolId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un rol con el Id ingresado' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Security].UserRol WHERE UserId = @UserId AND RolId = @RolId)
	BEGIN
		SELECT 1 AS IsSuccess, 'El usuario ya tiene el rol asignado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Security].[UserRol] (UserId, RolId) VALUES (@UserId, @RolId)
		--
		SELECT 0 AS IsSuccess, 'Rol del usuario ingresado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--DELETE UserRol
IF OBJECT_ID('Security.uspDeleteUserRol', 'P') IS NOT NULL
	DROP PROCEDURE [Security].uspDeleteUserRol
GO
CREATE PROC [Security].uspDeleteUserRol (
	@UserId INT,
	@RolId INT
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
	IF (@RolId IS NULL OR @RolId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del rol es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Security].UserRol WHERE UserId = @UserId AND RolId = @RolId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un usuario con el rol ingresado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [Security].UserRol WHERE UserId = @UserId AND RolId = @RolId
		--
		SELECT 0 AS IsSuccess, 'Rol del usuario eliminado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--DELETE ALL UserRol
IF OBJECT_ID('Security.uspDeleteAllUserRol', 'P') IS NOT NULL
	DROP PROCEDURE [Security].uspDeleteAllUserRol
GO
CREATE PROC [Security].uspDeleteAllUserRol (
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
	IF NOT EXISTS (SELECT 1 FROM [Security].UserRol WHERE UserId = @UserId)
	BEGIN
		SELECT 2 AS IsSuccess, 'El usuario no tiene ningun rol asignado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [Security].UserRol WHERE UserId = @UserId
		--
		SELECT 0 AS IsSuccess, 'Roles del usuario eliminados exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET UserRol
IF OBJECT_ID('Security.uspGetUserRol', 'P') IS NOT NULL
	DROP PROCEDURE [Security].uspGetUserRol
GO
CREATE PROC [Security].uspGetUserRol (
	@UserId INT = NULL,
	@RolId INT = NULL
)
AS
BEGIN
	--
	IF (@UserId IS NULL OR @UserId = '' OR @RolId IS NULL OR @RolId = '')
	BEGIN
		SELECT UserId, RolId FROM [Security].UserRol
	END
	ELSE
	BEGIN
		SELECT UserId, RolId FROM [Security].UserRol WHERE UserId = @UserId AND RolId = @RolId
	END
END
GO