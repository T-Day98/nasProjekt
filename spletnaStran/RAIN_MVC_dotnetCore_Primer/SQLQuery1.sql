
use "Baza"

create table Novica(
 id int not null primary key identity(1,1),
 avtor text not null,
 naziv text null,
 besedilo text null,
 datum text null
);