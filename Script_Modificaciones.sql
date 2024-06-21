---ROLL OUT
ALTER TABLE TBL_DIAS_TRABAJADOS
ADD ID_PERFIL NUMERIC(18,0)
---ROLL BACK
ALTER TABLE TBL_DIAS_TRABAJADOS
DROP COLUMN ID_PERFIL 

	
---ROLL OUT
CREATE TABLE [dbo].[TBL_SISTEMA](
	[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL Primary key,
	[PARAMETRO] [varchar](255) NULL,
	[VALOR] [varchar](255) NULL
)
---ROLL BACK
DROP TABLE [dbo].[TBL_SISTEMA]


---ROLL OUT
CREATE TABLE [dbo].[TBL_TOKENS_DIAN](
	[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL Primary key,
	[FECHA_TOKEN] [datetime] NULL,
	[FECHA_VENCIMIENTO] [datetime] NULL,
	[ACCESS_TOKEN] [varchar](255) NULL,
	[EXPIRES_IN] [int] NULL,
	[TOKEN_TYPE] [varchar](255) NULL,
	[SCOPE] [varchar](255) NULL
)
ALTER TABLE TBL_PRODUCTOS
ADD UP_DIAN NUMERIC(2,0)
UPDATE TBL_PRODUCTOS
SET UP_DIAN = 0
ALTER TABLE TBL_PRODUCTOS
ADD ID_DIAN VARCHAR(255), ACCOUNT_GROUP_DIAN NUMERIC(10,0)
---ROLL BACK
DROP TABLE [dbo].[TBL_PRODUCTOS]





