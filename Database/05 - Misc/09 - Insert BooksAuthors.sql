USE LibraryManagementDB
GO

--INSERT BookAuthor
EXEC [Library].uspInsertBookAuthor 1, 1
GO
EXEC [Library].uspInsertBookAuthor 1, 2
GO
EXEC [Library].uspInsertBookAuthor 2, 3
GO
EXEC [Library].uspInsertBookAuthor 3, 4 
GO
EXEC [Library].uspInsertBookAuthor 4, 4 
GO
EXEC [Library].uspInsertBookAuthor 5, 5 
GO
EXEC [Library].uspInsertBookAuthor 6, 6 
GO
EXEC [Library].uspInsertBookAuthor 6, 7 
GO
EXEC [Library].uspInsertBookAuthor 7, 6 
GO
EXEC [Library].uspInsertBookAuthor 7, 7 
GO