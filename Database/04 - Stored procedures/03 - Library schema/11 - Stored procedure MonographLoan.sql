-- CREATE STORED PROCEDURES FOR MonographLoan TABLE
USE LibraryManagementDB
GO

-- IsSuccess (0. Éxito, 1. No paso una validación, 2. No existe el recurso, 3. Error en la base de datos)

--INSERT MonographLoan
IF OBJECT_ID('Library.uspInsertMonographLoan', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspInsertMonographLoan;  
GO
CREATE PROC [Library].uspInsertMonographLoan (
	@StudentId INT,
	@MonographId INT
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
	-- VERIFICAR SI EL ESTUDIANTE GENERA UNA SOLICITUD DUPLICADA PARA LA MISMA MONOGRAFIA
	IF ([Library].ufnCheckDuplicateMonographRequest(@StudentId, @MonographId) = 1)
	BEGIN
		SELECT 1 AS IsSuccess, 'Aún tiene una solicitud de préstamo pendiente de esta monografía' AS [Message]
		RETURN
	END
	-- VERIFICAR SI EL ESTUDIANTE YA HA GENERADO UNA SOLICITUD
	IF ([Library].ufnHasStudentReachedMaxMonographRequest(@StudentId) = 1)
	BEGIN
		SELECT 1 AS IsSuccess, 'Se ha alcanzado el máximo de solicitudes' AS [Message]
		RETURN
	END
	-- VERIFICAR SI EL ESTUDIANTE PUEDE HACER UN PRESTAMO
	IF ([Library].ufnCanStudentBorrowMonograph(@StudentId) = 1)
	BEGIN
		SELECT 1 AS IsSuccess, 'Solo se puede prestar una monografía a la vez' AS [Message]
		RETURN
	END
	-- VERIFICAR SI LA MONOGRAFIA ESTA DISPONIBLE
	IF ([Library].ufnCheckMonographAvailability(@MonographId) = 0)
	BEGIN
		SELECT 1 AS IsSuccess, 'La monografía no esta disponible para préstamo en estos momentos' AS [Message]
		RETURN
	END
	--
	BEGIN TRAN
	BEGIN TRY
		-- MARCAR LA MONOGRAFIA COMO NO DISPONIBLE
		DECLARE @SPResult TABLE (
			IsSuccess INT,
			[Message] VARCHAR(255)
		)
		INSERT INTO @SPResult EXEC [Library].uspUpdateIsAvailableMonograph @MonographId, 0
		-- VERIFICAR SI OCURRIO UN ERROR AL MARCAR LA MONOGRAFIA COMO NO DISPONIBLE
		IF ((SELECT IsSuccess FROM @SPResult) != 0)
		BEGIN
			--
			SELECT IsSuccess, [Message] FROM @SPResult
			--
			IF @@TRANCOUNT > 0
				ROLLBACK TRAN;
		END
		-- CREAR UNA NUEVA SOLICITUD DE PRESTAMO
		INSERT INTO [Library].MonographLoan (StudentId, MonographId) VALUES (@StudentId, @MonographId)
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
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--UPDATE BorrowedMonographLoan
IF OBJECT_ID('Library.uspUpdateBorrowedMonographLoan', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateBorrowedMonographLoan;  
GO
CREATE PROC [Library].uspUpdateBorrowedMonographLoan (
	@MonographLoanId INT,
	@DueDate DATETIME,
	@UserId INT
)
AS
BEGIN
	-- VERIFICAR ID SOLICITUD DE PRESTAMO
	IF (@MonographLoanId IS NULL OR @MonographLoanId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la solicitud de préstamo es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].MonographLoan WHERE MonographLoanId = @MonographLoanId)
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
	-- VERIFICAR ID DEL USUARIO 
	IF (@UserId IS NULL OR @UserId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del usuario que aprobó el préstamo del libro es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Security].[User] WHERE UserId = @UserId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un usuario con el Id proporcionado' AS [Message]
		RETURN
	END
	-- VERIFICAR QUE LA SOLICITUD A APROBAR TENGA POR ESTADO 'CREADA'
	IF ([Library].ufnCheckMonographLoanState(@MonographLoanId, 'CREADA') = 0)
	BEGIN
		SELECT 1 AS IsSuccess, 'Solo se pueden aprobar solicitudes con estado CREADA' AS [Message]
		RETURN
	END
	--
	BEGIN TRAN
	BEGIN TRY
		-- OBTENER ID DEL ESTUDIANTE AL QUE SE LE APROBARA EL PRESTAMO
		DECLARE @StudentId TINYINT = (SELECT StudentId FROM [Library].MonographLoan WHERE MonographLoanId = @MonographLoanId)
		-- ACTUALIZAR INFORMACION DEL ESTUDIANTE PARA INDICAR QUE SE LE HA PRESTADO UNA MONOGRAFIA
		DECLARE @SPResult TABLE (
			IsSuccess INT,
			[Message] VARCHAR(255)
		)
		INSERT INTO @SPResult EXEC [University].uspUpdateHasBorrowedMonographStudent @StudentId, 1
		-- VERIFICAR SI OCURRIO UN ERROR AL ACTUALIZAR PRESTAMO DEL ESTUDIANTE
		IF ((SELECT IsSuccess FROM @SPResult) != 0)
		BEGIN
			--
			SELECT IsSuccess, [Message] FROM @SPResult
			--
			IF @@TRANCOUNT > 0
				ROLLBACK TRAN;
		END
		-- ACTUALIZAR ESTADO DE LA SOLICITUD A 'PRESTADA'
		UPDATE [Library].MonographLoan SET [State] = 'PRESTADA', DueDate = @DueDate, BorrowedUserId = @UserId WHERE MonographLoanId = @MonographLoanId
		-- RETORNAR MENSAJE DE EXITO SI TODO SALIO BIEN
		SELECT 0 AS IsSuccess, 'Solicitud de préstamo actualizada exitosamente' AS [Message]
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
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--UPDATE ReturnedMonographLoan
IF OBJECT_ID('Library.uspUpdateReturnedMonographLoan', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspUpdateReturnedMonographLoan;  
GO
CREATE PROC [Library].uspUpdateReturnedMonographLoan (
	@MonographLoanId INT,
	@UserId INT
)
AS
BEGIN
	-- VERIFICAR ID SOLICITUD DE PRESTAMO
	IF (@MonographLoanId IS NULL OR @MonographLoanId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la solicitud de préstamo es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].MonographLoan WHERE MonographLoanId = @MonographLoanId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una solicitud de préstamo con el Id proporcionado' AS [Message]
		RETURN
	END
	-- VERIFICAR ID DEL USUARIO 
	IF (@UserId IS NULL OR @UserId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id del usuario que aprobó el préstamo del libro es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Security].[User] WHERE UserId = @UserId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe un usuario con el Id proporcionado' AS [Message]
		RETURN
	END
	-- VERIFICAR QUE LA SOLICITUD PARA HACER LA DEVOLUCION TENGA POR ESTADO 'PRESTADA'
	IF ([Library].ufnCheckMonographLoanState(@MonographLoanId, 'PRESTADA') = 0)
	BEGIN
		SELECT 1 AS IsSuccess, 'Solo se puede hacer devoluciones de solicitudes con estado PRESTADA' AS [Message]
		RETURN
	END
	--
	BEGIN TRAN
	BEGIN TRY
		-- OBTENER ID DEL ESTUDIANTE AL QUE SE LE PRESTO LA MONOGRAFIA
		DECLARE @StudentId TINYINT = (SELECT StudentId FROM [Library].MonographLoan WHERE MonographLoanId = @MonographLoanId)
		-- ACTUALIZAR INFORMACION DEL ESTUDIANTE PARA INDICAR QUE HA DEVUELTO LA MONOGRAFIA QUE SE LE PRESTO
		DECLARE @SPResult TABLE (
			IsSuccess INT,
			[Message] VARCHAR(255)
		)
		INSERT INTO @SPResult EXEC [University].uspUpdateHasBorrowedMonographStudent @StudentId, 0
		-- VERIFICAR SI OCURRIO UN ERROR AL ACTUALIZAR DEVOLUCION DEL ESTUDIANTE
		IF ((SELECT IsSuccess FROM @SPResult) != 0)
		BEGIN
			--
			SELECT IsSuccess, [Message] FROM @SPResult
			--
			IF @@TRANCOUNT > 0
				ROLLBACK TRAN;
		END
		-- LIMPIAR RESULTADOS ANTERIORES
		DELETE FROM @SPResult 
		-- ACTUALIZAR Y MARCAR COMO DISPONIBLE NUEVAMENTE LA MONOGRAFIA
		DECLARE @MonographId INT = (SELECT MonographId FROM [Library].MonographLoan WHERE MonographLoanId = @MonographLoanId)
		INSERT INTO @SPResult EXEC [Library].uspUpdateIsAvailableMonograph @MonographId, 1
		-- VERIFICAR SI OCURRIO UN ERROR AL MARCAR COMO DISPONIBLE LA MONOGRAFIA NUEVAMENTE
		IF ((SELECT IsSuccess FROM @SPResult) != 0)
		BEGIN
			--
			SELECT IsSuccess, [Message] FROM @SPResult
			--
			IF @@TRANCOUNT > 0
				ROLLBACK TRAN;
		END
		-- ACTUALIZAR ESTADO DE LA SOLICITUD A 'DEVUELTO'
		UPDATE [Library].MonographLoan SET [State] = 'DEVUELTA', ReturnDate = GETDATE(), ReturnedUserId = @UserId WHERE MonographLoanId = @MonographLoanId
		-- RETORNAR MENSAJE DE EXITO SI TODO SALIO BIEN
		SELECT 0 AS IsSuccess, 'Solicitud de préstamo actualizada exitosamente' AS [Message]
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
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--DELETE MonographLoan
IF OBJECT_ID('Library.uspDeleteMonographLoan', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspDeleteMonographLoan;  
GO
CREATE PROC [Library].uspDeleteMonographLoan (
	@MonographLoanId INT
)
AS
BEGIN
	-- VERIFICAR ID SOLICITUD DE PRESTAMO
	IF (@MonographLoanId IS NULL OR @MonographLoanId = '')
	BEGIN
		SELECT 1 AS IsSuccess, 'Id de la solicitud de préstamo es obligatorio' AS [Message]
		RETURN
	END
	--
	IF NOT EXISTS (SELECT 1 FROM [Library].MonographLoan WHERE MonographLoanId = @MonographLoanId)
	BEGIN
		SELECT 2 AS IsSuccess, 'No existe una solicitud de préstamo con el Id proporcionado' AS [Message]
		RETURN
	END
	-- VERIFICAR QUE LA SOLICITUD A ELIMINAR NO TENGA POR ESTADO 'PRESTADA' O 'DEVUELTA'
	DECLARE @MonographLoanState CHAR(9) = (SELECT [State] FROM [Library].MonographLoan WHERE MonographLoanId = @MonographLoanId)
	IF (@MonographLoanState != 'CREADA')
	BEGIN
		SELECT 1 AS IsSuccess, 'No se pueden eliminar las solicitudes pendientes ni ya devueltas' AS [Message]
		RETURN
	END
	--
	BEGIN TRAN
	BEGIN TRY
		-- OBTENER ID DE LA MONOGRAFIA QUE SE QUERIA PRESTAR
		DECLARE @MonographId INT = (SELECT MonographId FROM [Library].MonographLoan WHERE MonographLoanId = @MonographLoanId)
		-- ACTUALIZAR Y MARCAR COMO DISPONIBLE NUEVAMENTE LA MONOGRAFIA
		DECLARE @SPResult TABLE (
			IsSuccess INT,
			[Message] VARCHAR(255)
		)
		INSERT INTO @SPResult EXEC [Library].uspUpdateIsAvailableMonograph @MonographId, 1
		-- VERIFICAR SI OCURRIO UN ERROR AL ACTUALIZAR DEVOLUCION DEL ESTUDIANTE
		IF ((SELECT IsSuccess FROM @SPResult) != 0)
		BEGIN
			--
			SELECT IsSuccess, [Message] FROM @SPResult
			--
			IF @@TRANCOUNT > 0
				ROLLBACK TRAN;
		END
		-- ACTUALIZAR ESTADO DE LA SOLICITUD A 'ELIMINADA'
		UPDATE [Library].MonographLoan SET [State] = 'ELIMINADA' WHERE MonographLoanId = @MonographLoanId
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
		SELECT 3 AS IsSuccess, ERROR_MESSAGE() AS [Message]
	END CATCH
END
GO

--GET MonographLoan
IF OBJECT_ID('Library.uspGetMonographLoan', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetMonographLoan;  
GO
CREATE PROC [Library].uspGetMonographLoan (
	@MonographLoanId INT = NULL
)
AS
BEGIN
	--
	IF (@MonographLoanId IS NULL OR @MonographLoanId = '')
	BEGIN
		SELECT MonographLoanId, StudentId, MonographId, [State], LoanDate, DueDate, ReturnDate, BorrowedUserId, ReturnedUserId FROM [Library].MonographLoan
		ORDER BY LoanDate DESC
	END
	ELSE
	BEGIN
		SELECT MonographLoanId, StudentId, MonographId, [State], LoanDate, DueDate, ReturnDate, BorrowedUserId, ReturnedUserId FROM [Library].MonographLoan 
		WHERE MonographLoanId = @MonographLoanId ORDER BY LoanDate DESC
	END
END
GO

-- GET MonographLoanByStudentCarnet
IF OBJECT_ID('Library.uspMonographLoanByStudentCarnet', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspMonographLoanByStudentCarnet;  
GO
CREATE PROC [Library].uspMonographLoanByStudentCarnet (
	@Carnet CHAR(10)
)
AS
BEGIN
	--
	SELECT bl.MonographLoanId, bl.StudentId, bl.MonographId, bl.[State], bl.LoanDate, bl.DueDate, bl.ReturnDate, bl.BorrowedUserId, bl.ReturnedUserId 
	FROM [Library].MonographLoan AS bl
	INNER JOIN [University].Student AS s ON bl.StudentId = s.StudentId AND s.Carnet = @Carnet
	ORDER BY bl.LoanDate DESC
END
GO

-- GET MonographLoanByState
IF OBJECT_ID('Library.uspGetMonographLoanByState', 'P') IS NOT NULL  
    DROP PROCEDURE [Library].uspGetMonographLoanByState;  
GO
CREATE PROC [Library].uspGetMonographLoanByState (
	@State CHAR(9)
)
AS
BEGIN
	--
	SELECT bl.MonographLoanId, bl.StudentId, bl.MonographId, bl.[State], bl.LoanDate, bl.DueDate, bl.ReturnDate, bl.BorrowedUserId, bl.ReturnedUserId 
	FROM [Library].MonographLoan AS bl
	WHERE bl.[State] = @State
	ORDER BY bl.LoanDate DESC
END
GO