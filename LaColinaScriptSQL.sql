USE [COLINADB]
GO

CREATE TABLE [dbo].[TBL_PRECIOS_SUBPRODUCTOS](
	[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[ID_SUBPRODUCTO] [numeric](18, 0) NULL,
	[DESCRIPCION] [varchar](255) NULL,
	[PRECIO_INDIVIDUAL] [numeric](18, 0) NULL,
	[CANTIDAD_PORCION] [numeric](18, 3) NULL,
	[VALOR_MEDIDA] [numeric](18, 3) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TBL_PRODUCTOS]    Script Date: 12/09/2018 10:27:20 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_PRODUCTOS](
	[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[PRODUCTO] [varchar](255) NULL,
	[ESTADO] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TBL_PRODUCTOS_SOLICITUD]    Script Date: 12/09/2018 10:27:20 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_PRODUCTOS_SOLICITUD](
	[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[FECHA_REGISTRO] [datetime] NULL,
	[ID_SOLICITUD] [numeric](18, 0) NULL,
	[ID_SUBPRODUCTO] [numeric](18, 0) NULL,
	[ID_MESERO] [numeric](18, 0) NULL,
	[PRECIO_PRODUCTO] [numeric](18, 0) NULL,
	[PRECIO_FINAL] [numeric](18, 0) NULL,
	[ESTADO_PRODUCTOS] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TBL_SOLICITUD]    Script Date: 12/09/2018 10:27:20 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_SOLICITUD](
	[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[FECHA_SOLICITUD] [datetime] NULL,
	[ID_MESA] [numeric](18, 0) NULL,
	[ID_MESERO] [numeric](18, 0) NULL,
	[IDENTIFICACION_CLIENTE] [varchar](255) NULL,
	[NOMBRE_CLIENTE] [varchar](255) NULL,
	[ESTADO_SOLICITUD] [varchar](255) NULL,
	[OBSERVACIONES] [ntext] NULL,
	[OTROS_COBROS] [numeric](18, 0) NULL,
	[DESCUENTOS] [numeric](18, 0) NULL,
	[TOTAL] [numeric](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TBL_SUBPRODUCTOS]    Script Date: 12/09/2018 10:27:20 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_SUBPRODUCTOS](
	[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[ID_PRODUCTO] [numeric](18, 0) NOT NULL,
	[FECHA_INGRESO] [datetime] NULL,
	[NOMBRE_SUBPRODUCTO] [varchar](255) NULL,
	[PRECIO_UNITARIO] [varchar](255) NULL,
	[CANTIDAD_EXISTENCIA] [numeric](18, 3) NULL,
	[DESCRIPCION] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TBL_SUBPRODUCTOS_LOG]    Script Date: 12/09/2018 10:27:20 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_SUBPRODUCTOS_LOG](
	[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[ID_PRINCIPAL] [numeric](18, 0) NULL,
	[ID_PRODUCTO] [numeric](18, 0) NOT NULL,
	[FECHA_INGRESO] [datetime] NULL,
	[NOMBRE_SUBPRODUCTO] [varchar](255) NULL,
	[PRECIO_UNITARIO] [varchar](255) NULL,
	[CANTIDAD_EXISTENCIA] [decimal](18, 0) NULL,
	[DESCRIPCION] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TBL_USUARIOS]    Script Date: 12/09/2018 10:27:20 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_USUARIOS](
	[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[CEDULA] [numeric](18, 0) NULL,
	[NOMBRE] [varchar](255) NULL,
	[CONTRASEÑA] [varchar](255) NULL,
	[ID_PERFIL] [numeric](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


