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

import org.openmhealth.schema.domain.omh.DataPoint;
import org.springframework.data.repository.NoRepositoryBean;
import org.springframework.data.repository.Repository;

import java.util.Optional;


/**
 * A repository of data points.
 *
 * @see org.springframework.data.repository.CrudRepository
 * @author Emerson Farrugia
 */
@NoRepositoryBean
public interface DataPointRepository extends Repository<DataPoint, String>, CustomDataPointRepository {

    boolean exists(String id);

    Optional<DataPoint> findOne(String id);

    DataPoint save(DataPoint dataPoint);

    Iterable<DataPoint> save(Iterable<DataPoint> dataPoints);

    void delete(String id);

    Long deleteByIdAndHeaderUserId(String id, String userId);
}
