
    if exists (select * from dbo.sysobjects where id = object_id(N'[Menu]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Menu]

    if exists (select * from dbo.sysobjects where id = object_id(N'[User]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [User]

    create table [Menu] (
        ID UNIQUEIDENTIFIER not null,
       Controller NVARCHAR(255) null,
       Action NVARCHAR(255) null,
       IsAdministration BIT null,
       Name NVARCHAR(255) null,
       primary key (ID)
    )

    create table [User] (
        ID UNIQUEIDENTIFIER not null,
       Email NVARCHAR(255) null,
       UserID NVARCHAR(255) null,
       Password NVARCHAR(255) null,
       UserRole INT null,
       Name NVARCHAR(255) null,
       primary key (ID)
    )
