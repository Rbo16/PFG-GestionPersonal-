USE [master]
GO
/****** Object:  User [##MS_PolicyEventProcessingLogin##]    Script Date: 11/07/2023 0:59:21 ******/
CREATE USER [##MS_PolicyEventProcessingLogin##] FOR LOGIN [##MS_PolicyEventProcessingLogin##] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [##MS_AgentSigningCertificate##]    Script Date: 11/07/2023 0:59:21 ******/
CREATE USER [##MS_AgentSigningCertificate##] FOR LOGIN [##MS_AgentSigningCertificate##]
GO
/****** Object:  Table [dbo].[Ausencia]    Script Date: 11/07/2023 0:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ausencia](
	[IdAusencia] [int] IDENTITY(1,1) NOT NULL,
	[IdSolicitante] [int] NOT NULL,
	[IdAutorizador] [int] NULL,
	[Razon] [varchar](20) NOT NULL,
	[FechaInicioA] [datetime] NOT NULL,
	[FechaFinA] [datetime] NOT NULL,
	[DescripcionAus] [text] NULL,
	[JustificantePDF] [varchar](50) NULL,
	[EstadoA] [int] NOT NULL,
	[FechaUltModif] [datetime] NOT NULL,
	[IdModif] [int] NOT NULL,
	[Borrado] [bit] NOT NULL,
 CONSTRAINT [PK__Ausencia__0B5BA41468AFF551] PRIMARY KEY CLUSTERED 
(
	[IdAusencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contrato]    Script Date: 11/07/2023 0:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contrato](
	[IdContrato] [int] IDENTITY(1,1) NOT NULL,
	[IdEmpleado] [int] NOT NULL,
	[HorasTrabajo] [float] NOT NULL,
	[HoraEntrada] [time](3) NOT NULL,
	[HoraSalida] [time](3) NOT NULL,
	[HorasDescanso] [float] NOT NULL,
	[Salario] [float] NOT NULL,
	[Puesto] [varchar](20) NOT NULL,
	[VacacionesMes] [float] NOT NULL,
	[FechaAlta] [datetime] NOT NULL,
	[FechaBaja] [datetime] NULL,
	[Duracion] [varchar](20) NOT NULL,
	[TipoContrato] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
	[DocumentoPDF] [varchar](50) NOT NULL,
	[FechaUltModif] [datetime] NOT NULL,
	[IdModif] [int] NOT NULL,
	[Borrado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdContrato] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Departamento]    Script Date: 11/07/2023 0:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departamento](
	[IdDepartamento] [int] IDENTITY(1,1) NOT NULL,
	[IdJefeDep] [int] NULL,
	[NombreD] [varchar](20) NOT NULL,
	[DescripcionD] [varchar](50) NOT NULL,
	[FechaUltModif] [datetime] NOT NULL,
	[IdModif] [int] NOT NULL,
	[Borrado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDepartamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empleado]    Script Date: 11/07/2023 0:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empleado](
	[IdEmpleado] [int] IDENTITY(1,1) NOT NULL,
	[NombreE] [varchar](20) NOT NULL,
	[Apellido] [varchar](20) NOT NULL,
	[Usuario] [varchar](20) NOT NULL,
	[Contrasenia] [varchar](100) NOT NULL,
	[DNI] [char](9) NOT NULL,
	[NumSS] [char](12) NOT NULL,
	[Rol] [int] NOT NULL,
	[Tlf] [varchar](20) NULL,
	[CorreoE] [varchar](50) NULL,
	[IdDepartamento] [int] NULL,
	[EstadoE] [int] NOT NULL,
	[FechaUltModif] [datetime] NOT NULL,
	[IdModif] [int] NULL,
	[Borrado] [bit] NOT NULL,
 CONSTRAINT [PK__Empleado__CE6D8B9EE56267AE] PRIMARY KEY CLUSTERED 
(
	[IdEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumEstadoAusencia]    Script Date: 11/07/2023 0:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EnumEstadoAusencia](
	[IdEstadoA] [int] IDENTITY(1,1) NOT NULL,
	[EstadoA] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEstadoA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumEstadoEmpleado]    Script Date: 11/07/2023 0:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EnumEstadoEmpleado](
	[IdEstadoE] [int] IDENTITY(1,1) NOT NULL,
	[EstadoE] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEstadoE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumPrioridad]    Script Date: 11/07/2023 0:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EnumPrioridad](
	[IdPrioridad] [int] IDENTITY(1,1) NOT NULL,
	[Prioridad] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPrioridad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumTipoContrato]    Script Date: 11/07/2023 0:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EnumTipoContrato](
	[IdTipoContrato] [int] IDENTITY(1,1) NOT NULL,
	[TipoContrato] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdTipoContrato] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumTipoEmpleado]    Script Date: 11/07/2023 0:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EnumTipoEmpleado](
	[IdRol] [int] IDENTITY(1,1) NOT NULL,
	[NombreRol] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ParticipacionProyecto]    Script Date: 11/07/2023 0:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ParticipacionProyecto](
	[IdParticipacionProyecto] [int] IDENTITY(1,1) NOT NULL,
	[IdEmpleado] [int] NOT NULL,
	[IdProyecto] [int] NOT NULL,
	[FechaUltModif] [date] NOT NULL,
	[IdModif] [int] NOT NULL,
	[Borrado] [bit] NOT NULL,
 CONSTRAINT [PK__Particip__27C41D243CA2AD7F] PRIMARY KEY CLUSTERED 
(
	[IdParticipacionProyecto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proyecto]    Script Date: 11/07/2023 0:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proyecto](
	[IdProyecto] [int] IDENTITY(1,1) NOT NULL,
	[NombreP] [varchar](20) NOT NULL,
	[Cliente] [varchar](20) NOT NULL,
	[FechaInicioP] [date] NOT NULL,
	[FechaFinP] [date] NULL,
	[Tiempo] [varchar](20) NOT NULL,
	[Presupuesto] [int] NOT NULL,
	[Prioridad] [int] NOT NULL,
	[DescripcionP] [text] NOT NULL,
	[FechaUltModif] [datetime] NOT NULL,
	[IdModif] [int] NOT NULL,
	[Borrado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdProyecto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Ausencia] ADD  CONSTRAINT [DF__Ausencia__Borrad__536D5C82]  DEFAULT ((0)) FOR [Borrado]
GO
ALTER TABLE [dbo].[Contrato] ADD  DEFAULT ((0)) FOR [Borrado]
GO
ALTER TABLE [dbo].[Departamento] ADD  DEFAULT ((0)) FOR [Borrado]
GO
ALTER TABLE [dbo].[Empleado] ADD  CONSTRAINT [DF__Empleado__Borrad__442B18F2]  DEFAULT ((0)) FOR [Borrado]
GO
ALTER TABLE [dbo].[Ausencia]  WITH CHECK ADD  CONSTRAINT [FK__Ausencia__Estado__5649C92D] FOREIGN KEY([EstadoA])
REFERENCES [dbo].[EnumEstadoAusencia] ([IdEstadoA])
GO
ALTER TABLE [dbo].[Ausencia] CHECK CONSTRAINT [FK__Ausencia__Estado__5649C92D]
GO
ALTER TABLE [dbo].[Ausencia]  WITH CHECK ADD  CONSTRAINT [FK__Ausencia__IdAuto__5555A4F4] FOREIGN KEY([IdAutorizador])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Ausencia] CHECK CONSTRAINT [FK__Ausencia__IdAuto__5555A4F4]
GO
ALTER TABLE [dbo].[Ausencia]  WITH CHECK ADD  CONSTRAINT [FK__Ausencia__IdModi__573DED66] FOREIGN KEY([IdModif])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Ausencia] CHECK CONSTRAINT [FK__Ausencia__IdModi__573DED66]
GO
ALTER TABLE [dbo].[Ausencia]  WITH CHECK ADD  CONSTRAINT [FK__Ausencia__IdSoli__546180BB] FOREIGN KEY([IdSolicitante])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Ausencia] CHECK CONSTRAINT [FK__Ausencia__IdSoli__546180BB]
GO
ALTER TABLE [dbo].[Contrato]  WITH CHECK ADD  CONSTRAINT [FK__Contrato__IdEmpl__08D548FA] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Contrato] CHECK CONSTRAINT [FK__Contrato__IdEmpl__08D548FA]
GO
ALTER TABLE [dbo].[Contrato]  WITH CHECK ADD  CONSTRAINT [FK__Contrato__IdModi__0BB1B5A5] FOREIGN KEY([IdModif])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Contrato] CHECK CONSTRAINT [FK__Contrato__IdModi__0BB1B5A5]
GO
ALTER TABLE [dbo].[Departamento]  WITH CHECK ADD  CONSTRAINT [FK__Departame__IdJef__5CF6C6BC] FOREIGN KEY([IdJefeDep])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Departamento] CHECK CONSTRAINT [FK__Departame__IdJef__5CF6C6BC]
GO
ALTER TABLE [dbo].[Departamento]  WITH CHECK ADD  CONSTRAINT [FK__Departame__IdMod__5DEAEAF5] FOREIGN KEY([IdModif])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Departamento] CHECK CONSTRAINT [FK__Departame__IdMod__5DEAEAF5]
GO
ALTER TABLE [dbo].[Empleado]  WITH CHECK ADD  CONSTRAINT [FK__Empleado__Estado__4707859D] FOREIGN KEY([EstadoE])
REFERENCES [dbo].[EnumEstadoEmpleado] ([IdEstadoE])
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK__Empleado__Estado__4707859D]
GO
ALTER TABLE [dbo].[Empleado]  WITH CHECK ADD  CONSTRAINT [FK__Empleado__IdDepa__5C02A283] FOREIGN KEY([IdDepartamento])
REFERENCES [dbo].[Departamento] ([IdDepartamento])
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK__Empleado__IdDepa__5C02A283]
GO
ALTER TABLE [dbo].[Empleado]  WITH CHECK ADD  CONSTRAINT [FK__Empleado__IdModi__47FBA9D6] FOREIGN KEY([IdModif])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK__Empleado__IdModi__47FBA9D6]
GO
ALTER TABLE [dbo].[Empleado]  WITH CHECK ADD  CONSTRAINT [FK__Empleado__Rol__46136164] FOREIGN KEY([Rol])
REFERENCES [dbo].[EnumTipoEmpleado] ([IdRol])
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK__Empleado__Rol__46136164]
GO
ALTER TABLE [dbo].[ParticipacionProyecto]  WITH CHECK ADD  CONSTRAINT [FK__Participa__IdEmp__153B1FDF] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[ParticipacionProyecto] CHECK CONSTRAINT [FK__Participa__IdEmp__153B1FDF]
GO
ALTER TABLE [dbo].[ParticipacionProyecto]  WITH CHECK ADD  CONSTRAINT [FK__Participa__IdMod__17236851] FOREIGN KEY([IdModif])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[ParticipacionProyecto] CHECK CONSTRAINT [FK__Participa__IdMod__17236851]
GO
ALTER TABLE [dbo].[ParticipacionProyecto]  WITH CHECK ADD FOREIGN KEY([IdProyecto])
REFERENCES [dbo].[Proyecto] ([IdProyecto])
GO
ALTER TABLE [dbo].[Proyecto]  WITH CHECK ADD  CONSTRAINT [FK__Proyecto__IdModi__125EB334] FOREIGN KEY([IdModif])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Proyecto] CHECK CONSTRAINT [FK__Proyecto__IdModi__125EB334]
GO
ALTER TABLE [dbo].[Proyecto]  WITH CHECK ADD FOREIGN KEY([Prioridad])
REFERENCES [dbo].[EnumPrioridad] ([IdPrioridad])
GO
