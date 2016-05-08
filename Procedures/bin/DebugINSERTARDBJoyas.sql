USE DBJoyas 
GO  
CREATE PROCEDURE SPU_INSERTAR_Sucursal
@Id char(8), 
@IdEmpresa char(8), 
@Descripcion varchar(150), 
@Direccion varchar(250), 
@Estado char(1) 
AS
BEGIN
	BEGIN TRANSACTION TR_Sucursal
	Declare @Mensaje nvarchar(30)
		BEGIN
				Declare 
				SET @ = 'S'+(SELECT RIGHT('0000000'+LTRIM(STR(COUNT(*)+1)),7) FROM Sucursal)
				INSERT INTO Sucursal(Id,IdEmpresa,Descripcion,Direccion,Estado) VALUES (@Id,@IdEmpresa,@Descripcion,@Direccion,@Estado)
					IF @@ERROR<>0
						BEGIN
						SET @Mensaje='Error al registrar al Sucursal'
						GOTO ERROR
					END
			OK:
			COMMIT TRANSACTION TR_Sucursal
			SET @Mensaje='Sucursal Registrado'
			PRINT(@Mensaje)
			GOTO FIN
		ERROR:
			ROLLBACK TRANSACTION TR_Sucursal
			RAISERROR(@Mensaje,15,1)
			GOTO FIN
		FIN:
			END
END
GO

