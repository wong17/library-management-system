-- CREATE STORED PROCEDURES FOR User TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. �xito, 1. Error en la bd o no paso una validaci�n, 2. No existe el recurso)

-- INSERT User
IF OBJECT_ID('Security.uspInsertUser', 'P') IS NOT NULL  
    DROP PROCEDURE [Security].uspInsertUser;  
GO
CREATE PROC [Security].uspInsertUser (
	@UserName VARCHAR(50),
	@Email VARCHAR(250),
	@Password VARBINARY(64)
)
AS
BEGIN
	--
	IF (@UserName IS NULL OR @UserName = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de usuario es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Email IS NULL OR @Email = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Correo electronico del usuario es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Password IS NULL OR @Password = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Contrase�a del usuario es obligatoria' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Security].[User] WHERE UserName = @UserName)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe un usuario con el mismo nombre en la base de datos' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Security].[User] WHERE Email = @Email)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe un usuario con el mismo correo electronico en la base de datos' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Security].[User] (UserName, Email, [Password]) VALUES (@UserName, @Email, HASHBYTES('SHA2_512', @Password))
		--
		SELECT 0 AS IsSuccess, 'Usuario registrado exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- UPDATE User
IF OBJECT_ID('Security.uspUpdateUser', 'P') IS NOT NULL  
    DROP PROCEDURE [Security].uspUpdateUser;  
GO
CREATE PROC [Security].uspUpdateUser (
	@UserId INT,
	@Email VARCHAR(250),
	@Password VARBINARY(64)
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
	IF (@Email IS NULL OR @Email = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Correo electronico del usuario es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Password IS NULL OR @Password = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Contrase�a del usuario es obligatoria' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Security].[User] WHERE Email = @Email)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe un usuario con el mismo correo electronico en la base de datos' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [Security].[User] SET Email = @Email, [Password] = HASHBYTES('SHA2_512', @Password) WHERE UserId = @UserId
		--
		SELECT 0 AS IsSuccess, 'Usuario actualizado exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- DELETE User
IF OBJECT_ID('Security.uspDeleteUser', 'P') IS NOT NULL  
    DROP PROCEDURE [Security].uspDeleteUser;  
GO
CREATE PROC [Security].uspDeleteUser (
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
	BEGIN TRY
		-- ELIMINAR TODOS LOS ROLES
		DECLARE @SPResult TABLE (
			IsSuccess INT,
			[Message] VARCHAR(255)
		)
		INSERT INTO @SPResult EXEC [Security].uspDeleteAllUserRole @UserId
		IF ((SELECT IsSuccess FROM @SPResult) != 0)
		BEGIN
			--
			SELECT IsSuccess, [Message] FROM @SPResult
			--
			IF @@TRANCOUNT > 0
				ROLLBACK TRAN;
		END
		-- ELIMINAR AL USUARIO
		DELETE FROM [Security].[User] WHERE UserId = @UserId
		--
		SELECT 0 AS IsSuccess, 'Usuario eliminado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- GET User
IF OBJECT_ID('Security.uspGetUser', 'P') IS NOT NULL  
    DROP PROCEDURE [Security].uspGetUser;  
GO
CREATE PROC [Security].uspGetUser (
	@UserId INT = NULL
)
AS
BEGIN
	--
	IF (@UserId IS NULL OR @UserId = '')
	BEGIN
		SELECT UserId, UserName, Email, [Password], AccessToken, RefreshToken, RefreshTokenExpiryTime, LockoutEnabled, AccessFailedCount 
		FROM [Security].[User]
	END
	ELSE
	BEGIN
		SELECT UserId, UserName, Email, [Password], AccessToken, RefreshToken, RefreshTokenExpiryTime, LockoutEnabled, AccessFailedCount 
		FROM [Security].[User]
		WHERE UserId = @UserId
	END
END
GO