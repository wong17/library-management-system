USE LibraryManagementDB
GO

--INSERT BookSubCategory
EXEC [Library].uspInsertBookSubCategory 1, 1
GO
EXEC [Library].uspInsertBookSubCategory 1, 3
GO
EXEC [Library].uspInsertBookSubCategory 1, 4
GO
EXEC [Library].uspInsertBookSubCategory 2, 3
GO
EXEC [Library].uspInsertBookSubCategory 3, 2
GO
EXEC [Library].uspInsertBookSubCategory 3, 4
GO
EXEC [Library].uspInsertBookSubCategory 4, 1
GO
EXEC [Library].uspInsertBookSubCategory 5, 5
GO