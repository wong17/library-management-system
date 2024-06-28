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

EXEC [Library].uspInsertBookSubCategory 6, 6
GO
EXEC [Library].uspInsertBookSubCategory 6, 7
GO
EXEC [Library].uspInsertBookSubCategory 6, 8
GO
EXEC [Library].uspInsertBookSubCategory 6, 14
GO
EXEC [Library].uspInsertBookSubCategory 7, 15
GO
EXEC [Library].uspInsertBookSubCategory 7, 16
GO
EXEC [Library].uspInsertBookSubCategory 7, 11
GO
EXEC [Library].uspInsertBookSubCategory 7, 12
GO
EXEC [Library].uspInsertBookSubCategory 7, 13
GO