CREATE TABLE Machines (
    MachineId INT PRIMARY KEY IDENTITY(1,1),
    MachineName NVARCHAR(100) NOT NULL,
    Model NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NULL,
    OperatorId NVARCHAR(450) NOT NULL, -- Foreign key referencing ApplicationUser (AspNetUsers)

    CONSTRAINT FK_Machines_Operator FOREIGN KEY (OperatorId) REFERENCES AspNetUsers(Id)
);
