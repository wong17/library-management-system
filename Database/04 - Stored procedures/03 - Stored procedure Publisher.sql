-- CREATE STORED PROCEDURES FOR Publisher TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. Error en la bd o no paso una validación, 2. No existe el recurso)

-- INSERT Publisher
IF OBJECT_ID('Library.uspInsertPublisher', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertPublisher;  
GO
CREATE PROC [Library].uspInsertPublisher (
	@Name VARCHAR(100)
)
AS
BEGIN
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Editorial es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z-. ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Editorial solo puede tener mayúsculas, minúsculas, guiones, puntos y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Publisher WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una Editorial con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Library].Publisher ([Name]) VALUES (@Name)
		--
		SELECT 0 AS IsSuccess, 'Editorial registrada exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- UPDATE Publisher
IF OBJECT_ID('Library.uspUpdatePublisher', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdatePublisher;  
GO
CREATE PROC [Library].uspUpdatePublisher (
	@PublisherId INT,
	@Name VARCHAR(100)
)
AS
BEGIN
	--
	IF (@PublisherId IS NULL OR @PublisherId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Editorial es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Publisher WHERE PublisherId = @PublisherId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Editorial con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF (@Name IS NULL OR @Name = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Editorial es obligatorio' AS [Message]
		RETURN
	END
	--
	IF (@Name LIKE '%[^a-zA-Z-. ]%')
	BEGIN
		SELECT 1 AS IsSuccess, 'Nombre de la Editorial solo puede tener mayúsculas, minúsculas, guiones, puntos y espacios' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Publisher WHERE [Name] = @Name)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe una Editorial con el mismo nombre' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		UPDATE [Library].Publisher SET [Name] = @Name WHERE PublisherId = @PublisherId
		--
		SELECT 0 AS IsSuccess, 'Editorial actualizada exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- DELETE Publisher
IF OBJECT_ID('Library.uspDeletePublisher', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeletePublisher;  
GO
CREATE PROC [Library].uspDeletePublisher (
	@PublisherId INT
)
AS
BEGIN
	--
	IF (@PublisherId IS NULL OR @PublisherId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Editorial es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Publisher WHERE PublisherId = @PublisherId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una Editorial con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].Book WHERE PublisherId = @PublisherId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede eliminar la Editorial porque hay libros asociados a ella' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [Library].Publisher WHERE PublisherId = @PublisherId
		--
		SELECT 0 AS IsSuccess, 'Editorial eliminada exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

-- GET Publisher
IF OBJECT_ID('Library.uspGetPublisher', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetPublisher;  
GO
CREATE PROC [Library].uspGetPublisher (
	@PublisherId INT = NULL
)
AS
BEGIN
	IF (@PublisherId IS NULL OR @PublisherId = '')
	BEGIN
		SELECT PublisherId, [Name] FROM [Library].Publisher
	END
	ELSE
	BEGIN
		SELECT PublisherId, [Name] FROM [Library].Publisher WHERE PublisherId = @PublisherId
	END
END
GO
