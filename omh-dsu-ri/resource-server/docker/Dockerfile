FROM java:openjdk-8-jre
MAINTAINER Emerson Farrugia <emerson@openmhealth.org>

ENV SERVER_PREFIX /opt/omh-dsu-ri/resource-server
 
RUN mkdir -p $SERVER_PREFIX
ADD resource-server.jar $SERVER_PREFIX/
EXPOSE 8083

CMD /usr/bin/java -jar $SERVER_PREFIX/resource-server.jar --spring.config.location=file:$SERVER_PREFIX/
