-- CREATE FUNCTIONS FOR Student TABLE
USE LibraryManagementDB
GO

-- PARA COMPROBAR SI AUN PUEDE PRESTAR OTRO LIBRO (MAX 3)
IF OBJECT_ID (N'Library.ufnCanStudentBorrowBook', N'FN') IS NOT NULL
    DROP FUNCTION [Library].ufnCanStudentBorrowBook;
GO
CREATE FUNCTION [Library].ufnCanStudentBorrowBook (
	@StudentId INT
) RETURNS BIT
AS
BEGIN
	--
	DECLARE @StudentBorrowedBooks SMALLINT = (SELECT BorrowedBooks FROM [University].Student WHERE StudentId = @StudentId)
	IF (@StudentBorrowedBooks = 3)
		RETURN 0

	RETURN 1
END
GO

-- PARA COMPROBAR SI AUN PUEDE PRESTAR UNA MONOGRAFIA (MAX 1)
IF OBJECT_ID (N'Library.ufnCanStudentBorrowMonograph', N'FN') IS NOT NULL
    DROP FUNCTION [Library].ufnCanStudentBorrowMonograph;
GO
CREATE FUNCTION [Library].ufnCanStudentBorrowMonograph (
	@StudentId INT
) RETURNS BIT
AS
BEGIN
	--
	RETURN (
		SELECT HasBorrowedMonograph FROM [University].Student WHERE StudentId = @StudentId
	)
END
GO