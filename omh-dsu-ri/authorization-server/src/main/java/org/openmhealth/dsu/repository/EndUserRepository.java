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

import org.openmhealth.dsu.domain.EndUser;
import org.springframework.data.repository.Repository;

import java.util.Optional;


/**
 * A repository of user accounts.
 *
 * @author Emerson Farrugia
 */
public interface EndUserRepository extends Repository<EndUser, String> {

    /**
     * @see org.springframework.data.repository.CrudRepository#findOne(java.io.Serializable)
     */
    Optional<EndUser> findOne(String username);

    /**
     * @see org.springframework.data.repository.CrudRepository#save(Object)
     */
    EndUser save(EndUser endUser);

    /**
     * @see org.springframework.data.repository.CrudRepository#delete(java.io.Serializable)
     */
    void delete(String username);
}
