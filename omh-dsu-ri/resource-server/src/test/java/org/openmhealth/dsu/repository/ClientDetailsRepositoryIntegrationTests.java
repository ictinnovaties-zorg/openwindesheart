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

package org.openmhealth.dsu.repository;

import org.junit.After;
import org.junit.Test;
import org.openmhealth.dsu.domain.SimpleClientDetails;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;

import java.util.Collection;
import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

import static com.google.common.collect.Lists.newArrayList;
import static org.hamcrest.CoreMatchers.equalTo;
import static org.junit.Assert.assertThat;
import static org.openmhealth.dsu.configuration.OAuth2Properties.CLIENT_ROLE;
import static org.openmhealth.dsu.configuration.OAuth2Properties.DATA_POINT_RESOURCE_ID;


/**
 * A suite of integration tests for a client details repository.
 *
 * @author Emerson Farrugia
 */
public abstract class ClientDetailsRepositoryIntegrationTests {

    private static final String TEST_CLIENT_ID = "test";
    private static final String TEST_CLIENT_SECRET = "secret";
    private static final List<String> TEST_RESOURCE_IDS = newArrayList(DATA_POINT_RESOURCE_ID);
    private static final List<String> TEST_GRANT_TYPES = newArrayList("authorization_code", "implicit");
    private static final List<String> TEST_ROLES = newArrayList(CLIENT_ROLE);
    private static final List<String> TEST_SCOPES = newArrayList("read", "write");

    @Autowired
    private ClientDetailsRepository clientDetailsRepository;

    protected List<GrantedAuthority> asGrantedAuthorities(Collection<String> roles) {

        return roles.stream().map(SimpleGrantedAuthority::new).collect(Collectors.toList());
    }

    protected SimpleClientDetails newClientDetails() {

        SimpleClientDetails clientDetails = new SimpleClientDetails();

        clientDetails.setClientId(TEST_CLIENT_ID);
        clientDetails.setClientSecret(TEST_CLIENT_SECRET);
        clientDetails.setResourceIds(TEST_RESOURCE_IDS);
        clientDetails.setAuthorizedGrantTypes(TEST_GRANT_TYPES);
        clientDetails.setAuthorities(asGrantedAuthorities(TEST_ROLES));
        clientDetails.setScope(TEST_SCOPES);

        return clientDetails;
    }

    @After
    public void deleteFixture() {

        clientDetailsRepository.delete(TEST_CLIENT_ID);
    }

    @Test
    public void findOneShouldReturnSavedClientDetails() {

        SimpleClientDetails expected = clientDetailsRepository.save(newClientDetails());

        Optional<SimpleClientDetails> actual = clientDetailsRepository.findOne(TEST_CLIENT_ID);

        assertThat(actual.isPresent(), equalTo(true));
        assertThat(actual.get(), equalTo(expected));
    }
}
