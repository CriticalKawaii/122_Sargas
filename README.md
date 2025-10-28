# ОТЧЁТ  
## по учебной практике

---
## Предментая область
**Учёт платежей**

---
### Специальность  
**09.02.07** — *Информационные системы и программирование*  

### Профессиональный модуль  
**ПМ.01** — *Разработка модулей программного обеспечения для компьютерных систем*  

### Междисциплинарный курс  
**МДК.01.02** — *Поддержка и тестирование программных модулей*  
---
### Выполнил:
Студент **4 курса**, группа **4ИСИП-122**  
**Саргас К.И.**


### Проверил:
Руководитель практики от Колледжа информатики и программирования  
Преподаватель ВКК, к.п.н. **Т.Г. Аксёнова**

---

## SQL-скрипт базы данных `edchenko_DB_Payment`

```sql
USE [master]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 20.10.2025 23:10:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 20.10.2025 23:10:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Num] [decimal](18, 0) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 20.10.2025 23:10:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Login] [varchar](max) NOT NULL,
	[Password] [varchar](max) NOT NULL,
	[Role] [varchar](50) NULL,
	[FIO] [varchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 
GO
INSERT [dbo].[Categories] ([ID], [Name]) VALUES (1, N'Налоги')
GO
INSERT [dbo].[Categories] ([ID], [Name]) VALUES (2, N'Закупка')
GO
INSERT [dbo].[Categories] ([ID], [Name]) VALUES (3, N'Личное')
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Payments] ON 
GO
INSERT [dbo].[Payments] ([ID], [UserID], [CategoryID], [Date], [Name], [Num], [Price]) VALUES (1, 1, 1, CAST(N'2021-01-01' AS Date), N'ЖКХ', CAST(1 AS Decimal(18, 0)), CAST(222 AS Decimal(18, 0)))
GO
INSERT [dbo].[Payments] ([ID], [UserID], [CategoryID], [Date], [Name], [Num], [Price]) VALUES (2, 5, 2, CAST(N'2025-01-01' AS Date), N'Закупки', CAST(2 AS Decimal(18, 0)), CAST(111 AS Decimal(18, 0)))
GO
INSERT [dbo].[Payments] ([ID], [UserID], [CategoryID], [Date], [Name], [Num], [Price]) VALUES (3, 2, 3, CAST(N'2001-01-01' AS Date), N'Покупка сока Хеннесси', CAST(999 AS Decimal(18, 0)), CAST(2220 AS Decimal(18, 0)))
GO
INSERT [dbo].[Payments] ([ID], [UserID], [CategoryID], [Date], [Name], [Num], [Price]) VALUES (4, 10, 3, CAST(N'2024-01-01' AS Date), N'Заказ с озона ', CAST(1 AS Decimal(18, 0)), CAST(222 AS Decimal(18, 0)))
GO
INSERT [dbo].[Payments] ([ID], [UserID], [CategoryID], [Date], [Name], [Num], [Price]) VALUES (5, 8, 1, CAST(N'1111-01-01' AS Date), N'Налог на воздух', CAST(1 AS Decimal(18, 0)), CAST(9999 AS Decimal(18, 0)))
GO
SET IDENTITY_INSERT [dbo].[Payments] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([ID], [Login], [Password], [Role], [FIO], [Photo]) VALUES (1, N'user1', N'B2E98AD6F6EB8508DD6A14CFA704BAD7F05F6FB1', N'User', N'user1', NULL)
GO
INSERT [dbo].[Users] ([ID], [Login], [Password], [Role], [FIO], [Photo]) VALUES (2, N'admin', N'B2E98AD6F6EB8508DD6A14CFA704BAD7F05F6FB1', N'Admin', N'admin', NULL)
GO
INSERT [dbo].[Users] ([ID], [Login], [Password], [Role], [FIO], [Photo]) VALUES (4, N'user', N'B2E98AD6F6EB8508DD6A14CFA704BAD7F05F6FB1', N'User', N'user ', NULL)
GO
INSERT [dbo].[Users] ([ID], [Login], [Password], [Role], [FIO], [Photo]) VALUES (5, N'user2', N'B2E98AD6F6EB8508DD6A14CFA704BAD7F05F6FB1', N'User', N'user2', NULL)
GO
INSERT [dbo].[Users] ([ID], [Login], [Password], [Role], [FIO], [Photo]) VALUES (6, N'admin1', N'B2E98AD6F6EB8508DD6A14CFA704BAD7F05F6FB1', N'Admin', N'admin1', NULL)
GO
INSERT [dbo].[Users] ([ID], [Login], [Password], [Role], [FIO], [Photo]) VALUES (7, N'admin2', N'B2E98AD6F6EB8508DD6A14CFA704BAD7F05F6FB1', N'Admin', N'admin2', NULL)
GO
INSERT [dbo].[Users] ([ID], [Login], [Password], [Role], [FIO], [Photo]) VALUES (8, N'user3', N'B2E98AD6F6EB8508DD6A14CFA704BAD7F05F6FB1', N'User', N'user3', NULL)
GO
INSERT [dbo].[Users] ([ID], [Login], [Password], [Role], [FIO], [Photo]) VALUES (9, N'user4', N'B2E98AD6F6EB8508DD6A14CFA704BAD7F05F6FB1', N'User', N'user4', NULL)
GO
INSERT [dbo].[Users] ([ID], [Login], [Password], [Role], [FIO], [Photo]) VALUES (10, N'user5', N'B2E98AD6F6EB8508DD6A14CFA704BAD7F05F6FB1', N'User', N'user5', NULL)
GO
INSERT [dbo].[Users] ([ID], [Login], [Password], [Role], [FIO], [Photo]) VALUES (11, N'user6', N'B2E98AD6F6EB8508DD6A14CFA704BAD7F05F6FB1', N'User', N'user6', NULL)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_Categories] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([ID])
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_Categories]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_Users]
GO

