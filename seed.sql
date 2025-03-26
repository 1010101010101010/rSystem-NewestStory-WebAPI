Create table Story
(
 Id int not null identity,
 Title varchar(100),
 StoryUrl nvarchar(250),
 Created DateTime not null default(getDate()),
 Modified DateTime not null default(getDate()),
)

Select * from dbo.Story

insert into Story values('Chanakya','https://chanakya_bio.com/story/1',GETDATE(),GETDATE())
insert into Story values('Chanakya1','https://chanakya_bio.com/story/2',GETDATE(),GETDATE())
insert into Story values('Chanakya2','https://chanakya_bio.com/story/3',GETDATE(),GETDATE())
insert into Story values('Chanakya3','https://chanakya_bio.com/story/4',GETDATE(),GETDATE())

insert into Story values('Chanakya','https://chanakya_bio.com/story/1',GETDATE(),GETDATE())
insert into Story values('Chanakya','https://chanakya_bio.com/story/1',GETDATE(),GETDATE())