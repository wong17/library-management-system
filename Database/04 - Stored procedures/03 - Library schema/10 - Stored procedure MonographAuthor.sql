-- CREATE STORED PROCEDURES FOR MonographAuthor TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. No paso una validación, 2. No existe el recurso, 3. Error en la base de datos)

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
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--INSERT MANY MonographAuthor
IF OBJECT_ID('Library.uspInsertManyMonographAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertManyMonographAuthor;  
GO
CREATE PROC [Library].uspInsertManyMonographAuthor (
	@MonographAuthors [Library].MonographAuthorType READONLY
)
AS
BEGIN
	-- Verificar que el Id de la Monografia no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @MonographAuthors AS ma WHERE ma.MonographId IS NULL OR ma.MonographId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la Monografia es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que existen todas las Monografias
	IF EXISTS (SELECT 1 FROM @MonographAuthors AS ma LEFT JOIN [Library].Monograph AS db ON ma.MonographId = db.MonographId WHERE db.MonographId IS NULL)
	BEGIN
		SELECT 2 AS IsSuccess, 'Una o más Monografias no existen en la base de datos' AS [Message];
		RETURN
	END
	-- Verificar que el Id del Autor no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @MonographAuthors AS ma WHERE ma.AuthorId IS NULL OR ma.AuthorId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Author es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que existen todos los Autores
	IF EXISTS (SELECT 1 FROM @MonographAuthors AS ma LEFT JOIN [Library].Author AS db ON ma.AuthorId = db.AuthorId WHERE db.AuthorId IS NULL)
	BEGIN
		SELECT 2 AS IsSuccess, 'Uno o más Autores no existen en la base de datos' AS [Message];
		RETURN
	END
	-- Verificar que no vayan relaciones de Monografia-Autores repetidas
	IF EXISTS (SELECT 1 FROM @MonographAuthors AS ma GROUP BY ma.MonographId, ma.AuthorId HAVING COUNT(*) > 1)
    BEGIN
        SELECT 1 AS IsSuccess, 'Una o más relaciones Monografia-Autores están repetidos en la entrada' AS [Message];
        RETURN;
    END
	-- Verificar que no existan las relaciones de Libros-Autores en la base de datos
	IF EXISTS (SELECT 1 FROM @MonographAuthors AS ma INNER JOIN [Library].MonographAuthor AS db ON ma.MonographId = db.MonographId AND ma.AuthorId = db.AuthorId)
    BEGIN
        SELECT 1 AS IsSuccess, 'Una o más relaciones Monografia-Autores ya existen en la base de datos' AS [Message];
        RETURN;
    END
	--
	BEGIN TRAN
	BEGIN TRY
		-- Realizar la inserción 
		INSERT INTO [Library].MonographAuthor(MonographId, AuthorId) SELECT ma.MonographId, ma.AuthorId FROM @MonographAuthors AS ma
		--
		SELECT 0 AS IsSuccess, 'Autor de la monografia registrado exitosamente' AS [Message];
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
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
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