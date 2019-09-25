# Open mHealth Storage Endpoint [![Build Status](https://travis-ci.org/openmhealth/omh-dsu-ri.svg?branch=master)](https://travis-ci.org/openmhealth/omh-dsu-ri)

This repository contains the Java reference implementation of an [Open mHealth](http://www.openmhealth.org/)
[Data Point API](docs/raml/API.yml) storage endpoint.

> This code is in its early stages and requires further work and testing. Please do not use it in production without proper testing.

### tl;dr

* this repository contains a secure endpoint that offers an API for storing and retrieving data points
* data points conform to the Open mHealth [data point schema](http://www.openmhealth.org/documentation/#/schema-docs/schema-library/schemas/omh_data-point)
* the code consists of an [OAuth 2.0](http://oauth.net/2/) authorization server and resource server
* the authorization server manages access tokens
* the resource server implements the data point API documented [here](docs/raml/API.yml)
* the servers are written in Java using the [Spring Framework](http://projects.spring.io/spring-framework/), [Spring Security OAuth 2.0](http://projects.spring.io/spring-security-oauth/) and [Spring Boot](http://projects.spring.io/spring-boot/)
* the authorization server needs [PostgreSQL](http://www.postgresql.org/) to store client credentials and access tokens, and [MongoDB](http://www.mongodb.org/) to store user accounts
* the resource server needs PostgreSQL to read access tokens and MongoDB to store data points
* you can get everything up and running in a few commands using Docker Compose
* you can pull Docker containers for both servers from our [Docker Hub page](https://registry.hub.docker.com/repos/openmhealth/)
* you can use a Postman [collection](https://www.getpostman.com/collections/18e6065476d59772c748) to easily issue API requests
  
### Overview

A *data point* is a JSON document that represents a piece of data and conforms to
the [data-point](http://www.openmhealth.org/documentation/#/schema-docs/schema-library/schemas/omh_data-point) schema. The header of a data point conforms to
  the [header](http://www.openmhealth.org/documentation/#/schema-docs/schema-library/schemas/omh_header) schema, and the body can conform to any schema you like.
The header is designed to contain operational metadata, such as identifiers and provenance, whereas the body contains
the data being acquired or computed.

The *data point API* is a simple RESTful API that supports the creation, retrieval, and deletion of data points. The
API authorizes access using OAuth 2.0.

This implementation uses two components that reflect the [OAuth 2.0 specification](http://tools.ietf.org/html/rfc6749).
A *resource server* manages data point resources and implements the data point API. The resource server authorizes
requests using OAuth 2.0 access tokens. An *authorization server* manages the granting of access tokens.

### Installation

There are two ways to get up and running.

1. You can use Docker.
  * This is the fastest way to get up and running and isolates the install from your system.
1. You can build all the code from source and run it natively.

### Option 1. Using Docker containers

If you don't have Docker, Docker Compose, and Docker Machine installed, download [Docker Toolbox](https://www.docker.com/toolbox)
and follow the installation instructions for your platform. If you don't have a running Docker machine, follow these instructions
to [deploy one locally](https://docs.docker.com/machine/get-started/), or these instructions to
[deploy to the cloud](https://docs.docker.com/machine/get-started-cloud/) on any of these
[cloud platforms](https://docs.docker.com/machine/drivers/).

Once you have a running Docker host, in a terminal

1. Clone this Git repository.
1. Run `docker-machine ls` to find the name and IP address of your active Docker host.
1. Run `eval "$(docker-machine env host)"` to prepare environment variables, *replacing `host` with the name of your Docker host*.

Now, if you want to use pre-built Docker containers,

1. Run `docker-compose -f docker-compose-init-postgres.yml up -d` to download and run the containers.
  * If you want to keep the containers in the foreground and see logs, omit the `-d`.
  * If you want to just want to see logs, run `docker-compose -f docker-compose-init-postgres.yml logs`.

Otherwise, if you prefer to build and run your own containers, e.g. to customize them,

1. Run `./gradlew build -x test` to compile the code while skipping tests.
  * If you want to run the tests, you'll need to bring up a MongoDB instance with hostname `omh-mongo`.
1. Run `docker-compose -f docker-compose-build.yml up -d` to build and run the containers.
  * If you want to keep the containers in the foreground and see logs, omit the `-d`.
  * If you want to just want to see logs, run `docker-compose -f docker-compose-build.yml logs`.

The authorization and resource servers will start running on ports 8082 and 8083, respectively.
It can take up to a minute for the containers to start up.

#### Option 2. Building from source and running natively

We will add documentation on running the servers natively [on request](https://github.com/openmhealth/omh-dsu-ri/issues).

The Docker commands in option 1 automatically initialize the Spring Security OAuth schema in the PostgreSQL database.
To initialize the schema manually, you will need to source the
 [OAuth 2.0 DDL script](resources/rdbms/postgresql/oauth2-ddl.sql).

> Please note that the remainder of this document assumes you are using Docker. It should be straightforward to translate
any commands over to running the servers natively, but feel free to ask for help if something isn't clear.

### Configuring the servers

The [authorization server configuration file](authorization-server/src/main/resources/application.yml) and [resource
server configuration file](resource-server/src/main/resources/application.yml) are written in YAML using
[Spring Boot conventions](http://docs.spring.io/spring-boot/docs/current/reference/htmlsingle/#boot-features-external-config).

If you want to override the default configuration, you can either

* Add [environment variables](https://docs.docker.com/compose/compose-file/#environment) to the Docker Compose file.
    * e.g. setting `logging.level.org.springframework: DEBUG` will change the logging level
    * e.g. setting `spring.data.mongodb.host: foo` will change the MongoDB host
* Create an `application.yml` file in the `/opt/omh-dsu-ri/*-server` directory with the overriding YAML properties.

It is possible to use multiple resource servers with the same authorization server.

#### Adding clients

The authorization server manages the granting of access tokens to clients according to the OAuth 2.0 specification. Since it is
good practice to not roll your own security infrastructure, we leverage [Spring Security OAuth 2.0](http://projects.spring.io/spring-security-oauth/)
in this implementation. You can find the Spring Security OAuth 2.0 developer guide [here](http://projects.spring.io/spring-security-oauth/docs/oauth2.html).

> It is beyond the scope of this document to explain the workings of OAuth 2.0, Spring Security and Spring Security OAuth.
> The configuration information in this document is meant to help you get started, but is in no way a replacement
> for reading the documentation of the respective standards and projects.

The authorization server uses Spring Security OAuth 2.0's `JdbcClientDetailsService` to store OAuth 2.0 client credentials.
This necessitates access to a PostgreSQL database, although we intend to release a MongoDB service down the road to
require either MongoDB or PostgreSQL, but not both.

The client details in the `oauth_client_details` table controls the identity and authentication of clients, the grant
types they can use to show they have been granted authorization, and the resources they can access and actions they
can take once they have an access token. Specifically, the client details table contains

* the identity of the client (column `client_id`)
* the resource identifiers (column `resource_ids`) the client can access , `dataPoints` in our case
* the client secret, if any (column `client_secret`)
* the scope (column `scope`) to which the client is limited, in our case some comma-separated combination of
    * `read_data_points` if the client is allowed to read data points
    * `write_data_points` if the client is allowed to write data points
    * `delete_data_points` if the client is allowed to delete data points
* the authorization grant types (column `authorized_grant_types`) the client is limited to, some comma-separated combination of
    * `authorization_code`, documented in the [Authorization Code](http://tools.ietf.org/html/rfc6749#section-1.3.1) section of the OAuth 2.0 spec
    * `implicit`, documented in the [Implicit](http://tools.ietf.org/html/rfc6749#section-1.3.2) section
    * `password`, documented in the [Resource Owner Password Credentials](http://tools.ietf.org/html/rfc6749#section-1.3.3) section of the OAuth 2.0 spec
    * `refresh_token`, documented in the [Refresh Token](http://tools.ietf.org/html/rfc6749#section-1.5) section
    * N.B. the `client_credentials` grant type in the [Client Credentials](http://tools.ietf.org/html/rfc6749#section-1.3.4) section is not yet supported, but slated to be
* the Spring Security authorities (column `authorities`) the token bearer has, in our case `ROLE_CLIENT`

To create a client,

1. Connect to the `omh` PostgreSQL database.
1. Add a row to the `oauth_client_details` table, as shown in this [sample script](resources/rdbms/common/oauth2-sample-data.sql).


#### Adding end-users

The data points accessible over the data point API belong to a user. In OAuth 2.0, this user is called the *resource owner* or *end-user*.
A client requests authorization from the authorization server to access the data points of one or more users.

The authorization server includes a simple RESTful endpoint to create users. To create a user, either execute the following command

```bash
curl -H "Content-Type:application/json" --data '{"username": "testUser", "password": "testUserPassword"}' http://host:8082/users
```

or use the *create an end-user/success or conflict* request in the Postman collection discussed [below](#issuing-requests-with-postman).

The user creation endpoint is primitive by design; it is only meant as a way to bootstrap a couple users
when first starting out. In general, the creation of users is typically the concern of a user management component,
not the authorization server. And it's quite common
 for integrators to already have a user management system complete with its own user account database before introducing the
 authorization server.

To integrate a user management system with the authorization server, you would

1. Disable the `org.openmhealth.dsu.controller.EndUserController`, usually by commenting out the `@Controller` annotation.
1. Provide your own implementation of either the `org.openmhealth.dsu.service.EndUserService` or the
 `org.openmhealth.dsu.repository.EndUserRepository`, populating `org.openmhealth.dsu.domain.EndUser` instances with data read from your own data stores or APIs.

### Issuing requests with Postman

Your code interacts with the authorization and resource servers by sending them HTTP
requests. To make learning about those requests easier, we've created a [Postman](http://www.getpostman.com/) collection
that contains a predefined set of requests for different actions and outcomes. Postman is a Chrome
packaged application whose UI lets you craft and send HTTP requests.

> These instructions are written for Postman 1.0.1. If you're using a newer version and they don't work,
> [let us know](https://github.com/openmhealth/omh-dsu-ri/issues) and we'll fix them.

To set up the collection,

1. [Download Postman](https://chrome.google.com/webstore/detail/postman-rest-client-packa/fhbjgbiflinjbdggehcddcbncdddomop).
1. [Start it](http://www.getpostman.com/docs/launch).
1. Click the *Import* button, choose the *Download from link* tab and paste `https://www.getpostman.com/collections/18e6065476d59772c748`
1. The collection should now be available. The folder names describe the requests, and the request names describe the expected outcome.
1. Create an [environment](https://www.getpostman.com/docs/environments). Environments provide values for the `{{...}}` placeholders in the collection.
   Add the following environment keys and values, possibly changing the values if you've customised the installation.
    * `authorizationServer.host` - IP address of your Docker host (on Mac OS X and Windows, `docker-machine ip <host>` will print this IP to the console)
    * `authorizationServer.port` -  `8082`
    * `resourceServer.host` -  IP address of your Docker host
    * `resourceServer.port` -  `8083`
    * `accessToken` - issue the *get access token using RO password grant/success* request and copy the `access_token` value from the response here, without quotes
    * `apiVersion` -  `1.0.M1`

To send a request, pick the request and click its *Send* button. The different requests should be self-explanatory,
and correspond to the verbs and resources in the [data point API](docs/raml/API.yml).

The folders also have descriptions,
which you can currently only see by clicking the corresponding *Edit folder* button (but Postman are
[working on that](https://github.com/a85/POSTMan-Chrome-Extension/issues/816)). You can see the request descriptions by
selecting the request.

### Using the authorization server

We may add documentation here if we find that the Postman collection isn't sufficient.

### Using the resource server

The data point API is documented in a [RAML file](docs/raml/API.yml) to avoid ambiguity.

A data point looks something like this

```json
{
    "header": {
        "id": "123e4567-e89b-12d3-a456-426655440000",
        "creation_date_time": "2013-02-05T07:25:00Z",
        "schema_id": {
            "namespace": "omh",
            "name": "physical-activity",
            "version": "1.0"
        },
        "acquisition_provenance": {
            "source_name": "RunKeeper",
            "modality": "sensed"
        },
        "user_id": "joe"
    },
    "body": {
        "activity_name": "walking",
        "distance": {
            "value": 1.5,
            "unit": "mi"
        },
        "reported_activity_intensity": "moderate",
        "effective_time_frame": {
            "time_interval": {
                "date": "2013-02-05",
                "part_of_day": "morning"
            }
        }
    }
}
```

We may add documentation here if we find that the Postman collection isn't sufficient.


### Roadmap

The following features are scheduled for future milestones

* support the client credentials grant type
* improve test coverage
* support refresh tokens
* make it easier to customise the authorization code and implicit grant forms
* support SSL out of the box
* filter data points based on effective time frames
* filter data points based on their bodies

If you have other feature requests, [create an issue for each](https://github.com/openmhealth/omh-dsu-ri/issues)
and we'll figure out how to prioritise them.


### Contributing

If you'd like to contribute any code

1. [Open an issue](https://github.com/openmhealth/omh-dsu-ri/issues) to let us know what you're going to work on.
  1. This lets us give you feedback early and lets us put you in touch with people who can help.
2. Fork this repository.
3. Create your feature branch `feature/do-x-y-z` from the `develop` branch.
4. Commit and push your changes to your fork.
5. Create a pull request.