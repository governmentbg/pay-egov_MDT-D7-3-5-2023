﻿PRINT 'Insert [EserviceClients]'
GO

SET IDENTITY_INSERT [dbo].[EserviceClients] ON 
GO

--Production ECSCS Vpos athorization data:
--DskVposMerchantId='a84aef4b68fd11e5ab17005056b679e2',
--DskVposMerchantPassword='vps1bbty2'
INSERT [dbo].[EserviceClients] ([EserviceClientId], [VposClientId], [Alias], [ClientId], [AisName], [ServiceName], [ServiceDescription], [AccountBank], [AccountBIC], [AccountIBAN], [SecretKey], [IsActive], [DskVposMerchantId], [DskVposMerchantPassword], [Gid], [IsAuthPassAuthorized], [DepartmentId], [IsEpayVposEnabled])
VALUES (1, 1, N'Ecscs', N'ecscs_86b6db8a-e9fd-43a9-b299-ac5fb9ba055b', N'ИС "Електронно свидетелство за съдимост"', N'Издаване на електронно свидетелство за съдимост', NULL, 'Банка ДСК', 'STSABGSF', 'BG94RZBB91551060362319', N'DB6C6A50FEBE00ED5B2755B7BBC388E812418C2439EC5DC71A8786F01288642D513F41096E4458502E15BCED6BE5DA450A019895F6D1C2AA8E960CC88A2DCCDA', 1, N'90000003', N'test3test3', 'dd3652ce-af7d-4221-901f-a72d539cd8d2', 1, 1, 1)

INSERT [dbo].[EserviceClients] ([EserviceClientId], [VposClientId], [Alias], [ClientId], [AisName], [ServiceName], [ServiceDescription], [AccountBank], [AccountBIC], [AccountIBAN], [SecretKey], [IsActive], [DskVposMerchantId], [DskVposMerchantPassword], [Gid], [IsAuthPassAuthorized], BoricaVposTerminalId, BoricaVposMerchantId, BoricaVposRequestSignCertFileName, BoricaVposRequestSignCertPassword, BoricaVposRequestSignCertValidTo, [DepartmentId], [IsEpayVposEnabled])
VALUES (2, 2, N'Rta', N'rta_e0d07219-2cfc-4d5e-9d17-b777b7992422', N'Портал на изпълнителна агенция "Автомобилна администрация"', N'Промяна на списъците към разрешенията за обучение за придобиване на правоспособност за управление на МПС', NULL, 'УниКредит Булбанк', 'UNCRBGSF', 'BG94RZBB91551060362319', N'SDFKLGHLRTYJH467345LKJDFVLEKJ578LEKJLK24L5JK7LM45E7KLM4356H23KH7JKL3647JKJKLH45KJLH6376782A2136NMCNHAWPASDIJETLVSKDJPWRTYEPYPASW', 1, NULL, NULL, 'bbac2475-bb65-4d27-ba7f-440180d3363c', 0, N'62160735', N'6210015673', N'RTA_ePaymentsRequestSign_Test.p12', '', '2030-11-25', 3, 1)

