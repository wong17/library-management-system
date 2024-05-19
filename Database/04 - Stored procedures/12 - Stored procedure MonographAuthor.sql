-- CREATE STORED PROCEDURES FOR MonographAuthor TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. Error en la bd o no paso una validación, 2. No existe el recurso)

--INSERT MonographAuthor
IF OBJECT_ID('Library.uspInsertMonographAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertMonographAuthor;  
GO
CREATE PROC [Library].uspInsertMonographAuthor (
	@MonographId INT,
	@AuthorId INT
)
AS
BEGIN
	--
	IF (@MonographId IS NULL OR @MonographId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la monografía es obligatoria' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Monograph WHERE MonographId = @MonographId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una monografía con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF (@AuthorId IS NULL OR @AuthorId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Autor es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Author WHERE AuthorId = @AuthorId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un Autor con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF EXISTS (SELECT 1 FROM [Library].MonographAuthor WHERE MonographId = @MonographId AND AuthorId = @AuthorId)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe la relación entre la monografía y la autor proporcionado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Library].MonographAuthor (MonographId, AuthorId) VALUES (@MonographId, @AuthorId)
		--
		SELECT 0 AS IsSuccess, 'Autor de la monografía registrado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--DELETE MonographAuthor
IF OBJECT_ID('Library.uspDeleteMonographAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeleteMonographAuthor;  
GO
CREATE PROC [Library].uspDeleteMonographAuthor (
	@MonographId INT,
	@AuthorId INT
)
AS
BEGIN
	--
	IF (@MonographId IS NULL OR @MonographId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la monografía es obligatoria' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Monograph WHERE MonographId = @MonographId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una monografía con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF (@AuthorId IS NULL OR @AuthorId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Autor es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Author WHERE AuthorId = @AuthorId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un Autor con el Id proporcionado' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].MonographAuthor WHERE MonographId = @MonographId AND AuthorId = @AuthorId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No existe la relación entre la monografía y la autor proporcionado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [Library].MonographAuthor WHERE MonographId = @MonographId AND AuthorId = @AuthorId
		--
		SELECT 0 AS IsSuccess, 'Autor de la monografía eliminado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET MonographAuthor
IF OBJECT_ID('Library.uspGetMonographAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetMonographAuthor;  
GO
CREATE PROC [Library].uspGetMonographAuthor (
	@MonographId INT = NULL,
	@AuthorId INT = NULL
)
AS
BEGIN
	--
	IF (@MonographId IS NULL OR @MonographId = '' OR @AuthorId IS NULL OR @AuthorId = '')
	BEGIN
		SELECT MonographId, AuthorId, CreatedOn, ModifiedOn FROM [Library].MonographAuthor
	END
	ELSE
	BEGIN
		SELECT MonographId, AuthorId, CreatedOn, ModifiedOn FROM [Library].MonographAuthor WHERE MonographId = @MonographId AND AuthorId = @AuthorId
	END
END
GO