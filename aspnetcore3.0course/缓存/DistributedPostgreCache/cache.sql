
CREATE TABLE public.cachetable
(
    id character varying(449) COLLATE pg_catalog."default" NOT NULL,
    value bytea,
    expiresattime timestamp with time zone,
    slidingexpirationinseconds interval,
    absoluteexpiration timestamp with time zone,
    CONSTRAINT cachetable_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;
ALTER TABLE public.cachetable
    OWNER to postgres;