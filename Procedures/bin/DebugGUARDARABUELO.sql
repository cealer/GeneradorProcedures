USE ABUELO 
GO  
CREATE PROCEDURE SPU_INSERTAR_PERSONAS
@IdPersona char(8), 
@ApellidoP nvarchar(20), 
@ApellidoM nvarchar(20), 
@Nombres nvarchar(40), 
@Sexo char(1), 
@FechaNac date= NULL,
@Telefono char(9)= NULL,
@Celu_Movi char(9)= NULL,
@Celu_Claro char(9)= NULL,
@Estado char(1) 
AS
INSERT INTO PERSONAS VALUES (@IdPersona,@ApellidoP,@ApellidoM,@Nombres,@Sexo,@FechaNac,@Telefono,@Celu_Movi,@Celu_Claro,@Estado)
GO

