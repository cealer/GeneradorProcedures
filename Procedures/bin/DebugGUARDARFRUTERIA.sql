USE FRUTERIA 
GO  
CREATE PROCEDURE SPU_INSERTAR_FRUTA
@NombreFruta nvarchar(30), 
@Precio money, 
@IdUnidad smallint 
AS
Declare @IdFruta smallint()
SELECT IdFruta = 'FRU'+RIGHT('000'+LTRIM(STR(COUNT(*)+1)),3)
INSERT INTO FRUTA VALUES (@IdFruta,@NombreFruta,@Precio,@IdUnidad)
GO

