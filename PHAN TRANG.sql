SELECT * 
FROM   HOCSINH
ORDER BY id OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY

SELECT * 
FROM   HOCSINH
ORDER BY id OFFSET 10 ROWS FETCH NEXT 10 ROWS ONLY

CREATE PROC PhanTrang
AS
	DECLARE @NameTable TABLE(Id int)
	DECLARE @LineStart int
	BEGIN
		SELECT *  FROM  @NameTable ORDER BY Id OFFSET @LineStart ROWS FETCH NEXT 10 ROWS ONLY
	END
DROP PROC PhanTrang