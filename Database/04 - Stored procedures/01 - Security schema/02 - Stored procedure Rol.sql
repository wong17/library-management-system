-- CREATE STORED PROCEDURES FOR Rol TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. Error en la bd o no paso una validación, 2. No existe el recurso)

--INSERT Rol
IF OBJECT_ID('Security.uspInsertRol', 'P') IS NOT NULL  
    DROP PROCEDURE [Security].uspInsertRol;  
GO
CREATE PROC [Security].uspInsertRol (
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
	IF EXISTS (SELECT 1 FROM [Security].[Rol] WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe un rol con el mismo nombre en la base de datos' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Security].Rol ([Name], [Description]) VALUES (@Name, @Description)
		--
		SELECT 0 AS IsSuccess, 'Rol registrado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--UPDATE ROL
IF OBJECT_ID('Security.uspUpdateRol', 'P') IS NOT NULL  
    DROP PROCEDURE [Security].uspUpdateRol;  
GO
CREATE PROC [Security].uspUpdateRol (
	@RolId INT,
	@Name VARCHAR(100),
	@Description VARCHAR(500)
)
AS
BEGIN
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
	IF EXISTS (SELECT 1 FROM [Security].[Rol] WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe un rol con el mismo nombre en la base de datos' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [Security].Rol SET [Name] = @Name, [Description] = @Description WHERE RolId = @RolId
		--
		SELECT 0 AS IsSuccess, 'Rol actualizado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--DELETE Rol
IF OBJECT_ID('Security.uspDeleteRol', 'P') IS NOT NULL  
    DROP PROCEDURE [Security].uspDeleteRol;  
GO
CREATE PROC [Security].uspDeleteRol (
	@RolId INT
)
AS
BEGIN
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
	BEGIN TRY
		--
		DELETE FROM [Security].Rol WHERE RolId = @RolId
		--
		SELECT 0 AS IsSuccess, 'Rol eliminado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET Rol
IF OBJECT_ID('Security.uspGetRol', 'P') IS NOT NULL  
    DROP PROCEDURE [Security].uspGetRol;  
GO
CREATE PROC [Security].uspGetRol (
	@RolId INT = NULL
)
AS
BEGIN
	--
	IF (@RolId IS NULL OR @RolId = '')
	BEGIN
		SELECT RolId, [Name], [Description] FROM Security.Rol
	END
	ELSE
	BEGIN
		SELECT RolId, [Name], [Description] FROM Security.Rol WHERE RolId = @RolId
	END
END
GO