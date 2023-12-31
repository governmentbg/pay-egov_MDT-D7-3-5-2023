﻿CREATE TABLE [dbo].[Banks](
	[BankId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[BIC] [nvarchar](50) NOT NULL,
	[CertificateId] [int] NULL,
 CONSTRAINT [PK_Banks] PRIMARY KEY CLUSTERED 
(
	[BankId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]