INSERT [dbo].[EserviceClients] ([EserviceClientId], [VposClientId], [Alias], [ClientId], [AisName], [ServiceName], [ServiceDescription], [AccountBank], [AccountBIC], [AccountIBAN], [SecretKey], [IsActive], [DskVposMerchantId], [DskVposMerchantPassword], [Gid], [IsAuthPassAuthorized], BoricaVposTerminalId, BoricaVposMerchantId, BoricaVposRequestSignCertFileName, BoricaVposRequestSignCertPassword, BoricaVposRequestSignCertValidTo, [DepartmentId], [IsEpayVposEnabled])
VALUES (3, 2, N'Nacid', N'nacid_f1417e88-519a-4741-9adf-1496b798403e', N'Портал за електронни административни услуги на "НАЦИД"', N'Издаване на служебна бележка ', N'Издаване на служебна бележка за завършени научноизследователски проекти.' + char(13) + char(10) + N'Издаване на служебна бележка за депозиран научен ръкопис.' + char(13) + char(10) + N'Издаване на служебна бележка за защитен дисертационен труд.' + char(13) + char(10) + N'Издаване на служебна бележка за заета академична длъжност.', 'УниКредит Булбанк', 'UNCRBGSF', 'BG97UNCR96603119995010', N'AEIRJBVM4673MKSDLKMPETNSIPM74H789SCDWMVBN782F34564DAQWEERERMBVD95IAPJTRRRYUUIIGVBN34566UDFBUHERIJHSXCKMV234345634LKSDFGJRTYIJ4J5', 1, NULL, NULL, 'ef2cea14-fb93-4c4c-a059-d3ae61857285', 0, N'62160737', N'6210015685', N'NACID_ePaymentsRequestSign_Test.p12', '', '2030-12-02', 2, 1)

INSERT [dbo].[EserviceClients] ([EserviceClientId], [VposClientId], [Alias], [ClientId], [AisName], [ServiceName], [ServiceDescription], [AccountBank], [AccountBIC], [AccountIBAN], [SecretKey], [IsActive], [DskVposMerchantId], [DskVposMerchantPassword], [Gid], [IsAuthPassAuthorized], [DepartmentId], [IsEpayVposEnabled])
VALUES (4, 1, N'ArchimedLovech', N'archimed_eProcess_Lovech_e39460e1-ce74-48f9-845f-3703d0781b64', N'ИС "Archimed eProcess", гр. Ловеч', N'', NULL, 'Банка ДСК', 'STSABGSF', 'BG94RZBB91551060362319', N'SKLDJFHLERTJKSJH76WTUY23894690XM345SDaMCERNJ4WEYUCCVNNKPWEUXCNV5MBNSRKJDTHEKJFDVHUKHEJ276489CVVZGFWURTOTONXCVMNSRUH569034NNXXMNC', 1, N'ffb2070bd0bb11e58734005056b679e2', N'vpst34tcrdpmnt18', '7e74b8b0-9f5a-4889-86a8-ee522121021a', 0, 4, 1)

INSERT [dbo].[EserviceClients] ([EserviceClientId], [VposClientId], [Alias], [ClientId], [AisName], [ServiceName], [ServiceDescription], [AccountBank], [AccountBIC], [AccountIBAN], [SecretKey], [IsActive], [DskVposMerchantId], [DskVposMerchantPassword], [Gid], [IsAuthPassAuthorized], [DepartmentId], [IsEpayVposEnabled])
VALUES (5, 1, N'ArchimedStaraZagora', N'archimed_eProcess_StaraZagora_47adf7ba-2843-4097-8208-6aed116d1f55', N'ИС "Archimed eProcess", гр. Стара Загора', N'', NULL, 'Банка ДСК', 'STSABGSF', 'BG94RZBB91551060362319', N'ASD567DFGJH54K62323UYSDFVNXCVTYOPOIWYGHSKLYUIBNCFGTI27HHHCVBASDSFIKU32ZMKLPIASFRQSDHGZXCBHSJKHG234BNKBVXKJHFKDJHJ234628CBSHD52CV', 1, N'55b801d3d0bb11e58734005056b679e2', N'st34asumvur6dfs', 'a4f6001a-5465-44e1-be72-1847c4b386fc', 0, 4, 1)

