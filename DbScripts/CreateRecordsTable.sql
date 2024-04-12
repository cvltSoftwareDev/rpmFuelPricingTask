﻿CREATE TABLE [dbo].[Records]
(
    [ID] INT IDENTITY(1,1) PRIMARY KEY,
    [Period] DATETIME NOT NULL,
    [Price] DECIMAL(10, 3) NOT NULL
);