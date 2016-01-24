USE ABUELO 
GO  
CREATE PROCEDURE SPU_INSERTAR_PERSONAS
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
BEGIN
BEGIN TRANSACTION TR_PERSONAS
Declare @Mensaje nvarchar(30)
BEGIN
Declare @IdPersona char(8)
SET @IdPersona = 'PER'+RIGHT('0000'+LTRIM(STR(COUNT(*)+1)),4)
INSERT INTO PERSONAS(IdPersona,ApellidoP,ApellidoM,Nombres,Sexo,FechaNac,Telefono,Celu_Movi,Celu_Claro,Estado) VALUES (@IdPersona,@ApellidoP,@ApellidoM,@Nombres,@Sexo,@FechaNac,@Telefono,@Celu_Movi,@Celu_Claro,@Estado)
END
END
GO

