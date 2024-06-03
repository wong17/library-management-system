-- CREATE STORED PROCEDURES FOR Rol TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. No paso una validación, 2. No existe el recurso, 3. Error en la base de datos)

--INSERT Role
IF OBJECT_ID('Security.uspInsertRole', 'P') IS NOT NULL  
    DROP PROCEDURE [Security].uspInsertRole;  
GO
CREATE PROC [Security].uspInsertRole (
	@Name VARCHAR(100),
	@Description VARCHAR(500)
)
AS
BEGIN
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre del rol es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Description IS NULL OR @Description = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Descripción del rol es obligatorio' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Security].[Role] WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe un rol con el mismo nombre en la base de datos' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Security].[Role] ([Name], [Description]) VALUES (@Name, @Description)
		--
		SELECT 0 AS IsSuccess, 'Rol registrado exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		--
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--UPDATE Role
IF OBJECT_ID('Security.uspUpdateRole', 'P') IS NOT NULL  
    DROP PROCEDURE [Security].uspUpdateRole;  
GO
CREATE PROC [Security].uspUpdateRole (
	@RoleId INT,
	@Name VARCHAR(100),
	@Description VARCHAR(500)
)
AS
BEGIN
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
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre del rol es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Description IS NULL OR @Description = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Descripción del rol es obligatorio' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Security].[Role] WHERE [Name] = @Name AND RoleId != @RoleId)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe un rol con el mismo nombre en la base de datos' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [Security].[Role] SET [Name] = @Name, [Description] = @Description WHERE RoleId = @RoleId
		--
		SELECT 0 AS IsSuccess, 'Rol actualizado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--DELETE Role
IF OBJECT_ID('Security.uspDeleteRole', 'P') IS NOT NULL  
    DROP PROCEDURE [Security].uspDeleteRole;  
GO
CREATE PROC [Security].uspDeleteRole (
	@RoleId INT
)
AS
BEGIN
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
	BEGIN TRY
		--
		DELETE FROM [Security].[Role] WHERE RoleId = @RoleId
		--
		SELECT 0 AS IsSuccess, 'Rol eliminado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET Role
IF OBJECT_ID('Security.uspGetRole', 'P') IS NOT NULL  
    DROP PROCEDURE [Security].uspGetRole;  
GO
CREATE PROC [Security].uspGetRole (
	@RoleId INT = NULL
)
AS
BEGIN
	--
	IF (@RoleId IS NULL OR @RoleId = '')
	BEGIN
		SELECT RoleId, [Name], [Description] FROM [Security].[Role]
	END
	ELSE
	BEGIN
		SELECT RoleId, [Name], [Description] FROM [Security].[Role] WHERE RoleId = @RoleId
	END
END
GO