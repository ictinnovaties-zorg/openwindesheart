/*
 * Copyright 2014 Open mHealth
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package org.openmhealth.dsu.configuration;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.oauth2.provider.ClientDetailsService;
import org.springframework.security.oauth2.provider.client.JdbcClientDetailsService;
import org.springframework.security.oauth2.provider.token.TokenStore;
import org.springframework.security.oauth2.provider.token.store.JdbcTokenStore;

import javax.sql.DataSource;


/**
 * A configuration of OAuth2 support objects backed by a relational database. The DDL files to create the corresponding
 * database schemas are available in the resources directory at the root of the project.
 *
 * @author Emerson Farrugia
 */
@Configuration
public class JdbcOAuth2SupportConfiguration {

    @Autowired
    private DataSource dataSource;

    /**
     * @return the store used to persist OAuth2 access and refresh tokens
     */
    @Bean
    public TokenStore tokenStore() {

        return new JdbcTokenStore(dataSource);
    }

    /**
     * @return the service used to retrieve OAuth2 client details
     */
    @Bean
    public ClientDetailsService clientDetailsService() {

        return new JdbcClientDetailsService(dataSource);
    }
}
