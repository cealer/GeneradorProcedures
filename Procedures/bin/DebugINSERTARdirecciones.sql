USE direcciones 
GO  
CREATE PROCEDURE SPU_INSERTAR_direcciones
@idDirecion int, 
@Direccion nvarchar(50)= NULL
AS
BEGIN
	BEGIN TRANSACTION TR_direcciones
	Declare @Mensaje nvarchar(30)
		BEGIN
			IF LEN(@Direccion)=0
                    BEGIN
					SET @Mensaje='El direccion no puede estar en blanco'
				END
				Declare 
				SET @ = ''+(SELECT RIGHT(''+LTRIM(STR(COUNT(*)+1)),0) FROM direcciones)
				INSERT INTO direcciones(idDirecion,Direccion) VALUES (@idDirecion,@Direccion)
					IF @@ERROR<>0
						BEGIN
						SET @Mensaje='Error al registrar al direcciones'
						GOTO ERROR
					END
			OK:
			COMMIT TRANSACTION TR_direcciones
			SET @Mensaje='direcciones Registrado'
			PRINT(@Mensaje)
			GOTO FIN
		ERROR:
			ROLLBACK TRANSACTION TR_direcciones
			RAISERROR(@Mensaje,15,1)
			GOTO FIN
		FIN:
			END
END
GO

