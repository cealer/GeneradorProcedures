USE FRUTERIA 
GO  
CREATE PROCEDURE SPU_INSERTAR_FRUTA
@NombreFruta nvarchar(30), 
@Precio money, 
@IdUnidad char(8) 
AS
BEGIN
	BEGIN TRANSACTION TR_FRUTA
	Declare @Mensaje nvarchar(30)
		BEGIN
			IF LEN(@NombreFruta)=0
                    BEGIN
					SET @Mensaje='El nombrefruta no puede estar en blanco'
				END
			IF @Precio>0
                    BEGIN
					SET @Mensaje='El precio no puede ser menor o igual que 0'
				END
				Declare @IdFruta char(8)
				SET @IdFruta = 'FRU'+(SELECT RIGHT('00000'+LTRIM(STR(COUNT(*)+1)),5) FROM FRUTA)
				INSERT INTO FRUTA(IdFruta,NombreFruta,Precio,IdUnidad) VALUES (@IdFruta,@NombreFruta,@Precio,@IdUnidad)
					IF @@ERROR<>0
						BEGIN
						SET @Mensaje='Error al registrar al FRUTA'
						GOTO ERROR
					END
			OK:
			COMMIT TRANSACTION TR_FRUTA
			SET @Mensaje='FRUTA Registrado'
			PRINT(@Mensaje)
			GOTO FIN
		ERROR:
			ROLLBACK TRANSACTION TR_FRUTA
			RAISERROR(@Mensaje,15,1)
			GOTO FIN
		FIN:
			END
END
GO

