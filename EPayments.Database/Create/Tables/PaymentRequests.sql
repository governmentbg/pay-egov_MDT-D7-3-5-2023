﻿PRINT 'PaymentRequests'
GO

CREATE TABLE [dbo].[PaymentRequests](
    [PaymentRequestId]                      INT                 NOT NULL IDENTITY,
    [Gid]                                   UNIQUEIDENTIFIER    NOT NULL UNIQUE,
    [PaymentRequestXmlId]                   INT                 NOT NULL,
    [EserviceClientId]                      INT                 NOT NULL,
    [IsPaymentMultiple]                     BIT                 NOT NULL,
    [ServiceProviderName]                   NVARCHAR(MAX)       NOT NULL,
    [ServiceProviderBank]                   NVARCHAR(100)       NOT NULL,
    [ServiceProviderBIC]                    NVARCHAR(50)        NOT NULL,
    [ServiceProviderIBAN]                   NVARCHAR(50)        NOT NULL,
    [Currency]                              NVARCHAR(10)        NOT NULL,
    [PaymentTypeCode]                       NVARCHAR(10)        NULL,
    [PaymentAmount]                         DECIMAL(18, 4)      NOT NULL,
    [PaymentReason]                         NVARCHAR(200)       NOT NULL,
    [ApplicantUinTypeId]                    INT                 NOT NULL,
    [ApplicantUin]                          NVARCHAR(50)        NOT NULL,
    [ApplicantName]                         NVARCHAR(MAX)       NOT NULL,
    [PaymentReferenceType]                  NVARCHAR(50)        NULL,
    [PaymentReferenceNumber]                NVARCHAR(50)        NOT NULL,
    [PaymentReferenceDate]                  DATETIME2           NOT NULL,
    [ExpirationDate]                        DATETIME2           NOT NULL,
    [AdditionalInformation]                 NVARCHAR(MAX)       NULL,
    [AdministrativeServiceUri]              NVARCHAR(100)       NULL,
    [AdministrativeServiceSupplierUri]      NVARCHAR(100)       NULL,
    [AdministrativeServiceNotificationURL]  NVARCHAR(MAX)       NULL,
    [AisPaymentId]                          NVARCHAR(MAX)       NULL,
    [PaymentRequestIdentifier]              NVARCHAR(50)        NOT NULL UNIQUE,
    [PaymentRequestAccessCode]              NVARCHAR(10)        NULL,
    [IsTempRequest]                         BIT                 NOT NULL,
    [IsVposAuthorized]                      BIT                 NOT NULL,
    [VposResultId]                          INT                 NULL,
    [VposAuthorizationId]                   NVARCHAR(MAX)       NULL,
    [PaymentRequestStatusId]                INT                 NOT NULL,
    [PaidStatusPaymentMethodId]             INT                 NULL,
    [PaidStatusPaymentMethodDescription]    NVARCHAR(MAX)       NULL,
    [PaymentRequestStatusChangeTime]        DATETIME2           NOT NULL,
    [CreateDate]                            DATETIME2           NOT NULL,
CONSTRAINT [PK_PaymentRequests]                             PRIMARY KEY ([PaymentRequestId]),
CONSTRAINT [FK_PaymentRequests_PaymentRequestXmls]          FOREIGN KEY ([PaymentRequestXmlId])         REFERENCES [dbo].[PaymentRequestXmls] ([PaymentRequestXmlId]),
CONSTRAINT [FK_PaymentRequests_EserviceClients]             FOREIGN KEY ([EserviceClientId])            REFERENCES [dbo].[EserviceClients] ([EserviceClientId]),
CONSTRAINT [FK_PaymentRequests_PaymentRequestStatuses]      FOREIGN KEY ([PaymentRequestStatusId])      REFERENCES [dbo].[PaymentRequestStatuses] ([PaymentRequestStatusId]),
CONSTRAINT [FK_PaymentRequests_PaidStatusPaymentMethods]    FOREIGN KEY ([PaidStatusPaymentMethodId])   REFERENCES [dbo].[PaidStatusPaymentMethods] ([PaidStatusPaymentMethodId]),
CONSTRAINT [FK_PaymentRequests_UinTypes]                    FOREIGN KEY ([ApplicantUinTypeId])          REFERENCES [dbo].[UinTypes] ([UinTypeId])
);



