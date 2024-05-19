-- CREATE FUNCTIONS FOR MonographLoan TABLE
USE LibraryManagementDB
GO

-- COMPROBAR SI LA MONOGRAFIA AUN ESTA DISPONIBLE PARA PRESTAMO
IF OBJECT_ID (N'Library.ufnCheckMonographAvailability', N'FN') IS NOT NULL
    DROP FUNCTION [Library].ufnCheckMonographAvailability;
GO
CREATE FUNCTION [Library].ufnCheckMonographAvailability (
	@MonographId INT
) RETURNS BIT
AS
BEGIN
	RETURN (
		SELECT IsAvailable FROM [Library].Monograph WHERE MonographId = @MonographId
	)
END
GO

-- PARA EVITAR QUE GENERE OTRA SOLICITUD PIDIENDO LA MISMA MONOGRAFIA 2 VECES
IF OBJECT_ID (N'Library.ufnCheckDuplicateMonographRequest', N'FN') IS NOT NULL
    DROP FUNCTION [Library].ufnCheckDuplicateMonographRequest;
GO
CREATE FUNCTION [Library].ufnCheckDuplicateMonographRequest (
	@StudentId INT,
	@MonographId INT
) RETURNS BIT
AS
BEGIN
	--
	RETURN (
		SELECT 1 FROM [Library].MonographLoan 
		WHERE (StudentId = @StudentId AND MonographId = @MonographId) AND [State] = 'CREADA'
	)
END
GO

-- PARA EVITAR QUE GENERE MAS DE UAN SOLICITUD
IF OBJECT_ID (N'Library.ufnHasStudentReachedMaxMonographRequest', N'FN') IS NOT NULL
    DROP FUNCTION [Library].ufnHasStudentReachedMaxMonographRequest;
GO
CREATE FUNCTION [Library].ufnHasStudentReachedMaxMonographRequest (
	@StudentId INT
) RETURNS BIT
AS
BEGIN
	--
	RETURN (SELECT CASE 
						WHEN COUNT(1) = 1 THEN 1 
						ELSE 0 
					END 
			FROM [Library].MonographLoan 
			WHERE StudentId = @StudentId AND [State] = 'CREADA'
	)
END
GO

-- VERIFICAR QUE LA SOLICITUD A APROBAR TENGO POR ESTADO 'CREADA' EN EL CASO DE APROBAR 
-- O 'PRESTADA' EN EL CASO DE DEVOLVER
IF OBJECT_ID (N'Library.ufnCheckMonographLoanState', N'FN') IS NOT NULL
    DROP FUNCTION [Library].ufnCheckMonographLoanState;
GO
CREATE FUNCTION [Library].ufnCheckMonographLoanState (
	@MonographLoanId INT,
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
		FROM [Library].MonographLoan 
		WHERE MonographLoanId = @MonographLoanId AND [State] = @State
	)
END
GO