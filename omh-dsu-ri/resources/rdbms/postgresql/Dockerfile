# This image adds the authorization schema and sample OAuth 2.0 credentials to a PostgreSQL database.

FROM postgres:latest
MAINTAINER Emerson Farrugia <emerson@openmhealth.org>
# Originally contributed by Matthew Schulkind <mschulkind@gmail.com>.

COPY create-database.sql oauth2-ddl.sql /docker-entrypoint-initdb.d/
