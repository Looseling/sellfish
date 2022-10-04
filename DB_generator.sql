create table comments
(
    id            bigint auto_increment
        primary key,
    user_id       bigint null,
    comment_date  date   not null,
    commecnt_text text   null
);

create table fish
(
    id          bigint auto_increment
        primary key,
    fish_name   varchar(50) not null,
    fish_weight int         not null,
    fish_amount int         not null,
    fish_price  float       not null,
    description text        null
);

create table cart
(
    id          bigint auto_increment
        primary key,
    user_id     bigint null,
    item_id     bigint null,
    item_weight float  null,
    constraint cart_fish_fk
        foreign key (item_id) references fish (id)
);

create table user
(
    id              bigint auto_increment
        primary key,
    first_name      varchar(50)   not null,
    last_name       varchar(50)   not null,
    password        varchar(1000) not null,
    email           varchar(255)  null,
    address         varchar(765)  not null,
    user_comment_id bigint        null,
    cart_id         bigint        null,
    is_admin        tinyint(1)    not null,
    constraint user_pk
        unique (cart_id),
    constraint user_cart_fk
        foreign key (cart_id) references cart (id),
    constraint user_commecnt_fk
        foreign key (user_comment_id) references comments (id)
);

alter table cart
    add constraint cart_user_fk
        foreign key (user_id) references user (id);

alter table comments
    add constraint comments_user_fk
        foreign key (user_id) references user (id);

