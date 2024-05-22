-- CREATE STORED PROCEDURES FOR BookLoan TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. Error en la bd o no paso una validación, 2. No existe el recurso)

--INSERT BookLoan
IF OBJECT_ID('Library.uspInsertBookLoan', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertBookLoan;  
GO
CREATE PROC [Library].uspInsertBookLoan (
	@StudentId INT,
	@BookId INT,
	@TypeOfLoan VARCHAR(10)
)
AS
BEGIN
	-- VERIFICAR ID DEL ESTUDIANTE
	IF (@StudentId IS NULL OR @StudentId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del Estudiante es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [University].Student WHERE StudentId = @StudentId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un Estudiante con el Id proporcionado' AS [Message]
		RETURN
	END
	-- VERIFICAR EL TIPO DE PRESTAMO
	IF (@TypeOfLoan != 'SALA' AND @TypeOfLoan != 'DOMICILIO')
	BEGIN
		SELECT 1 AS IsSuccess, 'El tipo de préstamo no es válido' AS [Message]
		RETURN
	END
	-- VERIFICAR SI EL ESTUDIANTE GENERA UNA SOLICITUD DUPLICADA PARA EL MISMO LIBRO
	IF ([Library].ufnCheckDuplicateBookRequest(@StudentId, @BookId) = 1)
	BEGIN
		SELECT 1 AS IsSuccess, 'Aún tiene una solicitud de préstamo pendiente de este libro' AS [Message]
		RETURN
	END
	-- VERIFICAR SI EL ESTUDIANTE HA GENERADO UN MAXIMO DE 3 SOLICITUDES Y QUEDEN PENDIENTES POR APROBAR ('CREADA')
	IF ([Library].ufnHasStudentReachedMaxBookRequest(@StudentId) = 1)
	BEGIN
		SELECT 1 AS IsSuccess, 'Se ha alcanzado el máximo de solicitudes' AS [Message]
		RETURN
	END
	-- VERIFICAR SI EL ESTUDIANTE YA PRESTO EL LIBRO Y LO QUIERE VOLVER A SOLICITAR
	IF ([Library].ufnHasStudentBorrowedBook(@StudentId, @BookId) = 1)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede tener 2 copias del mismo libro' AS [Message]
		RETURN
	END
	-- VERIFICAR SI EL ESTUDIANTE AUN PUEDE HACER OTRO PRESTAMO
	IF ([Library].ufnCanStudentBorrowBook(@StudentId) = 0)
	BEGIN
		SELECT 1 AS IsSuccess, 'No se puede crear la solicitud, cantidad máxima de libros préstados alcanzada' AS [Message]
		RETURN
	END
	-- VERIFICAR SI AUN QUEDAN COPIAS DISPONIBLES DEL LIBRO PARA PRESTAMO
	IF ([Library].ufnCheckBookAvailability(@BookId) = 0)
	BEGIN
		SELECT 1 AS IsSuccess, 'El libro no esta disponible, no quedan más unidades para préstamo' AS [Message]
		RETURN
	END
	--
	BEGIN TRAN
	BEGIN TRY
		-- AUMENTAR EN 1 LA CANTIDAD DE COPIAS PRESTADAS DEL LIBRO
		DECLARE @SPResult TABLE (
			IsSuccess INT,
			[Message] VARCHAR(255)
		)
		INSERT INTO @SPResult EXEC [Library].uspUpdateBorrowedBooks @BookId, 1
		-- VERIFICAR SI OCURRIO UN ERROR AL ACTUALIZAR LA CANTIDAD DE COPIAS PRESTADAS DEL LIBRO
		IF ((SELECT IsSuccess FROM @SPResult) != 0)
		BEGIN
			--
			SELECT IsSuccess, [Message] FROM @SPResult
			--
			IF @@TRANCOUNT > 0
				ROLLBACK TRAN;
		END
		-- CREAR NUEVA SOLICITUD DE PRESTAMO
		INSERT INTO [Library].BookLoan (StudentId, BookId, TypeOfLoan) VALUES (@StudentId, @BookId, @TypeOfLoan)
		-- RETORNAR MENSAJE DE EXITO SI TODO SALIO BIEN
		SELECT 0 AS IsSuccess, 'Solicitud de préstamo creada exitosamente' AS [Message], SCOPE_IDENTITY() AS Result
		--
		IF @@ERROR = 0
			IF @@TRANCOUNT > 0
				COMMIT TRAN;
	END TRY
	BEGIN CATCH
		--
		IF @@TRANCOUNT > 0
			ROLLBACK TRAN;
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--UPDATE BorrowedBookLoan
IF OBJECT_ID('Library.uspUpdateBorrowedBookLoan', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateBorrowedBookLoan;  
GO
CREATE PROC [Library].uspUpdateBorrowedBookLoan (
	@BookLoanId INT,
	@DueDate DATETIME
)
AS
BEGIN
	-- VERIFICAR ID SOLICITUD DE PRESTAMO
	IF (@BookLoanId IS NULL OR @BookLoanId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la solicitud de préstamo es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].BookLoan WHERE BookLoanId = @BookLoanId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una solicitud de préstamo con el Id proporcionado' AS [Message]
		RETURN
	END
	-- VERIFICAR FECHA DE VENCIMIENTO DEL PRESTAMO
	IF (@DueDate IS NULL)
	BEGIN
		SELECT 1 AS IsSuccess, 'Fecha de vencimiento del préstamo es obligatoria' AS [Message]
		RETURN
	END
	-- VERIFICAR QUE LA SOLICITUD A APROBAR TENGA POR ESTADO 'CREADA'
	IF ([Library].ufnCheckBookLoanState(@BookLoanId, 'CREADA') = 0)
	BEGIN
		SELECT 1 AS IsSuccess, 'Solo se pueden aprobar solicitudes con estado CREADA' AS [Message]
		RETURN
	END
	--
	BEGIN TRAN
	BEGIN TRY
		-- OBTENER ID DEL ESTUDIANTE AL QUE SE LE APROBARA EL PRESTAMO
		DECLARE @StudentId TINYINT = (SELECT StudentId FROM [Library].BookLoan WHERE BookLoanId = @BookLoanId)
		-- AUMENTAR EN 1 LA CANTIDAD DE LIBROS PRESTADOS POR EL ESTUDIANTE
		DECLARE @SPResult TABLE (
			IsSuccess INT,
			[Message] VARCHAR(255)
		)
		INSERT INTO @SPResult EXEC [University].uspUpdateBorrowedBooksStudent @StudentId, 1
		-- VERIFICAR SI OCURRIO UN ERROR AL ACTUALIZAR LA CANTIDAD DE LIBROS PRESTADOS POR EL ESTUDIANTE
		IF ((SELECT IsSuccess FROM @SPResult) != 0)
		BEGIN
			--
			SELECT IsSuccess, [Message] FROM @SPResult
			--
			IF @@TRANCOUNT > 0
				ROLLBACK TRAN;
		END
		-- ACTUALIZAR ESTADO DE LA SOLICITUD A 'PRESTADO'
		UPDATE [Library].BookLoan SET [State] = 'PRESTADO', DueDate = @DueDate WHERE BookLoanId = @BookLoanId
		-- RETORNAR MENSAJE DE EXITO SI TODO SALIO BIEN
		SELECT 0 AS IsSuccess, 'Solicitud de préstamo aprobada exitosamente' AS [Message]
		--
		IF @@ERROR = 0
			IF @@TRANCOUNT > 0
				COMMIT TRAN;
	END TRY
	BEGIN CATCH
		--
		IF @@TRANCOUNT > 0
			ROLLBACK TRAN;
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--UPDATE ReturnedBookLoan
IF OBJECT_ID('Library.uspUpdateReturnedBookLoan', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateReturnedBookLoan;  
GO
CREATE PROC [Library].uspUpdateReturnedBookLoan (
	@BookLoanId INT
)
AS
BEGIN
	-- VERIFICAR ID SOLICITUD DE PRESTAMO
	IF (@BookLoanId IS NULL OR @BookLoanId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la solicitud de préstamo es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].BookLoan WHERE BookLoanId = @BookLoanId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una solicitud de préstamo con el Id proporcionado' AS [Message]
		RETURN
	END
	-- VERIFICAR QUE LA SOLICITUD PARA HACER LA DEVOLUCION TENGA POR ESTADO 'PRESTADA'
	IF ([Library].ufnCheckBookLoanState(@BookLoanId, 'PRESTADO') = 0)
	BEGIN
		SELECT 1 AS IsSuccess, 'Solo se puede hacer devoluciones de solicitudes con estado PRESTADO' AS [Message]
		RETURN
	END
	--
	BEGIN TRAN
	BEGIN TRY
		-- OBTENER ID DEL ESTUDIANTE AL QUE SE LE PRESTO EL LIBRO
		DECLARE @StudentId INT = (SELECT StudentId FROM [Library].BookLoan WHERE BookLoanId = @BookLoanId)
		-- RESTAR EN 1 LA CANTIDAD DE LIBROS PRESTADOS POR EL ESTUDIANTE
		DECLARE @SPResult TABLE (
			IsSuccess INT,
			[Message] VARCHAR(255)
		)
		INSERT INTO @SPResult EXEC [University].uspUpdateBorrowedBooksStudent @StudentId, -1
		-- VERIFICAR SI OCURRIO UN ERROR AL ACTUALIZAR LA CANTIDAD DE LIBROS PRESTADOS POR EL ESTUDIANTE
		IF ((SELECT IsSuccess FROM @SPResult) != 0)
		BEGIN
			--
			SELECT 1 AS IsSuccess, (SELECT [Message] FROM @SPResult) AS [MESSAGE]
			--
			IF @@TRANCOUNT > 0
				ROLLBACK TRAN;
		END
		-- LIMPIAR RESULTADOS ANTERIORES
		DELETE FROM @SPResult 
		-- RESTAR EN 1 LA CANTIDAD DE COPIAS PRESTADAS DEL LIBRO
		DECLARE @BookId INT = (SELECT BookId FROM [Library].BookLoan WHERE BookLoanId = @BookLoanId)
		INSERT INTO @SPResult EXEC [Library].uspUpdateBorrowedBooks @BookId, -1
		-- VERIFICAR SI OCURRIO UN ERROR AL ACTUALIZAR LA CANTIDAD DE COPIAS PRESTADAS DEL LIBRO
		IF ((SELECT IsSuccess FROM @SPResult) != 0)
		BEGIN
			--
			SELECT IsSuccess, [Message] FROM @SPResult
			--
			IF @@TRANCOUNT > 0
				ROLLBACK TRAN;
		END
		-- ACTUALIZAR ESTADO DE LA SOLICITUD A 'DEVUELTO'
		UPDATE [Library].BookLoan SET [State] = 'DEVUELTO', ReturnDate = GETDATE() WHERE BookLoanId = @BookLoanId
		-- RETORNAR MENSAJE DE EXITO SI TODO SALIO BIEN
		SELECT 0 AS IsSuccess, 'Devolución del libro realizada exitosamente' AS [Message]
		--
		IF @@ERROR = 0
			IF @@TRANCOUNT > 0
				COMMIT TRAN;
	END TRY
	BEGIN CATCH
		--
		IF @@TRANCOUNT > 0
			ROLLBACK TRAN;
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--DELETE BookLoan
IF OBJECT_ID('Library.uspDeleteBookLoan', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeleteBookLoan;  
GO
CREATE PROC [Library].uspDeleteBookLoan (
	@BookLoanId INT
)
AS
BEGIN
	-- VERIFICAR ID SOLICITUD DE PRESTAMO
	IF (@BookLoanId IS NULL OR @BookLoanId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la solicitud de préstamo es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].BookLoan WHERE BookLoanId = @BookLoanId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una solicitud de préstamo con el Id proporcionado' AS [Message]
		RETURN
	END
	-- VERIFICAR QUE LA SOLICITUD A ELIMINAR NO TENGA POR ESTADO 'PRESTADA' O 'DEVUELTA'
	DECLARE @BookLoanState CHAR(9) = (SELECT [State] FROM [Library].BookLoan WHERE BookLoanId = @BookLoanId)
	IF (@BookLoanState != 'CREADA')
	BEGIN
		SELECT 1 AS IsSuccess, 'No se pueden eliminar las solicitudes pendientes ni ya devueltas' AS [Message]
		RETURN
	END
	--
	BEGIN TRAN
	BEGIN TRY
		-- OBTENER ID DEL LIBRO QUE SE QUERIA PRESTAR
		DECLARE @BookId INT = (SELECT BookId FROM [Library].BookLoan WHERE BookLoanId = @BookLoanId)
		-- RESTAR 1 EN LA CANTIDAD DE COPIAS PRESTADAS DEL LIBRO
		DECLARE @SPResult TABLE (
			IsSuccess INT,
			[Message] VARCHAR(255)
		)
		INSERT INTO @SPResult EXEC [Library].uspUpdateBorrowedBooks @BookId, -1
		-- VERIFICAR SI OCURRIO UN ERROR AL ACTUALIZAR LA CANTIDAD DE COPIAS PRESTADAS DEL LIBRO
		IF ((SELECT IsSuccess FROM @SPResult) != 0)
		BEGIN
			--
			SELECT IsSuccess, [Message] FROM @SPResult
			--
			IF @@TRANCOUNT > 0
				ROLLBACK TRAN;
		END
		-- ACTUALIZAR ESTADO DE LA SOLICITUD A 'ELIMINADA'
		UPDATE [Library].BookLoan SET [State] = 'ELIMINADA' WHERE BookLoanId = @BookLoanId
		-- RETORNAR MENSAJE DE EXITO SI TODO SALIO BIEN
		SELECT 0 AS IsSuccess, 'Solicitud de préstamo eliminada exitosamente' AS [Message]
		--
		IF @@ERROR = 0
			IF @@TRANCOUNT > 0
				COMMIT TRAN;
	END TRY
	BEGIN CATCH
		--
		IF @@TRANCOUNT > 0
			ROLLBACK TRAN;
		--
		SELECT 1 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET BookLoan
IF OBJECT_ID('Library.uspGetBookLoan', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetBookLoan;  
GO
CREATE PROC [Library].uspGetBookLoan (
	@BookLoanId INT = NULL
)
AS
BEGIN
	--
	IF (@BookLoanId IS NULL OR @BookLoanId = '')
	BEGIN
		SELECT BookLoanId, StudentId, BookId, TypeOfLoan, [State], LoanDate, DueDate, ReturnDate FROM [Library].BookLoan
	END
	ELSE
	BEGIN
		SELECT BookLoanId, StudentId, BookId, TypeOfLoan, [State], LoanDate, DueDate, ReturnDate FROM [Library].BookLoan WHERE BookLoanId = @BookLoanId
	END
END
GO