INSERT [dbo].[EserviceClients] ([EserviceClientId], [VposClientId], [Alias], [ClientId], [AisName], [ServiceName], [ServiceDescription], [AccountBank], [AccountBIC], [AccountIBAN], [SecretKey], [IsActive], [DskVposMerchantId], [DskVposMerchantPassword], [Gid], [IsAuthPassAuthorized], [DepartmentId], [IsEpayVposEnabled])
VALUES (6, 1, N'ArchimedHaskovo', N'archimed_eProcess_Haskovo_3add2798-8bde-4ac9-bc55-eeb46e6e5a63', N'ИС "Archimed eProcess", гр. Хасково', N'', NULL, 'Банка ДСК', 'STSABGSF', 'BG94RZBB91551060362319', N'YYTURTGCVBDY467DFNZXCNWEIU2UHDFVXCMBSKDFG34MNXBCSGERKERHERUKTH958ZGHASFDLSDHKLPETIYJHXCXBVGTOXCHSDFJ234456MXCVHNKSFGIPORUZNBNFU3', 1, N'b79d0933d0bb11e58734005056b679e2', N'hger457dfghhtr56g', 'f3702984-8b1d-42ea-9249-7865d5c47872', 0, 4, 1)

INSERT [dbo].[EserviceClients] ([EserviceClientId], [VposClientId], [Alias], [ClientId], [AisName], [ServiceName], [ServiceDescription], [AccountBank], [AccountBIC], [AccountIBAN], [SecretKey], [IsActive], [DskVposMerchantId], [DskVposMerchantPassword], [Gid], [IsAuthPassAuthorized], [DepartmentId], [IsEpayVposEnabled])
VALUES (7, 1, N'NsiInfostat', N'nsi_infostat_af42a889-0621-4a27-975d-51516ff8b7c3', N'ИС "Инфостат" НСИ', N'', NULL, 'Банка ДСК', 'STSABGSF', 'BG94RZBB91551060362319', N'S23LPNVUUPTYASBK9UQ5Y1Z79UEXD4PZIEHDQXEYZHIYP7YABUZQS1X98D2MAQTP6TGYNUPT647HZ4BGYQKITTSJGCEBBEC5EYA67KBM11REXVCNE52GUM27ZYM1MC98', 1, N'2ee50607d0bc11e58734005056b679e2', N'vrtlps1txmcnsrc16', '08bf5c16-5b65-475b-a8fe-f6dc89f7c62e', 0, 5, 1)

INSERT [dbo].[EserviceClients] ([EserviceClientId], [Gid], [Alias], [AisName], [ServiceName], [ServiceDescription], [DepartmentId], [AccountBank], [AccountBIC], [AccountIBAN], [ClientId], [SecretKey], [VposClientId], [DskVposMerchantId], [DskVposMerchantPassword], [BoricaVposTerminalId], [BoricaVposMerchantId], [BoricaVposRequestSignCertFileName], [BoricaVposRequestSignCertPassword], [BoricaVposRequestSignCertValidTo], [BoricaVposRequestSignCertExpMailSend], [IsAuthPassAuthorized], [IsActive], [IsEpayVposEnabled])
VALUES (8, N'd1f5a248-7fd1-40db-953c-6c5fa68543d4', N'Bim', N'Деловодна система БИМ', N'', NULL, 6, N'УниКредит Булбанк', N'UNCRBGSF', N'BG94RZBB91551060362319', N'bim_d271ea7f-9e44-44bb-878e-7e2c4cd697f0', N'OTUSXVELIK23ZCVGIUPQQWEBKLGHWER76923486UYYQXCNI7112VTDJOIJDFGJH65893UJHSGH743JKB9B58GYBPLGMZX453XCZ795234563345CASCSZDSDFBDFG2DF', 2, NULL, NULL, N'62160816', N'6210016755', N'BIM_ePaymentsRequestSign_Test.p12', N'', CAST(N'2030-05-11 00:00:00.0000000' AS DateTime2), NULL, 0, 1, 1)
GO

