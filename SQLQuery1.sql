CREATE TABLE ApplicationUsers (
    ApplicationUserId INT IDENTITY(1,1) PRIMARY KEY, -- A unique primary key for this table
    UserId NVARCHAR(450) NOT NULL, -- Foreign key to AspNetUsers table
    FirstName NVARCHAR(100) NULL,
    Address NVARCHAR(255) NULL,
    CONSTRAINT FK_ApplicationUsers_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);
