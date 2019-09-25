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

import com.google.common.collect.Range;
import org.openmhealth.dsu.domain.DataPointSearchCriteria;
import org.openmhealth.schema.domain.omh.DataPoint;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Sort;
import org.springframework.data.mongodb.core.MongoOperations;
import org.springframework.data.mongodb.core.query.Criteria;
import org.springframework.data.mongodb.core.query.Query;

import javax.annotation.Nullable;
import java.time.OffsetDateTime;

import static com.google.common.base.Preconditions.checkArgument;
import static com.google.common.base.Preconditions.checkNotNull;
import static com.google.common.collect.BoundType.CLOSED;
import static org.springframework.data.mongodb.core.query.Criteria.where;


/**
 * @author Emerson Farrugia
 */
public class MongoDataPointRepositoryImpl implements CustomDataPointRepository {

    @Autowired
    private MongoOperations mongoOperations;

    // if a data point is filtered by its data and not just its header, these queries will need to be written using
    // the MongoDB Java driver instead of Spring Data MongoDB, since there is no mapping information to work against
    @Override
    public Iterable<DataPoint> findBySearchCriteria(DataPointSearchCriteria searchCriteria, @Nullable Integer offset,
                                                    @Nullable Integer limit) {

        checkNotNull(searchCriteria);
        checkArgument(offset == null || offset >= 0);
        checkArgument(limit == null || limit >= 0);

        Query query = newQuery(searchCriteria);

        if (offset != null) {
            query.skip(offset);
        }

        if (limit != null) {
            query.limit(limit);
        }

        return mongoOperations.find(query, DataPoint.class);
    }

    @Override
    public DataPoint findLastDataPoint(DataPointSearchCriteria searchCriteria) {

        Query query = new Query();
        query.limit(1);
        query.addCriteria(where("header.user_id").is(searchCriteria.getUserId()));
        query.addCriteria(where("header.schema_id.namespace").is(searchCriteria.getSchemaNamespace()));
        query.addCriteria(where("header.schema_id.name").is(searchCriteria.getSchemaName()));
        query.addCriteria(where("header.schema_id.version.major").is(searchCriteria.getSchemaVersion().getMajor()));
        query.addCriteria(where("header.schema_id.version.minor").is(searchCriteria.getSchemaVersion().getMinor()));
        query.with(new Sort(Sort.Direction.DESC, "body.effective_time_frame.date_time"));

        if (mongoOperations.count(query, DataPoint.class) > 0) {
            return mongoOperations.find(query, DataPoint.class).get(0);
        }
        return null;
    }

    private Query newQuery(DataPointSearchCriteria searchCriteria) {

        Query query = new Query();

        query.addCriteria(where("header.user_id").is(searchCriteria.getUserId()));
        query.addCriteria(where("header.schema_id.namespace").is(searchCriteria.getSchemaNamespace()));
        query.addCriteria(where("header.schema_id.name").is(searchCriteria.getSchemaName()));
        query.addCriteria(where("header.schema_id.version.major").is(searchCriteria.getSchemaVersion().getMajor()));
        query.addCriteria(where("header.schema_id.version.minor").is(searchCriteria.getSchemaVersion().getMinor()));

        if (searchCriteria.getSchemaVersion().getQualifier().isPresent()) {
            query.addCriteria(where("header.schema_id.version.qualifier")
                    .is(searchCriteria.getSchemaVersion().getQualifier().get()));
        }
        else {
            query.addCriteria(where("header.schema_id.version.qualifier").exists(false));
        }

        if (searchCriteria.getCreationTimestampRange().isPresent()) {
            addCreationTimestampCriteria(query, searchCriteria.getCreationTimestampRange().get());
        }

        return query;
    }

    void addCreationTimestampCriteria(Query query, Range<OffsetDateTime> timestampRange) {

        if (timestampRange.hasLowerBound() || timestampRange.hasUpperBound()) {

            Criteria timestampCriteria = where("header.creation_date_time");

            if (timestampRange.hasLowerBound()) {
                if (timestampRange.lowerBoundType() == CLOSED) {
                    timestampCriteria = timestampCriteria.gte(timestampRange.lowerEndpoint());
                }
                else {
                    timestampCriteria = timestampCriteria.gt(timestampRange.lowerEndpoint());
                }
            }

            if (timestampRange.hasUpperBound()) {
                if (timestampRange.upperBoundType() == CLOSED) {
                    timestampCriteria = timestampCriteria.lte(timestampRange.upperEndpoint());
                }
                else {
                    timestampCriteria = timestampCriteria.lt(timestampRange.upperEndpoint());
                }
            }

            query.addCriteria(timestampCriteria);
        }
    }
}