INSERT [dbo].[EserviceClients] ([EserviceClientId], [Gid], [Alias], [AisName], [ServiceName], [ServiceDescription], [DepartmentId], [AccountBank], [AccountBIC], [AccountIBAN], [ClientId], [SecretKey], [VposClientId], [DskVposMerchantId], [DskVposMerchantPassword], [BoricaVposTerminalId], [BoricaVposMerchantId], [BoricaVposRequestSignCertFileName], [BoricaVposRequestSignCertPassword], [BoricaVposRequestSignCertValidTo], [BoricaVposRequestSignCertExpMailSend], [IsAuthPassAuthorized], [IsActive], [IsEpayVposEnabled])
VALUES (9, N'a5f7c828-8af4-46d3-ad4a-0bdcddd8bfe5', N'SirmaCloudServiceRadomir', N'Облачна платформа за Общински електронни услуги - Радомир', N'', NULL, 7, N'Банка ДСК', N'STSABGSF', N'BG94RZBB91551060362319', N'sirma_cloudService_Radomir_86507198-9a2e-4194-9bc1-5abf40ed4c8c', N'ASDMBWU457NMZCYTIONNQU6934H6HNCJYOEY99MSSDFSDFAAQQIROO2XXVUTTPPASDJFBNRXCVUITPL73NBX12ERXOFGTLX342GHCVBTYI4OAAFGHOTYRHCV4568323B', 1, N'8e42d54ef4da11e5b823005056b679e2', N'mmibb22psttrm34a', NULL, NULL, NULL, NULL, NULL, NULL, 0, 1, 1)
GO

INSERT [dbo].[EserviceClients] ([EserviceClientId], [Gid], [Alias], [AisName], [ServiceName], [ServiceDescription], [DepartmentId], [AccountBank], [AccountBIC], [AccountIBAN], [ClientId], [SecretKey], [VposClientId], [DskVposMerchantId], [DskVposMerchantPassword], [BoricaVposTerminalId], [BoricaVposMerchantId], [BoricaVposRequestSignCertFileName], [BoricaVposRequestSignCertPassword], [BoricaVposRequestSignCertValidTo], [BoricaVposRequestSignCertExpMailSend], [IsAuthPassAuthorized], [IsActive], [IsEpayVposEnabled])
VALUES (10, N'39e38d03-e85f-4c4d-9db1-e6c8f5562641', N'SirmaCloudServiceGabrovo', N'Облачна платформа за Общински електронни услуги - Габрово', N'', NULL, 8, N'Банка ДСК', N'STSABGSF', N'BG94RZBB91551060362319', N'sirma_cloudService_Gabrovo_5adfdae3-50bb-43b3-ac49-af683b12ea28', N'PPTYBN365XCVSDLKFJ234UIERJGDJKKKLSLLL40ZNCMP34549SQ13YEIOJUJCNXGRIO5689HHH1234540XNBNTUYUIUIPILMZDAJAHER45RTFBJRDKUSDFUWZOARRTTT', 1, N'a3055257523d11e6aced005056b679e2', N'ImeonWeb4Gabrovo', NULL, NULL, NULL, NULL, NULL, NULL, 0, 1, 1)
GO

INSERT [dbo].[EserviceClients] ([EserviceClientId], [Gid], [Alias], [AisName], [ServiceName], [ServiceDescription], [DepartmentId], [AccountBank], [AccountBIC], [AccountIBAN], [ClientId], [SecretKey], [VposClientId], [DskVposMerchantId], [DskVposMerchantPassword], [BoricaVposTerminalId], [BoricaVposMerchantId], [BoricaVposRequestSignCertFileName], [BoricaVposRequestSignCertPassword], [BoricaVposRequestSignCertValidTo], [BoricaVposRequestSignCertExpMailSend], [IsAuthPassAuthorized], [IsActive], [IsEpayVposEnabled])
VALUES (11, N'ca8fe422-2bbe-4e26-a7e1-d6c83a1835e3', N'SirmaCloudServiceBurgas', N'Облачна платформа за Общински електронни услуги - Бургас', N'', NULL, 9, N'Общинска Банка', N'SOMBBGSF', N'BG94RZBB91551060362319', N'sirma_cloudService_Burgas_3e026fc9-ad7f-473b-87d5-0209e62f776b', N'TUCMKNJKOPUOSIZQT5689VNXBVGGTRRECR737285OUPLMMMVNVNG827579SHSHZVRQRYOHSOTNQDPIOUIONMH86395689KJDK160953TYGHFHKJMBNCBDGR344655SDF', 2, NULL, NULL, N'13000043', N'1300000040', N'SirmaCloudService_Burgas_ePaymentsRequestSign_Test.p12', N'', CAST(N'2030-05-11 00:00:00.0000000' AS DateTime2), NULL, 0, 1, 1)
GO

