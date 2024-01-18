CREATE TABLE stores (
    id uuid NOT NULL,
    created_at timestamp with time zone NOT NULL,
    external_id character varying(24) NOT NULL,
    streamer_name character varying(100) NOT NULL,
    uri character varying(100) NOT NULL,
    CONSTRAINT pk_stores PRIMARY KEY(id)
);

CREATE UNIQUE INDEX ix_stores_external_id ON stores(external_id ASC);

CREATE TABLE store_items (
    id uuid NOT NULL,
    created_at timestamp with time zone NOT NULL,
    external_id character varying(24) NOT NULL,
    store_id uuid NOT NULL,
    "name" character varying(100) NOT NULL,
    cost integer NOT NULL,
    CONSTRAINT pk_store_items PRIMARY KEY(id),
    CONSTRAINT fk_Store_items_store_id FOREIGN KEY(store_id) REFERENCES stores(id)
);

CREATE UNIQUE INDEX ix_store_items_external_id ON store_items(external_id ASC);