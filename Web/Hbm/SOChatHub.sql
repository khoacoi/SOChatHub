
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKE18665EB158DC991]') AND parent_object_id = OBJECT_ID('[OAuthMembership]'))
alter table [OAuthMembership]  drop constraint FKE18665EB158DC991


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKC70167A4158DC991]') AND parent_object_id = OBJECT_ID('[WebMemberShip]'))
alter table [WebMemberShip]  drop constraint FKC70167A4158DC991


    if exists (select * from dbo.sysobjects where id = object_id(N'[Menu]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Menu]

    if exists (select * from dbo.sysobjects where id = object_id(N'[OAuthMembership]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [OAuthMembership]

    if exists (select * from dbo.sysobjects where id = object_id(N'[UserProfile]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [UserProfile]

    if exists (select * from dbo.sysobjects where id = object_id(N'[WebMemberShip]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [WebMemberShip]

    create table [Menu] (
        ID UNIQUEIDENTIFIER not null,
       Controller NVARCHAR(255) null,
       Action NVARCHAR(255) null,
       IsAdministration BIT null,
       Name NVARCHAR(255) null,
       primary key (ID)
    )

    create table [OAuthMembership] (
        ID UNIQUEIDENTIFIER not null,
       Provider NVARCHAR(255) null,
       ProviderUserID NVARCHAR(255) null,
       UserProfileID UNIQUEIDENTIFIER null,
       primary key (ID)
    )

    create table [UserProfile] (
        ID UNIQUEIDENTIFIER not null,
       UserName NVARCHAR(255) null,
       primary key (ID)
    )

    create table [WebMemberShip] (
        ID UNIQUEIDENTIFIER not null,
       Email NVARCHAR(255) null,
       Password NVARCHAR(255) null,
       CreatedDate DATETIME null,
       UserProfileID UNIQUEIDENTIFIER null,
       primary key (ID)
    )

    alter table [OAuthMembership] 
        add constraint FKE18665EB158DC991 
        foreign key (UserProfileID) 
        references [UserProfile]

    alter table [WebMemberShip] 
        add constraint FKC70167A4158DC991 
        foreign key (UserProfileID) 
        references [UserProfile]
