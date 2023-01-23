CREATE TABLE translations
(
    id                 serial primary key,
    entity_id          int,
    entity_name        text,
    language_code      text,
    translation_fields jsonb,
    created_date       timestamp default (now() at time zone 'utc'),
    updated_date       timestamp default (now() at time zone 'utc'),
    UNIQUE (entity_id, entity_name, language_code)
);

CREATE TABLE Integration_items_outbox
(
    id            serial primary key,
    name_item     text,
    data          text,
    is_deleted    bool      default false,
    deletion_date timestamp,
    created_date  timestamp default (now() at time zone 'utc'),
    updated_date  timestamp default (now() at time zone 'utc')
);
