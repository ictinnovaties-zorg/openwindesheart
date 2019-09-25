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

import org.openmhealth.dsu.domain.SimpleClientDetails;
import org.springframework.data.repository.Repository;
import org.springframework.security.oauth2.provider.client.JdbcClientDetailsService;

import java.util.Optional;


/**
 * A repository of OAuth 2.0 client details. This infrastructure code is similar to {@link JdbcClientDetailsService},
 * but allows persistence to be backed by any Spring Data compatible data source, notably MongoDB.
 *
 * @author Emerson Farrugia
 */
public interface ClientDetailsRepository extends Repository<SimpleClientDetails, String> {

    /**
     * @see org.springframework.data.repository.CrudRepository#findOne(java.io.Serializable)
     */
    Optional<SimpleClientDetails> findOne(String clientId);

    /**
     * @see org.springframework.data.repository.CrudRepository#findAll()
     */
    Iterable<SimpleClientDetails> findAll();

    /**
     * @see org.springframework.data.repository.CrudRepository#save(Object)
     */
    SimpleClientDetails save(SimpleClientDetails clientDetails);

    /**
     * @see org.springframework.data.repository.CrudRepository#delete(java.io.Serializable)
     */
    void delete(String clientId);
}
