USE FRUTERIA 
GO  
CREATE PROCEDURE SPU_INSERTAR_FRUTA
@IdFruta smallint, 
@NombreFruta nvarchar(30), 
@Precio money, 
@IdUnidad smallint 
AS
INSERT INTO FRUTA VALUES (@IdFruta,@NombreFruta,@Precio,@IdUnidad)
GO

