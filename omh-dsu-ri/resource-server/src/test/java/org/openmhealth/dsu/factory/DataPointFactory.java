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

package org.openmhealth.dsu.factory;

import org.openmhealth.dsu.domain.DataPointBuilder;
import org.openmhealth.dsu.domain.DataPointSearchCriteriaBuilder;
import org.openmhealth.schema.domain.omh.CaloriesBurned;
import org.openmhealth.schema.domain.omh.KcalUnitValue;
import org.openmhealth.schema.domain.omh.SchemaVersion;
import org.openmhealth.schema.domain.omh.TimeInterval;
import org.springframework.stereotype.Service;

import java.time.OffsetDateTime;

import static java.time.ZoneOffset.UTC;
import static org.openmhealth.schema.domain.omh.KcalUnit.KILOCALORIE;


/**
 * A factory that builds objects to support data point tests.
 *
 * @author Emerson Farrugia
 */
@Service
public class DataPointFactory {

    public static final String TEST_USER_ID = "test";
    public static final String TEST_SCHEMA_NAMESPACE = "test";
    public static final String TEST_SCHEMA_NAME = "test";
    public static final SchemaVersion TEST_SCHEMA_VERSION = new SchemaVersion(1, 1);

    public static DataPointBuilder newDataPointBuilder() {

        return new DataPointBuilder()
                .setUserId(TEST_USER_ID)
                .setSchemaNamespace(TEST_SCHEMA_NAMESPACE)
                .setSchemaName(TEST_SCHEMA_NAME)
                .setSchemaVersion(TEST_SCHEMA_VERSION)
                .setBody(newKcalBurnedBody());
    }

    public static DataPointSearchCriteriaBuilder newSearchCriteriaBuilder() {

        return new DataPointSearchCriteriaBuilder()
                .setUserId(TEST_USER_ID)
                .setSchemaNamespace(TEST_SCHEMA_NAMESPACE)
                .setSchemaName(TEST_SCHEMA_NAME)
                .setSchemaVersion(TEST_SCHEMA_VERSION);
    }

    public static CaloriesBurned newKcalBurnedBody() {

        TimeInterval effectiveTimeInterval = TimeInterval.ofStartDateTimeAndEndDateTime(
                OffsetDateTime.of(2013, 2, 5, 6, 25, 0, 0, UTC),
                OffsetDateTime.of(2013, 2, 5, 7, 25, 0, 0, UTC)
        );

        return new CaloriesBurned.Builder(new KcalUnitValue(KILOCALORIE, 160))
                .setActivityName("walking")
                .setEffectiveTimeFrame(effectiveTimeInterval)
                .build();
    }
}