INSERT [dbo].[EserviceClients] ([EserviceClientId], [Gid], [Alias], [AisName], [ServiceName], [ServiceDescription], [DepartmentId], [AccountBank], [AccountBIC], [AccountIBAN], [ClientId], [SecretKey], [VposClientId], [DskVposMerchantId], [DskVposMerchantPassword], [BoricaVposTerminalId], [BoricaVposMerchantId], [BoricaVposRequestSignCertFileName], [BoricaVposRequestSignCertPassword], [BoricaVposRequestSignCertValidTo], [BoricaVposRequestSignCertExpMailSend], [IsAuthPassAuthorized], [IsActive], [IsEpayVposEnabled]) 
VALUES (101, N'71f1199e-fa51-433c-8dda-e0dee07c3a3e', N'TestAisClientAlias', N'Тестов АИС', N'Тестовo електронна услуга', NULL, 99, N'Тестова УниKредит Булбанк', N'ТестBIC', N'ТестIBAN', N'testAisClient', N'8F70C29ACBB38F39B0900C26B3A20B0683E62C97A4E578358904686D0023988D0C9873AD23EE87003B36EE5221617FEC0345E3B1138FE1B57EF5DE4771E3CF42', 1, N'90000003', N'test3test3', NULL, NULL, NULL, NULL, NULL, NULL, 0, 1, 1)
GO

INSERT [dbo].[EserviceClients] ([EserviceClientId], [Gid], [Alias], [AisName], [ServiceName], [ServiceDescription], [DepartmentId], [AccountBank], [AccountBIC], [AccountIBAN], [ClientId], [SecretKey], [VposClientId], [DskVposMerchantId], [DskVposMerchantPassword], [BoricaVposTerminalId], [BoricaVposMerchantId], [BoricaVposRequestSignCertFileName], [BoricaVposRequestSignCertPassword], [BoricaVposRequestSignCertValidTo], [BoricaVposRequestSignCertExpMailSend], [IsAuthPassAuthorized], [IsActive], [IsEpayVposEnabled]) 
VALUES (102, N'273df0a4-dac7-4720-8d75-017a6f51af2b', N'TestAisClientAlias_Borica', N'Тестов АИС_Borica', N'Тестовo електронна услуга_Borica', NULL, 99, N'Тестова Банка Уникредит', N'ТестBIC_Уни', N'ТестIBAN_Уни', N'testAisClient_Borica', N'KLJSHLK45H6LK56J7234LKJDFLKBM5P0789IKFGBMP4ASYL357893278CVBCVCNSDFSFGYFU76834RBSDZSADASUIPEVXT85MOP23456CBERERTYRTUPN2136MBNCYYK', 2, NULL, NULL, N'62160735', N'6210015673', N'RTA_ePaymentsRequestSign_Test.p12', N'', CAST(N'2030-11-25T00:00:00.0000000' AS DateTime2), NULL, 0, 1, 1)
GO

