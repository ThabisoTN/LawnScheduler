CREATE TABLE Bookings (
    BookingId INT PRIMARY KEY IDENTITY(1,1),
    MachineId INT NOT NULL,
    CustomerId NVARCHAR(450) NOT NULL,
    BookingDate DATETIME NOT NULL,    -- Date when the booking is made
    ScheduledDate DATETIME NOT NULL,  -- Date for which the machine is scheduled
    EndDate DATETIME NULL,
    IsConfirmed BIT NOT NULL DEFAULT 0, -- Marks if the booking is confirmed
    IsConflict BIT NOT NULL DEFAULT 0,  -- Marks if the booking has a conflict
    CONSTRAINT FK_Booking_Machine FOREIGN KEY (MachineId) REFERENCES Machines(MachineId),
    CONSTRAINT FK_Booking_Customer FOREIGN KEY (CustomerId) REFERENCES AspNetUsers(Id)
);
