USE LibraryManagementDB
GO

--INSERT Authors
EXEC [Library].uspInsertAuthor 'Earl. W. Swokowski', 0
GO
EXEC [Library].uspInsertAuthor 'Jefferey A. Cole', 0
GO
EXEC [Library].uspInsertAuthor 'Charles H. Lehmann', 0
GO
EXEC [Library].uspInsertAuthor 'Aurelio Baldor', 0
GO
EXEC [Library].uspInsertAuthor 'James Stewart', 0
GO

EXEC [Library].uspInsertAuthor 'Br. William Perez', 1
GO
EXEC [Library].uspInsertAuthor 'Br. Jose Lopez', 1
GO