INSERT [dbo].[EserviceClients] ([EserviceClientId], [Gid], [Alias], [AisName], [ServiceName], [ServiceDescription], [DepartmentId], [AccountBank], [AccountBIC], [AccountIBAN], [ClientId], [SecretKey], [VposClientId], [DskVposMerchantId], [DskVposMerchantPassword], [BoricaVposTerminalId], [BoricaVposMerchantId], [BoricaVposRequestSignCertFileName], [BoricaVposRequestSignCertPassword], [BoricaVposRequestSignCertValidTo], [BoricaVposRequestSignCertExpMailSend], [IsAuthPassAuthorized], [IsActive], [IsEpayVposEnabled]) 
VALUES (103, N'579d8683-4678-4709-833b-88c396d28e80', N'AIS_RUSE', N'ИС "Съд Русе"', N'', NULL, 99, N'Уникредит Булбанк', N'UNCRBGSF', N'UNCFTYUI76HJ65FG', N'ais_ruse_e4a006ba-e1db-45f0-a05b-24866587fae1', N'392154C938344295949EB4D1D4ACF4ABF48169040C7C452F83E3D7FB263AE993F6DD0A74534F4650B6661EC69204DA311EF7A465FB564A83BA8CBD286AD51887', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 1, 1)
GO

INSERT [dbo].[EserviceClients] ([EserviceClientId], [Gid], [Alias], [AisName], [ServiceName], [ServiceDescription], [DepartmentId], [AccountBank], [AccountBIC], [AccountIBAN], [ClientId], [SecretKey], [VposClientId], [DskVposMerchantId], [DskVposMerchantPassword], [BoricaVposTerminalId], [BoricaVposMerchantId], [BoricaVposRequestSignCertFileName], [BoricaVposRequestSignCertPassword], [BoricaVposRequestSignCertValidTo], [BoricaVposRequestSignCertExpMailSend], [IsAuthPassAuthorized], [IsActive], [IsEpayVposEnabled]) 
VALUES (105, N'fdc2a3c3-f9d5-4daf-b67c-64e6cd4abf84', N'Eforms', N'ИС "Е-Форми"', N'', NULL, 99, N'Българска народна банка', N'BNBGBGSD', N'BG93BNBG96613000142701', N'eforms_a0cb5d01-4724-4cd3-bca7-10f14b20b91d', N'GJLEJ56DFKLGJRKLJY4J34ILRFSDKVCJ4I659QDFVIK345T9QLPSDKFLP76QLBMKCBXFG3557TSUFYBTY5Q978IAHVNJEU49TYUSJ2U34J532333RARARRJ659Q79Q7Z', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 1, 1)
GO

INSERT [dbo].[EserviceClients] ([EserviceClientId], [Gid], [Alias], [AisName], [ServiceName], [ServiceDescription], [DepartmentId], [AccountBank], [AccountBIC], [AccountIBAN], [ClientId], [SecretKey], [VposClientId], [DskVposMerchantId], [DskVposMerchantPassword], [BoricaVposTerminalId], [BoricaVposMerchantId], [BoricaVposRequestSignCertFileName], [BoricaVposRequestSignCertPassword], [BoricaVposRequestSignCertValidTo], [BoricaVposRequestSignCertExpMailSend], [IsAuthPassAuthorized], [IsActive], [IsEpayVposEnabled]) 
VALUES (106, N'bfbe6ea7-429c-4783-84d2-e014f155cd41', N'EformsVidin', N'ИС "Е-Форми Видин"', N'', NULL, 99, N'Търговска банка', N'DEMIBGSF', N'BG44DEMI92408400046021', N'eforms_vidin_89416c10-3aa0-4fc9-8d7d-ca0c891f7ce4', N'IYPWPZMVMCMAQEQWEQZXCZXCZXSDFSDFPPCN34345NSDC257699XXCVUJITUIMVNK99ICHKUCYKGLVUJFU583A23423423423ZXCZXC235A7E769DYGKNJCYURVBS856', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 1, 1)
GO

