-- CREATE BACKUP
BACKUP DATABASE [LibraryManagementDB] 
TO DISK = 'D:\Repositories\library-management-system\Database\LibraryManagementDB.Bak'
WITH FORMAT; 

-- RESTORE BACKUP
RESTORE DATABASE [LibraryManagementDB] 
FROM DISK = N'D:\Repositories\library-management-system\Database\LibraryManagementDB.Bak' 
WITH FILE = 1,
NOUNLOAD,  STATS = 5