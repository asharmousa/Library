create database LibraryManagement ;
use LibraryManagement ;

create table UserTable(
UserId int PRIMARY KEY IDENTITY(1,1),
First_Name varchar(250) not null,
Last_Name varchar(250) not null,
Role varchar(250),
Email varchar(250) unique not null , 
constraint ck_Stud_EmailDomain check (Email like '%@ses.yu.edu.jo'),
Password varchar(100) not null ,
--ConfirmPassword varchar(100),
Phone_Number varchar(10),
Profile_Picture varchar(max), 
Stud_Status varchar(50) not null, 
On_BlackList Bit DEFAULT 0
);


CREATE TABLE Book (
    Book_Id INT PRIMARY KEY IDENTITY(1,1),
    Book_Title VARCHAR(250) NOT NULL,
    Book_Author VARCHAR(250) NOT NULL,
    Book_Category VARCHAR(250),
    Book_Status VARCHAR(50) NOT NULL, -- e.g., 'Available', 'Borrowed'
    UserId INT, -- must match UserTable datatype
    Borrow_Date DATE,
    Due_Date DATE,
    Return_Date DATE,
    IsBorrowed BIT DEFAULT 0,
    CONSTRAINT FK_Book_User FOREIGN KEY (UserId) REFERENCES UserTable(UserId)
);
INSERT INTO Book (
    Book_Title,
    Book_Author,
    Book_Category,
    Book_Status,
    UserId,
    Borrow_Date,
    Due_Date,
    Return_Date,
    IsBorrowed
)
VALUES (
    'Clean Code',
    'Robert C. Martin',
    'Programming',
    'Available',
    NULL,          -- no user yet, since it's not borrowed
    NULL,          -- borrow date empty
    NULL,          -- due date empty
    NULL,          -- return date empty
    0              -- not borrowed
);

create table Room(
Room_Id int primary key identity (1,1),
Room_Name varchar(50) not null ,
Capacity varchar(50),
UserId int not null ,
foreign key (UserId) references UserTable (UserId), 
Reservation_Date date , 
Reservation_Duration int ,
Start_Time time not null ,
--End_time as dateadd( hour , 2 , Start_Time ),
Reservation_Status varchar(50) not null,
Room_Status varchar(50) not null,
IsReserved BIT DEFAULT 0
);

ALTER TABLE Room DROP COLUMN End_Time;

ALTER TABLE Room
ALTER COLUMN Start_Time TIME NULL;

ALTER TABLE Room
ADD End_Time TIME NULL;

INSERT INTO Room (Room_Name, Capacity, UserId, Reservation_Date, Reservation_Duration, Start_Time, Reservation_Status, Room_Status, IsReserved)
VALUES 
('Conference Room A', '10', 1, NULL, 2, NULL, 'Available', 'Available', 0),

('Meeting Room B', '6', 3, NULL, 2, NULL, 'Available', 'Available', 0),

('Training Room C', '20', 3, '2026-02-21', 2, '14:00', 'Reserved', 'Reserved', 1),

('Meeting Room C', '8', 3, NULL, 2, NULL, 'Available', 'Available', 0);

create table Feedback(
Feedback_Id int primary key identity (1,1) ,
UserId int not null ,
foreign key (UserId) references UserTable (UserId), 
Message varchar(1500),
Feedback_Type varchar(50),
Rating int check (Rating between 1 and 5) , 
Submitted_Date date 
);

ALTER TABLE Feedback 
ADD IsApproved BIT NOT NULL DEFAULT 0;







--ALTER TABLE Feedback
--ADD CONSTRAINT FK_Feedback_User_FirstName
--    FOREIGN KEY (First_Name) REFERENCES UserTable(First_Name);

--ALTER TABLE Feedback
--ADD CONSTRAINT FK_Feedback_User_LastName
 --   FOREIGN KEY (Last_Name) REFERENCES UserTable(Last_Name);