INSERT [dbo].[EserviceClients] ([EserviceClientId], [Gid], [Alias], [AisName], [ServiceName], [ServiceDescription], [DepartmentId], [AccountBank], [AccountBIC], [AccountIBAN], [ClientId], [SecretKey], [VposClientId], [DskVposMerchantId], [DskVposMerchantPassword], [BoricaVposTerminalId], [BoricaVposMerchantId], [BoricaVposRequestSignCertFileName], [BoricaVposRequestSignCertPassword], [BoricaVposRequestSignCertValidTo], [BoricaVposRequestSignCertExpMailSend], [IsAuthPassAuthorized], [IsActive], [IsEpayVposEnabled]) 
VALUES (107, N'52f349dd-3b60-4e64-a04b-222429cce0dd', N'ИС "eFormsStrumyani', N'ИС "Еформи - Струмяни', N'Община Струмяни', NULL, 99, N'Интернешънъл Асет Банк АД', N'IABGBGSF', N'UNCFTYUI76HJ65FG', N'ais_strumyani_d6bc5494-03dd-4056-b31b-04a87c25fd04', N'3B120E41F3224CE08FE206CFE0B21C5D47BD0AEAC1DF437889736602448F1DCEE9823FCB4FE748F1925A6909D2430EBE6CE80B750B35435AA6C278A7DD7D0256', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 1, 1)
GO

INSERT [dbo].[EserviceClients] ([EserviceClientId], [Gid], [Alias], [AisName], [ServiceName], [ServiceDescription], [DepartmentId], [AccountBank], [AccountBIC], [AccountIBAN], [ClientId], [SecretKey], [VposClientId], [DskVposMerchantId], [DskVposMerchantPassword], [BoricaVposTerminalId], [BoricaVposMerchantId], [BoricaVposRequestSignCertFileName], [BoricaVposRequestSignCertPassword], [BoricaVposRequestSignCertValidTo], [BoricaVposRequestSignCertExpMailSend], [IsAuthPassAuthorized], [IsActive], [IsEpayVposEnabled]) 
VALUES (109, N'6a658673-0cff-4724-8514-62352ff3b633', N'ИС "Министерство на туризма', N'ИС "Еформи - Министество на туризма', N'Министество на туризма', NULL, 99, N'Банка ДСК ЕАД    ', N'STSABGSF', N'BG02STSA93003157335601', N'ais_stru_6044b3a1-dd57-4ed4-8ada-0d45c316fca8', N'5306682707A34858ABF600CD9B8E315995B8ADEB531E40B1A223D72C05D59F1371823B1D93304090B1B7083010E6FB9DBD1305E07087463F90F69346300B5A22', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 1, 1)
GO

INSERT [dbo].[EserviceClients] ([EserviceClientId], [Gid], [Alias], [AisName], [ServiceName], [ServiceDescription], [DepartmentId], [AccountBank], [AccountBIC], [AccountIBAN], [ClientId], [SecretKey], [VposClientId], [DskVposMerchantId], [DskVposMerchantPassword], [BoricaVposTerminalId], [BoricaVposMerchantId], [BoricaVposRequestSignCertFileName], [BoricaVposRequestSignCertPassword], [BoricaVposRequestSignCertValidTo], [BoricaVposRequestSignCertExpMailSend], [IsAuthPassAuthorized], [IsActive], [IsEpayVposEnabled]) 
VALUES (111, N'293f3652-d3fb-4ca4-b042-0b11282f303e', N'bnra', N'Агенция за ядрено регулиране', N'Агенция за ядрено регулиране', NULL, 1, N'Тестова Банка', N'TB001122', N'BG00TBTB00112233445566', N'epayments_ais_client_293f3652-d3fb-4ca4-b042-0b11282f303e', N'SGY2TNQD3TMRPW3WY8BUVR46VZLV4AZNUHVPZVHR3QPP56SGYKAWC6R7EQ2PB5YJN5UTV1LK2KJZ48AQQHVUHJ1LCFB11VYEUU13MD76J64KLYBLF8E2QK1G5S2NVHLL', 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, 1)
GO

