-- CREATE STORED PROCEDURES FOR BookAuthor TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. No paso una validación, 2. No existe el recurso, 3. Error en la base de datos)

--INSERT BookAuthor
IF OBJECT_ID('Library.uspInsertBookAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertBookAuthor;  
GO
CREATE PROC [Library].uspInsertBookAuthor (
	@BookId INT,
	@AuthorId INT
)
AS
BEGIN
	--
	IF (@BookId IS NULL OR @BookId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Libro es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Book WHERE BookId = @BookId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un Libro con el Id proporcionado' AS [Message]
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
	IF EXISTS (SELECT 1 FROM [Library].BookAuthor WHERE BookId = @BookId AND AuthorId = @AuthorId)
	BEGIN
		SELECT 1 AS IsSuccess, 'Ya existe la relación entre el libro y la autor proporcionado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		INSERT INTO [Library].BookAuthor (BookId, AuthorId) VALUES (@BookId, @AuthorId)
		--
		SELECT 0 AS IsSuccess, 'Autor del libro registrado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--INSERT MANY BookAuthor
IF OBJECT_ID('Library.uspInsertManyBookAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertManyBookAuthor;  
GO
CREATE PROC [Library].uspInsertManyBookAuthor (
	@BookAuthors [Library].BookAuthorType READONLY
)
AS
BEGIN
	-- Verificar que el Id del Libro no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @BookAuthors AS ba WHERE ba.BookId IS NULL OR ba.BookId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Libro es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que existen todos los Libros
	IF EXISTS (SELECT 1 FROM @BookAuthors AS ba LEFT JOIN [Library].Book AS db ON ba.BookId = db.BookId WHERE db.BookId IS NULL)
	BEGIN
		SELECT 2 AS IsSuccess, 'Uno o más Libros no existen en la base de datos' AS [Message];
		RETURN
	END
	-- Verificar que el Id del Autor no sea NULL ni ''
	IF EXISTS (SELECT 1 FROM @BookAuthors AS ba WHERE ba.AuthorId IS NULL OR ba.AuthorId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Author es obligatorio' AS [Message]
		RETURN
	END
	-- Verificar que existen todos los Autores
	IF EXISTS (SELECT 1 FROM @BookAuthors AS ba LEFT JOIN [Library].Author AS db ON ba.AuthorId = db.AuthorId WHERE db.AuthorId IS NULL)
	BEGIN
		SELECT 2 AS IsSuccess, 'Uno o más Autores no existen en la base de datos' AS [Message];
		RETURN
	END
	-- Verificar que no vayan relaciones de Libros-Autores repetidas
	IF EXISTS (SELECT 1 FROM @BookAuthors AS ba GROUP BY ba.BookId, ba.AuthorId HAVING COUNT(*) > 1)
    BEGIN
        SELECT 1 AS IsSuccess, 'Una o más relaciones Libro-Autores están repetidos en la entrada' AS [Message];
        RETURN;
    END
	-- Verificar que no existan las relaciones de Libros-Autores en la base de datos
	IF EXISTS (SELECT 1 FROM @BookAuthors AS ba INNER JOIN [Library].BookAuthor AS db ON ba.BookId = db.BookId AND ba.AuthorId = db.AuthorId)
    BEGIN
        SELECT 1 AS IsSuccess, 'Una o más relaciones Libro-Autores ya existen en la base de datos' AS [Message];
        RETURN;
    END
	--
	BEGIN TRAN
	BEGIN TRY
		-- Realizar la inserción 
		INSERT INTO [Library].BookAuthor(BookId, AuthorId) SELECT ba.BookId, ba.AuthorId FROM @BookAuthors AS ba
		--
		SELECT 0 AS IsSuccess, 'Autores del libro registrados exitosamente' AS [Message];
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

--DELETE BookAuthor
IF OBJECT_ID('Library.uspDeleteBookAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeleteBookAuthor;  
GO
CREATE PROC [Library].uspDeleteBookAuthor (
	@BookId INT,
	@AuthorId INT
)
AS
BEGIN
	--
	IF (@BookId IS NULL OR @BookId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Libro es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].Book WHERE BookId = @BookId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un Libro con el Id proporcionado' AS [Message]
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
	IF NOT EXISTS (SELECT 1 FROM [Library].BookAuthor WHERE BookId = @BookId AND AuthorId = @AuthorId)
	BEGIN
		SELECT 1 AS IsSuccess, 'No existe la relación entre el libro y la autor proporcionado' AS [Message]
		RETURN
	END
	--
	BEGIN TRY
		--
		DELETE FROM [Library].BookAuthor WHERE BookId = @BookId AND AuthorId = @AuthorId
		--
		SELECT 0 AS IsSuccess, 'Autor del libro eliminado exitosamente' AS [Message]
	END TRY
	BEGIN CATCH
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET BookAuthor
IF OBJECT_ID('Library.uspGetBookAuthor', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetBookAuthor;  
GO
CREATE PROC [Library].uspGetBookAuthor (
	@BookId INT = NULL,
	@AuthorId INT = NULL
)
AS
BEGIN
	--
	IF (@BookId IS NULL OR @BookId = '' OR @AuthorId IS NULL OR @AuthorId = '')
	BEGIN
		SELECT BookId, AuthorId, CreatedOn, ModifiedOn FROM [Library].BookAuthor
	END
	ELSE
	BEGIN
		SELECT BookId, AuthorId, CreatedOn, ModifiedOn FROM [Library].BookAuthor WHERE BookId = @BookId AND AuthorId = @AuthorId
	END
END
GO