-- CREATE STORED PROCEDURES FOR UserRol TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. No paso una validación, 2. No existe el recurso, 3. Error en la base de datos)

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
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--INSERT MANY UserRole
IF OBJECT_ID('Security.uspInsertManyUserRole', 'P') IS NOT NULL
	DROP PROCEDURE [Security].uspInsertManyUserRole
GO
CREATE PROC [Security].uspInsertManyUserRole (
	@UserRoles [Security].UserRoleType READONLY
)
AS
BEGIN
	-- Verificar que el Id del Usuario no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @UserRoles AS ur WHERE ur.UserId IS NULL OR ur.UserId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Usuario es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que existen todos los Usuarios
	IF EXISTS (SELECT 1 FROM @UserRoles AS ur LEFT JOIN [Security].[User] AS db ON ur.UserId = db.UserId WHERE db.UserId IS NULL)
	BEGIN
		SELECT 2 AS IsSuccess, 'Uno o más Usuarios no existen en la base de datos' AS [Message];
		RETURN
	END
	-- Verificar que el Id del Rol no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @UserRoles AS ur WHERE ur.RoleId IS NULL OR ur.RoleId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Rol es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que existen todos los Roles
	IF EXISTS (SELECT 1 FROM @UserRoles AS ur LEFT JOIN [Security].[Role] AS db ON ur.RoleId = db.RoleId WHERE db.RoleId IS NULL)
	BEGIN
		SELECT 2 AS IsSuccess, 'Uno o más Roles no existen en la base de datos' AS [Message];
		RETURN
	END
	-- Verificar que no vayan relaciones de Usuario-Roles repetidas
	IF EXISTS (SELECT 1 FROM @UserRoles AS ur GROUP BY ur.UserId, ur.RoleId HAVING COUNT(*) > 1)
    BEGIN
        SELECT 1 AS IsSuccess, 'Una o más relaciones Usuario-Roles están repetidos en la entrada' AS [Message];
        RETURN;
    END
	-- Verificar que no existan las relaciones de Usuario-Roles en la base de datos
	IF EXISTS (SELECT 1 FROM @UserRoles AS ur INNER JOIN [Security].UserRole AS db ON ur.UserId = db.UserId AND ur.RoleId = db.RoleId)
    BEGIN
        SELECT 1 AS IsSuccess, 'Una o más relaciones Usuario-Roles ya existen en la base de datos' AS [Message];
        RETURN;
    END
	--
	BEGIN TRAN
	BEGIN TRY
		-- Realizar la inserción 
		INSERT INTO [Security].UserRole(UserId, RoleId) SELECT ur.UserId, ur.RoleId FROM @UserRoles AS ur
		--
		SELECT 0 AS IsSuccess, 'Roles de los usuarios registrados exitosamente' AS [Message];
		--
		IF @@ERROR = 0
			IF @@TRANCOUNT > 0
				COMMIT TRAN;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;
		--
        SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message];
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
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
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
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
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