INSERT [dbo].[EserviceClients] ([EserviceClientId], [Gid], [Alias], [AisName], [ServiceName], [ServiceDescription], [DepartmentId], [AccountBank], [AccountBIC], [AccountIBAN], [ClientId], [SecretKey], [VposClientId], [DskVposMerchantId], [DskVposMerchantPassword], [BoricaVposTerminalId], [BoricaVposMerchantId], [BoricaVposRequestSignCertFileName], [BoricaVposRequestSignCertPassword], [BoricaVposRequestSignCertValidTo], [BoricaVposRequestSignCertExpMailSend], [IsAuthPassAuthorized], [IsActive], [IsEpayVposEnabled]) 
VALUES (112, N'f681003a-e41f-4a7f-9130-93b9a6d893a2', N'marad', N'Изпълнителна агенция „Морска администрация“', N'Изпълнителна агенция „Морска администрация“', NULL, 1, N'Банка ДСК', N'STSABGSF', N'BG94RZBB91551060362319', N'epayments_ais_client_f681003a-e41f-4a7f-9130-93b9a6d893a2', N'ZN6BTPR5I1JYIS8UUYF7KPW23KHQA64SX6V7318PQEVZ2INRTD7RUNLL7NUU6FS5XALC7U2NER6AYNQR5E4IA6FRM1F4Q1BAIWSEL3DRXP4FIN7NL28XY1TEWVWA2WFL', 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, 1)
GO

INSERT [dbo].[EserviceClients] ([EserviceClientId], [Gid], [Alias], [AisName], [ServiceName], [ServiceDescription], [DepartmentId], [AccountBank], [AccountBIC], [AccountIBAN], [ClientId], [SecretKey], [VposClientId], [DskVposMerchantId], [DskVposMerchantPassword], [BoricaVposTerminalId], [BoricaVposMerchantId], [BoricaVposRequestSignCertFileName], [BoricaVposRequestSignCertPassword], [BoricaVposRequestSignCertValidTo], [BoricaVposRequestSignCertExpMailSend], [IsAuthPassAuthorized], [IsActive], [IsEpayVposEnabled]) 
VALUES (114, N'1400a6c3-1cc4-4946-a0c7-0d25c86c231d', N'nrz', N'Национален регистър на запорите', N'Национален регистър на запорите', NULL, 1, N'Тестова Банка', N'TB001122', N'BG00TBTB00112233445566', N'epayments_ais_client_1400a6c3-1cc4-4946-a0c7-0d25c86c231d', N'JKYKBB6Y1CMKXI1Y16E67785TYINQT4KX5H8BA1U1E4NVHY71R83RUFAUL28C1ERP7T1LNFGTQXI6U5XYAS5NLSURG8D6G4YLDYK7YSKI7GW1SLN2W5STXYWBQQN1P8K', 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, 1)
GO

INSERT [dbo].[EserviceClients] ([EserviceClientId], [Gid], [Alias], [AisName], [ServiceName], [ServiceDescription], [DepartmentId], [AccountBank], [AccountBIC], [AccountIBAN], [ClientId], [SecretKey], [VposClientId], [DskVposMerchantId], [DskVposMerchantPassword], [BoricaVposTerminalId], [BoricaVposMerchantId], [BoricaVposRequestSignCertFileName], [BoricaVposRequestSignCertPassword], [BoricaVposRequestSignCertValidTo], [BoricaVposRequestSignCertExpMailSend], [IsAuthPassAuthorized], [IsActive], [IsEpayVposEnabled]) 
VALUES (116, N'6177e8fb-d693-4e4a-a0c8-6d097d5b0d27', N'HCL Leap', N'HCLLeap', N'HCLLeap', NULL, 1, N'Тестова Банка', N'TB001122', N'BG00TBTB00112233445566', N'epayments_ais_client_6177e8fb-d693-4e4a-a0c8-6d097d5b0d27', N'GVKF7AZ2FEILLFSLA8SPIYQ312YGITKVDFKSD8KFAR5W7UX3FX2CYVBPEHGTA1EQFGRBHLA1NJ8X7DDEXIYD4K6PH6UQMEV6AUSDYH5FM1IU8T5CDYLSUB4YVKAN48LH', 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, 1)
GO

SET IDENTITY_INSERT [dbo].[EserviceClients] OFF
GO
