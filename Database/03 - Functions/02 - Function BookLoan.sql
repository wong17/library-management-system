-- CREATE FUNCTIONS FOR BookLoan TABLE
USE LibraryManagementDB
GO

-- PARA COMPROBAR SI YA SE LE PRESTO EL LIBRO QUE ESTA SOLICITANDO NUEVAMENTE
IF OBJECT_ID (N'Library.ufnHasStudentBorrowedBook', N'FN') IS NOT NULL
    DROP FUNCTION [Library].ufnHasStudentBorrowedBook;
GO
CREATE FUNCTION [Library].ufnHasStudentBorrowedBook (
	@StudentId INT,
	@BookId INT
) RETURNS BIT
AS
BEGIN
	-- 
	RETURN (
		SELECT 1 FROM [Library].BookLoan 
		WHERE (StudentId = @StudentId AND BookId = @BookId) AND [State] = 'PRESTADO'
	)
END
GO

-- PARA EVITAR QUE GENERE OTRA SOLICITUD PIDIENDO EL MISMO LIBRO 2 VECES
IF OBJECT_ID (N'Library.ufnCheckDuplicateBookRequest', N'FN') IS NOT NULL
    DROP FUNCTION [Library].ufnCheckDuplicateBookRequest;
GO
CREATE FUNCTION [Library].ufnCheckDuplicateBookRequest (
	@StudentId INT,
	@BookId INT
) RETURNS BIT
AS
BEGIN
	--
	RETURN (
		SELECT 1 FROM [Library].BookLoan 
		WHERE (StudentId = @StudentId AND BookId = @BookId) AND [State] = 'CREADA'
	)
END
GO

-- PARA EVITAR QUE GENERE MAS DE 3 SOLICITUDES DISTINTAS Y QUEDEN PENDIENTES POR APROBAR ('CREADA')
IF OBJECT_ID (N'Library.ufnHasStudentReachedMaxBookRequest', N'FN') IS NOT NULL
    DROP FUNCTION [Library].ufnHasStudentReachedMaxBookRequest;
GO
CREATE FUNCTION [Library].ufnHasStudentReachedMaxBookRequest (
	@StudentId INT
) RETURNS BIT
AS
BEGIN
	--
	RETURN (SELECT CASE 
						WHEN COUNT(1) = 3 THEN 1 
						ELSE 0 
					END 
			FROM [Library].BookLoan 
			WHERE StudentId = @StudentId AND [State] = 'CREADA'
	)
END
GO

-- COMPROBAR SI AUN QUEDAN COPIAS DISPONIBLES DEL LIBRO PARA PRESTAMO
IF OBJECT_ID (N'Library.ufnCheckBookAvailability', N'FN') IS NOT NULL
    DROP FUNCTION [Library].ufnCheckBookAvailability;
GO
CREATE FUNCTION [Library].ufnCheckBookAvailability (
	@BookId INT
) RETURNS BIT
AS
BEGIN
	--
	RETURN (
		SELECT IsAvailable FROM [Library].Book WHERE BookId = @BookId
	)
END
GO

-- VERIFICAR QUE LA SOLICITUD A APROBAR TENGO POR ESTADO 'CREADA' EN EL CASO DE APROBAR 
-- O 'PRESTADA' EN EL CASO DE DEVOLVER
IF OBJECT_ID (N'Library.ufnCheckBookLoanState', N'FN') IS NOT NULL
    DROP FUNCTION [Library].ufnCheckBookLoanState;
GO
CREATE FUNCTION [Library].ufnCheckBookLoanState (
	@BookLoanId INT,
	@State CHAR(9)
) RETURNS BIT
AS
BEGIN
	--
	RETURN (
		SELECT CASE
					WHEN  COUNT(1) = 1 THEN 1
					ELSE 0
				END
		FROM [Library].BookLoan 
		WHERE BookLoanId = @BookLoanId AND [State] = @State
	)
END
GO